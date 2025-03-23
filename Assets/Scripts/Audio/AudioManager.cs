using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource backgroundMusicSource;
    public AudioSource sfxSource; // General SFX
    public AudioSource player1AudioSource;
    public AudioSource player2AudioSource;

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

        // Set initial volumes
        backgroundMusicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
        player1AudioSource.volume = sfxVolume;
        player2AudioSource.volume = sfxVolume;

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
        player1AudioSource.volume = volume;
        player2AudioSource.volume = volume;
    
        Debug.Log("SFX Volume set to: " + volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}
