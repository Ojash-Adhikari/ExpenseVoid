using ExpenseVoid.Interface;
using ExpenseVoid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExpenseVoid.Services
{
    public class GroupService : IGroup
    {
        private readonly string groupFilePath = Path.Combine(AppContext.BaseDirectory, "ExpenseVoid", "Tags&Source", "Group.json");
        public async Task SaveGroupAsync(Group group)
        {
            try
            {
                if (group.GroupID == Guid.Empty)
                {
                    group.GroupID = Guid.NewGuid();
                }
                var groups = await LoadGroupsAsync();
                groups.Add(group);
                await SaveGroupsAsync(groups);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving Target: {ex.Message}");
                throw;
            }
        }
        private async Task SaveGroupsAsync(List<Group> groups)
        {
            try
            {
                var json = JsonSerializer.Serialize(groups, new JsonSerializerOptions { WriteIndented = true });

                await File.WriteAllTextAsync(groupFilePath, json);
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
        public async Task EditGroupAsync(Group group)
        {
            try
            {

                var groups = await LoadGroupsAsync();


                var existingGroup = groups.FirstOrDefault(u => u.GroupID == group.GroupID);

                if (existingGroup != null)
                {

                    existingGroup.GroupName = group.GroupName;


                    await SaveGroupsAsync(groups);
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

        public async Task<List<Group>> LoadGroupsAsync()
        {
            try
            {
                if (!File.Exists(groupFilePath))
                {
                    return new List<Group>();
                }

                var json = await File.ReadAllTextAsync(groupFilePath);
                return JsonSerializer.Deserialize<List<Group>>(json) ?? new List<Group>();
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON deserialization error: {jsonEx.Message}");
                return new List<Group>();
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"I/O error while loading users: {ioEx.Message}");
                return new List<Group>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while loading users: {ex.Message}");
                return new List<Group>();
            }
        }

        public async Task RemoveGroupAsync(Group group)
        {
            try
            {
                var groups = await LoadGroupsAsync();
                var groupToRemove = groups.FirstOrDefault(u => u.GroupID == group.GroupID);

                if (groupToRemove != null)
                {
                    groups.Remove(groupToRemove);


                    await SaveGroupsAsync(groups);
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