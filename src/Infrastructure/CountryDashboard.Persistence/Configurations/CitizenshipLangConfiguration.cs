using CountryDashboard.Domain.Entities.Country;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class CitizenshipLangConfiguration : BaseEntityConfiguration<CitizenshipLang>
    {
        public override void Configure(EntityTypeBuilder<CitizenshipLang> entity)
        {
            base.Configure(entity); // Sets up default audit mappings
            
            // Override audit mappings for Legacy Columns
            entity.Property(e => e.CreatedDate)
                  .HasColumnName("CreationDate")
                  .HasColumnType("datetime")
                  .HasDefaultValueSql("(getdate())")
                  .IsRequired(); // Strict

            entity.Property(e => e.CreatedBy)
                  .HasColumnName("CreationBy")
                  .IsRequired(); // Strict

            entity.Property(e => e.LastModifiedDate)
                  .HasColumnName("LastModificationDate")
                  .HasColumnType("datetime");

            entity.Property(e => e.LastModifiedBy)
                  .HasColumnName("Modifiedby"); // Note lowercase 'b'

            entity.ToTable("CitizenshipLang", "Country");

            entity.HasKey(e => new { e.CitizenshipId, e.LangId });

            entity.Property(e => e.CitizenshipId).HasColumnName("CitizenshipID");
            entity.Property(e => e.LangId).HasColumnName("LanguageID");
            
            entity.Property(e => e.MigrationId).HasColumnName("MigrationID");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}
