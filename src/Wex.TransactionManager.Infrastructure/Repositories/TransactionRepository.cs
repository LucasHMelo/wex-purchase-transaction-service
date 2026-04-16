using Microsoft.EntityFrameworkCore;
using Wex.TransactionManager.Domain.Entities;
using Wex.TransactionManager.Domain.Repositories;
using Wex.TransactionManager.Infrastructure.Data.DbContexts;

namespace Wex.TransactionManager.Infrastructure.Repositories;

public class TransactionRepository
    : ITransactionRepository
{
    private readonly WexTransactionDbContext _context;
    private DbSet<Transaction> _transactions 
        => _context.Set<Transaction>();

    public TransactionRepository(WexTransactionDbContext context) 
        => _context = context;

    public async Task Insert(
        Transaction aggregate, 
        CancellationToken cancellationToken
    )
        => await _transactions.AddAsync(aggregate, cancellationToken);


   
    public Task<Transaction> Get(Guid id, CancellationToken cancellationToken) => throw new NotImplementedException();
}