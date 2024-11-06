using UnityEngine;
using System.Collections.Generic;

public class AudioAnalyzer : MonoBehaviour
{
    public AudioSource audioSource;
    public float[] samples = new float[1024];
    public float envelopeValue;
    public float envelopeMultiplier = 1.4f;

    public float beatSensitivity = 1.2f;
    private float previousEnvelopeValue;

    // References to all orbs
    public List<OrbMorpher> orbMorphers = new List<OrbMorpher>();
    public List<OrbLightController> orbLightControllers = new List<OrbLightController>();

    // Reference to BackgroundSphereController for the background sphere
    public BackgroundSphereController backgroundSphereController;

    // Movement parameters
    public float movementIntensity = 0.5f; // Controls how much the orbs move towards/away from the center
    public Transform sceneCenter; // The center point of the scene (optional, defaults to Vector3.zero if not set)

    void Start()
    {
        // Find all orbs in the scene and populate the lists
        OrbMorpher[] morphers = FindObjectsOfType<OrbMorpher>();
        orbMorphers.AddRange(morphers);

        OrbLightController[] lights = FindObjectsOfType<OrbLightController>();
        orbLightControllers.AddRange(lights);
    }

    void Update()
    {
        // Get the audio data
        audioSource.GetOutputData(samples, 0);

        // Calculate envelope (overall intensity of sound)
        envelopeValue = CalculateRMS(samples);

        // Apply envelope to orbs and background
        ApplyEnvelopeToOrbs();
        ApplyEnvelopeToBackground();

        // Detect beats and trigger beat-based effects for both orbs and background
        DetectBeat();
    }

    private float CalculateRMS(float[] audioSamples)
    {
        float sum = 0;
        foreach (float sample in audioSamples)
        {
            sum += sample * sample;
        }
        return Mathf.Sqrt(sum / audioSamples.Length);
    }

    private void ApplyEnvelopeToOrbs()
    {
        // Scale the envelope value by a multiplier to control morphing and lighting intensity
        float intensity = envelopeValue * envelopeMultiplier;

        // Calculate movement offset based on envelope intensity
        float movementOffset = (intensity - 0.5f) * movementIntensity;

        // Apply morphing, lighting effects, and movement to each orb
        for (int i = 0; i < orbMorphers.Count; i++)
        {
            orbMorphers[i].Morph(intensity); // Update morphing based on audio envelope
            orbLightControllers[i].UpdateLight(intensity); // Update lighting intensity

            // Move the orb towards or away from the scene center
            Vector3 directionToCenter = (sceneCenter != null ? sceneCenter.position : Vector3.zero) - orbMorphers[i].transform.position;
            orbMorphers[i].transform.position += directionToCenter.normalized * movementOffset * Time.deltaTime;
        }
    }

    private void ApplyEnvelopeToBackground()
    {
        // Update background sphere properties based on the envelope value
        if (backgroundSphereController != null)
        {
            backgroundSphereController.UpdateWithEnvelope(envelopeValue * envelopeMultiplier);
        }
    }

    private void DetectBeat()
    {
        // Detect beat by comparing current envelope value with previous envelope value
        if (envelopeValue > previousEnvelopeValue * beatSensitivity)
        {
            // Trigger beat effects on each orb
            for (int i = 0; i < orbMorphers.Count; i++)
            {
                orbMorphers[i].ApplyBeatEffect();
                orbLightControllers[i].OnBeat();
            }

            // Trigger beat effect on the background sphere
            if (backgroundSphereController != null)
            {
                backgroundSphereController.OnBeat();
            }
        }
        else
        {
            // Reset color and shape to base settings for each orb
            for (int i = 0; i < orbMorphers.Count; i++)
            {
                orbMorphers[i].ResetMorph();
                orbLightControllers[i].ResetColor();
            }

            // Reset the background sphere color
            if (backgroundSphereController != null)
            {
                backgroundSphereController.ResetColor();
            }
        }
        // Store the current envelope value for the next frame’s beat detection comparison
        previousEnvelopeValue = envelopeValue;
    }
}




