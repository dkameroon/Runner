using UnityEngine;

public class ObstaclePatternSpawnService
{
    private readonly ObstacleSpawnConfig _obstacleSpawnConfig;
    private readonly LanePositionProvider _lanePositionProvider;
    private readonly IObstacleFactory _obstacleFactory;

    public ObstaclePatternSpawnService(
        ObstacleSpawnConfig obstacleSpawnConfig,
        LanePositionProvider lanePositionProvider,
        IObstacleFactory obstacleFactory)
    {
        _obstacleSpawnConfig = obstacleSpawnConfig;
        _lanePositionProvider = lanePositionProvider;
        _obstacleFactory = obstacleFactory;
    }

    public void SpawnPattern(ObstacleWavePatternStruct pattern, float spawnZ)
    {
        SpawnObstacleOnLane(EPlayerLane.Left, pattern.LeftObstacleType, spawnZ);
        SpawnObstacleOnLane(EPlayerLane.Center, pattern.CenterObstacleType, spawnZ);
        SpawnObstacleOnLane(EPlayerLane.Right, pattern.RightObstacleType, spawnZ);
    }

    private void SpawnObstacleOnLane(EPlayerLane lane, EObstacleType obstacleType, float spawnZ)
    {
        if (obstacleType == EObstacleType.None)
        {
            return;
        }

        float x = _lanePositionProvider.GetLaneX(lane);
        Vector3 position = new Vector3(x, _obstacleSpawnConfig.GroundY, spawnZ);

        _obstacleFactory.Create(obstacleType, position, Quaternion.identity);
    }
}