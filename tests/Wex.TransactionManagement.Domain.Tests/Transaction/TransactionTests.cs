
using DomainEntity = Wex.TransactionManager.Domain.Transaction;
using Wex.TransactionManager.Domain.Exceptions;
using Shouldly;

namespace Wex.TransactionManagement.Domain.Tests.Transaction;

[Collection(nameof(TransactionTestFixture))]
public class TransactionTests
{

    //     Description: must not exceed 50 characters
    // Transaction date: must be a valid date format
    // Purchase amount: must be a valid positive amount rounded to the nearest cent
    // Unique identifier: must uniquely identify the purchase
    private readonly TransactionTestFixture _transactionTestFixture;

    public TransactionTests(TransactionTestFixture transactionTestFixture)
    {
        _transactionTestFixture = transactionTestFixture;
    }

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Transaction - Aggregates")]
    public void Instantiate()
    {
        // Arrange 
        var validData = _transactionTestFixture.GetValidTransaction();
        var datetimeBefore = DateTime.UtcNow;

        // Act
        var transaction = new DomainEntity.Transaction(validData.Description, validData.Amount);
        var datetimeAfter = DateTime.UtcNow;

        // Asert
        transaction.ShouldNotBeNull();
        transaction.Id.ToString().ShouldNotBeEmpty();
        transaction.Description.ShouldBeEquivalentTo(validData.Description);
        transaction.Amount.ShouldBe(validData.Amount);
        transaction.Id.ShouldNotBe(default(Guid));
        transaction.CreatedAt.ShouldNotBe(default(DateTime));
        transaction.CreatedAt.ShouldBeGreaterThan(datetimeBefore);
        transaction.CreatedAt.ShouldBeLessThan(datetimeAfter);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Transaction - Aggregates")]
    [InlineData(null)]
    public void InstantiateErrorWhenDescriptionIsNull(string? description)
    {
        Action action =
            () => new DomainEntity.Transaction(description!, 100);
        var exception = Assert.Throws<EntityValidationException>(action);
        exception.Message.ShouldBeEquivalentTo("Description should not be null");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan50Characters))]
    [Trait("Domain", "Transaction - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan50Characters()
    {
        var invalidDescription = String.Join(null, Enumerable.Range(1, 51).Select(_ => "a").ToArray());
        Action action =
            () => new DomainEntity.Transaction(invalidDescription, 100);
        var exception = Should.Throw<EntityValidationException>(action);
        exception.Message.ShouldBeEquivalentTo("Description should be less or equal 50 characters long");
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenAmountIsInvalid))]
    [Trait("Domain", "Transaction - Aggregates")]
    [InlineData(-1)]
    public void InstantiateErrorWhenAmountIsInvalid(double amount)
    {
        Action action =
            () => new DomainEntity.Transaction("Description", amount);
        var exception = Should.Throw<EntityValidationException>(action);
        exception.Message.ShouldBeEquivalentTo("Amount should not be under zero");

    }

    // [Fact]
    // public void TransactionEntity_Create_ShouldReturnTrue()
    // {
    //     // Arrange 

    //     // Act

    //     // Asert

    // }


}
