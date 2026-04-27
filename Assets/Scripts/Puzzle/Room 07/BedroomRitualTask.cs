using UnityEngine;

public class BedroomRitualTask : MonoBehaviour, IInteractable
{
    public enum TaskType { Window, TeaSet, Dollhouse }
    public TaskType myTask;
    public string requiredItemID;
    private bool isDone = false;

    public void Interact() { AttemptTask(); }
    public void OnInteract(PlayerContext context) { AttemptTask(); }
    public void OnFocus(PlayerContext context) { }
    public void OnBlur(PlayerContext context) { }

    void AttemptTask()
    {
        if (isDone) return;

        // Window doesn't need an item
        if (myTask == TaskType.Window)
        {
            CompleteTask("I tied the curtains tightly. She says it keeps bad things out.");
            return;
        }

        // TeaSet and Dollhouse need items from Inventory
        if (InventoryManager.Instance != null && InventoryManager.Instance.HasItem(requiredItemID))
        {
            InventoryManager.Instance.RemoveItem(requiredItemID);
            CompleteTask("I placed it exactly where it belongs.");
        }
        else
        {
            DialogueSystemV2.Instance?.StartDialogue("Something is missing here...", "Lisa");
        }
    }

    void CompleteTask(string dialogueMessage)
    {
        isDone = true;
        DialogueSystemV2.Instance?.StartDialogue(dialogueMessage, "Lisa");

        if (myTask == TaskType.Window) Room07_BedroomController.Instance.isWindowTied = true;
        if (myTask == TaskType.TeaSet) Room07_BedroomController.Instance.isTeaSetPlaced = true;
        if (myTask == TaskType.Dollhouse) Room07_BedroomController.Instance.isDollPlaced = true;

        Room07_BedroomController.Instance.CheckPuzzleProgress();
    }
}