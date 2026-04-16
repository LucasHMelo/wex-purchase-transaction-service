using MediatR;

namespace Wex.TransactionManager.Application.UseCases.Transactions.CreateTransactions;

public interface ICreateTransaction : IRequestHandler<CreateTransactionInput, CreateTransactionOutput>
{
    Task<CreateTransactionOutput> Handle(CreateTransactionInput transaction, CancellationToken cancellationToken);
}
