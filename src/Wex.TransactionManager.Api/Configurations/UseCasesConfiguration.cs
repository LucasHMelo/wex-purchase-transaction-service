using MediatR;
using Wex.TransactionManager.Application.Interfaces;
using Wex.TransactionManager.Application.Services;
using Wex.TransactionManager.Application.UseCases.Transactions.CreateTransactions;
using Wex.TransactionManager.Application.UseCases.Transactions.GetTransactions;
using Wex.TransactionManager.Domain.Repositories;
using Wex.TransactionManager.Infrastructure;
using Wex.TransactionManager.Infrastructure.Repositories;

namespace Wex.TransactionManager.Api.Configurations;

public static class UseCasesConfiguration
{
    public static IServiceCollection AddUseCases(
        this IServiceCollection services
    )
    {
        services.AddMediatR(typeof(CreateTransaction));
        services.AddMediatR(typeof(GetTransaction).Assembly);
        services.AddRepositories();
        services.AddServices();
        return services;
    }

    private static IServiceCollection AddRepositories(
            this IServiceCollection services
        )
    {
        services.AddTransient<ITransactionRepository, TransactionRepository>();
        services.AddTransient<IExchangeRateRepository, ExchangeRateRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        return services;
    }

    private static IServiceCollection AddServices(
            this IServiceCollection services
        )
    {
        services.AddScoped<IExchangeRateService, ExchangeRateService>();
        return services;
    }

}