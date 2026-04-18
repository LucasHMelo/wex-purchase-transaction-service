
using Wex.TransactionManager.Application.DTOs;
using Wex.TransactionManager.Application.Interfaces;

namespace Wex.TransactionManager.Application.Services;

public class ExchangeRateService : IExchangeRateService
{
    private readonly ITreasuryApiClient _treasuryApiClient;

    public ExchangeRateService(
        ITreasuryApiClient treasuryApiClient)
    {
        _treasuryApiClient = treasuryApiClient;
    }

    public async Task<ExchangeRateResult> GetExchangeRateWithDateAsync(string currency, DateTime date, CancellationToken cancellationToken = default)
    {
        var today = DateTime.Today;
        var requestedDate = date.Date;

        // if (requestedDate == today)
        // {
            
        //     var rate = await _treasuryApiClient.GetExchangeRateAsync(currency, requestedDate, cancellationToken);
            
        //     if (!rate.HasValue)
        //     {
        //         throw new Exception(currency);
        //     }
            
        //     return new ExchangeRateResult { Rate = rate.Value, RateDate = requestedDate };
        // }

        var sixMonthsAgo = today.AddMonths(-6);
        var startDate = requestedDate < sixMonthsAgo ? requestedDate : sixMonthsAgo;

        var ratesFromApi = await _treasuryApiClient.GetExchangeRatesRangeAsync(currency, startDate, cancellationToken);

        var finalRateResult = FindRateInBucketWithDate(ratesFromApi, requestedDate);
        if (finalRateResult == null)
        {
            throw new Exception(currency);
        }
        
        
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
