using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObstacleSpawnSystem : ITickable, IRestartable
{
    public int ActiveObstacleCount => _registry.Active.Count;

    private readonly RunnerGameConfig _runnerGameConfig;
    private readonly ObstacleSpawnConfig _spawnConfig;
    private readonly ObstacleDifficultyConfig _difficultyConfig;
    private readonly IObstacleFactory _factory;
    private readonly Transform _cameraTransform;
    private readonly Transform _playerTransform;
    private readonly IObstaclePoolService _poolService;
    private readonly ObstacleRegistryService _registry;
    private readonly GameplaySessionService _gameplaySessionService;

    private readonly System.Random _random = new();
    private readonly List<ObstacleWavePatternStruct> _singleLanePatterns = new();
    private readonly List<ObstacleWavePatternStruct> _doubleLanePatterns = new();
    private readonly List<ObstacleWavePatternStruct> _tripleLanePatterns = new();

    private float _timer;
    private float _startDelayTimer;
    private float _lastSpawnZ;
    private float _activeGameplayTime;
    private int _lastOccupiedLaneCount = 1;

    public ObstacleSpawnSystem(
        RunnerGameConfig runnerGameConfig,
        ObstacleSpawnConfig spawnConfig,
        ObstacleDifficultyConfig difficultyConfig,
        IObstacleFactory factory,
        CameraTargetFollowView cameraTargetFollowView,
        PlayerView playerView,
        IObstaclePoolService poolService,
        ObstacleRegistryService registry,
        GameplaySessionService gameplaySessionService)
    {
        _runnerGameConfig = runnerGameConfig;
        _spawnConfig = spawnConfig;
        _difficultyConfig = difficultyConfig;
        _factory = factory;
        _cameraTransform = cameraTargetFollowView.transform;
        _playerTransform = playerView.transform;
        _poolService = poolService;
        _registry = registry;
        _gameplaySessionService = gameplaySessionService;

        BuildWavePatternLibrary();
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

        float currentSpawnInterval = GetCurrentSpawnInterval();

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

        ObstacleWavePatternStruct pattern = GetSmartRandomWavePattern();
        SpawnPattern(pattern, nextZ);

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

    private void BuildWavePatternLibrary()
    {
        _singleLanePatterns.Clear();
        _doubleLanePatterns.Clear();
        _tripleLanePatterns.Clear();

        EObstacleType[] allObstacleTypes =
        {
            EObstacleType.None,
            EObstacleType.Dodge,
            EObstacleType.Slide,
            EObstacleType.Jump
        };

        for (int i = 0; i < allObstacleTypes.Length; i++)
        {
            for (int j = 0; j < allObstacleTypes.Length; j++)
            {
                for (int k = 0; k < allObstacleTypes.Length; k++)
                {
                    ObstacleWavePatternStruct pattern = new ObstacleWavePatternStruct(
                        allObstacleTypes[i],
                        allObstacleTypes[j],
                        allObstacleTypes[k]);

                    if (IsPatternValid(pattern) == false)
                    {
                        continue;
                    }

                    switch (pattern.OccupiedLaneCount)
                    {
                        case 1:
                            _singleLanePatterns.Add(pattern);
                            break;
                        case 2:
                            _doubleLanePatterns.Add(pattern);
                            break;
                        case 3:
                            _tripleLanePatterns.Add(pattern);
                            break;
                    }
                }
            }
        }
    }

    private bool IsPatternValid(ObstacleWavePatternStruct pattern)
    {
        if (pattern.OccupiedLaneCount <= 0)
        {
            return false;
        }

        if (_spawnConfig.DisallowTripleDodgePattern && pattern.IsTripleDodgePattern)
        {
            return false;
        }

        return true;
    }

    private ObstacleWavePatternStruct GetSmartRandomWavePattern()
    {
        int occupiedLaneCount = GetSmartOccupiedLaneCount();

        return occupiedLaneCount switch
        {
            1 => GetRandomPatternFromList(_singleLanePatterns),
            2 => GetRandomPatternFromList(_doubleLanePatterns),
            3 => GetRandomPatternFromList(_tripleLanePatterns),
            _ => GetRandomPatternFromList(_singleLanePatterns)
        };
    }

    private int GetSmartOccupiedLaneCount()
    {
        GetCurrentPatternChances(
            out float singleChance,
            out float doubleChance,
            out float tripleChance);

        if (_lastOccupiedLaneCount >= 2)
        {
            singleChance += 0.20f;
            doubleChance -= 0.10f;
            tripleChance -= 0.10f;
        }

        singleChance = Mathf.Max(0f, singleChance);
        doubleChance = Mathf.Max(0f, doubleChance);
        tripleChance = Mathf.Max(0f, tripleChance);

        float totalChance = singleChance + doubleChance + tripleChance;

        if (totalChance <= 0f)
        {
            return 1;
        }

        float roll = (float)_random.NextDouble() * totalChance;

        if (roll < singleChance)
        {
            return 1;
        }

        roll -= singleChance;

        if (roll < doubleChance)
        {
            return 2;
        }

        return 3;
    }

    private void GetCurrentPatternChances(
        out float singleChance,
        out float doubleChance,
        out float tripleChance)
    {
        if (_activeGameplayTime < _difficultyConfig.MediumDifficultyTime)
        {
            singleChance = _difficultyConfig.EasySingleLaneChance;
            doubleChance = _difficultyConfig.EasyDoubleLaneChance;
            tripleChance = _difficultyConfig.EasyTripleLaneChance;
            return;
        }

        if (_activeGameplayTime < _difficultyConfig.HardDifficultyTime)
        {
            singleChance = _difficultyConfig.MediumSingleLaneChance;
            doubleChance = _difficultyConfig.MediumDoubleLaneChance;
            tripleChance = _difficultyConfig.MediumTripleLaneChance;
            return;
        }

        singleChance = _difficultyConfig.HardSingleLaneChance;
        doubleChance = _difficultyConfig.HardDoubleLaneChance;
        tripleChance = _difficultyConfig.HardTripleLaneChance;
    }

    private float GetCurrentSpawnInterval()
    {
        if (_activeGameplayTime < _difficultyConfig.MediumDifficultyTime)
        {
            return _difficultyConfig.EasySpawnIntervalSeconds;
        }

        if (_activeGameplayTime < _difficultyConfig.HardDifficultyTime)
        {
            return _difficultyConfig.MediumSpawnIntervalSeconds;
        }

        return _difficultyConfig.HardSpawnIntervalSeconds;
    }

    private ObstacleWavePatternStruct GetRandomPatternFromList(List<ObstacleWavePatternStruct> patterns)
    {
        if (patterns.Count == 0)
        {
            return new ObstacleWavePatternStruct(
                EObstacleType.Dodge,
                EObstacleType.None,
                EObstacleType.None);
        }

        int index = _random.Next(0, patterns.Count);
        return patterns[index];
    }

    private void SpawnPattern(ObstacleWavePatternStruct pattern, float spawnZ)
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

        float x = GetLaneX(lane, _runnerGameConfig.LaneOffsetX);
        Vector3 position = new Vector3(x, _spawnConfig.GroundY, spawnZ);
        Quaternion rotation = Quaternion.identity;

        _factory.Create(obstacleType, position, rotation);
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
}