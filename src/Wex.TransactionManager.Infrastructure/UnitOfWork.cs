using System;
using Wex.TransactionManager.Application.Interfaces;
using Wex.TransactionManager.Infrastructure.Data.DbContexts;

namespace Wex.TransactionManager.Infrastructure;

public class UnitOfWork
    : IUnitOfWork
{
    private readonly WexTransactionDbContext _context;

    public UnitOfWork(WexTransactionDbContext context)
        => _context = context;

    public Task Commit(CancellationToken cancellationToken)
        => _context.SaveChangesAsync(cancellationToken);

    public Task Rollback(CancellationToken cancellationToken)
        => Task.CompletedTask;
}