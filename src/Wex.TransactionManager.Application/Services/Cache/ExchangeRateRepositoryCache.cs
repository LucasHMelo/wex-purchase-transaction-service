using Wex.TransactionManager.Domain.Entities;
using ZiggyCreatures.Caching.Fusion;

namespace Wex.TransactionManager.Application.Services.Cache;

public class ExchangeRateRepositoryCache
{
    private readonly IFusionCache _cache;

    public ExchangeRateRepositoryCache(IFusionCache cache)
    {
        _cache = cache;
    }

    public override async Task<ExchangeRate?> Get(Guid id, CancellationToken cancellationToken)
    {
        return await _cache.GetOrSetAsync(
            nameof(ExchangeRate),
            async ctx =>
            {
                return await base.Get(id, cancellationToken);
            }
        );
    }
}
