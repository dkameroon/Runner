using System;
using Zenject;

public class AuthFlowService : IInitializable, IDisposable
{
    private const int MinPasswordLength = 6;

    private readonly AuthWindow _authWindow;
    private readonly MainMenuWindow _mainMenuWindow;
    private readonly IAuthenticationService _authenticationService;

    public bool IsAuthScreenActive { get; private set; }

    public AuthFlowService(
        AuthWindow authWindow,
        MainMenuWindow mainMenuWindow,
        IAuthenticationService authenticationService)
    {
        _authWindow = authWindow;
        _mainMenuWindow = mainMenuWindow;
        _authenticationService = authenticationService;
    }

    public void Initialize()
    {
        _authWindow.SignInRequested += OnSignInRequested;
        _authWindow.SignUpRequested += OnSignUpRequested;
        IsAuthScreenActive = false;
    }

    public void Dispose()
    {
        _authWindow.SignInRequested -= OnSignInRequested;
        _authWindow.SignUpRequested -= OnSignUpRequested;
    }

    public void ShowAuthScreen()
    {
        IsAuthScreenActive = true;
        _mainMenuWindow.Hide();
        _authWindow.Show();
    }

    public void HideAuthScreen()
    {
        IsAuthScreenActive = false;
        _authWindow.Hide();
    }

    private async void OnSignInRequested(string email, string password)
    {
        if (TryValidateSignIn(email, password, out string errorMessage) == false)
        {
            _authWindow.SetError(errorMessage);
            return;
        }

        AuthOperationResultData result = await _authenticationService.SignInAsync(email, password);

        if (result.IsSuccess == false)
        {
            _authWindow.SetError(result.ErrorMessage);
            return;
        }

        HideAuthScreen();
        _mainMenuWindow.Show();
    }

    private async void OnSignUpRequested(string email, string login, string password, string confirmPassword)
    {
        if (TryValidateSignUp(email, login, password, confirmPassword, out string errorMessage) == false)
        {
            _authWindow.SetError(errorMessage);
            return;
        }

        AuthOperationResultData result = await _authenticationService.SignUpAsync(email, login, password, confirmPassword);

        if (result.IsSuccess == false)
        {
            _authWindow.SetError(result.ErrorMessage);
            return;
        }

        HideAuthScreen();
        _mainMenuWindow.Show();
    }

    private bool TryValidateSignIn(string email, string password, out string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            errorMessage = "Email is required.";
            return false;
        }

        if (email.Contains("@") == false)
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

    private bool TryValidateSignUp(
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

        if (email.Contains("@") == false)
        {
            errorMessage = "Email is invalid.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(login))
        {
            errorMessage = "Login is required.";
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
}