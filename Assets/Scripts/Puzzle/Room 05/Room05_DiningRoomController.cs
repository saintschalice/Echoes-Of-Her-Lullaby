using UnityEngine;
using TMPro;
using System.Collections;

public class Room05_DiningRoomController : MonoBehaviour
{
    public static Room05_DiningRoomController Instance { get; private set; }

    [Header("Puzzle Configuration")]
    public string correctCode = "332412";
    public GameObject sideboard;
    public GameObject spoonPickup;
    public GameObject bedroomKey;
    public GameObject looseFloorboard;
    public Sprite openFloorboardSprite;
    public GameObject emilyEnemy;

    [Header("Cabinet UI")]
    public GameObject cabinetPanel;
    public TMP_InputField codeInput;

    [Header("Table & Ritual")]
    public GameObject tablePanel;
    public GameObject placedSpoonIcon;
    public GameObject ghostlyCutlery;

    // PERSISTENT FLAGS (Naka-save sa PC)
    public bool isCalendarSeen { get { return PlayerPrefs.GetInt("R05_Calendar", 0) == 1; } set { PlayerPrefs.SetInt("R05_Calendar", value ? 1 : 0); } }
    public bool isCabinetOpen { get { return PlayerPrefs.GetInt("R05_Cabinet", 0) == 1; } set { PlayerPrefs.SetInt("R05_Cabinet", value ? 1 : 0); } }
    public bool hasSpoon { get { return PlayerPrefs.GetInt("R05_HasSpoon", 0) == 1; } set { PlayerPrefs.SetInt("R05_HasSpoon", value ? 1 : 0); } }
    public bool isSpoonPlaced { get { return PlayerPrefs.GetInt("R05_SpoonPlaced", 0) == 1; } set { PlayerPrefs.SetInt("R05_SpoonPlaced", value ? 1 : 0); } }
    public int chairsFixed { get { return PlayerPrefs.GetInt("R05_Chairs", 0); } set { PlayerPrefs.SetInt("R05_Chairs", value); } }

    private void Awake() { if (Instance == null) Instance = this; }

    void Start()
    {
        if (cabinetPanel != null) cabinetPanel.SetActive(false);
        if (tablePanel != null) tablePanel.SetActive(false);
        if (emilyEnemy != null) emilyEnemy.SetActive(false);

        SyncRoomState();
    }

    private void SyncRoomState()
    {
        // Spoon visibility
        if (spoonPickup != null) spoonPickup.SetActive(isCabinetOpen && !hasSpoon && !isSpoonPlaced);

        // Cabinet color/state
        if (sideboard != null && isCabinetOpen) sideboard.GetComponent<SpriteRenderer>().color = Color.green;

        // Ritual Progress
        if (ghostlyCutlery != null) ghostlyCutlery.SetActive(chairsFixed >= 3);
        if (placedSpoonIcon != null) placedSpoonIcon.SetActive(isSpoonPlaced);

        // Key & Floorboard
        if (looseFloorboard != null && isSpoonPlaced && openFloorboardSprite != null)
            looseFloorboard.GetComponent<SpriteRenderer>().sprite = openFloorboardSprite;

        // I-check sa inventory manager kung nakuha na talaga ang susi
        bool alreadyHasKey = (InventoryManager.Instance != null) && InventoryManager.Instance.HasItem("bedroom_key");
        if (bedroomKey != null) bedroomKey.SetActive(isSpoonPlaced && !alreadyHasKey);
    }

    // ==========================================================
    // 1. CALENDAR & CABINET
    // ==========================================================
    public void OnCalendarInteract()
    {
        isCalendarSeen = true; PlayerPrefs.Save();
        TryShowDialogue("There are red marks on certain dates... It looks like a sequence.");
    }

    public void OnCabinetInteract()
    {
        if (isCabinetOpen) { TryShowDialogue(hasSpoon || isSpoonPlaced ? "It's empty." : "The sideboard is open."); return; }
        if (!isCalendarSeen) TryShowDialogue("It's locked tight. I need a clue.");
        else StartCoroutine(ShowCabinetUISequence());
    }

    IEnumerator ShowCabinetUISequence()
    {
        TryShowDialogue("Maybe the calendar dates work here...");
        yield return new WaitForSeconds(1.5f);
        if (cabinetPanel != null) { cabinetPanel.SetActive(true); codeInput.text = ""; codeInput.ActivateInputField(); }
    }

    public void OnEnterPressed()
    {
        if (codeInput.text.Trim() == correctCode)
        {
            isCabinetOpen = true;
            PlayerPrefs.Save();

            // 1. Force Sync the whole room
            SyncRoomState();

            // 2. EMERGENCY OVERRIDE: Siguraduhin nating mag-check ang object sa Hierarchy
            if (spoonPickup != null)
            {
                spoonPickup.SetActive(true);
                Debug.Log("[DEBUG] Spoon_Pickup is now FORCED to Active: " + spoonPickup.activeSelf);
            }

            CloseCabinetUI();
            TryShowDialogue("Click! It unlocked. A silver spoon is inside.");
        }
        else
        {
            codeInput.text = "";
            TryShowDialogue("Wrong code.");
        }
    }
    public void CloseCabinetUI() => cabinetPanel.SetActive(false);

    // ==========================================================
    // 2. SPOON & TABLE
    // ==========================================================
    public void OnSpoonInteract()
    {
        hasSpoon = true; PlayerPrefs.Save();
        if (spoonPickup != null) spoonPickup.SetActive(false);
        if (InventoryManager.Instance != null) InventoryManager.Instance.AddItem("spoon");
        TryShowDialogue("Got the silver spoon.");
    }

    public void OnTableInteract()
    {
        if (isSpoonPlaced) { TryShowDialogue("The table is set properly."); return; }
        if (chairsFixed < 3) { TryShowDialogue("The table setting is incomplete. Mother wouldn't be pleased."); return; }

        bool hasSpoonInInventory = (InventoryManager.Instance != null) && InventoryManager.Instance.HasItem("spoon");
        if (hasSpoon || hasSpoonInInventory) tablePanel.SetActive(true);
        else TryShowDialogue("It's missing a spoon.");
    }

    public void OnPlaceSpoonConfirmed()
    {
        isSpoonPlaced = true; hasSpoon = false; PlayerPrefs.Save();
        if (InventoryManager.Instance != null) InventoryManager.Instance.RemoveItem("spoon");
        tablePanel.SetActive(false); SyncRoomState();
        TryShowDialogue("I placed the spoon... Something moved under the floor boards.");
    }

    // ==========================================================
    // 3. KEY & CHAIRS (With Bridge Methods)
    // ==========================================================

    // Ito ang tinitirahan ng logic para sa lahat ng upuan
    public void FixChair(string type)
    {
        // Guard clause para sa dialogue lang kung tapos na
        if (chairsFixed >= 3 && PlayerPrefs.GetInt("R05_Chairs", 0) >= 3)
        {
            switch (type)
            {
                case "Child": TryShowDialogue("This chair is cold as ice."); break;
                case "Mother": TryShowDialogue("The blood here is still wet."); break;
                case "Father": TryShowDialogue("The dust on this chair is thick."); break;
            }
            return;
        }

        chairsFixed++; PlayerPrefs.Save();
        switch (type)
        {
            case "Child": TryShowDialogue("Someone tied a child to this chair... regularly."); break;
            case "Mother": TryShowDialogue("It was thrown backward violently."); break;
            case "Father": TryShowDialogue("Empty chair covered in dust."); break;
        }

        if (chairsFixed >= 3) { SyncRoomState(); TryShowDialogue("The table... it's manifesting cutlery."); }
    }

    public void OnChairInteract() => FixChair("Child");
    public void OnMotherChairInteract() => FixChair("Mother");
    public void OnFatherChairInteract() => FixChair("Father");

    public void OnFloorboardInteract()
    {
        if (isSpoonPlaced) TryShowDialogue("The board is open. I can see a key.");
        else TryShowDialogue("It's loose, but I can't lift it.");
    }

    public void OnKeyInteract()
    {
        // Mas maluwag na pickup logic
        if (bedroomKey != null) bedroomKey.SetActive(false);
        if (InventoryManager.Instance != null) InventoryManager.Instance.AddItem("bedroom_key");
        TryShowDialogue("Got the Bedroom Key. Time to go.");
        if (emilyEnemy != null) emilyEnemy.SetActive(true);
    }

    private void TryShowDialogue(string text) { if (DialogueSystemV2.Instance != null) DialogueSystemV2.Instance.StartDialogue(text, "Lisa"); }

    // ==========================================================
    // DEBUG TOOLS
    // ==========================================================
    [ContextMenu("Reset Room 05 Puzzle")]
    public void ResetPuzzle()
    {
        PlayerPrefs.DeleteKey("R05_Calendar");
        PlayerPrefs.DeleteKey("R05_Cabinet");
        PlayerPrefs.DeleteKey("R05_HasSpoon");
        PlayerPrefs.DeleteKey("R05_SpoonPlaced");
        PlayerPrefs.DeleteKey("R05_Chairs");
        PlayerPrefs.Save();
        Debug.Log("Room 05 Puzzle Reset!");
    }
}