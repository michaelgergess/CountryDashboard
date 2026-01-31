using CountryDashboard.Application.Common.Interfaces.Auth;

namespace CountryDashboard.Authentication.Services
{
    public class TokenInfo(IHttpContextAccessor httpContextAccessor) : ITokenInfo
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public int? UserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
                return int.TryParse(userIdClaim, out var id) ? id : null;
            }
        }

        public int? CountryID
        {
            get
            {
                var countryIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("countryId")?.Value;
                return int.TryParse(countryIdClaim, out var id) ? id : null;
            }

        }
    }
}
