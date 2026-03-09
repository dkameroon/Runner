using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardEntryElement : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TMP_Text _rankText;
    [SerializeField] private TMP_Text _userLoginText;
    [SerializeField] private TMP_Text _scoreText;

    [Header("Visuals")]
    [SerializeField] private Image _backgroundImage;

    [Header("Colors")]
    [SerializeField] private Color _firstPlaceColor = new Color(1f, 0.9f, 0.2f, 1f);
    [SerializeField] private Color _secondPlaceColor = new Color(0.85f, 0.93f, 0.97f, 1f);
    [SerializeField] private Color _thirdPlaceColor = new Color(1f, 0.65f, 0.35f, 1f);
    [SerializeField] private Color _defaultPlaceColor = new Color(0.92f, 0.92f, 0.92f, 1f);

    public void SetData(LeaderboardEntryData entryData)
    {
        _rankText.text = $"{entryData.Rank}.";
        _userLoginText.text = entryData.UserLogin;
        _scoreText.text = entryData.Score.ToString();

        ApplyStyle(entryData.Rank);
    }

    private void ApplyStyle(int rank)
    {
        switch (rank)
        {
            case 1:
                _backgroundImage.color = _firstPlaceColor;
                break;

            case 2:
                _backgroundImage.color = _secondPlaceColor;
                break;

            case 3:
                _backgroundImage.color = _thirdPlaceColor;
                break;

            default:
                _backgroundImage.color = _defaultPlaceColor;
                break;
        }
    }
}