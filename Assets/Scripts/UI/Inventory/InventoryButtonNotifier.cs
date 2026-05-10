using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Shows a notification badge/highlight on the inventory button when new items are obtained
/// </summary>
public class InventoryButtonNotifier : MonoBehaviour
{
    public static InventoryButtonNotifier Instance { get; private set; }

    [Header("UI References")]
    public GameObject notificationBadge; // Red dot or exclamation mark
    public Image glowEffect; // Yellow glow around button

    [Header("Animation Settings")]
    public float pulseSpeed = 2f;
    public Color glowColor = new Color(1f, 1f, 0f, 0.8f); // Yellow

    private int newItemCount = 0;
    private bool isAnimating = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        HideNotification();
    }

    /// <summary>
    /// Show notification badge when new item is obtained
    /// </summary>
    public void ShowNewItemNotification()
    {
        newItemCount++;

        if (notificationBadge != null)
        {
            notificationBadge.SetActive(true);
        }

        if (glowEffect != null && !isAnimating)
        {
            glowEffect.gameObject.SetActive(true);
            StartCoroutine(AnimateGlow());
        }

        Debug.Log($"[InventoryButton] Showing notification badge (new items: {newItemCount})");
    }

    /// <summary>
    /// Hide notification badge when inventory is opened
    /// </summary>
    public void HideNotification()
    {
        newItemCount = 0;
        isAnimating = false;

        if (notificationBadge != null)
        {
            notificationBadge.SetActive(false);
        }

        if (glowEffect != null)
        {
            glowEffect.gameObject.SetActive(false);
        }

        Debug.Log("[InventoryButton] Hiding notification badge");
    }

    IEnumerator AnimateGlow()
    {
        if (glowEffect == null) yield break;

        isAnimating = true;
        float time = 0f;

        while (isAnimating && newItemCount > 0)
        {
            time += Time.deltaTime * pulseSpeed;
            float alpha = 0.4f + Mathf.Sin(time) * 0.4f; // Pulse between 0 and 0.8
            Color color = glowColor;
            color.a = alpha;
            glowEffect.color = color;
            yield return null;
        }

        isAnimating = false;
    }

    /// <summary>
    /// Check if there are new items
    /// </summary>
    public bool HasNewItems()
    {
        return newItemCount > 0;
    }
}
