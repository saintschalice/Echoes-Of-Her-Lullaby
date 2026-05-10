using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// Displays a full-screen item notification when player picks up an item
/// Shows item name/description and "Tap anywhere to continue" prompt
/// </summary>
public class ItemNotificationUI : MonoBehaviour
{
    public static ItemNotificationUI Instance { get; private set; }

    [Header("UI Components")]
    public GameObject notificationPanel;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public GameObject tapToContinuePrompt;
    public Image itemIconImage; // Optional: to show item sprite
    public Image itemIconGlow; // Optional: glowing background for icon

    [Header("Animation Settings")]
    public float fadeInDuration = 0.3f;
    public float fadeOutDuration = 0.3f;
    public float glowPulseSpeed = 2f; // Speed of glow pulsing

    private CanvasGroup canvasGroup;
    private bool isShowing = false;
    private bool waitingForInput = false;
    private System.Collections.Generic.Queue<ItemNotificationData> notificationQueue = new System.Collections.Generic.Queue<ItemNotificationData>();
    private GameObject cachedJoystickUI; // Cache joystick reference

    // Data structure for queued notifications
    private class ItemNotificationData
    {
        public string itemName;
        public string description;
        public Sprite itemIcon;

        public ItemNotificationData(string name, string desc, Sprite icon)
        {
            itemName = name;
            description = desc;
            itemIcon = icon;
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (transform.parent != null)
            {
                transform.SetParent(null, true);
            }
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        canvasGroup = notificationPanel?.GetComponent<CanvasGroup>();
        if (canvasGroup == null && notificationPanel != null)
        {
            canvasGroup = notificationPanel.AddComponent<CanvasGroup>();
        }
    }

    void Start()
    {
        if (notificationPanel != null)
        {
            notificationPanel.SetActive(false);
        }

        if (tapToContinuePrompt != null)
        {
            tapToContinuePrompt.SetActive(false);
        }
    }

    void Update()
    {
        if (waitingForInput && isShowing)
        {
            // Check for any input to continue
            if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
            {
                StartCoroutine(HideNotification());
            }
        }
    }

    /// <summary>
    /// Show item notification with name and description
    /// </summary>
    public void ShowItemNotification(string itemName, string description = "", Sprite itemIcon = null)
    {
        // Add to queue
        notificationQueue.Enqueue(new ItemNotificationData(itemName, description, itemIcon));

        // If not currently showing, start showing
        if (!isShowing)
        {
            StartCoroutine(ProcessNotificationQueue());
        }
    }

    IEnumerator ProcessNotificationQueue()
    {
        while (notificationQueue.Count > 0)
        {
            // Dequeue BEFORE any waits to prevent queue empty error
            ItemNotificationData data = notificationQueue.Dequeue();
            
            // Show notification immediately (don't wait for dialogue)
            // Notification should appear BEFORE dialogue, not after
            yield return StartCoroutine(ShowNotificationCoroutine(data.itemName, data.description, data.itemIcon));
            
            // Small delay between notifications (if multiple items)
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator ShowNotificationCoroutine(string itemName, string description, Sprite itemIcon)
    {
        isShowing = true;
        waitingForInput = false;

        // Pause game
        EmilyGhost emilyAI = FindFirstObjectByType<EmilyGhost>();
        if (emilyAI != null) emilyAI.isPaused = true;

        // Disable player controls
        JoystickPlayerController playerController = JoystickPlayerController.Instance;
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Disable joystick UI - try multiple names and cache it
        if (cachedJoystickUI == null)
        {
            cachedJoystickUI = GameObject.Find("Joystick");
            if (cachedJoystickUI == null)
            {
                cachedJoystickUI = GameObject.Find("FloatingJoystick");
            }
            if (cachedJoystickUI == null)
            {
                cachedJoystickUI = GameObject.Find("VariableJoystick");
            }
        }
        
        if (cachedJoystickUI != null)
        {
            cachedJoystickUI.SetActive(false);
            Debug.Log($"[ItemNotification] Joystick hidden: {cachedJoystickUI.name}");
        }
        else
        {
            Debug.LogWarning("[ItemNotification] Could not find joystick to hide!");
        }

        // Close inventory if open (but don't disable the button)
        if (InventoryUI.Instance != null && InventoryUI.Instance.IsOpen)
        {
            InventoryUI.Instance.CloseInventory();
            Debug.Log("[ItemNotification] Closed open inventory");
        }

        // Set text
        if (itemNameText != null)
        {
            itemNameText.text = itemName;
        }

        if (itemDescriptionText != null)
        {
            itemDescriptionText.text = description;
        }

        // Set icon if provided
        if (itemIconImage != null && itemIcon != null)
        {
            itemIconImage.sprite = itemIcon;
            itemIconImage.gameObject.SetActive(true);
            
            // Enable glow if available
            if (itemIconGlow != null)
            {
                itemIconGlow.gameObject.SetActive(true);
                StartCoroutine(AnimateGlow());
            }
        }
        else if (itemIconImage != null)
        {
            itemIconImage.gameObject.SetActive(false);
            if (itemIconGlow != null) itemIconGlow.gameObject.SetActive(false);
        }

        // Show panel
        if (notificationPanel != null)
        {
            notificationPanel.SetActive(true);
        }

        // Fade in
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            float elapsed = 0f;
            while (elapsed < fadeInDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);
                yield return null;
            }
            canvasGroup.alpha = 1f;
        }

        // Show tap to continue prompt
        if (tapToContinuePrompt != null)
        {
            tapToContinuePrompt.SetActive(true);
            StartCoroutine(AnimateTapPrompt());
        }

        waitingForInput = true;

        Debug.Log("[ItemNotification] Showing notification, waiting for input...");
    }

    IEnumerator HideNotification()
    {
        waitingForInput = false;

        // Stop tap prompt animation
        if (tapToContinuePrompt != null)
        {
            tapToContinuePrompt.SetActive(false);
        }

        // Fade out
        if (canvasGroup != null)
        {
            float elapsed = 0f;
            while (elapsed < fadeOutDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeOutDuration);
                yield return null;
            }
            canvasGroup.alpha = 0f;
        }

        // Hide panel
        if (notificationPanel != null)
        {
            notificationPanel.SetActive(false);
        }

        // Resume game
        EmilyGhost emilyAI = FindFirstObjectByType<EmilyGhost>();
        if (emilyAI != null) emilyAI.isPaused = false;

        // Re-enable player controls
        JoystickPlayerController playerController = JoystickPlayerController.Instance;
        if (playerController != null)
        {
            playerController.enabled = true;
        }

        // CRITICAL FIX: Always re-enable joystick after notification
        // Use cached reference first, then search if needed
        if (cachedJoystickUI != null)
        {
            cachedJoystickUI.SetActive(true);
            Debug.Log($"[ItemNotification] Joystick re-enabled: {cachedJoystickUI.name}");
        }
        else
        {
            // Fallback: search again if cache is null
            cachedJoystickUI = GameObject.Find("Joystick");
            if (cachedJoystickUI == null)
            {
                cachedJoystickUI = GameObject.Find("FloatingJoystick");
            }
            if (cachedJoystickUI == null)
            {
                cachedJoystickUI = GameObject.Find("VariableJoystick");
            }
            
            if (cachedJoystickUI != null)
            {
                cachedJoystickUI.SetActive(true);
                Debug.Log($"[ItemNotification] Joystick found and re-enabled: {cachedJoystickUI.name} (fallback)");
            }
            else
            {
                Debug.LogWarning("[ItemNotification] Could not find joystick to re-enable! Tried: Joystick, FloatingJoystick, VariableJoystick");
            }
        }

        // Inventory button should work now - no need to manually restore
        Debug.Log("[ItemNotification] Controls restored, inventory button should work now");

        isShowing = false;

        Debug.Log("[ItemNotification] Notification hidden, game resumed");
    }

    IEnumerator AnimateTapPrompt()
    {
        if (tapToContinuePrompt == null) yield break;

        CanvasGroup promptCanvasGroup = tapToContinuePrompt.GetComponent<CanvasGroup>();
        if (promptCanvasGroup == null)
        {
            promptCanvasGroup = tapToContinuePrompt.AddComponent<CanvasGroup>();
        }

        float time = 0f;
        float fadeDuration = 1.5f; // Duration for one fade cycle

        while (tapToContinuePrompt.activeSelf)
        {
            time += Time.unscaledDeltaTime;
            
            // Fade in and out using sine wave (0 to 1 to 0)
            float alpha = (Mathf.Sin(time * (Mathf.PI / fadeDuration)) + 1f) * 0.5f;
            // Clamp between 0.3 and 1.0 so it's never completely invisible
            alpha = Mathf.Lerp(0.3f, 1f, alpha);
            
            promptCanvasGroup.alpha = alpha;
            yield return null;
        }

        promptCanvasGroup.alpha = 1f;
    }

    IEnumerator AnimateGlow()
    {
        if (itemIconGlow == null) yield break;

        Color originalColor = itemIconGlow.color;
        float time = 0f;

        while (itemIconGlow.gameObject.activeSelf && isShowing)
        {
            time += Time.unscaledDeltaTime * glowPulseSpeed;
            float alpha = 0.5f + Mathf.Sin(time) * 0.3f; // Pulse between 0.2 and 0.8
            itemIconGlow.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        itemIconGlow.color = originalColor;
    }

    /// <summary>
    /// Check if notification is currently showing
    /// </summary>
    public bool IsShowing()
    {
        return isShowing;
    }
}
