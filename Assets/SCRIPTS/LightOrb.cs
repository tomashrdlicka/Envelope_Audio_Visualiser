using UnityEngine;

public class OrbLightController : MonoBehaviour
{
    private Light orbLight;

    public float baseIntensity = 1f;
    public float maxIntensity = 3f;
    public float intensityLerpSpeed = 5f;

    public float baseRange = 5f;
    public float maxRange = 10f;
    public float rangeLerpSpeed = 3f;

    public Color baseColor = Color.white;
    public Color beatColor = Color.red;
    public float colorLerpSpeed = 10f;

    private Color targetColor;
    private float targetIntensity;
    private float targetRange;

    void Start()
    {
        orbLight = GetComponent<Light>();

        orbLight.color = baseColor;
        orbLight.intensity = baseIntensity;
        orbLight.range = baseRange;

        targetColor = baseColor;
        targetIntensity = baseIntensity;
        targetRange = baseRange;
    }

    void Update()
    {
        orbLight.color = Color.Lerp(orbLight.color, targetColor, Time.deltaTime * colorLerpSpeed);
        orbLight.intensity = Mathf.Lerp(orbLight.intensity, targetIntensity, Time.deltaTime * intensityLerpSpeed);
        orbLight.range = Mathf.Lerp(orbLight.range, targetRange, Time.deltaTime * rangeLerpSpeed);
    }

    public void UpdateLight(float envelopeValue)
    {
        // Apply exponential scaling to the envelope value to enhance the effect of small values
        float boostedEnvelope = Mathf.Pow(envelopeValue, 0.5f); // Square root scaling to boost low values

        // Ensure boostedEnvelope has a minimum value to avoid being too subtle
        float minEnvelopeEffect = 0.1f; // Adjust as needed
        boostedEnvelope = Mathf.Max(boostedEnvelope, minEnvelopeEffect);

        // Lerp between base and max using the boosted envelope value
        targetIntensity = Mathf.Lerp(baseIntensity, maxIntensity, boostedEnvelope);
        targetRange = Mathf.Lerp(baseRange, maxRange, boostedEnvelope);
    }

    public void OnBeat()
    {
        targetColor = beatColor;
    }

    public void ResetColor()
    {
        targetColor = baseColor;
    }
}


