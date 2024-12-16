using ExpenseVoid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseVoid.Interface
{
    public interface ITag
    {
        Task SaveTagAsync(Tag tag);
        Task RemoveTagAsync(Tag tag);
        Task EditTagAsync(Tag tag);
        Task<List<Tag>> LoadTagsAsync();
    }
}
