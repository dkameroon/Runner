using UnityEngine;

public class AudioPlayerView : MonoBehaviour
{
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _soundAudioSource;

    public void PlayMusicLoop(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }

        if (_musicAudioSource.clip == clip && _musicAudioSource.isPlaying)
        {
            return;
        }

        _musicAudioSource.clip = clip;
        _musicAudioSource.loop = true;
        _musicAudioSource.Play();
    }

    public void StopMusic()
    {
        if (_musicAudioSource.isPlaying == false)
        {
            return;
        }

        _musicAudioSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        _musicAudioSource.volume = Mathf.Clamp01(volume);
    }

    public void SetSoundVolume(float volume)
    {
        _soundAudioSource.volume = Mathf.Clamp01(volume);
    }

    public void PlaySound(AudioClip clip, float volumeScale = 1f)
    {
        if (clip == null)
        {
            return;
        }

        _soundAudioSource.PlayOneShot(clip, Mathf.Clamp01(volumeScale));
    }
}