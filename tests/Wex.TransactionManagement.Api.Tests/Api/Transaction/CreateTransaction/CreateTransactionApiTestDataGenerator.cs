namespace Wex.TransactionManagement.E2ETests.Api.Transaction.CreateTransaction;

public class CreateTransactionApiTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs()
    {
        var fixture = new CreateTransactionApiTestFixture();
        var invalidInputsList = new List<object[]>();
        var totalInvalidCases = 3;

        for (int index = 0; index < totalInvalidCases; index++)
        {
            switch (index % totalInvalidCases)
            {
                case 0:
                    var input1 = fixture.getExampleInput();
                    input1.Description = fixture.GetInvalidDescriptionTooLong();
                    invalidInputsList.Add(new object[] {
                        input1,
                        "Name should be at least or equal 50 characters long"
                    });
                    break;
                case 1:
                    var input2 = fixture.getExampleInput();
                    input2.Amount = fixture.GetInvalidValue();
                    invalidInputsList.Add(new object[] {
                        input2,
                        "Value should be positive"
                    });
                    break;
                default:
                    break;
            }
        }

        return invalidInputsList;
    }
}
