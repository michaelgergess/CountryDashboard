

namespace CountryDashboard.Application.Features.Users.Queries.GetUser
{
    public class GetUserQuery : IRequest<ApiResponse>
    {
        public int Id { get; set; }
    }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, ApiResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRedisCacheService _cache;

        public GetUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IRedisCacheService cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<ApiResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            string key = CacheKeys.User(request.Id);

            // 1️⃣ try cache
            var cachedUser = await _cache.GetAsync<User>(key);
            if (cachedUser != null)
            {
                var mapped = _mapper.Map<GetUserResponse>(cachedUser);
                return ApiResponse.Success(mapped);
            }

            // 2️⃣ DB fallback
            var repo = _unitOfWork.Repository<User>();
            var user = await repo.GetByIdAsync(request.Id, cancellationToken);

            if (user == null)
                return ApiResponse.Failure($"User {request.Id} not found");

            // Store in cache
            await _cache.SetAsync(key, user, TimeSpan.FromHours(1));

            var mappedUser = _mapper.Map<GetUserResponse>(user);
            return ApiResponse.Success(mappedUser);
        }
    }
}
