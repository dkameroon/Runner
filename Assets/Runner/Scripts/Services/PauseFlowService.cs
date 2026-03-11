using System;
using Zenject;

public class PauseFlowService : IInitializable, IDisposable
{
    private readonly PauseButtonView _pauseButtonView;
    private readonly GamePopupService _gamePopupService;
    private readonly GameFlowSystem _gameFlowSystem;
    private readonly PlayerRespawnSystem _playerRespawnSystem;

    private PausePopup _pausePopup;
    private bool _isProcessingPopupAction;

    public PauseFlowService(
        PauseButtonView pauseButtonView,
        GamePopupService gamePopupService,
        GameFlowSystem gameFlowSystem,
        PlayerRespawnSystem playerRespawnSystem)
    {
        _pauseButtonView = pauseButtonView;
        _gamePopupService = gamePopupService;
        _gameFlowSystem = gameFlowSystem;
        _playerRespawnSystem = playerRespawnSystem;
    }

    public void Initialize()
    {
        _pauseButtonView.Clicked += OnPauseClicked;
        _gamePopupService.PausePopupShown += OnPausePopupShown;
        _gamePopupService.PausePopupHidden += OnPausePopupHidden;
        _gameFlowSystem.StateChanged += OnGameStateChanged;

        _pauseButtonView.SetVisible(false);
        _isProcessingPopupAction = false;
    }

    public void Dispose()
    {
        _pauseButtonView.Clicked -= OnPauseClicked;
        _gamePopupService.PausePopupShown -= OnPausePopupShown;
        _gamePopupService.PausePopupHidden -= OnPausePopupHidden;
        _gameFlowSystem.StateChanged -= OnGameStateChanged;

        UnsubscribeFromPausePopup();
    }

    private void OnPauseClicked()
    {
        if (_gameFlowSystem.CurrentState != EGameLoopState.Playing)
        {
            return;
        }

        _gameFlowSystem.PauseGame();
    }

    private void OnPausePopupShown(PausePopup pausePopup)
    {
        if (pausePopup == null)
        {
            return;
        }

        if (_pausePopup == pausePopup)
        {
            return;
        }

        UnsubscribeFromPausePopup();

        _pausePopup = pausePopup;
        _pausePopup.ResumeClicked += OnResumeClicked;
        _pausePopup.RestartClicked += OnRestartClicked;
        _pausePopup.MainMenuClicked += OnMainMenuClicked;

        _isProcessingPopupAction = false;
    }

    private void OnPausePopupHidden()
    {
        _isProcessingPopupAction = false;
        UnsubscribeFromPausePopup();
    }

    private void UnsubscribeFromPausePopup()
    {
        if (_pausePopup == null)
        {
            return;
        }

        _pausePopup.ResumeClicked -= OnResumeClicked;
        _pausePopup.RestartClicked -= OnRestartClicked;
        _pausePopup.MainMenuClicked -= OnMainMenuClicked;
        _pausePopup = null;
    }

    private void OnResumeClicked()
    {
        if (TryBeginPopupAction() == false)
        {
            return;
        }

        _gamePopupService.HidePausePopup();
        _gameFlowSystem.ResumeGameFromPause();
    }

    private void OnRestartClicked()
    {
        if (TryBeginPopupAction() == false)
        {
            return;
        }

        _gamePopupService.HidePausePopup();
        _playerRespawnSystem.Respawn();
        _gameFlowSystem.StartGame();
    }

    private void OnMainMenuClicked()
    {
        if (TryBeginPopupAction() == false)
        {
            return;
        }

        _gamePopupService.HidePausePopup();
        _playerRespawnSystem.Respawn();
        _gameFlowSystem.EnterMainMenu();
    }

    private bool TryBeginPopupAction()
    {
        if (_pausePopup == null)
        {
            return false;
        }

        if (_isProcessingPopupAction)
        {
            return false;
        }

        _isProcessingPopupAction = true;
        return true;
    }

    private void OnGameStateChanged(EGameLoopState state)
    {
        bool isPlaying = state == EGameLoopState.Playing;
        bool isPaused = state == EGameLoopState.Paused;

        _pauseButtonView.SetVisible(isPlaying);

        if (isPaused)
        {
            _gamePopupService.ShowPausePopup();
            return;
        }

        _gamePopupService.HidePausePopup();
    }
}