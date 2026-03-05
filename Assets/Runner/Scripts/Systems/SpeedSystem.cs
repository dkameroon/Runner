using UnityEngine;
using Zenject;

public class SpeedSystem : ITickable, ISpeedProvider
{
    public float CurrentSpeed { get; private set; }

    private readonly RunnerGameConfig _runnerGameConfig;

    public SpeedSystem(RunnerGameConfig runnerGameConfig)
    {
        _runnerGameConfig = runnerGameConfig;
        ResetSpeed();
    }

    public void ResetSpeed()
    {
        CurrentSpeed = _runnerGameConfig.StartSpeed;
    }

    public void Tick()
    {
        float newSpeed = CurrentSpeed +
                         _runnerGameConfig.SpeedIncreasePerSecond * Time.deltaTime;

        CurrentSpeed = Mathf.Min(newSpeed, _runnerGameConfig.MaxSpeed);
    }
}