
using Xunit;
using NSubstitute;
using DomainEntity = Wex.TransactionManager.Domain.Transaction;

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

    // [Fact]
    // public void TransactionEntity_Create_ShouldReturnTrue()
    // {
    //     // Arrange 

    //     // Act

    //     // Asert

    // }


}
