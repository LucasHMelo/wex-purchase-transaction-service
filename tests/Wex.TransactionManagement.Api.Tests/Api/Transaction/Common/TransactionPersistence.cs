using Microsoft.EntityFrameworkCore;
using Wex.TransactionManager.Infrastructure.Data.DbContexts;
using DomainEntity = Wex.TransactionManager.Domain.Entities;

namespace Wex.TransactionManagement.E2ETests.Api.Transaction.Common;

public class CategoryPersistence(WexTransactionDbContext context)
{
    private readonly WexTransactionDbContext _context = context;

    public async Task<DomainEntity.Transaction?> GetById(Guid id)
        => await _context
            .Transactions.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
}