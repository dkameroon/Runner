public class PlayerContinueService
{
    private readonly PlayerView _playerView;
    private readonly PlayerMovementSystem _playerMovementSystem;
    private readonly PlayerCollisionView _playerCollisionView;
    private readonly CameraTargetFollowView _cameraTargetFollowView;
    private readonly ObstacleSpawnSystem _obstacleSpawnSystem;

    public PlayerContinueService(
        PlayerView playerView,
        PlayerMovementSystem playerMovementSystem,
        PlayerCollisionView playerCollisionView,
        CameraTargetFollowView cameraTargetFollowView,
        ObstacleSpawnSystem obstacleSpawnSystem)
    {
        _playerView = playerView;
        _playerMovementSystem = playerMovementSystem;
        _playerCollisionView = playerCollisionView;
        _cameraTargetFollowView = cameraTargetFollowView;
        _obstacleSpawnSystem = obstacleSpawnSystem;
    }

    public void ContinueAfterDefeat()
    {
        float playerZ = _playerView.Position.z;

        _playerMovementSystem.ContinueAfterDefeat();
        _playerCollisionView.EnableReviveInvulnerability();
        _obstacleSpawnSystem.RestartAfterContinue(playerZ);
        _cameraTargetFollowView.SnapToPlayer();
    }
}