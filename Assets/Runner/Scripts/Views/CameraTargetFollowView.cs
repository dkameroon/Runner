using UnityEngine;
using Zenject;

public class CameraTargetFollowView : MonoBehaviour, ICameraRespawnSync
{
    [SerializeField] private Transform _player;
    [SerializeField] private bool _followY;
    
    private void LateUpdate()
    {
        Follow();
    }

    public void SnapToPlayer()
    {
        Follow();
    }

    private void Follow()
    {
        if (_player == null)
        {
            return;
        }

        Vector3 position = transform.position;
        position.z = _player.position.z;

        if (_followY)
        {
            position.y = _player.position.y;
        }

        transform.position = position;
    }
}