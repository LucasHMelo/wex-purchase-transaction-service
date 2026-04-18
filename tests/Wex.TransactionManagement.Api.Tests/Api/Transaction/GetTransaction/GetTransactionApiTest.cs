using System.Net;
using Microsoft.AspNetCore.Http;
using Shouldly;
using Wex.TransactionManager.Application.UseCases.Transactions.GetTransactions;

namespace Wex.TransactionManagement.E2ETests.Api.Transaction.GetTransaction;

[Collection(nameof(GetTransactionApiTestFixture))]
public class GetTransactionApiTest
{
    private readonly GetTransactionApiTestFixture _fixture;

    public GetTransactionApiTest(GetTransactionApiTestFixture fixture) 
        => _fixture = fixture;

    [Fact(DisplayName = nameof(GetTransaction))]
    [Trait("EndToEnd/API", "Transaction/Get - Endpoints")]
    public async Task GetTransaction()
    {
        var exampleTransactionsList = _fixture.GetExampleTransactionsList(20);
        await _fixture.Persistence.InsertList(exampleTransactionsList);
        var exampleTransaction = exampleTransactionsList[10];

        var (response, output) = await _fixture.ApiClient.Get<GetTransactionOutput>(
            $"/api/transaction/{exampleTransaction.Id}"
        );

        response.ShouldNotBeNull();
        response!.StatusCode.ShouldBe((HttpStatusCode) StatusCodes.Status200OK);
        output.ShouldNotBeNull();
        output!.Id.ShouldBe(exampleTransaction.Id);
        output.Description.ShouldBe(exampleTransaction.Description);
        output.Amount.ShouldBe(exampleTransaction.Amount.Value.ToString());
    }
}
