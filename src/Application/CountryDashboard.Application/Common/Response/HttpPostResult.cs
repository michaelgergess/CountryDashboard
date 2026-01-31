namespace CountryDashboard.Application.Common.Response;

public record HttpPostResult<TResponse>(TResponse? Data,
                                             HttpStatusCode StatusCode,
                                             bool IsSucceded,
                                             string? RawResponse = null);
