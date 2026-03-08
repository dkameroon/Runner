using TMPro;
using UnityEngine;

public class DebugOverlayView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _fpsText;
    [SerializeField] private TextMeshProUGUI _frameTimeText;
    [SerializeField] private TextMeshProUGUI _speedText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _obstaclesText;
    [SerializeField] private TextMeshProUGUI _segmentsText;
    [SerializeField] private TextMeshProUGUI _playerStateText;

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void SetFps(int fps)
    {
        _fpsText.text = $"FPS: {fps}";
    }

    public void SetFrameTime(float frameTimeMilliseconds)
    {
        _frameTimeText.text = $"Frame Time: {frameTimeMilliseconds:F1} ms";
    }

    public void SetSpeed(float speed)
    {
        _speedText.text = $"Speed: {speed:F2}";
    }

    public void SetScore(int score)
    {
        _scoreText.text = $"Score: {score}";
    }

    public void SetObstaclesCount(int obstaclesCount)
    {
        _obstaclesText.text = $"Obstacles: {obstaclesCount}";
    }

    public void SetSegmentsCount(int segmentsCount)
    {
        _segmentsText.text = $"Segments: {segmentsCount}";
    }

    public void SetPlayerState(string playerState)
    {
        _playerStateText.text = $"State: {playerState}";
    }
}