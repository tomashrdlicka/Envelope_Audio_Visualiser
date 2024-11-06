using UnityEngine;

public class OrbMorpher : MonoBehaviour
{
    private MeshFilter meshFilter;
    private Vector3[] baseVertices;
    private Vector3[] displacedVertices;

    private float morphIntensity = 1.3f;       // Base intensity for morphing
    private float noiseScale = 0.6f;           // Decreased for finer details
    private float noiseSpeed = 1.5f;           // Increased for faster noise animation
    private float beatMorphMultiplier = 1.5f;  // Reduced for a more subtle effect

    private float noiseOffset = 0f;            // Noise animation offset
    private float currentMorphIntensity;       // Dynamic intensity for morphing
    private float maxMorphIntensity = 4.0f;    // Increased to allow more morphing
    private float maxOrbScale = 2.0f;          // Maximum allowable scale of the orb
    private bool isResetting = false;          // Flag to control reset behavior

    private float decayDuration = 0.5f;        // Duration for the beat effect to decay

    void Start()
    {
        // Initialize mesh and vertex data
        meshFilter = GetComponent<MeshFilter>();
        baseVertices = meshFilter.mesh.vertices;
        displacedVertices = new Vector3[baseVertices.Length];

        // Set initial morph intensity
        currentMorphIntensity = morphIntensity;
    }

    public void Morph(float intensity)
    {
        // Apply exponential scaling to boost low intensity values
        float boostedIntensity = Mathf.Pow(intensity, 0.5f); // Adjust exponent for desired boost (0.5 gives square root)

        // Calculate the scale factor based on the object's current scale
        float scaleFactor = Mathf.Clamp01(1.0f - (transform.localScale.x / maxOrbScale));
        float effectiveMorphIntensity = currentMorphIntensity * Mathf.Lerp(1f, 0.8f, scaleFactor);

        // Ensure the effective morph intensity does not drop too low
        effectiveMorphIntensity = Mathf.Clamp(effectiveMorphIntensity, 0.3f, maxMorphIntensity);

        // Animate the noise offset for continuous, smooth morphing
        noiseOffset += Time.deltaTime * noiseSpeed;

        // Gradually bring current intensity back to base morphIntensity for smooth reset if not resetting
        if (!isResetting)
        {
            currentMorphIntensity = Mathf.Lerp(currentMorphIntensity, morphIntensity, Time.deltaTime * 1.5f);
        }

        // Loop through each vertex and apply Perlin noise-based displacement
        for (int i = 0; i < baseVertices.Length; i++)
        {
            Vector3 vertex = baseVertices[i];

            // Generate separate noise offsets for each axis
            float noiseOffsetX = noiseOffset;
            float noiseOffsetY = noiseOffset + 100f; // Arbitrary offset to differentiate axes
            float noiseOffsetZ = noiseOffset + 200f;

            // Generate noise values for each axis to create smooth offsets
            float noiseX = Mathf.PerlinNoise(vertex.x * noiseScale + noiseOffsetX, vertex.y * noiseScale + noiseOffsetX);
            float noiseY = Mathf.PerlinNoise(vertex.y * noiseScale + noiseOffsetY, vertex.z * noiseScale + noiseOffsetY);
            float noiseZ = Mathf.PerlinNoise(vertex.z * noiseScale + noiseOffsetZ, noiseOffsetZ * 0.5f);

            // Combine noise values and scale by boosted intensity to create the offset
            float combinedNoise = (noiseX + noiseY + noiseZ) / 3.0f;

            // Apply the offset with exponential scaling for more pronounced movement at lower intensities
            Vector3 offset = vertex.normalized * combinedNoise * boostedIntensity * effectiveMorphIntensity * 1.0f;

            // Apply a clamp with more leeway to allow greater movement
            float dynamicClamp = Mathf.Lerp(0.5f, 1.5f, transform.localScale.x / maxOrbScale);
            offset = Vector3.ClampMagnitude(offset, dynamicClamp);

            // Apply the offset to create a noticeable morphing effect
            displacedVertices[i] = vertex + offset;
        }

        // Update the mesh with the new vertex positions
        Mesh mesh = meshFilter.mesh;
        mesh.vertices = displacedVertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    public void ApplyBeatEffect()
    {
        // Apply an instant boost to the morph intensity for the beat effect
        currentMorphIntensity = morphIntensity * beatMorphMultiplier;

        // Start a coroutine to gradually decrease the morph intensity back to the base level
        StopAllCoroutines();
        StartCoroutine(GraduallyDecreaseMorphIntensity());
    }

    private System.Collections.IEnumerator GraduallyDecreaseMorphIntensity()
    {
        float elapsedTime = 0f;
        float initialIntensity = currentMorphIntensity;

        while (elapsedTime < decayDuration)
        {
            elapsedTime += Time.deltaTime;
            currentMorphIntensity = Mathf.Lerp(initialIntensity, morphIntensity, elapsedTime / decayDuration);
            yield return null;
        }

        // Ensure the intensity is exactly the base level at the end
        currentMorphIntensity = morphIntensity;
    }

    public void ResetMorph()
    {
        // Indicate that the morph is resetting
        isResetting = true;

        // Gradually return the displaced vertices to their base positions
        for (int i = 0; i < displacedVertices.Length; i++)
        {
            displacedVertices[i] = Vector3.Lerp(displacedVertices[i], baseVertices[i], Time.deltaTime * 2f);
        }

        // Gradually reduce the currentMorphIntensity back to base
        currentMorphIntensity = Mathf.Lerp(currentMorphIntensity, morphIntensity, Time.deltaTime * 2f);

        // Update the mesh with interpolated vertices
        Mesh mesh = meshFilter.mesh;
        mesh.vertices = displacedVertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // Stop resetting when close enough to the base state
        if (Vector3.Distance(displacedVertices[0], baseVertices[0]) < 0.001f)
        {
            isResetting = false;
        }
    }
}




