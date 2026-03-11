using UnityEngine;
using Zenject;

public class PopupFactory
{
    private readonly DiContainer _diContainer;
    private readonly DefeatPopup _defeatPopupPrefab;
    private readonly PausePopup _pausePopupPrefab;
    private readonly SettingsPopup _settingsPopupPrefab;
    private readonly PopupCanvasRootView _popupCanvasRootView;

    public PopupFactory(
        DiContainer diContainer,
        DefeatPopup defeatPopupPrefab,
        PausePopup pausePopupPrefab,
        SettingsPopup settingsPopupPrefab,
        PopupCanvasRootView popupCanvasRootView)
    {
        _diContainer = diContainer;
        _defeatPopupPrefab = defeatPopupPrefab;
        _pausePopupPrefab = pausePopupPrefab;
        _settingsPopupPrefab = settingsPopupPrefab;
        _popupCanvasRootView = popupCanvasRootView;
    }

    public DefeatPopup CreateDefeatPopup()
    {
        RectTransform popupRoot = _popupCanvasRootView.Root;
        DefeatPopup defeatPopup = _diContainer.InstantiatePrefabForComponent<DefeatPopup>(
            _defeatPopupPrefab,
            popupRoot);

        return defeatPopup;
    }

    public PausePopup CreatePausePopup()
    {
        RectTransform popupRoot = _popupCanvasRootView.Root;
        PausePopup pausePopup = _diContainer.InstantiatePrefabForComponent<PausePopup>(
            _pausePopupPrefab,
            popupRoot);

        return pausePopup;
    }

    public SettingsPopup CreateSettingsPopup()
    {
        RectTransform popupRoot = _popupCanvasRootView.Root;
        SettingsPopup settingsPopup = _diContainer.InstantiatePrefabForComponent<SettingsPopup>(
            _settingsPopupPrefab,
            popupRoot);

        return settingsPopup;
    }
}