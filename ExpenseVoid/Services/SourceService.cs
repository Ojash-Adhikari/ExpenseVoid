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
    public class SourceService : ISource
    {
        private readonly string sourceFilePath = Path.Combine(AppContext.BaseDirectory,"Tags&Source","Source.json");
        public async Task SaveSourceAsync(Source source)
        {
            try
            {
                if (source.SourceId == Guid.Empty)
                {
                    source.SourceId = Guid.NewGuid();
                }
                var sources = await LoadSourceAsync();
                sources.Add(source);
                await SaveSourcesAsync(sources);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Saving Target : {ex.Message}");
            }
        }
        private async Task SaveSourcesAsync(List<Source> source)
        {
            try
            {
                var json = JsonSerializer.Serialize(source, new JsonSerializerOptions { WriteIndented = true });

                await File.WriteAllTextAsync(sourceFilePath, json);
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
        public async Task EditSourceAsync(Source source)
        {
            try
            {
                var sources = await LoadSourceAsync();
                var exisitingSources = sources.FirstOrDefault(u => u.SourceId == source.SourceId);
                if (exisitingSources != null)
                {
                    exisitingSources.Name = source.Name;
                    exisitingSources.Description = source.Description;

                    await SaveSourcesAsync(sources);
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

        public async Task<List<Source>> LoadSourceAsync()
        {
            try
            {
                if (!File.Exists(sourceFilePath))
                {
                    return new List<Source>();
                }

                var json = await File.ReadAllTextAsync(sourceFilePath);
                return JsonSerializer.Deserialize<List<Source>>(json) ?? new List<Source>();
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON deserialization error: {jsonEx.Message}");
                return new List<Source>();
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"I/O error while loading users: {ioEx.Message}");
                return new List<Source>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while loading users: {ex.Message}");
                return new List<Source>();
            }
        }

        public async Task RemoveSourceAsync(Source source)
        {
            try
            {
                var sources = await LoadSourceAsync();
                var sourcesToRemove = sources.FirstOrDefault(u => u.SourceId == source.SourceId);

                if (sourcesToRemove != null)
                {
                    sources.Remove(sourcesToRemove);


                    await SaveSourcesAsync(sources);
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
