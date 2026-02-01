using CountryDashboard.Domain.Entities.Country;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class AggregatorConfiguration : BaseEntityConfiguration<Aggregator>
    {
        public override void Configure(EntityTypeBuilder<Aggregator> entity)
        {
            base.Configure(entity);
            
            entity.ToTable("Aggregator", "Country");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            
            entity.Property(e => e.CreatedDate)
                  .HasColumnName("CreatedDate")
                  .HasColumnType("datetime")
                  .IsRequired();

            entity.Property(e => e.CreatedBy)
                  .HasColumnName("CreatedBy")
                  .IsRequired();

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.DescriptionNative).HasMaxLength(1000);
            entity.Property(e => e.Guid)
                .HasDefaultValueSql("(newid())")
                .HasAnnotation("Relational:DefaultConstraintName", "DF_Aggregator_GUID")
                .HasColumnName("GUID");
            entity.Property(e => e.IconUrl)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("IconURL");
            entity.Property(e => e.IntegrationKey).HasMaxLength(255);
            entity.Property(e => e.Link).HasMaxLength(255);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.NameNative)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.NameUnique)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
