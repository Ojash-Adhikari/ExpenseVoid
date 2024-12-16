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
    public class DebtService : IDebt
    {
        private readonly string debtFilePath = Path.Combine(AppContext.BaseDirectory, "ExpeneVoid", "Debt", "Debt.json");
        public async Task SaveDebtAsync(Debt debt)
        {
            try
            {
                if (debt.DebtId == Guid.Empty)
                {
                    debt.DebtId = Guid.NewGuid();
                }
                var debts = await LoadDebtAsync();
                debts.Add(debt);
                await SaveDebtsAsync(debts);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Saving Target : {ex.Message}");
            }
        }
        private async Task SaveDebtsAsync(List<Debt> debt)
        {
            try
            {
                var json = JsonSerializer.Serialize(debt, new JsonSerializerOptions { WriteIndented = true });

                await File.WriteAllTextAsync(debtFilePath, json);
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
        public async Task EditDebtAsync(Debt debt)
        {
            try
            {
                var debts = await LoadDebtAsync();
                var exisitingDebts = debts.FirstOrDefault(u => u.DebtId == debt.DebtId);
                if (exisitingDebts != null)
                {
                    exisitingDebts.Source = debt.Source;
                    exisitingDebts.Amount = debt.Amount;
                    exisitingDebts.BorrowedDate = debt.BorrowedDate;
                    exisitingDebts.ReceivedDate = debt.ReceivedDate;
                    exisitingDebts.DueDate = debt.DueDate;
                    exisitingDebts.IsCleared = debt.IsCleared;
                    exisitingDebts.Notes = debt.Notes;
                    exisitingDebts.DebtColor = debt.DebtColor;

                    await SaveDebtsAsync(debts);
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

        public async Task<List<Debt>> LoadDebtAsync()
        {
            try
            {
                if (!File.Exists(debtFilePath))
                {
                    return new List<Debt>();
                }

                var json = await File.ReadAllTextAsync(debtFilePath);
                return JsonSerializer.Deserialize<List<Debt>>(json) ?? new List<Debt>();
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON deserialization error: {jsonEx.Message}");
                return new List<Debt>();
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"I/O error while loading users: {ioEx.Message}");
                return new List<Debt>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while loading users: {ex.Message}");
                return new List<Debt>();
            }
        }

        public async Task RemoveDebtAsync(Debt debt)
        {
            try
            {
                var debts = await LoadDebtAsync ();
                var debtToRemove = debts.FirstOrDefault(u => u.DebtId == debt.DebtId);

                if (debtToRemove != null)
                {
                    debts.Remove(debtToRemove);


                    await SaveDebtsAsync(debts);
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
