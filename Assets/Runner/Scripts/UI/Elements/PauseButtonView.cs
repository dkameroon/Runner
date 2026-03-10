using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseButtonView : MonoBehaviour
{
    [SerializeField] private Button _button;

    public event Action Clicked;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClicked);
    }

    public void SetVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    private void OnClicked()
    {
        Clicked?.Invoke();
    }
}