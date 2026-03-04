using UnityEngine;

public class PlayerView : MonoBehaviour, IMovementToggle, IRespawnable
{
    private RunnerGameConfig _runnerGameConfig;

    private PlayerModel _playerModel;
    private float _currentForwardSpeed;
    private float _laneTargetX;
    private float _laneLerpSpeed;

    private bool _isMovementEnabled;
    private bool _isInitialized;

    public void Construct(RunnerGameConfig runnerGameConfig)
    {
        _runnerGameConfig = runnerGameConfig;
        InitializeIfNeeded();
    }

    private void Start()
    {
        InitializeIfNeeded();
    }

    private void Update()
    {
        if (!_isInitialized)
        {
            return;
        }

        if (!_isMovementEnabled)
        {
            return;
        }

        MoveForward();
        MoveToLaneTargetX();
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        _isMovementEnabled = isEnabled;
    }

    public void TryChangeLane(int direction)
    {
        if (!_isInitialized)
        {
            return;
        }

        EPlayerLane newLane = GetLaneByOffset(_playerModel.CurrentLane, direction);
        if (newLane == _playerModel.CurrentLane)
        {
            return;
        }

        _playerModel.SetLane(newLane);
        ApplyLaneTargetX(newLane);
    }

    private void InitializeIfNeeded()
    {
        if (_isInitialized)
        {
            return;
        }

        if (_runnerGameConfig == null)
        {
            return;
        }

        _playerModel = new PlayerModel(EPlayerLane.Center);

        _currentForwardSpeed = _runnerGameConfig.StartSpeed;
        _laneLerpSpeed = CalculateLaneLerpSpeed(_runnerGameConfig.LaneChangeDurationSeconds);

        ApplyLaneInstant(_playerModel.CurrentLane);

        _isMovementEnabled = true;
        _isInitialized = true;
    }

    private void MoveForward()
    {
        transform.position += Vector3.forward * (_currentForwardSpeed * Time.deltaTime);
    }

    private void MoveToLaneTargetX()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Lerp(position.x, _laneTargetX, _laneLerpSpeed * Time.deltaTime);
        transform.position = position;
    }

    private void ApplyLaneInstant(EPlayerLane lane)
    {
        _laneTargetX = GetLaneX(lane);

        Vector3 position = transform.position;
        position.x = _laneTargetX;
        transform.position = position;
    }

    private void ApplyLaneTargetX(EPlayerLane lane)
    {
        _laneTargetX = GetLaneX(lane);
    }

    private float GetLaneX(EPlayerLane lane)
    {
        float offset = _runnerGameConfig.LaneOffsetX;

        return lane switch
        {
            EPlayerLane.Left => -offset,
            EPlayerLane.Center => 0f,
            EPlayerLane.Right => offset,
            _ => 0f
        };
    }

    private static EPlayerLane GetLaneByOffset(EPlayerLane currentLane, int direction)
    {
        int value = (int)currentLane + direction;

        if (value < (int)EPlayerLane.Left)
        {
            return currentLane;
        }

        if (value > (int)EPlayerLane.Right)
        {
            return currentLane;
        }

        return (EPlayerLane)value;
    }

    private static float CalculateLaneLerpSpeed(float durationSeconds)
    {
        const float MinDurationSeconds = 0.01f;

        if (durationSeconds < MinDurationSeconds)
        {
            durationSeconds = MinDurationSeconds;
        }

        return 1f / durationSeconds;
    }
    
    public void Respawn()
    {
        SetMovementEnabled(true);
        _playerModel.SetLane(EPlayerLane.Center);
        ApplyLaneInstant(EPlayerLane.Center);
        _currentForwardSpeed = _runnerGameConfig.StartSpeed;
        
    }
}