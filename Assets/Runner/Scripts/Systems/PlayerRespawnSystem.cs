using UnityEngine;

public class PlayerRespawnSystem
{
    private readonly Transform _playerTransform;
    private readonly IRespawnable _respawnable;
    private readonly ICameraRespawnSync _cameraRespawnSync;
    private readonly PlayerScoreSystem _playerScoreSystem;

    private readonly Vector3 _startPosition;

    public PlayerRespawnSystem(
        Transform playerTransform,
        IRespawnable respawnable,
        ICameraRespawnSync cameraRespawnSync,
        PlayerScoreSystem playerScoreSystem)
    {
        _playerTransform = playerTransform;
        _respawnable = respawnable;
        _cameraRespawnSync = cameraRespawnSync;
        _playerScoreSystem = playerScoreSystem;

        _startPosition = playerTransform.position;
    }

    public void Respawn()
    {
        _playerTransform.position = _startPosition;
        _playerScoreSystem.Reset();
        _respawnable.Respawn();
        _cameraRespawnSync?.SnapToPlayer();
    }
}