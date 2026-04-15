using System.Net.Http.Headers;
using Wex.TransactionManager.Domain.Exceptions;
using Wex.TransactionManager.Domain.SeedWork;
using Wex.TransactionManager.Domain.Validation;
using Wex.TransactionManager.Domain.ValueObjects;

namespace Wex.TransactionManager.Domain.Transaction;

public class Transaction : AggregateRoot
{
    public string Description { get; private set; }
    public Money Amount { get; private set; }
    public DateTime CreatedAt { get; private set; } 

    public Transaction(string description, decimal amount)
        : base()
    {
        Description = description;
        Amount = Money.Create(amount);
        CreatedAt = DateTime.UtcNow;

        Validate();        
    }

    public void Validate()
    {
        DomainValidation.NotNull(Description, nameof(Description));
        DomainValidation.MaxLength(Description, 50, nameof(Description));
    }
}
