using Microsoft.AspNetCore.Mvc.Controllers;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using CountryDashboard.Application.Common.Interfaces.Auth;
using CountryDashboard.Authentication.Jwt;
using CountryDashboard.Domain.Constants;

namespace CountryDashboard.API.Filters;

/// <summary>
/// <para>
/// Custom authorization attribute responsible for:
/// - Extracting and validating JWT Bearer tokens
/// - Validating token audience
/// - Enforcing PS Admin–only access where required
/// - Validating endpoint-level permissions for non-admin users
/// </para>
/// <para>
/// This filter is implemented using <see cref="IAsyncActionFilter"/>
/// to allow full control over request execution before the action runs.
/// </para>
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class PSIdentityAuthorize : Attribute, IAsyncActionFilter
{
    /// <inheritdoc/>
    public async Task OnActionExecutionAsync(ActionExecutingContext context,
                                             ActionExecutionDelegate next)
    {
        // Skip authorization completely for AllowAnonymous endpoints
        if (context.ActionDescriptor.EndpointMetadata
            .OfType<AllowAnonymousAttribute>()
            .Any())
        {
            await next();
            return;
        }

        // Resolve required services from the DI container
        var tokenValidator = context.HttpContext.RequestServices
            .GetRequiredService<ITokenValidatorService>();

        var configuration = context.HttpContext.RequestServices
            .GetRequiredService<IConfiguration>();

        var jwtSettings = configuration
            .GetSection("JwtSettings")
            .Get<JwtSettings>();

        // 1 Extract Bearer token from Authorization header
        if (!TryExtractBearerToken(context, out var token))
        {
            SetUnauthorizedResponse(context, "Bearer token is missing or invalid.");
            return;
        }

        // 2 Validate token signature, expiration, and claims
        var principal = await tokenValidator.ValidateTokenAsync(token);
        if (principal == null)
        {
            SetUnauthorizedResponse(context, "Invalid or expired token.");
            return;
        }

        // 3 Validate token audience against configured allowed audiences
        if (!ValidateAudience(principal, jwtSettings.Audiences))
        {
            SetForbiddenResponse(context, "Token audience is not allowed.");
            return;
        }

        // 4 Apply authorization rules (PS Admin / permissions)
        if (!CheckAuthorization(context, principal, configuration))
        {
            // Response is already set inside the method
            return;
        }

        // Assign the validated principal to the HttpContext
        // so it can be accessed by controllers and downstream middleware
        context.HttpContext.User = principal;

        // Continue execution
        await next();
    }

    /// <summary>
    /// Attempts to extract the JWT Bearer token from the Authorization header.
    /// </summary>
    private static bool TryExtractBearerToken(ActionExecutingContext context, out string token)
    {
        token = string.Empty;

        if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
            return false;

        var authHeaderValue = authHeader.ToString();

        if (string.IsNullOrWhiteSpace(authHeaderValue) ||
            !authHeaderValue.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        token = authHeaderValue.Substring("Bearer ".Length).Trim();
        return !string.IsNullOrWhiteSpace(token);
    }

    /// <summary>
    /// Validates that the token audience matches one of the allowed audiences.
    /// If no audiences are configured, validation is skipped.
    /// </summary>
    private static bool ValidateAudience(ClaimsPrincipal principal, List<string> allowedAudiences)
    {
        if (allowedAudiences == null || allowedAudiences.Count == 0)
            return true;

        var tokenAudiences = principal.Claims
            .Where(c => c.Type == JwtRegisteredClaimNames.Aud || c.Type == "aud")
            .Select(c => c.Value)
            .ToList();

        return tokenAudiences.Any(aud => allowedAudiences.Contains(aud, StringComparer.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Applies authorization rules based on:
    /// - PS Admin–only endpoints
    /// - Admin bypass
    /// - Endpoint-level permission matching
    /// </summary>
    private static bool CheckAuthorization(ActionExecutingContext context,
                                           ClaimsPrincipal principal,
                                           IConfiguration configuration)
    {
        // Ensure the endpoint descriptor can be resolved
        if (context.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor)
        {
            SetForbiddenResponse(context, "Unable to determine endpoint information.");
            return false;
        }

        // Check whether the endpoint or controller is marked as PS Admin only
        var isPsAdminOnly =
            controllerActionDescriptor.MethodInfo.IsDefined(typeof(PsAdminOnlyAttribute), inherit: true) ||
            controllerActionDescriptor.ControllerTypeInfo.IsDefined(typeof(PsAdminOnlyAttribute), inherit: true);

        // Determine if the current user is a PS Admin
        var isPsAdmin = IsPsAdmin(principal);

        // PS Admin–only endpoints
        if (isPsAdminOnly)
        {
            if (!isPsAdmin)
            {
                // Unauthorized is returned because the user is authenticated
                // but lacks PS Admin privileges
                SetUnauthorizedResponse(context, "PS Admin privileges are required.");
                return false;
            }

            // PS Admin is allowed → skip permission validation
            return true;
        }

        // Admin bypass for regular endpoints
        if (isPsAdmin)
            return true;

        // Non-admin users: validate endpoint permissions

        var projectName = configuration["ProjectName"] ?? string.Empty;
        var moduleName = string.Empty;

        if (context.Controller is BaseController baseController)
            moduleName = baseController.ModuleName;

        // Construct the permission key for the current endpoint
        var fullEndpointName = GetFullEndpointName(controllerActionDescriptor,
                                                   projectName,
                                                   moduleName);

        // Collect all permission claims (supports multiple formats)
        var permissions = new List<string>();
        var permissionClaims = principal.Claims
            .Where(c => c.Type == CustomClaimType.Permission ||
                        c.Type == "permissions" ||
                        c.Type == "Permission")
            .ToList();

        foreach (var claim in permissionClaims)
        {
            // Handle JSON array permissions
            if (claim.Value?.StartsWith("[") == true && claim.Value.EndsWith("]"))
            {
                try
                {
                    var jsonPermissions = System.Text.Json.JsonSerializer
                        .Deserialize<List<string>>(claim.Value);

                    if (jsonPermissions != null)
                        permissions.AddRange(jsonPermissions);
                }
                catch
                {
                    // Fallback to single permission value
                    if (!string.IsNullOrWhiteSpace(claim.Value))
                        permissions.Add(claim.Value);
                }
            }
            else if (!string.IsNullOrWhiteSpace(claim.Value))
            {
                permissions.Add(claim.Value);
            }
        }

        // Validate that the required endpoint permission exists
        if (!permissions.Contains(fullEndpointName, StringComparer.OrdinalIgnoreCase))
        {
            SetForbiddenResponse(context,
                                 $"Access denied. Required permission: {fullEndpointName}");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Sets a 401 Unauthorized JSON response.
    /// </summary>
    private static void SetUnauthorizedResponse(ActionExecutingContext context, string message)
    {
        context.Result = new JsonResult(
            ApiResponse.Failure(message, HttpStatusCode.Unauthorized))
        {
            StatusCode = (int)HttpStatusCode.Unauthorized
        };
    }

    /// <summary>
    /// Sets a 403 Forbidden JSON response.
    /// </summary>
    private static void SetForbiddenResponse(ActionExecutingContext context, string message)
    {
        context.Result = new JsonResult(
            ApiResponse.Failure(message, HttpStatusCode.Forbidden))
        {
            StatusCode = (int)HttpStatusCode.Forbidden
        };
    }

    /// <summary>
    /// Determines the HTTP method associated with the action.
    /// Defaults to GET if no attribute is found.
    /// </summary>
    private static string GetHttpMethodFromAttributes(MethodInfo methodInfo)
    {
        var httpAttrs = methodInfo.GetCustomAttributes(true);

        foreach (var attr in httpAttrs)
        {
            switch (attr)
            {
                case HttpGetAttribute: return "GET";
                case HttpPostAttribute: return "POST";
                case HttpPutAttribute: return "PUT";
                case HttpDeleteAttribute: return "DELETE";
                case HttpPatchAttribute: return "PATCH";
            }
        }

        return "GET";
    }

    /// <summary>
    /// Builds the full permission key in the format:
    /// project.module.controller.action_httpMethod
    /// </summary>
    private static string GetFullEndpointName(ControllerActionDescriptor controllerActionDescriptor,
                                             string projectName,
                                             string moduleName)
    {
        if (controllerActionDescriptor == null)
            return string.Empty;

        var controllerName = controllerActionDescriptor.ControllerName;
        var actionName = controllerActionDescriptor.ActionName;

        var methodInfo = controllerActionDescriptor.MethodInfo;
        var httpMethod = GetHttpMethodFromAttributes(methodInfo);

        return $"{projectName}.{moduleName}.{controllerName}.{actionName}_{httpMethod}";
    }

    /// <summary>
    /// Determines whether the given principal represents a PS Admin user.
    /// </summary>
    private static bool IsPsAdmin(ClaimsPrincipal principal)
    {
        var claim = principal.FindFirst("IsPsAdmin");
        if (claim == null) return false;

        // Supports different claim value formats (boolean or string)
        return (bool.TryParse(claim.Value, out var isAdmin) && isAdmin) ||
               string.Equals(claim.Value, "true", StringComparison.OrdinalIgnoreCase);
    }
}