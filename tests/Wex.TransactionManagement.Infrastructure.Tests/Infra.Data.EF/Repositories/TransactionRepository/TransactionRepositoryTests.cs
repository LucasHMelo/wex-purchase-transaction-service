using Shouldly;
using Wex.TransactionManager.Infrastructure.Data.DbContexts;
using InfraRepository = Wex.TransactionManager.Infrastructure.Repositories;

namespace Wex.TransactionManagement.IntegrationTests.Infra.Data.EF.Repositories.TransactionRepository;

[Collection(nameof(TransactionRepositoryTestFixture))]
public class TransactionRepositoryTests(TransactionRepositoryTestFixture fixture)
{
    private readonly TransactionRepositoryTestFixture _fixture = fixture;

    [Fact(DisplayName = nameof(Insert))]
    [Trait("Integration/Infra.Data", "TransactionRepository - Repositories")]
    public async Task Insert()
    {
        WexTransactionDbContext dbContext = _fixture.CreateDbContext();
        var exampleTransaction = _fixture.GetExampleTransaction();
        var transactionRepository = new InfraRepository.TransactionRepository(dbContext);

        await transactionRepository.Insert(exampleTransaction, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var dbTransaction = await dbContext.Transactions.FindAsync(exampleTransaction.Id);

        dbTransaction.ShouldNotBeNull();
        dbTransaction.Amount.ShouldBe(exampleTransaction.Amount);
        dbTransaction.Description.ShouldBe(exampleTransaction.Description);
        dbTransaction.CreatedAt.ShouldBe(exampleTransaction.CreatedAt);
    }

}
