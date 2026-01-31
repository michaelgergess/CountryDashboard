

namespace CountryDashboard.Persistence.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.FirstName).IsRequired().HasMaxLength(40).HasColumnType("varchar(40)");

            builder.Property(u => u.LastName).IsRequired().HasMaxLength(40).HasColumnType("varchar(40)");
            builder.Property(u => u.Name).IsRequired().HasMaxLength(80).HasColumnType("varchar(80)");

        }
    }
}
