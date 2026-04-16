using Microsoft.EntityFrameworkCore;
using Wex.TransactionManager.Infrastructure.Data.DbContexts;
using DomainEntity = Wex.TransactionManager.Domain.Entities;

namespace Wex.TransactionManagement.E2ETests.Api.Transaction.Common;

public class TransactionPersistence(WexTransactionDbContext context)
{
    private readonly WexTransactionDbContext _context = context;

    public async Task<DomainEntity.Transaction?> GetById(string id)
        => await _context
            .Transactions.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
}
