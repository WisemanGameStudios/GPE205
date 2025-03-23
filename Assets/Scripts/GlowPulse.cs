using UnityEngine;

public class GlowPulse : MonoBehaviour
{
    public Renderer targetRenderer;
    public string emissionColorProperty = "_EmissionColor";
    public Color baseColor = Color.green;
    public float pulseSpeed = 2f;
    public float minIntensity = 1f;
    public float maxIntensity = 3f;

    private Material mat;

    void Start()
    {
        if (targetRenderer == null) targetRenderer = GetComponent<Renderer>();
        mat = targetRenderer.material;
        mat.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
        mat.SetColor(emissionColorProperty, baseColor * intensity);
    }
}