using UnityEngine;
using Zenject;

public class PlayerCollisionView : MonoBehaviour
{
    [SerializeField] private LayerMask _deathLayerMask;

    private PlayerStateMachineSystem _playerStateMachineSystem;
    private RunnerGameConfig _runnerGameConfig;

    private float _invulnerabilityTimer;

    [Inject]
    public void Construct(
        PlayerStateMachineSystem playerStateMachineSystem,
        RunnerGameConfig runnerGameConfig)
    {
        _playerStateMachineSystem = playerStateMachineSystem;
        _runnerGameConfig = runnerGameConfig;
    }

    private void Update()
    {
        if (_invulnerabilityTimer <= 0f)
        {
            return;
        }

        _invulnerabilityTimer -= Time.deltaTime;
    }

    public void EnableReviveInvulnerability()
    {
        _invulnerabilityTimer = _runnerGameConfig.ReviveInvulnerabilitySeconds;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_invulnerabilityTimer > 0f)
        {
            return;
        }

        if (_playerStateMachineSystem.CurrentStateType == EPlayerState.Dead)
        {
            return;
        }

        ObstacleView obstacleView = other.GetComponentInParent<ObstacleView>();

        if (obstacleView == null)
        {
            return;
        }

        int layer = obstacleView.gameObject.layer;
        int otherLayerBit = 1 << layer;

        if ((_deathLayerMask.value & otherLayerBit) == 0)
        {
            return;
        }

        _playerStateMachineSystem.SetDead();
    }
}