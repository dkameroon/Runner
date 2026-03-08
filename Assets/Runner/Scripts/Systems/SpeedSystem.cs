using UnityEngine;
using Zenject;

public class SpeedSystem : ITickable, ISpeedProvider, IRestartable
{
    public float CurrentSpeed { get; private set; }

    private readonly RunnerGameConfig _runnerGameConfig;
    private readonly GameplaySessionService _gameplaySessionService;

    public SpeedSystem(
        RunnerGameConfig runnerGameConfig,
        GameplaySessionService gameplaySessionService)
    {
        _runnerGameConfig = runnerGameConfig;
        _gameplaySessionService = gameplaySessionService;
        ResetSpeed();
    }

    public void Tick()
    {
        if (_gameplaySessionService.IsGameplayActive == false)
            return;

        float newSpeed = CurrentSpeed +
                         _runnerGameConfig.SpeedIncreasePerSecond * Time.deltaTime;

        CurrentSpeed = Mathf.Min(newSpeed, _runnerGameConfig.MaxSpeed);
    }

    public void Restart()
    {
        ResetSpeed();
    }

    private void ResetSpeed()
    {
        CurrentSpeed = _runnerGameConfig.StartSpeed;
    }
}