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
    public class TransactionTypeService : ITransactionType
    {
        private readonly string transactiontypeFilePath = Path.Combine(AppContext.BaseDirectory, "ExpenseVoid", "Transaction", "TransactionType.json");
        public async Task SaveTransactionTypeAsync(TransactionType transactionType)
        {
            try
            {
                if(transactionType.TypeID == Guid.Empty)
                {
                    transactionType.TypeID = Guid.NewGuid();
                }
                var transactionTypes = await LoadTransactionTypesAsync();
                transactionTypes.Add(transactionType);
                await SaveTransactionTypesAsync(transactionTypes);
            }
            catch (Exception ex)
            { 
                Console.WriteLine($"Error Saving Target : {ex.Message}");
            }
        }
        private async Task SaveTransactionTypesAsync(List<TransactionType> transactionType)
        {
            try
            {
                var json = JsonSerializer.Serialize(transactionType, new JsonSerializerOptions { WriteIndented = true });

                await File.WriteAllTextAsync(transactiontypeFilePath, json);
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
        public async Task EditTransactionTypeAsync(TransactionType transactionType)
        {
            //try
            //{
            //    var transactionTypes = await LoadTransactionTypesAsync();
            //    var exisitingTransactionTypes = transactionTypes.FirstOrDefault(u => u.TypeID == transactionType.TypeID);
            //    if (exisitingTransactionTypes != null) 
            //    { 
            //        exisitingTransactionTypes.Name = transactionType.Name;

            //        await SaveTransactionTypesAsync(transactionTypes);
            //    }
            //    else
            //    {
            //        throw new Exception("Target Not Found");
            //    }

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error Editing Target {ex.Message}");
            //}
        }

        public async Task<List<TransactionType>> LoadTransactionTypesAsync()
        {
            try
            {
                if (!File.Exists(transactiontypeFilePath))
                {
                    return new List<TransactionType>();
                }

                var json = await File.ReadAllTextAsync(transactiontypeFilePath);
                return JsonSerializer.Deserialize<List<TransactionType>>(json) ?? new List<TransactionType>();
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON deserialization error: {jsonEx.Message}");
                return new List<TransactionType>();
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"I/O error while loading users: {ioEx.Message}");
                return new List<TransactionType>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while loading users: {ex.Message}");
                return new List<TransactionType>();
            }
        }

        public async Task RemoveTransactionTypeAsync(TransactionType transactionType)
        {
            try
            {
                var transactionTypes = await LoadTransactionTypesAsync();
                var transactionTypeToRemove = transactionTypes.FirstOrDefault(u => u.TypeID == transactionType.TypeID);

                if (transactionTypeToRemove != null)
                {
                    transactionTypes.Remove(transactionTypeToRemove);


                    await SaveTransactionTypesAsync(transactionTypes);
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
