using Wex.TransactionManager.Application.DTOs;

namespace Wex.TransactionManager.Application.Interfaces;

public interface ITreasuryApiClient
{
    Task<decimal?> GetExchangeRateAsync(string currency, DateTime date, CancellationToken cancellationToken = default);
    Task<List<ExchangeRateDto>> GetExchangeRatesRangeAsync(string currency, DateTime startDate, CancellationToken cancellationToken = default);
}
