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
    public class ProfileService : IProfile
    {
        private readonly string profileFilePath = Path.Combine(AppContext.BaseDirectory, "ExpenseVoid", "Account", "Profile.json");
        public async Task SaveProfileAsync(Profile profile)
        {
            try
            {
                if (profile.ProfileID == Guid.Empty)
                {
                    profile.ProfileID = Guid.NewGuid();
                }
                var profiles = await LoadProfilesAsync();
                profiles.Add(profile);
                await SaveProfilesAsync(profiles);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving user: {ex.Message}");
                throw;
            }
        }
        private async Task SaveProfilesAsync(List<Profile> profile)
        {
            try
            {
                var json = JsonSerializer.Serialize(profile, new JsonSerializerOptions { WriteIndented = true });

                await File.WriteAllTextAsync(profileFilePath, json);
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"I/O error while loading users: {ioEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while saving users: {ex.Message}");
                throw;
            }
        }
        public async Task EditProfileAsync(Profile profile)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Profile>> LoadProfilesAsync()
        {
            try
            {
                if (!File.Exists(profileFilePath))
                {
                    return new List<Profile>();
                }

                var json = await File.ReadAllTextAsync(profileFilePath);
                return JsonSerializer.Deserialize<List<Profile>>(json) ?? new List<Profile>();
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON deserialization error: {jsonEx.Message}");
                return new List<Profile>();
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"I/O error while loading users: {ioEx.Message}");
                return new List<Profile>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while loading users: {ex.Message}");
                return new List<Profile>();
            }
        }

        public async Task RemoveProfileAsync(Profile profile)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Profile>> GetProfileByUserIdAsync(Guid userId)
        {
            try
            {
                var groups = await LoadProfilesAsync();
                return groups.Where(t => t.User?.UserID == userId).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching Profile for user ID {userId}: {ex.Message}");
                throw;
            }
        }
    }
}
