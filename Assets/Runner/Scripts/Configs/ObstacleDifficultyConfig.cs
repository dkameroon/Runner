using UnityEngine;

[CreateAssetMenu(
    fileName = "Config_ObstacleDifficulty",
    menuName = "Runner/Configs/Obstacle Difficulty Config")]
public class ObstacleDifficultyConfig : ScriptableObject
{
    [Header("Time Thresholds")]
    [field: SerializeField] public float MediumDifficultyTime { get; private set; } = 30f;
    [field: SerializeField] public float HardDifficultyTime { get; private set; } = 60f;

    [Header("Easy")]
    [field: SerializeField, Range(0f, 1f)] public float EasySingleLaneChance { get; private set; } = 0.75f;
    [field: SerializeField, Range(0f, 1f)] public float EasyDoubleLaneChance { get; private set; } = 0.25f;
    [field: SerializeField, Range(0f, 1f)] public float EasyTripleLaneChance { get; private set; } = 0.0f;
    [field: SerializeField] public float EasySpawnIntervalSeconds { get; private set; } = 0.95f;

    [Header("Medium")]
    [field: SerializeField, Range(0f, 1f)] public float MediumSingleLaneChance { get; private set; } = 0.55f;
    [field: SerializeField, Range(0f, 1f)] public float MediumDoubleLaneChance { get; private set; } = 0.35f;
    [field: SerializeField, Range(0f, 1f)] public float MediumTripleLaneChance { get; private set; } = 0.10f;
    [field: SerializeField] public float MediumSpawnIntervalSeconds { get; private set; } = 0.80f;

    [Header("Hard")]
    [field: SerializeField, Range(0f, 1f)] public float HardSingleLaneChance { get; private set; } = 0.35f;
    [field: SerializeField, Range(0f, 1f)] public float HardDoubleLaneChance { get; private set; } = 0.45f;
    [field: SerializeField, Range(0f, 1f)] public float HardTripleLaneChance { get; private set; } = 0.20f;
    [field: SerializeField] public float HardSpawnIntervalSeconds { get; private set; } = 0.70f;
}