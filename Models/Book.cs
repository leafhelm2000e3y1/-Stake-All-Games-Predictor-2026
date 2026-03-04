using System;

namespace LibraryManagementSystem.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
        public bool IsAvailable { get; set; } = true;
        public DateTime? DueDate { get; set; }

        public override string ToString() =>
            $"ID: {Id}, Title: '{Title}', Author: {Author}, ISBN: {ISBN}, " +
            $"Genre: {Genre}, Year: {PublicationYear}, Available: {IsAvailable}";
    }
}
