using UnityEngine;
using TMPro;
using System.Collections;

public class Room05_DiningRoomController : MonoBehaviour
{
    [Header("Puzzle Objects")]
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

    [Header("Table UI")]
    public GameObject tablePanel;
    public GameObject placedSpoonIcon;

    // State Checkers - DITO ANG FIX PARA SA ERROR
    private bool hasSpoon = false; // Ito ang nawawalang variable
    private bool isCabinetOpen = false;
    private bool isSpoonPlaced = false;
    private bool isCalendarSeen = false;

    void Start()
    {
        if (cabinetPanel != null) cabinetPanel.SetActive(false);
        if (tablePanel != null) tablePanel.SetActive(false);
        if (spoonPickup != null) spoonPickup.SetActive(false);
        if (bedroomKey != null) bedroomKey.SetActive(false);
        if (placedSpoonIcon != null) placedSpoonIcon.SetActive(false);
        if (emilyEnemy != null) emilyEnemy.SetActive(false);
    }

    // ==========================================================
    // 1. CALENDAR
    // ==========================================================
    public void OnCalendarInteract()
    {
        isCalendarSeen = true;
        TryShowDialogue("There are red marks on certain dates... It looks like a sequence. I should take note of this.");
    }

    // ==========================================================
    // 2. CABINET (Dialogue -> Wait -> UI)
    // ==========================================================
    public void OnCabinetInteract()
    {
        if (isCabinetOpen)
        {
            // Kung nakuha na ang spoon o nasa mesa na, "Empty" na dapat ito
            if (isSpoonPlaced || hasSpoon)
            {
                TryShowDialogue("It's empty ngayon.");
                if (spoonPickup != null) spoonPickup.SetActive(false);
                return;
            }

            if (spoonPickup != null) spoonPickup.SetActive(true);
            TryShowDialogue("The sideboard is open.");
            return;
        }

        if (!isCalendarSeen)
        {
            TryShowDialogue("It's locked tight with a 6-digit code. I should look around for a clue.");
        }
        else
        {
            StartCoroutine(ShowCabinetUISequence());
        }
    }

    IEnumerator ShowCabinetUISequence()
    {
        TryShowDialogue("A combination lock... Maybe the dates from the calendar will work here.");
        yield return new WaitForSeconds(2.0f);

        if (cabinetPanel != null)
        {
            cabinetPanel.SetActive(true);
            if (codeInput != null) codeInput.text = "";
            if (codeInput != null) codeInput.ActivateInputField();
        }
    }

    public void OnEnterPressed()
    {
        if (codeInput.text.Trim() == correctCode)
        {
            isCabinetOpen = true;
            if (sideboard != null) sideboard.GetComponent<SpriteRenderer>().color = Color.green;

            // FIX: Lalabas lang ang spoon kung wala pa kay Lisa at wala pa sa mesa
            if (spoonPickup != null && !hasSpoon && !isSpoonPlaced)
                spoonPickup.SetActive(true);

            CloseCabinetUI();
            TryShowDialogue("Click! It unlocked. A silver spoon is sitting inside.");
        }
        else
        {
            if (codeInput != null) codeInput.text = "";
            TryShowDialogue("The lock didn't move. Wrong code.");
        }
    }

    public void OnClearPressed() => codeInput.text = "";
    public void CloseCabinetUI() => cabinetPanel.SetActive(false);

    // ==========================================================
    // 3. TABLE & SPOON
    // ==========================================================
    public void OnSpoonInteract()
    {
        hasSpoon = true;
        if (spoonPickup != null) spoonPickup.SetActive(false);
        if (InventoryManager.Instance != null) InventoryManager.Instance.AddItem("spoon");

        TryShowDialogue("Got the silver spoon. Lisa earned this through good behavior.");
    }

    public void OnTableInteract()
    {
        if (isSpoonPlaced)
        {
            TryShowDialogue("The table is set properly. The floorboard is na-pried open.");
            return;
        }

        if (hasSpoon)
        {
            if (tablePanel != null) tablePanel.SetActive(true);
        }
        else
        {
            TryShowDialogue("I can't sit. The table setting is incomplete. Mother wouldn't be pleased.");
        }
    }

    public void OnPlaceSpoonConfirmed()
    {
        isSpoonPlaced = true;
        hasSpoon = false;

        // FORCE HIDE: Siguraduhing wala na ang spoon sa cabinet
        if (spoonPickup != null) spoonPickup.SetActive(false);

        if (InventoryManager.Instance != null)
            InventoryManager.Instance.RemoveItem("spoon");

        if (tablePanel != null) tablePanel.SetActive(false);
        if (placedSpoonIcon != null) placedSpoonIcon.SetActive(true);

        if (looseFloorboard != null && openFloorboardSprite != null)
        {
            looseFloorboard.GetComponent<SpriteRenderer>().sprite = openFloorboardSprite;
        }

        if (bedroomKey != null) bedroomKey.SetActive(true);

        TryShowDialogue("I placed the spoon... (CREAK)... Something moved under the floorboards.");
    }

    // ==========================================================
    // 4. FLOORBOARD & KEY
    // ==========================================================
    public void OnFloorboardInteract()
    {
        if (isSpoonPlaced)
        {
            if (bedroomKey != null) bedroomKey.SetActive(true);
            TryShowDialogue("The board is open. I can see a key glinting in the dark.");
        }
        else
        {
            TryShowDialogue("This floorboard is loose, but I can't lift it with my bare hands. I need a tool.");
        }
    }

    public void OnKeyInteract()
    {
        if (!isSpoonPlaced) return;

        if (bedroomKey != null) bedroomKey.SetActive(false);
        if (InventoryManager.Instance != null) InventoryManager.Instance.AddItem("bedroom_key");

        TryShowDialogue("Got the Bedroom Key. I need to get out of here before she finds me.");
        if (emilyEnemy != null) emilyEnemy.SetActive(true);
    }

    // ==========================================================
    // 5. THE ORIGINAL DIALOGUES (STRICTLY PRESERVED)
    // ==========================================================
    public void OnChairInteract() => TryShowDialogue("Someone tied a child to this chair... regularly.");
    public void OnMotherChairInteract() => TryShowDialogue("It was thrown backward violently, blood stains on the seat.");
    public void OnFatherChairInteract() => TryShowDialogue("Empty chair covered in dust shows he abandoned his family.");

    private void TryShowDialogue(string text)
    {
        if (DialogueSystemV2.Instance != null)
            DialogueSystemV2.Instance.StartDialogue(text, "Lisa");
    }
}