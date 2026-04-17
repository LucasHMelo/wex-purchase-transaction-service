using MediatR;
using Wex.TransactionManager.Domain.Repositories;

namespace Wex.TransactionManager.Application.UseCases.Transactions.GetTransactions;

public class GetTransaction(ITransactionRepository TransactionRepository) 
    : IGetTransaction
{
    private readonly ITransactionRepository _transactionRepository = TransactionRepository;

    public async Task<GetTransactionOutput> Handle(
        GetTransactionInput request, 
        CancellationToken cancellationToken
    )
    {
        var transaction = await _transactionRepository.Get(request.Id, cancellationToken);
        return GetTransactionOutput.FromTransaction(transaction);
    }
}
