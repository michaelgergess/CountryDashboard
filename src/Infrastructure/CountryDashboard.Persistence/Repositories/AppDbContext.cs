using CountryDashboard.Domain.Entities.Country;
using CountryDashboard.Domain.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using CountryDashboard.Application.Common.Interfaces;

// Alias to resolve ambiguity
using OrderTypeCountry = CountryDashboard.Domain.Entities.Country.OrderType;
using OrderTypeEnum = CountryDashboard.Domain.Entities.Enum.OrderType;

namespace CountryDashboard.Persistence.Repositories
{
    public class AppDbContext : DbContext, IDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public AppDbContext()
        {
        }

        public virtual DbSet<Aggregator> Aggregators { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<AuthenticationPlatform> AuthenticationPlatforms { get; set; }
        public virtual DbSet<AuthenticationType> AuthenticationTypes { get; set; }
        public virtual DbSet<BannerSizeType> BannerSizeTypes { get; set; }
        public virtual DbSet<BannerType> BannerTypes { get; set; }
        public virtual DbSet<CateringConfig> CateringConfigs { get; set; }
        public virtual DbSet<Citizenship> Citizenships { get; set; }
        public virtual DbSet<CitizenshipLang> CitizenshipLangs { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<ContactMethod> ContactMethods { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<DealType> DealTypes { get; set; }
        public virtual DbSet<DeleteReason> DeleteReasons { get; set; }
        public virtual DbSet<DisplayMode> DisplayModes { get; set; }
        public virtual DbSet<Extension> Extensions { get; set; }
        public virtual DbSet<Facility> Facilities { get; set; }
        public virtual DbSet<FacilityLang> FacilityLangs { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<LoyaltyConfig> LoyaltyConfigs { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; }
        public virtual DbSet<OrderTypeCountry> OrderTypes { get; set; } 
        public virtual DbSet<OrderTypeEnum> OrderTypes1 { get; set; } 
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<PaymentOrderType> PaymentOrderTypes { get; set; }
        public virtual DbSet<PaymentRequestSource> PaymentRequestSources { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        public virtual DbSet<PrivacyRequestType> PrivacyRequestTypes { get; set; }
        public virtual DbSet<Reference> References { get; set; }
        public virtual DbSet<RequestSource> RequestSources { get; set; }
        public virtual DbSet<SocialPlatform> SocialPlatforms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence("InstanceID_Sequence").StartsAt(18L);
            
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            
             foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IConcurrencyEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType).Property<byte[]>("RowVersion")
                        .IsRowVersion()
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();
                }
            }
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        {
            return await base.Database.BeginTransactionAsync(cancellationToken);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ToggleDeletedAsync<TEntity>(int id, CancellationToken cancellationToken)
            where TEntity : class, ISoftDeletable
        {
            var entity = await Set<TEntity>().FindAsync(id);

            if (entity == null)
                throw new Exception($"{typeof(TEntity).Name} with Id {id} not found");

            entity.IsDeleted = !entity.IsDeleted;

            await SaveChangesAsync(cancellationToken);

            return entity.IsDeleted;
        }
    }
}
