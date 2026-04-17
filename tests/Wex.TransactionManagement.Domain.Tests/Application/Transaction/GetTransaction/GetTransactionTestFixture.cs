using System;
using NSubstitute;
using Wex.TransactionManagement.UnitTests.Common;
using Wex.TransactionManager.Domain.Repositories;
using DomainEntity = Wex.TransactionManager.Domain.Entities;

namespace Wex.TransactionManagement.UnitTests.Application.Transaction.GetTransaction;

[CollectionDefinition(nameof(GetTransactionTestFixture))]
public class GetTransactionTestFixtureCollection :
    ICollectionFixture<GetTransactionTestFixture> {}
    
public class GetTransactionTestFixture : BaseFixture
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

    public DomainEntity.Transaction GetValidTransaction()
        => new(
            GetValidTransactionDescription(),
            GetValidTransactionAmount(),
            GetValidTransactionDate()
        );


    public ITransactionRepository GetRepositoryMock()
        => Substitute.For<ITransactionRepository>();

}
