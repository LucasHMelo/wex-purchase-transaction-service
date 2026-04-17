using System;
using MediatR;

namespace Wex.TransactionManager.Application.UseCases.Transactions.GetTransactions;

public interface IGetTransaction: IRequestHandler<GetTransactionInput, GetTransactionOutput>
{}
