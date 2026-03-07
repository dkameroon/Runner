using UnityEngine;
using Zenject;

public class PlayerCollisionView : MonoBehaviour
{
    [SerializeField] private LayerMask _deathLayerMask;

    private PlayerStateMachineSystem _playerStateMachineSystem;

    [Inject]
    public void Construct(PlayerStateMachineSystem playerStateMachineSystem)
    {
        _playerStateMachineSystem = playerStateMachineSystem;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter" +  other.gameObject.name);
        if (_playerStateMachineSystem.CurrentStateType == EPlayerState.Dead)
            return;

        ObstacleView obstacleView = other.GetComponentInParent<ObstacleView>();
        if (obstacleView == null)
            return;

        int layer = obstacleView.gameObject.layer;

        int otherLayerBit = 1 << layer;
        if ((_deathLayerMask.value & otherLayerBit) == 0)
            return;

        _playerStateMachineSystem.SetDead();
    }
}