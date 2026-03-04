using UnityEngine;

[CreateAssetMenu(menuName = "Configs/RunnerGameConfig")]
public class RunnerGameConfig : ScriptableObject
{
    [SerializeField] private float _startSpeed = 6f;
    [SerializeField] private float _speedIncreasePerSecond = 0.1f;
    [SerializeField] private float _warmupSeconds = 3f;
    [SerializeField] private float _laneOffsetX = 1.5f;
    [SerializeField] private float _laneChangeDurationSeconds = 0.12f;

    public float LaneOffsetX => _laneOffsetX;
    public float LaneChangeDurationSeconds => _laneChangeDurationSeconds;
    public float StartSpeed => _startSpeed;
    public float SpeedIncreasePerSecond => _speedIncreasePerSecond;
    public float WarmupSeconds => _warmupSeconds;
}