using System;

namespace TaskManagementSystem.Utilities
{
    public static class DateValidator
    {
        public static bool IsValidDate(string input, out DateTime result)
        {
            return DateTime.TryParse(input, out result) && result >= DateTime.Now.Date;
        }

        public static DateTime GetValidDateInput(string prompt)
        {
            DateTime date;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? string.Empty;
                if (IsValidDate(input, out date))
                    return date;
                Console.WriteLine("Invalid date format or past date. Please enter a valid future date (YYYY-MM-DD).");
            }
        }
    }
}
