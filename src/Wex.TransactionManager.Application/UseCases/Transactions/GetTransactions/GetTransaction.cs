using MediatR;
using Wex.TransactionManager.Application.Services;
using Wex.TransactionManager.Domain.Repositories;
using Wex.TransactionManager.Domain.ValueObjects;

namespace Wex.TransactionManager.Application.UseCases.Transactions.GetTransactions;

public class GetTransaction(ITransactionRepository TransactionRepository,
    IExchangeRateService exchangeRateService)
    : IGetTransaction
{
    private readonly ITransactionRepository _transactionRepository = TransactionRepository;
    private readonly IExchangeRateService _exchangeRateService = exchangeRateService;
    public async Task<GetTransactionOutput> Handle(
        GetTransactionInput request,
        CancellationToken cancellationToken
    )
    {
        var transaction = await _transactionRepository.Get(request.Id, cancellationToken) 
            ?? throw new KeyNotFoundException($"Transaction with ID {request.Id} not found");

        if (request.TargetCurrency == transaction.Amount.Currency)
        {
            return GetTransactionOutput.FromSameCurrencyTransaction(transaction);
        }

        var exchangeRateResult = await _exchangeRateService.GetExchangeRateWithDateAsync(
            request.TargetCurrency,
            transaction.TransactionDate,
            cancellationToken);

        var convertedAmount = ConvertAmount(transaction.Amount, exchangeRateResult.Rate, request.TargetCurrency);

        return GetTransactionOutput.FromTransaction(transaction, convertedAmount, exchangeRateResult);
    }

    private static Money ConvertAmount(Money originalAmount, decimal exchangeRate, string targetCurrency)
    {
        decimal convertedAmount;
        if (originalAmount.Currency == "USD")
        {
            convertedAmount = originalAmount.Value * exchangeRate;
        }
        else if (targetCurrency == "USD")
        {
            convertedAmount = originalAmount.Value / exchangeRate;
        }
        else
        {
            throw new NotSupportedException("Direct conversion between non-USD currencies is not supported. Please convert to USD first, then to the target currency.");
        }

        return Money.Create(convertedAmount, targetCurrency);
    }
}
