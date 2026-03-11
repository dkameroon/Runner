using UnityEngine;
using Zenject;

public class PlayerInputView : MonoBehaviour
{
    private PlayerMovementSystem _playerMovementSystem;
    private PlayerStateMachineSystem _playerStateMachineSystem;
    private GameFlowSystem _gameFlowSystem;
    private IPlayerInputStrategy _inputStrategy;

    [Inject]
    public void Construct(
        PlayerMovementSystem playerMovementSystem,
        PlayerStateMachineSystem playerStateMachineSystem,
        GameFlowSystem gameFlowSystem,
        IPlayerInputStrategy inputStrategy)
    {
        _playerMovementSystem = playerMovementSystem;
        _playerStateMachineSystem = playerStateMachineSystem;
        _gameFlowSystem = gameFlowSystem;
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
        if (_inputStrategy == null || _playerStateMachineSystem == null || _gameFlowSystem == null)
        {
            return;
        }

        if (_gameFlowSystem.CurrentState != EGameLoopState.Playing)
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
        if (_playerMovementSystem == null || _playerStateMachineSystem == null || _gameFlowSystem == null)
        {
            return;
        }

        if (_gameFlowSystem.CurrentState != EGameLoopState.Playing)
        {
            return;
        }

        switch (command)
        {
            case EPlayerInputCommand.LaneLeft:
                _playerMovementSystem.TryChangeLane(-1);
                break;

            case EPlayerInputCommand.LaneRight:
                _playerMovementSystem.TryChangeLane(1);
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