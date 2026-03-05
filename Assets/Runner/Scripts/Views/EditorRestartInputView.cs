using UnityEngine;
using Zenject;

public class EditorRestartInputView : MonoBehaviour
{
    private PlayerRespawnSystem _playerRespawnSystem;
    private SpeedSystem _speedSystem;
    private PlayerScoreSystem _playerScoreSystem;
    private PlayerScoreUpdateView _playerScoreUpdateView;
    private PlayerStateMachineSystem _playerStateMachineSystem;

    [Inject]
    public void Construct(
        PlayerRespawnSystem playerRespawnSystem,
        SpeedSystem speedSystem,
        PlayerScoreSystem playerScoreSystem,
        PlayerScoreUpdateView playerScoreUpdateView,
        PlayerStateMachineSystem playerStateMachineSystem)
    {
        _playerRespawnSystem = playerRespawnSystem;
        _speedSystem = speedSystem;
        _playerScoreSystem = playerScoreSystem;
        _playerScoreUpdateView = playerScoreUpdateView;
        _playerStateMachineSystem = playerStateMachineSystem;
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.R))
            return;

        _playerRespawnSystem.Respawn();

        _playerScoreSystem.Reset();
        _playerScoreUpdateView.ResetTracking();

        _speedSystem.ResetSpeed();

        _playerStateMachineSystem.SetRun();
    }
}