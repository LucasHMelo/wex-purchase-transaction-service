namespace Wex.TransactionManager.Application.UseCases.Transactions.CreateTransactions;

public interface ICreateTransaction
{
    Task<Guid> HandleAsync(CreateTransactionInput transaction, CancellationToken cancellationToken);
}
