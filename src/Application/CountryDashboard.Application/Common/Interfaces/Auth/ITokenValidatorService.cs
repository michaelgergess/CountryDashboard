namespace CountryDashboard.Application.Common.Interfaces.Auth;

/// <summary>
/// Service responsible for validating JWT Bearer tokens
/// </summary>
public interface ITokenValidatorService
{
    /// <summary>
    /// Validates a JWT token and returns the ClaimsPrincipal if valid
    /// </summary>
    /// <param name="token">The JWT token string</param>
    /// <returns>ClaimsPrincipal if token is valid, null otherwise</returns>
    Task<ClaimsPrincipal> ValidateTokenAsync(string token);
}