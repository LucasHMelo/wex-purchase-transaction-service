
using Xunit;
using NSubstitute;
using DomainEntity = Wex.TransactionManager.Domain.Transaction;
using Wex.TransactionManager.Domain.Exceptions;

namespace Wex.TransactionManagement.Domain.Tests.Transaction;

public class TransactionTests
{

    //     Description: must not exceed 50 characters
    // Transaction date: must be a valid date format
    // Purchase amount: must be a valid positive amount rounded to the nearest cent
    // Unique identifier: must uniquely identify the purchase



    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Transaction - Aggregates")]
    public void Instantiate()
    {
        // Arrange 
        var validData = new
        {
            TransactionId = Guid.NewGuid(),
            Description = "category name",
            Amount = 0
        };
        var datetimeBefore = DateTime.UtcNow;

        // Act
        var transaction = new DomainEntity.Transaction(validData.Description, validData.Amount);
        var datetimeAfter = DateTime.UtcNow;

        // Asert
        Assert.NotNull(transaction);
        Assert.NotEmpty(transaction.Id.ToString());
        Assert.Equal(validData.Description, transaction.Description);
        Assert.True(validData.Amount == transaction.Amount);
        Assert.NotEqual(default(Guid), transaction.Id);
        Assert.NotEqual(default(DateTime), transaction.CreatedAt);
        Assert.True(transaction.CreatedAt > datetimeBefore);
        Assert.True(transaction.CreatedAt < datetimeAfter);

    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Transaction - Aggregates")]
    [InlineData(null)]
    public void InstantiateErrorWhenDescriptionIsNull(string? description)
    {
        Action action =
            () => new DomainEntity.Transaction(description!, 100);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should not be null", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan50Characters))]
    [Trait("Domain", "Transaction - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan50Characters()
    {
        var invalidDescription = String.Join(null, Enumerable.Range(1, 51).Select(_ => "a").ToArray());
        Action action =
            () => new DomainEntity.Transaction(invalidDescription, 100);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should be less or equal 50 characters long", exception.Message);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenAmountIsInvalid))]
    [Trait("Domain", "Transaction - Aggregates")]
    [InlineData(-1)]
    public void InstantiateErrorWhenAmountIsInvalid(double amount)
    {
        Action action =
            () => new DomainEntity.Transaction("Description", amount);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Amount should not be under zero", exception.Message);
    }

    // [Fact]
    // public void TransactionEntity_Create_ShouldReturnTrue()
    // {
    //     // Arrange 

    //     // Act

    //     // Asert

    // }


}
