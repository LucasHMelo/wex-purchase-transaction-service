using Wex.TransactionManager.Application.Services;
using Wex.TransactionManager.Domain.ValueObjects;
using DomainEntity = Wex.TransactionManager.Domain.Entities;

namespace Wex.TransactionManager.Application.UseCases.Transactions.GetTransactions;

public class GetTransactionOutput
{
    public GetTransactionOutput(Guid id, string description, Money originalAmount, Money convertedAmount, string exchangeRateDate, decimal exchangeRate, DateTime transactionDate, DateTime createdAt)
    {
        Id = id;
        Description = description;
        OriginalAmount = originalAmount;
        ConvertedAmount = convertedAmount;
        ExchangeRateDate = exchangeRateDate;
        ExchangeRate = exchangeRate;
        TransactionDate = transactionDate;
        CreatedAt = createdAt;
    }

    public Guid Id { get; set; }
    public string Description { get; set; }
    public Money OriginalAmount { get; set; } = Money.Zero();
    public Money ConvertedAmount { get; set; } = Money.Zero();
    public string ExchangeRateDate { get; set; } = string.Empty;
    public decimal ExchangeRate { get; set; }
    public DateTime TransactionDate { get; set; }
    public DateTime CreatedAt { get; set; }

    

    public static GetTransactionOutput FromSameCurrencyTransaction(DomainEntity.Transaction transaction)
        => new(
            transaction.Id,
            transaction.Description,
            transaction.Amount,
            transaction.Amount,
            transaction.TransactionDate.ToString("yyyy-MM-dd"),
            1.0m,
            transaction.TransactionDate,
            transaction.CreatedAt
        );

    public static GetTransactionOutput FromTransaction(DomainEntity.Transaction transaction, Money convertedAmount, ExchangeRateResult result )
        => new(
            transaction.Id,
            transaction.Description,
            transaction.Amount,
            convertedAmount,
            result.RateDate.ToString("yyyy-MM-dd"),
            result.Rate,
            transaction.TransactionDate,
            transaction.CreatedAt
        );
}
