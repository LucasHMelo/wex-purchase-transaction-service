using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Wex.TransactionManager.Application.UseCases.Transactions.CreateTransactions;

public class CreateTransactionInput : IRequest<CreateTransactionOutput>
{
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (Amount <= 0)
            yield return new ValidationResult("Amount must be greater than 0");

        if (string.IsNullOrWhiteSpace(Description))
            yield return new ValidationResult("Description required");

        if(TransactionDate == default)
        {
            yield return new ValidationResult("TransactionDate required");
        }
    }

}
