using Microsoft.EntityFrameworkCore;
using Wex.TransactionManager.Domain.Entities;
using Wex.TransactionManager.Domain.Repositories;
using Wex.TransactionManager.Infrastructure.Data.DbContexts;
using ZiggyCreatures.Caching.Fusion;

namespace Wex.TransactionManager.Infrastructure.Repositories;

public class ExchangeRateRepository : IExchangeRateRepository
{
    private readonly WexTransactionDbContext _context;
    private readonly IFusionCache _cache;

    private DbSet<ExchangeRate> _exchangeRates
        => _context.Set<ExchangeRate>();

    public ExchangeRateRepository(WexTransactionDbContext context, IFusionCache cache)
    {
        _context = context;
        _cache = cache;
    } 

    public async Task Insert(
        ExchangeRate exchangeRate, 
        CancellationToken cancellationToken
    )
        => await _exchangeRates.AddAsync(exchangeRate, cancellationToken);

    public virtual async Task<ExchangeRate?> Get(Guid id, CancellationToken cancellationToken) 
        => await _exchangeRates.FirstOrDefaultAsync( 
            x => x.Id == id, 
            cancellationToken
        );

}
