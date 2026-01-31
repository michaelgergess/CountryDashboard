namespace CountryDashboard.Application.Common.Interfaces;

public interface IExternalHttpService
{
    Task<string> GetAsync(string url, string token = null);
    Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data, string token = null);
    Task<HttpPostResult<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest data, string token = null, Dictionary<string, string> headers = null);
}
