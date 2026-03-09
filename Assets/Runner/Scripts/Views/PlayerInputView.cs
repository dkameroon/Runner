using UnityEngine;
using Zenject;

public class PlayerInputView : MonoBehaviour
{
    private PlayerView _playerView;
    private PlayerStateMachineSystem _playerStateMachineSystem;
    private IPlayerInputStrategy _inputStrategy;

    [Inject]
    public void Construct(
        PlayerView playerView,
        PlayerStateMachineSystem playerStateMachineSystem,
        IPlayerInputStrategy inputStrategy)
    {
        _playerView = playerView;
        _playerStateMachineSystem = playerStateMachineSystem;
        _inputStrategy = inputStrategy;

        _inputStrategy.CommandTriggered += OnCommandTriggered;
    }

    private void OnDestroy()
    {
        if (_inputStrategy == null)
        {
            return;
        }

        _inputStrategy.CommandTriggered -= OnCommandTriggered;
    }

    private void Update()
    {
        if (_inputStrategy == null || _playerStateMachineSystem == null)
        {
            return;
        }

        if (_playerStateMachineSystem.CanProcessInput() == false)
        {
            return;
        }

        _inputStrategy.Tick();
    }

    private void OnCommandTriggered(EPlayerInputCommand command)
    {
        if (_playerView == null || _playerStateMachineSystem == null)
        {
            return;
        }

        switch (command)
        {
            case EPlayerInputCommand.LaneLeft:
                _playerView.TryChangeLane(-1);
                break;

            case EPlayerInputCommand.LaneRight:
                _playerView.TryChangeLane(1);
                break;

            case EPlayerInputCommand.Jump:
                _playerStateMachineSystem.RequestJump();
                break;

            case EPlayerInputCommand.Slide:
                _playerStateMachineSystem.RequestSlide();
                break;
        }
    }
}