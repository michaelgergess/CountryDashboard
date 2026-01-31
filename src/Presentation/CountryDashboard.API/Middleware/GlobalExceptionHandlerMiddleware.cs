

namespace CountryDashboard.API.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Call next middleware
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ApiResponse response;

            switch (exception)
            {
                case ValidationException ve:
                    response = ve.ApiResponse; // Use our ValidationException ApiResponse
                    break;
                case KeyNotFoundException knf:
                    response = ApiResponse.Failure(knf.Message, HttpStatusCode.NotFound);
                    break;
                default:
                    response = ApiResponse.Failure("An unexpected error occurred.", HttpStatusCode.InternalServerError);
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;

            var result = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(result);
        }
    }
}
