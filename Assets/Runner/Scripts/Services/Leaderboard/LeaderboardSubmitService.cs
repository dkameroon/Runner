using System;
using Zenject;

public class LeaderboardSubmitService : IInitializable, IDisposable
{
    private readonly ILeaderboardService _leaderboardService;
    private readonly IAuthenticationService _authenticationService;
    private readonly PlayerScoreSystem _playerScoreSystem;
    private readonly PlayerStateMachineSystem _playerStateMachineSystem;

    public LeaderboardSubmitService(
        ILeaderboardService leaderboardService,
        IAuthenticationService authenticationService,
        PlayerScoreSystem playerScoreSystem,
        PlayerStateMachineSystem playerStateMachineSystem)
    {
        _leaderboardService = leaderboardService;
        _authenticationService = authenticationService;
        _playerScoreSystem = playerScoreSystem;
        _playerStateMachineSystem = playerStateMachineSystem;
    }

    public void Initialize()
    {
        _playerStateMachineSystem.StateChanged += OnPlayerStateChanged;
    }

    public void Dispose()
    {
        _playerStateMachineSystem.StateChanged -= OnPlayerStateChanged;
    }

    private async void OnPlayerStateChanged(EPlayerState state)
    {
        if (state != EPlayerState.Dead)
        {
            return;
        }

        if (_authenticationService.IsAuthorized == false)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(_authenticationService.UserId))
        {
            return;
        }

        string userLogin = string.IsNullOrWhiteSpace(_authenticationService.UserLogin)
            ? "Player"
            : _authenticationService.UserLogin;

        int score = _playerScoreSystem.CurrentScore;

        if (score <= 0)
        {
            return;
        }

        await _leaderboardService.SubmitScoreAsync(
            _authenticationService.UserId,
            userLogin,
            score);
    }
}