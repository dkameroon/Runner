using System;

public class GamePopupService
{
    private readonly PopupFactory _popupFactory;

    private DefeatPopup _defeatPopupInstance;
    private PausePopup _pausePopupInstance;

    private bool _isDefeatPopupVisible;
    private bool _isPausePopupVisible;

    public DefeatPopup CurrentDefeatPopup => _defeatPopupInstance;
    public PausePopup CurrentPausePopup => _pausePopupInstance;

    public event Action<DefeatPopup> DefeatPopupShown;
    public event Action DefeatPopupHidden;

    public event Action<PausePopup> PausePopupShown;
    public event Action PausePopupHidden;

    public GamePopupService(PopupFactory popupFactory)
    {
        _popupFactory = popupFactory;
        _isDefeatPopupVisible = false;
        _isPausePopupVisible = false;
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
}