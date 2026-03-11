using UnityEngine;
using Zenject;

public class PlayerScoreUpdateView : MonoBehaviour
{
    private PlayerScoreSystem _playerScoreSystem;
    private GameplaySessionService _gameplaySessionService;

    private float _lastTrackedZPosition;

    [Inject]
    public void Construct(
        PlayerScoreSystem playerScoreSystem,
        GameplaySessionService gameplaySessionService)
    {
        _playerScoreSystem = playerScoreSystem;
        _gameplaySessionService = gameplaySessionService;
    }

    private void Awake()
    {
        ResetTracking();
    }

    private void OnEnable()
    {
        ResetTracking();
    }

    public void ResetTracking()
    {
        _lastTrackedZPosition = transform.position.z;
    }

    private void Update()
    {
        float currentZPosition = transform.position.z;

        if (!_gameplaySessionService.IsGameplayActive)
        {
            _lastTrackedZPosition = currentZPosition;
            return;
        }

        float deltaDistance = currentZPosition - _lastTrackedZPosition;

        if (deltaDistance <= 0f)
        {
            _lastTrackedZPosition = currentZPosition;
            return;
        }

        _playerScoreSystem.AddDistance(deltaDistance);

        _lastTrackedZPosition = currentZPosition;
    }
}