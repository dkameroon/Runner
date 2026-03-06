public class PlayerStateContextModel
{
    public PlayerStateContextModel(
        PlayerView playerView,
        PlayerAnimatorView playerAnimatorView,
        PlayerHitboxView playerHitboxView,
        RunnerGameConfig runnerGameConfig)
    {
        PlayerView = playerView;
        PlayerAnimatorView = playerAnimatorView;
        PlayerHitboxView = playerHitboxView;
        RunnerGameConfig = runnerGameConfig;
    }

    public PlayerView PlayerView { get; }
    public PlayerAnimatorView PlayerAnimatorView { get; }
    public RunnerGameConfig RunnerGameConfig { get; }
    public PlayerHitboxView PlayerHitboxView { get; }
}