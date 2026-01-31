namespace CountryDashboard.Application.Common.Interfaces
{
    public interface IRepository<T> : IReadRepository<T> where T : class
    {
        Task AddAsync(T entity, CancellationToken cancellationToken);
        void Update(T entity);
        void Delete(T entity, CancellationToken cancellationToken);
    }
}

