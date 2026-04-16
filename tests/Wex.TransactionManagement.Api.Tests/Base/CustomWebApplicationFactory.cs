using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wex.TransactionManager.Infrastructure.Data.DbContexts;

namespace Wex.TransactionManagement.E2ETests.Base;

public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup>
    where TStartup : class
{
    protected override void ConfigureWebHost(
        IWebHostBuilder builder
    )
    {
        builder.ConfigureServices(services => {
            var dbOptions = services.FirstOrDefault(
                x => x.ServiceType == typeof(
                    DbContextOptions<WexTransactionDbContext>
                )
            );
            if(dbOptions is not null)
                services.Remove(dbOptions);
            services.AddDbContext<WexTransactionDbContext>(
                options => {
                    options.UseInMemoryDatabase("end2end-tests-db");
                }
            );
        });

        base.ConfigureWebHost(builder);
    }
}