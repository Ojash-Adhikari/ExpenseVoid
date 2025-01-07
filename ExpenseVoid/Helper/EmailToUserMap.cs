using ExpenseVoid.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseVoid.Models;
using ExpenseVoid.Services;
using ExpenseVoid.Interface;

namespace ExpenseVoid.Helper
{
    public class EmailToUserMap
    {
        private readonly IUser _userService;

        // Constructor for dependency injection
        public EmailToUserMap(IUser userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task<User?> GetUserByEmailAsync(PreferencesStoreClone storage)
        {
            try
            {
                // Retrieve the email from storage
                string email = storage.Get<string>("Email");

                if (string.IsNullOrEmpty(email))
                {
                    Console.WriteLine("No email found in storage.");
                    return null;
                }

                // Load all users
                var users = await _userService.LoadUsersAsync();

                // Find the user with the matching email
                var user = users.FirstOrDefault(u => u.Email == email);

                if (user != null)
                {
                    Console.WriteLine($"User found: {user.FirstName} {user.LastName}");
                }
                else
                {
                    Console.WriteLine("User not found.");
                }

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user by email: {ex.Message}");
                return null;
            }
        }
    }
}
