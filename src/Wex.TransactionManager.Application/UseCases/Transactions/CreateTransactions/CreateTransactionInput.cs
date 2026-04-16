using MediatR;

namespace Wex.TransactionManager.Application.UseCases.Transactions.CreateTransactions;

public class CreateTransactionInput : IRequest<CreateTransactionOutput>
{
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }

}
