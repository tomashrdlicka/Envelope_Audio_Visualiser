using UnityEngine;

public class BackgroundSphereController : MonoBehaviour
{
    public Material backgroundMaterial;

    // Properties for controlling effects
    public Color baseColor = new Color(0, 0.3f, 0.8f);
    public Color beatColor = new Color(0, 0.8f, 1);
    public float maxDistortionStrength = 0.2f;
    public float minDistortionStrength = 0.05f;
    public float maxEmissionIntensity = 1.0f;
    public float pulseSpeed = 0.25f;
    public float beatFlashDuration = 0.05f;

    private Color currentEmissionColor;
    private float targetDistortionStrength;
    private float lastColorChangeTime;        // Tracks the last time the color was changed
    private float barDuration;                // Duration of one bar

    public float bpm = 175f;                  // BPM of the song

    void Start()
    {
        // Calculate the duration of one bar (4 beats)
        float beatDuration = 60f / bpm;
        barDuration = 4 * beatDuration;

        // Initialize properties
        currentEmissionColor = baseColor;
        targetDistortionStrength = minDistortionStrength;

        // Set initial shader properties
        backgroundMaterial.SetColor("_BaseColor", baseColor);
        backgroundMaterial.SetColor("_EmissionColor", currentEmissionColor);
        backgroundMaterial.SetFloat("_DistortionStrength", targetDistortionStrength);
        backgroundMaterial.SetFloat("_PulseSpeed", pulseSpeed);

        lastColorChangeTime = Time.time;
    }

    // Called by AudioAnalyzer for envelope value updates
    public void UpdateWithEnvelope(float envelopeValue)
    {
        // Adjust distortion based on envelope (smooth audio intensity)
        targetDistortionStrength = Mathf.Lerp(minDistortionStrength, maxDistortionStrength, envelopeValue);

        // Adjust emission color intensity
        float emissionIntensity = Mathf.Lerp(1.0f, maxEmissionIntensity, envelopeValue);
        currentEmissionColor = baseColor * emissionIntensity;

        // Update material properties gradually
        backgroundMaterial.SetFloat("_DistortionStrength", targetDistortionStrength);

        // Trigger color change every bar
        if (Time.time >= lastColorChangeTime + barDuration)
        {
            OnBeat(); // Trigger color change
            lastColorChangeTime = Time.time; // Reset the color change timer
        }

        // Gradually transition back to the base color after the beat flash
        if (Time.time > lastColorChangeTime + beatFlashDuration)
        {
            backgroundMaterial.SetColor("_EmissionColor", Color.Lerp(backgroundMaterial.GetColor("_EmissionColor"), currentEmissionColor, Time.deltaTime * 3f));
        }
    }

    // Called to change the color on a beat (or bar in this case)
    public void OnBeat()
    {
        // Blend beat color subtly with the current emission color for a softer effect
        backgroundMaterial.SetColor("_EmissionColor", Color.Lerp(currentEmissionColor, beatColor, 0.5f));
    }

    public void ResetColor()
    {
        // Gradually reset to the base emission color with a smoother transition
        backgroundMaterial.SetColor("_EmissionColor", Color.Lerp(backgroundMaterial.GetColor("_EmissionColor"), currentEmissionColor, Time.deltaTime * 1.5f));
    }
}



