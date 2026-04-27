using UnityEngine;

public class MirrorJumpscare : MonoBehaviour, IInteractable
{
    public GameObject emilyEnemy;
    public UnifiedDoorInteraction bathroomDoor;

    public void Interact() { TriggerChase(); }
    public void OnInteract(PlayerContext context) { TriggerChase(); }
    public void OnFocus(PlayerContext context) { }
    public void OnBlur(PlayerContext context) { }

    void TriggerChase()
    {
        Debug.Log("JUMPSCARE! Escape to the bathroom!");

        // Unlock bathroom door instantly
        if (bathroomDoor != null)
        {
            bathroomDoor.UnlockDoor();
        }

        // Spawn Emily
        if (emilyEnemy != null)
        {
            emilyEnemy.transform.position = transform.position;
            emilyEnemy.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}