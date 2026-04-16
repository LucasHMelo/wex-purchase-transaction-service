using Wex.TransactionManager.Application.Interfaces;
using DomainEntity = Wex.TransactionManager.Domain.Entities;
using Wex.TransactionManager.Domain.Repositories;

namespace Wex.TransactionManager.Application.UseCases.Transactions.CreateTransactions;

public class CreateTransaction(
    ITransactionRepository transactionRepository,
    IUnitOfWork unitOfWork
    ) : ICreateTransaction
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreateTransactionOutput> Handle(
        CreateTransactionInput input, 
        CancellationToken cancellationToken)
    {
        var transaction = new DomainEntity.Transaction(
            input.Description,
            input.Amount,
            input.TransactionDate
        );

        await _transactionRepository.Insert(transaction, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
        
        return CreateTransactionOutput.FromTransaction(transaction) ;
    }

}
