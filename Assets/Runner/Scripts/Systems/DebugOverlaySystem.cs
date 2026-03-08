using UnityEngine;
using Zenject;

public class DebugOverlaySystem : ITickable, IInitializable
{
    private const float MillisecondsPerSecond = 1000f;

    private readonly DebugOverlayView _debugOverlayView;
    private readonly DebugOverlayConfig _debugOverlayConfig;
    private readonly SpeedSystem _speedSystem;
    private readonly PlayerScoreSystem _playerScoreSystem;
    private readonly ObstacleSpawnSystem _obstacleSpawnSystem;
    private readonly RunnerWorldSpawnSystem _runnerWorldSpawnSystem;
    private readonly PlayerStateMachineSystem _playerStateMachineSystem;

    private float _refreshTimer;
    private float _frameCounterTime;
    private int _frameCounter;
    private int _currentFps;
    private float _currentFrameTimeMilliseconds;

    public DebugOverlaySystem(
        DebugOverlayView debugOverlayView,
        DebugOverlayConfig debugOverlayConfig,
        SpeedSystem speedSystem,
        PlayerScoreSystem playerScoreSystem,
        ObstacleSpawnSystem obstacleSpawnSystem,
        RunnerWorldSpawnSystem runnerWorldSpawnSystem,
        PlayerStateMachineSystem playerStateMachineSystem)
    {
        _debugOverlayView = debugOverlayView;
        _debugOverlayConfig = debugOverlayConfig;
        _speedSystem = speedSystem;
        _playerScoreSystem = playerScoreSystem;
        _obstacleSpawnSystem = obstacleSpawnSystem;
        _runnerWorldSpawnSystem = runnerWorldSpawnSystem;
        _playerStateMachineSystem = playerStateMachineSystem;
    }

    public void Initialize()
    {
        bool isOverlayEnabled = ShouldEnableOverlay();
        _debugOverlayView.SetActive(isOverlayEnabled);

        if (isOverlayEnabled == false)
        {
            return;
        }

        UpdateOverlay();
    }

    public void Tick()
    {
        if (ShouldEnableOverlay() == false)
        {
            return;
        }

        CollectFrameStatistics();

        _refreshTimer += Time.deltaTime;

        if (_refreshTimer < _debugOverlayConfig.RefreshInterval)
        {
            return;
        }

        _refreshTimer = 0f;
        UpdateOverlay();
    }

    private bool ShouldEnableOverlay()
    {
        if (_debugOverlayConfig.IsEnabled == false)
        {
            return false;
        }

        if (_debugOverlayConfig.ShowOnlyInDevelopmentBuild &&
            Debug.isDebugBuild == false)
        {
            return false;
        }

        return true;
    }

    private void CollectFrameStatistics()
    {
        _frameCounter++;
        _frameCounterTime += Time.unscaledDeltaTime;

        if (_frameCounterTime <= 0f)
        {
            return;
        }

        _currentFps = Mathf.RoundToInt(_frameCounter / _frameCounterTime);

        if (_currentFps > 0)
        {
            _currentFrameTimeMilliseconds = MillisecondsPerSecond / _currentFps;
        }

        if (_frameCounterTime >= _debugOverlayConfig.RefreshInterval)
        {
            _frameCounter = 0;
            _frameCounterTime = 0f;
        }
    }

    private void UpdateOverlay()
    {
        _debugOverlayView.SetFps(_currentFps);
        _debugOverlayView.SetFrameTime(_currentFrameTimeMilliseconds);
        _debugOverlayView.SetSpeed(_speedSystem.CurrentSpeed);
        _debugOverlayView.SetScore(_playerScoreSystem.CurrentScore);
        _debugOverlayView.SetObstaclesCount(_obstacleSpawnSystem.ActiveObstacleCount);
        _debugOverlayView.SetSegmentsCount(_runnerWorldSpawnSystem.ActiveSegmentCount);
        _debugOverlayView.SetPlayerState(_playerStateMachineSystem.CurrentStateName);
    }
}