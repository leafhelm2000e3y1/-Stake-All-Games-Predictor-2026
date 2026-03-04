using LibraryManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagementSystem.Services
{
    public class LibraryService
    {
        private List<Book> _books = new();
        private List<User> _users = new();
        private List<Loan> _loans = new();
        private int _nextBookId = 1;
        private int _nextUserId = 1;
        private int _nextLoanId = 1;

        // Book operations
        public void AddBook(string title, string author, string isbn, string genre, int year)
        {
            _books.Add(new Book
            {
                Id = _nextBookId++,
                Title = title,
                Author = author,
                ISBN = isbn,
                Genre = genre,
                PublicationYear = year
            });
            Console.WriteLine($"Book '{title}' added successfully.");
        }

        public Book? FindBookByTitle(string title) =>
            _books.FirstOrDefault(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

        public Book? FindBookById(int id) => _books.FirstOrDefault(b => b.Id == id);

        public IEnumerable<Book> GetAllBooks() => _books;
        public IEnumerable<Book> GetAvailableBooks() => _books.Where(b => b.IsAvailable);

        // User operations
        public void AddUser(string name, string email)
        {
            _users.Add(new User
            {
                Id = _nextUserId++,
                Name = name,
                Email = email
            });
            Console.WriteLine($"User '{name}' added successfully.");
        }

        public User? FindUserById(int id) => _users.FirstOrDefault(u => u.Id == id);
        public IEnumerable<User> GetAllUsers() => _users;

        // Loan operations
        public bool LoanBook(int bookId, int userId)
        {
            var book = FindBookById(bookId);
            var user = FindUserById(userId);

            if (book == null || !book.IsAvailable)
            {
                Console.WriteLine("Book not available for loan.");
                return false;
            }

            if (user == null)
            {
                Console.WriteLine("User not found.");
                return false;
            }

            book.IsAvailable = false;
            book.DueDate = DateTime.Now.AddDays(14);

            _loans.Add(new Loan
            {
                LoanId = _nextLoanId++,
                BookId = bookId,
                UserId = userId,
                DueDate = book.DueDate.Value
            });

            user.BorrowedBookIds.Add(bookId);
            Console.WriteLine($"Book '{book.Title}' loaned to {user.Name}.");
            return true;
        }

        public bool ReturnBook(int bookId)
        {
            var book = FindBookById(bookId);
            if (book == null || book.IsAvailable)
            {
                Console.WriteLine("Invalid book ID or book already returned.");
                return false;
            }

            var loan = _loans.LastOrDefault(l => l.BookId == bookId && !l.ReturnDate.HasValue);
            if (loan != null)
            {
                loan.ReturnDate = DateTime.Now;
                var user = _users.First(u => u.Id == loan.UserId);
                user.BorrowedBookIds.Remove(bookId);
            }

            book.IsAvailable = true;
            book.DueDate = null;
            Console.WriteLine($"Book '{book.Title}' returned successfully.");
            return true;
        }

        // Search and reporting
        public IEnumerable<Book> SearchBooks(string query) =>
            _books.Where(b =>
                b.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                b.Author.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                b.Genre.Contains(query, StringComparison.OrdinalIgnoreCase));

        public IEnumerable<Loan> GetOverdueLoans() =>
            _loans.Where(l => l.IsOverdue && !l.ReturnDate.HasValue);

        public void GenerateReport()
        {
            Console.WriteLine("
=== LIBRARY REPORT ===");
            Console.WriteLine($"Total books: {_books.Count}");
            Console.WriteLine($"Available books: {GetAvailableBooks().Count()}");
            Console.WriteLine($"Registered users: {_users.Count}");
            Console.WriteLine($"Active loans: {_loans.Count(l => !l.ReturnDate.HasValue)}");
            Console.WriteLine($"Overdue loans: {GetOverdueLoans().Count()}");
        }
    }
}
