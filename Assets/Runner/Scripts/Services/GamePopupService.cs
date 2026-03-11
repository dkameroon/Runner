using System;

public class GamePopupService
{
    private readonly PopupFactory _popupFactory;

    private DefeatPopup _defeatPopupInstance;
    private PausePopup _pausePopupInstance;
    private SettingsPopup _settingsPopupInstance;

    private bool _isDefeatPopupVisible;
    private bool _isPausePopupVisible;
    private bool _isSettingsPopupVisible;

    public DefeatPopup CurrentDefeatPopup => _defeatPopupInstance;
    public PausePopup CurrentPausePopup => _pausePopupInstance;
    public SettingsPopup CurrentSettingsPopup => _settingsPopupInstance;

    public event Action<DefeatPopup> DefeatPopupShown;
    public event Action DefeatPopupHidden;

    public event Action<PausePopup> PausePopupShown;
    public event Action PausePopupHidden;

    public event Action<SettingsPopup> SettingsPopupShown;
    public event Action SettingsPopupHidden;

    public GamePopupService(PopupFactory popupFactory)
    {
        _popupFactory = popupFactory;
        _isDefeatPopupVisible = false;
        _isPausePopupVisible = false;
        _isSettingsPopupVisible = false;
    }

    public DefeatPopup ShowDefeatPopup()
    {
        DefeatPopup defeatPopup = GetOrCreateDefeatPopup();

        if (_isDefeatPopupVisible)
        {
            return defeatPopup;
        }

        defeatPopup.Show();
        _isDefeatPopupVisible = true;

        DefeatPopupShown?.Invoke(defeatPopup);

        return defeatPopup;
    }

    public void HideDefeatPopup()
    {
        if (_defeatPopupInstance == null)
        {
            return;
        }

        if (_isDefeatPopupVisible == false)
        {
            return;
        }

        _defeatPopupInstance.Hide();
        _isDefeatPopupVisible = false;

        DefeatPopupHidden?.Invoke();
    }

    public PausePopup ShowPausePopup()
    {
        PausePopup pausePopup = GetOrCreatePausePopup();

        if (_isPausePopupVisible)
        {
            return pausePopup;
        }

        pausePopup.Show();
        _isPausePopupVisible = true;

        PausePopupShown?.Invoke(pausePopup);

        return pausePopup;
    }

    public void HidePausePopup()
    {
        if (_pausePopupInstance == null)
        {
            return;
        }

        if (_isPausePopupVisible == false)
        {
            return;
        }

        _pausePopupInstance.Hide();
        _isPausePopupVisible = false;

        PausePopupHidden?.Invoke();
    }

    public SettingsPopup ShowSettingsPopup()
    {
        SettingsPopup settingsPopup = GetOrCreateSettingsPopup();

        if (_isSettingsPopupVisible)
        {
            return settingsPopup;
        }

        settingsPopup.Show();
        _isSettingsPopupVisible = true;

        SettingsPopupShown?.Invoke(settingsPopup);

        return settingsPopup;
    }

    public void HideSettingsPopup()
    {
        if (_settingsPopupInstance == null)
        {
            return;
        }

        if (_isSettingsPopupVisible == false)
        {
            return;
        }

        _settingsPopupInstance.Hide();
        _isSettingsPopupVisible = false;

        SettingsPopupHidden?.Invoke();
    }

    private DefeatPopup GetOrCreateDefeatPopup()
    {
        if (_defeatPopupInstance != null)
        {
            return _defeatPopupInstance;
        }

        _defeatPopupInstance = _popupFactory.CreateDefeatPopup();
        _defeatPopupInstance.Hide();

        return _defeatPopupInstance;
    }

    private PausePopup GetOrCreatePausePopup()
    {
        if (_pausePopupInstance != null)
        {
            return _pausePopupInstance;
        }

        _pausePopupInstance = _popupFactory.CreatePausePopup();
        _pausePopupInstance.Hide();

        return _pausePopupInstance;
    }

    private SettingsPopup GetOrCreateSettingsPopup()
    {
        if (_settingsPopupInstance != null)
        {
            return _settingsPopupInstance;
        }

        _settingsPopupInstance = _popupFactory.CreateSettingsPopup();
        _settingsPopupInstance.Hide();

        return _settingsPopupInstance;
    }
}