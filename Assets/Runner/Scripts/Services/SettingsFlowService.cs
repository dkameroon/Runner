using System;
using Zenject;

public class SettingsFlowService : IInitializable, IDisposable
{
    private enum ESettingsOpenSource
    {
        None = 0,
        MainMenu = 1,
        PausePopup = 2,
        DefeatPopup = 3
    }

    private readonly MainMenuWindow _mainMenuWindow;
    private readonly GamePopupService _gamePopupService;
    private readonly AudioSettingsService _audioSettingsService;
    private readonly GameAudioService _gameAudioService;

    private PausePopup _pausePopup;
    private DefeatPopup _defeatPopup;
    private SettingsPopup _settingsPopup;

    private ESettingsOpenSource _settingsOpenSource;

    public SettingsFlowService(
        MainMenuWindow mainMenuWindow,
        GamePopupService gamePopupService,
        AudioSettingsService audioSettingsService,
        GameAudioService gameAudioService)
    {
        _mainMenuWindow = mainMenuWindow;
        _gamePopupService = gamePopupService;
        _audioSettingsService = audioSettingsService;
        _gameAudioService = gameAudioService;
    }

    public void Initialize()
    {
        _mainMenuWindow.SettingsClicked += OnMainMenuSettingsClicked;

        _gamePopupService.PausePopupShown += OnPausePopupShown;
        _gamePopupService.PausePopupHidden += OnPausePopupHidden;

        _gamePopupService.DefeatPopupShown += OnDefeatPopupShown;
        _gamePopupService.DefeatPopupHidden += OnDefeatPopupHidden;

        _gamePopupService.SettingsPopupShown += OnSettingsPopupShown;
        _gamePopupService.SettingsPopupHidden += OnSettingsPopupHidden;
    }

    public void Dispose()
    {
        _mainMenuWindow.SettingsClicked -= OnMainMenuSettingsClicked;

        _gamePopupService.PausePopupShown -= OnPausePopupShown;
        _gamePopupService.PausePopupHidden -= OnPausePopupHidden;

        _gamePopupService.DefeatPopupShown -= OnDefeatPopupShown;
        _gamePopupService.DefeatPopupHidden -= OnDefeatPopupHidden;

        _gamePopupService.SettingsPopupShown -= OnSettingsPopupShown;
        _gamePopupService.SettingsPopupHidden -= OnSettingsPopupHidden;

        UnsubscribeFromPausePopup();
        UnsubscribeFromDefeatPopup();
        UnsubscribeFromSettingsPopup();
    }

    private void OnMainMenuSettingsClicked()
    {
        _gameAudioService.PlayButtonClick();

        _settingsOpenSource = ESettingsOpenSource.MainMenu;
        _gamePopupService.ShowSettingsPopup();
    }

    private void OnPausePopupShown(PausePopup pausePopup)
    {
        if (pausePopup == null)
        {
            return;
        }

        if (_pausePopup == pausePopup)
        {
            return;
        }

        UnsubscribeFromPausePopup();

        _pausePopup = pausePopup;
        _pausePopup.SettingsClicked += OnPauseSettingsClicked;
    }

    private void OnPausePopupHidden()
    {
        UnsubscribeFromPausePopup();
    }

    private void OnDefeatPopupShown(DefeatPopup defeatPopup)
    {
        if (defeatPopup == null)
        {
            return;
        }

        if (_defeatPopup == defeatPopup)
        {
            return;
        }

        UnsubscribeFromDefeatPopup();

        _defeatPopup = defeatPopup;
        _defeatPopup.SettingsClicked += OnDefeatSettingsClicked;
    }

    private void OnDefeatPopupHidden()
    {
        UnsubscribeFromDefeatPopup();
    }

    private void OnSettingsPopupShown(SettingsPopup settingsPopup)
    {
        if (settingsPopup == null)
        {
            return;
        }

        if (_settingsPopup == settingsPopup)
        {
            ApplySettingsToPopup(_settingsPopup);
            return;
        }

        UnsubscribeFromSettingsPopup();

        _settingsPopup = settingsPopup;
        _settingsPopup.MusicEnabledChanged += OnMusicEnabledChanged;
        _settingsPopup.SoundEnabledChanged += OnSoundEnabledChanged;
        _settingsPopup.MusicVolumeChanged += OnMusicVolumeChanged;
        _settingsPopup.SoundVolumeChanged += OnSoundVolumeChanged;
        _settingsPopup.CloseClicked += OnSettingsCloseClicked;

        ApplySettingsToPopup(_settingsPopup);
    }

    private void OnSettingsPopupHidden()
    {
        UnsubscribeFromSettingsPopup();
    }

    private void OnPauseSettingsClicked()
    {
        _gameAudioService.PlayButtonClick();

        _settingsOpenSource = ESettingsOpenSource.PausePopup;

        _gamePopupService.HidePausePopup();
        _gamePopupService.ShowSettingsPopup();
    }

    private void OnDefeatSettingsClicked()
    {
        _gameAudioService.PlayButtonClick();

        _settingsOpenSource = ESettingsOpenSource.DefeatPopup;
        _gamePopupService.ShowSettingsPopup();
    }

    private void OnMusicEnabledChanged(bool isEnabled)
    {
        _audioSettingsService.SetMusicEnabled(isEnabled);
    }

    private void OnSoundEnabledChanged(bool isEnabled)
    {
        _audioSettingsService.SetSoundEnabled(isEnabled);
    }

    private void OnMusicVolumeChanged(float volume)
    {
        _audioSettingsService.SetMusicVolume(volume);
    }

    private void OnSoundVolumeChanged(float volume)
    {
        _audioSettingsService.SetSoundVolume(volume);
    }

    private void OnSettingsCloseClicked()
    {
        _gameAudioService.PlayButtonClick();

        ESettingsOpenSource openSource = _settingsOpenSource;

        _gamePopupService.HideSettingsPopup();
        _settingsOpenSource = ESettingsOpenSource.None;

        if (openSource == ESettingsOpenSource.PausePopup)
        {
            _gamePopupService.ShowPausePopup();
        }
    }

    private void ApplySettingsToPopup(SettingsPopup settingsPopup)
    {
        settingsPopup.SetMusicEnabled(_audioSettingsService.IsMusicEnabled);
        settingsPopup.SetSoundEnabled(_audioSettingsService.IsSoundEnabled);
        settingsPopup.SetMusicVolume(_audioSettingsService.MusicVolume);
        settingsPopup.SetSoundVolume(_audioSettingsService.SoundVolume);
    }

    private void UnsubscribeFromPausePopup()
    {
        if (_pausePopup == null)
        {
            return;
        }

        _pausePopup.SettingsClicked -= OnPauseSettingsClicked;
        _pausePopup = null;
    }

    private void UnsubscribeFromDefeatPopup()
    {
        if (_defeatPopup == null)
        {
            return;
        }

        _defeatPopup.SettingsClicked -= OnDefeatSettingsClicked;
        _defeatPopup = null;
    }

    private void UnsubscribeFromSettingsPopup()
    {
        if (_settingsPopup == null)
        {
            return;
        }

        _settingsPopup.MusicEnabledChanged -= OnMusicEnabledChanged;
        _settingsPopup.SoundEnabledChanged -= OnSoundEnabledChanged;
        _settingsPopup.MusicVolumeChanged -= OnMusicVolumeChanged;
        _settingsPopup.SoundVolumeChanged -= OnSoundVolumeChanged;
        _settingsPopup.CloseClicked -= OnSettingsCloseClicked;
        _settingsPopup = null;
    }
}