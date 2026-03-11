using System.Collections.Generic;

public class PlayerStateFactory
{
    private readonly PlayerStateContextModel _context;

    public PlayerStateFactory(PlayerStateContextModel context)
    {
        _context = context;
    }

    public Dictionary<EPlayerState, IPlayerState> CreateStates(PlayerStateMachineSystem stateMachineSystem)
    {
        return new Dictionary<EPlayerState, IPlayerState>
        {
            { EPlayerState.Idle, new PlayerIdleState(_context) },
            { EPlayerState.Run, new PlayerRunState(_context) },
            { EPlayerState.Jump, new PlayerJumpState(_context, stateMachineSystem) },
            { EPlayerState.Slide, new PlayerSlideState(_context, stateMachineSystem) },
            { EPlayerState.Dead, new PlayerDeadState(_context) }
        };
    }
}