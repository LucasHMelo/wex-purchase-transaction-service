using UseCases = Wex.TransactionManager.Application.UseCases.Transactions.CreateTransactions;

namespace Wex.TransactionManagement.Application.Tests.UnitTests.CreateTransaction;

public class CreateTransactionTests
{
    [Fact(DisplayName = nameof(CreateTransaction))]
    [Trait("Application", "CreateTransaction - Use Cases")]
    public async void CreateTransaction()
    {
        var repositoryMock = new Mock<ITransaCreateTransactionRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var useCase = new UseCases.CreateTransaction(
            repositoryMock.Object, 
            unitOfWorkMock.Object
        );
        var input = new CreateTransactionInput(
            "Transaction Name",
            "Transaction Description",
            true
        );

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Create(
                It.IsAny<Transaction>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
        unitOfWorkMock.Verify(
            uow => uow.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );
        output.ShouldNotBeNull();
        output.Name.Should().Be("Transaction Name");
        output.Description.Should().Be("Transaction Description");
        output.IsActive.Should().Be(true);
        (output.Id != null && output.Id != Guid.Empty).Should().BeTrue();
        (output.CreatedAt != null && output.CreatedAt != default(DateTime)).Should().BeTrue();
    }

}
