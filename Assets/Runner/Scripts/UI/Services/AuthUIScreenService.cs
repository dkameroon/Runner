public class AuthUIScreenService
{
    private readonly AuthWindow _authWindow;
    private readonly MainMenuWindow _mainMenuWindow;

    public AuthUIScreenService(
        AuthWindow authWindow,
        MainMenuWindow mainMenuWindow)
    {
        _authWindow = authWindow;
        _mainMenuWindow = mainMenuWindow;
    }

    public void ShowUnauthorizedState()
    {
        _mainMenuWindow.Hide();
        _authWindow.Show();
    }

    public void ShowAuthorizedState()
    {
        _authWindow.Hide();
        _mainMenuWindow.Show();
    }

    public void HideAuthScreen()
    {
        _authWindow.Hide();
    }
}