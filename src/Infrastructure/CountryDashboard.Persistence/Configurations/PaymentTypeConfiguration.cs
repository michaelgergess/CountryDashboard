using CountryDashboard.Domain.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class PaymentTypeConfiguration : IEntityTypeConfiguration<PaymentType>
    {
        public void Configure(EntityTypeBuilder<PaymentType> entity)
        {
            entity.ToTable("PaymentType", "Enum");

            entity.HasKey(e => e.Id).HasName("PK__PaymentT__3214EC27427EB535");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Guid).HasColumnName("GUID");
            entity.Property(e => e.MigrationId).HasColumnName("MigrationID");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
