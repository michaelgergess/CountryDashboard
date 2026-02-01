using CountryDashboard.Domain.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class ReferenceConfiguration : IEntityTypeConfiguration<Reference>
    {
        public void Configure(EntityTypeBuilder<Reference> entity)
        {
            entity.ToTable("Reference", "Enum");

            entity.HasKey(e => e.Id).HasName("enum_reference_id_primary");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AffectOnPoints).HasMaxLength(50);
            entity.Property(e => e.Guid)
                .HasDefaultValueSql("(newid())")
                .HasAnnotation("Relational:DefaultConstraintName", "DF_Reference_GUID")
                .HasColumnName("GUID");
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("('0')")
                .HasAnnotation("Relational:DefaultConstraintName", "DF__Reference__IsDel__468862B0");
            entity.Property(e => e.MigrationId).HasColumnName("MigrationID");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
