using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerCollisionView : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _movementToggleBehaviour;

    private IMovementToggle _movementToggle;
    private PlayerDeathSystem _playerDeathSystem;

    public void Construct(PlayerDeathSystem playerDeathSystem)
    {
        _playerDeathSystem = playerDeathSystem;

        _movementToggle = _movementToggleBehaviour as IMovementToggle;
        if (_movementToggle == null)
        {
            Debug.LogError("PlayerCollisionView: _movementToggleBehaviour does not implement IMovementToggle.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_playerDeathSystem == null || _movementToggle == null)
        {
            return;
        }

        if (_playerDeathSystem.IsDead)
        {
            return;
        }

        if (!other.TryGetComponent(out ObstacleView obstacleView))
        {
            return;
        }

        _playerDeathSystem.Kill();
        _movementToggle.SetMovementEnabled(false);

        Debug.Log($"Player died by obstacle type: {obstacleView.ObstacleType}");
    }
}