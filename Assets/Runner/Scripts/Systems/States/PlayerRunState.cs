public class PlayerRunState : IPlayerState
{
    public EPlayerState StateType => EPlayerState.Run;

    private readonly PlayerStateContextModel _context;

    public PlayerRunState(PlayerStateContextModel context)
    {
        _context = context;
    }

    public void Enter()
    {
        _context.PlayerMovementSystem.SetMovementEnabled(true);
        _context.PlayerAnimatorView.SetDead(false);
        _context.PlayerAnimatorView.SetRunning(true);
    }

    public void Exit()
    {
    }

    public void Tick()
    {
    }
}