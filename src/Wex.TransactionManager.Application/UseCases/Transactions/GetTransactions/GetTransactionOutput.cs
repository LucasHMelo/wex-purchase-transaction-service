using DomainEntity = Wex.TransactionManager.Domain.Entities;

namespace Wex.TransactionManager.Application.UseCases.Transactions.GetTransactions;

public class GetTransactionOutput
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string Amount { get; set; }
    public string TransactionDate { get; set; }
    public string CreatedAt { get; set; }

    public GetTransactionOutput(
        Guid id,
        string description,
        string amount,
        string transactionDate,
        string createdAt
    )
    {
        Id = id;
        Description = description;
        Amount = amount;
        TransactionDate = transactionDate;
        CreatedAt = createdAt;
    }

    public static GetTransactionOutput FromTransaction(DomainEntity.Transaction transaction)
        => new(
            transaction.Id,
            transaction.Description,
            transaction.Amount.Value.ToString(),
            transaction.TransactionDate.ToString(),
            transaction.CreatedAt.ToString()
        );
}
