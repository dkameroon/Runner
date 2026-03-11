using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class MainMenuFlowService : IInitializable, IDisposable
{
    private readonly MainMenuWindow _mainMenuWindow;
    private readonly AuthFlowService _authFlowService;
    private readonly IAuthenticationService _authenticationService;

    private bool _isLogoutInProgress;

    public MainMenuFlowService(
        MainMenuWindow mainMenuWindow,
        AuthFlowService authFlowService,
        IAuthenticationService authenticationService)
    {
        _mainMenuWindow = mainMenuWindow;
        _authFlowService = authFlowService;
        _authenticationService = authenticationService;
    }

    public void Initialize()
    {
        _mainMenuWindow.LogoutClicked += OnLogoutClicked;
        _isLogoutInProgress = false;
    }

    public void Dispose()
    {
        _mainMenuWindow.LogoutClicked -= OnLogoutClicked;
    }

    private void OnLogoutClicked()
    {
        _ = HandleLogoutAsync();
    }

    private async Task HandleLogoutAsync()
    {
        if (_isLogoutInProgress)
        {
            return;
        }

        _isLogoutInProgress = true;

        try
        {
            AuthOperationResultData result = await _authenticationService.SignOutAsync();

            if (result.IsSuccess == false)
            {
                Debug.LogWarning($"Sign out failed: {result.ErrorMessage}");
                return;
            }

            _authFlowService.ShowAuthScreen();
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
        finally
        {
            _isLogoutInProgress = false;
        }
    }
}