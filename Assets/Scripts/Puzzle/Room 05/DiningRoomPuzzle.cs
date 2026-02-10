using UnityEngine;

public class DiningRoomPuzzle : MonoBehaviour
{
    public string correctCode = "332412";
    public GameObject sideboard; // Blue square
    public GameObject spoonPickup; // Puting square
    public GameObject bedroomKey; // Yellow square
    public bool hasSpoon = false;

    public void SolveCalendar(string input)
    {
        if (input == correctCode)
        {
            if (sideboard != null) sideboard.GetComponent<SpriteRenderer>().color = Color.green;
            if (spoonPickup != null) spoonPickup.SetActive(true);
        }
    }

    public void PickUpSpoon()
    {
        hasSpoon = true;
        if (spoonPickup != null) spoonPickup.SetActive(false);
    }

    public void OpenFloorboard()
    {
        if (hasSpoon && bedroomKey != null)
        {
            bedroomKey.SetActive(true);
            Debug.Log("A floorboard creaks open. The key is revealed.");
        }
    }
}