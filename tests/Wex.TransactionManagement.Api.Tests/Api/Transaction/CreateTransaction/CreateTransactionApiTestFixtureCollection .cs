using System;
using Wex.TransactionManagement.E2ETests.Api.Transaction.Common;
using Wex.TransactionManager.Application.UseCases.Transactions.CreateTransactions;

namespace Wex.TransactionManagement.E2ETests.Api.Transaction.CreateTransaction;

[CollectionDefinition(nameof(CreateTransactionApiTestFixture))]
public class CreateTransactionApiTestFixtureCollection
    : ICollectionFixture<CreateTransactionApiTestFixture>
{ }

public class CreateTransactionApiTestFixture
    : TransactionBaseFixture
{
    public CreateTransactionInput getExampleInput()
        => new CreateTransactionInput
        {
            Amount = GetValidTransactionAmount(),
            Description = GetValidTransactionDescription(),
            TransactionDate = GetValidTransactionDate()
        };

}