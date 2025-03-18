using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
