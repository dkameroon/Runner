public class PlayerStateContextModel
{
    public PlayerStateContextModel(PlayerView playerView, RunnerGameConfig runnerGameConfig)
    {
        PlayerView = playerView;
        RunnerGameConfig = runnerGameConfig;
    }

    public PlayerView PlayerView { get; }
    public RunnerGameConfig RunnerGameConfig { get; }
}