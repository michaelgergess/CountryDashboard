using CountryDashboard.Domain.Entities.Country;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class OrderStatusConfiguration : BaseEntityConfiguration<OrderStatus>
    {
        public override void Configure(EntityTypeBuilder<OrderStatus> entity)
        {
            base.Configure(entity);
            
            entity.ToTable("OrderStatus", "Country");

            entity.HasKey(e => e.Id).HasName("country_orderstatus_id_primary");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CountryId).HasColumnName("CountryID");

            // Base handles Audit fields. Nullable.
            
            entity.Property(e => e.Guid)
                .HasDefaultValueSql("(newid())")
                .HasAnnotation("Relational:DefaultConstraintName", "DF_OrderStatus_GUID_1")
                .HasColumnName("GUID");
            entity.Property(e => e.IntegrationKey).HasMaxLength(255);
            entity.Property(e => e.IsDeleted).HasAnnotation("Relational:DefaultConstraintName", "DF_OrderStatus_IsDeleted");
            entity.Property(e => e.MigrationId).HasColumnName("MigrationID");
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.NameNative)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.NameUnique)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.StatusIconOff)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.StatusIconOn)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.Country).WithMany(p => p.OrderStatuses)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderStatus_Country");
        }
    }
}
