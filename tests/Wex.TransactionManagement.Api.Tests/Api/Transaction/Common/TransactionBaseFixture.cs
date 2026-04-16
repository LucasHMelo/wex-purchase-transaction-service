using System;
using Wex.TransactionManagement.E2ETests.Base;

namespace Wex.TransactionManagement.E2ETests.Api.Transaction.Common;

public class TransactionBaseFixture
    : BaseFixture
{
    public TransactionPersistence Persistence;

    public TransactionBaseFixture()
        : base()
    {
        Persistence = new TransactionPersistence(
            CreateDbContext()
        );
    }

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

}