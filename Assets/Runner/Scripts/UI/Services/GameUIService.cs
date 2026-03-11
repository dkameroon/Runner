using System;
using Zenject;

public class GameUIService : IInitializable, IDisposable
{
    private readonly MainMenuWindow _mainMenuWindow;
    private readonly GamePopupService _gamePopupService;
    private readonly GameHUDView _gameHudView;
    private readonly AuthFlowService _authFlowService;
    private readonly IAdsService _adsService;
    private readonly GameFlowSystem _gameFlowSystem;
    private readonly PlayerStateMachineSystem _playerStateMachineSystem;
    private readonly PlayerScoreSystem _playerScoreSystem;

    public GameUIService(
        MainMenuWindow mainMenuWindow,
        GamePopupService gamePopupService,
        GameHUDView gameHudView,
        AuthFlowService authFlowService,
        IAdsService adsService,
        PlayerStateMachineSystem playerStateMachineSystem,
        GameFlowSystem gameFlowSystem,
        PlayerScoreSystem playerScoreSystem)
    {
        _mainMenuWindow = mainMenuWindow;
        _gamePopupService = gamePopupService;
        _gameHudView = gameHudView;
        _authFlowService = authFlowService;
        _adsService = adsService;
        _playerStateMachineSystem = playerStateMachineSystem;
        _gameFlowSystem = gameFlowSystem;
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
        _playerStateMachineSystem.StateChanged -= OnPlayerStateChanged;
    }

    private void OnGameLoopStateChanged(EGameLoopState state)
    {
        ApplyState(state);
    }

    private void OnPlayerStateChanged(EPlayerState state)
    {
        if (state != EPlayerState.Dead)
        {
            return;
        }

        HandlePlayerDefeat();
    }

    private void HandlePlayerDefeat()
    {
        if (_gameFlowSystem.CurrentState == EGameLoopState.Defeat)
        {
            return;
        }

        DefeatPopup defeatPopup = _gamePopupService.ShowDefeatPopup();

        defeatPopup.SetScore(_playerScoreSystem.CurrentScore);
        defeatPopup.SetWatchAdButtonState(_adsService.IsRewardedAdReady);

        _gameFlowSystem.ShowDefeat();
    }

    private void ApplyState(EGameLoopState state)
    {
        switch (state)
        {
            case EGameLoopState.MainMenu:
                if (_authFlowService.IsAuthScreenActive == false)
                {
                    _mainMenuWindow.Show();
                }

                _gamePopupService.HideDefeatPopup();
                _gameHudView.Hide();
                break;

            case EGameLoopState.Playing:
                _mainMenuWindow.Hide();
                _gamePopupService.HideDefeatPopup();
                _gameHudView.Show();
                break;

            case EGameLoopState.Defeat:
                _mainMenuWindow.Hide();
                _gameHudView.Hide();
                break;
        }
    }
}