
using DomainEntity = Wex.TransactionManager.Domain.Entities;
using Wex.TransactionManager.Domain.Exceptions;
using Shouldly;

namespace Wex.TransactionManagement.UnitTests.Domain.Transaction;

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

    [Fact(DisplayName = nameof(InstantiateWithIdempotencyKey))]
    [Trait("Domain", "Transaction - Aggregates")]
    public void InstantiateWithIdempotencyKey()
    {
        // Arrange 
        var datetimeBefore = DateTime.UtcNow.AddSeconds(-1);
        var validData = _transactionTestFixture.GetValidTransaction();

        // Act
        var transaction = new DomainEntity
            .Transaction(validData.Description, 
                         validData.Amount.Value,
                         validData.TransactionDate,
                         validData.IdempotencyKey);
        var datetimeAfter = DateTime.UtcNow.AddSeconds(1);

        // Asert
        transaction.ShouldNotBeNull();
        transaction.Id.ToString().ShouldNotBeEmpty();
        transaction.Description.ShouldBeEquivalentTo(validData.Description);
        transaction.Amount.Value.ShouldBe(validData.Amount.Value);
        transaction.Id.ShouldNotBe(default(Guid));
        transaction.TransactionDate.ShouldNotBe(default(DateTime));
        transaction.IdempotencyKey.ShouldBe(validData.IdempotencyKey);
        transaction.CreatedAt.ShouldNotBe(default(DateTime));
        transaction.CreatedAt.ShouldBeGreaterThan(datetimeBefore);
        transaction.CreatedAt.ShouldBeLessThan(datetimeAfter);
    }

    [Fact(DisplayName = nameof(InstantiateWithoutIdempotencyKey))]
    [Trait("Domain", "Transaction - Aggregates")]
    public void InstantiateWithoutIdempotencyKey()
    {
        // Arrange 
        var datetimeBefore = DateTime.UtcNow.AddSeconds(-1);
        var validData = _transactionTestFixture.GetValidTransaction();

        // Act
        var transaction = new DomainEntity
            .Transaction(validData.Description, 
                         validData.Amount.Value,
                         validData.TransactionDate);
        var datetimeAfter = DateTime.UtcNow.AddSeconds(1);

        // Asert
        transaction.ShouldNotBeNull();
        transaction.Id.ToString().ShouldNotBeEmpty();
        transaction.Description.ShouldBeEquivalentTo(validData.Description);
        transaction.Amount.Value.ShouldBe(validData.Amount.Value);
        transaction.Id.ShouldNotBe(default(Guid));
        transaction.IdempotencyKey.ShouldBeNull();
        transaction.TransactionDate.ShouldNotBe(default(DateTime));
        transaction.CreatedAt.ShouldNotBe(default(DateTime));
        transaction.CreatedAt.ShouldBeGreaterThan(datetimeBefore);
        transaction.CreatedAt.ShouldBeLessThan(datetimeAfter);
    }

    [Theory(DisplayName = nameof(InstantiateWhenAmountIsNotDoubleRounded))]
    [Trait("Domain", "Transaction - Aggregates")]
    [InlineData(1.121212)]
    [InlineData(1.1)]
    [InlineData(1.001)]
    public void InstantiateWhenAmountIsNotDoubleRounded(decimal amount)
    {
        // Arrange 
        var validData = _transactionTestFixture.GetValidTransaction();
        var datetimeBefore = DateTime.UtcNow.AddSeconds(-1);
        // Act
        var transaction = new DomainEntity.Transaction(validData.Description, amount, DateTime.UtcNow);
        var datetimeAfter = DateTime.UtcNow.AddSeconds(1);

        // Asert
        var amountRounded = Math.Round(amount, 2, MidpointRounding.AwayFromZero);

        transaction.ShouldNotBeNull();
        transaction.Id.ToString().ShouldNotBeEmpty();
        transaction.Description.ShouldBeEquivalentTo(validData.Description);
        transaction.Amount.Value.ShouldBe(amountRounded);
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
            () => new DomainEntity.Transaction(description!, 100, DateTime.UtcNow);
        var exception = Assert.Throws<EntityValidationException>(action);
        exception.Message.ShouldBeEquivalentTo("Description should not be null");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan50Characters))]
    [Trait("Domain", "Transaction - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan50Characters()
    {
        var invalidDescription = String.Join(null, Enumerable.Range(1, 51).Select(_ => "a").ToArray());
        Action action =
            () => new DomainEntity.Transaction(invalidDescription, 100, DateTime.UtcNow);
        var exception = Should.Throw<EntityValidationException>(action);
        exception.Message.ShouldBeEquivalentTo("Description should be less or equal 50 characters long");
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenAmountIsInvalid))]
    [Trait("Domain", "Transaction - Aggregates")]
    [InlineData(-1)]
    [InlineData(-1.000)]
    public void InstantiateErrorWhenAmountIsInvalid(decimal amount)
    {
        Action action =
            () => new DomainEntity.Transaction("Description", amount, DateTime.UtcNow);
        var exception = Should.Throw<EntityValidationException>(action);
        exception.Message.ShouldBeEquivalentTo("Value must be positive");
    }

    // [Fact]
    // public void TransactionEntity_Create_ShouldReturnTrue()
    // {
    //     // Arrange 

    //     // Act

    //     // Asert

    // }


}
