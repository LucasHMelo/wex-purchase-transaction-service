using NSubstitute;
using Wex.TransactionManager.Application.Interfaces;
using Wex.TransactionManager.Application.UseCases.Transactions.CreateTransactions;
using Wex.TransactionManager.Domain.Repositories;
using UseCases = Wex.TransactionManager.Application.UseCases.Transactions.CreateTransactions;
using DomainEntity = Wex.TransactionManager.Domain.Entities;
using Shouldly;
using Wex.TransactionManager.Domain.Exceptions;

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

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Received(1).Insert(
            Arg.Any<DomainEntity.Transaction>(),
            Arg.Any<CancellationToken>()
        );

        await unitOfWorkMock.Received(1).Commit(
            Arg.Any<CancellationToken>()
        );

        output.ShouldNotBe(default);
    }

    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateAggregate))]
    [Trait("Application", "CreateTransaction - Use Cases")]
    [MemberData(nameof(GetInvalidInputs))]
    public async void ThrowWhenCantInstantiateAggregate(
        CreateTransactionInput input,
        string exceptionMessage
    )
    {
        var useCase = new UseCases.CreateTransaction(
            _fixture.GetRepositoryMock(),
            _fixture.GetUnitOfWorkMock()
        );

        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        var ex = await Should
            .ThrowAsync<EntityValidationException>(task);
        ex.Message.ShouldBe(exceptionMessage);
    }

    public static IEnumerable<object[]> GetInvalidInputs()
    {
        var fixture = new CreateTransactionTestFixture();
        var invalidInputsList = new List<object[]>();

        var invalidInputTooLongDescription = fixture.GetInput();
        var tooLongDescriptionForTransaction = fixture.Faker.Commerce.ProductDescription();
        while (tooLongDescriptionForTransaction.Length <= 50)
            tooLongDescriptionForTransaction = $"{tooLongDescriptionForTransaction} {fixture.Faker.Commerce.ProductDescription()}";
        invalidInputTooLongDescription.Description = tooLongDescriptionForTransaction;
        invalidInputsList.Add(new object[] {
            invalidInputTooLongDescription,
            "Description should be less or equal 50 characters long"
        });
  
        return invalidInputsList;
    }
}
