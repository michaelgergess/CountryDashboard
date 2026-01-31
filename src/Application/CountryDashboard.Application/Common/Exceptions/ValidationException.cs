


namespace CountryDashboard.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public ApiResponse ApiResponse { get; private set; }

    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        ApiResponse = ApiResponse.Failure("One or more validation failures have occurred.");
        ApiResponse.StatusCode = HttpStatusCode.BadRequest;
    }

    public ValidationException(ValidationResult validationResult)
        : base("One or more validation failures have occurred.")
    {
        if (validationResult == null)
            throw new ArgumentNullException(nameof(validationResult));

        var errors = validationResult.Errors
            .Where(f => f != null)
            .Select(f => f.ErrorMessage)
            .ToList();

        ApiResponse = ApiResponse.Failure(errors);
    }
}

