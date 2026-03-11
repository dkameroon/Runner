public class PlayerIdleState : IPlayerState
{
    public EPlayerState StateType => EPlayerState.Idle;

    private readonly PlayerStateContextModel _context;

    public PlayerIdleState(PlayerStateContextModel context)
    {
        _context = context;
    }

    public void Enter()
    {
        _context.PlayerMovementSystem.SetMovementEnabled(false);
        _context.PlayerAnimatorView.SetRunning(false);
    }

    public void Exit()
    {
    }

    public void Tick()
    {
    }
}