using CountryDashboard.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryDashboard.Persistence.Configurations
{
    public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T>
        where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.CreatedDate)
                   .HasColumnName("CreatedDate")
                   .HasColumnType("datetime");

            builder.Property(x => x.LastModifiedDate)
                   .HasColumnName("ModifiedDate")
                   .HasColumnType("datetime");

            builder.Property(x => x.CreatedBy)
                   .HasColumnName("CreatedBy");

            builder.Property(x => x.LastModifiedBy)
                   .HasColumnName("ModifiedBy");
        }
    }
}
