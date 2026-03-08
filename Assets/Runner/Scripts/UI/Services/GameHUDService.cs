using Zenject;

public class GameHUDService : ITickable
{
    private readonly GameHUDView _gameHudView;
    private readonly PlayerScoreSystem _playerScoreSystem;
    private readonly GameFlowSystem _gameFlowSystem;

    public GameHUDService(
        GameHUDView gameHudView,
        PlayerScoreSystem playerScoreSystem,
        GameFlowSystem gameFlowSystem)
    {
        _gameHudView = gameHudView;
        _playerScoreSystem = playerScoreSystem;
        _gameFlowSystem = gameFlowSystem;
    }

    public void Tick()
    {
        if (_gameFlowSystem.CurrentState != EGameLoopState.Playing)
            return;

        _gameHudView.SetScore(_playerScoreSystem.CurrentScore);
    }
}