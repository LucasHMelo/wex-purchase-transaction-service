using MediatR;
using Wex.TransactionManager.Application.Interfaces;
using Wex.TransactionManager.Application.UseCases.Transactions.CreateTransactions;
using Wex.TransactionManager.Domain.Repositories;
using Wex.TransactionManager.Infrastructure.Repositories;

namespace Wex.TransactionManager.Api.Configurations;

public static class UseCasesConfiguration
{
    public static IServiceCollection AddUseCases(
        this IServiceCollection services
    )
    {
        services.AddMediatR(typeof(CreateTransaction));
        services.AddRepositories();
        return services;
    }

    private static IServiceCollection AddRepositories(
            this IServiceCollection services
        )
    {
        services.AddTransient<
            ITransactionRepository, TransactionRepository>();
        services.AddTransient<IUnitOfWork, IUnitOfWork>();
        return services;
    }


}