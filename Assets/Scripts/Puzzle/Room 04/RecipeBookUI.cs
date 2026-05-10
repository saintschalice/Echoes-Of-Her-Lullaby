using UnityEngine;
using UnityEngine.UI;

public class RecipeBookUI : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject panel; // Assign the parent panel holding the image/button
    public Image recipeImage; // The UI Image component to show the sprite
    public Button closeButton;

    [Header("Content")]
    public Sprite defaultRecipeSprite; // Assign your recipe sprite here

    public static RecipeBookUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // CRITICAL FIX: Ensure panel starts completely disabled
        if (panel != null)
        {
            panel.SetActive(false);
            
            // Also disable CanvasGroup if it exists to prevent blocking
            CanvasGroup panelCG = panel.GetComponent<CanvasGroup>();
            if (panelCG != null)
            {
                panelCG.alpha = 0f;
                panelCG.interactable = false;
                panelCG.blocksRaycasts = false;
            }
        }

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseBook);
    }

    public void OpenBook()
    {
        if (panel != null)
        {
            panel.SetActive(true);

            // CRITICAL: Enable CanvasGroup for interaction
            CanvasGroup panelCG = panel.GetComponent<CanvasGroup>();
            if (panelCG != null)
            {
                panelCG.alpha = 1f;
                panelCG.interactable = true;
                panelCG.blocksRaycasts = true;
            }

            // Set sprite if assigned
            if (recipeImage != null && defaultRecipeSprite != null)
            {
                recipeImage.sprite = defaultRecipeSprite;
            }

            // Notify Controller ONLY when opened
            if (KitchenRoomController.Instance != null)
            {
                KitchenRoomController.Instance.OnRecipeBookRead();
            }

            // Pause Emily AI
            EmilyGhost emilyAI = FindFirstObjectByType<EmilyGhost>();
            if (emilyAI != null) emilyAI.isPaused = true;

            // CRITICAL FIX: Disable player controls while viewing recipe
            JoystickPlayerController playerController = FindFirstObjectByType<JoystickPlayerController>();
            if (playerController != null) playerController.enabled = false;

            // Optional: Play open sound
            // AudioManager.Instance?.PlaySFX(openSound);
            
            Debug.Log("[RecipeBook] Recipe book opened");
        }
    }

    public void CloseBook()
    {
        // Safety check: don't close if already closed
        if (panel == null || !panel.activeSelf)
        {
            Debug.Log("[RecipeBook] Already closed, skipping");
            return;
        }

        panel.SetActive(false);

        // CRITICAL: Disable CanvasGroup to stop blocking raycasts
        CanvasGroup panelCG = panel.GetComponent<CanvasGroup>();
        if (panelCG != null)
        {
            panelCG.alpha = 0f;
            panelCG.interactable = false;
            panelCG.blocksRaycasts = false;
            Debug.Log("[RecipeBook] CanvasGroup disabled - no longer blocking");
        }

        // Resume Emily AI
        EmilyGhost emilyAI = FindFirstObjectByType<EmilyGhost>();
        if (emilyAI != null)
        {
            emilyAI.isPaused = false;
            Debug.Log("[RecipeBook] Emily AI resumed");
        }

        // CRITICAL FIX: Re-enable player controls
        JoystickPlayerController playerController = FindFirstObjectByType<JoystickPlayerController>();
        if (playerController != null)
        {
            playerController.enabled = true;
            Debug.Log("[RecipeBook] Player controls re-enabled");
        }

        // CRITICAL: Notify InventoryManager that action ended
        // This will reopen the inventory if it was open before
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.NotifyActionEnded();
            Debug.Log("[RecipeBook] Notified InventoryManager - inventory should reopen");
        }
        else
        {
            Debug.LogWarning("[RecipeBook] InventoryManager.Instance is NULL!");
        }
        
        Debug.Log("[RecipeBook] Recipe book closed successfully");
    }

    void Update()
    {
        // CRITICAL FIX: Allow closing with tap anywhere or ESC key
        if (panel != null && panel.activeSelf)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape))
            {
                CloseBook();
            }
        }
    }
}