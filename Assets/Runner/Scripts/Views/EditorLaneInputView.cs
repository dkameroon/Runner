using UnityEngine;

public class EditorLaneInputView : MonoBehaviour
{
    [SerializeField] private PlayerView _playerView;

    private void Update()
    {
        if (_playerView == null)
        {
            Debug.LogError("PlayerView is not assigned");
            return;
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _playerView.TryChangeLane(-1);
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _playerView.TryChangeLane(1);
        }
    }
}