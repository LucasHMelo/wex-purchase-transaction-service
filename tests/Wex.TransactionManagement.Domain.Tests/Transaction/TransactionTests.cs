
using Xunit;
using NSubstitute;

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
            CreatedAt = DateTime.UtcNow,
            Amount = 0
        };

        // Act

        var transaction = new Transaction(validData.TransactionId, validData.Description, validData.CreatedAt, validData.Amount);

        // Asert
        transaction.TransactionId.ShouldBe(validData.TransactionId);
        transaction.Description.ShouldBe(validData.Description);
        transaction.CreatedAt.ShouldBe(validData.CreatedAt);
        transaction.Amount.ShouldBe(validData.Amount);

    }

    // [Fact]
    // public void TransactionEntity_Create_ShouldReturnTrue()
    // {
    //     // Arrange 

    //     // Act

    //     // Asert

    // }


}
