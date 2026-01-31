namespace CountryDashboard.Application.Features.Users.Commands.AddUser
{
    public class AddUserCommand : IRequest<ApiResponse>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }

        public void Mapping(TypeAdapterConfig config)
        {
            config.NewConfig<AddUserCommand, User>();
            config.NewConfig<User, AddUserCommand>();
        }
    }

    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, ApiResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRedisCacheService _cache;

        public AddUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IRedisCacheService cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<ApiResponse> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<User>(request);

            await _unitOfWork.Repository<User>().AddAsync(entity, cancellationToken);
            var saveResult = await _unitOfWork.CommitAsync(cancellationToken);

            if (saveResult <= 0)
                return ApiResponse.Failure("Failed to add user.", HttpStatusCode.BadRequest);

            // Cache single user
            await _cache.SetAsync(CacheKeys.User(entity.Id), entity, TimeSpan.FromHours(1));

            // Invalidate users list
            await _cache.RemoveAsync(key: CacheKeys.UsersList);

            return ApiResponse.Success(entity);
        }
    }
}
