using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Vector3 defaultScale; // Stores the original scale of the button
    private Button button; // Reference to the Button component
    private AudioSource audioSource; // Audio source for hover sounds
    private Color originalColor; // Stores the original button color
    private Image buttonImage; // Reference to the button's image

    // Audio Settings
    public AudioClip hoverSound; // Sound to play on hover

    // Color Settings
    public Color hoverColor = Color.gray; // Default hover color (gray)

    void Start()
    {
        // Store the original scale
        defaultScale = transform.localScale;

        // Get the Button component
        button = GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError($"No Button component found on {gameObject.name}! Ensure this script is attached to a UI Button.");
        }

        // Get the button image
        buttonImage = GetComponent<Image>();

        if (buttonImage != null)
        {
            originalColor = buttonImage.color; // Store original button color
        }
        else
        {
            Debug.LogWarning($"No Image component found on {gameObject.name}. Color change effect will not work.");
        }

        // Check or add an AudioSource for sound effects
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning($"No AudioSource found on {gameObject.name}. Adding one automatically.");
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

   
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = defaultScale * 1.1f; // Slight zoom-in effect
        
        if (buttonImage != null)
        {
            buttonImage.color = hoverColor; // Change button color to gray
        }

        // Play hover sound if assigned
        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = defaultScale; // Reset scale

        if (buttonImage != null)
        {
            buttonImage.color = originalColor; // Reset to original color
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        transform.localScale = defaultScale * 0.95f; // Quick shrink effect
        Invoke(nameof(ResetScale), 0.1f); // Restore scale after a short delay
    }
    
    void ResetScale()
    {
        transform.localScale = defaultScale;
    }
}