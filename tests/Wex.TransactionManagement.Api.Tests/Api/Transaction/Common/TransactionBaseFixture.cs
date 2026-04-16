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

    public string GetInvalidDescriptionTooLong()
    {
        var tooLongDescriptionForTransaction = Faker.Commerce.ProductDescription();
        while (tooLongDescriptionForTransaction.Length <= 50)
            tooLongDescriptionForTransaction = $"{tooLongDescriptionForTransaction} {Faker.Commerce.ProductDescription()}";
        return tooLongDescriptionForTransaction;
    }

    public decimal GetInvalidValue()
    {
        return -1;
    }

    public DateTime GetValidTransactionDate()
    {
        return DateTime.UtcNow;
    }

}