

namespace CountryDashboard.API.Controllers.Common
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    [RequiredHeaders(
    HeaderProperties.CountryID
    )]
    [HeaderClaimsValidation]

    public class BaseController : ControllerBase
    {
        public string ModuleName { get; internal set; }

        protected IMediator Mediator => HttpContext.RequestServices.GetRequiredService<IMediator>() ??
            throw new InvalidOperationException("Mediator service is not registered.");
    }
}
