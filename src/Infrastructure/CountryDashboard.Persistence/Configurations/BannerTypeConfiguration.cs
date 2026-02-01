using CountryDashboard.Domain.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class BannerTypeConfiguration : IEntityTypeConfiguration<BannerType>
    {
        public void Configure(EntityTypeBuilder<BannerType> entity)
        {
            entity.ToTable("BannerType", "Enum");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Guid)
                .HasDefaultValueSql("(newid())")
                .HasAnnotation("Relational:DefaultConstraintName", "DF_BannerType_GUID")
                .HasColumnName("GUID");
            entity.Property(e => e.MigrationId).HasColumnName("MigrationID");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
