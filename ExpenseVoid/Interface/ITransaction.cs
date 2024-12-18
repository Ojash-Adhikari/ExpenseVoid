using ExpenseVoid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseVoid.Interface
{
    public interface ITransaction
    {
        Task SaveTransactionAsync(Transaction transaction);
        Task RemoveTransactionAsync(Transaction transaction);
        Task EditTransactionAsync(Transaction transaction);
        Task<List<Transaction>> LoadTransactionAsync();

        Task ProcessTransactionAsync(Transaction transaction);
    }
}
