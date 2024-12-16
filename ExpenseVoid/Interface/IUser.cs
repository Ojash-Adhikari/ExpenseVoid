using ExpenseVoid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseVoid.Interface
{
    public interface IUser
    {
        Task SaveUserAsync(User user);
        Task RemoveUserAsync(User user);
        Task EditUserAsync(User user);
        bool ValidatePassword(string storedPassword, string providedPassword);
        Task<List<User>> LoadUsersAsync();
    }
}
