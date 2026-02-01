using CountryDashboard.Domain.Entities.Country;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class PaymentConfiguration : BaseEntityConfiguration<Payment>
    {
        public override void Configure(EntityTypeBuilder<Payment> entity)
        {
            base.Configure(entity);
            
            entity.ToTable("Payment", "Country");

            entity.HasKey(e => e.Id).HasName("PK_CountryPayment");

            entity.Property(e => e.Id).HasColumnName("ID");
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
                .HasAnnotation("Relational:DefaultConstraintName", "DF_Payment_GUID")
                .HasColumnName("GUID");
            entity.Property(e => e.IntegrationKey)
                .HasMaxLength(255)
                .HasDefaultValue("")
                .HasAnnotation("Relational:DefaultConstraintName", "DF_CountryPayment_IntegrationKey");
            entity.Property(e => e.IsDeleted).HasAnnotation("Relational:DefaultConstraintName", "DF_Payment_IsDeleted");
            entity.Property(e => e.LogoUrl)
                .IsRequired()
                .HasMaxLength(2000)
                .HasColumnName("LogoURL");
            entity.Property(e => e.MigrationId).HasColumnName("MigrationID");
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.NameNative)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.NameUnique).HasMaxLength(200);
            entity.Property(e => e.ResponseFailUrl)
                .HasMaxLength(400)
                .HasColumnName("ResponseFailURL");
            entity.Property(e => e.ResponseSuccessUrl)
                .HasMaxLength(400)
                .HasColumnName("ResponseSuccessURL");
            entity.Property(e => e.SecurityField1)
                .IsRequired()
                .HasMaxLength(4000);
            entity.Property(e => e.SecurityField2)
                .IsRequired()
                .HasMaxLength(4000);
            entity.Property(e => e.SecurityField3)
                .IsRequired()
                .HasMaxLength(4000);
            entity.Property(e => e.SecurityField4)
                .IsRequired()
                .HasMaxLength(4000);
            entity.Property(e => e.SecurityField5)
                .IsRequired()
                .HasMaxLength(4000);
            entity.Property(e => e.SecurityField6)
                .IsRequired()
                .HasMaxLength(4000);
            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.Url)
                .IsRequired()
                .HasMaxLength(4000)
                .HasColumnName("URL");

            entity.HasOne(d => d.Country).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CountryPayment_Country");
        }
    }
}
