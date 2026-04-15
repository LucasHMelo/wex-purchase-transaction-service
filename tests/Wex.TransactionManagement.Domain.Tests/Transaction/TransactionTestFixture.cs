using Wex.TransactionManagement.Domain.Tests.Common;
using DomainEntity = Wex.TransactionManager.Domain.Transaction;

namespace Wex.TransactionManagement.Domain.Tests.Transaction;

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
        var categoryDescription =
            Faker.Commerce.ProductDescription();
        if (categoryDescription.Length > 50)
            categoryDescription =
                categoryDescription[..50];
        return categoryDescription;
    }

    public DomainEntity.Transaction GetValidTransaction()
        => new (
            GetValidTransactionDescription(),
            GetValidTransactionAmount()
        );

}

[CollectionDefinition(nameof(TransactionTestFixture))]
public class CategoryTestFixtureCollection
    : ICollectionFixture<TransactionTestFixture>
{ }
