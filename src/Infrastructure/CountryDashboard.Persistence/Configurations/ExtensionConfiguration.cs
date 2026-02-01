using CountryDashboard.Domain.Entities.Country;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class ExtensionConfiguration : BaseEntityConfiguration<Extension>
    {
        public override void Configure(EntityTypeBuilder<Extension> entity)
        {
            base.Configure(entity);
            
            entity.ToTable("Extension", "Country");

            entity.HasKey(e => e.CountryId);

            entity.Property(e => e.CountryId)
                .ValueGeneratedNever()
                .HasColumnName("CountryID");
                
            // Base handles Audit fields. Nullable.
            
            entity.Property(e => e.CmpId)
                .HasMaxLength(35)
                .HasColumnName("cmpID");
            entity.Property(e => e.CountryPhoneCode).HasMaxLength(10);
            
            entity.Property(e => e.CurrencyIsocode)
                .HasMaxLength(3)
                .HasDefaultValue("")
                .HasAnnotation("Relational:DefaultConstraintName", "CountryExtension_CurrencyIsoCode")
                .HasColumnName("CurrencyISOCode");
            entity.Property(e => e.CurrencyName).HasMaxLength(30);
            entity.Property(e => e.CurrencyNameNative).HasMaxLength(30);
            entity.Property(e => e.CustomerServiceLine).HasMaxLength(200);
            entity.Property(e => e.EnableAddressMapLocation)
                .HasDefaultValue(false)
                .HasAnnotation("Relational:DefaultConstraintName", "DF_CountryExtension_EnableAddressMapLocation");
            entity.Property(e => e.EnableAgeConfirmation)
                .HasDefaultValue(false)
                .HasAnnotation("Relational:DefaultConstraintName", "DF_CountryExtension_EnableAgeConfirmation");
            entity.Property(e => e.EnableCalories).HasAnnotation("Relational:DefaultConstraintName", "CountryExtension_CaloriesEnabled_Default");
            entity.Property(e => e.EnableCmp).HasColumnName("EnableCMP");
            entity.Property(e => e.EnableMarketingConfirmation)
                .HasDefaultValue(false)
                .HasAnnotation("Relational:DefaultConstraintName", "DF_CountryExtension_EnableMarketingConfirmation");
            entity.Property(e => e.EnableOrderNotificationConfirmation)
                .HasDefaultValue(false)
                .HasAnnotation("Relational:DefaultConstraintName", "DF_CountryExtension_EnableOrderNotificationConfirmation");
            entity.Property(e => e.EnablePrivacyPolicyConfirmation)
                .HasDefaultValue(false)
                .HasAnnotation("Relational:DefaultConstraintName", "DF_CountryExtension_EnablePrivacyPolicyConfirmation");
            entity.Property(e => e.EnableTermsConfirmation)
                .HasDefaultValue(false)
                .HasAnnotation("Relational:DefaultConstraintName", "DF_CountryExtension_EnableTermsConfirmation");
            entity.Property(e => e.IsBogodiscountEnabled).HasColumnName("IsBOGODiscountEnabled");
            entity.Property(e => e.IsDeliveryTrackingPopupIosenabled).HasColumnName("IsDeliveryTrackingPopupIOSEnabled");
            entity.Property(e => e.IsTaxInclusive).HasAnnotation("Relational:DefaultConstraintName", "DF_CountryExtension_TaxInclusive");
            
            entity.Property(e => e.OrderEmail)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.PhoneRegex)
                .IsRequired()
                .HasMaxLength(255)
                .HasDefaultValue("")
                .HasAnnotation("Relational:DefaultConstraintName", "CountryExtension_PhoneCode");
            entity.Property(e => e.TaxLabelDisplayName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TaxLabelDisplayNameNative)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.TikTokAndroidAppId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TikTokIosAppId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TimeDifference)
                .HasDefaultValue(0)
                .HasAnnotation("Relational:DefaultConstraintName", "DF_CountryExtension_TimeDifference");
            entity.Property(e => e.TotalDiscountPercentage)
                .HasDefaultValue(0.0)
                .HasAnnotation("Relational:DefaultConstraintName", "DF_CountryExtension_TotalDiscountPercentage");
            entity.Property(e => e.TotalDiscountValue)
                .HasDefaultValue(0.0)
                .HasAnnotation("Relational:DefaultConstraintName", "DF_CountryExtension_TotalDiscountValue");

            entity.HasOne(d => d.Country).WithOne(p => p.Extension)
                .HasForeignKey<Extension>(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CountryExtension_Country");
        }
    }
}
