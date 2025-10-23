using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiaryReaderUI : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject diaryPanel;
    public Image[] diaryPageImages; // 4 image slots for diary pages
    public Button closeButton;
    public Button nextPageButton;
    public Button previousPageButton;
    public TextMeshProUGUI pageNumberText;
    public TextMeshProUGUI titleText; // For showing "Diary"

    [Header("Player UI References")]
    [Tooltip("Assign your Joystick GameObject here manually.")]
    public GameObject joystickObject;
    private JoystickPlayerController playerController;

    [Header("Diary Page Sprites")]
    public Sprite diaryPage1Sprite;
    public Sprite diaryPage2Sprite;
    public Sprite diaryPage3Sprite;
    public Sprite diaryPage4Sprite;

    private int currentPage = 0;
    private Sprite[] currentContent;

    void Start()
    {
        if (diaryPanel != null)
            diaryPanel.SetActive(false);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseReader);

        if (nextPageButton != null)
            nextPageButton.onClick.AddListener(NextPage);

        if (previousPageButton != null)
            previousPageButton.onClick.AddListener(PreviousPage);

        // Cache player controller for reuse
        playerController = FindFirstObjectByType<JoystickPlayerController>();
    }

    // ------------------------------
    // Show full diary
    // ------------------------------
    public void ShowDiary()
    {
        if (diaryPanel != null)
            diaryPanel.SetActive(true);

        currentContent = new Sprite[] { diaryPage1Sprite, diaryPage2Sprite, diaryPage3Sprite, diaryPage4Sprite };
        currentPage = 0;

        if (titleText != null)
            titleText.text = "Emily's Diary";

        DisplayPage();
        DisablePlayerControls();
    }

    // ------------------------------
    // Close reader
    // ------------------------------
    public void CloseReader()
    {
        if (diaryPanel != null)
            diaryPanel.SetActive(false);

        EnablePlayerControls();
        currentContent = null;
    }

    public void CloseDiary() => CloseReader(); // alias

    // ------------------------------
    // Page handling
    // ------------------------------
    void DisplayPage()
    {
        foreach (var pageImage in diaryPageImages)
        {
            if (pageImage != null)
                pageImage.gameObject.SetActive(false);
        }

        if (currentContent != null && currentPage >= 0 && currentPage < currentContent.Length)
        {
            if (diaryPageImages.Length > 0 && diaryPageImages[0] != null)
            {
                diaryPageImages[0].gameObject.SetActive(true);
                diaryPageImages[0].sprite = currentContent[currentPage];
            }
        }

        if (pageNumberText != null)
        {
            int totalPages = currentContent != null ? currentContent.Length : 0;
            pageNumberText.text = totalPages > 0 ? $"Page {currentPage + 1} of {totalPages}" : "";
        }

        if (previousPageButton != null)
            previousPageButton.interactable = currentPage > 0;

        if (nextPageButton != null && currentContent != null)
            nextPageButton.interactable = currentPage < currentContent.Length - 1;
    }

    void NextPage()
    {
        if (currentContent != null && currentPage < currentContent.Length - 1)
        {
            currentPage++;
            DisplayPage();
            AudioManager.Instance?.PlaySFX(null);
        }
    }

    void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            DisplayPage();
            AudioManager.Instance?.PlaySFX(null);
        }
    }

    // ------------------------------
    // Player control handling
    // ------------------------------
    void DisablePlayerControls()
    {
        if (playerController != null)
            playerController.enabled = false;

        if (joystickObject != null)
            joystickObject.SetActive(false);
        else
            Debug.LogWarning("[DiaryReaderUI] Joystick not assigned in inspector!");
    }

    void EnablePlayerControls()
    {
        if (playerController != null)
            playerController.enabled = true;

        if (joystickObject != null)
            joystickObject.SetActive(true);
        else
            Debug.LogWarning("[DiaryReaderUI] Joystick not assigned in inspector!");
    }

    public bool IsReaderOpen()
    {
        return diaryPanel != null && diaryPanel.activeSelf;
    }
}
