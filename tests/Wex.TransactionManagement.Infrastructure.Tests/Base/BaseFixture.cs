using Bogus;

namespace Wex.TransactionManagement.IntegrationTests.Base;

public class BaseFixture
{
    public BaseFixture() 
        => Faker = new Faker();

    protected Faker Faker { get; set; }
}