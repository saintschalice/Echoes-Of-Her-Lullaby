using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveSlotUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI saveNameText;
    public TextMeshProUGUI saveTimeText;
    public TextMeshProUGUI playtimeText;
    public TextMeshProUGUI chapterText;
    public Button slotButton;
    public Button deleteButton;
    public GameObject emptySlotIndicator;
    public GameObject filledSlotIndicator;

    private int slotIndex;
    private bool isForSaving;
    private SaveUIManager uiManager;

    public int SlotIndex => slotIndex;

    public void Initialize(int index, bool canSave, SaveUIManager manager)
    {
        slotIndex = index;
        isForSaving = canSave; // For regular slots, this determines if they can save or only load
        uiManager = manager;

        if (slotButton != null)
        {
            slotButton.onClick.AddListener(OnSlotClicked);
        }

        if (deleteButton != null)
        {
            deleteButton.onClick.AddListener(OnDeleteClicked);
            // Hide delete button for AutoSave slot
            if (slotIndex == 0)
            {
                deleteButton.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateSlotInfo(GameSaveData saveData)
    {
        bool hasData = saveData != null;

        if (emptySlotIndicator != null)
            emptySlotIndicator.SetActive(!hasData);

        if (filledSlotIndicator != null)
            filledSlotIndicator.SetActive(hasData);

        if (deleteButton != null && slotIndex != 0) // Don't show delete for AutoSave
            deleteButton.gameObject.SetActive(hasData);

        if (hasData)
        {
            if (saveNameText != null)
            {
                // Special handling for AutoSave slot
                if (slotIndex == 0)
                    saveNameText.text = $"[AutoSave] {saveData.saveName}";
                else
                    saveNameText.text = saveData.saveName;
            }

            if (saveTimeText != null)
                saveTimeText.text = saveData.saveDate;

            if (playtimeText != null)
            {
                int hours = Mathf.FloorToInt(saveData.playtimeSeconds / 3600f);
                int minutes = Mathf.FloorToInt((saveData.playtimeSeconds % 3600f) / 60f);
                playtimeText.text = $"{hours:00}:{minutes:00}";
            }

            if (chapterText != null)
                chapterText.text = $"CHAPTER {saveData.currentChapter}";
        }
        else
        {
            if (saveNameText != null)
            {
                if (slotIndex == 0)
                    saveNameText.text = "[AutoSave] No Data";
                else
                    saveNameText.text = isForSaving ? "Empty Slot" : "No Save Data";
            }

            if (saveTimeText != null)
                saveTimeText.text = "";

            if (playtimeText != null)
                playtimeText.text = "";

            if (chapterText != null)
                chapterText.text = "";
        }
    }

    void OnSlotClicked()
    {
        if (uiManager == null) return;
        uiManager.OnSlotClicked(slotIndex);
    }

    void OnDeleteClicked()
    {
        if (uiManager != null)
        {
            uiManager.OnDeleteSlotClicked(slotIndex);
        }
    }
}