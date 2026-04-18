
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

        var rate = await _treasuryApiClient.GetExchangeRateAsync(currency, requestedDate, cancellationToken);

        return new ExchangeRateResult { Rate = rate.Value, RateDate = requestedDate };
    }
}
