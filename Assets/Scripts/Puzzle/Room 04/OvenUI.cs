using UnityEngine;
using UnityEngine.UI;
using TMPro; // Assuming TextMeshPro for modern UI

public class OvenUI : MonoBehaviour
{
    public static OvenUI Instance { get; private set; }

    [Header("UI Components")]
    public GameObject panel;
    public TMP_Dropdown tempDropdown; // Options: 300, 325, 350, 400
    public TMP_Dropdown timeDropdown; // Options: 8, 10, 12, 15
    public Button confirmButton;
    public Button closeButton;
    public TextMeshProUGUI feedbackText;

    [Header("Correct Settings")]
    public int correctTempIndex = 2; // Assuming 0=300, 1=325, 2=350, 3=400
    public int correctTimeIndex = 2; // Assuming 0=8, 1=10, 2=12, 3=15

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (panel != null) panel.SetActive(false);

        confirmButton.onClick.AddListener(OnConfirmPressed);
        if (closeButton != null) closeButton.onClick.AddListener(CloseUI);
    }

    public void OpenUI()
    {
        if (panel != null)
        {
            panel.SetActive(true);
            feedbackText.text = "Set temperature and time...";

            // Notify Inventory/Manager to pause or hide other UI
            InventoryManager.Instance?.NotifyActionStarted();

            // Pause Emily AI
            EmilyGhost emilyAI = FindFirstObjectByType<EmilyGhost>();
            if (emilyAI != null) emilyAI.isPaused = true;
        }
    }

    public void CloseUI()
    {
        if (panel != null) panel.SetActive(false);

        // Resume Emily AI
        EmilyGhost emilyAI = FindFirstObjectByType<EmilyGhost>();
        if (emilyAI != null) emilyAI.isPaused = false;

        InventoryManager.Instance?.NotifyActionEnded();
    }

    void OnConfirmPressed()
    {
        if (tempDropdown.value == correctTempIndex && timeDropdown.value == correctTimeIndex)
        {
            // Correct!
            feedbackText.text = "That should be right.";

            if (KitchenRoomController.Instance != null)
            {
                KitchenRoomController.Instance.OnOvenSetCorrect();
            }

            if (DialogueSystemV2.Instance != null)
            {
                DialogueSystemV2.Instance.StartDialogue("That feels like the right temperature.", "Lisa");
            }

            Invoke(nameof(CloseUI), 1.5f);
        }
        else
        {
            // Incorrect
            feedbackText.text = "That doesn't feel right...";

            if (DialogueSystemV2.Instance != null)
            {
                DialogueSystemV2.Instance.StartDialogue("That doesn't seem correct for cookies.", "Lisa");
            }
        }
    }
}