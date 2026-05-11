using UnityEngine;

/// <summary>
/// Handles interactions with mirrors in Room 09 - Master Bedroom's Bathroom
/// Uses IInteractable interface to work with PlayerInteractionController
/// Same pattern as Room07_Interactable and Room08_Interactable
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Room09_Interactable : MonoBehaviour, IInteractable
{
    [Header("Mirror Settings")]
    public int mirrorNumber; // 1, 2, 3, or 4
    
    [Header("Interaction Settings")]
    public float interactionRadius = 2.5f;
    public bool debugRadius = true;
    
    private bool puzzleCompleted = false;
    
    // References to puzzle scripts
    private Mirror1_MedicineCabinet mirror1;
    private Mirror2_BathtubDrain mirror2;
    private Mirror3_VanityTerror mirror3;
    private Mirror4_EvidenceSequence mirror4;

    private void Start()
    {
        // Ensure collider is trigger
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.isTrigger = true;
        }
        
        // Get references based on mirror number
        switch (mirrorNumber)
        {
            case 1:
                mirror1 = GetComponent<Mirror1_MedicineCabinet>();
                if (mirror1 == null)
                {
                    Debug.LogError($"[Room09] Mirror {mirrorNumber} missing Mirror1_MedicineCabinet component!");
                }
                break;
            case 2:
                mirror2 = GetComponent<Mirror2_BathtubDrain>();
                if (mirror2 == null)
                {
                    Debug.LogError($"[Room09] Mirror {mirrorNumber} missing Mirror2_BathtubDrain component!");
                }
                break;
            case 3:
                mirror3 = GetComponent<Mirror3_VanityTerror>();
                if (mirror3 == null)
                {
                    Debug.LogError($"[Room09] Mirror {mirrorNumber} missing Mirror3_VanityTerror component!");
                }
                break;
            case 4:
                mirror4 = GetComponent<Mirror4_EvidenceSequence>();
                if (mirror4 == null)
                {
                    Debug.LogError($"[Room09] Mirror {mirrorNumber} missing Mirror4_EvidenceSequence component!");
                }
                break;
            default:
                Debug.LogError($"[Room09] Invalid mirror number: {mirrorNumber}. Must be 1-4!");
                break;
        }
    }

    // Main interaction method - called by mobile button or keyboard
    public void Interact()
    {
        DoInteract();
    }

    // Core interaction logic
    private void DoInteract()
    {
        if (puzzleCompleted)
        {
            Debug.Log($"[Room09] Mirror {mirrorNumber} puzzle already completed");
            DialogueSystemV2.Instance?.StartDialogue("I've already solved this mirror's puzzle.", "Lisa");
            return;
        }
        
        Debug.Log($"[Room09] ⭐ Interacting with Mirror {mirrorNumber}");
        
        // Start the appropriate puzzle
        switch (mirrorNumber)
        {
            case 1:
                if (mirror1 != null)
                {
                    mirror1.StartPuzzle();
                }
                else
                {
                    Debug.LogError("[Room09] Mirror1_MedicineCabinet component not found!");
                }
                break;
                
            case 2:
                if (mirror2 != null)
                {
                    mirror2.StartPuzzle();
                }
                else
                {
                    Debug.LogError("[Room09] Mirror2_BathtubDrain component not found!");
                }
                break;
                
            case 3:
                if (mirror3 != null)
                {
                    mirror3.StartPuzzle();
                }
                else
                {
                    Debug.LogError("[Room09] Mirror3_VanityTerror component not found!");
                }
                break;
                
            case 4:
                if (mirror4 != null)
                {
                    mirror4.StartPuzzle();
                }
                else
                {
                    Debug.LogError("[Room09] Mirror4_EvidenceSequence component not found!");
                }
                break;
        }
    }

    // IInteractable implementation - called by PlayerInteractionController
    public void OnInteract(PlayerContext context)
    {
        Interact();
    }

    // IInteractable implementation - called when player focuses on this object
    public void OnFocus(PlayerContext context)
    {
        Debug.Log($"[Room09] ⭐ Focused on Mirror {mirrorNumber}");
        // Optional: Show highlight or prompt
    }

    // IInteractable implementation - called when player stops focusing on this object
    public void OnBlur(PlayerContext context)
    {
        Debug.Log($"[Room09] ❌ Blurred from Mirror {mirrorNumber}");
        // Optional: Hide highlight or prompt
    }

    public void MarkAsCompleted()
    {
        puzzleCompleted = true;
        Debug.Log($"[Room09] ✅ Mirror {mirrorNumber} marked as completed");
    }
    
    // Visualization for the Editor
    private void OnDrawGizmosSelected()
    {
        if (debugRadius)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, interactionRadius);
        }
    }
}


