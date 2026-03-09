using UnityEngine;
using Zenject;

public class PopupFactory
{
    private readonly DiContainer _diContainer;
    private readonly DefeatPopup _defeatPopupPrefab;
    private readonly PopupCanvasRootView _popupCanvasRootView;

    public PopupFactory(
        DiContainer diContainer,
        DefeatPopup defeatPopupPrefab,
        PopupCanvasRootView popupCanvasRootView)
    {
        _diContainer = diContainer;
        _defeatPopupPrefab = defeatPopupPrefab;
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
}