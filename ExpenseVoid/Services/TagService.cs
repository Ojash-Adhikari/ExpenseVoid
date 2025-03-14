using ExpenseVoid.Interface;
using ExpenseVoid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.System;

namespace ExpenseVoid.Services
{
    
    public class TagService : ITag
    {
        private readonly string tagFilePath = Path.Combine(AppContext.BaseDirectory, "ExpenseVoid", "Tags&Source", "TagDetails.json");

        private readonly List<Tag> permanentTags = new List<Tag>
        {
            new Tag { TagId = Guid.NewGuid(), TagName = "Yearly", TagColor = "#FF5733" },
            new Tag { TagId = Guid.NewGuid(), TagName = "Monthly", TagColor = "#33FF57" },
            new Tag { TagId = Guid.NewGuid(), TagName = "Food", TagColor = "#3357FF" },
            new Tag { TagId = Guid.NewGuid(), TagName = "Drinks", TagColor = "#FFC300" },
            new Tag { TagId = Guid.NewGuid(), TagName = "Clothes", TagColor = "#DAF7A6" },
            new Tag { TagId = Guid.NewGuid(), TagName = "Gadgets", TagColor = "#581845" },
            new Tag { TagId = Guid.NewGuid(), TagName = "Miscellaneous", TagColor = "#900C3F" },
            new Tag { TagId = Guid.NewGuid(), TagName = "Fuel", TagColor = "#C70039" },
            new Tag { TagId = Guid.NewGuid(), TagName = "Rent", TagColor = "#FF5733" },
            new Tag { TagId = Guid.NewGuid(), TagName = "EMI", TagColor = "#FFC300" },
            new Tag { TagId = Guid.NewGuid(), TagName = "Party", TagColor = "#DAF7A6" }
        };

        public async Task SaveTagAsync(Tag tag)
        {
            try
            {
                if (tag.TagId == Guid.Empty)
                {
                    tag.TagId = Guid.NewGuid();
                }
                var tags = await LoadTagsAsync();
                tags.Add(tag);
                await SaveTagsAsync(tags);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving tag: {ex.Message}");
                throw;
            }
        }
        private async Task SaveTagsAsync(List<Tag> tags)
        {
            try
            {
                var json = JsonSerializer.Serialize(tags, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(tagFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving tags: {ex.Message}");
                throw;
            }
        }
        public async Task EditTagAsync(Tag tag)
        {
            try
            {

                var tags = await LoadTagsAsync();


                var existingTag = tags.FirstOrDefault(u => u.TagId == tag.TagId);

                if (existingTag != null)
                {
 
                    existingTag.TagName = tag.TagName;
                    existingTag.TagColor = tag.TagColor;
               
                    
                    await SaveTagsAsync(tags);
                }
                else
                {
                    throw new Exception("Tag not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error editing tag: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Tag>> LoadTagsAsync()
        {
            try
            {
                // Check if file exists
                if (!File.Exists(tagFilePath))
                {
                    // Save permanent tags if file does not exist
                    await SaveTagsAsync(permanentTags);
                    return permanentTags;
                }

                // Load existing tags
                var json = await File.ReadAllTextAsync(tagFilePath);
                var tags = JsonSerializer.Deserialize<List<Tag>>(json) ?? new List<Tag>();

                // Ensure permanent tags are added
                foreach (var permanentTag in permanentTags)
                {
                    if (!tags.Any(t => t.TagName == permanentTag.TagName))
                    {
                        tags.Add(permanentTag);
                    }
                }

                // Save updated tags list
                await SaveTagsAsync(tags);

                return tags;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading tags: {ex.Message}");
                return new List<Tag>();
            }
        }


        public async Task RemoveTagAsync(Tag tag)
        {
            try
            {
                var tags = await LoadTagsAsync();
                var tagToRemove = tags.FirstOrDefault(t => t.TagId == tag.TagId);

                if (tagToRemove != null && !permanentTags.Any(pt => pt.TagName == tagToRemove.TagName))
                {
                    tags.Remove(tagToRemove);
                    await SaveTagsAsync(tags);
                }
                else
                {
                    Console.WriteLine("Cannot remove permanent tag.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing tag: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Tag>> GetTagsByUserIdAsync(Guid userId)
        {
            try
            {
                var tags = await LoadTagsAsync();
                return tags.Where(t => t.User?.UserID == userId).ToList();
            }
            catch
            {
                Console.WriteLine($"Error Fetching Tags through userId {userId}");
                throw;
            }
        }

        public async Task<List<Tag>> GetTagsForUserAsync(Guid userId)
        {
            try
            {
                // Fetch tags specific to the user
                var userTags = await GetTagsByUserIdAsync(userId);

                // Combine permanent tags with user-specific tags
                var combinedTags = permanentTags
                    .Concat(userTags)
                    .GroupBy(tag => tag.TagId) // Ensure no duplicate TagId
                    .Select(group => group.First())
                    .ToList();

                return combinedTags;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Fetching Tags for userId {userId}: {ex.Message}");
                throw;
            }
        }
    }
}
