using NSubstitute;
using Shouldly;
using UseCase = Wex.TransactionManager.Application.UseCases.Transactions.GetTransactions;
using DomainEntity = Wex.TransactionManager.Domain.Entities;
using Wex.TransactionManager.Application.Exceptions;

namespace Wex.TransactionManagement.UnitTests.Application.Transaction.GetTransaction;

[Collection(nameof(GetTransactionTestFixture))]
public class GetTransactionTests(GetTransactionTestFixture fixture)
{
    private readonly GetTransactionTestFixture _fixture = fixture;

    [Fact(DisplayName = nameof(GetTransaction))]
    [Trait("Application", "GetTransaction - Use Cases")]
    public async Task GetTransaction()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleTransaction = _fixture.GetValidTransaction();

        repositoryMock.Get(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>()
        ).Returns(exampleTransaction);

        var input = new UseCase.GetTransactionInput(exampleTransaction.Id);
        var useCase = new UseCase.GetTransaction(repositoryMock);

        var output = await useCase.Handle(input, CancellationToken.None);

        await repositoryMock.Received(1).Get(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>()
        );

        output.ShouldNotBeNull();
        output.Description.ShouldBe(exampleTransaction.Description);
        output.Amount.ShouldBeEquivalentTo(exampleTransaction.Amount.Value.ToString());
        output.Id.ShouldBe(exampleTransaction.Id);
        output.CreatedAt.ShouldBe(exampleTransaction.CreatedAt.ToString());
        output.TransactionDate.ShouldBe(exampleTransaction.TransactionDate.ToString());
    }

    [Fact(DisplayName = nameof(NotFoundExceptionWhenTransactionDoesntExist))]
    [Trait("Application", "GetTransaction - Use Cases")]
    public async Task NotFoundExceptionWhenTransactionDoesntExist()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleGuid = Guid.NewGuid();

        repositoryMock.Get(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>()
        ).Returns(_ => Task.FromException<DomainEntity.Transaction>(
            new NotFoundException($"Transaction '{exampleGuid}' not found")
        ));

        var input = new UseCase.GetTransactionInput(exampleGuid);
        var useCase = new UseCase.GetTransaction(repositoryMock);

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await Should.ThrowAsync<NotFoundException>(task);
        await repositoryMock.Received(1).Get(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>()
        );
    }

}
