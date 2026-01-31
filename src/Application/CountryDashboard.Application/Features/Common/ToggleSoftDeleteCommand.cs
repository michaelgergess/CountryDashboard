namespace CountryDashboard.Application.Features.Common
{
    public record ToggleSoftDeleteCommand<T>(int Id) : IRequest<bool>
        where T : class, ISoftDeletable;

    public class ToggleSoftDeleteHandler<T> : IRequestHandler<ToggleSoftDeleteCommand<T>, bool>
        where T : class, ISoftDeletable
    {
        private readonly IUnitOfWork _unitOfWork;

        public ToggleSoftDeleteHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(ToggleSoftDeleteCommand<T> request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.ToggleDeletedAsync<T>(request.Id, cancellationToken);
        }
    }
}