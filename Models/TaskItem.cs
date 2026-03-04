using System;

namespace TaskManagementSystem.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? CompletedAt { get; set; }

        public override string ToString() =>
            $"ID: {Id}, Title: '{Title}', Due: {DueDate:yyyy-MM-dd}, " +
            $"Priority: {Priority}, Completed: {IsCompleted}";
    }
}
