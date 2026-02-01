using CountryDashboard.Domain.Entities.Country;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class FacilityLangConfiguration : IEntityTypeConfiguration<FacilityLang>
    {
        public void Configure(EntityTypeBuilder<FacilityLang> entity)
        {
            entity.ToTable("FacilityLang", "Country");

            entity.HasKey(e => new { e.FacilityId, e.LanguageId }).HasName("PK_CountryFacilityLang");

            entity.Property(e => e.FacilityId).HasColumnName("FacilityID");
            entity.Property(e => e.LanguageId).HasColumnName("LanguageID");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
