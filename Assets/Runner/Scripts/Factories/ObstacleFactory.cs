using UnityEngine;

public class ObstacleFactory : IObstacleFactory
{
    private readonly IObstaclePoolService _poolService;
    private readonly ObstacleRegistryService _registry;

    public ObstacleFactory(IObstaclePoolService poolService, ObstacleRegistryService registry)
    {
        _poolService = poolService;
        _registry = registry;
    }

    public ObstacleView Create(EObstacleType obstacleType, Vector3 position, Quaternion rotation)
    {
        ObstacleView obstacle = _poolService.Get(obstacleType);

        obstacle.SetPosition(position);
        obstacle.SetRotation(rotation);

        _registry.Add(obstacle);

        return obstacle;
    }
}