

namespace Demo_Test.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id must be greater than 0");

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

    }
}
