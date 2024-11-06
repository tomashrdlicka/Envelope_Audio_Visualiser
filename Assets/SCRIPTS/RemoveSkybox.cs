using UnityEngine;

public class SkyboxRemover : MonoBehaviour
{
    void Start()
    {
        // Remove the skybox by setting it to null
        RenderSettings.skybox = null;

        // Optional: Set the camera's clear flags to a solid color
        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.backgroundColor = Color.black; // Set the background color as desired
    }
}

