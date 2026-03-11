using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PausePopup : MonoBehaviour
{
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private TextMeshProUGUI _titleText;

    public event Action ResumeClicked;
    public event Action RestartClicked;
    public event Action MainMenuClicked;
    public event Action SettingsClicked;

    private void OnEnable()
    {
        _resumeButton.onClick.AddListener(OnResumeClicked);
        _restartButton.onClick.AddListener(OnRestartClicked);
        _mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        _settingsButton.onClick.AddListener(OnSettingsClicked);
    }

    private void OnDisable()
    {
        _resumeButton.onClick.RemoveListener(OnResumeClicked);
        _restartButton.onClick.RemoveListener(OnRestartClicked);
        _mainMenuButton.onClick.RemoveListener(OnMainMenuClicked);
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

    private void OnResumeClicked()
    {
        ResumeClicked?.Invoke();
    }

    private void OnRestartClicked()
    {
        RestartClicked?.Invoke();
    }

    private void OnMainMenuClicked()
    {
        MainMenuClicked?.Invoke();
    }

    private void OnSettingsClicked()
    {
        SettingsClicked?.Invoke();
    }
}