namespace LibraryManagementSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<int> BorrowedBookIds { get; set; } = new();

        public override string ToString() =>
            $"ID: {Id}, Name: {Name}, Email: {Email}, " +
            $"Borrowed books: {BorrowedBookIds.Count}";
    }
}
