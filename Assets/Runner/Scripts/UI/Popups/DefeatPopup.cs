using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DefeatPopup : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _watchAdButton;
    [SerializeField] private TextMeshProUGUI _scoreValueText;

    private PlayerRespawnSystem _playerRespawnSystem;
    private GameFlowSystem _gameFlowSystem;

    public event Action WatchAdClicked;

    [Inject]
    public void Construct(
        PlayerRespawnSystem playerRespawnSystem,
        GameFlowSystem gameFlowSystem)
    {
        _playerRespawnSystem = playerRespawnSystem;
        _gameFlowSystem = gameFlowSystem;
    }

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(OnRestartClicked);
        _mainMenuButton.onClick.AddListener(OnExitClicked);
        _watchAdButton.onClick.AddListener(OnWatchAdButtonClicked);
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(OnRestartClicked);
        _mainMenuButton.onClick.RemoveListener(OnExitClicked);
        _watchAdButton.onClick.RemoveListener(OnWatchAdButtonClicked);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetScore(int score)
    {
        _scoreValueText.text = $"SCORE: {score}";
    }

    public void SetWatchAdButtonState(bool isEnabled)
    {
        _watchAdButton.interactable = isEnabled;
    }

    private void OnRestartClicked()
    {
        _playerRespawnSystem.Respawn();
        _gameFlowSystem.StartGame();
    }

    private void OnExitClicked()
    {
        _playerRespawnSystem.Respawn();
        _gameFlowSystem.EnterMainMenu();
    }

    private void OnWatchAdButtonClicked()
    {
        WatchAdClicked?.Invoke();
    }
}