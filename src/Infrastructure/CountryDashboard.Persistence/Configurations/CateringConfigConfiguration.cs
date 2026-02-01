using CountryDashboard.Domain.Entities.Country;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class CateringConfigConfiguration : BaseEntityConfiguration<CateringConfig>
    {
        public override void Configure(EntityTypeBuilder<CateringConfig> entity)
        {
            base.Configure(entity);
            
            entity.ToTable("CateringConfig", "Country");

            entity.HasKey(e => e.Id).HasName("PK__Catering__3214EC0728240B69");

            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            
            entity.Property(e => e.CreatedDate)
                  .HasColumnName("CreatedDate")
                  .HasColumnType("datetime")
                  .IsRequired();

            entity.Property(e => e.CreatedBy)
                  .HasColumnName("CreatedBy")
                  .IsRequired();
            
            entity.Property(e => e.DelevaryCharge).HasAnnotation("Relational:DefaultConstraintName", "DF_CountryCateringConfig_CateringDelevaryCharge");
            entity.Property(e => e.MaxOrderHours).HasAnnotation("Relational:DefaultConstraintName", "DF_CountryCateringConfig_CateringMaxOrderHours");
            entity.Property(e => e.MinOrderHours).HasAnnotation("Relational:DefaultConstraintName", "DF_CountryCateringConfig_CateringMinOrderHours");
            entity.Property(e => e.MinValue).HasAnnotation("Relational:DefaultConstraintName", "DF_CountryCateringConfig_CateringMinValue");
            
            entity.HasOne(d => d.Country).WithMany(p => p.CateringConfigs)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK_CountryCateringConfig_Country");
        }
    }
}
