using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using CountryDashboard.Application.Common.Attributes;
using CountryDashboard.Application.Common.Interfaces.Services;
using CountryDashboard.Application.Common.Response;

namespace CountryDashboard.Authentication.Middleware;

public class CustomRateLimitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _cache;
    private readonly ILogger<CustomRateLimitMiddleware> _logger;

    public CustomRateLimitMiddleware(RequestDelegate next, IConfiguration configuration, IMemoryCache cache, ILogger<CustomRateLimitMiddleware> logger)
    {
        _next = next;
        _configuration = configuration;
        _cache = cache;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if rate limiter is enabled from config
        var rateLimitEnabled = _configuration.GetValue<bool>("RateLimiting:Enabled");
        if (!rateLimitEnabled)
        {
            await _next(context);
            return;
        }

        var requestInfoService = context.RequestServices.GetRequiredService<IRequestInfoService>();
        var requestHeaderService = context.RequestServices.GetRequiredService<IRequestHeaderService>();

        var endpoint = context.GetEndpoint();
        if (endpoint == null)
        {
            await _next(context);
            return;
        }

        var attr = endpoint.Metadata.GetMetadata<RateLimitAttribute>();
        if (attr == null)
        {
            await _next(context);
            return;
        }

        var key = requestHeaderService.DeviceID
                  ?? requestHeaderService.BrowserID
                  ?? requestHeaderService.UserIP
                  ?? requestInfoService.GetRequestIP();

        int minuteCount = 0, hourCount = 0, dayCount = 0;

        if (attr.HasPerMinute)
        {
            var minuteKey = $"{key}_minute";
            minuteCount = _cache.GetOrCreate(minuteKey, e =>
            {
                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return 0;
            });
        }

        if (attr.HasPerHour)
        {
            var hourKey = $"{key}_hour";
            hourCount = _cache.GetOrCreate(hourKey, e =>
            {
                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                return 0;
            });
        }

        if (attr.HasPerDay)
        {
            var dayKey = $"{key}_day";
            dayCount = _cache.GetOrCreate(dayKey, e =>
            {
                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
                return 0;
            });
        }

        // If limit exceeded
        if ((attr.HasPerMinute && minuteCount >= attr.PerMinute) ||
            (attr.HasPerHour && hourCount >= attr.PerHour) ||
            (attr.HasPerDay && dayCount >= attr.PerDay))
        {
            _logger.LogWarning("Rate limit exceeded for key: {Key}. Endpoint: {Endpoint}. IP: {IP}",
                key, endpoint.DisplayName, requestInfoService.GetRequestIP());


            var apiResponse = ApiResponse.Failure("Too many requests. Please try again later.",
                                                  HttpStatusCode.TooManyRequests);

            // Inject request info
            apiResponse.RequestIP = requestInfoService.GetRequestIP();
            apiResponse.RequestOrigin = context.Request.Headers["Origin"].ToString();

            // Generate public message

            var propsToIgnore = new[] { "MessageID", "ContentParameters", "TitleParameters", "DatabaseContent" };
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await context.Response.WriteAsJsonAsync(apiResponse, options);
            return;
        }

        // Otherwise, increase counts
        if (attr.HasPerMinute)
            _cache.Set($"{key}_minute", minuteCount + 1, TimeSpan.FromMinutes(1));

        if (attr.HasPerHour)
            _cache.Set($"{key}_hour", hourCount + 1, TimeSpan.FromHours(1));

        if (attr.HasPerDay)
            _cache.Set($"{key}_day", dayCount + 1, TimeSpan.FromDays(1));

        await _next(context);
    }
}