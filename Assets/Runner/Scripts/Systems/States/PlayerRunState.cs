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
        _context.PlayerView.SetMovementEnabled(true);
    }

    public void Exit() { }
    public void Tick() { }
}