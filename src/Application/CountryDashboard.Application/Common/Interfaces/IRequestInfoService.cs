namespace CountryDashboard.Application.Common.Interfaces;

public interface IRequestInfoService
{
    string GetRequestIP();
    string? GetOrigin();
}
