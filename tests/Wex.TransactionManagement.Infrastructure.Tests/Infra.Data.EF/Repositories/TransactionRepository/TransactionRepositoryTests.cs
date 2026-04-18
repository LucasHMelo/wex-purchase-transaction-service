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

     [Fact(DisplayName = nameof(Get))]
    [Trait("Integration/Infra.Data", "TransactionRepository - Repositories")]
    public async Task Get()
    {
        WexTransactionDbContext dbContext = _fixture.CreateDbContext();
        var exampleTransaction = _fixture.GetExampleTransaction();
        var exampleTransactionsList = _fixture.GetExampleTransactionsList(15);
        exampleTransactionsList.Add(exampleTransaction);
        await dbContext.AddRangeAsync(exampleTransactionsList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var transactionRepository = new InfraRepository.TransactionRepository(dbContext);

        var dbTransaction = await transactionRepository.Get(
            exampleTransaction.Id, 
            CancellationToken.None);

        dbTransaction.ShouldNotBeNull();
        dbTransaction.Id.ShouldBe(exampleTransaction.Id);
        dbTransaction.Description.ShouldBe(exampleTransaction.Description);
        dbTransaction.Amount.Value.ShouldBe(exampleTransaction.Amount.Value);
        dbTransaction.TransactionDate.ShouldBe(exampleTransaction.TransactionDate);
        dbTransaction.CreatedAt.ShouldBe(exampleTransaction.CreatedAt);
    }

}
