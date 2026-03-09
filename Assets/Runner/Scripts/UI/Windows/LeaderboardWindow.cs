using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardWindow : BaseWindow
{
    [Header("Content")]
    [SerializeField] private RectTransform _contentRoot;

    [Header("Player Best Score")]
    [SerializeField] private TMP_Text _playerNicknameText;
    [SerializeField] private TMP_Text _playerBestScoreText;

    [Header("Buttons")]
    [SerializeField] private Button _returnToMenuButton;

    public event Action ReturnToMenuClicked;

    protected override void Awake()
    {
        base.Awake();
        _returnToMenuButton.onClick.AddListener(OnReturnToMenuButtonClicked);
    }

    private void OnDestroy()
    {
        _returnToMenuButton.onClick.RemoveListener(OnReturnToMenuButtonClicked);
    }

    public void ClearEntries()
    {
        for (int i = _contentRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(_contentRoot.GetChild(i).gameObject);
        }
    }

    public void AddEntry(LeaderboardEntryElement entryElement)
    {
        entryElement.transform.SetParent(_contentRoot, false);
    }

    public void SetPlayerBestScore(string playerNickname, int score)
    {
        _playerNicknameText.text = playerNickname;
        _playerBestScoreText.text = score.ToString();
    }

    private void OnReturnToMenuButtonClicked()
    {
        ReturnToMenuClicked?.Invoke();
    }
}