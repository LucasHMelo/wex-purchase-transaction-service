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
        var exampleTransaction = _fixture.GetExampleCategory();
        var transactionRepository = new TransactionRepository(dbContext);

        await transactionRepository.Insert(exampleTransaction, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var dbTransaction = await dbContext.Categories.Find(exampleTransaction.Id);
        dbTransaction.Should().NotBeNull();
        dbTransaction.Name.Should().Be(exampleTransaction.Name);
        dbTransaction.Description.Should().Be(exampleTransaction.Description);
        dbTransaction.IsActive.Should().Be(exampleTransaction.IsActive);
        dbTransaction.CreatedAt.Should().Be(exampleTransaction.CreatedAt);
    }

}
