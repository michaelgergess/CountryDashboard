using CountryDashboard.Domain.Entities.Country;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class AreaConfiguration : BaseEntityConfiguration<Area>
    {
        public override void Configure(EntityTypeBuilder<Area> entity)
        {
            base.Configure(entity);
            
            entity.ToTable("Area", "Country");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CityId).HasColumnName("CityID");
            
            entity.Property(e => e.CreatedDate)
                  .HasColumnName("CreatedDate") 
                  .HasColumnType("datetime")
                  .IsRequired();

            entity.Property(e => e.CreatedBy)
                  .HasColumnName("CreatedBy")
                  .IsRequired();
            
            entity.Property(e => e.Guid)
                .HasDefaultValueSql("(newid())")
                .HasAnnotation("Relational:DefaultConstraintName", "DF_Area_GUID")
                .HasColumnName("GUID");
            entity.Property(e => e.Integrationkey)
                .HasMaxLength(255)
                .HasDefaultValue("-1")
                .HasAnnotation("Relational:DefaultConstraintName", "DF__Area__Integratio__3572E547");
            entity.Property(e => e.MigrationId).HasColumnName("MigrationID");
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.NameNative)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.NameUnique).HasMaxLength(255);

            entity.HasOne(d => d.City).WithMany(p => p.Areas)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Area_City");
        }
    }
}
