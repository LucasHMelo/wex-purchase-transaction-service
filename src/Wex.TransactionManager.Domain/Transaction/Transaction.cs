namespace Wex.TransactionManager.Domain.Transaction;

public class Transaction(string description, double amount)
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Description { get; private set; } = description;
    public double Amount { get; private set; } = amount;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
}
