using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Wex.TransactionManager.Application.UseCases.Transactions.CreateTransactions;
using DomainEntity = Wex.TransactionManager.Domain.Entities;

namespace Wex.TransactionManagement.E2ETests.Api.Transaction.CreateTransaction;

[Collection(nameof(CreateTransactionApiTestFixture))]
public class CreateTransactionApiTest(CreateTransactionApiTestFixture fixture)
{
    private readonly CreateTransactionApiTestFixture _fixture = fixture;

    [Fact(DisplayName = nameof(CreateTransaction))]
    [Trait("EndToEnd/API", "Transaction/Create - Endpoints")]
    public async Task CreateTransaction()
    {
        var input = _fixture.getExampleInput();

        var (response, output) = await _fixture.ApiClient
            .Post<CreateTransactionOutput>(
                "/api/transaction",
                input
            );

        output.TransactionId.ShouldNotBe(default);
        output.TransactionId.ToString().ShouldNotBe("");
        DomainEntity.Transaction dbTransaction = await _fixture.Persistence
            .GetById(output.TransactionId.ToString());
        dbTransaction.ShouldNotBeNull();
        dbTransaction.Amount.Value.ShouldBeEquivalentTo(input.Amount);
        dbTransaction.Description.ShouldBeEquivalentTo(input.Description);
        dbTransaction.TransactionDate.ShouldBe(input.TransactionDate, TimeSpan.FromMilliseconds(1));
        dbTransaction.Id.ShouldNotBe(default);
    }

    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateAggregate))]
    [Trait("EndToEnd/API", "Transaction/Create - Endpoints")]
    [MemberData(
        nameof(CreateTransactionApiTestDataGenerator.GetInvalidInputs),
        MemberType = typeof(CreateTransactionApiTestDataGenerator)
    )]
    public async Task ThrowWhenCantInstantiateAggregate(
        CreateTransactionInput input,
        string expectedDetail
    ){
        var (response, output) = await _fixture.
            ApiClient.Post<ProblemDetails>(
                "/api/transaction",
                input
            );

        response.ShouldNotBeNull();
        response!.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        output.ShouldNotBeNull();
        output!.Title.ShouldBe("One or more validation errors ocurred");
        output.Status.ShouldBe((int)StatusCodes.Status422UnprocessableEntity);
        output.Status.ShouldBe((int)HttpStatusCode.UnprocessableEntity);
        output.Detail.ShouldBe(expectedDetail);
    }

}
