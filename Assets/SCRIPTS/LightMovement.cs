using UnityEngine;
using System.Collections;

public class LightMovement : MonoBehaviour
{
    public float rotationSpeed = 60f; // Degrees per second
    public float radius = 5f;         // Orbit radius
    public Vector3 orbitCenter;       // Center point of orbit
    public float bpm = 175f;          // Beats per minute of the song

    private Vector3 rotationAxis;     // Axis of rotation
    private float currentAngle = 0f;  // Current angle of rotation


    void Start()
    {
        // Initialize rotation axis and position
        currentAngle = Random.Range(0f, 360f); // Randomize start position
        ChangePath(); // Set initial path
        UpdatePosition();
    }

    void Update()
    {
        // Rotate around the orbit center
        currentAngle += rotationSpeed * Time.deltaTime;

        // Keep angle within 0-360 degrees
        if (currentAngle >= 360f)
        {
            currentAngle -= 360f;
            ChangePath(); // Change path after a full rotation
        }

        UpdatePosition();
    }

    void UpdatePosition()
    {
        // Calculate new position
        float radianAngle = currentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(
            Mathf.Cos(radianAngle),
            Mathf.Sin(radianAngle),
            0f
        );

        // Apply rotation axis
        offset = Quaternion.FromToRotation(Vector3.forward, rotationAxis) * offset;

        // Set position
        transform.position = orbitCenter + offset * radius;
    }

    IEnumerator SmoothChangePath(Vector3 newAxis, float duration)
    {
        Vector3 initialAxis = rotationAxis;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            rotationAxis = Vector3.Slerp(initialAxis, newAxis, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        rotationAxis = newAxis;
    }

    public void ChangePath()
    {
        // Generate a new random rotation axis
        Vector3 newAxis = Random.onUnitSphere;
        StartCoroutine(SmoothChangePath(newAxis, 2f)); // Smoothly transition over 2 seconds

        // Generate a new random radius within the range
        radius = Random.Range(radius * 0.8f, radius * 1.2f);

        // Optionally, reset angle to create a smooth transition
        currentAngle = 0f;
    }
}
