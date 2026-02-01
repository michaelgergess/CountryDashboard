using CountryDashboard.Domain.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class DisplayModeConfiguration : IEntityTypeConfiguration<DisplayMode>
    {
        public void Configure(EntityTypeBuilder<DisplayMode> entity)
        {
            entity.ToTable("DisplayMode", "Enum");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IsDeleted).HasAnnotation("Relational:DefaultConstraintName", "DF_DisplayMode_IsDeleted");
            entity.Property(e => e.MigrationId).HasColumnName("MigrationID");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
