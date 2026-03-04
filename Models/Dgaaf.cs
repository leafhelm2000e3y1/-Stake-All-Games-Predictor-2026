using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace FitnessTracker
{
    // Models
    public class Workout
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public List<Exercise> Exercises { get; set; } = new();
        public double TotalCaloriesBurned => Exercises.Sum(e => e.CaloriesBurned);
        public TimeSpan TotalDuration => TimeSpan.FromMinutes(Exercises.Sum(e => e.DurationMinutes));
    }

    public class Exercise
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // Cardio, Strength, Yoga, etc.
        public double WeightUsedKg { get; set; } // for strength training
        public int Repetitions { get; set; }
        public int Sets { get; set; }
        public double DurationMinutes { get; set; } // for cardio
        public double CaloriesBurned { get; set; }

        public void CalculateCalories()
        {
            if (Type.ToLower().Contains("cardio"))
            {
                CaloriesBurned = DurationMinutes * 8; // 8 cal/min estimate
            }
            else if (Type.ToLower().Contains("strength"))
            {
                CaloriesBurned = (WeightUsedKg * Repetitions * Sets) * 0.1;
            }
            else
            {
                CaloriesBurned = DurationMinutes * 5; // general estimate
            }
        }
    }

    // Main application class
    public class FitnessTrackerApp
    {
        private List<Workout> _workouts = new();
        private int _nextWorkoutId = 1;
        private const string DataFile = "fitness_data.json";

        public FitnessTrackerApp()
        {
            LoadData();
        }

        // Main menu loop
        public void Run()
        {
            DisplayWelcome();

            while (true)
            {
                DisplayMenu();
                string choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1": AddWorkout(); break;
            case "2": ViewAllWorkouts(); break;
            case "3": ViewWorkoutDetails(); break;
            case "4": SearchWorkoutsByDate(); break;
            case "5": ShowMonthlySummary(); break;
            case "6": ShowTopExercises(); break;
            case "7": SaveAndExit(); return;
            default:
                Console.WriteLine("Invalid choice. Please enter 1–7.
");
                break;
        }
    }
}

private void DisplayWelcome()
{
    Console.Clear();
    Console.WriteLine("=================================================");
    Console.WriteLine("          FITNESS TRACKER APPLICATION");
    Console.WriteLine("=================================================");
    Console.WriteLine();
}

private void DisplayMenu()
{
    Console.WriteLine("
MAIN MENU:");
    Console.WriteLine("1. Add New Workout");
    Console.WriteLine("2. View All Workouts");
    Console.WriteLine("3. View Workout Details");
    Console.WriteLine("4. Search Workouts by Date");
    Console.WriteLine("5. Monthly Summary");
    Console.WriteLine("6. Top Exercises");
    Console.WriteLine("7. Save and Exit");
    Console.Write("
Enter your choice (1–7): ");
}

// Add new workout with exercises
private void AddWorkout()
{
    Console.WriteLine("
--- ADD NEW WORKOUT ---");
    Console.Write("Workout name: ");
    string name = Console.ReadLine() ?? "Unnamed Workout";

    var workout = new Workout
    {
        Id = _nextWorkoutId++,
        Name = name,
        Date = GetDateInput("Workout date (YYYY-MM-DD): ")
    };

    Console.WriteLine("Add exercises (enter 'done' to finish):");

    while (true)
    {
        Console.Write("Exercise name (or 'done' to finish): ");
        string exerciseName = Console.ReadLine()?.Trim() ?? string.Empty;

        if (exerciseName.ToLower() == "done") break;

        var exercise = CreateExercise(exerciseName);
        workout.Exercises.Add(exercise);
    }

    _workouts.Add(workout);
    Console.WriteLine($"Workout '{name}' added with {workout.Exercises.Count} exercises.
");
}

private Exercise CreateExercise(string name)
{
    var exercise = new Exercise { Name = name };

    Console.Write($"Exercise type (Cardio/Strength/Yoga): ");
    exercise.Type = Console.ReadLine()?.Trim() ?? "General";

    if (exercise.Type.ToLower().Contains("strength"))
    {
        exercise.WeightUsedKg = GetDoubleInput("Weight used (kg): ");
        exercise.Repetitions = GetIntInput("Repetitions: ");
        exercise.Sets = GetIntInput("Sets: ");
    }
    else
    {
        exercise.DurationMinutes = GetDoubleInput("Duration (minutes): ");
    }

    exercise.CalculateCalories();
    return exercise;
}

// View all workouts
private void ViewAllWorkouts()
{
    if (_workouts.Count == 0)
    {
        Console.WriteLine("No workouts found.
");
        return;
    }

    Console.WriteLine("
=== ALL WORKOUTS ===");
    foreach (var workout in _workouts.OrderByDescending(w => w.Date))
    {
        Console.WriteLine($"{workout.Id}. {workout.Name} - {workout.Date:yyyy-MM-dd}");
        Console.WriteLine($"   Duration: {workout.TotalDuration:mm} min, Calories: {workout.TotalCaloriesBurned:F1}");
    }
    Console.WriteLine();
}

// View detailed workout info
private void ViewWorkoutDetails()
{
    int id = GetIntInput("Enter workout ID: ");
    var workout = _workouts.FirstOrDefault(w => w.Id == id);

    if (workout == null)
    {
        Console.WriteLine("Workout not found.
");
        return;
    }

    Console.WriteLine($"
=== WORKOUT DETAILS: {workout.Name} ===");
    Console.WriteLine($"Date: {workout.Date:yyyy-MM-dd}");
    Console.WriteLine($"Total duration: {workout.TotalDuration:mm} minutes");
    Console.WriteLine($"Total calories burned: {workout.TotalCaloriesBurned:F1}");

    Console.WriteLine("
Exercises:");
    foreach (var exercise in workout.Exercises)
    {
        Console.WriteLine($"  {exercise.Name} ({exercise.Type}):");
        Console.WriteLine($"    Calories: {exercise.CaloriesBurned:F1}, Duration: {exercise.DurationMinutes} min");
        if (exercise.WeightUsedKg > 0)
            Console.WriteLine($"    Weight: {exercise.WeightUsedKg}kg, Reps: {exercise.Repetitions}, Sets: {exercise.Sets}");
    }
    Console.WriteLine();
}

// Search workouts by date
private void SearchWorkoutsByDate()
{
    DateTime date = GetDateInput("Enter date to search (YYYY-MM-DD): ");
    var found = _workouts.Where(w => w.Date.Date == date.Date).ToList();

    if (found.Count == 0)
    {
        Console.WriteLine("No workouts found for this date.
");
        return;
    }

    Console.WriteLine($"
Workouts on {date:yyyy-MM-dd}:");
    foreach (var workout in found)
    {
        Console.WriteLine($"- {workout.Name}: {workout.TotalCaloriesBurned:F1} cal");
    }
    Console.WriteLine();
}

// Monthly summary
private void ShowMonthlySummary()
{
    if (_workouts.Count == 0) return;

    var monthly = _workouts
        .GroupBy(w => new { w.Date.Year, w.Date.Month })
        .Select(g =>
                            new
            {
                Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                TotalWorkouts = g.Count(),
                TotalCalories = g.Sum(w => w.TotalCaloriesBurned),
                TotalDuration = g.Sum(w => w.TotalDuration.TotalMinutes)
            })
        .OrderByDescending(x => x.Month)
        .ToList();

    Console.WriteLine("
=== MONTHLY SUMMARY ===");
    foreach (var month in monthly)
    {
        Console.WriteLine($"{month.Month}: {month.TotalWorkouts} workouts, " +
                          $"{month.TotalCalories:F1} cal, " +
                          $"{month.TotalDuration:F0} min");
    }
    Console.WriteLine();
}

// Top exercises by frequency
private void ShowTopExercises()
{
    var topExercises = _workouts
        .SelectMany(w => w.Exercises)
        .GroupBy(e => e.Name)
        .Select(g => new
        {
            Name = g.Key,
            Count = g.Count(),
            TotalCalories = g.Sum(e => e.CaloriesBurned)
        })
        .OrderByDescending(x => x.Count)
        .Take(5)
        .ToList();

    if (topExercises.Count == 0)
    {
        Console.WriteLine("No exercises found.
");
        return;
    }

    Console.WriteLine("
=== TOP 5 EXERCISES ===");
    for (int i = 0; i < topExercises.Count; i++)
    {
        var ex = topExercises[i];
        Console.WriteLine($"{i + 1}. {ex.Name}: {ex.Count} times, {ex.TotalCalories:F1} total cal");
    }
    Console.WriteLine();
}

// Input helpers
private DateTime GetDateInput(string prompt)
{
    DateTime date;
    while (true)
    {
        Console.Write(prompt);
        string input = Console.ReadLine() ?? string.Empty;
        if (DateTime.TryParse(input, out date))
            return date;
        Console.WriteLine("Invalid date format. Please use YYYY-MM-DD.");
    }
}

private double GetDoubleInput(string prompt)
{
    double value;
    while (true)
    {
        Console.Write(prompt);
        string input = Console.ReadLine() ?? string.Empty;
        if (double.TryParse(input, out value) && value >= 0)
            return value;
        Console.WriteLine("Please enter a valid positive number.");
    }
}

private int GetIntInput(string prompt)
{
    int value;
    while (true)
    {
        Console.Write(prompt);
        string input = Console.ReadLine() ?? string.Empty;
        if (int.TryParse(input, out value) && value > 0)
            return value;
        Console.WriteLine("Please enter a valid positive integer.");
    }
}

// Save and exit
private void SaveAndExit()
{
    SaveData();
    Console.WriteLine("Fitness Tracker data saved. Thank you for using the application!
");
}

private void SaveData()
{
    try
    {
        string json = JsonConvert.SerializeObject(_workouts, Formatting.Indented);
        File.WriteAllText(DataFile, json);
        Console.WriteLine("Data saved successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error saving data: {ex.Message}");
    }
}

private void LoadData()
{
    if (!File.Exists(DataFile))
    {
        Console.WriteLine("No existing fitness data found. Starting with empty tracker.");
        return;
    }

    try
    {
        string json = File.ReadAllText(DataFile);
        _workouts = JsonConvert.DeserializeObject<List<Workout>>(json) ?? new();
        if (_workouts.Count > 0)
            _nextWorkoutId = _workouts.Max(w => w.Id) + 1;
        Console.WriteLine($"Loaded {_workouts.Count} workouts from data file.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading data: {ex.Message}. Starting with empty tracker.");
        _workouts = new();
    }
}
}

// Entry point
public class Program
{
    public static void Main(string[] args)
    {
        var app = new FitnessTrackerApp();
        app.Run();
    }
}
}
