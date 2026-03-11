public class PlayerGameStateService
{
    private readonly PlayerMovementSystem _playerMovementSystem;
    private readonly PlayerAnimatorView _playerAnimatorView;
    private readonly PlayerStateMachineSystem _playerStateMachineSystem;
    private readonly GameplaySessionService _gameplaySessionService;

    public PlayerGameStateService(
        PlayerMovementSystem playerMovementSystem,
        PlayerAnimatorView playerAnimatorView,
        PlayerStateMachineSystem playerStateMachineSystem,
        GameplaySessionService gameplaySessionService)
    {
        _playerMovementSystem = playerMovementSystem;
        _playerAnimatorView = playerAnimatorView;
        _playerStateMachineSystem = playerStateMachineSystem;
        _gameplaySessionService = gameplaySessionService;
    }

    public void ApplyMainMenuState()
    {
        _gameplaySessionService.Deactivate();

        _playerMovementSystem.SetMovementEnabled(false);
        _playerAnimatorView.SetPaused(false);

        _playerStateMachineSystem.Resume();
        _playerStateMachineSystem.SetIdle();
    }

    public void ApplyPlayingState()
    {
        _gameplaySessionService.Activate();

        _playerAnimatorView.SetPaused(false);

        _playerStateMachineSystem.Resume();
        _playerMovementSystem.SetMovementEnabled(true);
        _playerMovementSystem.Resume();

        _playerStateMachineSystem.SetRun();
    }

    public void ApplyPausedState()
    {
        _gameplaySessionService.Deactivate();

        _playerMovementSystem.Pause();
        _playerStateMachineSystem.Pause();
        _playerAnimatorView.SetPaused(true);
    }

    public void ApplyResumeFromPauseState()
    {
        _gameplaySessionService.Activate();

        _playerAnimatorView.SetPaused(false);

        _playerStateMachineSystem.Resume();
        _playerMovementSystem.SetMovementEnabled(true);
        _playerMovementSystem.Resume();
    }

    public void ApplyDefeatState()
    {
        _gameplaySessionService.Deactivate();

        _playerAnimatorView.SetPaused(false);
        _playerStateMachineSystem.Resume();
        _playerMovementSystem.SetMovementEnabled(false);

        _playerStateMachineSystem.SetDead();
    }

    public void ApplyContinueAfterDefeatState()
    {
        _gameplaySessionService.Activate();

        _playerAnimatorView.SetPaused(false);

        _playerStateMachineSystem.Resume();
        _playerMovementSystem.SetMovementEnabled(true);
        _playerMovementSystem.Resume();

        _playerStateMachineSystem.SetRun();
    }
}