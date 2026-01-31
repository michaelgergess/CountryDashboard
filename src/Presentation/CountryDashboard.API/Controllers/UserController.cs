

namespace CountryDashboard.API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        [HttpPost("user")]
        public Task<ApiResponse> GetById([FromBody] GetUserQuery query) =>
            Mediator.Send(query);

        [HttpGet("users")]
        public Task<ApiResponse> Users() =>
            Mediator.Send(new GetUsersQuery());

        [HttpPost("createUser")]
        public Task<ApiResponse> Create([FromBody] AddUserCommand command) =>
            Mediator.Send(command);

        [HttpPost("updateUser")]
        public Task<ApiResponse> Update([FromBody] UpdateUserCommand command) =>
            Mediator.Send(command);

        [HttpPost("toggleDeleteUser")]
        public async Task<ApiResponse> ToggleDelete([FromBody] ToggleSoftDeleteCommand<User> command)
        {
            var result = await Mediator.Send(command);
            if (result)
                return ApiResponse.Success($"User with id {command.Id} soft delete toggled successfully");
            else
                return ApiResponse.Failure($"Failed to toggle soft delete for user with id {command.Id}");
        }
    }
}
