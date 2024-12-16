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
        private readonly string tagFilePath = Path.Combine(AppContext.BaseDirectory, "ExpenseVoid", "TagInfo", "TagDetails.json");
        public async Task SaveTagAsync(Tag tag)
        {
            try
            {
                if(tag.TagId == Guid.Empty)
                {
                    tag.TagId = Guid.NewGuid();
                }
                var tags = await LoadTagsAsync();
                tags.Add(tag);
                await SaveTagsAsync(tags);
            }
            catch (Exception ex)
            { 
                Console.WriteLine($"Error saving user: {ex.Message}");
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
            catch (IOException ioEx)
            {
                Console.WriteLine($"I/O error while loading tags: {ioEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while saving tags: {ex.Message}");
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
                    existingTag.TagType = tag.TagType;
               
                    
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
                if (!File.Exists(tagFilePath))
                {
                    return new List<Tag>();
                }

                var json = await File.ReadAllTextAsync(tagFilePath);
                return JsonSerializer.Deserialize<List<Tag>>(json) ?? new List<Tag>();
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON deserialization error: {jsonEx.Message}");
                return new List<Tag>();
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"I/O error while loading users: {ioEx.Message}");
                return new List<Tag>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while loading users: {ex.Message}");
                return new List<Tag>();
            }
        }

        public async Task RemoveTagAsync(Tag tag)
        {
            try
            {
                var tags = await LoadTagsAsync();
                var tagToRemove = tags.FirstOrDefault(u => u.TagId == tag.TagId);

                if (tagToRemove != null)
                {
                    tags.Remove(tagToRemove);


                    await SaveTagsAsync(tags);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Tag Not Removed: {ex.Message}");
                throw;
            }
        }

    }
}
