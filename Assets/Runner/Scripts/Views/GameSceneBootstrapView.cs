using UnityEngine;

public class GameSceneBootstrapView : MonoBehaviour
{
    [SerializeField] private RunnerGameConfig _runnerGameConfig;

    [SerializeField] private PlayerView _playerView;
    [SerializeField] private PlayerCollisionView _playerCollisionView;
    [SerializeField] private EditorRestartInputView _restartInputView;
    [SerializeField] private CameraTargetFollowView _cameraTargetFollowView;
    [SerializeField] private PlayerScoreUpdateView _playerScoreUpdateView;
    [SerializeField] private ScoreDebugView _scoreDebugView;

    private PlayerScoreSystem _playerScoreSystem;
    private PlayerDeathSystem _playerDeathSystem;

    private void Awake()
    {
        if (_runnerGameConfig == null)
        {
            Debug.LogError("GameSceneBootstrapView: RunnerGameConfig is not assigned.");
            return;
        }

        if (_playerView == null)
        {
            Debug.LogError("GameSceneBootstrapView: PlayerView is not assigned.");
            return;
        }

        if (_playerCollisionView == null)
        {
            Debug.LogError("GameSceneBootstrapView: PlayerCollisionView is not assigned.");
            return;
        }

        if (_playerScoreUpdateView == null)
        {
            Debug.LogError("GameSceneBootstrapView: PlayerScoreUpdateView is not assigned.");
            return;
        }

        _playerDeathSystem = new PlayerDeathSystem();
        _playerScoreSystem = new PlayerScoreSystem();

        _playerView.Construct(_runnerGameConfig);
        _playerCollisionView.Construct(_playerDeathSystem);

        _playerScoreUpdateView.Construct(_playerScoreSystem);

        if (_scoreDebugView != null)
        {
            _scoreDebugView.Construct(_playerScoreSystem);
        }

        PlayerRespawnSystem respawnSystem = new PlayerRespawnSystem(
            _playerView.transform,
            _playerDeathSystem,
            _playerView,
            _cameraTargetFollowView,
            _playerScoreSystem);

        if (_restartInputView != null)
        {
            _restartInputView.Construct(respawnSystem);
        }
        else
        {
            Debug.LogWarning("GameSceneBootstrapView: EditorRestartInputView is not assigned.");
        }
    }
}