using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CountryDashboard.Application.Common.Interfaces;
using CountryDashboard.Application.Common.Response;

namespace CountryDashboard.Integrations;

/// <summary>
/// A generic HTTP service responsible for sending POST requests to external APIs.
/// It abstracts HTTP logic from the application layer, aligning with Clean Architecture principles.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ExternalHttpService"/> class.
/// </remarks>
/// <param name="httpClient">The injected HttpClient instance used for making HTTP requests.</param>
public class ExternalHttpService(HttpClient httpClient, ILogger<ExternalHttpService> logger) : IExternalHttpService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<ExternalHttpService> _logger = logger;

    /// <summary>
    /// Sends a POST request to the specified URL with the given request data and optionally includes a JWT token.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request payload.</typeparam>
    /// <typeparam name="TResponse">The expected type of the response object.</typeparam>
    /// <param name="url">The target endpoint URL.</param>
    /// <param name="data">The request body to be serialized as JSON.</param>
    /// <param name="token">Optional JWT token for authorization (Bearer token).</param>
    /// <returns>The deserialized response object of type <typeparamref name="TResponse"/>; null if the request fails or deserialization fails.</returns>
    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data, string? token = null)
    {
        // Serialize the request payload to JSON
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // If a token is provided, add it to the Authorization header
        if (!string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        try
        {
            _logger.LogInformation("Sent request {Request} to url {Url}", content, url);

            // Send the HTTP POST request
            var response = await _httpClient.PostAsync(url, content);
            _logger.LogInformation("Received response from {Url} with status code: {StatusCode}", url, response.StatusCode);

            // If the response indicates failure, return default (null for reference types)
            if (!response.IsSuccessStatusCode)
            {
                // You may log the status code or throw an exception if needed
                return default;
            }

            // Read the response content as a string
            var responseContent = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Response content from {Url}: {ResponseContent}", url, responseContent);

            if (string.IsNullOrWhiteSpace(responseContent))
                return default;
            // Attempt to deserialize the JSON response into the expected response type
            return JsonSerializer.Deserialize<TResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while sending POST request to {Url}", url);
            throw ex; // Consider logging the exception or handling it as needed
        }
    }



    public async Task<HttpPostResult<TResponse>> PostAsync<TRequest, TResponse>(string url,
                                                                                TRequest data,
                                                                                string? token = null,
                                                                                Dictionary<string, string>? headers = null)
    {
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Prepare HttpRequestMessage to safely set per-request headers
        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = content
        };

        // Authorization header
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Add custom headers
        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        try
        {
            _logger.LogInformation("Sent request {Request} to {Url}", request, url);

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            _logger.LogWarning("Received response {Response} with content {Content} from {Url} with status code: {StatusCode}", response, responseContent, url, response.StatusCode);

            TResponse? result = default;
            if (response.IsSuccessStatusCode)
            {
                if (string.IsNullOrWhiteSpace(responseContent))
                    return default;

                result = JsonSerializer.Deserialize<TResponse>(responseContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return new HttpPostResult<TResponse>(result, response.StatusCode, response.IsSuccessStatusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while sending POST to {Url}", url);
            throw;
        }
    }
    public async Task<string?> GetAsync(string url, string? token = null)
    {
        // If a token is provided, add it to the Authorization header
        if (!string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        try
        {
            _logger.LogInformation("Sent request to url {Url}", url);

            // Send the HTTP POST request
            var response = await _httpClient.GetAsync(url);
            _logger.LogInformation("Received response from {Url} with response {Response} with status code: {StatusCode}", url, response, response.StatusCode);

            // Read the response content as a string
            var responseContent = await response.Content.ReadAsStringAsync();

            // If the response indicates failure, return default (null for reference types)
            if (!response.IsSuccessStatusCode)
            {
                // You may log the status code or throw an exception if needed                
                _logger.LogCritical("Failed to retrieve data from {Url}. Status code: {StatusCode}", url, response.StatusCode);
                return default;
            }

            _logger.LogInformation("Response content from {Url}: {ResponseContent}", url, responseContent);
            return responseContent;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while sending POST request to {Url}", url);
            throw ex; // Consider logging the exception or handling it as needed
        }
    }
}
