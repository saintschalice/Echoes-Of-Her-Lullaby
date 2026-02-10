using UnityEngine;

public class Room05_DiningRoomController : MonoBehaviour
{
    [Header("Puzzle Settings")]
    public string correctCode = "332412";
    public GameObject sideboard;
    public GameObject spoonPickup;
    public GameObject bedroomKey;
    public GameObject emilyEnemy;   // Yung Red Square Monster

    public bool hasSpoon = false;

    [Header("UI Reference")]
    public GameObject calendarUI;

    // ==========================================================
    // 1. CALENDAR PUZZLE LOGIC
    // ==========================================================
    public void SolveCalendar(string input)
    {
        if (input.Trim() == correctCode)
        {
            if (sideboard != null) sideboard.GetComponent<SpriteRenderer>().color = Color.green;
            if (spoonPickup != null) spoonPickup.SetActive(true);
            if (calendarUI != null) calendarUI.SetActive(false);

            TryShowDialogue("The sideboard clicked open. There is a spoon inside.");
        }
        else
        {
            TryShowDialogue("Nothing happened. Wrong code.");
        }
    }

    public void OnCalendarInteract()
    {
        TryShowDialogue("Every day is marked with a red sad face. I need a code.");
        if (calendarUI != null) calendarUI.SetActive(true);
    }

    // ==========================================================
    // 2. ITEM INTERACTIONS
    // ==========================================================
    public void OnSpoonInteract()
    {
        hasSpoon = true;
        if (spoonPickup != null) spoonPickup.SetActive(false);
        TryShowDialogue("Lisa earned her spoon through good behavior.");
    }

    public void OnTableInteract()
    {
        if (hasSpoon)
        {
            if (bedroomKey != null) bedroomKey.SetActive(true);
            TryShowDialogue("I tried to eat properly... A loose floorboard nearby creaked open!");
        }
        else
        {
            TryShowDialogue("I can't sit here yet. I need my spoon to be a 'good girl'.");
        }
    }

    public void OnKeyInteract()
    {
        if (bedroomKey != null) bedroomKey.SetActive(false);
        TryShowDialogue("I got the Bedroom Key! But... someone is coming!");

        // TRIGGER EMILY RAGE
        if (emilyEnemy != null)
        {
            Debug.Log("⚠️ EMILY IS RAGING! RUN!");
            emilyEnemy.SetActive(true);
        }
    }

    // ==========================================================
    // 3. FLAVOR TEXT (Environment & Chairs)
    // ==========================================================

    // CHILD CHAIR (Yung dati mong Chair logic)
    public void OnChairInteract()
    {
        TryShowDialogue("Someone tied a child to this chair... regularly. There are rope marks.");
    }

    // MOTHER CHAIR (New!)
    public void OnMotherChairInteract()
    {
        TryShowDialogue("It was thrown backward violently. Are those... blood stains on the seat?");
    }

    // FATHER CHAIR (New!)
    public void OnFatherChairInteract()
    {
        TryShowDialogue("Empty and covered in dust. He abandoned his family when they needed him most.");
    }

    public void OnCabinetInteract()
    {
        TryShowDialogue(hasSpoon ? "It's empty now." : "It's locked. I need a code.");
    }

    // Helper
    private void TryShowDialogue(string text)
    {
        if (DialogueSystemV2.Instance != null)
            DialogueSystemV2.Instance.StartDialogue(text, "Lisa");
        else
            Debug.LogError("Dialogue System Missing!");
    }
}