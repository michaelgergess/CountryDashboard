using Microsoft.AspNetCore.Http;

namespace CountryDashboard.RequestContext.Services
{
    public class RequestHeaderService : IRequestHeaderService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestHeaderService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private IHeaderDictionary? Headers =>
            _httpContextAccessor.HttpContext?.Request?.Headers;

        private string? GetHeader(string key)
        {
            return Headers?.TryGetValue(key, out var value) == true &&
                   !string.IsNullOrWhiteSpace(value)
                ? value.ToString()
                : null;
        }

        public int CountryId
        {
            get
            {
                var value = GetHeader("CountryID");
                if (int.TryParse(value, out var id))
                    return id;

                throw new ArgumentException("Invalid CountryID header value.");
            }
        }
        public string? BrowserID => GetHeader("BrowserID");
        public string? DeviceID => GetHeader("DeviceID");
        public string? UserIP => GetHeader("UserIP");
    }
}
