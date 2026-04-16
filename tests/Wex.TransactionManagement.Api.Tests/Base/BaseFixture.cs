using System;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Wex.TransactionManager.Infrastructure.Data.DbContexts;

namespace Wex.TransactionManagement.E2ETests.Base;

public class BaseFixture
{
    public BaseFixture()
    => Faker = new Faker();

    protected Faker Faker { get; set; }

    public WexTransactionDbContext CreateDbContext()
    {
        var context = new WexTransactionDbContext(
            new DbContextOptionsBuilder<WexTransactionDbContext>()
            .UseInMemoryDatabase("end2end-tests-db")
            .Options
        );
        return context;
    }
}