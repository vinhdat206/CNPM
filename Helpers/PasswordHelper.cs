using System.Text.RegularExpressions;

namespace CNPMFastFood.Helpers
{
    public static class PasswordHelper
    {
        // Kiểm tra password mạnh
        public static bool IsStrongPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            string pattern =
                @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_]).{8,}$";

            return Regex.IsMatch(password, pattern);
        }

        // Kiểm tra confirm password
        public static bool IsMatch(
            string password,
            string confirmPassword)
        {
            return password == confirmPassword;
        }
    }
}