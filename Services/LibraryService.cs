using LibraryManagementSystem.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace LibraryManagementSystem.Services
{
    public class FileService
    {
        private const string DataFilePath = "Data/library_data.json";

        public static void SaveData(List<Book> books, List<User> users, List<Loan> loans)
        {
            var data = new
            {
                Books = books,
                Users = users,
                Loans = loans
            };

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(DataFilePath, json);
            Console.WriteLine("Data saved successfully.");
        }

        public static (List<Book>, List<User>, List<Loan>) LoadData()
        {
            if (!File.Exists(DataFilePath))
            {
                Console.WriteLine("No existing data found. Starting with empty library.");
                return (new List<Book>(), new List<User>(), new List<Loan>());
            }

            try
            {
                string json = File.ReadAllText(DataFilePath);
                var data = JsonConvert.DeserializeObject<LibraryData>(json);
                return (data.Books ?? new List<Book>(),
                       data.Users ?? new List<User>(),
                       data.Loans ?? new List<Loan>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                return (new List<Book>(), new List<User>(), new List<Loan>());
            }
        }

        private class LibraryData
        {
            public List<Book> Books { get; set; } = new();
            public List<User> Users { get; set; } = new();
            public List<Loan> Loans { get; set; } = new();
        }
    }
}
