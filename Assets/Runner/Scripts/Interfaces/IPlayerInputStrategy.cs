using System;

public interface IPlayerInputStrategy
{
    event Action<EPlayerInputCommand> CommandTriggered;

    void Tick();
}