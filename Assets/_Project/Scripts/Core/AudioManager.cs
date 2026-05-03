using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; } // Singleton

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource; // UI
    [SerializeField] private AudioSource _weaponSfxSource; // Armi
    [SerializeField] private AudioSource _pitchSfxSource; // Ripetitivi come hit e loot

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
        {
            if (_musicSource.clip == clip) return;

            _musicSource.clip = clip;
            _musicSource.volume = volume;
            _musicSource.loop = true;
            _musicSource.Play();
        }
    }

    public void StopMusic()
    {
        _musicSource?.Stop();
    }

    public void PlayAudio(AudioClip clip, AudioChannel channel, float volume = 1f)
    {
        if (clip == null) return;

        switch (channel)
        {
            case AudioChannel.SFX:
                _sfxSource.PlayOneShot(clip, volume);
                break;

            case AudioChannel.Weapon:
                _weaponSfxSource.pitch = Random.Range(0.9f, 1.1f);
                _weaponSfxSource.PlayOneShot(clip, volume);
                break;

            case AudioChannel.PitchSFX:
                _pitchSfxSource.pitch = Random.Range(0.9f, 1.1f);
                _pitchSfxSource.PlayOneShot(clip, volume);
                break;

            case AudioChannel.Music:
                PlayMusic(clip);
                break;
        }
    }
}
