using ExpenseVoid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseVoid.Interface
{
    public interface ICurrency
    {
        Task<IList<Currency>> GetCurrencyLiveRatesAsync(string exchangeRateCurrencyCode);
    }
}
