namespace Demo_Test.Application.Features.Users.Commands.AddUser
{
    public class AddUserValidator : AbstractValidator<AddUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public AddUserValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("FirstName is required")
                .MaximumLength(40).WithMessage("FirstName must not exceed 40 characters")
                .Matches("^[A-Za-z]+$").WithMessage("FirstName must contain only English letters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("LastName is required")
                .MaximumLength(40).WithMessage("LastName must not exceed 40 characters")
                .Matches("^[A-Za-z]+$").WithMessage("LastName must contain only English letters");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(80).WithMessage("Name must not exceed 80 characters")
                .Matches("^[A-Za-z]+$").WithMessage("Name must contain only English letters");



            RuleFor(x => x.IsDeleted)
                .NotNull().WithMessage("IsDeleted is required");
        }

        //private async Task ValidateUserAsync(AddUserCommand command, ValidationContext<AddUserCommand> context, CancellationToken cancellationToken)
        //{
        //    var result = await _userRepository.ValidateUser(command.Name, command.RoleId, cancellationToken);

        //    if (result == null)
        //    {
        //        context.AddFailure("Validation", "Unexpected error validating user");
        //        return;
        //    }

        //    if (!result.RoleExists)
        //    {
        //        context.AddFailure(nameof(command.RoleId), "Role does not exist");
        //    }

        //    if (result.NameExists)
        //    {
        //        context.AddFailure(nameof(command.Name), $"User name '{command.Name}' already exists");
        //    }
        //}
    }
}
