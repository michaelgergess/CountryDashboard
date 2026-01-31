namespace CountryDashboard.API.Filters;

/// <summary>
/// Marks an endpoint or controller as accessible only by PS Admin users.
/// When applied, the request will be authorized only if the JWT token
/// contains IsPsAdmin = true.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class PsAdminOnlyAttribute : Attribute
{
}