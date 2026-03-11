public class PlayerStateContextModel
{
    public PlayerStateContextModel(
        PlayerMovementSystem playerMovementSystem,
        PlayerAnimatorView playerAnimatorView,
        PlayerHitboxView playerHitboxView,
        RunnerGameConfig runnerGameConfig)
    {
        PlayerMovementSystem = playerMovementSystem;
        PlayerAnimatorView = playerAnimatorView;
        PlayerHitboxView = playerHitboxView;
        RunnerGameConfig = runnerGameConfig;
    }

    public PlayerMovementSystem PlayerMovementSystem { get; }
    public PlayerAnimatorView PlayerAnimatorView { get; }
    public PlayerHitboxView PlayerHitboxView { get; }
    public RunnerGameConfig RunnerGameConfig { get; }
}