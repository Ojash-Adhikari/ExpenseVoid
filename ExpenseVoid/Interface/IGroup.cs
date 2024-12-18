using ExpenseVoid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseVoid.Interface
{
    public interface IGroup
    {
        Task SaveGroupAsync(Group group);
        Task RemoveGroupAsync(Group group);
        Task EditGroupAsync(Group group);
        Task<List<Group>> LoadGroupsAsync();
    }
}
