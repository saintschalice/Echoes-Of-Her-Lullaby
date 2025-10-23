using UnityEngine;
using System.Collections;

/// <summary>
/// Manages UI state when dialogues appear.
/// Reopens inventory/UI elements if they were open before dialogue started.
/// Now safely ignores custom UIs like the DiaryReaderUI.
/// </summary>
public class UIStateManager : MonoBehaviour
{
    public static UIStateManager Instance { get; private set; }

    private bool wasInventoryOpen = false;
    private bool isProcessingDialogue = false;
    private InventoryUI inventoryUI;
    private Coroutine reopenCoroutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[UIStateManager] Instance created");
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Hook into dialogue and inventory events
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();

        // Find InventoryUI
        inventoryUI = FindFirstObjectByType<InventoryUI>();

        // Hook into InventoryManager events
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnItemUsed += OnItemUsed;
            Debug.Log("[UIStateManager] Hooked into InventoryManager events");
        }
    }

    void OnDestroy()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnItemUsed -= OnItemUsed;
        }

        if (reopenCoroutine != null)
        {
            StopCoroutine(reopenCoroutine);
        }
    }

    public void OnDialogueStarted()
    {
        if (inventoryUI != null && inventoryUI.IsOpen)
        {
            wasInventoryOpen = true;
            inventoryUI.ForceCloseInventory();
            isProcessingDialogue = true;
            Debug.Log("[UIStateManager] Dialogue started - inventory was open, closing it");
        }
        else
        {
            wasInventoryOpen = false;
            isProcessingDialogue = true;
        }
    }

    public void OnDialogueEnded()
    {
        if (isProcessingDialogue && wasInventoryOpen)
        {
            if (reopenCoroutine != null) StopCoroutine(reopenCoroutine);
            reopenCoroutine = StartCoroutine(ReopenInventoryDelayed());
        }
        isProcessingDialogue = false;
    }

    void OnItemUsed(InventoryItem item)
    {
        Debug.Log($"[UIStateManager] Item used: {item.itemName}");

        // Remember if inventory was open during item use
        if (inventoryUI != null && inventoryUI.IsOpen)
        {
            wasInventoryOpen = true;
            Debug.Log("[UIStateManager] Item used with inventory open - will reopen after dialogue");
        }
    }

    void Update()
    {
        // 🔒 NEW: Prevent UI interference if Diary or other custom reader UI is open
        DiaryReaderUI diary = FindFirstObjectByType<DiaryReaderUI>(FindObjectsInactive.Include);
        if (diary != null && diary.IsReaderOpen())
        {
            // Skip any automatic reopening or dialogue UI logic
            return;
        }

        // Monitor dialogue state changes
        if (isProcessingDialogue && wasInventoryOpen)
        {
            bool dialogueActive = DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive();

            if (!dialogueActive)
            {
                if (reopenCoroutine != null) StopCoroutine(reopenCoroutine);
                reopenCoroutine = StartCoroutine(ReopenInventoryDelayed());
                isProcessingDialogue = false;
            }
        }
    }

    IEnumerator ReopenInventoryDelayed()
    {
        // Wait a bit longer to ensure everything is properly closed
        yield return new WaitForSeconds(0.5f);

        // Double-check dialogue is really closed
        if (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield break;
        }

        // 🔒 Skip reopening if Diary UI is open
        DiaryReaderUI diary = FindFirstObjectByType<DiaryReaderUI>(FindObjectsInactive.Include);
        if (diary != null && diary.IsReaderOpen())
        {
            yield break;
        }

        // Restore inventory if it was previously open
        if (inventoryUI != null && wasInventoryOpen)
        {
            Debug.Log("[UIStateManager] Reopening inventory");
            inventoryUI.OpenInventory();
            inventoryUI.RefreshInventory();
            wasInventoryOpen = false;
        }

        reopenCoroutine = null;
    }

    /// <summary>
    /// Call this before showing any dialogue.
    /// </summary>
    public void PrepareForDialogue()
    {
        OnDialogueStarted();
    }

    /// <summary>
    /// Call this after dialogue ends.
    /// </summary>
    public void DialogueComplete()
    {
        OnDialogueEnded();
    }

    /// <summary>
    /// Force restore UI immediately.
    /// </summary>
    public void ForceRestoreUI()
    {
        if (inventoryUI != null && wasInventoryOpen)
        {
            inventoryUI.OpenInventory();
            inventoryUI.RefreshInventory();
            wasInventoryOpen = false;
            Debug.Log("[UIStateManager] Force restored inventory");
        }
    }
}
