using TaskManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagementSystem.Services
{
    public class NotificationService
    {
        private List<TaskItem> _tasks;

        public NotificationService(List<TaskItem> tasks)
        {
            _tasks = tasks;
        }

        public void CheckDueSoonTasks()
        {
            var soonDueTasks = _tasks.Where(t =>
                !t.IsCompleted &&
                t.DueDate <= DateTime.Now.AddDays(3) &&
                t.DueDate >= DateTime.Now).ToList();

            if (soonDueTasks.Count > 0)
            {
                Console.WriteLine("
=== URGENT TASKS ===");
                foreach (var task in soonDueTasks)
                {
                    int daysLeft = (task.DueDate - DateTime.Now).Days;
                    Console.WriteLine($"Task '{task.Title}' is due in {daysLeft} days!");
                }
            }
        }

        public void CheckOverdueTasks()
        {
            var overdueTasks = _tasks.Where(t =>
                !t.IsCompleted && t.DueDate < DateTime.Now).ToList();

            if (overdueTasks.Count > 0)
            {
                Console.WriteLine("
=== OVERDUE TASKS ===");
                foreach (var task in overdueTasks)
                {
                    int daysOverdue = (DateTime.Now - task.DueDate).Days;
                    Console.WriteLine($"Task '{task.Title}' is {daysOverdue} days overdue!");
                }
            }
        }
    }
}
