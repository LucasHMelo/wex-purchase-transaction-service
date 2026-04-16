using Wex.TransactionManagement.UnitTests.Common;
using DomainEntity = Wex.TransactionManager.Domain.Entities;

namespace Wex.TransactionManagement.UnitTests.Domain.Transaction;

public class TransactionTestFixture : BaseFixture
{
    public TransactionTestFixture()
        : base() { }

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

    public TransactionManager.Domain.Entities.Transaction GetValidTransaction()
        => new (
            GetValidTransactionDescription(),
            GetValidTransactionAmount(),
            GetValidTransactionDate()
        );

}

[CollectionDefinition(nameof(TransactionTestFixture))]
public class TransactionTestFixtureCollection
    : ICollectionFixture<TransactionTestFixture>
{ }
