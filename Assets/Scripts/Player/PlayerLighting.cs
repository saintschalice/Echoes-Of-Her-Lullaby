using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLighting : MonoBehaviour
{
    [Header("Flashlight Settings")]
    public float flashlightRange = 10f;
    public float flashlightIntensity = 2f;
    public Color lightColor = new Color(1f, 0.95f, 0.8f);

    [Header("Cone Settings")]
    [Range(30f, 120f)]
    public float coneAngle = 60f;
    public float coneOffset = 0.5f; // How far in front of character the cone starts

    [Header("Rotation Settings")]
    public bool smoothRotation = true;
    public float rotationSpeed = 10f;
    public bool snapToCardinalDirections = false;

    [Header("Flicker Settings")]
    public bool enableFlicker = true;
    public float flickerSpeed = 3f;
    public float flickerIntensity = 0.15f;

    // Components
    private GameObject flashlightContainer;
    private Light2D mainLight;
    private Light2D[] coneLights;
    private JoystickPlayerController playerController;

    // State
    private Vector2 currentDirection = Vector2.down;
    private float currentRotation = -90f;
    private float targetRotation = -90f;
    private float flickerTimer = 0f;

    void Start()
    {
        if (Application.isPlaying)
        {
            playerController = GetComponent<JoystickPlayerController>();
        }

        CreateFlashlightSystem();
    }

    void OnValidate()
    {
        CreateFlashlightSystem();
        UpdateConeShape();
    }

    void CreateFlashlightSystem()
    {
        // Find or create container
        if (flashlightContainer == null)
        {
            Transform containerT = transform.Find("FlashlightSystem");
            flashlightContainer = (containerT != null) ? containerT.gameObject : new GameObject("FlashlightSystem");
            flashlightContainer.transform.SetParent(transform);
            flashlightContainer.transform.localPosition = Vector3.zero;
        }

        // Find or create main Light2D
        if (mainLight == null)
        {
            Transform lightT = flashlightContainer.transform.Find("FlashlightFreeform");
            mainLight = (lightT != null) ? lightT.GetComponent<Light2D>() : null;
        }

        if (mainLight == null)
        {
            CreateFlashlight();
        }
    }

    void CreateFlashlight()
    {
        GameObject mainLightObj = new GameObject("FlashlightFreeform");
        mainLightObj.transform.SetParent(flashlightContainer.transform);

        mainLightObj.transform.localPosition = new Vector3(0, coneOffset, 0);

        mainLight = mainLightObj.AddComponent<Light2D>();

        // **CRITICAL FIX: Use the recommended Freeform Light Type**
        mainLight.lightType = Light2D.LightType.Freeform;

        // Configure properties
        mainLight.intensity = flashlightIntensity;
        mainLight.color = lightColor;
        mainLight.pointLightOuterRadius = flashlightRange;

        mainLight.pointLightInnerRadius = 0.5f;
        mainLight.shadowsEnabled = false;

        coneLights = new Light2D[] { mainLight };

        // Initial shape definition
        UpdateConeShape();
    }

    void UpdateConeShape()
    {
        if (mainLight == null || mainLight.lightType != Light2D.LightType.Freeform) return;

        // 1. Calculate the width of the cone's base at the flashlightRange distance
        float halfAngleRad = (coneAngle * 0.5f) * Mathf.Deg2Rad;
        float coneWidth = flashlightRange * Mathf.Tan(halfAngleRad) * 2f;

        // 2. Define the vertices of the triangular cone (Origin, Left Base, Right Base)
        Vector3[] coneVertices = new Vector3[3];
        coneVertices[0] = Vector3.zero;
        coneVertices[1] = new Vector3(-coneWidth * 0.5f, flashlightRange, 0f);
        coneVertices[2] = new Vector3(coneWidth * 0.5f, flashlightRange, 0f);

        // 3. Set the Freeform shape (using a known stable workaround for older APIs)
        // Check if the light component exposes the shape property directly (common in newer URP)
        if (mainLight.GetType().GetProperty("shape") != null)
        {
            mainLight.GetType().GetProperty("shape").GetValue(mainLight, null)
                .GetType().GetProperty("vertices").SetValue(mainLight.GetType().GetProperty("shape").GetValue(mainLight, null), coneVertices);
        }
        // If the above reflection fails, we rely on the Light Cookie/Manual setup (Step 3)
    }

    void Update()
    {
        if (Application.isPlaying)
        {
            UpdateFlashlightDirection();

            if (enableFlicker)
            {
                ApplyFlickerEffect();
            }
        }
    }

    void UpdateFlashlightDirection()
    {
        if (playerController == null) return;

        Vector2 movement = playerController.GetMovementDirection();

        if (movement.magnitude > 0.1f)
        {
            currentDirection = movement.normalized;
            targetRotation = snapToCardinalDirections
                ? GetCardinalRotation(currentDirection)
                : Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg + 90f;
        }

        currentRotation = smoothRotation
            ? Mathf.LerpAngle(currentRotation, targetRotation, rotationSpeed * Time.deltaTime)
            : targetRotation;

        flashlightContainer.transform.localRotation = Quaternion.Euler(0, 0, currentRotation);
    }

    float GetCardinalRotation(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0 ? 90f : -90f;
        }
        else
        {
            return direction.y > 0 ? 180f : 0f;
        }
    }

    void ApplyFlickerEffect()
    {
        flickerTimer += Time.deltaTime * flickerSpeed;

        float flicker = Mathf.PerlinNoise(flickerTimer * 2f, 0f);
        float intensityMultiplier = 1f - (flickerIntensity * 0.5f) + (flicker * flickerIntensity);

        if (mainLight != null)
        {
            mainLight.intensity = flashlightIntensity * intensityMultiplier;
        }
    }

    public void SetFlashlightEnabled(bool enabled)
    {
        if (flashlightContainer != null)
        {
            flashlightContainer.SetActive(enabled);
        }
    }

    public void SetIntensity(float intensity)
    {
        flashlightIntensity = Mathf.Clamp(intensity, 0f, 10f);

        if (!enableFlicker && mainLight != null)
        {
            mainLight.intensity = flashlightIntensity;
        }
    }

    public void SetConeAngle(float angle)
    {
        coneAngle = Mathf.Clamp(angle, 30f, 120f);
        UpdateConeShape();
    }

    void OnDrawGizmosSelected()
    {
        if (flashlightContainer == null || mainLight == null) return;

        Gizmos.color = Color.yellow;

        Vector3 lightPosition = mainLight.transform.position;
        Vector3 forward = Quaternion.Euler(0, 0, currentRotation - 90f) * Vector3.up;
        float halfAngle = coneAngle * 0.5f;

        Vector3 leftEdge = Quaternion.Euler(0, 0, -halfAngle) * forward;
        Vector3 rightEdge = Quaternion.Euler(0, 0, halfAngle) * forward;

        Gizmos.DrawRay(lightPosition, leftEdge * flashlightRange);
        Gizmos.DrawRay(lightPosition, rightEdge * flashlightRange);

        // Draw arc for Gizmo visualization
        int segments = 20;
        Vector3 prevDir = Vector3.zero;

        for (int i = 0; i <= segments; i++)
        {
            float angle = -halfAngle + (halfAngle * 2f * i / segments);
            Vector3 dir = Quaternion.Euler(0, 0, angle) * forward;

            if (i > 0)
            {
                float prevAngle = -halfAngle + (halfAngle * 2f * (i - 1) / segments);
                prevDir = Quaternion.Euler(0, 0, prevAngle) * forward;

                Gizmos.DrawLine(
                    lightPosition + prevDir * flashlightRange,
                    lightPosition + dir * flashlightRange
                );
            }
        }
    }
}