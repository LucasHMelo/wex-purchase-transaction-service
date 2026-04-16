using Microsoft.EntityFrameworkCore;
using Shouldly;
using UnitOfWorkInfra = Wex.TransactionManager.Infrastructure;

namespace Wex.TransactionManagement.IntegrationTests.Infra.Data.EF.UnitOfWorkTests;

[Collection(nameof(UnitOfWorkTestFixture))]
public class UnitOfWorkTest(UnitOfWorkTestFixture fixture)
{
    private readonly UnitOfWorkTestFixture _fixture = fixture;

    [Fact(DisplayName = nameof(Commit))]
    [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
    public async Task Commit()
    {
        var dbContext = _fixture.CreateDbContext();
        var exampleTransactionsList = _fixture.GetExampleTransactionsList();
        await dbContext.AddRangeAsync(exampleTransactionsList);
        var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);

        await unitOfWork.Commit(CancellationToken.None);

        var assertDbContext = _fixture.CreateDbContext(true);
        var savedTransactions = assertDbContext.Transactions
            .AsNoTracking().ToList();
        savedTransactions.Count.ShouldBeGreaterThanOrEqualTo(exampleTransactionsList.Count);
    }


    [Fact(DisplayName = nameof(Rollback))]
    [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
    public async Task Rollback()
    {
        var dbContext = _fixture.CreateDbContext(true);
        var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);

        var task = async () 
            => await unitOfWork.Rollback(CancellationToken.None);

        await Should.NotThrowAsync(task);
    }
}
