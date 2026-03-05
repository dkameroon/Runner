using UnityEngine;
using Zenject;

public class PlayerScoreUpdateView : MonoBehaviour
{
    private PlayerScoreSystem _playerScoreSystem;
    private float _lastZPosition;

    [Inject]
    public void Construct(PlayerScoreSystem playerScoreSystem)
    {
        _playerScoreSystem = playerScoreSystem;
        ResetTracking();
    }

    public void ResetTracking()
    {
        _lastZPosition = transform.position.z;
    }

    private void Update()
    {
        float currentZPosition = transform.position.z;
        float deltaMeters = currentZPosition - _lastZPosition;

        _playerScoreSystem.AddDistance(deltaMeters);

        _lastZPosition = currentZPosition;
    }
}