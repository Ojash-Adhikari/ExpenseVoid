using ExpenseVoid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseVoid.Interface
{
    public interface ISource
    {
        Task SaveSourceAsync(Source source);
        Task RemoveSourceAsync(Source source);
        Task EditSourceAsync(Source source);
        Task<List<Source>> LoadSourceAsync();
        Task<List<Source>> GetSourceByUser(Guid userId);


    }
}
