namespace CountryDashboard.Application.Common.Interfaces.Auth;

public interface IJwtTokenGenerator
{
    string GenerateNotificationToken();
    ClaimsPrincipal ValidateToken(string token);
}
