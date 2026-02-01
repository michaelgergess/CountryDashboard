using CountryDashboard.Domain.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class DeleteReasonConfiguration : IEntityTypeConfiguration<DeleteReason>
    {
        public void Configure(EntityTypeBuilder<DeleteReason> entity)
        {
            entity.ToTable("DeleteReason", "Enum");

            entity.HasKey(e => e.Id).HasName("enum_deletereason_id_primary");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Guid)
                .HasDefaultValueSql("(newid())")
                .HasAnnotation("Relational:DefaultConstraintName", "DF_DeleteReason_GUID")
                .HasColumnName("GUID");
            entity.Property(e => e.MigrationId).HasColumnName("MigrationID");
            entity.Property(e => e.Reason)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
