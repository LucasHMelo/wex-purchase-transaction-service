using Bogus;

namespace Wex.TransactionManagement.UnitTests.Common;

public abstract class BaseFixture
{
    public Faker Faker { get; set; }

    protected BaseFixture() 
        => Faker = new Faker();
}
