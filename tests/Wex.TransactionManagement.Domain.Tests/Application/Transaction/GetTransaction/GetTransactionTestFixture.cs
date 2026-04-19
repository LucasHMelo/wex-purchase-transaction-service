using System;
using NSubstitute;
using Wex.TransactionManagement.UnitTests.Common;
using Wex.TransactionManager.Application.Services;
using Wex.TransactionManager.Application.UseCases.Transactions.GetTransactions;
using Wex.TransactionManager.Domain.Repositories;
using Microsoft.Extensions.Logging;
using DomainEntity = Wex.TransactionManager.Domain.Entities;
using UseCase = Wex.TransactionManager.Application.UseCases.Transactions.GetTransactions;



namespace Wex.TransactionManagement.UnitTests.Application.Transaction.GetTransaction;

[CollectionDefinition(nameof(GetTransactionTestFixture))]
public class GetTransactionTestFixtureCollection :
    ICollectionFixture<GetTransactionTestFixture>
{ }

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

    public DateTime GetValidRateDate()
    {
        return DateTime.UtcNow;
    }

    public decimal GetValidRate()
    {
        return Faker.Random.Number(9999);
    }

    public string GetValidTargetCurrency()
    {
        return Faker.Finance.Currency().Code;
    }

    public DomainEntity.Transaction GetValidTransaction()
        => new(
            GetValidTransactionDescription(),
            GetValidTransactionAmount(),
            GetValidTransactionDate()
        );

    public GetTransactionInput GetValidTransactionInput()
        => new(
            Guid.NewGuid(),
            GetValidTargetCurrency()
        );

    public ExchangeRateResult GetValidExchangeRateResult()
        => new ExchangeRateResult
        {
            Rate = GetValidRate(),
            RateDate = GetValidRateDate()
        };



    public ITransactionRepository GetRepositoryMock()
        => Substitute.For<ITransactionRepository>();

    public IExchangeRateService GetRateExchangeServiceMock()
        => Substitute.For<IExchangeRateService>();

    public ILogger<UseCase.GetTransaction> GetLoggerMock()
        => Substitute.For<ILogger<UseCase.GetTransaction>>();
}
