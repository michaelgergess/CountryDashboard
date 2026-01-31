namespace CountryDashboard.Authentication.Services;

/// <summary>
/// Service that validates JWT tokens using the existing token generator
/// </summary>
public class TokenValidatorService(IJwtTokenGenerator jwtTokenGenerator,
                                   IExternalHttpService externalHttpService,
                                   ITokenRevocationValidationService tokenRevocationValidationService) : ITokenValidatorService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
    private readonly IExternalHttpService _externalHttpService = externalHttpService;
    private readonly ITokenRevocationValidationService _tokenRevocationValidationService = tokenRevocationValidationService;

    public async Task<ClaimsPrincipal> ValidateTokenAsync(string token)
    {
        //validate jwt token
        try
        {
            var principals = _jwtTokenGenerator.ValidateToken(token);
            if (principals == null)
                return null;

            // validate token isn't blacklisted in the redis cache
            try
            {
                var isTokenRevoked = await _tokenRevocationValidationService.IsTokenRevokedAsync(principals);

                if (isTokenRevoked)
                    return null;
                else
                    return principals;
            }
            catch (Exception ex)
            {
                // if redis cache check fails
                // call identity to validate token (not implemented here)
                const string url = "https://localhost:7211/api/Auth/ValidateToken";
                ValidateTokenRequest req = new() { AccessToken = token };
                var result = await _externalHttpService.PostAsync<ValidateTokenRequest, ValidateTokenResponse>(url, req, null);
                if (result.IsValid)
                    return principals;
                else
                    return null;

            }
        }
        catch
        {
            return null;
        }

        return null;
    }

    public class ValidateTokenRequest
    {
        public string AccessToken { get; set; }
    }

    public class ValidateTokenResponse
    {
        public bool IsValid { get; set; }
        public Guid? UserId { get; set; }
        public string? Message { get; set; }
    }
}