using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnSystem
{
    private readonly PlayerView _playerView;
    private readonly CameraTargetFollowView _cameraTargetFollowView;
    private readonly PlayerScoreSystem _playerScoreSystem;
    private readonly List<IRestartable> _restartables;

    private readonly Vector3 _startPosition;

    public PlayerRespawnSystem(
        PlayerView playerView,
        CameraTargetFollowView cameraTargetFollowView,
        PlayerScoreSystem playerScoreSystem,
        List<IRestartable> restartables)
    {
        _playerView = playerView;
        _cameraTargetFollowView = cameraTargetFollowView;
        _playerScoreSystem = playerScoreSystem;
        _restartables = restartables;

        _startPosition = playerView.transform.position;
    }

    public void Respawn()
    {
        _playerView.transform.position = _startPosition;
        _playerScoreSystem.Reset();
        _playerView.Respawn();
        

        for (int i = 0; i < _restartables.Count; i++)
        {
            _restartables[i].Restart();
        }

        _cameraTargetFollowView.SnapToPlayer();
    }
}