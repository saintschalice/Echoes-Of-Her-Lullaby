using UnityEngine;

public class CameraLighting : MonoBehaviour
{
    [Header("Post-Processing")]
    public bool enableVignette = true;
    public float vignetteIntensity = 0.4f;

    void Start()
    {
        // Ensure camera renders in the right order
        Camera cam = GetComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.05f, 0.03f, 0.08f); // Very dark purple
    }
}