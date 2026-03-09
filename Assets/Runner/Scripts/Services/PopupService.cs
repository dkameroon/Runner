using UnityEngine;

public class PopupService
{
    private readonly PopupFactory _popupFactory;

    private DefeatPopup _defeatPopupInstance;

    public DefeatPopup CurrentDefeatPopup => _defeatPopupInstance;

    public PopupService(PopupFactory popupFactory)
    {
        _popupFactory = popupFactory;
    }

    public DefeatPopup ShowDefeatPopup()
    {
        DefeatPopup defeatPopup = GetOrCreateDefeatPopup();
        defeatPopup.Show();

        return defeatPopup;
    }

    public void HideDefeatPopup()
    {
        if (_defeatPopupInstance == null)
        {
            return;
        }

        _defeatPopupInstance.Hide();
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
}