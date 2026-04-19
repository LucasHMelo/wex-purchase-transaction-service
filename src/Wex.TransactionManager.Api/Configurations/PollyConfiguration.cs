using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Wex.TransactionManager.Application.Interfaces;
using Wex.TransactionManager.Infrastructure.Clients;
using Wex.TransactionManager.Infrastructure.Configurations;

namespace Wex.TransactionManager.Api.Configurations;

public static class PollyConfiguration
{
    public static IServiceCollection AddPollyConfiguration(
        this IServiceCollection services, IConfiguration configuration
    )
    {
        var retry = RetryPolicy();
        var circuitBreaker = CircuitBreakerPolicy();

        services.Configure<TreasuryApiSettings>(configuration.GetSection("TreasuryApi"));
        services.AddHttpClient<ITreasuryApiClient, TreasuryApiClient>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<TreasuryApiSettings>>().Value;

            client.BaseAddress = new Uri(settings.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
        })
        .AddPolicyHandler(retry)
        .AddPolicyHandler(circuitBreaker);

        return services;
    }

    private static AsyncRetryPolicy<HttpResponseMessage> RetryPolicy()
    {
        return Policy<HttpResponseMessage>
        .Handle<HttpRequestException>()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    private static AsyncCircuitBreakerPolicy<HttpResponseMessage> CircuitBreakerPolicy()
    {
        return Policy<HttpResponseMessage>
        .Handle<HttpRequestException>()
        .AdvancedCircuitBreakerAsync(0.5, TimeSpan.FromSeconds(30), 5, TimeSpan.FromSeconds(30));
    }

}
