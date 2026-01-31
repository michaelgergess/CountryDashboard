


using System.Linq.Expressions;

namespace CountryDashboard.Application.Common.Interfaces
{
    public interface IReadRepository<T> where T : class
    {
        IQueryable<T> Query();
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
    }

}
