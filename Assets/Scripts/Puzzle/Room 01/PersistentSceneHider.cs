using UnityEngine;

/// <summary>
/// Hides all visible objects in PersistentScene during intro cutscene.
/// Attach this to a GameObject in PersistentScene.
/// </summary>
public class PersistentSceneHider : MonoBehaviour
{
    [Header("Objects to Hide During Cutscene")]
    [Tooltip("Lisa GameObject")]
    public GameObject lisa;
    
    [Tooltip("PersistentUI (contains joystick, inventory, etc.)")]
    public GameObject persistentUI;
    
    [Tooltip("Any other objects that should be hidden during cutscene")]
    public GameObject[] otherObjectsToHide;
    
    [Header("Settings")]
    [Tooltip("Hide objects on new game (first time playing)")]
    public bool hideOnNewGame = true;
    
    [Header("Debug")]
    public bool debugMode = true;
    
    private bool objectsHidden = false;
    
    public static PersistentSceneHider Instance { get; private set; }
    
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
        if (!hideOnNewGame) return;
        
        // Check if this is a new game
        if (PlayerPrefs.HasKey("LoadSlotOnStart"))
        {
            int loadSlot = PlayerPrefs.GetInt("LoadSlotOnStart");
            
            if (loadSlot == -1)
            {
                // NEW GAME - Hide everything
                HideAllObjects();
                Debug.Log("[PersistentHider] NEW GAME - All persistent objects hidden");
            }
            else
            {
                // LOAD GAME - Show everything
                ShowAllObjects();
                Debug.Log("[PersistentHider] LOAD GAME - All persistent objects visible");
            }
        }
        else
        {
            // No flag, assume normal gameplay
            ShowAllObjects();
        }
    }
    
    /// <summary>
    /// Hide Lisa, UI, and other objects for cutscene
    /// </summary>
    public void HideAllObjects()
    {
        if (objectsHidden) return;
        
        if (lisa != null)
        {
            lisa.SetActive(false);
            if (debugMode) Debug.Log("[PersistentHider] Lisa hidden");
        }
        
        if (persistentUI != null)
        {
            persistentUI.SetActive(false);
            if (debugMode) Debug.Log("[PersistentHider] PersistentUI hidden (joystick, inventory, etc.)");
        }
        
        foreach (var obj in otherObjectsToHide)
        {
            if (obj != null)
            {
                obj.SetActive(false);
                if (debugMode) Debug.Log($"[PersistentHider] {obj.name} hidden");
            }
        }
        
        objectsHidden = true;
    }
    
    /// <summary>
    /// Show Lisa, UI, and other objects after cutscene
    /// </summary>
    public void ShowAllObjects()
    {
        if (lisa != null)
        {
            lisa.SetActive(true);
            if (debugMode) Debug.Log("[PersistentHider] Lisa shown");
        }
        
        if (persistentUI != null)
        {
            persistentUI.SetActive(true);
            if (debugMode) Debug.Log("[PersistentHider] PersistentUI shown (joystick, inventory, etc.)");
        }
        
        foreach (var obj in otherObjectsToHide)
        {
            if (obj != null)
            {
                obj.SetActive(true);
                if (debugMode) Debug.Log($"[PersistentHider] {obj.name} shown");
            }
        }
        
        objectsHidden = false;
    }
    
    /// <summary>
    /// Check if objects are currently hidden
    /// </summary>
    public bool AreObjectsHidden()
    {
        return objectsHidden;
    }
}
