
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace CountryDashboard.Application.Common.Interfaces;

public interface IDbContext
{
    DbSet<T> Set<T>() where T : class;

    // Add these for detaching
    //TODO (Technical Review): needed to deatach entity after adding it to unit of work 
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

    // Add ChangeTracker access
    ChangeTracker ChangeTracker { get; }

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    // for Command to execute by stored procedures
    Task<bool> ToggleDeletedAsync<TEntity>(int id, CancellationToken cancellationToken) where TEntity : class, ISoftDeletable;

}


