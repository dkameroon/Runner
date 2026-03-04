using UnityEngine;

public class PlayerRespawnSystem
{
    private readonly Transform _playerTransform;
    private readonly PlayerDeathSystem _playerDeathSystem;
    private readonly IRespawnable _respawnable;
    private readonly ICameraRespawnSync _cameraRespawnSync;
    private readonly PlayerScoreSystem _playerScoreSystem;

    private readonly Vector3 _startPosition;

    public PlayerRespawnSystem(
        Transform playerTransform,
        PlayerDeathSystem playerDeathSystem,
        IRespawnable respawnable,
        ICameraRespawnSync cameraRespawnSync,
        PlayerScoreSystem playerScoreSystem)
    {
        _playerTransform = playerTransform;
        _playerDeathSystem = playerDeathSystem;
        _respawnable = respawnable;
        _cameraRespawnSync = cameraRespawnSync;
        _playerScoreSystem = playerScoreSystem;

        _startPosition = playerTransform.position;
    }

    public void Respawn()
    {
        if (_playerTransform == null)
        {
            Debug.LogError("PlayerRespawnSystem: _playerTransform is null");
            return;
        }

        if (_playerDeathSystem == null)
        {
            Debug.LogError("PlayerRespawnSystem: _playerDeathSystem is null");
            return;
        }

        if (_playerScoreSystem == null)
        {
            Debug.LogError("PlayerRespawnSystem: _playerScoreSystem is null");
            return;
        }

        if (_respawnable == null)
        {
            Debug.LogError("PlayerRespawnSystem: _respawnable is null");
            return;
        }

        _playerTransform.position = _startPosition;

        _playerDeathSystem.Reset();
        _playerScoreSystem.Reset();
        _respawnable.Respawn();

        _cameraRespawnSync?.SnapToPlayer();
    }
}