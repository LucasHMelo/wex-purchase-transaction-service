using Wex.TransactionManager.Domain.Exceptions;
using Wex.TransactionManager.Domain.ValueObjects;

namespace Wex.TransactionManagement.Domain.Tests.ValueObjects;

public class MoneyTests
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Money - ValueObjects")]
    public void Instantiate()
    {
        // Arrange
        decimal amount = 100.50m;
        string currency = "USD";

        // Act
        var money = Money.Create(amount, currency);

        // Assert
        Assert.Equal(100.50m, money.Value);
        Assert.Equal("USD", money.Currency);
    }

    [Fact(DisplayName = nameof(InstantiateWithNegativeAmountShouldThrowArgumentException))]
    [Trait("Domain", "Money - ValueObjects")]
    public void InstantiateWithNegativeAmountShouldThrowArgumentException()
    {
        // Arrange
        decimal amount = -10m;
        string currency = "USD";

        // Act & Assert
        Assert.Throws<EntityValidationException>(() => Money.Create(amount, currency));
    }

    [Fact(DisplayName = nameof(InstantiateWithZeroAmountShouldThrowArgumentException))]
    [Trait("Domain", "Money - ValueObjects")]
    public void InstantiateWithZeroAmountShouldThrowArgumentException()
    {
        // Arrange
        decimal amount = 0m;
        string currency = "USD";

        // Act & Assert
        Assert.Throws<EntityValidationException>(() => Money.Create(amount, currency));
    }

    [Fact(DisplayName = nameof(InstantiateWithEmptyCurrencyShouldThrowArgumentException))]
    [Trait("Domain", "Money - ValueObjects")]
    public void InstantiateWithEmptyCurrencyShouldThrowArgumentException()
    {
        // Arrange
        decimal amount = 100m;
        string currency = "";

        // Act & Assert
        Assert.Throws<EntityValidationException>(() => Money.Create(amount, currency));
    }

    [Fact(DisplayName = nameof(InstantiateWithNullCurrencyShouldThrowArgumentException))]
    [Trait("Domain", "Money - ValueObjects")]
    public void InstantiateWithNullCurrencyShouldThrowArgumentException()
    {
        // Arrange
        decimal amount = 100m;
        string currency = null!;

        // Act & Assert
        Assert.Throws<EntityValidationException>(() => Money.Create(amount, currency));
    }

    [Theory(DisplayName = nameof(InstantiateWithMoreThanTwoDecimalPlacesShouldRoundToTwoDecimalPlaces))]
    [Trait("Domain", "Money - ValueObjects")]
    [InlineData(250.123, 250.12)]  // down
    [InlineData(555.9652, 555.97)]  // up
    [InlineData(100.001, 100.00)]  // down
    [InlineData(100.999, 101.00)]  // up
    [InlineData(878.1, 878.10)]  // up
    [InlineData(1000.00001, 1000.00)]  // down
    public void InstantiateWithMoreThanTwoDecimalPlacesShouldRoundToTwoDecimalPlaces(decimal input, decimal expected)
    {
        // Arrange
        string currency = "USD";

        // Act
        var money = Money.Create(input, currency);

        // Assert
        Assert.Equal(expected, money.Value);
    }

    [Fact(DisplayName = nameof(InstantiateWithLowercaseCurrencyShouldConvertToUppercase))]
    [Trait("Domain", "Money - ValueObjects")]
    public void InstantiateWithLowercaseCurrencyShouldConvertToUppercase()
    {
        // Arrange
        decimal amount = 100m;
        string currency = "usd";

        // Act
        var money = Money.Create(amount, currency);

        // Assert
        Assert.Equal("USD", money.Currency);
    }

}
