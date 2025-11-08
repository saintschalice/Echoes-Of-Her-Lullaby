using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Global diary manager that syncs with SaveSystem.
/// Stores collected diary pages as item IDs and loads corresponding sprites.
/// </summary>
public class GlobalDiaryManager : MonoBehaviour
{
    public static GlobalDiaryManager Instance { get; private set; }

    // Maps IDs -> sprites (assigned in inspector)
    [Header("Registered Diary Pages")]
    public List<PageEntry> registeredPages = new List<PageEntry>();

    // Runtime storage
    private List<Sprite> collectedSprites = new List<Sprite>();
    private List<string> collectedIds = new List<string>();

    public event Action OnPagesChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadFromSaveSystem();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Loads diary page IDs from SaveSystem and rebuilds the sprite list.
    /// Call this after loading a save slot.
    /// </summary>
    public void LoadFromSaveSystem()
    {
        collectedSprites.Clear();
        collectedIds.Clear();

        if (SaveSystem.Instance == null || SaveSystem.Instance.GetCurrentSaveData() == null)
            return;

        var inventory = SaveSystem.Instance.GetCurrentSaveData().inventoryItems;

        foreach (var itemId in inventory)
        {
            var entry = registeredPages.Find(e => e.pageId == itemId);
            if (entry != null)
            {
                collectedIds.Add(entry.pageId);
                collectedSprites.Add(entry.sprite);
            }
        }

        OnPagesChanged?.Invoke();
    }

    /// <summary>
    /// Called when player picks up a new diary page.
    /// Automatically writes to SaveSystem.
    /// </summary>
    public void AddDiaryPage(string id)
    {
        if (collectedIds.Contains(id)) return;

        var entry = registeredPages.Find(e => e.pageId == id);
        if (entry == null)
        {
            Debug.LogError($"[GlobalDiaryManager] Page ID not registered: {id}");
            return;
        }

        collectedIds.Add(id);
        collectedSprites.Add(entry.sprite);

        // Save to SaveSystem
        SaveSystem.Instance.AddInventoryItem(id);

        OnPagesChanged?.Invoke();
    }

    public List<Sprite> GetCollectedSprites() => new List<Sprite>(collectedSprites);
    public List<string> GetCollectedIds() => new List<string>(collectedIds);

    public int Count => collectedIds.Count;
}

[Serializable]
public class PageEntry
{
    public string pageId;
    public Sprite sprite;
}
