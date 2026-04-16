namespace Wex.TransactionManager.Application.UseCases.Transactions.CreateTransactions;

public class CreateTransactionInput
{
    public decimal Amount { get; init; }
    public string Description { get; set; } = string.Empty;
    public DateTime TransactionDate { get; init; }

}
