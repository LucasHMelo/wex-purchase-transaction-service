using System;
using Wex.TransactionManager.Application.UseCases.Transactions.GetTransactions;

namespace Wex.TransactionManagement.UnitTests.Application.Transaction.GetTransaction;


[Collection(nameof(GetTransactionTestFixture))]
public class GetTransactionInputValidatorTest(GetTransactionTestFixture fixture)
{
    private readonly GetTransactionTestFixture _fixture = fixture;

    // [Fact(DisplayName = nameof(ValidationOk))]
    // [Trait("Application", "GetTransactionInputValidation - UseCases")]
    // public void ValidationOk()
    // {
    //     var validInput = new GetTransactionInput(Guid.NewGuid());
    //     var validator = new GetTransactionInputValidator();

    //     var validationResult = validator.Validate(validInput);

    //     validationResult.Should().NotBeNull();
    //     validationResult.IsValid.Should().BeTrue();
    //     validationResult.Errors.Should().HaveCount(0);
    // }

    // [Fact(DisplayName = nameof(InvalidWhenEmptyGuidId))]
    // [Trait("Application", "GetTransactionInputValidation - UseCases")]
    // public void InvalidWhenEmptyGuidId()
    // {
    //     var invalidInput = new GetTransactionInput(Guid.Empty);
    //     var validator = new GetTransactionInputValidator();

    //     var validationResult = validator.Validate(invalidInput);

    //     validationResult.Should().NotBeNull();
    //     validationResult.IsValid.Should().BeFalse();
    //     validationResult.Errors.Should().HaveCount(1);
    //     validationResult.Errors[0].ErrorMessage
    //         .Should().Be("'TransactionId' must not be empty.");
    // }

}
