using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLighting : MonoBehaviour
{
    [Header("Light Settings")]
    public float baseRadius = 8f;
    public float baseIntensity = 2f;
    public Color lightColor = new Color(1f, 0.8f, 0.4f); // Warm candlelight color

    [Header("Flickering Settings")]
    public float flickerSpeed = 3f;
    public float radiusVariation = 0.5f;
    public float intensityVariation = 0.3f;

    public Light2D playerLight;
    private float flickerTimer;

    void Start()
    {
        if (playerLight == null)
        {
            GameObject lightObj = new GameObject("PlayerLight2D");
            lightObj.transform.SetParent(transform);
            lightObj.transform.localPosition = Vector3.zero;

            playerLight = lightObj.AddComponent<Light2D>();
        }

        ConfigureLight();
    }

    void ConfigureLight()
    {
        playerLight.lightType = Light2D.LightType.Point;
        playerLight.pointLightOuterRadius = baseRadius;
        playerLight.intensity = baseIntensity;
        playerLight.color = lightColor;
    }

    void Update()
    {
        ApplyFlickerEffect();
    }

    void ApplyFlickerEffect()
    {
        flickerTimer += Time.deltaTime * flickerSpeed;

        float radiusFlicker = Mathf.PerlinNoise(flickerTimer, 0) * radiusVariation;
        float intensityFlicker = Mathf.PerlinNoise(flickerTimer * 1.5f, 100) * intensityVariation;

        playerLight.pointLightOuterRadius = baseRadius + radiusFlicker;
        playerLight.intensity = baseIntensity + intensityFlicker;
    }
}