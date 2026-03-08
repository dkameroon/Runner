using TMPro;
using UnityEngine;

public class GameHUDView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

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
        _scoreText.text = score.ToString();
    }
}