using MediatR;
using Microsoft.Extensions.Logging;
using Wex.TransactionManager.Application.Services;
using Wex.TransactionManager.Domain.Repositories;
using Wex.TransactionManager.Domain.ValueObjects;

namespace Wex.TransactionManager.Application.UseCases.Transactions.GetTransactions;

public class GetTransaction(ITransactionRepository TransactionRepository,
    IExchangeRateService exchangeRateService,
    ILogger<GetTransaction> logger)
    : IRequestHandler<GetTransactionInput, GetTransactionOutput>
{
    private readonly ITransactionRepository _transactionRepository = TransactionRepository;
    private readonly IExchangeRateService _exchangeRateService = exchangeRateService;
    private readonly ILogger<GetTransaction> _logger = logger;

    public async Task<GetTransactionOutput> Handle(
        GetTransactionInput request,
        CancellationToken cancellationToken
    )
    {
        var transaction = await _transactionRepository.Get(request.Id, cancellationToken);
        if (transaction == null)
        {
            _logger.LogWarning("Transaction with ID {TransactionId} not found", request.Id);
            throw new KeyNotFoundException($"Transaction with ID {request.Id} not found");
        }

        if (request.TargetCurrency == transaction.Amount.Currency)
        {
            _logger.LogInformation("Target currency {TargetCurrency} is same as original currency {OriginalCurrency}", 
                request.TargetCurrency, transaction.Amount.Currency);

            return GetTransactionOutput.FromSameCurrencyTransaction(transaction);
        }

        var exchangeRateResult = await _exchangeRateService.GetExchangeRateWithDateAsync(
            request.TargetCurrency,
            transaction.TransactionDate,
            cancellationToken);

        var convertedAmount = ConvertAmount(transaction.Amount, exchangeRateResult.Rate, request.TargetCurrency);

        _logger.LogInformation("Converted Transaction {TransactionId}: {OriginalAmount} {OriginalCurrency} = {ConvertedAmount} {TargetCurrency} (rate: {ExchangeRate} from {RateDate})",
            transaction.Id,
            transaction.Amount.Value,
            transaction.Amount.Currency,
            convertedAmount.Value,
            request.TargetCurrency,
            exchangeRateResult.Rate,
            exchangeRateResult.RateDate);

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
            throw new NotSupportedException("This convertion is not supported.");
        }

        return Money.Create(convertedAmount, targetCurrency);
    }
}
