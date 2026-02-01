using CountryDashboard.Domain.Entities.Country;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class PaymentRequestSourceConfiguration : BaseEntityConfiguration<PaymentRequestSource>
    {
        public override void Configure(EntityTypeBuilder<PaymentRequestSource> entity)
        {
            base.Configure(entity);
            
            entity.ToTable("PaymentRequestSource", "Country");

            entity.HasKey(e => new { e.PaymentId, e.RequestSource });

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.RequestSource).HasMaxLength(200);
            
            entity.Property(e => e.CreatedDate)
                  .HasColumnName("CreatedDate")
                  .HasColumnType("datetime")
                  .IsRequired();

            entity.Property(e => e.CreatedBy)
                  .HasColumnName("CreatedBy")
                  .IsRequired();

            entity.Property(e => e.DeviceModel).HasMaxLength(200);
            entity.Property(e => e.IsDeleted).HasAnnotation("Relational:DefaultConstraintName", "DF_PaymentRequestSource_IsDeleted");
            entity.Property(e => e.MigrationId).HasColumnName("MigrationID");
        }
    }
}
