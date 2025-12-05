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

        // Ensure it starts closed
        if (panel != null) panel.SetActive(false);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseBook);
    }

    public void OpenBook()
    {
        if (panel != null)
        {
            panel.SetActive(true);

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

            // Optional: Play open sound
            // AudioManager.Instance?.PlaySFX(openSound);
        }
    }

    public void CloseBook()
    {
        if (panel != null) panel.SetActive(false);

        // Notify InventoryManager that action ended (re-opens inventory grid if needed)
        InventoryManager.Instance?.NotifyActionEnded();
    }
}