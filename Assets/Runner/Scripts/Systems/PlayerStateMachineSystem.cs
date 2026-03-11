using System;
using System.Collections.Generic;
using Zenject;

public class PlayerStateMachineSystem : IInitializable, ITickable, IRestartable
{
    public EPlayerState CurrentStateType => _currentState.StateType;
    public string CurrentStateName => CurrentStateType.ToString();

    public event Action<EPlayerState> StateChanged;

    private readonly Dictionary<EPlayerState, IPlayerState> _stateByType;

    private IPlayerState _currentState;
    private bool _isPaused;

    public PlayerStateMachineSystem(PlayerStateFactory playerStateFactory)
    {
        _stateByType = playerStateFactory.CreateStates(this);
    }

    public void Initialize()
    {
        _isPaused = false;
        _currentState = GetState(EPlayerState.Idle);
        _currentState.Enter();

        StateChanged?.Invoke(_currentState.StateType);
    }

    public void Tick()
    {
        if (_isPaused)
        {
            return;
        }

        _currentState.Tick();
    }

    public bool CanProcessInput()
    {
        if (_isPaused)
        {
            return false;
        }

        return _currentState.StateType != EPlayerState.Dead &&
               _currentState.StateType != EPlayerState.Idle;
    }

    public void SetIdle()
    {
        SwitchState(EPlayerState.Idle);
    }

    public void SetRun()
    {
        SwitchState(EPlayerState.Run);
    }

    public void SetDead()
    {
        _isPaused = false;
        SwitchState(EPlayerState.Dead);
    }

    public void RequestJump()
    {
        if (_isPaused)
        {
            return;
        }

        if (_currentState.StateType != EPlayerState.Run)
        {
            return;
        }

        SwitchState(EPlayerState.Jump);
    }

    public void RequestSlide()
    {
        if (_isPaused)
        {
            return;
        }

        if (_currentState.StateType != EPlayerState.Run)
        {
            return;
        }

        SwitchState(EPlayerState.Slide);
    }

    public void Pause()
    {
        if (_currentState.StateType == EPlayerState.Dead)
        {
            return;
        }

        if (_currentState.StateType == EPlayerState.Idle)
        {
            return;
        }

        _isPaused = true;
    }

    public void Resume()
    {
        _isPaused = false;
    }

    internal void ReturnToRun()
    {
        if (_isPaused)
        {
            return;
        }

        SwitchState(EPlayerState.Run);
    }

    public void Restart()
    {
        _isPaused = false;
        SetRun();
    }

    private void SwitchState(EPlayerState newStateType)
    {
        IPlayerState newState = GetState(newStateType);

        if (newState == _currentState)
        {
            return;
        }

        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();

        StateChanged?.Invoke(_currentState.StateType);
    }

    private IPlayerState GetState(EPlayerState stateType)
    {
        if (_stateByType.TryGetValue(stateType, out IPlayerState state))
        {
            return state;
        }

        throw new InvalidOperationException($"State {stateType} is not registered.");
    }
}