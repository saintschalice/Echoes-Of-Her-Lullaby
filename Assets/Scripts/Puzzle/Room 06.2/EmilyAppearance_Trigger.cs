using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EmilyAppearanceTrigger : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Titingnan natin kung may nade-detect ba talaga yung sahig
        Debug.Log("[Emily Trigger] May tumapak sa trigger: " + other.name); 

        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("[Emily Trigger] Player ang tumapak!"); 

            // 2. Iche-check natin kung na-save ba yung "R06_PhotoInteracted" nung chineck mo yung picture
            int photoStatus = PlayerPrefs.GetInt("R06_PhotoInteracted", 0);
            Debug.Log("[Emily Trigger] Photo Status sa PlayerPrefs ay: " + photoStatus); 

            if (photoStatus == 1)
            {
                hasTriggered = true;
                Debug.Log("[Emily Trigger] Ready na palabasin si Emily!"); 

                if (Room06_HallwayController.Instance != null)
                {
                    Room06_HallwayController.Instance.TriggerEmilyChase();
                    Debug.Log("[Emily Trigger] SUCCESS! Tinawag na ang Hallway Controller."); 
                }
                else
                {
                    Debug.LogError("[Emily Trigger] ERROR: Hindi mahanap ang Room06_HallwayController sa scene!");
                }
            }
            else
            {
                Debug.LogWarning("[Emily Trigger] Hindi lumabas si Emily dahil ang Photo Status ay hindi 1. Natingnan na ba ang picture?");
            }
        }
    }
}