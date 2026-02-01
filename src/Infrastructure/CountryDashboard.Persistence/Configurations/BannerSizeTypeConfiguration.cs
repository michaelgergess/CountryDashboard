using CountryDashboard.Domain.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class BannerSizeTypeConfiguration : IEntityTypeConfiguration<BannerSizeType>
    {
        public void Configure(EntityTypeBuilder<BannerSizeType> entity)
        {
            entity.ToTable("BannerSizeType", "Enum");

            entity.HasKey(e => e.Id).HasName("PK__BannerSi__3214EC27DFD3D3AB");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Guid).HasColumnName("GUID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        }
    }
}
