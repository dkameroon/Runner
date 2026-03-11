using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class UIButtonSoundView : MonoBehaviour
{
    private Button _button;
    private GameAudioService _gameAudioService;

    [Inject]
    public void Construct(GameAudioService gameAudioService)
    {
        _gameAudioService = gameAudioService;
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        _gameAudioService.PlayButtonClick();
    }
}