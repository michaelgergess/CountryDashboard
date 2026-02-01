using CountryDashboard.Domain.Entities.Country;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class CitizenshipConfiguration : BaseEntityConfiguration<Citizenship>
    {
        public override void Configure(EntityTypeBuilder<Citizenship> entity)
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
                  .HasColumnName("LastModificationBy");

            entity.ToTable("Citizenship", "Country");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CountryId).HasColumnName("CountryID");

            entity.Property(e => e.DisplayOrder).HasAnnotation("Relational:DefaultConstraintName", "DF_Citizenship_DisplayOrder");
            entity.Property(e => e.MigrationId).HasColumnName("MigrationID");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasAnnotation("Relational:DefaultConstraintName", "DF_Citizenship_Status");
        }
    }
}
