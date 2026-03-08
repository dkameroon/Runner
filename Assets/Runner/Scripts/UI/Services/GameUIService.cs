using System;
using Zenject;

public class GameUIService : IInitializable, IDisposable
{
    private readonly MainMenuWindow _mainMenuWindow;
    private readonly DefeatPopup _defeatPopup;
    private readonly GameHUDView _gameHudView;
    private readonly GameFlowSystem _gameFlowSystem;
    private readonly PlayerStateMachineSystem _playerStateMachineSystem;
    private readonly PlayerScoreSystem _playerScoreSystem;
    

    public GameUIService(
        MainMenuWindow mainMenuWindow,
        GameFlowSystem gameFlowSystem,
        DefeatPopup defeatPopup,
        PlayerStateMachineSystem playerStateMachineSystem, 
        GameHUDView gameHudView,
        PlayerScoreSystem playerScoreSystem)
    {
        _mainMenuWindow = mainMenuWindow;
        _gameFlowSystem = gameFlowSystem;
        _defeatPopup = defeatPopup;
        _playerStateMachineSystem = playerStateMachineSystem;
        _gameHudView = gameHudView;
        _playerScoreSystem = playerScoreSystem;
    }

    public void Initialize()
    {
        _gameFlowSystem.StateChanged += OnGameLoopStateChanged;
        _playerStateMachineSystem.StateChanged += OnPlayerStateChanged;
        ApplyState(_gameFlowSystem.CurrentState);
    }

    public void Dispose()
    {
        _gameFlowSystem.StateChanged -= OnGameLoopStateChanged;
        _playerStateMachineSystem.StateChanged += OnPlayerStateChanged;
    }

    private void OnGameLoopStateChanged(EGameLoopState state)
    {
        ApplyState(state);
    }
    
    private void OnPlayerStateChanged(EPlayerState state)
    {
        if (state != EPlayerState.Dead)
            return;

        _defeatPopup.SetScore(_playerScoreSystem.CurrentScore);
        _gameFlowSystem.ShowDefeat();
    }

    private void ApplyState(EGameLoopState state)
    {
        switch (state)
        {
            case EGameLoopState.MainMenu:
                _mainMenuWindow.Show();
                _defeatPopup.Hide();
                _gameHudView.Hide();
                break;

            case EGameLoopState.Playing:
                _mainMenuWindow.Hide();
                _defeatPopup.Hide();
                _gameHudView.Show();
                break;

            case EGameLoopState.Defeat:
                _mainMenuWindow.Hide();
                _defeatPopup.Show();
                _gameHudView.Hide();
                break;
        }
    }
}