using MediatR;

namespace Wex.TransactionManager.Application.UseCases.Transactions.GetTransactions;

public class GetTransactionInput : IRequest<GetTransactionOutput>
{
    public Guid Id { get; set; }
    public GetTransactionInput(Guid id) 
        => Id = id;
}
