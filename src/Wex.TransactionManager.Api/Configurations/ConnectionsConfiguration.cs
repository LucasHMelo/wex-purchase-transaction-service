using Microsoft.EntityFrameworkCore;
using Wex.TransactionManager.Infrastructure.Data.DbContexts;

namespace Wex.TransactionManager.Api.Configurations;

public static class ConnectionsConfiguration
{
    public static IServiceCollection AddAppConections(
        this IServiceCollection services)
    {
        services.AddDbConnection();
        return services;
    }

    private static IServiceCollection AddDbConnection(
        this IServiceCollection services)
    {
        services.AddDbContext<WexTransactionDbContext>(
            options => options.UseInMemoryDatabase(
                "InMemory-Wex-Database"
            )
        );
        return services;
    }
}