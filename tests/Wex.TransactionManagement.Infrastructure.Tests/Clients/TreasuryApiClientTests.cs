using System.Net;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Wex.TransactionManager.Infrastructure.Clients;

namespace Wex.TransactionManagement.IntegrationTests.Clients;

public class TreasuryApiClientTests
{
    private readonly HttpMessageHandler _httpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly ILogger<TreasuryApiClient> _logger;
    private readonly TreasuryApiClient _treasuryApiClient;

    public TreasuryApiClientTests()
    {
        _httpMessageHandler = Substitute.ForPartsOf<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandler);
        _logger = Substitute.For<ILogger<TreasuryApiClient>>();

        _treasuryApiClient = new TreasuryApiClient(_httpClient, _logger);
    }

    private TreasuryApiClient CreateClient(
    Func<HttpRequestMessage, HttpResponseMessage> handler,
    out List<HttpRequestMessage> capturedRequests)
    {
        var requests = new List<HttpRequestMessage>();
        capturedRequests = requests;

        var fakeHandler = new FakeHttpMessageHandler(req =>
        {
            requests.Add(req);
            return handler(req);
        });

        var httpClient = new HttpClient(fakeHandler);

        var logger = Substitute.For<ILogger<TreasuryApiClient>>();

        return new TreasuryApiClient(httpClient, logger);
    }

    [Fact]
    public async Task GetExchangeRateAsync_WithValidResponse_ShouldReturnRate()
    {
        var client = CreateClient((req) =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(@"{
                ""data"": [
                    {
                        ""country_currency_desc"": ""BRAZIL-REAL"",
                        ""exchange_rate"": ""5.25"",
                        ""record_date"": ""2023-12-15""
                    }
                ]
            }")
            },
            out var requests);

        var result = await client.GetExchangeRateAsync("BRL", new DateTime(2023, 12, 15));

        Assert.Equal(5.25m, result);

        var request = requests.Single();

        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Contains("rates_of_exchange", request.RequestUri!.ToString());
    }

    [Fact]
    public async Task GetExchangeRateAsync_WithEmptyData_ShouldReturnNull()
    {
        var client = CreateClient((req) =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(@"{ ""data"": [] }")
            },
            out _);

        var result = await client.GetExchangeRateAsync("BRL", new DateTime(2023, 12, 15));

        Assert.Null(result);
    }


    [Fact]
    public async Task GetExchangeRateAsync_WithHttpError_ShouldReturnNull()
    {
        var client = CreateClient((req) =>
            new HttpResponseMessage(HttpStatusCode.InternalServerError),
            out _);

        var result = await client.GetExchangeRateAsync("BRL", new DateTime(2023, 12, 15));

        Assert.Null(result);
    }

    [Fact]
    public async Task GetExchangeRateAsync_WithTimeout_ShouldReturnNull()
    {
        var client = CreateClient((req) =>
            new HttpResponseMessage(HttpStatusCode.RequestTimeout),
            out _);

        var result = await client.GetExchangeRateAsync("BRL", new DateTime(2023, 12, 15));

        Assert.Null(result);
    }

    [Fact]
    public async Task GetExchangeRateAsync_WithException_ShouldReturnNull()
    {
        var handler = new FakeHttpMessageHandler((req) =>
            throw new HttpRequestException("Network error"));

        var client = new TreasuryApiClient(new HttpClient(handler),
            Substitute.For<ILogger<TreasuryApiClient>>());

        var result = await client.GetExchangeRateAsync("BRL", new DateTime(2023, 12, 15));

        Assert.Null(result);
    }

    [Fact]
    public async Task GetExchangeRateAsync_WithInvalidJson_ShouldReturnNull()
    {
        var client = CreateClient((req) =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{ invalid json }")
            },
            out _);

        var result = await client.GetExchangeRateAsync("BRL", new DateTime(2023, 12, 15));

        Assert.Null(result);
    }

    [Fact]
    public async Task GetExchangeRateAsync_WithMultipleRecords_ShouldReturnFirst()
    {
        var client = CreateClient((req) =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(@"{
                ""data"": [
                    { ""exchange_rate"": ""5.25"" },
                    { ""exchange_rate"": ""5.20"" }
                ]
            }")
            },
            out _);

        var result = await client.GetExchangeRateAsync("BRL", new DateTime(2023, 12, 15));

        Assert.Equal(5.25m, result);
    }

    [Fact]
    public async Task GetExchangeRateAsync_ShouldBuildCorrectUrl()
    {
        var client = CreateClient((req) =>
        {
            Assert.Equal(HttpMethod.Get, req.Method);
            Assert.Contains("rates_of_exchange", req.RequestUri!.ToString());
            Assert.Contains("filter=", req.RequestUri!.Query);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(@"{ ""data"": [] }")
            };
        },
        out _);

        await client.GetExchangeRateAsync("BRL", new DateTime(2023, 12, 15));
    }
}