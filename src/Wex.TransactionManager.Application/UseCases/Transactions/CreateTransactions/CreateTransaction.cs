using Wex.TransactionManager.Application.Interfaces;
using Wex.TransactionManager.Domain.Entities;
using Wex.TransactionManager.Domain.Repositories;

namespace Wex.TransactionManager.Application.UseCases.Transactions.CreateTransactions;

public class CreateTransaction(
    ITransactionRepository transactionRepository,
    IUnitOfWork unitOfWork
    ) : ICreateTransaction
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Guid> HandleAsync(
        CreateTransactionInput input, 
        CancellationToken cancellationToken)
    {
        var transaction = new Transaction(
            input.Description,
            input.Amount,
            input.TransactionDate
        );

        await _transactionRepository.Insert(transaction, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
        
        return Guid.NewGuid();
    }

}
