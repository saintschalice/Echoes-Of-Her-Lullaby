using UnityEngine;

public class AtmosphericLighting : MonoBehaviour
{
    [Header("Main Light")]
    public Light characterLight;

    [Header("Flickering Effect")]
    public bool enableFlicker = true;
    public float flickerSpeed = 2f;
    public float flickerAmount = 0.3f;

    [Header("Range Adjustment")]
    public float baseRange = 8f;
    public float rangeVariation = 2f;

    private float baseIntensity;
    private float targetIntensity;
    private float flickerTimer;

    void Start()
    {
        if (characterLight == null)
            characterLight = GetComponentInChildren<Light>();

        baseIntensity = characterLight.intensity;
        targetIntensity = baseIntensity;
    }

    void Update()
    {
        if (enableFlicker)
        {
            ApplyFlickerEffect();
        }
    }

    void ApplyFlickerEffect()
    {
        flickerTimer += Time.deltaTime * flickerSpeed;

        // Create natural candle-like flicker
        float flicker = Mathf.PerlinNoise(flickerTimer, 0) * flickerAmount;
        float rangeFlicker = Mathf.PerlinNoise(flickerTimer * 0.5f, 100) * rangeVariation;

        characterLight.intensity = baseIntensity + flicker;
        characterLight.range = baseRange + rangeFlicker;
    }
}