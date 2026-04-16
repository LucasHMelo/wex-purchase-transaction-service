using Bogus;
using Microsoft.EntityFrameworkCore;
using Wex.TransactionManager.Infrastructure.Data.DbContexts;

namespace Wex.TransactionManagement.IntegrationTests.Base;

public class BaseFixture
{
    public BaseFixture() 
        => Faker = new Faker();

    protected Faker Faker { get; set; }

    public WexTransactionDbContext CreateDbContext(bool preserveData = false)
    {
        var context =new WexTransactionDbContext(
            new DbContextOptionsBuilder<WexTransactionDbContext>()
            .UseInMemoryDatabase("integration-tests-db")
            .Options
        );
        if (preserveData == false)
            context.Database.EnsureDeleted();
        return context;
    }
}