namespace CountryDashboard.Persistence.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly IDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(IDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        // ----------- READ METHODS -----------

        // Returns an IQueryable for further querying and projection by the caller
        //when calling this method, ensure to apply AsNoTracking for read-only scenarios
        // add CancellationToken when calling ToListAsync or similar methods on the returned IQueryable
        public IQueryable<T> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbSet.FindAsync(id, cancellationToken);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dbSet.AnyAsync(predicate, cancellationToken);
        }

        // ----------- WRITE METHODS -----------

        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity, CancellationToken cancellationToken)
        {
            _dbSet.Remove(entity);
        }
    }
}
