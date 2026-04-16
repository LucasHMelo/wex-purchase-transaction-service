using Microsoft.EntityFrameworkCore;
using Wex.TransactionManagement.IntegrationTests.Base;
using Wex.TransactionManager.Domain.Entities;

namespace Wex.TransactionManagement.IntegrationTests.Infra.Data.EF.Repositories.TransactionRepository;

[CollectionDefinition(nameof(TransactionRepositoryTestFixture))]
public class TransactionRepositoryTestFixtureCollection
    : ICollectionFixture<TransactionRepositoryTestFixture>
{}

public class TransactionRepositoryTestFixture
    : BaseFixture
{
    public decimal GetValidTransactionAmount()
    {
        return Faker.Random.Number(9999);
    }

    public string GetValidTransactionDescription()
    {
        var transactionDescription =
            Faker.Commerce.ProductDescription();
        if (transactionDescription.Length > 50)
            transactionDescription =
                transactionDescription[..50];
        return transactionDescription;
    }

    public DateTime GetValidTransactionDate()
    {
        return DateTime.UtcNow;
    }

    public Transaction GetExampleTransaction()
        => new Transaction(GetValidTransactionDescription(), GetValidTransactionAmount(), GetValidTransactionDate());

    
     public WexTransactionDbContext CreateDbContext()
    {
        var dbContext = new WexTransactionDbContext(
            new DbContextOptionsBuilder<WexTransactionDbContext>()
            .UseInMemoryDatabase("integration-tests-db")
            .Options
        );
    }
}