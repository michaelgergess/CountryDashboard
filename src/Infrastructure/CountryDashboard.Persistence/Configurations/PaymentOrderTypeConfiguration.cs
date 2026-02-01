using CountryDashboard.Domain.Entities.Country;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class PaymentOrderTypeConfiguration : BaseEntityConfiguration<PaymentOrderType>
    {
        public override void Configure(EntityTypeBuilder<PaymentOrderType> entity)
        {
            base.Configure(entity);
            
            entity.ToTable("PaymentOrderType", "Country");

            entity.HasKey(e => new { e.OrderTypeId, e.PaymentId });

            entity.Property(e => e.OrderTypeId).HasColumnName("OrderTypeID");
            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            
            entity.Property(e => e.CreatedDate)
                  .HasColumnName("CreatedDate")
                  .HasColumnType("datetime")
                  .IsRequired();

            entity.Property(e => e.CreatedBy)
                  .HasColumnName("CreatedBy")
                  .IsRequired();

            entity.Property(e => e.IntegrationKey)
                .HasMaxLength(255)
                .HasDefaultValue("")
                .HasAnnotation("Relational:DefaultConstraintName", "DF_PaymentOrderType_IntegrationKey");
            entity.Property(e => e.IsDeleted).HasAnnotation("Relational:DefaultConstraintName", "DF_Country.PaymentOrderType_IsDeleted");
            entity.Property(e => e.MigrationId).HasColumnName("MigrationID");

            entity.HasOne(d => d.OrderType).WithMany(p => p.PaymentOrderTypes)
                .HasForeignKey(d => d.OrderTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PaymentOrderType_OrderType");
        }
    }
}
