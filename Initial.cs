using System;

/// <summary>
/// Simple console calculator application supporting basic arithmetic operations.
/// Demonstrates user input handling, error validation, and loop control.
/// </summary>
public class CalculatorApp
{
    /// <summary>
    /// Main entry point of the application.
    /// Runs the calculator loop until user chooses to exit.
    /// </summary>
    public static void Main(string[] args)
    {
        Console.WriteLine("=== Simple Calculator ===");
        Console.WriteLine("Supported operations: +, -, *, /");
        Console.WriteLine("Type 'exit' to quit the application.");
        Console.WriteLine("----------------------------------------");

        bool continueCalculation = true;

        while (continueCalculation)
        {
            try
            {
                // Get user input
                double num1 = GetNumber("Enter first number: ");
                double num2 = GetNumber("Enter second number: ");

                char op = GetOperator();

                // Perform calculation
                double result = Calculate(num1, num2, op);

                // Display result
                Console.WriteLine($"Result: {num1} {op} {num2} = {result:F2}");
                Console.WriteLine();

                // Ask if user wants to continue
                continueCalculation = AskToContinue();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine("Please try again.
");
            }
        }

        Console.WriteLine("Calculator terminated. Goodbye!");
    }

    /// <summary>
    /// Safely reads a double value from user input with error handling.
    /// </summary>
    /// <param name="prompt">Message to display to the user</param>
    /// <returns>Valid double number</returns>
    private static double GetNumber(string prompt)
    {
        double number;
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();

            if (double.TryParse(input, out number))
            {
                return number;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }
    }

    /// <summary>
    /// Gets valid operator from user input.
    /// </summary>
    /// <returns>Valid operator character (+, -, *, /)</returns>
    private static char GetOperator()
    {
        while (true)
        {
            Console.Write("Enter operator (+, -, *, /): ");
            string input = Console.ReadLine();

            if (!string.IsNullOrEmpty(input) && "+-*/".Contains(input[0]))
            {
                return input[0];
            }
            else
            {
                Console.WriteLine("Invalid operator. Please use +, -, *, or /.");
            }
        }
    }

    /// <summary>
    /// Performs arithmetic operation based on provided operator and operands.
    /// </summary>
    /// <param name="a">First operand</param>
    /// <param name="b">Second operand</param>
    /// <param name="op">Operator (+, -, *, /)</param>
    /// <returns>Result of the operation</returns>
    /// <exception cref="DivideByZeroException">Thrown when attempting to divide by
