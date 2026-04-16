using Wex.TransactionManagement.UnitTests.Common;
using DomainEntity = Wex.TransactionManager.Domain.Transaction;

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

    public DomainEntity.Transaction GetValidTransaction()
        => new (
            GetValidTransactionDescription(),
            GetValidTransactionAmount()
        );

}

[CollectionDefinition(nameof(TransactionTestFixture))]
public class TransactionTestFixtureCollection
    : ICollectionFixture<TransactionTestFixture>
{ }
