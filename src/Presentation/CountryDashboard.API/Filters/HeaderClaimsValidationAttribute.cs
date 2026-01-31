namespace CountryDashboard.API.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class HeaderClaimsValidationAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Skip if endpoint allows anonymous access
            var endpoint = context.HttpContext.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<AllowAnonymousAttribute>() != null)
                return;

            var ignoredHeaders = endpoint?.Metadata?.GetMetadata<IgnoreRequiredHeadersAttribute>()?.IgnoredHeaders;
            if (ignoredHeaders != null && !ignoredHeaders.Any())
                return;

            var headerService = context.HttpContext.RequestServices.GetService<IRequestHeaderService>();

            if (headerService == null)
            {
                context.Result = CreateApiErrorResponse(
                    ApiResponse.Failure("Missing required dependencies.", HttpStatusCode.InternalServerError)
                );
                return;
            }

            var result = ValidateHeaders(headerService, "asdasd");

            if (!result.IsSuccess)
            {
                context.Result = CreateApiErrorResponse(result);
            }
        }

        private ApiResponse ValidateHeaders(IRequestHeaderService header, string token)
        {
            // CountryID validation
            if (header.CountryId.ToString() != "6")
                return ApiResponse.Failure("Mismatch CountryID", HttpStatusCode.Forbidden);

            return ApiResponse.Success(null);
        }

        private JsonResult CreateApiErrorResponse(ApiResponse apiResponse)
        {
            return new JsonResult(apiResponse)
            {
                StatusCode = (int)apiResponse.StatusCode
            };
        }
    }
}