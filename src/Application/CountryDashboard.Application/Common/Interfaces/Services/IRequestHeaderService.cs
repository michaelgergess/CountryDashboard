namespace CountryDashboard.Application.Common.Interfaces.Services;

public interface IRequestHeaderService
{
    public int CountryId { get; }
    public string? DeviceID { get; }
    public string? BrowserID { get; }
    public string? UserIP { get; }
}
