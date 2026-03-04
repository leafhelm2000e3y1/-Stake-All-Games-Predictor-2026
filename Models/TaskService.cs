using TaskManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace TaskManagementSystem.Services
{
    public class TaskService
    {
        private List<TaskItem> _tasks = new();
        private int _nextId = 1;
        private const string DataFilePath = "Data/tasks_data.json";

        public TaskService()
        {
            LoadData();
        }

        public void AddTask(string title, string description, DateTime dueDate, Priority priority)
        {
            _tasks.Add(new TaskItem
            {
                Id = _nextId++,
                Title = title,
                Description = description,
                DueDate = dueDate,
                Priority = priority
            });
            Console.WriteLine($"Task '{title}' added successfully.");
        }

        public bool CompleteTask(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null && !task.IsCompleted)
            {
                task.IsCompleted = true;
                task.CompletedAt = DateTime.Now;
                Console.WriteLine($"Task '{task.Title}' marked as completed.");
                return true;
            }
            Console.WriteLine("Task not found or already completed.");
            return false;
        }

        public IEnumerable<TaskItem> GetAllTasks() => _tasks;
        public IEnumerable<TaskItem> GetPendingTasks() => _tasks.Where(t => !t.IsCompleted);
        public IEnumerable<TaskItem> GetCompletedTasks() => _tasks.Where(t => t.IsCompleted);

        public IEnumerable<TaskItem> FilterByPriority(Priority priority) =>
            _tasks.Where(t => t.Priority == priority);

        public IEnumerable<TaskItem> SearchTasks(string query) =>
            _tasks.Where(t =>
                t.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                t.Description.Contains(query, StringComparison.OrdinalIgnoreCase));

        public void SaveData()
        {
            string json = JsonConvert.SerializeObject(_tasks, Formatting.Indented);
            File.WriteAllText(DataFilePath, json);
            Console.WriteLine("Tasks saved successfully.");
        }

        private void LoadData()
        {
            if (!File.Exists(DataFilePath))
            {
                Console.WriteLine("No existing tasks found. Starting with empty list.");
                return;
            }

            try
            {
                string json = File.ReadAllText(DataFilePath);
                _tasks = JsonConvert.DeserializeObject<List<TaskItem>>(json) ?? new();
                if (_tasks.Count > 0)
                    _nextId = _tasks.Max(t => t.Id) + 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading tasks: {ex.Message}");
            }
        }
    }
}
