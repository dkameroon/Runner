public class LanePositionProvider
{
    private readonly RunnerGameConfig _runnerGameConfig;

    public LanePositionProvider(RunnerGameConfig runnerGameConfig)
    {
        _runnerGameConfig = runnerGameConfig;
    }

    public float GetLaneX(EPlayerLane lane)
    {
        float offset = _runnerGameConfig.LaneOffsetX;

        return lane switch
        {
            EPlayerLane.Left => -offset,
            EPlayerLane.Center => 0f,
            EPlayerLane.Right => offset,
            _ => 0f
        };
    }
}