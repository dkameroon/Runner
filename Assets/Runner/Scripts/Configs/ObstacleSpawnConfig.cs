using UnityEngine;

[CreateAssetMenu(
    fileName = "ObstacleSpawnConfig_Main",
    menuName = "Runner/Configs/ObstacleSpawnConfig")]
public class ObstacleSpawnConfig : ScriptableObject
{
    [Header("Spawn Distance")]
    [field: SerializeField] public float SpawnDistanceAhead { get; private set; } = 35.0f;
    [field: SerializeField] public float MinDistanceBetweenObstacles { get; private set; } = 12.0f;

    [Header("Cleanup")]
    [field: SerializeField] public float DespawnDistanceBehindPlayer { get; private set; } = 15.0f;

    [Header("Spawn Timing")]
    [field: SerializeField] public float SpawnIntervalSeconds { get; private set; } = 1.0f;

    [Header("Pool")]
    [field: SerializeField] public int PrewarmCountPerType { get; private set; } = 5;
    
    [Header("Ground")]
    [field: SerializeField] public float GroundY { get; private set; } = 0f;
}