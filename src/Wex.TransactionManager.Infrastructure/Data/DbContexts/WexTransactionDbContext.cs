
using Microsoft.EntityFrameworkCore;
using Wex.TransactionManager.Domain.Entities;
using Wex.TransactionManager.Infrastructure.Configurations;

namespace Wex.TransactionManager.Infrastructure.Data.DbContexts;

public class WexTransactionDbContext : DbContext
{
    public DbSet<Transaction> Transactions => Set<Transaction>();

    public WexTransactionDbContext(
        DbContextOptions<WexTransactionDbContext> options
    ) : base(options) {}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new TransactionConfiguration());
    }
}
