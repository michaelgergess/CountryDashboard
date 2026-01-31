namespace CountryDashboard.Application.Common.Response
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; }
        public object? Value { get; set; } = null;
        public string FirstError { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public string RequestIP { get; set; }
        public string RequestOrigin { get; set; }

        public ApiResponse() { }

        public ApiResponse(bool success, object? value, List<string> errors, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            IsSuccess = success;
            Value = value;
            Errors = errors ?? new List<string>();
            FirstError = Errors.FirstOrDefault() ?? string.Empty;
            StatusCode = statusCode;
        }

        // Success response
        public static ApiResponse Success(object? value, HttpStatusCode statusCode = HttpStatusCode.OK)
            => new ApiResponse(true, value, new List<string>(), statusCode);

        // Failure with single error message
        public static ApiResponse Failure(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => new ApiResponse(false, null, new List<string> { errorMessage }, statusCode);

        // Failure with multiple error messages
        public static ApiResponse Failure(List<string> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => new ApiResponse(false, null, errors ?? new List<string>(), statusCode);

    }


}
