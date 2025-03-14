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
        Task SaveTagAsync(Tag tag); // Create
        Task RemoveTagAsync(Tag tag); // Delete
        Task EditTagAsync(Tag tag); // Update
        Task<List<Tag>> LoadTagsAsync(); // Read

        Task<List<Tag>> GetTagsByUserIdAsync(Guid userId);
        Task<List<Tag>> GetTagsForUserAsync(Guid userId);
    }
}
