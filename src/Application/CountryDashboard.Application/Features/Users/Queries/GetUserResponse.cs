namespace Demo_Test.Application.Features.Users.Queries
{
    public class GetUserResponse : IMapFrom<User>
    {
        public int Id { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public bool IsDeleted { get; init; }

        public void Mapping(TypeAdapterConfig config)
        {
            config.NewConfig<User, GetUserResponse>();
            config.NewConfig<GetUserResponse, User>();
        }
    }
}
