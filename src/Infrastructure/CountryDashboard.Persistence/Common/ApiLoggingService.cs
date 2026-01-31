using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using System.Text.Json;
using CountryDashboard.Application.Common.Interfaces.Services;
using CountryDashboard.Application.Common.Response;

namespace CountryDashboard.Persistence.Common;

public class ApiLoggingService : IApiLoggingService
{
    private readonly ILogger<ApiLoggingService> _logger;

    public ApiLoggingService(ILogger<ApiLoggingService> logger)
    {
        _logger = logger;
    }

    public Task LogAsync(HttpContext context,
                         ApiResponse? apiResponse,
                         long durationMs,
                         string? requestBody = null,
                         string? responseBody = null)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = false,
                PropertyNameCaseInsensitive = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            if (apiResponse?.Errors?.Count > 0)
            {
                _logger.LogError(
                    "API Log | Path: {Path} | Method: {Method} | StatusCode: {StatusCode} | DurationMs: {DurationMs} | RequestBody: {RequestBody} | ResponseBody: {ResponseBody} | FirstError: {FirstError} | Errors: {Errors} | RequestTime: {RequestTime}",
                    context.Request.Path,
                    context.Request.Method,
                    context.Response.StatusCode,
                    durationMs,
                    requestBody ?? string.Empty,
                    responseBody ?? string.Empty,
                    apiResponse.Errors,
                    DateTime.UtcNow
                );
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to log API request/response");
        }

        return Task.CompletedTask;
    }
}
