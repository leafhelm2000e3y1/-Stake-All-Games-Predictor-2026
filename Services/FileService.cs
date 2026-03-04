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
                return (new List<
