namespace CountryDashboard.Authentication
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSection = configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(jwtSection);

            var jwtSettings = jwtSection.Get<JwtSettings>();
            services.AddSingleton(implementationInstance: jwtSettings);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateIssuerSigningKey = true,
                   ValidateLifetime = true,

                   ValidIssuer = jwtSettings.Issuer,
                   ValidAudience = jwtSettings.Audience,

                   IssuerSigningKey = new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(jwtSettings.SecretKey)
                   ),

                   ClockSkew = TimeSpan.Zero
               };
           });


            services.AddAuthorization();
            services.AddScoped<ITokenValidatorService, TokenValidatorService>();
            services.AddScoped<ITokenRevocationValidationService, TokenRevocationValidationService>();
            services.AddScoped<ITokenInfo, TokenInfo>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();


            // Register IHttpContextAccessor
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
