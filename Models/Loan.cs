using System;

namespace LibraryManagementSystem.Models
{
    public class Loan
    {
        public int LoanId { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime LoanDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(14);
        public DateTime? ReturnDate { get; set; }

        public bool IsOverdue => !ReturnDate.HasValue && DateTime.Now > DueDate;
    }
}
