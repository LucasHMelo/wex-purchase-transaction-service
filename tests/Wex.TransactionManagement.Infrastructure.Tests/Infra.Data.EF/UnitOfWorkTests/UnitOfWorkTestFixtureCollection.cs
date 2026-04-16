using Wex.TransactionManagement.IntegrationTests.Base;
using Wex.TransactionManager.Domain.Entities;

namespace Wex.TransactionManagement.IntegrationTests.Infra.Data.EF.UnitOfWorkTests;

[CollectionDefinition(nameof(UnitOfWorkTestFixture))]
public class UnitOfWorkTestFixtureCollection
    : ICollectionFixture<UnitOfWorkTestFixture>
{ }

public class UnitOfWorkTestFixture
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

    public List<Transaction> GetExampleTransactionsList(int length = 10)
        => Enumerable.Range(1, length)
            .Select(_ => GetExampleTransaction()).ToList();

}