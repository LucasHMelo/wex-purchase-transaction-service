using NSubstitute;
using Shouldly;
using UseCase = Wex.TransactionManager.Application.UseCases.Transactions.GetTransactions;
using DomainEntity = Wex.TransactionManager.Domain.Entities;
using Wex.TransactionManager.Application.Exceptions;
using Microsoft.Extensions.Logging;

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
        var rateExchangeMock = _fixture.GetRateExchangeServiceMock();
        var loggerMock = _fixture.GetLoggerMock();
        var exampleTransactionInput = _fixture.GetValidTransactionInput();
        var exampleRate = _fixture.GetValidExchangeRateResult();

        repositoryMock.Get(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>()
        ).Returns(exampleTransaction);

        rateExchangeMock.GetExchangeRateWithDateAsync(
            Arg.Any<string>(),
            Arg.Any<DateTime>(),
            Arg.Any<CancellationToken>()
        ).Returns(exampleRate);

        var input = new UseCase.GetTransactionInput(exampleTransactionInput.Id, exampleTransactionInput.TargetCurrency);
        var useCase = new UseCase.GetTransaction(repositoryMock, rateExchangeMock, loggerMock);

        var output = await useCase.Handle(input, CancellationToken.None);

        await repositoryMock.Received(1).Get(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>()
        );

        output.ShouldNotBeNull();
        output.Description.ShouldBe(exampleTransaction.Description);
        output.OriginalAmount.Value.ShouldBeEquivalentTo(exampleTransaction.Amount.Value);
        output.Id.ShouldBe(exampleTransaction.Id);
        output.CreatedAt.ShouldBeEquivalentTo(exampleTransaction.CreatedAt);
        output.TransactionDate.ShouldBeEquivalentTo(exampleTransaction.TransactionDate);
    }

    [Fact(DisplayName = nameof(NotFoundExceptionWhenTransactionDoesntExist))]
    [Trait("Application", "GetTransaction - Use Cases")]
    public async Task NotFoundExceptionWhenTransactionDoesntExist()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var loggerMock = _fixture.GetLoggerMock();
        var rateExchangeMock = _fixture.GetRateExchangeServiceMock();
        var exampleTransactionInput = _fixture.GetValidTransactionInput();
        var exampleRate = _fixture.GetValidExchangeRateResult();

        rateExchangeMock.GetExchangeRateWithDateAsync(
            Arg.Any<string>(),
            Arg.Any<DateTime>(),
            Arg.Any<CancellationToken>()
        ).Returns(exampleRate);

        repositoryMock.Get(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>()
        ).Returns(_ => Task.FromException<DomainEntity.Transaction>(
            new NotFoundException($"Transaction '{exampleTransactionInput.Id}' not found")
        ));

        var input = new UseCase.GetTransactionInput(exampleTransactionInput.Id, exampleTransactionInput.TargetCurrency);
        var useCase = new UseCase.GetTransaction(repositoryMock, rateExchangeMock, loggerMock);

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await Should.ThrowAsync<NotFoundException>(task);
        await repositoryMock.Received(1).Get(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>()
        );
    }

}
