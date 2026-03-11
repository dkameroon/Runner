using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class LeaderboardSubmitService : IInitializable, IDisposable
{
    private const string DefaultUserLogin = "Player";

    private readonly ILeaderboardService _leaderboardService;
    private readonly IAuthenticationService _authenticationService;
    private readonly PlayerScoreSystem _playerScoreSystem;
    private readonly PlayerStateMachineSystem _playerStateMachineSystem;

    private bool _isSubmitting;

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
        _isSubmitting = false;
    }

    public void Dispose()
    {
        _playerStateMachineSystem.StateChanged -= OnPlayerStateChanged;
    }

    private void OnPlayerStateChanged(EPlayerState state)
    {
        if (state != EPlayerState.Dead)
        {
            return;
        }

        _ = HandlePlayerDeadAsync();
    }

    private async Task HandlePlayerDeadAsync()
    {
        if (_isSubmitting)
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

        int score = _playerScoreSystem.CurrentScore;

        if (score <= 0)
        {
            return;
        }

        string userLogin = string.IsNullOrWhiteSpace(_authenticationService.UserLogin)
            ? DefaultUserLogin
            : _authenticationService.UserLogin;

        _isSubmitting = true;

        try
        {
            LeaderboardSubmitResultData result = await _leaderboardService.SubmitScoreAsync(
                _authenticationService.UserId,
                userLogin,
                score);

            if (result.IsSuccess == false)
            {
                Debug.LogWarning($"Leaderboard submit failed: {result.ErrorMessage}");
            }
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
        finally
        {
            _isSubmitting = false;
        }
    }
}