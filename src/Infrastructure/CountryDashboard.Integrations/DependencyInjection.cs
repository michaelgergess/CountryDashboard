using CountryDashboard.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CountryDashboard.Integrations;

public static class DependencyInjection
{
    public static IServiceCollection AddCountryDashboardIntegrations(this IServiceCollection services)
    {

        // Regiter External HTTP service
        services.AddScoped<IExternalHttpService, ExternalHttpService>();




        return services;
    }
}
