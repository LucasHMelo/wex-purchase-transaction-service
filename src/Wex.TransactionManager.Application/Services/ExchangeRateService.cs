
using Microsoft.Extensions.Logging;
using Wex.TransactionManager.Application.DTOs;
using Wex.TransactionManager.Application.Interfaces;
using ZiggyCreatures.Caching.Fusion;

namespace Wex.TransactionManager.Application.Services;

public class ExchangeRateService(
    ITreasuryApiClient treasuryApiClient,
    ILogger<ExchangeRateService> logger,
    IFusionCache cache) : IExchangeRateService
{
    private readonly ITreasuryApiClient _treasuryApiClient = treasuryApiClient;
    private readonly IFusionCache _cache = cache;
    private readonly ILogger<ExchangeRateService> _logger = logger;

    public async Task<ExchangeRateResult> GetExchangeRateWithDateAsync(string currency, DateTime date, CancellationToken cancellationToken = default)
    {
        var today = DateTime.Today;
        var requestedDate = date.Date;
        var cacheKey = $"rate:{currency}:{requestedDate}";
        var options = CreateOptions(date);

        if (requestedDate == today)
        {
            _logger.LogInformation("Fetching current day rate for {Currency} on {Date} (no cache)", currency, requestedDate);
            
            var rate = await _cache.GetOrSetAsync<decimal?>(cacheKey, async _ =>
            {
                return await _treasuryApiClient.GetExchangeRateAsync(currency, requestedDate, cancellationToken);
            }, options);

            if (!rate.HasValue)
            {
                throw new Exception(currency);
            }

            return new ExchangeRateResult { Rate = rate.Value, RateDate = requestedDate };
        }

        var sixMonthsAgo = today.AddMonths(-6);
        var startDate = requestedDate < sixMonthsAgo ? requestedDate : sixMonthsAgo;
        _logger.LogInformation("Fetching exchange rates bucket for {Currency} from {StartDate}", currency, startDate);


        var rates = await _cache.GetOrSetAsync<List<ExchangeRateDto>>(cacheKey, async _ =>
            {
                return await _treasuryApiClient.GetExchangeRatesRangeAsync(currency, startDate, cancellationToken);
            }, options);

        var finalRateResult = FindRateInBucketWithDate(rates, requestedDate);
        if (finalRateResult == null)
        {
            throw new Exception(currency);
        }
        _logger.LogInformation("Retrieved and cached exchange rate bucket for {Currency} with {Count} rates",
            currency, rates.Count);

        return finalRateResult;
    }

    private ExchangeRateResult? FindRateInBucketWithDate(List<ExchangeRateDto> rates, DateTime requestedDate)
    {
        var exactMatch = rates.FirstOrDefault(r => r.Date == requestedDate);
        if (exactMatch != null)
        {
            return new ExchangeRateResult { Rate = exactMatch.Rate, RateDate = exactMatch.Date };
        }

        var priorRate = rates
            .Where(r => r.Date < requestedDate)
            .OrderByDescending(r => r.Date)
            .FirstOrDefault();

        if (priorRate != null)
        {
            return new ExchangeRateResult { Rate = priorRate.Rate, RateDate = priorRate.Date };
        }

        return null;
    }

    private static FusionCacheEntryOptions CreateOptions(DateTime date)
{
    var now = DateTime.UtcNow;

    if (date.Date == now.Date)
    {
        return new FusionCacheEntryOptions
        {
            Duration = TimeSpan.FromHours(1),

            IsFailSafeEnabled = true,
            FailSafeMaxDuration = TimeSpan.FromDays(30 * 6),

            EagerRefreshThreshold = 0.7f
        };
    }

    if (date >= now.AddDays(-30))
    {
        return new FusionCacheEntryOptions
        {
            Duration = TimeSpan.FromDays(30),

            IsFailSafeEnabled = true,
            FailSafeMaxDuration = TimeSpan.FromDays(180)
        };
    }

    return new FusionCacheEntryOptions
    {
        Duration = TimeSpan.MaxValue 
    };
}
}
