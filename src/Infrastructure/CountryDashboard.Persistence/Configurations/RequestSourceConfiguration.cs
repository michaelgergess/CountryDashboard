using CountryDashboard.Domain.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class RequestSourceConfiguration : BaseEntityConfiguration<RequestSource>
    {
        public override void Configure(EntityTypeBuilder<RequestSource> entity)
        {
            base.Configure(entity);
            
            entity.ToTable("RequestSource", "Enum");

            entity.HasKey(e => e.Id).HasName("enums_requestSource_id_primary");

            entity.Property(e => e.Id).HasColumnName("ID");
            
            entity.Property(e => e.CreatedDate)
                  .HasColumnName("CreatedDate")
                  .HasColumnType("datetime")
                  .IsRequired();

            entity.Property(e => e.CreatedBy)
                  .HasColumnName("CreatedBy")
                  .IsRequired();

            entity.Property(e => e.Guid)
                .HasDefaultValueSql("(newid())")
                .HasAnnotation("Relational:DefaultConstraintName", "DF_RequestSource_GUID")
                .HasColumnName("GUID");
            entity.Property(e => e.MigrationId).HasColumnName("MigrationID");
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
