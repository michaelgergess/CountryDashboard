namespace CountryDashboard.Application.Common.Interfaces.Auth
{
    public interface ITokenInfo
    {
        int? UserId { get; }
        int? CountryID { get; }
        // role + permissions can be added later as needed
    }
}
