using Shouldly;
using DomainEntity = Wex.TransactionManager.Domain.Entities;

namespace Wex.TransactionManagement.E2ETests.Api.Transaction.CreateTransaction;

[Collection(nameof(CreateTransactionApiTestFixture))]
public class CreateTransactionApiTest(CreateTransactionApiTestFixture fixture)
{
    private readonly CreateTransactionApiTestFixture _fixture = fixture;

    [Fact(DisplayName = nameof(CreateTransaction))]
    [Trait("EndToEnd/API", "Transaction - Endpoints")]
    public async Task CreateTransaction()
    {
        var input = _fixture.getExampleInput();

        var (response, output) = await _fixture.ApiClient
            .Post<Guid>(
                "/transactions",
                input
            );

        output.ShouldNotBe(default);;
        DomainEntity.Transaction dbTransaction = await _fixture.Persistence
            .GetById(output);
        dbTransaction.ShouldNotBeNull();
        dbTransaction.Amount.Value.ShouldBeEquivalentTo(input.Amount);
        dbTransaction.Description.ShouldBeEquivalentTo(input.Description);
        dbTransaction.TransactionDate.ShouldBeEquivalentTo(input.TransactionDate);
        dbTransaction.Id.ShouldNotBe(default);
    }

}
