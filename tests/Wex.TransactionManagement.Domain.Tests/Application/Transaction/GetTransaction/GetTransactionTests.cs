using NSubstitute;
using Shouldly;
using UseCase = Wex.TransactionManager.Application.UseCases.Transactions.GetTransactions;

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

        repositoryMock.Received(1).Get(
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

    //  [Fact(DisplayName = nameof(NotFoundExceptionWhenCategoryDoesntExist))]
    // [Trait("Application", "GetCategory - Use Cases")]
    // public async Task NotFoundExceptionWhenCategoryDoesntExist()
    // {
    //     var repositoryMock = _fixture.GetRepositoryMock();
    //     var exampleGuid = Guid.NewGuid();
    //     repositoryMock.Setup(x => x.Get(
    //         It.IsAny<Guid>(),
    //         It.IsAny<CancellationToken>()
    //     )).ThrowsAsync(
    //         new NotFoundException($"Category '{exampleGuid}' not found")
    //     );
    //     var input = new UseCase.GetCategoryInput(exampleGuid);
    //     var useCase = new UseCase.GetCategory(repositoryMock.Object);

    //     var task = async () 
    //         => await useCase.Handle(input, CancellationToken.None);

    //     await task.Should.ThrowAsync<NotFoundException>();
    //     repositoryMock.Verify(x => x.Get(
    //         It.IsAny<Guid>(),
    //         It.IsAny<CancellationToken>()
    //     ), Times.Once);
    // }

}
