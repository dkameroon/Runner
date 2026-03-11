using UnityEngine;
using Zenject;

public class ObstacleSpawnSystem : ITickable, IRestartable
{
    public int ActiveObstacleCount => _registry.Active.Count;

    private readonly RunnerGameConfig _runnerGameConfig;
    private readonly ObstacleSpawnConfig _spawnConfig;
    private readonly Transform _cameraTransform;
    private readonly Transform _playerTransform;
    private readonly IObstaclePoolService _poolService;
    private readonly ObstacleRegistryService _registry;
    private readonly GameplaySessionService _gameplaySessionService;
    private readonly ObstacleDifficultyProvider _obstacleDifficultyProvider;
    private readonly ObstacleWavePatternProvider _obstacleWavePatternProvider;
    private readonly ObstaclePatternSpawnService _obstaclePatternSpawnService;

    private float _timer;
    private float _startDelayTimer;
    private float _lastSpawnZ;
    private float _activeGameplayTime;
    private int _lastOccupiedLaneCount = 1;

    public ObstacleSpawnSystem(
        RunnerGameConfig runnerGameConfig,
        ObstacleSpawnConfig spawnConfig,
        CameraTargetFollowView cameraTargetFollowView,
        PlayerView playerView,
        IObstaclePoolService poolService,
        ObstacleRegistryService registry,
        GameplaySessionService gameplaySessionService,
        ObstacleDifficultyProvider obstacleDifficultyProvider,
        ObstacleWavePatternProvider obstacleWavePatternProvider,
        ObstaclePatternSpawnService obstaclePatternSpawnService)
    {
        _runnerGameConfig = runnerGameConfig;
        _spawnConfig = spawnConfig;
        _cameraTransform = cameraTargetFollowView.transform;
        _playerTransform = playerView.transform;
        _poolService = poolService;
        _registry = registry;
        _gameplaySessionService = gameplaySessionService;
        _obstacleDifficultyProvider = obstacleDifficultyProvider;
        _obstacleWavePatternProvider = obstacleWavePatternProvider;
        _obstaclePatternSpawnService = obstaclePatternSpawnService;

        ResetSpawnState(_playerTransform.position.z);
    }

    public void Tick()
    {
        if (_gameplaySessionService.IsGameplayActive == false)
        {
            return;
        }

        _startDelayTimer += Time.deltaTime;

        if (_startDelayTimer < _runnerGameConfig.WarmupSeconds)
        {
            return;
        }

        _activeGameplayTime += Time.deltaTime;
        _timer += Time.deltaTime;

        float currentSpawnInterval = _obstacleDifficultyProvider.GetSpawnIntervalSeconds(_activeGameplayTime);

        if (_timer < currentSpawnInterval)
        {
            return;
        }

        _timer = 0f;

        float cameraZ = _cameraTransform.position.z;
        float nextZ = Mathf.Max(
            _lastSpawnZ + _spawnConfig.MinDistanceBetweenObstacles,
            cameraZ + _spawnConfig.SpawnDistanceAhead);

        _lastSpawnZ = nextZ;

        ObstacleWavePatternStruct pattern = _obstacleWavePatternProvider.GetSmartRandomPattern(
            _activeGameplayTime,
            _lastOccupiedLaneCount);

        _obstaclePatternSpawnService.SpawnPattern(pattern, nextZ);

        _lastOccupiedLaneCount = pattern.OccupiedLaneCount;
    }

    public void Restart()
    {
        ReturnAllActiveObstaclesToPool();
        ResetSpawnState(_playerTransform.position.z);
    }

    public void RestartAfterContinue(float playerZ)
    {
        ReturnAllActiveObstaclesToPool();
        ResetSpawnState(playerZ);
    }

    private void ResetSpawnState(float referenceZ)
    {
        _timer = 0f;
        _startDelayTimer = 0f;
        _lastSpawnZ = referenceZ;
        _activeGameplayTime = 0f;
        _lastOccupiedLaneCount = 1;
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
}