namespace CountryDashboard.Application.Common.Interfaces
{
    public interface IConcurrencyEntity
    {
        byte[] RowVersion { get; set; }
    }
}
