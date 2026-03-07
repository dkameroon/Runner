using UnityEngine;

public class ObstacleFactory : IObstacleFactory
{
    private readonly IObstaclePoolService _poolService;
    private readonly ObstacleRegistryService _registry;
    private readonly SceneHierarchyService _sceneHierarchyService;

    public ObstacleFactory(
        IObstaclePoolService poolService,
        ObstacleRegistryService registry,
        SceneHierarchyService sceneHierarchyService)
    {
        _poolService = poolService;
        _registry = registry;
        _sceneHierarchyService = sceneHierarchyService;
    }

    public ObstacleView Create(EObstacleType obstacleType, Vector3 position, Quaternion rotation)
    {
        ObstacleView obstacle = _poolService.Get(obstacleType);

        obstacle.transform.SetParent(_sceneHierarchyService.ObstaclesRuntimeRoot, true);
        obstacle.SetPosition(position);
        obstacle.SetRotation(rotation);

        _registry.Add(obstacle);

        return obstacle;
    }
}