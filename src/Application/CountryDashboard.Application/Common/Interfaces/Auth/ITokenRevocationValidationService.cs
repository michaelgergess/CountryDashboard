using System.Security.Claims;

namespace CountryDashboard.Application.Common.Interfaces.Auth;

/// <summary>
/// Service for validating token revocation in JWT Bearer middleware
/// </summary>
public interface ITokenRevocationValidationService
{
    /// <summary>
    /// Validates if a token has been revoked by checking Redis blacklists
    /// </summary>
    /// <param name="principal">The validated claims principal from the JWT token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if token is revoked (should reject), false if token is valid</returns>
    Task<bool> IsTokenRevokedAsync(ClaimsPrincipal principal,
                                   CancellationToken cancellationToken = default);
}