using UnityEngine;
using System.Collections;

/// <summary>
/// Evidence pickup for Room 08 - Lisa's Bathroom
/// Shows notification when picked up and tracks collection progress
/// </summary>
public class Room08_EvidencePickup : MonoBehaviour
{
    [Header("Evidence Settings")]
    [Tooltip("Unique ID for this evidence (e.g., 'evidence_1', 'evidence_2')")]
    public string evidenceId = "evidence_1";
    
    [Tooltip("Display name shown in notification")]
    public string evidenceName = "Evidence";
    
    [Tooltip("Description shown in notification")]
    public string evidenceDescription = "A piece of evidence found in the bathroom.";
    
    [Header("Pickup Settings")]
    [Tooltip("Automatically pickup on trigger enter (no button needed)")]
    public bool autoPickup = true;
    
    [Tooltip("Pickup sound")]
    public AudioClip pickupSound;
    
    [Header("Visual Feedback")]
    [Tooltip("Particle effect when picked up")]
    public GameObject pickupEffect;
    
    [Header("Debug")]
    public bool debugMode = true;

    private bool hasBeenPickedUp = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasBeenPickedUp) return;
        
        if (other.CompareTag("Player") && autoPickup)
        {
            PickupEvidence();
        }
    }

    // Can also be called by interact button
    public void PickupEvidence()
    {
        if (hasBeenPickedUp) return;
        
        hasBeenPickedUp = true;
        
        if (debugMode) Debug.Log($"[Room08] Evidence picked up: {evidenceId}");
        
        StartCoroutine(PickupSequence());
    }

    IEnumerator PickupSequence()
    {
        // Play pickup sound
        if (pickupSound != null)
        {
            AudioManager.Instance?.PlaySFX(pickupSound);
        }
        
        // Show pickup effect
        if (pickupEffect != null)
        {
            GameObject effect = Instantiate(pickupEffect, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }
        
        // Hide this object immediately
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false;
        
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        
        // Show notification
        if (ItemNotificationUI.Instance != null)
        {
            ItemNotificationUI.Instance.ShowItemNotification(evidenceName, evidenceDescription);
            
            // Wait for notification to be dismissed
            while (ItemNotificationUI.Instance.IsShowing())
            {
                yield return null;
            }
        }
        else
        {
            // Fallback: Show dialogue
            DialogueSystemV2.Instance?.StartDialogue($"Found: {evidenceName}. {evidenceDescription}", "Lisa");
            
            while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
            {
                yield return null;
            }
        }
        
        yield return new WaitForSeconds(0.3f);
        
        // Mark as collected in flow controller
        Room08_FlowController flow = Room08_FlowController.Instance;
        if (flow != null)
        {
            flow.OnEvidenceCollected(evidenceId);
        }
        
        // Destroy this object
        Destroy(gameObject);
    }
}
