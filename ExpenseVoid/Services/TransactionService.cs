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
    public class TransactionService : ITransaction
    {
        private readonly string transactionFilePath = Path.Combine(AppContext.BaseDirectory, "ExpenseVoid", "Transaction", "Transaction.json");
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

    }
}
