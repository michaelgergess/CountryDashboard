using CountryDashboard.Domain.Entities.Country;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class SocialPlatformConfiguration : BaseEntityConfiguration<SocialPlatform>
    {
        public override void Configure(EntityTypeBuilder<SocialPlatform> entity)
        {
            base.Configure(entity);
            
            entity.ToTable("SocialPlatform", "Country");

            entity.HasKey(e => e.Id).HasName("PK_CountrySocialPlatform");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            
            // Base handles CreatedDate, ModifiedDate, etc mapped to correct columns. 
            // Nullable properties in BaseEntity match Nullable columns. No IsRequired() needed.
            
            entity.Property(e => e.Guid)
                .HasDefaultValueSql("(newid())")
                .HasAnnotation("Relational:DefaultConstraintName", "DF_SocialPlatform_GUID")
                .HasColumnName("GUID");
            entity.Property(e => e.IconUrl)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("IconURL");
            entity.Property(e => e.IntegrationKey)
                .HasMaxLength(255)
                .HasDefaultValue("")
                .HasAnnotation("Relational:DefaultConstraintName", "DF_SocialPlatform_IntegrationKey");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasAnnotation("Relational:DefaultConstraintName", "DF_SocialPlatform_IsDeleted");
            entity.Property(e => e.MigrationId).HasColumnName("MigrationID");
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Url)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("URL");

            entity.HasOne(d => d.Country).WithMany(p => p.SocialPlatforms)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CountrySocialPlatform_Country");
        }
    }
}
