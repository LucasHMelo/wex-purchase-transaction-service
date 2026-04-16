using Shouldly;
using DomainEntity = Wex.TransactionManager.Domain.Entities;

namespace Wex.TransactionManagement.E2ETests.Api.Transaction.CreateTransaction;

[Collection(nameof(CreateTransactionApiTestFixture))]
public class CreateTransactionApiTest(CreateTransactionApiTestFixture fixture)
{
    private readonly CreateTransactionApiTestFixture _fixture = fixture;

    [Fact(DisplayName = "CreateTransaction")]
    [Trait("EndToEnd/API", "Transaction - Endpoints")]
    public async Task CreateTransaction()
    {
        var input = _fixture.getExampleInput();

        Guid output = await _fixture.ApiClient
            .Post<Guid>(
                "/transactions",
                input
            );

        output.ShouldNotBe(default);;
        // DomainEntity.Transaction dbTransaction = await _fixture.Persistence
        //     .GetById(output.Id);
        // dbTransaction.ShouldNotBeNull();
        // dbTransaction.Name.ShoulBe(input.Name);
        // dbTransaction.Description.ShoulBe(input.Description);
        // dbTransaction.IsActive.ShoulBe(input.IsActive);
        // dbTransaction.Id.ShoulNotBeEmpty();
        // dbTransaction.CreatedAt.Should()
        //     .NotBeSameDateAs(default);
    }

}
