namespace CountryDashboard.Application.Common.Mapping
{
    public static class MapsterMapping
    {
        public static void RegisterMappings()
        {
            var config = TypeAdapterConfig.GlobalSettings;
            ApplyMappingsForAssembly(Assembly.GetExecutingAssembly(), config);
        }

        private static void ApplyMappingsForAssembly(Assembly assembly, TypeAdapterConfig config)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping")
                                 ?? type.GetInterface("IMapFrom`1")?.GetMethod("Mapping");

                methodInfo?.Invoke(instance, new object[] { config });
            }
        }
    }
}
