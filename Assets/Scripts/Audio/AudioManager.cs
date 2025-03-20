using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource backgroundMusicSource;
    public AudioSource sfxSource;

    [Header("UI Elements")]
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object persistent
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Load saved volume settings (if any)
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        backgroundMusicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;

        // Assign sliders (if present)
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = musicVolume;
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = sfxVolume;
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    public void SetMusicVolume(float volume)
    {
        backgroundMusicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}
