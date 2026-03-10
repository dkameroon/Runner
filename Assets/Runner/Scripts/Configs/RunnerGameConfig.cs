using UnityEngine;

[CreateAssetMenu(menuName = "Runner/Configs/Runner Game Config", fileName = "Config_RunnerGame")]
public class RunnerGameConfig : ScriptableObject
{
    [Header("Run Speed")]
    [field: SerializeField, Min(0.1f)] public float StartSpeed { get; private set; } = 6f;
    [field: SerializeField, Min(0f)] public float SpeedIncreasePerSecond { get; private set; } = 0.1f;
    [field: SerializeField, Min(0.1f)] public float MaxSpeed { get; private set; } = 13f;

    [Header("Jump")]
    [field: SerializeField, Min(0.1f)] public float JumpHeightMeters { get; private set; } = 1.5f;
    [field: SerializeField, Min(0.1f)] public float JumpDurationSeconds { get; private set; } = 0.6f;

    [Header("Slide")]
    [field: SerializeField, Min(0.01f)] public float SlideDurationSeconds { get; private set; } = 0.7f;

    [Header("Warmup")]
    [field: SerializeField, Min(0f)] public float WarmupSeconds { get; private set; } = 2.5f;
    [Header("Revive Invulnerability Seconds")]
    [field: SerializeField] public float ReviveInvulnerabilitySeconds { get; private set; } = 2f;

    [Header("Lanes")]
    [field: SerializeField, Min(0.01f)] public float LaneOffsetX { get; private set; } = 1.5f;
    [field: SerializeField, Min(0.01f)] public float LaneChangeDurationSeconds { get; private set; } = 0.12f;

    [Header("Score")]
    [field: SerializeField, Min(0f)] public float ScorePerMeter { get; private set; } = 1f;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (MaxSpeed < StartSpeed)
            MaxSpeed = StartSpeed;

        if (LaneOffsetX <= 0f)
            LaneOffsetX = 0.01f;

        if (LaneChangeDurationSeconds <= 0f)
            LaneChangeDurationSeconds = 0.01f;

        if (JumpDurationSeconds <= 0f)
            JumpDurationSeconds = 0.1f;

        if (JumpHeightMeters < 0f)
            JumpHeightMeters = 0f;
    }
#endif
}