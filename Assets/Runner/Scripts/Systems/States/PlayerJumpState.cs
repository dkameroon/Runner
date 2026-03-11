public class PlayerJumpState : IPlayerState
{
    public EPlayerState StateType => EPlayerState.Jump;

    private readonly PlayerStateContextModel _context;
    private readonly PlayerStateMachineSystem _stateMachine;

    public PlayerJumpState(PlayerStateContextModel context, PlayerStateMachineSystem stateMachine)
    {
        _context = context;
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        _context.PlayerAnimatorView.TriggerJump();
        _context.PlayerMovementSystem.JumpCompleted += OnJumpCompleted;
        _context.PlayerMovementSystem.DoJump();
    }

    public void Exit()
    {
        _context.PlayerMovementSystem.JumpCompleted -= OnJumpCompleted;
    }

    public void Tick()
    {
    }

    private void OnJumpCompleted()
    {
        _stateMachine.ReturnToRun();
    }
}