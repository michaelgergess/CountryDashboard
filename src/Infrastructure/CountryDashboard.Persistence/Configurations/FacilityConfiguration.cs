using CountryDashboard.Domain.Entities.Country;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class FacilityConfiguration : BaseEntityConfiguration<Facility>
    {
        public override void Configure(EntityTypeBuilder<Facility> entity)
        {
            base.Configure(entity);
            
            entity.ToTable("Facility", "Country");

            entity.HasKey(e => e.Id).HasName("PK_CountryFacility");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            
            entity.Property(e => e.CreatedDate)
                  .HasColumnName("CreatedDate")
                  .HasColumnType("datetime")
                  .IsRequired();

            entity.Property(e => e.CreatedBy)
                  .HasColumnName("CreatedBy")
                  .IsRequired();
            
            entity.Property(e => e.DisplayOrder)
                .HasDefaultValue(1)
                .HasAnnotation("Relational:DefaultConstraintName", "DF_Facility_DisplayOrder");
            entity.Property(e => e.Guid).HasColumnName("GUID");
            entity.Property(e => e.IconUrl)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("IconURL");
            
            entity.Property(e => e.NameUnique)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
