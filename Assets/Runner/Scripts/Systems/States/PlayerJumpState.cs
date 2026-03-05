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
        _context.PlayerView.JumpCompleted += OnJumpCompleted;
        _context.PlayerView.DoJump();
    }

    public void Exit()
    {
        _context.PlayerView.JumpCompleted -= OnJumpCompleted;
    }

    public void Tick() { }

    private void OnJumpCompleted()
    {
        _stateMachine.ReturnToRun();
    }
}