using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

public class LeaderboardFlowService : IInitializable, IDisposable
{
    private const int TopEntriesCount = 20;

    private readonly MainMenuWindow _mainMenuWindow;
    private readonly LeaderboardWindow _leaderboardWindow;
    private readonly ILeaderboardService _leaderboardService;
    private readonly LeaderboardEntryElementFactory _leaderboardEntryElementFactory;
    private readonly LeaderboardPlayerBestScoreProvider _leaderboardPlayerBestScoreProvider;

    private bool _isLoadingLeaderboard;

    public LeaderboardFlowService(
        MainMenuWindow mainMenuWindow,
        LeaderboardWindow leaderboardWindow,
        ILeaderboardService leaderboardService,
        LeaderboardEntryElementFactory leaderboardEntryElementFactory,
        LeaderboardPlayerBestScoreProvider leaderboardPlayerBestScoreProvider)
    {
        _mainMenuWindow = mainMenuWindow;
        _leaderboardWindow = leaderboardWindow;
        _leaderboardService = leaderboardService;
        _leaderboardEntryElementFactory = leaderboardEntryElementFactory;
        _leaderboardPlayerBestScoreProvider = leaderboardPlayerBestScoreProvider;
    }

    public void Initialize()
    {
        _mainMenuWindow.LeaderboardClicked += OnLeaderboardClicked;
        _leaderboardWindow.ReturnToMenuClicked += OnReturnToMenuClicked;
        _isLoadingLeaderboard = false;
    }

    public void Dispose()
    {
        _mainMenuWindow.LeaderboardClicked -= OnLeaderboardClicked;
        _leaderboardWindow.ReturnToMenuClicked -= OnReturnToMenuClicked;
    }

    private void OnLeaderboardClicked()
    {
        _ = ShowLeaderboardAsync();
    }

    private void OnReturnToMenuClicked()
    {
        _leaderboardWindow.Hide();
        _mainMenuWindow.Show();
    }

    private async Task ShowLeaderboardAsync()
    {
        if (_isLoadingLeaderboard)
        {
            return;
        }

        _isLoadingLeaderboard = true;

        try
        {
            _mainMenuWindow.Hide();

            IReadOnlyList<LeaderboardEntryData> entries =
                await _leaderboardService.LoadTopEntriesAsync(TopEntriesCount);

            RebuildEntries(entries);

            string playerLogin = _leaderboardPlayerBestScoreProvider.GetPlayerLogin();
            int playerBestScore = _leaderboardPlayerBestScoreProvider.GetPlayerBestScore(entries);

            _leaderboardWindow.SetPlayerBestScore(playerLogin, playerBestScore);
            _leaderboardWindow.Show();
        }
        catch (Exception exception)
        {
            UnityEngine.Debug.LogException(exception);
            _mainMenuWindow.Show();
        }
        finally
        {
            _isLoadingLeaderboard = false;
        }
    }

    private void RebuildEntries(IReadOnlyList<LeaderboardEntryData> entries)
    {
        _leaderboardWindow.ClearEntries();

        for (int index = 0; index < entries.Count; index++)
        {
            LeaderboardEntryElement entryElement = _leaderboardEntryElementFactory.Create();
            entryElement.SetData(entries[index]);
            _leaderboardWindow.AddEntry(entryElement);
        }
    }
}