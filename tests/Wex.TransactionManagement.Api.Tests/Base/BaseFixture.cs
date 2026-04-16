using System;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Wex.TransactionManager.Infrastructure.Data.DbContexts;

namespace Wex.TransactionManagement.E2ETests.Base;

public class BaseFixture
{
    public ApiClient ApiClient { get; set; }
    public CustomWebApplicationFactory<Program> WebAppFactory { get; set; }
    public HttpClient HttpClient { get; set; }

    public BaseFixture()
    {
        Faker = new Faker();
        WebAppFactory = new CustomWebApplicationFactory<Program>();
        HttpClient = WebAppFactory.CreateClient();
        ApiClient = new ApiClient(HttpClient);
    }

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