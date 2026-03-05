using UnityEngine;

public class PlayerSlideState : IPlayerState
{
    public EPlayerState StateType => EPlayerState.Slide;

    private readonly PlayerStateContextModel _context;
    private readonly PlayerStateMachineSystem _stateMachine;

    private float _timeLeft;

    public PlayerSlideState(PlayerStateContextModel context, PlayerStateMachineSystem stateMachine)
    {
        _context = context;
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        _context.PlayerView.DoSlide();
        _timeLeft = _context.RunnerGameConfig.SlideDurationSeconds;
    }

    public void Exit() { }

    public void Tick()
    {
        _timeLeft -= Time.deltaTime;
        if (_timeLeft <= 0f)
            _stateMachine.ReturnToRun();
    }
}