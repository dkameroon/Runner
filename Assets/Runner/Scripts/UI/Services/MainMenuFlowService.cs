using System;
using Zenject;

public class MainMenuFlowService : IInitializable, IDisposable
{
    private readonly MainMenuWindow _mainMenuWindow;
    private readonly AuthFlowService _authFlowService;
    private readonly IAuthenticationService _authenticationService;

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
    }

    public void Dispose()
    {
        _mainMenuWindow.LogoutClicked -= OnLogoutClicked;
    }

    private async void OnLogoutClicked()
    {
        AuthOperationResultData result = await _authenticationService.SignOutAsync();

        if (result.IsSuccess == false)
        {
            return;
        }

        _mainMenuWindow.Hide();
        _authFlowService.ShowAuthScreen();
    }
}