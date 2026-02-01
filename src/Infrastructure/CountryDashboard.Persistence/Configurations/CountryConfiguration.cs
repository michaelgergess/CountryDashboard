using CountryDashboard.Domain.Entities.Country;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class CountryConfiguration : BaseEntityConfiguration<Country>
    {
        public override void Configure(EntityTypeBuilder<Country> builder)
        {
            base.Configure(builder);

            builder.ToTable("Country", "Country");

            builder.HasIndex(e => e.NameUnique, "UN_Country_NameUnique").IsUnique();

            builder.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");

            // DisplayOrder not configured in TexasContext, assumed default

            builder.Property(e => e.CreatedDate)
                .IsRequired()
                .HasColumnName("CreatedDate")
                .HasColumnType("datetime");

            builder.Property(e => e.CreatedBy)
                .IsRequired()
                .HasColumnName("CreatedBy");
                
            builder.Property(e => e.FlagUrl)
                .IsRequired()
                .HasMaxLength(80)
                .HasColumnName("FlagURL");

            builder.Property(e => e.Guid)
                .HasDefaultValueSql("(newid())")
                .HasAnnotation("Relational:DefaultConstraintName", "DF_Country_GUID")
                .HasColumnName("GUID");

            builder.Property(e => e.IntegrationKey)
                .HasMaxLength(255)
                .HasDefaultValue("-1")
                .HasAnnotation("Relational:DefaultConstraintName", "DF_Country_IntegrationKey");

            // IsDeleted assumed default

            builder.Property(e => e.IsTwoFactorEnabled)
                .HasDefaultValue(true)
                .HasAnnotation("Relational:DefaultConstraintName", "DF_Country_IsTwoFactorEnabled");

            builder.Property(e => e.Isocode)
                .IsRequired()
                .HasMaxLength(2)
                .HasDefaultValue("eg")
                .HasAnnotation("Relational:DefaultConstraintName", "DF_Country_CountryISOCode")
                .HasColumnName("ISOCode");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.NameNative)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.NameUnique).HasMaxLength(255);

            builder.Property(e => e.RegionName).HasMaxLength(255);

            builder.Property(e => e.RegionNameNative).HasMaxLength(255);

            builder.Property(e => e.WebsiteUrl)
                .HasMaxLength(255)
                .HasColumnName("WebsiteURL");
        }
    }
}
