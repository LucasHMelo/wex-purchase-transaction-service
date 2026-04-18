using Microsoft.EntityFrameworkCore;
using Wex.TransactionManagement.IntegrationTests.Base;
using Wex.TransactionManager.Domain.Entities;
using Wex.TransactionManager.Infrastructure.Data.DbContexts;

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

    public List<Transaction> GetExampleTransactionsList(int length = 10)
        => Enumerable.Range(1, length)
            .Select(_ => GetExampleTransaction()).ToList();

    public Transaction GetExampleTransaction()
        => new Transaction(GetValidTransactionDescription(), GetValidTransactionAmount(), GetValidTransactionDate());

}