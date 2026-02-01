using CountryDashboard.Domain.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class EnumOrderTypeConfiguration : IEntityTypeConfiguration<OrderType>
    {
        public void Configure(EntityTypeBuilder<OrderType> entity)
        {
            entity.ToTable("OrderType", "Enum");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MigrationId).HasColumnName("MigrationID");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(400);
        }
    }
}
