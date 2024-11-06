using UnityEngine;
using System.Collections.Generic;

public class LightController : MonoBehaviour
{
    public GameObject lightPrefab;            // Prefab for the point lights
    public int numberOfLightsPerSet = 12;     // Number of lights per set
    public float innerMinRadius = 5f;         // Minimum radius for the third set
    public float innerMaxRadius = 15f;        // Maximum radius for the third set
    public float middleMinRadius = 15f;       // Minimum radius for the first set
    public float middleMaxRadius = 25f;       // Maximum radius for the first set
    public float outerMinRadius = 25f;        // Minimum radius for the second set
    public float outerMaxRadius = 35f;        // Maximum radius for the second set
    public float bpm = 175f;                  // Beats per minute of the song

    private List<LightMovement> lights = new List<LightMovement>();

    // Define the specific colors for the lights
    private Color[] lightColors;

    void Start()
    {
        // Initialize the specific colors
        lightColors = new Color[12];
        lightColors[0] = HexToColor("FAA200"); // Orange
        lightColors[1] = HexToColor("FAA200"); // Orange
        lightColors[2] = HexToColor("FA5B00"); // Reddish-Orange
        lightColors[3] = HexToColor("FA5B00"); // Reddish-Orange
        lightColors[4] = HexToColor("FAF600"); // Yellow
        lightColors[5] = HexToColor("FAF600"); // Yellow
        lightColors[6] = HexToColor("FA2F00"); // Reddish
        lightColors[7] = HexToColor("FA2F00"); // Reddish
        lightColors[8] = HexToColor("FAA200"); // Orange
        lightColors[9] = HexToColor("FA5B00"); // Reddish-Orange
        lightColors[10] = HexToColor("FAF600"); // Yellow
        lightColors[11] = HexToColor("FA2F00"); // Reddish

        // Calculate rotation speed based on BPM
        float rotationSpeed = CalculateRotationSpeed(bpm);

        // Create the third set of lights (radius 5-15, intensity 10, range 10)
        CreateLightSet(numberOfLightsPerSet, innerMinRadius, innerMaxRadius, 10f, 10f, rotationSpeed);

        // Create the first set of lights (radius 15-25, intensity 15, range 15)
        CreateLightSet(numberOfLightsPerSet, middleMinRadius, middleMaxRadius, 15f, 15f, rotationSpeed);

        // Create the second set of lights (radius 25-35, intensity 25, range 25)
        CreateLightSet(numberOfLightsPerSet, outerMinRadius, outerMaxRadius, 25f, 25f, rotationSpeed);
    }

    void CreateLightSet(int numberOfLights, float minRadius, float maxRadius, float intensity, float range, float rotationSpeed)
    {
        for (int i = 0; i < numberOfLights; i++)
        {
            // Instantiate the light prefab
            GameObject lightObj = Instantiate(lightPrefab, transform);

            // Get or add the Light component
            Light pointLight = lightObj.GetComponent<Light>();
            if (pointLight == null)
            {
                pointLight = lightObj.AddComponent<Light>();
            }
            pointLight.type = LightType.Point;

            // Assign the specific color
            pointLight.color = lightColors[i % lightColors.Length];

            // Set intensity and range
            pointLight.intensity = intensity;
            pointLight.range = range;

            // Add LightMovement component to control movement
            LightMovement lightMovement = lightObj.AddComponent<LightMovement>();
            lightMovement.rotationSpeed = rotationSpeed;
            lightMovement.radius = Random.Range(minRadius, maxRadius);
            lightMovement.orbitCenter = transform.position;
            lightMovement.ChangePath(); // Initialize a random path
            lightMovement.bpm = bpm;

            lights.Add(lightMovement);
        }
    }

    float CalculateRotationSpeed(float bpm)
    {
        // Calculate how long it takes for a beat in seconds
        float beatDuration = 60f / bpm;

        // Decide how many beats per full rotation (e.g., 4 bars in 4/4 time is 16 beats)
        int beatsPerRotation = 16; // Adjust as needed

        // Total duration for one full rotation
        float rotationDuration = beatDuration * beatsPerRotation;

        // Rotation speed in degrees per second (360 degrees over the rotation duration)
        float rotationSpeed = 360f / rotationDuration;

        return rotationSpeed;
    }

    // Helper method to convert hex strings to Color
    Color HexToColor(string hex)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString("#" + hex, out color))
        {
            return color;
        }
        else
        {
            Debug.LogWarning("Invalid color code: " + hex);
            return Color.white;
        }
    }
}


