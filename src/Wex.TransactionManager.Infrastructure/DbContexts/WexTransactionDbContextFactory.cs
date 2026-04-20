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

        var connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__TransactionDb");

        optionsBuilder.UseNpgsql(connectionString);

        return new WexTransactionDbContext(optionsBuilder.Options);
    }
}