using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnSystem
{
    private readonly PlayerView _playerView;
    private readonly PlayerCollisionView _playerCollisionView;
    private readonly CameraTargetFollowView _cameraTargetFollowView;
    private readonly PlayerScoreSystem _playerScoreSystem;
    private readonly PlayerScoreUpdateView _playerScoreUpdateView;
    private readonly List<IRestartable> _restartables;
    private readonly ObstacleSpawnSystem _obstacleSpawnSystem;

    private readonly Vector3 _startPosition;

    public PlayerRespawnSystem(
        PlayerView playerView,
        PlayerCollisionView playerCollisionView,
        CameraTargetFollowView cameraTargetFollowView,
        PlayerScoreSystem playerScoreSystem,
        List<IRestartable> restartables,
        PlayerScoreUpdateView playerScoreUpdateView,
        ObstacleSpawnSystem obstacleSpawnSystem)
    {
        _playerView = playerView;
        _playerCollisionView = playerCollisionView;
        _cameraTargetFollowView = cameraTargetFollowView;
        _playerScoreSystem = playerScoreSystem;
        _restartables = restartables;
        _playerScoreUpdateView = playerScoreUpdateView;
        _obstacleSpawnSystem = obstacleSpawnSystem;

        _startPosition = _playerView.transform.position;
    }

    public void Respawn()
    {
        _playerView.transform.position = _startPosition;
        _cameraTargetFollowView.SnapToPlayer();

        _playerScoreUpdateView.ResetTracking();
        _playerScoreSystem.Reset();
        _playerView.Respawn();

        for (int i = 0; i < _restartables.Count; i++)
        {
            _restartables[i].Restart();
        }

        _cameraTargetFollowView.SnapToPlayer();
    }

    public void ContinueAfterDefeat()
    {
        Vector3 playerPosition = _playerView.transform.position;

        _playerView.ContinueAfterDefeat();
        _playerCollisionView.EnableReviveInvulnerability();
        _obstacleSpawnSystem.RestartAfterContinue(playerPosition.z);
        _cameraTargetFollowView.SnapToPlayer();
    }
}