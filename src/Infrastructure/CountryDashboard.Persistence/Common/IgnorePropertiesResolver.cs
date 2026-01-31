using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace CountryDashboard.Persistence.Common;
/// <summary>
/// Custom JSON resolver to ignore specified properties during serialization and deserialization.
/// </summary>
/// <remarks>
/// Initializes a new instance of <see cref="IgnorePropertiesResolver"/> with properties to ignore.
/// </remarks>
/// <param name="propNamesToIgnore">List of property names to ignore.</param>
public class IgnorePropertiesResolver(IEnumerable<string> propNamesToIgnore) : DefaultJsonTypeInfoResolver
{
    private readonly HashSet<string> _propsToIgnore = new HashSet<string>(propNamesToIgnore, StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Modifies the JSON type info to remove ignored properties.
    /// </summary>
    /// <param name="type">The target type.</param>
    /// <param name="options">The JSON serializer options.</param>
    /// <returns>Modified JSON type info without the ignored properties.</returns>
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        var jsonTypeInfo = base.GetTypeInfo(type, options);

        // Ignore properties dynamically
        if (jsonTypeInfo.Kind == JsonTypeInfoKind.Object)
        {
            for (int i = jsonTypeInfo.Properties.Count - 1; i >= 0; i--)
            {
                var prop = jsonTypeInfo.Properties[i];
                if (_propsToIgnore.Contains(prop.Name))
                {
                    jsonTypeInfo.Properties.RemoveAt(i);
                }
            }
        }

        return jsonTypeInfo;
    }
}

