using NSubstitute;
using UseCase = Wex.TransactionManager.Application.UseCases.Transactions.GetCategory;

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
        
        var input = new UseCase.GetTransactionInput(exampleTransaction.Id);
        var useCase = new UseCase.GetTransaction(repositoryMock);

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Received(1).Get(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>()
        );

        output.ShouldNotBeNull();
        output.Description.ShouldBe(exampleTransaction.Description);
        output.Amount.ShouldBe(exampleTransaction.Amount);
        output.Id.ShouldBe(exampleTransaction.Id);
        output.CreatedAt.ShouldBe(exampleTransaction.CreatedAt);
        output.CreatedAt.ShouldBe(exampleTransaction.CreatedAt);
    }

}
