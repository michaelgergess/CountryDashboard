using CountryDashboard.Domain.Entities.Country;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class LoyaltyConfigConfiguration : BaseEntityConfiguration<LoyaltyConfig>
    {
        public override void Configure(EntityTypeBuilder<LoyaltyConfig> entity)
        {
            base.Configure(entity);
            
            entity.ToTable("LoyaltyConfig", "Country");

            entity.HasKey(e => e.CountryId).HasName("PK_LoyaltyConfig_1");

            entity.Property(e => e.CountryId)
                .ValueGeneratedNever()
                .HasColumnName("CountryID");
           
            // Base handles Audit fields. Nullable.

            entity.Property(e => e.PointsExpire).HasAnnotation("Relational:DefaultConstraintName", "DF_CountryLoyaltyConfig_PointsExpire");

            entity.HasOne(d => d.Country).WithOne(p => p.LoyaltyConfig)
                .HasForeignKey<LoyaltyConfig>(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CountryLoyaltyConfig_Country");
        }
    }
}
