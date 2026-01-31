
using CountryDashboard.Application.Common.Interfaces.Services;

namespace Texas.API.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequiredHeadersAttribute : Attribute, IActionFilter
    {
        private readonly HeaderProperties[] _headersToValidate;

        public RequiredHeadersAttribute(params HeaderProperties[] requiredHeaders)
        {
            _headersToValidate = requiredHeaders?.Length > 0
                ? requiredHeaders
                : Enum.GetValues(typeof(HeaderProperties)).Cast<HeaderProperties>().ToArray();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var endpoint = context.HttpContext.GetEndpoint();
            var ignoreAttr = endpoint?.Metadata.GetMetadata<IgnoreRequiredHeadersAttribute>();

            bool ShouldSkip(HeaderProperties header)
            {
                if (ignoreAttr == null) return false;
                if (ignoreAttr.IgnoredHeaders == null || ignoreAttr.IgnoredHeaders.Length == 0) return true;
                return ignoreAttr.IgnoredHeaders.Contains(header);
            }

            var headersToCheck = _headersToValidate.Where(h => !ShouldSkip(h)).ToArray();
            var headerService = context.HttpContext.RequestServices.GetService<IRequestHeaderService>();

            if (headerService == null)
            {
                context.Result = new JsonResult(ApiResponse.Failure("Header service is missing."))
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
                return;
            }

            var errors = new List<string>();

            foreach (var header in headersToCheck)
            {
                switch (header)
                {
                    case HeaderProperties.CountryID:
                        if (headerService.CountryId <= 0)
                        {
                            errors.Add("Header 'CountryID' is missing or invalid.");
                        }
                        break;

                        // Extend for other headers if needed
                }
            }

            if (errors.Any())
            {
                context.Result = new JsonResult(ApiResponse.Failure(errors))
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No-op
        }
    }
}
