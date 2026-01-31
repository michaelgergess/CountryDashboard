namespace CountryDashboard.Application.Common.Attributes;
// Custom attribute to set rate limiting rules
[AttributeUsage(AttributeTargets.Method)]
public class RateLimitAttribute : Attribute
{
    public int PerMinute { get; }
    public int PerHour { get; }
    public int PerDay { get; }

    public RateLimitAttribute(int perMinute = -1, int perHour = -1, int perDay = -1)
    {
        PerMinute = perMinute;
        PerHour = perHour;
        PerDay = perDay;
    }

    public bool HasPerMinute => PerMinute > -1;
    public bool HasPerHour => PerHour > -1;
    public bool HasPerDay => PerDay > -1;
}

