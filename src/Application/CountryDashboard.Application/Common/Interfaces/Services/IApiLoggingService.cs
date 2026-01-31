using Microsoft.AspNetCore.Http;

namespace CountryDashboard.Application.Common.Interfaces.Services;

public interface IApiLoggingService
{
    Task LogAsync(HttpContext context, ApiResponse? apiResponse, long durationMs, string? requestBody = null, string? responseBody = null);
}
