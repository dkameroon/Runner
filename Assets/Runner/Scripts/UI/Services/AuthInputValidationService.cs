using System.Text.RegularExpressions;

public class AuthInputValidationService
{
    private const int MinLoginLength = 3;
    private const int MinPasswordLength = 6;

    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled);

    public bool TryValidateSignIn(
        string email,
        string password,
        out string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            errorMessage = "Email is required.";
            return false;
        }

        if (IsEmailValid(email) == false)
        {
            errorMessage = "Email is invalid.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            errorMessage = "Password is required.";
            return false;
        }

        if (password.Length < MinPasswordLength)
        {
            errorMessage = $"Password must be at least {MinPasswordLength} characters.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    public bool TryValidateSignUp(
        string email,
        string login,
        string password,
        string confirmPassword,
        out string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            errorMessage = "Email is required.";
            return false;
        }

        if (IsEmailValid(email) == false)
        {
            errorMessage = "Email is invalid.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(login))
        {
            errorMessage = "Nickname is required.";
            return false;
        }

        if (login.Trim().Length < MinLoginLength)
        {
            errorMessage = $"Login must be at least {MinLoginLength} characters.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            errorMessage = "Password is required.";
            return false;
        }

        if (password.Length < MinPasswordLength)
        {
            errorMessage = $"Password must be at least {MinPasswordLength} characters.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(confirmPassword))
        {
            errorMessage = "Confirm password is required.";
            return false;
        }

        if (password != confirmPassword)
        {
            errorMessage = "Passwords do not match.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    private static bool IsEmailValid(string email)
    {
        return EmailRegex.IsMatch(email.Trim());
    }
}