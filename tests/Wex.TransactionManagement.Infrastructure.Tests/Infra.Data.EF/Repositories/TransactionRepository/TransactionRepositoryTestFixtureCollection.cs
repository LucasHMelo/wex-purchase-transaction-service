using Wex.TransactionManagement.IntegrationTests.Base;

namespace Wex.TransactionManagement.IntegrationTests.Infra.Data.EF.Repositories.TransactionRepository;

[CollectionDefinition(nameof(TransactionRepositoryTestFixture))]
public class TransactionRepositoryTestFixtureCollection
    : ICollectionFixture<TransactionRepositoryTestFixture>
{}

public class TransactionRepositoryTestFixture
    : BaseFixture
{
}