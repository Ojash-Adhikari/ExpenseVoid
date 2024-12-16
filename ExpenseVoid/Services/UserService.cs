using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ExpenseVoid.Interface;
using ExpenseVoid.Models;


namespace ExpenseVoid.Services
{
    public class UserService : IUser
    {
        private readonly string usersFilePath = Path.Combine(AppContext.BaseDirectory, "ExpenseVoid","Account","UserDetails.json");

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public bool ValidatePassword(string storedPassword, string providedPassword)
        {
            bool Status = false;
            string hashedPassword = HashPassword(providedPassword);
            Console.WriteLine(hashedPassword);
            if (storedPassword == hashedPassword)
            {
                Status = true;
            }
            else
            {
                Status = false;
            }
            
            return Status;
        }

        public async Task SaveUserAsync(User user)
        {
            try
            {
                if (user.UserID == Guid.Empty)
                {
                    user.UserID = Guid.NewGuid();
                }
                user.Password = HashPassword(user.Password);
                var users = await LoadUsersAsync();
                users.Add(user);
                await SaveUsersAsync(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving user: {ex.Message}");
                throw;
            }
        }

        public async Task RemoveUserAsync(User user)
        {
            try
            {
                var users = await LoadUsersAsync();
                var userToRemove = users.FirstOrDefault(u => u.UserID == user.UserID);

                if (userToRemove != null)
                {
                    users.Remove(userToRemove);


                    await SaveUsersAsync(users);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"User Not Removed: {ex.Message}");
                throw;
            }
        }

        public async Task<List<User>> LoadUsersAsync()
        {
            try
            {
                if (!File.Exists(usersFilePath))
                {
                    return new List<User>();
                }

                var json = await File.ReadAllTextAsync(usersFilePath);
                return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON deserialization error: {jsonEx.Message}");
                return new List<User>();
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"I/O error while loading users: {ioEx.Message}");
                return new List<User>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while loading users: {ex.Message}");
                return new List<User>();
            }
        }

        private async Task SaveUsersAsync(List<User> users)
        {
            try
            {
                var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });

                await File.WriteAllTextAsync(usersFilePath, json);
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

        public async Task EditUserAsync(User updatedUser)
        {
            try
            {
                // Load existing users from the JSON file
                var users = await LoadUsersAsync();

                // Find the user to edit
                var existingUser = users.FirstOrDefault(u => u.UserID == updatedUser.UserID);

                if (existingUser != null)
                {
                    // Update the user's properties
                    existingUser.FirstName = updatedUser.FirstName;
                    existingUser.LastName = updatedUser.LastName;
                    existingUser.PermanentAddress = updatedUser.PermanentAddress;
                    existingUser.TemporaryAddress = updatedUser.TemporaryAddress;
                    existingUser.Currency = updatedUser.Currency;
                    existingUser.Phone = updatedUser.Phone;
                    existingUser.Gender = updatedUser.Gender;
                    // Add other fields to update as needed

                    // Save the updated list back to the JSON file
                    await SaveUsersAsync(users);
                }
                else
                {
                    throw new Exception("User not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error editing user: {ex.Message}");
                throw;
            }
        }

    }
}