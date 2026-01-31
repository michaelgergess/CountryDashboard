using CountryDashboard.Application.Common.Interfaces;

namespace CountryDashboard.Domain.Entities
{
    public class User : ISoftDeletable
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }

    }
}
