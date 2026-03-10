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

    [Header("Wave Patterns")]
    [field: SerializeField, Range(0f, 1f)] public float SingleLanePatternChance { get; private set; } = 0.55f;
    [field: SerializeField, Range(0f, 1f)] public float DoubleLanePatternChance { get; private set; } = 0.35f;
    [field: SerializeField, Range(0f, 1f)] public float TripleLanePatternChance { get; private set; } = 0.10f;
    [field: SerializeField] public bool DisallowTripleDodgePattern { get; private set; } = true;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (SpawnDistanceAhead < 1f)
        {
            SpawnDistanceAhead = 1f;
        }

        if (MinDistanceBetweenObstacles < 0.1f)
        {
            MinDistanceBetweenObstacles = 0.1f;
        }

        if (DespawnDistanceBehindPlayer < 0f)
        {
            DespawnDistanceBehindPlayer = 0f;
        }

        if (SpawnIntervalSeconds < 0.05f)
        {
            SpawnIntervalSeconds = 0.05f;
        }

        if (PrewarmCountPerType < 1)
        {
            PrewarmCountPerType = 1;
        }

        float totalChance = SingleLanePatternChance + DoubleLanePatternChance + TripleLanePatternChance;

        if (totalChance <= 0f)
        {
            SingleLanePatternChance = 1f;
            DoubleLanePatternChance = 0f;
            TripleLanePatternChance = 0f;
        }
    }
#endif
}