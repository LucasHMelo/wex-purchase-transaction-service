using System.Net.Http.Headers;
using Wex.TransactionManager.Domain.Exceptions;
using Wex.TransactionManager.Domain.ValueObjects;

namespace Wex.TransactionManager.Domain.Transaction;

public class Transaction
{
    public Guid Id { get; private set; }
    public string Description { get; private set; }
    public Money Amount { get; private set; }
    public DateTime CreatedAt { get; private set; } 

    public Transaction(string description, decimal amount)
    {
        Id = Guid.NewGuid();
        Description = description;
        Amount = Money.Create(amount);
        CreatedAt = DateTime.UtcNow;

        Validate();        
    }

    public void Validate()
    {
        if (Description == null)
            throw new EntityValidationException($"{nameof(Description)} should not be null");
        if (Description.Length > 50)
            throw new EntityValidationException($"{nameof(Description)} should be less or equal 50 characters long");
    }
}
