using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Dynamic Diary Reader UI that is persistent across scenes.
/// Accepts any number of pages and displays them.
/// </summary>
public class DiaryReaderUI : MonoBehaviour
{
    public static DiaryReaderUI Instance { get; private set; }

    [Header("UI Components")]
    public GameObject diaryPanel;
    public Image[] diaryPageImages; // array of image slots (you can keep only the first used; we handle single-image paging)
    public Button closeButton;
    public Button nextPageButton;
    public Button previousPageButton;
    public TextMeshProUGUI pageNumberText;
    public TextMeshProUGUI titleText;

    [Header("Player UI References")]
    public GameObject joystickObject; // optional: will be disabled when diary is open

    [Header("Optional Default Pages")]
    // If you want to seed a few pages from the inspector, add them here
    public Sprite[] defaultPages;

    private List<Sprite> currentContent = new List<Sprite>();
    private int currentPage = 0;
    private JoystickPlayerController playerController;

    void Awake()
    {
        // Singleton & persistence
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

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

        // Cache player controller if present in scene
        playerController = FindFirstObjectByType<JoystickPlayerController>();

        // Subscribe to global manager (if present)
        if (GlobalDiaryManager.Instance != null)
            GlobalDiaryManager.Instance.OnPagesChanged += OnGlobalPagesChanged;

        // Seed default pages if inspector provided (optional)
        if (defaultPages != null && defaultPages.Length > 0)
        {
            foreach (var s in defaultPages)
                if (s != null) currentContent.Add(s);
        }
    }

    void OnDestroy()
    {
        if (GlobalDiaryManager.Instance != null)
            GlobalDiaryManager.Instance.OnPagesChanged -= OnGlobalPagesChanged;
    }

    void OnGlobalPagesChanged()
    {
        UpdatePages(GlobalDiaryManager.Instance.GetCollectedSprites());
    }

    /// <summary>
    /// Programmatically load a set of pages into the diary UI (replaces current list).
    /// </summary>
    public void UpdatePages(List<Sprite> pages)
    {
        currentContent = pages != null ? new List<Sprite>(pages) : new List<Sprite>();
        currentPage = Mathf.Clamp(currentPage, 0, Mathf.Max(0, currentContent.Count - 1));
        DisplayPage();
    }

    public void ShowDiary()
    {
        if (diaryPanel != null)
            diaryPanel.SetActive(true);

        // If GlobalDiaryManager exists, get its pages
        if (GlobalDiaryManager.Instance != null)
            UpdatePages(GlobalDiaryManager.Instance.GetCollectedSprites());

        if (titleText != null)
            titleText.text = "Diary";

        DisablePlayerControls();
        InventoryManager.Instance?.CloseInventoryUI();
    }

    public void CloseReader()
    {
        if (diaryPanel != null)
            diaryPanel.SetActive(false);

        EnablePlayerControls();
    }

    void DisplayPage()
    {
        // hide all page images
        foreach (var pageImage in diaryPageImages)
            if (pageImage != null)
                pageImage.gameObject.SetActive(false);

        if (currentContent != null && currentContent.Count > 0)
        {
            // we use the first diaryPageImages slot to display the active page sprite
            if (diaryPageImages.Length > 0 && diaryPageImages[0] != null)
            {
                diaryPageImages[0].gameObject.SetActive(true);
                diaryPageImages[0].sprite = currentContent[currentPage];
                diaryPageImages[0].SetNativeSize();
            }
        }

        if (pageNumberText != null)
        {
            int totalPages = currentContent != null ? currentContent.Count : 0;
            pageNumberText.text = totalPages > 0 ? $"Page {currentPage + 1} of {totalPages}" : "";
        }

        if (previousPageButton != null)
            previousPageButton.interactable = currentPage > 0;

        if (nextPageButton != null)
            nextPageButton.interactable = currentContent != null && currentPage < currentContent.Count - 1;
    }

    void NextPage()
    {
        if (currentContent != null && currentPage < currentContent.Count - 1)
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

    void DisablePlayerControls()
    {
        if (playerController != null)
            playerController.enabled = false;

        if (joystickObject != null)
            joystickObject.SetActive(false);
    }

    void EnablePlayerControls()
    {
        if (playerController != null)
            playerController.enabled = true;

        if (joystickObject != null)
            joystickObject.SetActive(true);
    }

    public bool IsReaderOpen()
    {
        return diaryPanel != null && diaryPanel.activeSelf;
    }
}
