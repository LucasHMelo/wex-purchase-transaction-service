using Wex.TransactionManagement.E2ETests.Api.Transaction.Common;

namespace Wex.TransactionManagement.E2ETests.Api.Transaction.GetTransaction;

[CollectionDefinition(nameof(GetTransactionApiTestFixture))]
public class GetTransactionApiTestFixtureCollection
    : ICollectionFixture<GetTransactionApiTestFixture>
{ }

public class GetTransactionApiTestFixture
    : TransactionBaseFixture
{ }