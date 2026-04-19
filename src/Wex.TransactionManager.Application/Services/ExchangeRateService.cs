
using Microsoft.Extensions.Logging;
using Wex.TransactionManager.Application.DTOs;
using Wex.TransactionManager.Application.Interfaces;

namespace Wex.TransactionManager.Application.Services;

public class ExchangeRateService(
    ITreasuryApiClient treasuryApiClient,
    ILogger<ExchangeRateService> logger) : IExchangeRateService
{
    private readonly ITreasuryApiClient _treasuryApiClient = treasuryApiClient;
    private readonly ILogger<ExchangeRateService> _logger = logger;

    public async Task<ExchangeRateResult> GetExchangeRateWithDateAsync(string currency, DateTime date, CancellationToken cancellationToken = default)
    {
        var today = DateTime.Today;
        var requestedDate = date.Date;

        if (requestedDate == today)
        {
            _logger.LogInformation("Fetching current day rate for {Currency} on {Date} (no cache)", currency, requestedDate);
            var rate = await _treasuryApiClient.GetExchangeRateAsync(currency, requestedDate, cancellationToken);
            
            if (!rate.HasValue)
            {
                throw new Exception(currency);
            }
            
            return new ExchangeRateResult { Rate = rate.Value, RateDate = requestedDate };
        }

        var sixMonthsAgo = today.AddMonths(-6);
        var startDate = requestedDate < sixMonthsAgo ? requestedDate : sixMonthsAgo;
             _logger.LogInformation("Fetching exchange rates bucket for {Currency} from {StartDate}", currency, startDate);
        var ratesFromApi = await _treasuryApiClient.GetExchangeRatesRangeAsync(currency, startDate, cancellationToken);

        var finalRateResult = FindRateInBucketWithDate(ratesFromApi, requestedDate);
        if (finalRateResult == null)
        {
            throw new Exception(currency);
        }
        _logger.LogInformation("Retrieved and cached exchange rate bucket for {Currency} with {Count} rates", 
            currency, ratesFromApi.Count);
        
        return finalRateResult;
    }

     private ExchangeRateResult? FindRateInBucketWithDate(List<ExchangeRateDto> rates, DateTime requestedDate)
    {
        // Try exact date match first
        var exactMatch = rates.FirstOrDefault(r => r.Date == requestedDate);
        if (exactMatch != null)
        {
            return new ExchangeRateResult { Rate = exactMatch.Rate, RateDate = exactMatch.Date };
        }
        
        // Find most recent rate before the requested date
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
}
