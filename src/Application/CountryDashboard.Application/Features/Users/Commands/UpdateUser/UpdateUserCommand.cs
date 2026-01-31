namespace CountryDashboard.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<ApiResponse>, IMapFrom<User>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public void Mapping(TypeAdapterConfig profile)
        {
            profile.NewConfig<User, UpdateUserCommand>();
            profile.NewConfig<UpdateUserCommand, User>();
        }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ApiResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly MapsterMapper.IMapper _mapper;
        private readonly IRedisCacheService _cache;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, MapsterMapper.IMapper mapper, IRedisCacheService cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<ApiResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<User>();
            var user = await repo.GetByIdAsync(request.Id, cancellationToken);

            if (user == null)
                return ApiResponse.Failure($"User {request.Id} not found");

            _mapper.Map(request, user);
            repo.Update(user);

            var res = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (res <= 0)
                return ApiResponse.Failure($"Failed to update user {request.Id}");

            // Update user cache
            await _cache.SetAsync(CacheKeys.User(user.Id), user, TimeSpan.FromHours(1));

            // Invalidate list cache
            await _cache.RemoveAsync(CacheKeys.UsersList);

            return ApiResponse.Success(user);
        }
    }
}
