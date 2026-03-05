using UnityEngine;
using Zenject;

public class EditorLaneInputView : MonoBehaviour
{
    private PlayerView _playerView;
    private PlayerStateMachineSystem _playerStateMachineSystem;

    [Inject]
    public void Construct(PlayerView playerView, PlayerStateMachineSystem playerStateMachineSystem)
    {
        _playerView = playerView;
        _playerStateMachineSystem = playerStateMachineSystem;
    }

    private void Update()
    {
        if (_playerStateMachineSystem.CurrentStateType == EPlayerState.Dead ||
            _playerStateMachineSystem.CurrentStateType == EPlayerState.Idle)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _playerView.TryChangeLane(-1);
            return;
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _playerView.TryChangeLane(1);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerStateMachineSystem.RequestJump();
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            _playerStateMachineSystem.RequestSlide();
        }
    }
}