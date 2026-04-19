using Wex.TransactionManager.Domain.Entities;

namespace Wex.TransactionManager.Domain.Repositories;

public interface IExchangeRateRepository
{
    Task<ExchangeRate?> Get(Guid id, CancellationToken cancellationToken);
}
