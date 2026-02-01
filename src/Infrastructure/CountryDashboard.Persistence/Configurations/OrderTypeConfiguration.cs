using CountryDashboard.Domain.Entities.Country;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public class OrderTypeConfiguration : BaseEntityConfiguration<OrderType>
    {
        public override void Configure(EntityTypeBuilder<OrderType> entity)
        {
            base.Configure(entity);
            
            entity.ToTable("OrderType", "Country");

            entity.HasKey(e => e.Id).HasName("PK_OrderTypes");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CountryId)
                .HasAnnotation("Relational:DefaultConstraintName", "DF_OrderType_CountryID")
                .HasColumnName("CountryID");
            
            entity.Property(e => e.CreatedDate)
                  .HasColumnName("CreatedDate")
                  .HasColumnType("datetime")
                  .IsRequired();

            entity.Property(e => e.CreatedBy)
                  .HasColumnName("CreatedBy")
                  .IsRequired();

            entity.Property(e => e.Guid)
                .HasDefaultValueSql("(newid())")
                .HasAnnotation("Relational:DefaultConstraintName", "DF_OrderType_GUID")
                .HasColumnName("GUID");
            entity.Property(e => e.IconUrl)
                .IsRequired()
                .HasMaxLength(400)
                .HasColumnName("IconURL");
            entity.Property(e => e.IntegrationKey)
                .HasMaxLength(255)
                .HasDefaultValue("-1")
                .HasAnnotation("Relational:DefaultConstraintName", "DF_OrderTypes_Integrationkey");
            entity.Property(e => e.IsBogodiscountEnabled).HasColumnName("IsBOGODiscountEnabled");
            entity.Property(e => e.MigrationId).HasColumnName("MigrationID");
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(400);
            entity.Property(e => e.NameNative)
                .IsRequired()
                .HasMaxLength(400);
            entity.Property(e => e.NameUnique).HasMaxLength(255);
            entity.Property(e => e.TotalDiscountPercentage)
                .HasDefaultValue(0.0)
                .HasAnnotation("Relational:DefaultConstraintName", "DF_OrderType_TotalDiscountPercentage");
            entity.Property(e => e.TotalDiscountValue)
                .HasDefaultValue(0.0)
                .HasAnnotation("Relational:DefaultConstraintName", "DF_OrderType_TotalDiscountValue");
            entity.Property(e => e.TypeId).HasColumnName("TypeID");

            entity.HasOne(d => d.Country).WithMany(p => p.OrderTypes)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderType_Country");

            entity.HasOne(d => d.Type).WithMany(p => p.OrderTypes)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderType_OrderType");
        }
    }
}
