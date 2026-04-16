namespace Wex.TransactionManager.Application.UseCases.Transactions.CreateTransactions;

public interface ICreateTransaction
{
    Task<object> CreateTransaction(CreateTransactionInput transaction);
}
