using UnityEngine;

public class ScoreDebugView : MonoBehaviour
{
    private PlayerScoreSystem _playerScoreSystem;

    private float _timer;

    private const float LogIntervalSeconds = 1f;

    public void Construct(PlayerScoreSystem playerScoreSystem)
    {
        _playerScoreSystem = playerScoreSystem;
    }

    private void Update()
    {
        if (_playerScoreSystem == null)
        {
            return;
        }

        _timer += Time.deltaTime;

        if (_timer < LogIntervalSeconds)
        {
            return;
        }

        _timer = 0f;

        Debug.Log($"Score: {_playerScoreSystem.Score}");
    }
}