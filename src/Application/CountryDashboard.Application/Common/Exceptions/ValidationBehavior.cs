

namespace CountryDashboard.Application.Common.Exceptions;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // If the response type is not ApiResponse, proceed without validation
        if (!typeof(TResponse).IsAssignableFrom(typeof(ApiResponse)))
            return await next();

        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Any())
        {
            var errorMessages = failures.Select(f => f.ErrorMessage).ToList();

            var errorResponse = ApiResponse.Failure(errorMessages, HttpStatusCode.BadRequest);

            return (TResponse)(object)errorResponse;
        }

        return await next();
    }
}
