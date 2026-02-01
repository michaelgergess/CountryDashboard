using CountryDashboard.Domain.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class AuthenticationTypeConfiguration : BaseEntityConfiguration<AuthenticationType>
    {
        public override void Configure(EntityTypeBuilder<AuthenticationType> entity)
        {
            base.Configure(entity);
            
            entity.ToTable("AuthenticationType", "Enum");

            entity.HasKey(e => e.Id).HasName("enums_authenticationtype_id_primary");

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
                .HasAnnotation("Relational:DefaultConstraintName", "DF_AuthenticationType_GUID")
                .HasColumnName("GUID");
            entity.Property(e => e.MigrationId).HasColumnName("MigrationID");
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.NameNative)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.NameUnique)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
