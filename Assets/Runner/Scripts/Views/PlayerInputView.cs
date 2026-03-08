using UnityEngine;
using Zenject;

public class PlayerInputView : MonoBehaviour
{
    [SerializeField] private bool _forceMobileInputInEditor;

    private PlayerView _playerView;
    private PlayerStateMachineSystem _playerStateMachineSystem;

    private IPlayerInputStrategy _inputStrategy;

    [Inject]
    public void Construct(
        PlayerView playerView,
        PlayerStateMachineSystem playerStateMachineSystem)
    {
        _playerView = playerView;
        _playerStateMachineSystem = playerStateMachineSystem;

#if UNITY_EDITOR
        _inputStrategy = _forceMobileInputInEditor
            ? new MobileSwipeInputStrategy()
            : new EditorKeyboardInputStrategy();
#else
        _inputStrategy = Application.isMobilePlatform
            ? new MobileSwipeInputStrategy()
            : new EditorKeyboardInputStrategy();
#endif

        _inputStrategy.CommandTriggered += OnCommandTriggered;
    }

    private void OnDestroy()
    {
        if (_inputStrategy != null)
        {
            _inputStrategy.CommandTriggered -= OnCommandTriggered;
        }
    }

    private void Update()
    {
        if (_inputStrategy == null || _playerStateMachineSystem == null)
            return;

        if (_playerStateMachineSystem.CanProcessInput() == false)
            return;

        _inputStrategy.Tick();
    }

    private void OnCommandTriggered(EPlayerInputCommand command)
    {
        if (_playerView == null || _playerStateMachineSystem == null)
            return;

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