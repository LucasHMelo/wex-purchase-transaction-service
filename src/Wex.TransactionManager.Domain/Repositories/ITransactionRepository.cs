using System.Transactions;
using Wex.TransactionManager.Domain.SeedWork;

namespace Wex.TransactionManager.Domain.Repositories;

public interface ITransactionRepository : IGenericRepository<Transaction>
{ }