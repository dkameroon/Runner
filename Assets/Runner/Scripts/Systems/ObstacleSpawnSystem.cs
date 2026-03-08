using UnityEngine;
using Zenject;

public class ObstacleSpawnSystem : ITickable, IRestartable
{
    public int ActiveObstacleCount => _registry.Active.Count;

    private readonly RunnerGameConfig _runnerGameConfig;
    private readonly ObstacleSpawnConfig _spawnConfig;
    private readonly IObstacleFactory _factory;
    private readonly Transform _cameraTransform;
    private readonly IObstaclePoolService _poolService;
    private readonly ObstacleRegistryService _registry;
    private readonly GameplaySessionService _gameplaySessionService;

    private float _timer;
    private float _startDelayTimer;
    private float _lastSpawnZ;

    private readonly System.Random _random = new();

    public ObstacleSpawnSystem(
        RunnerGameConfig runnerGameConfig,
        ObstacleSpawnConfig spawnConfig,
        IObstacleFactory factory,
        CameraTargetFollowView cameraTargetFollowView,
        IObstaclePoolService poolService,
        ObstacleRegistryService registry,
        GameplaySessionService gameplaySessionService)
    {
        _runnerGameConfig = runnerGameConfig;
        _spawnConfig = spawnConfig;
        _factory = factory;
        _cameraTransform = cameraTargetFollowView.transform;
        _poolService = poolService;
        _registry = registry;
        _gameplaySessionService = gameplaySessionService;

        ResetSpawnState();
    }

    public void Tick()
    {
        if (_gameplaySessionService.IsGameplayActive == false)
            return;

        _startDelayTimer += Time.deltaTime;

        if (_startDelayTimer < _runnerGameConfig.WarmupSeconds)
            return;

        _timer += Time.deltaTime;

        if (_timer < _spawnConfig.SpawnIntervalSeconds)
            return;

        _timer = 0f;

        float cameraZ = _cameraTransform.position.z;

        float nextZ = Mathf.Max(
            _lastSpawnZ + _spawnConfig.MinDistanceBetweenObstacles,
            cameraZ + _spawnConfig.SpawnDistanceAhead);

        _lastSpawnZ = nextZ;

        EPlayerLane lane = GetRandomLane();
        float x = GetLaneX(lane, _runnerGameConfig.LaneOffsetX);

        EObstacleType type = GetRandomObstacleType();

        Vector3 position = new Vector3(x, _spawnConfig.GroundY, nextZ);
        Quaternion rotation = Quaternion.identity;

        _factory.Create(type, position, rotation);
    }

    public void Restart()
    {
        ReturnAllActiveObstaclesToPool();
        ResetSpawnState();
    }

    private void ResetSpawnState()
    {
        _timer = 0f;
        _startDelayTimer = 0f;
        _lastSpawnZ = _cameraTransform.position.z;
    }

    private void ReturnAllActiveObstaclesToPool()
    {
        for (int i = _registry.Active.Count - 1; i >= 0; i--)
        {
            ObstacleView obstacle = _registry.Active[i];

            if (obstacle == null)
            {
                _registry.RemoveAt(i);
                continue;
            }

            _poolService.Return(obstacle);
            _registry.RemoveAt(i);
        }
    }

    private EPlayerLane GetRandomLane()
    {
        int value = _random.Next((int)EPlayerLane.Left, (int)EPlayerLane.Right + 1);
        return (EPlayerLane)value;
    }

    private static float GetLaneX(EPlayerLane lane, float offset)
    {
        return lane switch
        {
            EPlayerLane.Left => -offset,
            EPlayerLane.Center => 0f,
            EPlayerLane.Right => offset,
            _ => 0f
        };
    }

    private EObstacleType GetRandomObstacleType()
    {
        int value = _random.Next((int)EObstacleType.Dodge, (int)EObstacleType.Jump + 1);
        return (EObstacleType)value;
    }
}