using CountryDashboard.Domain.Entities.Country;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class AuthenticationPlatformConfiguration : BaseEntityConfiguration<AuthenticationPlatform>
    {
        public override void Configure(EntityTypeBuilder<AuthenticationPlatform> entity)
        {
            base.Configure(entity);
            
            entity.ToTable("AuthenticationPlatform", "Country");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AuthenticationTypeId).HasColumnName("AuthenticationTypeID");
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            
            entity.Property(e => e.CreatedDate)
                  .HasColumnName("CreatedDate")
                  .HasColumnType("datetime")
                  .IsRequired();

            entity.Property(e => e.CreatedBy)
                  .HasColumnName("CreatedBy")
                  .IsRequired();

            entity.Property(e => e.Guid)
                .HasDefaultValueSql("(newid())")
                .HasAnnotation("Relational:DefaultConstraintName", "DF_AuthenticationPlatform_GUID")
                .HasColumnName("GUID");
            entity.Property(e => e.IntegrationKey).HasMaxLength(255);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.Country).WithMany(p => p.AuthenticationPlatforms)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AuthenticationPlatform_Country");
        }
    }
}
