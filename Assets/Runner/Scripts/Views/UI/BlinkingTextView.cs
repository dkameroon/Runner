using TMPro;
using UnityEngine;

public class BlinkingTextView : MonoBehaviour
{
    [SerializeField] private float _blinkSpeed = 2f;

    private TextMeshProUGUI _text;
    private Color _baseColor;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _baseColor = _text.color;
    }

    private void Update()
    {
        float alpha = (Mathf.Sin(Time.time * _blinkSpeed) + 1f) * 0.5f;

        Color color = _baseColor;
        color.a = Mathf.Lerp(0.2f, 1f, alpha);

        _text.color = color;
    }
}