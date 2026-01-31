namespace CountryDashboard.Authentication.Services;
/// <summary>
/// Service for validating token revocation in JWT Bearer middleware
/// Checks Redis for device-level, user-level, and global token revocations
/// </summary>
public class TokenRevocationValidationService(IRedisCacheService redisCacheService) : ITokenRevocationValidationService
{
    private readonly IRedisCacheService _redisCacheService = redisCacheService;

    /// <summary>
    /// Validates if a token has been revoked by checking Redis blacklists
    /// </summary>
    /// <param name="principal">The validated claims principal from the JWT token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if token is revoked (should reject), false if token is valid</returns>
    public async Task<bool> IsTokenRevokedAsync(ClaimsPrincipal principal,
                                                CancellationToken cancellationToken = default)
    {
        try
        {
            // Extract claims

            // JTI (token identifier)
            var jti = principal.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

            // Try to get user ID from standard claims
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            // Issued At claim
            var iatClaim = principal.FindFirst(JwtRegisteredClaimNames.Iat)?.Value;

            // If essential claims are missing, reject the token
            if (string.IsNullOrEmpty(jti) || string.IsNullOrEmpty(userIdClaim))
                return true;

            // Parse issued at timestamp (Unix timestamp)
            if (!long.TryParse(iatClaim, out var tokenIat))
                return true; // Invalid IAT claim, reject

            // 1. Check device-level logout (JTI blacklist) - fail-fast
            var blacklistKey = $"auth:blacklist:jti:{jti}";
            var isJtiBlacklisted = await _redisCacheService.ExistsAsync(blacklistKey);
            if (isJtiBlacklisted)
                return true; // Token is blacklisted, reject

            // 2. Check user-level force logout (all devices)
            if (Guid.TryParse(userIdClaim, out var userId))
            {
                var userRevokedKey = $"auth:user:{userId}:revokedAt";
                var userRevokedAtStr = await _redisCacheService.GetAsync<string>(userRevokedKey);

                if (!string.IsNullOrEmpty(userRevokedAtStr) &&
                    long.TryParse(userRevokedAtStr, out var userRevokedAt))
                {
                    // Token is revoked if it was issued before the revocation timestamp
                    if (tokenIat < userRevokedAt)
                        return true; // Token was issued before user revocation, reject
                }
            }

            // 3. Check global revocation (optional)
            const string globalRevokedKey = "auth:global:revokedAt";
            var globalRevokedAtStr = await _redisCacheService.GetAsync<string>(globalRevokedKey);

            if (!string.IsNullOrEmpty(globalRevokedAtStr) &&
                long.TryParse(globalRevokedAtStr, out var globalRevokedAt))
            {
                // Token is revoked if it was issued before the global revocation timestamp
                if (tokenIat < globalRevokedAt)
                    return true; // Token was issued before global revocation, reject
            }

            // Token is valid (not revoked)
            return false;
        }
        catch
        {
            // On any error, reject the token for security
            return true;
        }
    }
}