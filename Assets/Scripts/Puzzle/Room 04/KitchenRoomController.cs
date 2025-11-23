using UnityEngine;
using System.Collections.Generic;

public class KitchenRoomController : MonoBehaviour
{
    public static KitchenRoomController Instance { get; private set; }

    [Header("Room Settings")]
    [SerializeField] private string roomName = "Room04_KitchenDining";
    [SerializeField] private string puzzleId = "kitchen_cookie_puzzle";

    [Header("State Flags (Read-Only Debug)")]
    public bool recipeRead;
    public bool hasFlour;
    public bool hasSugar;
    public bool hasVanilla;
    public bool hasChocolate;
    public bool hasEgg;
    public bool hasSalt;
    public bool doughMixed;
    public bool ovenSetCorrect;
    public bool cookiesBakedAndStored;
    public bool floorboardObtained;
    public bool bridgePlaced;

    // Item IDs corresponding to your ItemDatabase
    private const string ITEM_FLOUR = "flour";
    private const string ITEM_SUGAR = "sugar";
    private const string ITEM_VANILLA = "vanilla";
    private const string ITEM_CHOCOLATE = "chocolate";
    private const string ITEM_EGG = "egg";
    private const string ITEM_SALT = "salt";
    private const string ITEM_RECIPE_BOOK = "recipe_book_kitchen";
    private const string ITEM_BOWL_MIX = "bowl_cookie_mix";
    private const string ITEM_FLOORBOARD = "floorboard_bridge";

    // State Markers for RoomState.interactedObjects
    private const string MARKER_RECIPE_READ = "recipe_read";
    private const string MARKER_DOUGH_MIXED = "dough_mixed";
    private const string MARKER_OVEN_SET = "oven_set_350_12";
    private const string MARKER_COOKIES_BAKED = "cookies_baked";
    private const string MARKER_BRIDGE_PLACED = "bridge_placed";

    void Awake()
    {
        // Singleton pattern scoped to the scene (destroyed on load)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        // 1. Load existing state from SaveSystem
        LoadRoomState();

        // 2. Check for First Time Entry
        HandleRoomEntry();
    }

    /// <summary>
    /// Loads flags from SaveSystem (RoomState) and InventoryManager.
    /// </summary>
    public void LoadRoomState()
    {
        if (SaveSystem.Instance == null)
        {
            Debug.LogError("[KitchenRoomController] SaveSystem is missing!");
            return;
        }

        RoomState state = SaveSystem.Instance.GetRoomState(roomName);

        // 1. Load Inventory Flags (Directly from InventoryManager or SaveSystem)
        hasFlour = SaveSystem.Instance.HasItem(ITEM_FLOUR);
        hasSugar = SaveSystem.Instance.HasItem(ITEM_SUGAR);
        hasVanilla = SaveSystem.Instance.HasItem(ITEM_VANILLA);
        hasChocolate = SaveSystem.Instance.HasItem(ITEM_CHOCOLATE);
        hasEgg = SaveSystem.Instance.HasItem(ITEM_EGG);
        hasSalt = SaveSystem.Instance.HasItem(ITEM_SALT);

        // 2. Load Interaction Flags from RoomState.interactedObjects
        recipeRead = state.interactedObjects.Contains(MARKER_RECIPE_READ) || SaveSystem.Instance.HasItem(ITEM_RECIPE_BOOK);
        doughMixed = state.interactedObjects.Contains(MARKER_DOUGH_MIXED) || SaveSystem.Instance.HasItem(ITEM_BOWL_MIX);
        ovenSetCorrect = state.interactedObjects.Contains(MARKER_OVEN_SET);
        cookiesBakedAndStored = state.interactedObjects.Contains(MARKER_COOKIES_BAKED) || SaveSystem.Instance.IsPuzzleSolved(puzzleId);
        bridgePlaced = state.interactedObjects.Contains(MARKER_BRIDGE_PLACED);

        // Floorboard logic: if we have the item, we obtained it. 
        // If the bridge is placed, we technically "used" it, but the puzzle is solved.
        floorboardObtained = SaveSystem.Instance.HasItem(ITEM_FLOORBOARD);

        Debug.Log($"[KitchenRoomController] State Loaded. RecipeRead: {recipeRead}, Baked: {cookiesBakedAndStored}, Bridge: {bridgePlaced}");
    }

    /// <summary>
    /// Handles logic when the player enters the room (Intro Dialogue).
    /// </summary>
    private void HandleRoomEntry()
    {
        RoomState state = SaveSystem.Instance.GetRoomState(roomName);

        if (!state.hasBeenVisited)
        {
            Debug.Log("[KitchenRoomController] First visit detected.");

            // Trigger Intro Dialogue
            if (DialogueSystemV2.Instance != null)
            {
                DialogueSystemV2.Instance.StartDialogue("This kitchen smells like death masked by old vanilla. Something terrible happened here.", "Lisa");
            }

            // Mark as visited
            state.hasBeenVisited = true;
            SaveSystem.Instance.UpdateRoomState(roomName, state);
        }
    }

    // ========================================================================
    // PUBLIC ACTIONS (Called by Interactables)
    // ========================================================================

    public void OnRecipeBookRead()
    {
        recipeRead = true;
        AddRoomStateMarker(MARKER_RECIPE_READ);
        Debug.Log("[KitchenRoomController] Recipe book read.");
    }

    public void OnIngredientCollected(string ingredientId)
    {
        // Just refresh flags based on ID
        switch (ingredientId)
        {
            case ITEM_FLOUR: hasFlour = true; break;
            case ITEM_SUGAR: hasSugar = true; break;
            case ITEM_VANILLA: hasVanilla = true; break;
            case ITEM_CHOCOLATE: hasChocolate = true; break;
            case ITEM_EGG: hasEgg = true; break;
            case ITEM_SALT: hasSalt = true; break;
        }
        // No need to save a marker here, InventoryManager handles item persistence.
        Debug.Log($"[KitchenRoomController] Ingredient collected: {ingredientId}");
    }

    public void OnDoughMixed()
    {
        doughMixed = true;
        AddRoomStateMarker(MARKER_DOUGH_MIXED);
        Debug.Log("[KitchenRoomController] Dough mixed.");
    }

    public void OnOvenSetCorrect()
    {
        ovenSetCorrect = true;
        AddRoomStateMarker(MARKER_OVEN_SET);
        Debug.Log("[KitchenRoomController] Oven set to 350F / 12min.");
    }

    public void OnCookiesBakedAndStored()
    {
        if (cookiesBakedAndStored) return; // Already done

        cookiesBakedAndStored = true;
        AddRoomStateMarker(MARKER_COOKIES_BAKED);

        // This is the moment the puzzle is technically "solved" enough to get the reward
        SaveSystem.Instance.MarkPuzzleSolved(puzzleId);

        Debug.Log("[KitchenRoomController] Cookies baked and stored. Puzzle Solved!");
    }

    public void OnFloorboardObtained()
    {
        floorboardObtained = true;
        Debug.Log("[KitchenRoomController] Floorboard obtained.");
    }

    public void OnBridgePlaced()
    {
        bridgePlaced = true;
        AddRoomStateMarker(MARKER_BRIDGE_PLACED);
        Debug.Log("[KitchenRoomController] Bridge placed.");
    }

    // ========================================================================
    // HELPER METHODS
    // ========================================================================

    /// <summary>
    /// Helper to add a string marker to the RoomState's interactedObjects list and save immediately.
    /// </summary>
    private void AddRoomStateMarker(string marker)
    {
        RoomState state = SaveSystem.Instance.GetRoomState(roomName);

        if (!state.interactedObjects.Contains(marker))
        {
            state.interactedObjects.Add(marker);
            SaveSystem.Instance.UpdateRoomState(roomName, state);
        }
    }

    /// <summary>
    /// Check if player has all ingredients currently in inventory.
    /// </summary>
    public bool HasAllIngredients()
    {
        // Re-check inventory to be safe
        hasFlour = SaveSystem.Instance.HasItem(ITEM_FLOUR);
        hasSugar = SaveSystem.Instance.HasItem(ITEM_SUGAR);
        hasVanilla = SaveSystem.Instance.HasItem(ITEM_VANILLA);
        hasChocolate = SaveSystem.Instance.HasItem(ITEM_CHOCOLATE);
        hasEgg = SaveSystem.Instance.HasItem(ITEM_EGG);
        hasSalt = SaveSystem.Instance.HasItem(ITEM_SALT);

        return hasFlour && hasSugar && hasVanilla && hasChocolate && hasEgg && hasSalt;
    }
}