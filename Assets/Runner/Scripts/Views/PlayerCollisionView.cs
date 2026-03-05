using UnityEngine;
using Zenject;

[RequireComponent(typeof(Collider))]
public class PlayerCollisionView : MonoBehaviour
{
    private PlayerStateMachineSystem _playerStateMachineSystem;

    [Inject]
    public void Construct(PlayerStateMachineSystem playerStateMachineSystem)
    {
        _playerStateMachineSystem = playerStateMachineSystem;
    }

    private void OnTriggerEnter(Collider other)
    {
        _playerStateMachineSystem.SetDead();
        
    }
}