using System;
using Zenject;

public class GameFlowSystem : IInitializable
{
    public EGameLoopState CurrentState { get; private set; }

    public event Action<EGameLoopState> StateChanged;

    private readonly PlayerGameStateService _playerGameStateService;

    public GameFlowSystem(PlayerGameStateService playerGameStateService)
    {
        _playerGameStateService = playerGameStateService;
    }

    public void Initialize()
    {
        CurrentState = EGameLoopState.None;
        EnterMainMenu();
    }

    public void EnterMainMenu()
    {
        if (CurrentState == EGameLoopState.MainMenu)
        {
            return;
        }

        SetState(EGameLoopState.MainMenu, _playerGameStateService.ApplyMainMenuState);
    }

    public void StartGame()
    {
        if (CurrentState == EGameLoopState.Playing)
        {
            return;
        }

        SetState(EGameLoopState.Playing, _playerGameStateService.ApplyPlayingState);
    }

    public void PauseGame()
    {
        if (CurrentState != EGameLoopState.Playing)
        {
            return;
        }

        SetState(EGameLoopState.Paused, _playerGameStateService.ApplyPausedState);
    }

    public void ResumeGameFromPause()
    {
        if (CurrentState != EGameLoopState.Paused)
        {
            return;
        }

        SetState(EGameLoopState.Playing, _playerGameStateService.ApplyResumeFromPauseState);
    }

    public void ShowDefeat()
    {
        if (CurrentState == EGameLoopState.Defeat)
        {
            return;
        }

        SetState(EGameLoopState.Defeat, _playerGameStateService.ApplyDefeatState);
    }

    public void ResumeGameAfterContinue()
    {
        if (CurrentState != EGameLoopState.Defeat)
        {
            return;
        }

        SetState(EGameLoopState.Playing, _playerGameStateService.ApplyContinueAfterDefeatState);
    }

    private void SetState(EGameLoopState newState, Action applyStateAction)
    {
        CurrentState = newState;
        applyStateAction.Invoke();
        StateChanged?.Invoke(CurrentState);
    }
}