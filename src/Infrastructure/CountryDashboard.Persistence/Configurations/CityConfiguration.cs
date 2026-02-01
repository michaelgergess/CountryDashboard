using CountryDashboard.Domain.Entities.Country;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class CityConfiguration : BaseEntityConfiguration<City>
    {
        public override void Configure(EntityTypeBuilder<City> entity)
        {
            base.Configure(entity);
            
            entity.ToTable("City", "Country");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            
            entity.Property(e => e.CreatedDate)
                  .HasColumnName("CreatedDate") // Explicit for safety, though base does it
                  .HasColumnType("datetime")
                  .IsRequired();

            entity.Property(e => e.CreatedBy)
                  .HasColumnName("CreatedBy")
                  .IsRequired();
            
            entity.Property(e => e.Guid)
                .HasDefaultValueSql("(newid())")
                .HasAnnotation("Relational:DefaultConstraintName", "DF_City_GUID")
                .HasColumnName("GUID");
            entity.Property(e => e.IntegrationKey)
                .HasMaxLength(255)
                .HasDefaultValue("-1")
                .HasAnnotation("Relational:DefaultConstraintName", "DF__City__Integratio__42CCE065");
            entity.Property(e => e.MigrationId).HasColumnName("MigrationID");
            
            // ModifiedDate handled by Base
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.NameNative)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.NameUnique).HasMaxLength(255);

            entity.HasOne(d => d.Country).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_City_Country");
        }
    }
}
