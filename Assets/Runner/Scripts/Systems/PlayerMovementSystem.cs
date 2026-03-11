using System;
using UnityEngine;
using Zenject;

public class PlayerMovementSystem : ITickable, IInitializable, IMovementToggle, IRespawnable
{
    public event Action JumpCompleted;

    private const float MinLaneChangeDurationSeconds = 0.01f;
    private const float MinJumpDurationSeconds = 0.1f;

    private readonly PlayerView _playerView;
    private readonly RunnerGameConfig _runnerGameConfig;
    private readonly ISpeedProvider _speedProvider;
    private readonly LanePositionProvider _lanePositionProvider;

    private PlayerModel _playerModel;
    private float _laneTargetX;
    private float _laneLerpSpeed;

    private bool _isMovementEnabled;
    private bool _isInitialized;
    private bool _isPaused;

    private bool _isJumpActive;
    private float _jumpTimerSeconds;
    private float _jumpBaseY;

    public PlayerMovementSystem(
        PlayerView playerView,
        RunnerGameConfig runnerGameConfig,
        ISpeedProvider speedProvider,
        LanePositionProvider lanePositionProvider)
    {
        _playerView = playerView;
        _runnerGameConfig = runnerGameConfig;
        _speedProvider = speedProvider;
        _lanePositionProvider = lanePositionProvider;
    }

    public void Initialize()
    {
        _playerModel = new PlayerModel(EPlayerLane.Center);
        _laneLerpSpeed = CalculateLaneLerpSpeed(_runnerGameConfig.LaneChangeDurationSeconds);

        ApplyLaneInstant(_playerModel.CurrentLane);

        _isMovementEnabled = false;
        _isInitialized = true;
        _isPaused = false;
    }

    public void Tick()
    {
        if (_isInitialized == false)
        {
            return;
        }

        if (_isMovementEnabled == false)
        {
            return;
        }

        if (_isPaused)
        {
            return;
        }

        MoveForward();
        MoveToLaneTargetX();
        UpdateJump();
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        _isMovementEnabled = isEnabled;

        if (isEnabled == false)
        {
            _isPaused = false;
            CancelJumpAndSnapToBaseY();
        }
    }

    public void Pause()
    {
        if (_isInitialized == false)
        {
            return;
        }

        if (_isMovementEnabled == false)
        {
            return;
        }

        _isPaused = true;
    }

    public void Resume()
    {
        if (_isInitialized == false)
        {
            return;
        }

        if (_isMovementEnabled == false)
        {
            return;
        }

        _isPaused = false;
    }

    public void TryChangeLane(int direction)
    {
        if (_isInitialized == false)
        {
            return;
        }

        if (_isPaused)
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

    public void Respawn()
    {
        _playerModel.SetLane(EPlayerLane.Center);
        ApplyLaneInstant(EPlayerLane.Center);

        _isPaused = false;
        CancelJumpAndSnapToBaseY();
        SetMovementEnabled(true);
    }

    public void ContinueAfterDefeat()
    {
        _isPaused = false;
        CancelJumpAndSnapToBaseY();
        SetMovementEnabled(true);
    }

    public void DoJump()
    {
        if (_isInitialized == false)
        {
            return;
        }

        if (_isPaused)
        {
            return;
        }

        if (_isJumpActive)
        {
            return;
        }

        _isJumpActive = true;
        _jumpTimerSeconds = 0f;
        _jumpBaseY = _playerView.Position.y;
    }

    private void MoveForward()
    {
        _playerView.Position += Vector3.forward * (_speedProvider.CurrentSpeed * Time.deltaTime);
    }

    private void MoveToLaneTargetX()
    {
        Vector3 position = _playerView.Position;
        position.x = Mathf.Lerp(position.x, _laneTargetX, _laneLerpSpeed * Time.deltaTime);
        _playerView.Position = position;
    }

    private void UpdateJump()
    {
        if (_isJumpActive == false)
        {
            return;
        }

        float duration = Mathf.Max(_runnerGameConfig.JumpDurationSeconds, MinJumpDurationSeconds);

        _jumpTimerSeconds += Time.deltaTime;

        float t = _jumpTimerSeconds / duration;

        if (t >= 1f)
        {
            _isJumpActive = false;

            Vector3 finishPosition = _playerView.Position;
            finishPosition.y = _jumpBaseY;
            _playerView.Position = finishPosition;

            JumpCompleted?.Invoke();
            return;
        }

        float height = _runnerGameConfig.JumpHeightMeters;
        float yOffset = height * Mathf.Sin(Mathf.PI * t);

        Vector3 position = _playerView.Position;
        position.y = _jumpBaseY + yOffset;
        _playerView.Position = position;
    }

    private void CancelJumpAndSnapToBaseY()
    {
        if (_isJumpActive == false)
        {
            return;
        }

        _isJumpActive = false;

        Vector3 position = _playerView.Position;
        position.y = _jumpBaseY;
        _playerView.Position = position;
    }

    private void ApplyLaneInstant(EPlayerLane lane)
    {
        _laneTargetX = _lanePositionProvider.GetLaneX(lane);

        Vector3 position = _playerView.Position;
        position.x = _laneTargetX;
        _playerView.Position = position;
    }

    private void ApplyLaneTargetX(EPlayerLane lane)
    {
        _laneTargetX = _lanePositionProvider.GetLaneX(lane);
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
        if (durationSeconds < MinLaneChangeDurationSeconds)
        {
            durationSeconds = MinLaneChangeDurationSeconds;
        }

        return 1f / durationSeconds;
    }
}