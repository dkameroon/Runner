using System.Threading.Tasks;
using Zenject;

public class StartupAuthFlowSystem : IInitializable
{
    private readonly AuthFlowService _authFlowService;
    private readonly MainMenuWindow _mainMenuWindow;
    private readonly IAuthenticationService _authenticationService;

    public StartupAuthFlowSystem(
        AuthFlowService authFlowService,
        MainMenuWindow mainMenuWindow,
        IAuthenticationService authenticationService)
    {
        _authFlowService = authFlowService;
        _mainMenuWindow = mainMenuWindow;
        _authenticationService = authenticationService;
    }

    public void Initialize()
    {
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await _authenticationService.InitializeAsync();

        if (_authenticationService.IsAuthorized)
        {
            _authFlowService.HideAuthScreen();
            _mainMenuWindow.Show();
            return;
        }

        _authFlowService.ShowAuthScreen();
    }
}