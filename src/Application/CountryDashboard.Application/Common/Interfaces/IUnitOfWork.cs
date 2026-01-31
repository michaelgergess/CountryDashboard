namespace CountryDashboard.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>() where T : class;
        /// <summary>
        /// Begins a new database transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task BeginTransactionAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Saves all changes made in this context to the database asynchronously.
        /// Handles and rethrows exceptions with additional context.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to cancel the asynchronous operation.</param>
        /// <returns>
        /// A task that represents the asynchronous save operation. 
        /// The task result contains the number of state entries written to the database.
        /// </returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Commits the current transaction asynchronously and saves all changes to the database.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to cancel the asynchronous operation.</param>
        /// <returns>
        /// A task that represents the asynchronous commit operation.
        /// The task result contains the number of state entries written to the database.
        /// </returns>
        Task<int> CommitAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Rolls back the current transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous rollback operation.</returns>
        Task RollbackAsync(CancellationToken cancellationToken);
        Task<bool> ToggleDeletedAsync<T>(int id, CancellationToken cancellationToken = default)
            where T : class, ISoftDeletable;
    }

}
