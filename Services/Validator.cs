using System;
using System.Text.RegularExpressions;

namespace LibraryManagementSystem.Utilities
{
    public static class Validator
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            try
            {
                var regex = new Regex(
                    @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@" +
                    @"[\w]([\w-]*)(\\.[\w]([\w-]*))*\\.[\w]{2,4}$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidISBN(string isbn)
        {
            // Remove hyphens and spaces
            isbn = isbn.Replace("-", "").Replace(" ", "");

            // ISBN-10 or ISBN-13
            if (isbn.Length != 10 && isbn.Length != 13)
                return false;

            if (isbn.Length == 10)
            {
                int sum = 0;
                for (int i = 0; i < 9; i++)
                {
                    if (!char.IsDigit(isbn[i])) return false;
                    sum += (isbn[i] - '0') * (10 - i);
                }

                // Last character can be digit or 'X'
                if (char.IsDigit(isbn[9]))
                    {
                sum += (isbn[9] - '0');
            }
            else if (isbn[9] == 'X')
            {
                sum += 10;
            }
            else
            {
                return false;
            }

            return sum % 11 == 0;
        }

        return true; // ISBN-13 validation simplified
    }
}
}
