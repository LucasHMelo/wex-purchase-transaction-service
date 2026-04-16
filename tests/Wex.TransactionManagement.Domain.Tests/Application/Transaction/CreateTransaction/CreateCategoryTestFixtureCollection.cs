using NSubstitute;
using Wex.TransactionManagement.UnitTests.Common;
using Wex.TransactionManager.Application.Interfaces;
using Wex.TransactionManager.Application.UseCases.Transactions.CreateTransactions;
using Wex.TransactionManager.Domain.Repositories;

namespace Wex.TransactionManagement.UnitTests.Application.Transaction.CreateTransaction;

[CollectionDefinition(nameof(CreateTransactionTestFixture))]
public class CreateTransactionTestFixtureCollection
    : ICollectionFixture<CreateTransactionTestFixture>
{ }

public class CreateTransactionTestFixture : BaseFixture
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

    public CreateTransactionInput GetInput()
        => new()
        {
            Amount = GetValidTransactionAmount(),
            Description = GetValidTransactionDescription(),
            TransactionDate = GetValidTransactionDate()
        };


    public ITransactionRepository GetRepositoryMock()
        => Substitute.For<ITransactionRepository>();

    public IUnitOfWork GetUnitOfWorkMock()
            => Substitute.For<IUnitOfWork>();

}