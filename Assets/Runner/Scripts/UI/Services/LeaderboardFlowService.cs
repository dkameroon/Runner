using System;
using System.Collections.Generic;
using Zenject;

public class LeaderboardFlowService : IInitializable, IDisposable
{
    private readonly MainMenuWindow _mainMenuWindow;
    private readonly LeaderboardWindow _leaderboardWindow;
    private readonly ILeaderboardService _leaderboardService;
    private readonly LeaderboardEntryElement _leaderboardEntryElementPrefab;
    private readonly DiContainer _diContainer;
    private readonly IAuthenticationService _authenticationService;
    private readonly PlayerScoreSystem _playerScoreSystem;

    public LeaderboardFlowService(
        MainMenuWindow mainMenuWindow,
        LeaderboardWindow leaderboardWindow,
        ILeaderboardService leaderboardService,
        IAuthenticationService authenticationService,
        PlayerScoreSystem playerScoreSystem,
        LeaderboardEntryElement leaderboardEntryElementPrefab,
        DiContainer diContainer)
    {
        _mainMenuWindow = mainMenuWindow;
        _leaderboardWindow = leaderboardWindow;
        _leaderboardService = leaderboardService;
        _leaderboardEntryElementPrefab = leaderboardEntryElementPrefab;
        _diContainer = diContainer;
        _authenticationService = authenticationService;
        _playerScoreSystem = playerScoreSystem;
    }

    public void Initialize()
    {
        _mainMenuWindow.LeaderboardClicked += OnLeaderboardClicked;
        _leaderboardWindow.ReturnToMenuClicked += OnReturnToMenuClicked;
    }

    public void Dispose()
    {
        _mainMenuWindow.LeaderboardClicked -= OnLeaderboardClicked;
        _leaderboardWindow.ReturnToMenuClicked -= OnReturnToMenuClicked;
    }

    private async void OnLeaderboardClicked()
    {
        _mainMenuWindow.Hide();

        IReadOnlyList<LeaderboardEntryData> entries = await _leaderboardService.LoadTopEntriesAsync(20);

        _leaderboardWindow.ClearEntries();

        for (int i = 0; i < entries.Count; i++)
        {
            LeaderboardEntryElement entryElement =
                _diContainer.InstantiatePrefabForComponent<LeaderboardEntryElement>(
                    _leaderboardEntryElementPrefab);

            entryElement.SetData(entries[i]);
            _leaderboardWindow.AddEntry(entryElement);
        }

        string playerLogin = string.IsNullOrWhiteSpace(_authenticationService.UserLogin)
            ? "Player"
            : _authenticationService.UserLogin;

        int playerBestScore = 0;

        for (int i = 0; i < entries.Count; i++)
        {
            if (entries[i].UserId != _authenticationService.UserId)
            {
                continue;
            }

            playerBestScore = entries[i].Score;
            break;
        }

        _leaderboardWindow.SetPlayerBestScore(playerLogin, playerBestScore);
        _leaderboardWindow.Show();
    }

    private void OnReturnToMenuClicked()
    {
        _leaderboardWindow.Hide();
        _mainMenuWindow.Show();
    }
}