using System;
using UnityEngine;

public class AudioSettingsService
{
    private const string MusicEnabledPlayerPrefsKey = "AudioSettings.MusicEnabled";
    private const string SoundEnabledPlayerPrefsKey = "AudioSettings.SoundEnabled";
    private const string MusicVolumePlayerPrefsKey = "AudioSettings.MusicVolume";
    private const string SoundVolumePlayerPrefsKey = "AudioSettings.SoundVolume";

    private readonly AudioConfig _audioConfig;

    public bool IsMusicEnabled { get; private set; }
    public bool IsSoundEnabled { get; private set; }
    public float MusicVolume { get; private set; }
    public float SoundVolume { get; private set; }

    public event Action SettingsChanged;

    public AudioSettingsService(AudioConfig audioConfig)
    {
        _audioConfig = audioConfig;

        LoadSettings();
    }

    public void SetMusicEnabled(bool isEnabled)
    {
        if (IsMusicEnabled == isEnabled)
        {
            return;
        }

        IsMusicEnabled = isEnabled;
        SaveSettings();
        SettingsChanged?.Invoke();
    }

    public void SetSoundEnabled(bool isEnabled)
    {
        if (IsSoundEnabled == isEnabled)
        {
            return;
        }

        IsSoundEnabled = isEnabled;
        SaveSettings();
        SettingsChanged?.Invoke();
    }

    public void SetMusicVolume(float volume)
    {
        float clampedVolume = Mathf.Clamp01(volume);

        if (Mathf.Approximately(MusicVolume, clampedVolume))
        {
            return;
        }

        MusicVolume = clampedVolume;
        SaveSettings();
        SettingsChanged?.Invoke();
    }

    public void SetSoundVolume(float volume)
    {
        float clampedVolume = Mathf.Clamp01(volume);

        if (Mathf.Approximately(SoundVolume, clampedVolume))
        {
            return;
        }

        SoundVolume = clampedVolume;
        SaveSettings();
        SettingsChanged?.Invoke();
    }

    private void LoadSettings()
    {
        IsMusicEnabled = PlayerPrefs.GetInt(MusicEnabledPlayerPrefsKey, 1) == 1;
        IsSoundEnabled = PlayerPrefs.GetInt(SoundEnabledPlayerPrefsKey, 1) == 1;

        MusicVolume = PlayerPrefs.GetFloat(MusicVolumePlayerPrefsKey, _audioConfig.DefaultMusicVolume);
        SoundVolume = PlayerPrefs.GetFloat(SoundVolumePlayerPrefsKey, _audioConfig.DefaultSoundVolume);

        MusicVolume = Mathf.Clamp01(MusicVolume);
        SoundVolume = Mathf.Clamp01(SoundVolume);
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetInt(MusicEnabledPlayerPrefsKey, IsMusicEnabled ? 1 : 0);
        PlayerPrefs.SetInt(SoundEnabledPlayerPrefsKey, IsSoundEnabled ? 1 : 0);
        PlayerPrefs.SetFloat(MusicVolumePlayerPrefsKey, MusicVolume);
        PlayerPrefs.SetFloat(SoundVolumePlayerPrefsKey, SoundVolume);
        PlayerPrefs.Save();
    }
}