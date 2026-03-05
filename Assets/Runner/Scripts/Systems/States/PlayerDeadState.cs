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
        _context.PlayerView.SetMovementEnabled(false);
    }

    public void Exit() { }
    public void Tick() { }
}