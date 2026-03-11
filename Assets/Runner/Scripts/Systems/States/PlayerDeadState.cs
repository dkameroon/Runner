public class PlayerDeadState : IPlayerState
{
    public EPlayerState StateType => EPlayerState.Dead;

    private readonly PlayerStateContextModel _context;

    public PlayerDeadState(PlayerStateContextModel context)
    {
        _context = context;
    }

    public void Enter()
    {
        _context.PlayerAnimatorView.SetRunning(false);
        _context.PlayerAnimatorView.SetDead(true);
        _context.PlayerMovementSystem.SetMovementEnabled(false);
    }

    public void Exit()
    {
    }

    public void Tick()
    {
    }
}