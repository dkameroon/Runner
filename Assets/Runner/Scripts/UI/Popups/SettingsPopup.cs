using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour
{
    [SerializeField] private Toggle _musicEnabledToggle;
    [SerializeField] private Toggle _soundEnabledToggle;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _soundVolumeSlider;
    [SerializeField] private Button _closeButton;

    public event Action<bool> MusicEnabledChanged;
    public event Action<bool> SoundEnabledChanged;
    public event Action<float> MusicVolumeChanged;
    public event Action<float> SoundVolumeChanged;
    public event Action CloseClicked;

    private void OnEnable()
    {
        _musicEnabledToggle.onValueChanged.AddListener(OnMusicEnabledChanged);
        _soundEnabledToggle.onValueChanged.AddListener(OnSoundEnabledChanged);
        _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        _soundVolumeSlider.onValueChanged.AddListener(OnSoundVolumeChanged);
        _closeButton.onClick.AddListener(OnCloseClicked);
    }

    private void OnDisable()
    {
        _musicEnabledToggle.onValueChanged.RemoveListener(OnMusicEnabledChanged);
        _soundEnabledToggle.onValueChanged.RemoveListener(OnSoundEnabledChanged);
        _musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        _soundVolumeSlider.onValueChanged.RemoveListener(OnSoundVolumeChanged);
        _closeButton.onClick.RemoveListener(OnCloseClicked);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetMusicEnabled(bool isEnabled)
    {
        _musicEnabledToggle.SetIsOnWithoutNotify(isEnabled);
    }

    public void SetSoundEnabled(bool isEnabled)
    {
        _soundEnabledToggle.SetIsOnWithoutNotify(isEnabled);
    }

    public void SetMusicVolume(float volume)
    {
        _musicVolumeSlider.SetValueWithoutNotify(volume);
    }

    public void SetSoundVolume(float volume)
    {
        _soundVolumeSlider.SetValueWithoutNotify(volume);
    }

    private void OnMusicEnabledChanged(bool isEnabled)
    {
        MusicEnabledChanged?.Invoke(isEnabled);
    }

    private void OnSoundEnabledChanged(bool isEnabled)
    {
        SoundEnabledChanged?.Invoke(isEnabled);
    }

    private void OnMusicVolumeChanged(float volume)
    {
        MusicVolumeChanged?.Invoke(volume);
    }

    private void OnSoundVolumeChanged(float volume)
    {
        SoundVolumeChanged?.Invoke(volume);
    }

    private void OnCloseClicked()
    {
        CloseClicked?.Invoke();
    }
}