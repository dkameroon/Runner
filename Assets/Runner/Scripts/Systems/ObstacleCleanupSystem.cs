using UnityEngine;
using Zenject;

public class ObstacleCleanupSystem : ITickable
{
    private readonly ObstacleSpawnConfig _spawnConfig;
    private readonly IObstaclePoolService _poolService;
    private readonly ObstacleRegistryService _registry;
    private readonly Transform _playerTransform;

    public ObstacleCleanupSystem(
        ObstacleSpawnConfig spawnConfig,
        IObstaclePoolService poolService,
        ObstacleRegistryService registry,
        PlayerView playerView)
    {
        _spawnConfig = spawnConfig;
        _poolService = poolService;
        _registry = registry;
        _playerTransform = playerView.transform;
    }

    public void Tick()
    {
        float playerZ = _playerTransform.position.z;
        float despawnZ = playerZ - _spawnConfig.DespawnDistanceBehindPlayer;

        for (int i = _registry.Active.Count - 1; i >= 0; i--)
        {
            ObstacleView obstacle = _registry.Active[i];

            if (obstacle == null)
            {
                _registry.RemoveAt(i);
                continue;
            }

            if (obstacle.transform.position.z > despawnZ)
                continue;

            _registry.RemoveAt(i);
            _poolService.Return(obstacle);
        }
    }
}