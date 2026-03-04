using UnityEngine;

public class PlayerScoreUpdateView : MonoBehaviour
{
    private PlayerScoreSystem _playerScoreSystem;
    private float _lastZPosition;

    public void Construct(PlayerScoreSystem playerScoreSystem)
    {
        _playerScoreSystem = playerScoreSystem;
        _lastZPosition = transform.position.z;
    }

    public void ResetTracking()
    {
        _lastZPosition = transform.position.z;
    }

    private void Update()
    {
        if (_playerScoreSystem == null)
        {
            return;
        }

        float currentZ = transform.position.z;
        float delta = currentZ - _lastZPosition;

        _playerScoreSystem.UpdateScore(delta);

        _lastZPosition = currentZ;
    }
}