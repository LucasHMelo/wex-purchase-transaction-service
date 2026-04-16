using DomainEntity = Wex.TransactionManager.Domain.Entities;

namespace Wex.TransactionManager.Application.UseCases.Transactions.CreateTransactions;

public class CreateTransactionOutput
{
    public CreateTransactionOutput(Guid transactionId)
    {
        TransactionId = transactionId;
    }

    public Guid TransactionId { get; set; }

    

    public static CreateTransactionOutput FromTransaction(DomainEntity.Transaction transaction)
    => new CreateTransactionOutput(
        transaction.Id
    );
}
