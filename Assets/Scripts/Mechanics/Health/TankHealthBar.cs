using UnityEngine;
using UnityEngine.UI;

public class TankHealthBar : MonoBehaviour
{
    public float verticalOffset = 2.5f;
    private Slider slider;
    private Transform tankTransform;
    private Camera cam;
    private Canvas canvas;

    private float currentDisplayHealth;
    private float smoothSpeed = 10f;

    void Start()
    {
        tankTransform = transform;
        canvas = GetComponentInChildren<Canvas>();
        slider = GetComponentInChildren<Slider>();

        if (canvas == null || slider == null)
        {
            return;
        }

        canvas.renderMode = RenderMode.WorldSpace;
        currentDisplayHealth = slider.maxValue;
    }

    void Update()
    {
        if (slider == null || cam == null || tankTransform == null)
            return;

        // Smooth health bar animation
        slider.value = Mathf.Lerp(slider.value, currentDisplayHealth, Time.deltaTime * smoothSpeed);

        // Follow tank
        Vector3 worldPosition = tankTransform.position + Vector3.up * verticalOffset;
        Vector3 screenPosition = cam.WorldToScreenPoint(worldPosition);

        bool isBehind = screenPosition.z < 0;
        canvas.gameObject.SetActive(!isBehind);
        if (!isBehind)
        {
            canvas.transform.position = cam.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, screenPosition.z));
            canvas.transform.rotation = cam.transform.rotation;
        }
    }

    public void SetCamera(Camera camera)
    {
        cam = camera;
        if (canvas != null)
        {
            canvas.worldCamera = cam;
        }
    }

    public void SetHealth(float health)
    {
        currentDisplayHealth = health;
    }

    public void SetMaxHealth(float maxHealth)
    {
        if (slider != null)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
            currentDisplayHealth = maxHealth;
        }
    }
}