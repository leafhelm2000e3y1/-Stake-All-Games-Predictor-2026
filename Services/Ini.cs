using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagementSystem
{
    public class Program
    {
        private static LibraryService _libraryService = new();

        static void Main(string[] args)
        {
            LoadData();
            DisplayWelcomeMessage();

            while (true)
            {
                DisplayMenu();
                string choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1": AddBookMenu(); break;
            case "2": AddUserMenu(); break;
            case "3": LoanBookMenu(); break;
            case "4": ReturnBookMenu(); break;
            case "5": SearchBooksMenu(); break;
            case "6": DisplayAllBooks(); break;
            case "7": DisplayAllUsers(); break;
            case "8": GenerateReport(); break;
            case "9": SaveAndExit(); return;
            default:
                Console.WriteLine("Invalid choice. Please try again.
");
                break;
        }
    }
}

private static void LoadData()
{
    var (books, users, loans) = FileService.LoadData();
    _libraryService._books = books;
    _libraryService._users = users;
    _libraryService._loans = loans;
}

private static void DisplayWelcomeMessage()
{
    Console.Clear();
    Console.WriteLine("=================================================");
    Console.WriteLine("      LIBRARY MANAGEMENT SYSTEM");
    Console.WriteLine("=================================================");
    Console.WriteLine();
}

private static void DisplayMenu()
{
    Console.WriteLine("
MAIN MENU:");
    Console.WriteLine("1. Add Book");
    Console.WriteLine("2. Add User");
    Console.WriteLine("3. Loan Book");
    Console.WriteLine("4. Return Book");
    Console.WriteLine("5. Search Books");
    Console.WriteLine("6. Display All Books");
    Console.WriteLine("7. Display All Users");
    Console.WriteLine("8. Generate Report");
    Console.WriteLine("9. Save and Exit");
    Console.Write("
Enter your choice (1-9): ");
}

private static void AddBookMenu()
{
    Console.WriteLine("
--- ADD NEW BOOK ---");
    Console.Write("Title: ");
    string title = Console.ReadLine() ?? string.Empty;

    Console.Write("Author: ");
    string author = Console.ReadLine() ?? string.Empty;

    Console.Write("ISBN: ");
    string isbn = Console.ReadLine() ?? string.Empty;
    if (!Validator.IsValidISBN(isbn))
    {
        Console.WriteLine("Invalid ISBN format!");
        return;
    }

    Console.Write("Genre: ");
    string genre = Console.ReadLine() ?? string.Empty;

    Console.Write("Publication Year: ");
    if (!int.TryParse(Console.ReadLine(), out int year) || year < 1000 || year > DateTime.Now.Year)
    {
        Console.WriteLine("Invalid year!");
        return;
    }

    _libraryService.AddBook(title, author, isbn, genre, year);
}

// Аналогично реализуются остальные методы меню: AddUserMenu(), LoanBookMenu(), ReturnBookMenu(), SearchBooksMenu(), DisplayAllBooks(), DisplayAllUsers(), GenerateReport(), SaveAndExit()

private static void SaveAndExit()
{
    FileService.SaveData(_libraryService._books, _libraryService._users, _libraryService._loans);
    Console.WriteLine("Thank you for using Library Management System!");
}
}
}
