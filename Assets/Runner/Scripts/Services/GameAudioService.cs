using System;
using Zenject;

public class GameAudioService : IInitializable, IDisposable
{
    private readonly AudioSettingsService _audioSettingsService;
    private readonly AudioPlayerView _audioPlayerView;
    private readonly AudioConfig _audioConfig;

    public GameAudioService(
        AudioSettingsService audioSettingsService,
        AudioPlayerView audioPlayerView,
        AudioConfig audioConfig)
    {
        _audioSettingsService = audioSettingsService;
        _audioPlayerView = audioPlayerView;
        _audioConfig = audioConfig;
    }

    public void Initialize()
    {
        _audioSettingsService.SettingsChanged += OnSettingsChanged;

        StartMusicIfNeeded();
        ApplySettings();
    }

    public void Dispose()
    {
        _audioSettingsService.SettingsChanged -= OnSettingsChanged;
    }

    public void PlayButtonClick()
    {
        if (_audioSettingsService.IsSoundEnabled == false)
        {
            return;
        }

        _audioPlayerView.PlaySound(_audioConfig.ButtonClickSound, _audioSettingsService.SoundVolume);
    }

    public void StartMusicIfNeeded()
    {
        _audioPlayerView.PlayMusicLoop(_audioConfig.BackgroundMusic);
    }

    private void OnSettingsChanged()
    {
        ApplySettings();
    }

    private void ApplySettings()
    {
        ApplyMusicSettings();
        ApplySoundSettings();
    }

    private void ApplyMusicSettings()
    {
        float targetMusicVolume = _audioSettingsService.IsMusicEnabled
            ? _audioSettingsService.MusicVolume
            : 0f;

        _audioPlayerView.SetMusicVolume(targetMusicVolume);
    }

    private void ApplySoundSettings()
    {
        float targetSoundVolume = _audioSettingsService.IsSoundEnabled
            ? _audioSettingsService.SoundVolume
            : 0f;

        _audioPlayerView.SetSoundVolume(targetSoundVolume);
    }
}