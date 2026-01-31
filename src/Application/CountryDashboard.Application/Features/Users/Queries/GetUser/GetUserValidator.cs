
namespace Demo_Test.Application.Features.Users.Queries.GetUser
{
    public class GetUserValidator : AbstractValidator<GetUserQuery>
    {
        public GetUserValidator()
        {
            RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .WithMessage("Id must be greater than 0");
        }
    }
}
