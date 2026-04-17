using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Wex.TransactionManager.Infrastructure.Data.DbContexts;

namespace Wex.TransactionManager.Infrastructure.DbContexts;

public class WexTransactionDbContextFactory
    : IDesignTimeDbContextFactory<WexTransactionDbContext>
{
    public WexTransactionDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WexTransactionDbContext>();

        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=transactiondb;Username=postgres;Password=123456");

        return new WexTransactionDbContext(optionsBuilder.Options);
    }
}