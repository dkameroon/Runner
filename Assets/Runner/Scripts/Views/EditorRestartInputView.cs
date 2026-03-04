using UnityEngine;

public class EditorRestartInputView : MonoBehaviour
{
    private PlayerRespawnSystem _playerRespawnSystem;

    public void Construct(PlayerRespawnSystem playerRespawnSystem)
    {
        _playerRespawnSystem = playerRespawnSystem;
    }

    private void Update()
    {
        if (_playerRespawnSystem == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _playerRespawnSystem.Respawn();
        }
    }
}