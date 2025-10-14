using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPickup : MonoBehaviour
{
    [Header("Item Configuration")]
    public string itemId;
    [TextArea(2, 3)]
    public string pickupMessage = "You found {itemName}!";

    [Header("Visual Settings")]
    public SpriteRenderer itemSprite;
    public bool hideAfterPickup = true;
    public bool playPickupAnimation = true;

    [Header("Interaction")]
    public KeyCode interactKey = KeyCode.E;
    public float interactionRange = 2f;
    public LayerMask playerLayerMask = -1;

    [Header("UI Feedback")]
    public GameObject interactionPrompt;
    public TextMeshProUGUI promptText;

    [Header("Animation")]
    public float bobAmount = 0.5f;
    public float bobSpeed = 2f;
    public float rotationSpeed = 30f;

    [Header("Audio")]
    public AudioClip pickupSound;

    private bool isPickedUp = false;
    private bool playerInRange = false;
    private Transform playerTransform;
    private Vector3 startPosition;
    private InventoryItem itemData;
    private string uniquePickupId; // FIXED: Unique ID for this specific pickup instance

    void Start()
    {
        startPosition = transform.position;

        // FIXED: Create unique ID using scene name + position + item ID
        uniquePickupId = $"{UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}_pickup_{itemId}_{transform.position.x:F2}_{transform.position.y:F2}_{transform.position.z:F2}";

        // Get item data from database
        if (InventoryManager.Instance?.itemDatabase != null)
        {
            itemData = InventoryManager.Instance.itemDatabase.GetItem(itemId);
            if (itemData == null)
            {
                Debug.LogWarning($"Item not found in database: {itemId} on {gameObject.name}");
            }
            else
            {
                // Update sprite if not manually set
                if (itemSprite != null && itemSprite.sprite == null && itemData.itemIcon != null)
                {
                    itemSprite.sprite = itemData.itemIcon;
                }
            }
        }

        // Setup interaction prompt
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }

        // FIXED: Check if THIS SPECIFIC pickup was already collected
        CheckPickupStatus();
    }

    void CheckPickupStatus()
    {
        if (SaveSystem.Instance != null)
        {
            // FIXED: Check if this specific pickup instance was examined (picked up)
            if (SaveSystem.Instance.WasObjectExamined(uniquePickupId))
            {
                // This specific item instance was already picked up
                if (hideAfterPickup)
                {
                    gameObject.SetActive(false);
                }
                isPickedUp = true;
                Debug.Log($"Item {itemId} at {transform.position} was already picked up");
            }
        }
    }

    void Update()
    {
        if (isPickedUp) return;

        // Animate item
        AnimateItem();

        // Check for player in range
        CheckPlayerProximity();

        // Handle interaction
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            PickupItem();
        }
    }

    void AnimateItem()
    {
        if (!playPickupAnimation) return;

        // Bob up and down
        float bobOffset = Mathf.Sin(Time.time * bobSpeed) * bobAmount;
        transform.position = startPosition + Vector3.up * bobOffset;

        // Rotate slowly
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    void CheckPlayerProximity()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                return;
            }
        }

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        bool wasInRange = playerInRange;
        playerInRange = distance <= interactionRange;

        // Show/hide interaction prompt
        if (playerInRange != wasInRange)
        {
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(playerInRange);

                if (playerInRange && promptText != null && itemData != null)
                {
                    promptText.text = $"Press {interactKey} to pick up {itemData.itemName}";
                }
            }
        }
    }

    public void PickupItem()
    {
        if (isPickedUp || InventoryManager.Instance == null) return;

        // FIXED: Add to inventory (this adds the item TYPE to inventory)
        bool success = InventoryManager.Instance.AddItem(itemId);

        if (success)
        {
            isPickedUp = true;

            // FIXED: Mark THIS SPECIFIC pickup instance as collected
            if (SaveSystem.Instance != null)
            {
                SaveSystem.Instance.MarkObjectExamined(uniquePickupId);
                Debug.Log($"Marked pickup as collected: {uniquePickupId}");
            }

            // Play pickup sound
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            // Show pickup message
            ShowPickupMessage();

            // Hide interaction prompt
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }

            // Pickup animation
            if (playPickupAnimation)
            {
                StartCoroutine(PickupAnimation());
            }
            else if (hideAfterPickup)
            {
                gameObject.SetActive(false);
            }

            Debug.Log($"Picked up: {itemData?.itemName ?? itemId}");
        }
    }

    void ShowPickupMessage()
    {
        if (itemData == null) return;

        string message = pickupMessage.Replace("{itemName}", itemData.itemName);

        // Show through dialogue system if available
        DialogueSystemV2 dialogueSystem = FindFirstObjectByType<DialogueSystemV2>();
        if (dialogueSystem != null)
        {
            dialogueSystem.StartDialogue(message, "System");
        }
        else
        {
            Debug.Log(message);
        }
    }

    System.Collections.IEnumerator PickupAnimation()
    {
        // Animate item flying up and scaling down
        Vector3 originalScale = transform.localScale;
        Vector3 targetPosition = transform.position + Vector3.up * 2f;

        float animationTime = 0.5f;
        float elapsed = 0f;

        while (elapsed < animationTime)
        {
            float progress = elapsed / animationTime;

            // Move up
            transform.position = Vector3.Lerp(startPosition, targetPosition, progress);

            // Scale down
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, progress);

            // Fade out if has sprite renderer
            if (itemSprite != null)
            {
                Color color = itemSprite.color;
                color.a = 1f - progress;
                itemSprite.color = color;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (hideAfterPickup)
        {
            gameObject.SetActive(false);
        }
    }

    // Manual pickup for triggered events
    public void ForcePickup()
    {
        PickupItem();
    }

    // Check if this item can be picked up
    public bool CanPickup()
    {
        return !isPickedUp && InventoryManager.Instance != null;
    }

    void OnDrawGizmosSelected()
    {
        // Draw interaction range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRange);

        // Draw connection to player if in range
        if (playerInRange && playerTransform != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, playerTransform.position);
        }
    }

    // For puzzle systems or other scripts
    public string GetItemId() => itemId;
    public InventoryItem GetItemData() => itemData;
    public bool IsPickedUp() => isPickedUp;
    public string GetUniquePickupId() => uniquePickupId; // NEW: Expose unique ID
}