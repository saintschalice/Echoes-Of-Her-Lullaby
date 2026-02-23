using UnityEngine;

public class DiningRoom_Interact : MonoBehaviour
{
    private DiningRoom_Narrative narrative;
    private DiningRoomPuzzle puzzle; // Ang puzzle script na gagawin natin sa baba

    [TextArea] public string horrorLine;
    public bool isSpoon = false;
    public bool isTable = false;
    public bool isCalendar = false;

    void Start()
    {
        narrative = Object.FindFirstObjectByType<DiningRoom_Narrative>();
        puzzle = Object.FindFirstObjectByType<DiningRoomPuzzle>();
    }

    // Eto ang method na hinahanap ng iyong PlayerInteractionTracker via Reflection
    public void Interact()
    {
        if (narrative != null) narrative.ShowMessage(horrorLine);

        // Logic para sa puzzle progression
        if (isSpoon && puzzle != null) puzzle.PickUpSpoon();
        if (isTable && puzzle != null) puzzle.OpenFloorboard();
        if (isCalendar)
        {
            // Dito natin pwedeng palabasin ang Mobile Keyboard input box
            Debug.Log("Opening Calendar Input...");
        }
    }
}