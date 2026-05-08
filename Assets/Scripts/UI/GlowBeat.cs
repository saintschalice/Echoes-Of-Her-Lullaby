using UnityEngine;
using UnityEngine.UI;
using TMPro; // Kailangan ito para makilala ang TextMeshPro

public class GlowBeat : MonoBehaviour
{
    [Header("Target Elements (Kahit alin dito)")]
    public Image glowImage;
    public TextMeshProUGUI textToPulse;

    [Header("Beat Settings")]
    public float beatSpeed = 3f; // Gaano kabilis pumintig
    public float minAlpha = 0.3f; // Pinakamahinang glow/opacity
    public float maxAlpha = 1.0f; // Pinakamalakas na glow/opacity

    void Awake()
    {
        // Auto-detect components kung nakalimutang i-drag sa Inspector
        if (glowImage == null) glowImage = GetComponent<Image>();
        if (textToPulse == null) textToPulse = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // Compute ang pulse value (mula 0 hanggang 1)
        float pulse = (Mathf.Sin(Time.time * beatSpeed) + 1f) / 2f;
        float currentAlpha = Mathf.Lerp(minAlpha, maxAlpha, pulse);

        // I-apply sa Image (kung meron)
        if (glowImage != null)
        {
            Color c = glowImage.color;
            c.a = currentAlpha;
            glowImage.color = c;
        }

        // I-apply sa Text (kung meron)
        if (textToPulse != null)
        {
            Color c = textToPulse.color;
            c.a = currentAlpha;
            textToPulse.color = c;
        }
    }
}