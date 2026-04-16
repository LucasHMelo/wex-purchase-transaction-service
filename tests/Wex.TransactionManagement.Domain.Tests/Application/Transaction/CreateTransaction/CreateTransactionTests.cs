using NSubstitute;
using Wex.TransactionManager.Application.Interfaces;
using Wex.TransactionManager.Application.UseCases.Transactions.CreateTransactions;
using Wex.TransactionManager.Domain.Repositories;
using UseCases = Wex.TransactionManager.Application.UseCases.Transactions.CreateTransactions;
using DomainEntity = Wex.TransactionManager.Domain.Entities;
using Shouldly;

namespace Wex.TransactionManagement.UnitTests.Application.Transaction.CreateTransaction;

[Collection(nameof(CreateTransactionTestFixture))]
public class CreateTransactionTests(CreateTransactionTestFixture fixture)
{
    private readonly CreateTransactionTestFixture _fixture = fixture;

    [Fact(DisplayName = nameof(CreateTransaction))]
    [Trait("Application", "CreateTransaction - Use Cases")]
    public async void CreateTransaction()
    {
        var repositoryMock =  _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var useCase = new UseCases.CreateTransaction(
            repositoryMock,
            unitOfWorkMock
        );

        var input = new CreateTransactionInput
        {
            Amount = 100,
            Description = "Anything",
            TransactionDate = DateTime.UtcNow
        };

        var output = await useCase.HandleAsync(input, CancellationToken.None);

        repositoryMock.Received(1).Insert(
            Arg.Any<DomainEntity.Transaction>(),
            Arg.Any<CancellationToken>()
        );

        await unitOfWorkMock.Received(1).Commit(
            Arg.Any<CancellationToken>()
        );

        output.ShouldNotBe(default);
    }
}
