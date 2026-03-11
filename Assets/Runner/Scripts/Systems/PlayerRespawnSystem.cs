using UnityEngine;

public class PlayerRespawnSystem
{
    private readonly PlayerView _playerView;
    private readonly PlayerMovementSystem _playerMovementSystem;
    private readonly ICameraRespawnSync _cameraRespawnSync;
    private readonly PlayerScoreResetService _playerScoreResetService;
    private readonly GameplayRestartService _gameplayRestartService;

    private readonly Vector3 _startPosition;

    public PlayerRespawnSystem(
        PlayerView playerView,
        PlayerMovementSystem playerMovementSystem,
        ICameraRespawnSync cameraRespawnSync,
        PlayerScoreResetService playerScoreResetService,
        GameplayRestartService gameplayRestartService)
    {
        _playerView = playerView;
        _playerMovementSystem = playerMovementSystem;
        _cameraRespawnSync = cameraRespawnSync;
        _playerScoreResetService = playerScoreResetService;
        _gameplayRestartService = gameplayRestartService;

        _startPosition = _playerView.Position;
    }

    public void Respawn()
    {
        _playerView.Position = _startPosition;

        _playerScoreResetService.ResetScore();
        _playerMovementSystem.Respawn();
        _gameplayRestartService.RestartAll();

        _cameraRespawnSync.SnapToPlayer();
    }
}