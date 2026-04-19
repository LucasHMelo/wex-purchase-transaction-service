using Wex.TransactionManager.Domain.SeedWork;

namespace Wex.TransactionManager.Domain.Entities;

public class ExchangeRate : AggregateRoot
{
    public string Currency { get; private set; } = string.Empty;
    public decimal Rate { get; private set; }
    public DateTime Date { get; private set; }
    public DateTime RecordDate { get; private set; }
}
