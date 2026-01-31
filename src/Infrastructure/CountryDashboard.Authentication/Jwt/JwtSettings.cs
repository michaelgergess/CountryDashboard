
namespace CountryDashboard.Authentication.Jwt
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string NotificationsKey { get; set; }
        public int NotificationsExpiryInMinutes { get; set; }
        public List<string> Audiences { get; set; }
    }
}
