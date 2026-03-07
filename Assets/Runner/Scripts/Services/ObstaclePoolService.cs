using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObstaclePoolService : IObstaclePoolService
{
    private readonly DiContainer _container;
    private readonly ObstaclePrefabsConfig _prefabsConfig;
    private readonly ObstacleSpawnConfig _spawnConfig;
    private readonly SceneHierarchyService _sceneHierarchyService;

    private readonly Dictionary<EObstacleType, Queue<ObstacleView>> _poolsByType = new();
    private readonly System.Random _random = new();

    private readonly Transform _poolRoot;

    public ObstaclePoolService(
        DiContainer container,
        ObstaclePrefabsConfig prefabsConfig,
        ObstacleSpawnConfig spawnConfig,
        SceneHierarchyService sceneHierarchyService)
    {
        _container = container;
        _prefabsConfig = prefabsConfig;
        _spawnConfig = spawnConfig;
        _sceneHierarchyService = sceneHierarchyService;

        _poolRoot = _sceneHierarchyService.ObstaclesPoolRoot;

        Prewarm();
    }

    public ObstacleView Get(EObstacleType obstacleType)
    {
        if (!_poolsByType.TryGetValue(obstacleType, out Queue<ObstacleView> pool))
        {
            pool = new Queue<ObstacleView>();
            _poolsByType.Add(obstacleType, pool);
        }

        if (pool.Count > 0)
        {
            ObstacleView fromPool = pool.Dequeue();
            fromPool.Show();
            return fromPool;
        }

        return CreateNew(obstacleType);
    }

    public void Return(ObstacleView obstacleView)
    {
        if (obstacleView == null)
            return;

        EObstacleType type = obstacleView.ObstacleType;

        if (!_poolsByType.TryGetValue(type, out Queue<ObstacleView> pool))
        {
            pool = new Queue<ObstacleView>();
            _poolsByType.Add(type, pool);
        }

        obstacleView.Hide();
        obstacleView.transform.SetParent(_poolRoot, true);

        pool.Enqueue(obstacleView);
    }

    private void Prewarm()
    {
        foreach (ObstaclePrefabGroupStruct group in _prefabsConfig.Groups)
        {
            int count = Mathf.Max(0, _spawnConfig.PrewarmCountPerType);

            for (int i = 0; i < count; i++)
            {
                ObstacleView obstacle = CreateNew(group.ObstacleType);
                Return(obstacle);
            }
        }
    }

    private ObstacleView CreateNew(EObstacleType obstacleType)
    {
        if (!_prefabsConfig.TryGetGroup(obstacleType, out ObstaclePrefabGroupStruct group))
            throw new ArgumentException($"No prefab group for type: {obstacleType}");

        if (group.Prefabs == null || group.Prefabs.Count == 0)
            throw new ArgumentException($"Prefab list is empty for type: {obstacleType}");

        int index = _random.Next(0, group.Prefabs.Count);
        ObstacleView prefab = group.Prefabs[index];

        ObstacleView instance = _container.InstantiatePrefabForComponent<ObstacleView>(prefab);
        instance.transform.SetParent(_poolRoot, true);

        instance.Hide();
        return instance;
    }
}