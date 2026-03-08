using UnityEngine;
using Zenject;

public class EditorDebugRestartInputView : MonoBehaviour
{
    private PlayerRespawnSystem _playerRespawnSystem;
    private bool _isMissingDependencyLogged;

    [Inject]
    public void Construct(PlayerRespawnSystem playerRespawnSystem)
    {
        _playerRespawnSystem = playerRespawnSystem;
    }

    private void Update()
    {
        if (_playerRespawnSystem == null)
        {
            if (_isMissingDependencyLogged == false)
            {
                Debug.LogError("EditorDebugRestartInputView: PlayerRespawnSystem is not resolved.");
                _isMissingDependencyLogged = true;
            }

            enabled = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) == false)
            return;

        _playerRespawnSystem.Respawn();
    }
}