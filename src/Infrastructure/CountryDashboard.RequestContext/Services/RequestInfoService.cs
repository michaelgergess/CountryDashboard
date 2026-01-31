using Microsoft.AspNetCore.Http;
using CountryDashboard.Application.Common.Interfaces;

namespace CountryDashboard.RequestContext.Services;

public class RequestInfoService : IRequestInfoService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RequestInfoService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetRequestIP()
    {
        var ip = _httpContextAccessor.HttpContext?.Request?.Headers["X-Forwarded-For"].FirstOrDefault();
        if (string.IsNullOrEmpty(ip))
        {
            ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        }
        return ip ?? "0.0.0.0";
    }

    public string? GetOrigin()
    {
        return _httpContextAccessor.HttpContext?.Request.Headers["Origin"].FirstOrDefault();
    }

    // Add other methods like GetRequestOrigin etc.
}
