namespace CountryDashboard.RequestContext;

public static class DependancyInjections
{
    public static IServiceCollection AddRequestContextServices(this IServiceCollection services)
    {
        // Register IHttpContextAccessor
        services.AddHttpContextAccessor();

        // Register RequestHeaderService
        services.AddScoped<RequestHeaderService, RequestHeaderService>();
        return services;
    }
}
