using System;
using UnityEngine;
using Zenject;

public class ObstacleSpawnSystem : ITickable
{
    private readonly RunnerGameConfig _runnerGameConfig;
    private readonly ObstacleSpawnConfig _spawnConfig;
    private readonly IObstacleFactory _factory;
    private readonly Transform _playerTransform;

    private float _timer;
    private float _startDelayTimer;
    private float _lastSpawnZ;

    private readonly System.Random _random = new();

    private readonly Transform _cameraTransform;

    public ObstacleSpawnSystem(
        RunnerGameConfig runnerGameConfig,
        ObstacleSpawnConfig spawnConfig,
        IObstacleFactory factory,
        PlayerView playerView,
        CameraTargetFollowView cameraTargetFollowView)
    {
        _runnerGameConfig = runnerGameConfig;
        _spawnConfig = spawnConfig;
        _factory = factory;
        _playerTransform = playerView.transform;
        _cameraTransform = cameraTargetFollowView.transform;
    }

    public void Tick()
    {
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
        
        _lastSpawnZ = nextZ;

        EPlayerLane lane = GetRandomLane();
        float x = GetLaneX(lane, _runnerGameConfig.LaneOffsetX);

        EObstacleType type = GetRandomObstacleType();

        Vector3 position = new Vector3(x, _spawnConfig.GroundY, nextZ);
        Quaternion rotation = Quaternion.identity;

        _factory.Create(type, position, rotation);
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