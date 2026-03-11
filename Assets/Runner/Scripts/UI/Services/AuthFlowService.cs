using System;
using System.Threading.Tasks;
using Zenject;

public class AuthFlowService : IInitializable, IDisposable
{
    private readonly AuthWindow _authWindow;
    private readonly MainMenuWindow _mainMenuWindow;
    private readonly IAuthenticationService _authenticationService;
    private readonly AuthInputValidationService _authInputValidationService;

    private bool _isRequestInProgress;

    public bool IsAuthScreenActive { get; private set; }

    public AuthFlowService(
        AuthWindow authWindow,
        MainMenuWindow mainMenuWindow,
        IAuthenticationService authenticationService,
        AuthInputValidationService authInputValidationService)
    {
        _authWindow = authWindow;
        _mainMenuWindow = mainMenuWindow;
        _authenticationService = authenticationService;
        _authInputValidationService = authInputValidationService;
    }

    public void Initialize()
    {
        _authWindow.SignInRequested += OnSignInRequested;
        _authWindow.SignUpRequested += OnSignUpRequested;

        IsAuthScreenActive = false;
        _isRequestInProgress = false;
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

    public void ShowMainMenu()
    {
        IsAuthScreenActive = false;
        _authWindow.Hide();
        _mainMenuWindow.Show();
    }

    public void HideAuthScreen()
    {
        IsAuthScreenActive = false;
        _authWindow.Hide();
    }

    private void OnSignInRequested(string email, string password)
    {
        _ = HandleSignInAsync(email, password);
    }

    private void OnSignUpRequested(string email, string login, string password, string confirmPassword)
    {
        _ = HandleSignUpAsync(email, login, password, confirmPassword);
    }

    private async Task HandleSignInAsync(string email, string password)
    {
        if (_isRequestInProgress)
        {
            return;
        }

        if (_authInputValidationService.TryValidateSignIn(email, password, out string errorMessage) == false)
        {
            _authWindow.SetError(errorMessage);
            return;
        }

        _isRequestInProgress = true;

        try
        {
            AuthOperationResultData result = await _authenticationService.SignInAsync(email, password);

            if (result.IsSuccess == false)
            {
                _authWindow.SetError(result.ErrorMessage);
                return;
            }

            ShowMainMenu();
        }
        catch (Exception exception)
        {
            UnityEngine.Debug.LogException(exception);
            _authWindow.SetError("Unexpected sign-in error.");
        }
        finally
        {
            _isRequestInProgress = false;
        }
    }

    private async Task HandleSignUpAsync(string email, string login, string password, string confirmPassword)
    {
        if (_isRequestInProgress)
        {
            return;
        }

        if (_authInputValidationService.TryValidateSignUp(
                email,
                login,
                password,
                confirmPassword,
                out string errorMessage) == false)
        {
            _authWindow.SetError(errorMessage);
            return;
        }

        _isRequestInProgress = true;

        try
        {
            AuthOperationResultData result = await _authenticationService.SignUpAsync(
                email,
                login,
                password);

            if (result.IsSuccess == false)
            {
                _authWindow.SetError(result.ErrorMessage);
                return;
            }

            ShowMainMenu();
        }
        catch (Exception exception)
        {
            UnityEngine.Debug.LogException(exception);
            _authWindow.SetError("Unexpected sign-up error.");
        }
        finally
        {
            _isRequestInProgress = false;
        }
    }
}