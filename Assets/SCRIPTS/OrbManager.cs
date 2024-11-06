using UnityEngine;
using System.Collections.Generic;

public class OrbManager : MonoBehaviour
{
    public GameObject orbPrefab;
    public int baseNumberOfOrbs = 40;
    public float baseRadius = 5f;
    public float rotationSpeed = 65.6f; // Base rotation speed, can be adjusted
    public float baseScale = 1f;        // Base scale for the first set of orbs
    public float bpm = 175f;            // BPM for syncing rotation speed

    private List<GameObject> orbs = new List<GameObject>(); // List to store references to the orbs
    private Dictionary<GameObject, Vector3> initialPositions = new Dictionary<GameObject, Vector3>(); // Stores initial positions
    private Dictionary<GameObject, Vector3> rotationDirections = new Dictionary<GameObject, Vector3>(); // Stores unique rotation directions
    private Dictionary<GameObject, float> orbitRadii = new Dictionary<GameObject, float>(); // Stores unique orbit radii

    void Start()
    {
        // Place each layer with increasing orb counts and distance
        PlaceOrbs(baseRadius, baseNumberOfOrbs, baseScale, 0.1f);        // First layer with minimal offset
        PlaceOrbs(baseRadius * 2f, baseNumberOfOrbs * 2, baseScale, 0.3f); // Second layer with moderate offset
        PlaceOrbs(baseRadius * 4f, baseNumberOfOrbs * 4, baseScale, 0.5f); // Third layer with more pronounced offset

        // Assign random rotation directions and orbit radii to each orb
        foreach (GameObject orb in orbs)
        {
            Vector3 randomDirection = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            ).normalized; // Normalize to ensure uniform rotation speed

            float randomRadius = Random.Range(0.1f, 0.5f); // Random orbit radius for each orb

            rotationDirections[orb] = randomDirection;
            initialPositions[orb] = orb.transform.position;
            orbitRadii[orb] = randomRadius;
        }
    }

    void Update()
    {
        // Calculate the rotation speed based on BPM
        float rotationSpeedBPM = (bpm / 60f) * 360f; // Degrees per second

        // Rotate each orb around its own axis and apply a small orbit around its initial position
        foreach (GameObject orb in orbs)
        {
            if (orb != null) // Check if the orb still exists
            {
                // Rotate the orb on its axis
                orb.transform.Rotate(rotationDirections[orb] * rotationSpeed / 4 * Time.deltaTime);

                // Orbit around its initial position
                Vector3 initialPosition = initialPositions[orb];
                Vector3 orbitDirection = rotationDirections[orb];

                // Calculate the orbit position based on time
                float angle = rotationSpeedBPM * Time.time * 0.001f; // Scale angle to make it subtle
                Vector3 offset = orbitDirection * Mathf.Sin(angle) * orbitRadii[orb];
                orb.transform.position = initialPosition + offset;
            }
        }
    }

    void PlaceOrbs(float radius, int numberOfOrbs, float scaleMultiplier, float offsetVariation)
    {
        for (int i = 0; i < numberOfOrbs; i++)
        {
            // Calculate spherical coordinates using the Fibonacci sphere algorithm
            float theta = 2 * Mathf.PI * i / ((1 + Mathf.Sqrt(5)) / 2); // Golden angle in radians
            float phi = Mathf.Acos(1 - 2 * (i + 0.5f) / numberOfOrbs); // Latitude

            // Convert spherical coordinates to Cartesian
            float x = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(phi) * Mathf.Sin(theta);
            float z = radius * Mathf.Cos(phi);

            // Apply a random offset variation to add depth to the layer
            x += Random.Range(-offsetVariation, offsetVariation) * radius * 0.05f;
            y += Random.Range(-offsetVariation, offsetVariation) * radius * 0.05f;
            z += Random.Range(-offsetVariation, offsetVariation) * radius * 0.05f;

            // Position for each orb
            Vector3 position = new Vector3(x, y, z);

            // Instantiate and orient the orb
            GameObject orb = Instantiate(orbPrefab, position, Quaternion.identity, transform);
            orb.transform.localScale *= scaleMultiplier; // Scale the orb size
            orb.transform.LookAt(Vector3.zero);

            // Add the orb to the list for rotation management
            orbs.Add(orb);
        }
    }
}



