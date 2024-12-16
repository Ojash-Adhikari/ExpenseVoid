using ExpenseVoid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseVoid.Interface
{
    public interface ITransactionType
    {
        Task SaveTransactionTypeAsync(TransactionType transactionType);
        Task RemoveTransactionTypeAsync(TransactionType transactionType);
        Task EditTransactionTypeAsync(TransactionType transactionType);
        Task<List<TransactionType>> LoadTransactionTypesAsync();
    }
}
