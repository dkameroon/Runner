using System;
using Zenject;

public class PauseFlowService : IInitializable, IDisposable
{
    private readonly PauseButtonView _pauseButtonView;
    private readonly PopupFactory _popupFactory;
    private readonly GameFlowSystem _gameFlowSystem;
    private readonly PlayerRespawnSystem _playerRespawnSystem;

    private PausePopup _pausePopup;

    public PauseFlowService(
        PauseButtonView pauseButtonView,
        PopupFactory popupFactory,
        GameFlowSystem gameFlowSystem,
        PlayerRespawnSystem playerRespawnSystem)
    {
        _pauseButtonView = pauseButtonView;
        _popupFactory = popupFactory;
        _gameFlowSystem = gameFlowSystem;
        _playerRespawnSystem = playerRespawnSystem;
    }

    public void Initialize()
    {
        _pausePopup = _popupFactory.CreatePausePopup();
        _pausePopup.Hide();

        _pauseButtonView.Clicked += OnPauseClicked;
        _pausePopup.ResumeClicked += OnResumeClicked;
        _pausePopup.RestartClicked += OnRestartClicked;
        _pausePopup.MainMenuClicked += OnMainMenuClicked;
        _gameFlowSystem.StateChanged += OnGameStateChanged;

        _pauseButtonView.SetVisible(false);
    }

    public void Dispose()
    {
        _pauseButtonView.Clicked -= OnPauseClicked;
        _gameFlowSystem.StateChanged -= OnGameStateChanged;

        if (_pausePopup == null)
        {
            return;
        }

        _pausePopup.ResumeClicked -= OnResumeClicked;
        _pausePopup.RestartClicked -= OnRestartClicked;
        _pausePopup.MainMenuClicked -= OnMainMenuClicked;
    }

    private void OnPauseClicked()
    {
        _gameFlowSystem.PauseGame();
    }

    private void OnResumeClicked()
    {
        _gameFlowSystem.ResumeGameFromPause();
    }

    private void OnRestartClicked()
    {
        _playerRespawnSystem.Respawn();
        _gameFlowSystem.StartGame();
    }

    private void OnMainMenuClicked()
    {
        _playerRespawnSystem.Respawn();
        _gameFlowSystem.EnterMainMenu();
    }

    private void OnGameStateChanged(EGameLoopState state)
    {
        bool isPlaying = state == EGameLoopState.Playing;
        bool isPaused = state == EGameLoopState.Paused;

        _pauseButtonView.SetVisible(isPlaying);

        if (_pausePopup == null)
        {
            return;
        }

        if (isPaused)
        {
            _pausePopup.Show();
            return;
        }

        _pausePopup.Hide();
    }
}