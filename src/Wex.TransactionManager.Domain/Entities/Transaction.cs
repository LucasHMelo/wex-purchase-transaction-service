using Wex.TransactionManager.Domain.SeedWork;
using Wex.TransactionManager.Domain.Validation;
using Wex.TransactionManager.Domain.ValueObjects;

namespace Wex.TransactionManager.Domain.Entities;

public class Transaction : AggregateRoot
{
    public string Description { get; private set; }
    public Money Amount { get; private set; }
    public DateTime TransactionDate { get; private set; } 
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public Transaction(string description, decimal amount, DateTime transactionDate)
        : base()
    {
        Description = description;
        Amount = Money.Create(amount);
        TransactionDate = transactionDate;

        Validate();        
    }

    public void Validate()
    {
        DomainValidation.NotNull(Description, nameof(Description));
        DomainValidation.MaxLength(Description, 50, nameof(Description));
        DomainValidation.ValideDateTime(TransactionDate, nameof(TransactionDate));
    }
}
