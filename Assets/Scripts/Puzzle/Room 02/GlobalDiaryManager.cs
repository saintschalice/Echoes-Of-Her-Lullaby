using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Global diary manager that syncs with SaveSystem.
/// Pages are persisted via save flags (not inventory) so inventory stays clean.
/// After any 2 pages are collected, the player receives a single "diary_entries" item.
/// </summary>
public class GlobalDiaryManager : MonoBehaviour
{
    public static GlobalDiaryManager Instance { get; private set; }

    [Header("Registered Diary Pages")]
    [Tooltip("Add all diary pages here (id -> sprite).")]
    public List<PageEntry> registeredPages = new List<PageEntry>();

    // Runtime storage
    private readonly List<Sprite> collectedSprites = new List<Sprite>();
    private readonly List<string> collectedIds = new List<string>();

    public event Action OnPagesChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.OnGameLoaded += OnGameLoaded;
            SaveSystem.Instance.OnGameSaved += OnGameSaved;
        }

        LoadFromSaveSystem();
    }

    void OnDestroy()
    {
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.OnGameLoaded -= OnGameLoaded;
            SaveSystem.Instance.OnGameSaved -= OnGameSaved;
        }
    }

    void OnGameLoaded(GameSaveData _)
    {
        LoadFromSaveSystem();
    }

    void OnGameSaved(GameSaveData _)
    {
        // reserved
    }

    /// <summary>
    /// Persist page-ownership using save flags "diary_collected_{id}".
    /// Rebuild runtime sprite list from those flags.
    /// </summary>
    public void LoadFromSaveSystem()
    {
        collectedSprites.Clear();
        collectedIds.Clear();

        if (SaveSystem.Instance == null || SaveSystem.Instance.GetCurrentSaveData() == null)
        {
            Debug.Log("[GlobalDiaryManager] No save data available");
            return;
        }

        foreach (var entry in registeredPages)
        {
            if (entry == null || string.IsNullOrEmpty(entry.pageId)) continue;

            string flag = $"diary_collected_{entry.pageId}";
            bool owned = SaveSystem.Instance.WasDialogueTriggered(flag);
            if (owned)
            {
                collectedIds.Add(entry.pageId);
                if (entry.sprite != null)
                    collectedSprites.Add(entry.sprite);
            }
        }

        // Inventory cleanup: remove any stale diary_page_* items that might still be present.
        CleanupIndividualDiaryPageItemsFromInventory();

        OnPagesChanged?.Invoke();
        Debug.Log($"[GlobalDiaryManager] Loaded {collectedIds.Count} diary pages (via flags).");
    }

    public int GetCollectedPageCount() => collectedIds.Count;

    /// <summary>
    /// Add a diary page (id) and persist via save flag. Combines into "diary_entries" at 2+ pages.
    /// </summary>
    public void AddDiaryPage(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogError("[GlobalDiaryManager] Tried to add diary page with empty/null id.");
            return;
        }

        // Already tracked? Don't double-count, but do a quick cleanup.
        if (collectedIds.Contains(id))
        {
            Debug.Log($"[GlobalDiaryManager] Already have page: {id}");
            CleanupIndividualDiaryPageItemsFromInventory();
            return;
        }

        var entry = registeredPages.Find(e => e.pageId == id);
        if (entry == null)
        {
            Debug.LogError($"[GlobalDiaryManager] Page ID not registered: {id}");
            return;
        }

        collectedIds.Add(id);
        if (entry.sprite != null)
            collectedSprites.Add(entry.sprite);

        // Persist via a flag so LoadFromSaveSystem can rebuild correctly
        if (SaveSystem.Instance != null)
        {
            string flag = $"diary_collected_{id}";
            if (!SaveSystem.Instance.WasDialogueTriggered(flag))
                SaveSystem.Instance.TriggerDialogue(flag);
        }

        Debug.Log($"[GlobalDiaryManager] Added diary page {id}. Total collected = {collectedIds.Count}");
        int totalPages = collectedIds.Count;
        Debug.Log($"[GlobalDiaryManager] Checking combination condition... (totalPages = {totalPages})");

        // Auto-combine once you have 2+ pages
        if (totalPages >= 2)
        {
            bool hasDiaryEntries =
                (SaveSystem.Instance != null && SaveSystem.Instance.HasItem("diary_entries")) ||
                (InventoryManager.Instance != null && InventoryManager.Instance.HasItem("diary_entries"));

            Debug.Log($"[GlobalDiaryManager] Has diary_entries already? {hasDiaryEntries}");

            if (!hasDiaryEntries)
            {
                Debug.Log("[GlobalDiaryManager] Combining pages into diary_entries...");

                // Give the combined item exactly once
                if (InventoryManager.Instance != null)
                    InventoryManager.Instance.AddItem("diary_entries");

                if (SaveSystem.Instance != null && !SaveSystem.Instance.HasItem("diary_entries"))
                    SaveSystem.Instance.AddInventoryItem("diary_entries");

                // Remove individual page items from inventory + save
                foreach (string page in collectedIds)
                {
                    if (InventoryManager.Instance != null)
                        InventoryManager.Instance.RemoveItem(page);

                    if (SaveSystem.Instance != null && SaveSystem.Instance.HasItem(page))
                        SaveSystem.Instance.RemoveInventoryItem(page);

                    Debug.Log($"[GlobalDiaryManager] Removed {page} from inventory after combination.");
                }

                // FIX: Delay dialogue until AFTER diary UI has a chance to show
                // This prevents dialogue from appearing before the diary page is visible
                StartCoroutine(ShowCombinationDialogueAfterDelay());
            }
        }

        // Mark story progress
        if (SaveSystem.Instance != null)
            SaveSystem.Instance.OnStoryProgressMade();

        // Always clean up loose diary_page_* items so we only ever use diary_entries
        CleanupIndividualDiaryPageItemsFromInventory();

        // Notify UI to refresh pages
        OnPagesChanged?.Invoke();

        Debug.Log("[GlobalDiaryManager] Finalized AddDiaryPage for " + id +
                  $". Total pages tracked = {collectedIds.Count}. Has diary_entries = " +
                  ((SaveSystem.Instance != null && SaveSystem.Instance.HasItem("diary_entries")) ||
                   (InventoryManager.Instance != null && InventoryManager.Instance.HasItem("diary_entries"))));
    }




    /// <summary>
    /// Remove any diary_page_* items from inventory and save, keeping only "diary_entries".
    /// </summary>
    private void CleanupIndividualDiaryPageItemsFromInventory()
    {
        if (InventoryManager.Instance == null) return;

        bool changed = false;
        var inv = InventoryManager.Instance;
        var toRemove = new List<string>();

        List<InventoryItem> allItems = inv.GetAllItems();
        foreach (var itm in allItems)
        {
            if (itm.itemId.StartsWith("diary_page_", StringComparison.OrdinalIgnoreCase))
                toRemove.Add(itm.itemId);
        }

        foreach (var id in toRemove)
        {
            inv.RemoveItem(id);
            Debug.Log($"[GlobalDiaryManager] Removed '{id}' from player inventory.");
            changed = true;

            // Also mirror into SaveSystem if possible
            if (SaveSystem.Instance != null && SaveSystem.Instance.HasItem(id))
            {
                SaveSystem.Instance.RemoveInventoryItem(id);
                Debug.Log($"[GlobalDiaryManager] Removed '{id}' from save data.");
            }
        }

        if (changed && SaveSystem.Instance != null)
        {
            SaveSystem.Instance.OnStoryProgressMade();
        }
    }


    // Public accessors
    public List<Sprite> GetCollectedSprites() => new List<Sprite>(collectedSprites);
    public List<string> GetCollectedIds() => new List<string>(collectedIds);
    public int Count => collectedIds.Count;

    public bool HasDiaryPage(string pageId) => collectedIds.Contains(pageId);

    [ContextMenu("Debug: Show Collected Pages")]
    void ContextMenuDebugPages()
    {
        Debug.Log($"=== Collected Diary Pages ({collectedIds.Count}) ===");
        foreach (string id in collectedIds) Debug.Log($"  - {id}");
    }

    [ContextMenu("Debug: Add Test Page")]
    void ContextMenuAddTestPage()
    {
        AddDiaryPage("diary_page_1");
    }

    /// <summary>
    /// Coroutine to show combination dialogue AFTER a delay.
    /// This ensures the diary UI has time to show the page before dialogue appears.
    /// </summary>
    private IEnumerator ShowCombinationDialogueAfterDelay()
    {
        // Wait for item notification to finish (if showing)
        while (ItemNotificationUI.Instance != null && ItemNotificationUI.Instance.IsShowing())
        {
            yield return null;
        }

        // Additional small delay to ensure diary UI is visible
        yield return new WaitForSeconds(0.5f);

        // Now show the dialogue
        if (DialogueSystemV2.Instance != null)
        {
            DialogueSystemV2.Instance.StartDialogue(
                "These pages fit together... I can now read them in my diary.",
                "Lisa"
            );
            
            Debug.Log("[GlobalDiaryManager] Combination dialogue started after notification");
        }
        else
        {
            Debug.LogWarning("[GlobalDiaryManager] DialogueSystemV2.Instance is null!");
        }
    }

    public Sprite GetSpriteForPageId(string pageId)
    {
        if (string.IsNullOrEmpty(pageId)) return null;
        foreach (var entry in registeredPages)
        {
            if (entry == null) continue;
            if (string.Equals(entry.pageId, pageId, StringComparison.OrdinalIgnoreCase))
                return entry.sprite;
        }
        return null;
    }
}

[Serializable]
public class PageEntry
{
    public string pageId;
    public Sprite sprite;
}
