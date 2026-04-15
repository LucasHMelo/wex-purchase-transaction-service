using Bogus;

namespace Wex.TransactionManagement.Domain.Tests.Common;

public abstract class BaseFixture
{
    public Faker Faker { get; set; }

    protected BaseFixture() 
        => Faker = new Faker();
}
