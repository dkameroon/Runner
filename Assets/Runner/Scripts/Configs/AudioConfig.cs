using UnityEngine;

[CreateAssetMenu(fileName = "Config_Audio", menuName = "Configs/Audio Config")]
public class AudioConfig : ScriptableObject
{
    [Header("Clips")]
    [SerializeField] private AudioClip _backgroundMusic;
    [SerializeField] private AudioClip _buttonClickSound;

    [Header("Default Volumes")]
    [SerializeField] [Range(0f, 1f)] private float _defaultMusicVolume = 0.5f;
    [SerializeField] [Range(0f, 1f)] private float _defaultSoundVolume = 0.5f;

    public AudioClip BackgroundMusic => _backgroundMusic;
    public AudioClip ButtonClickSound => _buttonClickSound;
    public float DefaultMusicVolume => _defaultMusicVolume;
    public float DefaultSoundVolume => _defaultSoundVolume;
}