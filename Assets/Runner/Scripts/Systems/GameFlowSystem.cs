using System;
using Zenject;

public class GameFlowSystem : IInitializable
{
    public EGameLoopState CurrentState { get; private set; }

    public event Action<EGameLoopState> StateChanged;

    private readonly PlayerView _playerView;
    private readonly SpeedSystem _speedSystem;
    private readonly PlayerScoreSystem _playerScoreSystem;
    private readonly ObstacleSpawnSystem _obstacleSpawnSystem;
    private readonly PlayerStateMachineSystem _playerStateMachineSystem;
    private readonly GameplaySessionService _gameplaySessionService;

    public GameFlowSystem(
        PlayerView playerView,
        PlayerStateMachineSystem playerStateMachineSystem,
        GameplaySessionService gameplaySessionService)
    {
        _playerView = playerView;
        _playerStateMachineSystem = playerStateMachineSystem;
        _gameplaySessionService = gameplaySessionService;
    }

    public void Initialize()
    {
        EnterMainMenu();
    }

    public void EnterMainMenu()
    {
        CurrentState = EGameLoopState.MainMenu;
        _gameplaySessionService.StopGameplay();
        _playerView.SetMovementEnabled(false);
        _playerStateMachineSystem.SetIdle();

        StateChanged?.Invoke(CurrentState);
    }

    public void StartGame()
    {
        CurrentState = EGameLoopState.Playing;
        _gameplaySessionService.StartGameplay();
        _playerView.SetMovementEnabled(true);
        _playerStateMachineSystem.SetRun();

        StateChanged?.Invoke(CurrentState);
    }

    public void ShowDefeat()
    {
        CurrentState = EGameLoopState.Defeat;
        _gameplaySessionService.StopGameplay();
        _playerView.SetMovementEnabled(false);
        _playerStateMachineSystem.SetDead();

        StateChanged?.Invoke(CurrentState);
    }
}