using ExpenseVoid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseVoid.Interface
{
    public interface IDebt
    {
        Task SaveDebtAsync(Debt debt);
        Task RemoveDebtAsync(Debt debt);
        Task EditDebtAsync(Debt debt);
        Task<List<Debt>> LoadDebtAsync();
        Task<List<Debt>> GetDebtByUserIdAsync(Guid userId);
    }
}
