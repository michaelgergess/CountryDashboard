namespace CountryDashboard.Application;

public static class DependencyInjections
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // Mapster
        MapsterMapping.RegisterMappings();
        services.AddSingleton(TypeAdapterConfig.GlobalSettings);
        services.AddScoped<IMapper, ServiceMapper>();

        // MediatR scan
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
        });

        // Auto-register ToggleSoftDeleteHandler for all ISoftDeletable entities
        RegisterSoftDeletableHandlers(services);

        return services;
    }

    private static void RegisterSoftDeletableHandlers(IServiceCollection services)
    {
        var domainAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name?.Contains("Domain") == true);

        if (domainAssembly == null)
        {
            try
            {
                domainAssembly = Assembly.Load("CountryDashboard.Domain");
            }
            catch
            {
                return;
            }
        }

        var softDeletableTypes = domainAssembly.GetTypes()
            .Where(t => t.IsClass &&
                       !t.IsAbstract &&
                       typeof(ISoftDeletable).IsAssignableFrom(t));

        foreach (var entityType in softDeletableTypes)
        {
            var commandType = typeof(ToggleSoftDeleteCommand<>).MakeGenericType(entityType);
            var handlerInterfaceType = typeof(IRequestHandler<,>).MakeGenericType(commandType, typeof(bool));
            var handlerImplementationType = typeof(ToggleSoftDeleteHandler<>).MakeGenericType(entityType);

            services.AddTransient(handlerInterfaceType, handlerImplementationType);
        }
    }
}