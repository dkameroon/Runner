using System;
using UnityEngine;
using Zenject;

public class PlayerView : MonoBehaviour, IMovementToggle, IRespawnable
{
    public event Action JumpCompleted;

    private RunnerGameConfig _runnerGameConfig;
    private ISpeedProvider _speedProvider;

    private PlayerModel _playerModel;
    private float _laneTargetX;
    private float _laneLerpSpeed;

    private const float MinLaneChangeDurationSeconds = 0.01f;
    private const float MinJumpDurationSeconds = 0.1f;

    private bool _isMovementEnabled;
    private bool _isInitialized;

    private bool _isJumpActive;
    private float _jumpTimerSeconds;
    private float _jumpBaseY;

    [Inject]
    public void Construct(RunnerGameConfig runnerGameConfig, ISpeedProvider speedProvider)
    {
        _runnerGameConfig = runnerGameConfig;
        _speedProvider = speedProvider;

        InitializeIfNeeded();
    }

    private void Start()
    {
        InitializeIfNeeded();
    }

    private void Update()
    {
        if (!_isInitialized || !_isMovementEnabled)
            return;

        MoveForward();
        MoveToLaneTargetX();
        UpdateJump();
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        _isMovementEnabled = isEnabled;

        if (!isEnabled)
        {
            CancelJumpAndSnapToBaseY();
        }
    }

    public void TryChangeLane(int direction)
    {
        if (!_isInitialized)
            return;

        EPlayerLane newLane = GetLaneByOffset(_playerModel.CurrentLane, direction);

        if (newLane == _playerModel.CurrentLane)
            return;

        _playerModel.SetLane(newLane);
        ApplyLaneTargetX(newLane);
    }

    public void Respawn()
    {
        SetMovementEnabled(true);

        _playerModel.SetLane(EPlayerLane.Center);
        ApplyLaneInstant(EPlayerLane.Center);

        CancelJumpAndSnapToBaseY();
    }

    public void DoJump()
    {
        if (!_isInitialized)
            return;

        if (_isJumpActive)
            return;

        _isJumpActive = true;
        _jumpTimerSeconds = 0f;
        _jumpBaseY = transform.position.y;
    }
    

    private void InitializeIfNeeded()
    {
        if (_isInitialized)
            return;

        if (_runnerGameConfig == null || _speedProvider == null)
            return;

        _playerModel = new PlayerModel(EPlayerLane.Center);

        _laneLerpSpeed = CalculateLaneLerpSpeed(_runnerGameConfig.LaneChangeDurationSeconds);
        ApplyLaneInstant(_playerModel.CurrentLane);

        _isMovementEnabled = true;
        _isInitialized = true;
    }

    private void MoveForward()
    {
        transform.position += Vector3.forward * (_speedProvider.CurrentSpeed * Time.deltaTime);
    }

    private void MoveToLaneTargetX()
    {
        Vector3 position = transform.position;

        position.x = Mathf.Lerp(position.x, _laneTargetX, _laneLerpSpeed * Time.deltaTime);

        transform.position = position;
    }

    private void UpdateJump()
    {
        if (!_isJumpActive)
            return;

        float duration = Mathf.Max(_runnerGameConfig.JumpDurationSeconds, MinJumpDurationSeconds);

        _jumpTimerSeconds += Time.deltaTime;

        float t = _jumpTimerSeconds / duration;

        if (t >= 1f)
        {
            _isJumpActive = false;

            Vector3 finishPosition = transform.position;
            finishPosition.y = _jumpBaseY;

            transform.position = finishPosition;

            JumpCompleted?.Invoke();
            return;
        }

        float height = _runnerGameConfig.JumpHeightMeters;

        float yOffset = height * Mathf.Sin(Mathf.PI * t);

        Vector3 position = transform.position;
        position.y = _jumpBaseY + yOffset;

        transform.position = position;
    }

    private void CancelJumpAndSnapToBaseY()
    {
        if (!_isJumpActive)
            return;

        _isJumpActive = false;

        Vector3 position = transform.position;
        position.y = _jumpBaseY;

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
            return currentLane;

        if (value > (int)EPlayerLane.Right)
            return currentLane;

        return (EPlayerLane)value;
    }

    private static float CalculateLaneLerpSpeed(float durationSeconds)
    {
        if (durationSeconds < MinLaneChangeDurationSeconds)
            durationSeconds = MinLaneChangeDurationSeconds;

        return 1f / durationSeconds;
    }
}