using ExpenseVoid.Interface;
using ExpenseVoid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ExpenseVoid.Services
{
    public class TransactionService : ITransaction
    {
        private readonly string transactionFilePath = Path.Combine(AppContext.BaseDirectory, "ExpenseVoid", "Transaction", "Transaction.json");
        private readonly string usersFilePath = Path.Combine(AppContext.BaseDirectory, "ExpenseVoid", "Account", "UserDetails.json");

        public async Task SaveTransactionAsync(Transaction transaction)
        {
            try
            {
                if (transaction.TransactionID == Guid.Empty)
                {
                    transaction.TransactionID = Guid.NewGuid();
                }
                var transactions = await LoadTransactionAsync();
                transactions.Add(transaction);
                await SaveTransactionsAsync(transactions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Saving Target : {ex.Message}");
            }
        }
        private async Task SaveTransactionsAsync(List<Transaction> transaction)
        {
            try
            {
                var json = JsonSerializer.Serialize(transaction, new JsonSerializerOptions { WriteIndented = true });

                await File.WriteAllTextAsync(transactionFilePath, json);
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"I/O error while loading Target: {ioEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while saving Target: {ex.Message}");
                throw;
            }
        }
        public async Task EditTransactionAsync(Transaction transaction)
        {
            try
            {
                var transactions = await LoadTransactionAsync();
                var exisitingTransactions = transactions.FirstOrDefault(u => u.TransactionID == transaction.TransactionID);
                if (exisitingTransactions != null)
                {
                    exisitingTransactions.Tag = exisitingTransactions.Tag;
                    exisitingTransactions.TransactionAmount = transaction.TransactionAmount;
                    exisitingTransactions.TransactionName = transaction.TransactionName;
                    exisitingTransactions.TransactionRemarks = transaction.TransactionRemarks;
                    exisitingTransactions.TransactionType = transaction.TransactionType;
                    exisitingTransactions.TransactionDate = transaction.TransactionDate;
                    exisitingTransactions.TransactionColor = transaction.TransactionColor;

                    await SaveTransactionsAsync(transactions);
                }
                else
                {
                    throw new Exception("Target Not Found");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Editing Target {ex.Message}");
            }
        }

        public async Task<List<Transaction>> LoadTransactionAsync()
        {
            try
            {
                if (!File.Exists(transactionFilePath))
                {
                    return new List<Transaction>();
                }

                var json = await File.ReadAllTextAsync(transactionFilePath);
                return JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON deserialization error: {jsonEx.Message}");
                return new List<Transaction>();
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"I/O error while loading users: {ioEx.Message}");
                return new List<Transaction>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while loading users: {ex.Message}");
                return new List<Transaction>();
            }
        }

        public async Task RemoveTransactionAsync(Transaction transaction)
        {
            try
            {
                var transactions = await LoadTransactionAsync();
                var transactionToRemove = transactions.FirstOrDefault(u => u.TransactionID == transaction.TransactionID);

                if (transactionToRemove != null)
                {
                    transactions.Remove(transactionToRemove);


                    await SaveTransactionsAsync(transactions);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Target Not Removed: {ex.Message}");
                throw;
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
        private async Task<List<User>> LoadUsersAsync()
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
        //Calculation part

        public async Task ProcessTransactionAsync(Transaction transaction)
        {
            try
            {
                if (transaction.User == null || transaction.TransactionType == null || transaction.TransactionAmount == null)
                {
                    throw new ArgumentException("Transaction must have a user, type, and amount.");
                }

                // Load the existing users
                var users = await LoadUsersAsync();
                var userToUpdate = users.FirstOrDefault(u => u.UserID == transaction.User.UserID);

                if (userToUpdate == null)
                {
                    throw new Exception($"User with ID {transaction.User.UserID} not found.");
                }

                // Process based on TransactionType
                if (transaction.TransactionType.Debit.HasValue)
                {
                    // Deduct balance for Debit transactions
                    userToUpdate.Balance -= transaction.TransactionAmount.Value;
                }
                else if (transaction.TransactionType.Credit.HasValue)
                {
                    // Add balance for Credit transactions
                    userToUpdate.Balance += transaction.TransactionAmount.Value;
                }

                // Update the user object in the transaction
                transaction.User = userToUpdate;

                // Save the updated user details back to the JSON file
                await SaveUsersAsync(users);

                // Save the transaction with the updated user details
                await SaveTransactionAsync(transaction);

                Console.WriteLine($"Transaction processed successfully. New balance for user {userToUpdate.UserName}: {userToUpdate.Balance}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing transaction: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Transaction>> GetTransactionsByUserIdAsync(Guid userId)
        {
            try
            {
                var transactions = await LoadTransactionAsync();
                return transactions.Where(t => t.User?.UserID == userId).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching transactions for user ID {userId}: {ex.Message}");
                throw;
            }
        }


        public async Task SaveTransactionsToExcelAsync(string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            try
            {
                var transactions = await LoadTransactionAsync();
                if (transactions == null || !transactions.Any())
                {
                    Console.WriteLine("No transactions available to export.");
                    return;
                }

                using var package = new ExcelPackage();
                var worksheet = package.Workbook.Worksheets.Add("Transactions");

                // Add headers
                worksheet.Cells[1, 1].Value = "Transaction ID";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Amount";
                worksheet.Cells[1, 4].Value = "Type";
                worksheet.Cells[1, 5].Value = "Date";
                worksheet.Cells[1, 6].Value = "Remarks";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Populate transaction data
                for (int i = 0; i < transactions.Count; i++)
                {
                    var transaction = transactions[i];
                    worksheet.Cells[i + 2, 1].Value = transaction.TransactionID.ToString();
                    worksheet.Cells[i + 2, 2].Value = transaction.TransactionName;
                    worksheet.Cells[i + 2, 3].Value = transaction.TransactionAmount;
                    worksheet.Cells[i + 2, 4].Value = transaction.TransactionType?.Debit != null ? "Debit" : "Credit";
                    worksheet.Cells[i + 2, 5].Value = transaction.TransactionDate.ToString();
                    worksheet.Cells[i + 2, 6].Value = transaction.TransactionRemarks;
                }

                // Auto-fit columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Save the Excel file to the specified path
                await File.WriteAllBytesAsync(filePath, package.GetAsByteArray());
                Console.WriteLine($"Excel file saved successfully at {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving Excel file: {ex.Message}");
            }
        }

    }
}
