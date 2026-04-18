namespace Wex.TransactionManager.Application.Services;

public interface IExchangeRateService
{
    Task<ExchangeRateResult> GetExchangeRateWithDateAsync(string currency, DateTime date, CancellationToken cancellationToken = default);
}
