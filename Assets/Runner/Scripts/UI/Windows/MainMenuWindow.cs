using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class MainMenuWindow : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Button _leaderboardButton;
    [SerializeField] private Button _logoutButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;

    private GameFlowSystem _gameFlowSystem;

    public event Action LogoutClicked;
    public event Action LeaderboardClicked;
    public event Action SettingsClicked;

    [Inject]
    public void Construct(GameFlowSystem gameFlowSystem)
    {
        _gameFlowSystem = gameFlowSystem;
    }

    private void OnEnable()
    {
        _leaderboardButton.onClick.AddListener(OnLeaderboardClicked);
        _logoutButton.onClick.AddListener(OnLogoutClicked);
        _settingsButton.onClick.AddListener(OnSettingsClicked);
        _exitButton.onClick.AddListener(OnExitClicked);
    }

    private void OnDisable()
    {
        _leaderboardButton.onClick.RemoveListener(OnLeaderboardClicked);
        _logoutButton.onClick.RemoveListener(OnLogoutClicked);
        _settingsButton.onClick.RemoveListener(OnSettingsClicked);
        _exitButton.onClick.RemoveListener(OnExitClicked);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_gameFlowSystem.CurrentState != EGameLoopState.MainMenu)
        {
            return;
        }

        GameObject clickedObject = eventData.pointerPressRaycast.gameObject;

        if (clickedObject == null)
        {
            _gameFlowSystem.StartGame();
            return;
        }

        if (clickedObject.GetComponentInParent<Button>() != null)
        {
            return;
        }

        _gameFlowSystem.StartGame();
    }

    private void OnLeaderboardClicked()
    {
        LeaderboardClicked?.Invoke();
    }

    private void OnLogoutClicked()
    {
        LogoutClicked?.Invoke();
    }

    private void OnSettingsClicked()
    {
        SettingsClicked?.Invoke();
    }

    private void OnExitClicked()
    {
        Application.Quit();
    }
}