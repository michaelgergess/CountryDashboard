using CountryDashboard.Domain.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class DealTypeConfiguration : IEntityTypeConfiguration<DealType>
    {
        public void Configure(EntityTypeBuilder<DealType> entity)
        {
            entity.ToTable("DealTypes", "Enum");

            entity.HasKey(e => e.Id).HasName("enum_dealtypes_id_primary");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Guid)
                .HasDefaultValueSql("(newid())")
                .HasAnnotation("Relational:DefaultConstraintName", "DF_DealTypes_GUID")
                .HasColumnName("GUID");
            entity.Property(e => e.IsDeleted).HasAnnotation("Relational:DefaultConstraintName", "DF_DealTypes_IsDeleted");
            entity.Property(e => e.MigrationId).HasColumnName("MigrationID");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
