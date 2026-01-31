
using CountryDashboard.Application.Common.Interfaces.Services;
using CountryDashboard.Persistence.Common;

namespace CountryDashboard.Persistence
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddPersistenceServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Register audit interceptor
            services.AddScoped<AuditInterceptor>();

            // DbContext
            services.AddDbContext<AppDbContext>((sp, options) =>
            {
                var interceptor = sp.GetRequiredService<AuditInterceptor>();

                options.UseSqlServer(configuration.GetConnectionString("ConnectionStringDB"))
                       .EnableDetailedErrors()
                       .EnableSensitiveDataLogging()
                       .AddInterceptors(interceptor);
            });

            // Expose AppDbContext as IDbContext
            services.AddScoped<IDbContext>(provider => provider.GetRequiredService<AppDbContext>());
            services.AddScoped<IApiLoggingService, ApiLoggingService>();

            services.AddMemoryCache();

            // Auto-register all repositories
            RegisterRepositories(services);

            // Register generic repositories with caching wrapper
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

            // Unit Of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            var assembly = typeof(UnitOfWork).Assembly;

            var repoTypes = assembly.GetTypes()
                .Where(t => t.IsClass &&
                            t.GetInterfaces().Any(i =>
                                i.IsInterface &&
                                i.Name.EndsWith("Repository")));

            foreach (var implementation in repoTypes)
            {
                var interfaceType = implementation.GetInterfaces()
                    .First(i => i.Name.EndsWith("Repository"));

                services.AddScoped(interfaceType, implementation);
            }
        }
    }
}
