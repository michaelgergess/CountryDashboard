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
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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


        /// <summary>
        /// Saves changes asynchronously with cancellation support.
        /// </summary>
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
