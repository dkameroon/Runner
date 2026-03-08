using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;

public class DefeatPopup : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _watchAdButton;
    [SerializeField] private TextMeshProUGUI _scoreValueText;

    private PlayerRespawnSystem _playerRespawnSystem;
    private GameFlowSystem _gameFlowSystem;

    [Inject]
    public void Construct(
        PlayerRespawnSystem playerRespawnSystem,
        GameFlowSystem gameFlowSystem)
    {
        _playerRespawnSystem = playerRespawnSystem;
        _gameFlowSystem = gameFlowSystem;
    }

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(OnRestartClicked);
        _exitButton.onClick.AddListener(OnExitClicked);
        _watchAdButton.onClick.AddListener(OnWatchAdClicked);
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(OnRestartClicked);
        _exitButton.onClick.RemoveListener(OnExitClicked);
        _watchAdButton.onClick.RemoveListener(OnWatchAdClicked);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetScore(int score)
    {
        _scoreValueText.text = $"SCORE: {score}";
    }
    
    private void OnRestartClicked()
    {
        _playerRespawnSystem.Respawn();
        _gameFlowSystem.StartGame();
    }

    private void OnExitClicked()
    {
        _playerRespawnSystem.Respawn();
        _gameFlowSystem.EnterMainMenu();
    }

    private void OnWatchAdClicked()
    {
        Debug.Log("DefeatPopup: Rewarded Ads is not integrated yet.");
    }
}