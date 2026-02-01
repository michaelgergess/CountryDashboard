using CountryDashboard.Domain.Entities.Country;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class LanguageConfiguration : BaseEntityConfiguration<Language>
    {
        public override void Configure(EntityTypeBuilder<Language> entity)
        {
            base.Configure(entity);
            
            entity.ToTable("Language", "Country");

            entity.HasKey(e => e.Id).HasName("PK_CountryLanguage");

            entity.Property(e => e.Id).HasColumnName("ID");
            
            // Base handles Audit fields. Nullable.
            
            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(5);
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.Direction)
                .IsRequired()
                .HasMaxLength(3);
            entity.Property(e => e.Guid)
                .HasDefaultValueSql("(newid())")
                .HasAnnotation("Relational:DefaultConstraintName", "DF_Language_GUID")
                .HasColumnName("GUID");
            entity.Property(e => e.IntegrationKey).HasMaxLength(255);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.Country).WithMany(p => p.Languages)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CountryLanguage_Country");
        }
    }
}
