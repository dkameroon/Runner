using System;
using UnityEngine;
using Zenject;

public class PlayerStateMachineSystem : ITickable, IInitializable, IRestartable
{
    public EPlayerState CurrentStateType => _currentState.StateType;

    public event Action<EPlayerState> StateChanged;

    private readonly PlayerIdleState _idleState;
    private readonly PlayerRunState _runState;
    private readonly PlayerJumpState _jumpState;
    private readonly PlayerSlideState _slideState;
    private readonly PlayerDeadState _deadState;

    private IPlayerState _currentState;
    public string CurrentStateName => CurrentStateType.ToString();

    public PlayerStateMachineSystem(PlayerStateContextModel context)
    {
        _idleState = new PlayerIdleState(context);
        _runState = new PlayerRunState(context);
        _jumpState = new PlayerJumpState(context, this);
        _slideState = new PlayerSlideState(context, this);
        _deadState = new PlayerDeadState(context);

        _currentState = _idleState;
        _currentState.Enter();
        StateChanged?.Invoke(_currentState.StateType);
    }
    
    public void Initialize()
    {
        SetRun();
    }

    public void Tick()
    {
        _currentState.Tick();
    }

    public bool CanProcessInput()
    {
        return _currentState.StateType != EPlayerState.Dead &&
               _currentState.StateType != EPlayerState.Idle;
    }

    public void SetIdle() => SwitchState(_idleState);
    public void SetRun() => SwitchState(_runState);
    public void SetDead() => SwitchState(_deadState);

    public void RequestJump()
    {
        if (_currentState.StateType != EPlayerState.Run)
            return;

        SwitchState(_jumpState);
    }

    public void RequestSlide()
    {
        if (_currentState.StateType != EPlayerState.Run)
            return;

        SwitchState(_slideState);
    }

    internal void ReturnToRun()
    {
        SwitchState(_runState);
    }
    
    public void Restart()
    {
        SetRun();
    }

    private void SwitchState(IPlayerState newState)
    {
        if (newState == _currentState)
            return;

        _currentState.Exit();
        _currentState = newState;

        StateChanged?.Invoke(_currentState.StateType);

        _currentState.Enter();
        
    }
}