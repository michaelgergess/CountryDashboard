namespace CountryDashboard.Application.Features.Users.Queries.GetUsers
{
    public class GetUsersQuery : IRequest<ApiResponse>
    {
        public int? Id { get; set; }
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, ApiResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRedisCacheService _cache;

        public GetUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IRedisCacheService cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<ApiResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            // 1️⃣ Try cache
            var cachedUsers = await _cache.GetAsync<List<User>>(CacheKeys.UsersList);

            if (cachedUsers != null)
            {
                var mapped = _mapper.Map<List<GetUserResponse>>(cachedUsers);
                return ApiResponse.Success(mapped);
            }

            // 2️⃣ DB
            var users = await _unitOfWork.Repository<User>().Query().ToListAsync(cancellationToken);

            if (!users.Any())
                return ApiResponse.Failure("No users found");

            // Cache list
            await _cache.SetAsync(CacheKeys.UsersList, users, TimeSpan.FromMinutes(10));

            var mappedUsers = _mapper.Map<List<GetUserResponse>>(users);
            return ApiResponse.Success(mappedUsers);
        }
    }
}
