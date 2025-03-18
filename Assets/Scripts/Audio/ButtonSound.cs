using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public AudioSource buttonAudioSource;

    public void PlayButtonSound()
    {
        buttonAudioSource.PlayOneShot(buttonAudioSource.clip);
    }
}
