using System.Text;
using System.Text.Json;

namespace Wex.TransactionManagement.E2ETests.Base;

public class ApiClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<(HttpResponseMessage?, TOutput?)> Post<TOutput>(
        string route,
        object payload
    )
        where TOutput : class
    {
        var response = await _httpClient.PostAsync(
            route,
            new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            )
        );
        var outputString = await response.Content.ReadAsStringAsync();
        TOutput? output = null;
        if (!string.IsNullOrWhiteSpace(outputString))
            output = JsonSerializer.Deserialize<TOutput>(outputString,
                new JsonSerializerOptions { 
                    PropertyNameCaseInsensitive = true
                }
            );
        return (response, output);
    }

    public async Task<(HttpResponseMessage?, JsonElement)> Post(
        string route,
        object payload
    )
    {
        var response = await _httpClient.PostAsync(
            route,
            new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            )
        );
        var outputString = await response.Content.ReadAsStringAsync();
        var output = JsonSerializer.Deserialize<JsonElement>(outputString,
                new JsonSerializerOptions { 
                    PropertyNameCaseInsensitive = true
                }
            );
        return (response, output);
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Get<TOutput>(
        string route
    )
        where TOutput : class
    {
        var response = await _httpClient.GetAsync(route);
        var outputString = await response.Content.ReadAsStringAsync();
        TOutput? output = null;
        if (!string.IsNullOrWhiteSpace(outputString))
            output = JsonSerializer.Deserialize<TOutput>(outputString,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        return (response, output);
    }
}