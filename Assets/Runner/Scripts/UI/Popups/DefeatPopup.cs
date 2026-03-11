using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefeatPopup : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _watchAdButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private TextMeshProUGUI _scoreValueText;

    public event Action RestartClicked;
    public event Action MainMenuClicked;
    public event Action WatchAdClicked;
    public event Action SettingsClicked;

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(OnRestartClicked);
        _mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        _watchAdButton.onClick.AddListener(OnWatchAdButtonClicked);
        _settingsButton.onClick.AddListener(OnSettingsClicked);
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(OnRestartClicked);
        _mainMenuButton.onClick.RemoveListener(OnMainMenuClicked);
        _watchAdButton.onClick.RemoveListener(OnWatchAdButtonClicked);
        _settingsButton.onClick.RemoveListener(OnSettingsClicked);
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
        RestartClicked?.Invoke();
    }

    private void OnMainMenuClicked()
    {
        MainMenuClicked?.Invoke();
    }

    private void OnWatchAdButtonClicked()
    {
        WatchAdClicked?.Invoke();
    }

    private void OnSettingsClicked()
    {
        SettingsClicked?.Invoke();
    }
}