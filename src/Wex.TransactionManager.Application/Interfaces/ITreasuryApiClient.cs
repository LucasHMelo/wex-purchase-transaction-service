namespace Wex.TransactionManager.Application.Interfaces;

public interface ITreasuryApiClient
{
    Task<decimal?> GetExchangeRateAsync(string currency, DateTime date, CancellationToken cancellationToken = default);
}
