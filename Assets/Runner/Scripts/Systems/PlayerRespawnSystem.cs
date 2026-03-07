using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnSystem
{
    private readonly Transform _playerTransform;
    private readonly IRespawnable _respawnable;
    private readonly ICameraRespawnSync _cameraRespawnSync;
    private readonly PlayerScoreSystem _playerScoreSystem;
    private readonly List<IRestartable> _restartables;

    private readonly Vector3 _startPosition;

    public PlayerRespawnSystem(
        Transform playerTransform,
        IRespawnable respawnable,
        ICameraRespawnSync cameraRespawnSync,
        PlayerScoreSystem playerScoreSystem,
        List<IRestartable> restartables)
    {
        _playerTransform = playerTransform;
        _respawnable = respawnable;
        _cameraRespawnSync = cameraRespawnSync;
        _playerScoreSystem = playerScoreSystem;
        _restartables = restartables;

        _startPosition = playerTransform.position;
    }

    public void Respawn()
    {
        _playerTransform.position = _startPosition;
        _playerScoreSystem.Reset();
        _respawnable.Respawn();

        for (int i = 0; i < _restartables.Count; i++)
        {
            _restartables[i].Restart();
        }

        _cameraRespawnSync?.SnapToPlayer();
    }
}