using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Persistent diary UI that displays collected pages from GlobalDiaryManager (authoritative).
/// - No inventory reads here.
/// - Emits OnDiaryClosed when the user closes the panel (for quiz gating, etc.).
/// - Handles empty state, player input toggling, and safe refresh.
/// - Designed to be controlled by external quiz logic (e.g., SnugglesQuizManager).
/// </summary>
public class DiaryReaderUI : MonoBehaviour
{
    public static DiaryReaderUI Instance { get; private set; }
    public static event Action OnDiaryClosed;

    [Header("UI Components")]
    [Tooltip("Root panel that contains the diary UI.")]
    public GameObject diaryPanel;
    private CanvasGroup diaryCanvasGroup;

    [Tooltip("Optional: A single Image to preview the current page sprite (if not using per-page objects).")]
    public Image diaryPageImage;

    [Tooltip("Close button to hide the diary.")]
    public Button closeButton;

    [Tooltip("Buttons to navigate pages.")]
    public Button nextPageButton;
    public Button previousPageButton;

    [Tooltip("Text to display 1/N page indicator.")]
    public TextMeshProUGUI pageNumberText;

    [Tooltip("Title text (e.g., 'Diary Entries').")]
    public TextMeshProUGUI titleText;

    [Header("Page Container")]
    [Tooltip("If you use per-page prefabs/objects (one per page), assign them here in order. We'll toggle active based on index.")]
    public GameObject[] pageObjects;

    [Header("Player UI References")]
    [Tooltip("Joystick canvas object (or root) to disable while reading.")]
    public GameObject joystickObject;

    [Header("Empty State")]
    [Tooltip("Message to show when no diary pages are collected.")]
    public string emptyDiaryMessage = "No diary pages collected yet.";
    public TextMeshProUGUI emptyStateText;

    [Header("Display Settings")]
    public string diaryTitle = "Diary Entries";

    // Runtime
    private readonly List<Sprite> currentPages = new List<Sprite>();
    private int currentPageIndex = 0;
    private JoystickPlayerController playerController;
    private bool isInitialized;

    #region Unity Lifecycle

    private void Awake()
    {
        // Singleton
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

        // Fallback: attempt to find the diary panel by name if not set
        if (diaryPanel == null)
        {
            var found = GameObject.Find("DiaryPanel");
            if (found != null)
            {
                diaryPanel = found;
                Debug.Log("[DiaryReaderUI] Found DiaryPanel by name in Awake.");
            }
        }

        // Ensure CanvasGroup for alpha/interaction control
        if (diaryPanel != null)
        {
            diaryCanvasGroup = diaryPanel.GetComponent<CanvasGroup>();
            if (diaryCanvasGroup == null)
            {
                diaryCanvasGroup = diaryPanel.AddComponent<CanvasGroup>();
                Debug.Log("[DiaryReaderUI] Added CanvasGroup to DiaryPanel.");
            }
        }
    }

    private void Start()
    {
        InitializeUI();
    }

    private void OnEnable()
    {
        SubscribeToDiaryManager();
    }

    private void OnDisable()
    {
        UnsubscribeFromDiaryManager();
    }

    private void OnDestroy()
    {
        UnsubscribeFromDiaryManager();
    }

    #endregion

    #region Initialization & Subscriptions

    private void InitializeUI()
    {
        if (isInitialized) return;

        if (titleText != null) titleText.text = diaryTitle;

        if (diaryPanel != null)
        {
            diaryPanel.SetActive(false);
            if (diaryCanvasGroup == null)
                diaryCanvasGroup = diaryPanel.GetComponent<CanvasGroup>();
            if (diaryCanvasGroup != null)
            {
                diaryCanvasGroup.alpha = 0f;
                diaryCanvasGroup.blocksRaycasts = false;
                diaryCanvasGroup.interactable = false;
            }
        }

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseDiary);

        if (nextPageButton != null)
            nextPageButton.onClick.AddListener(NextPage);

        if (previousPageButton != null)
            previousPageButton.onClick.AddListener(PreviousPage);

        SubscribeToDiaryManager();
        RefreshPages(); // populate from GlobalDiaryManager

        isInitialized = true;
    }

    private void SubscribeToDiaryManager()
    {
        if (GlobalDiaryManager.Instance != null)
        {
            GlobalDiaryManager.Instance.OnPagesChanged -= OnPagesChanged;
            GlobalDiaryManager.Instance.OnPagesChanged += OnPagesChanged;
        }
    }

    private void UnsubscribeFromDiaryManager()
    {
        if (GlobalDiaryManager.Instance != null)
        {
            GlobalDiaryManager.Instance.OnPagesChanged -= OnPagesChanged;
        }
    }

    private void OnPagesChanged()
    {
        RefreshPages();
    }

    #endregion

    #region Public API

    /// <summary>Open the diary (from hotkey/menu/interaction).</summary>
    public void ShowDiary()
    {
        EnsurePanelReference();

        if (diaryPanel == null)
        {
            Debug.LogWarning("[DiaryReaderUI] Cannot open diary: diaryPanel is null.");
            return;
        }

        // Activate and make interactable
        diaryPanel.SetActive(true);
        if (diaryCanvasGroup == null)
            diaryCanvasGroup = diaryPanel.GetComponent<CanvasGroup>();
        if (diaryCanvasGroup != null)
        {
            diaryCanvasGroup.alpha = 1f;
            diaryCanvasGroup.blocksRaycasts = true;
            diaryCanvasGroup.interactable = true;
        }

        // Bring to front (avoid occlusion)
        diaryPanel.transform.SetAsLastSibling();

        // Optional animator (if you have one named "Open")
        var animator = diaryPanel.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = true;
            animator.Play("Open", -1, 0f);
        }

        if (!isInitialized) InitializeUI();

        RefreshPages();
        DisplayCurrentPage();

        DisablePlayerControls();
        InventoryManager.Instance?.CloseInventoryUI();

        Debug.Log("[DiaryReaderUI] Diary opened.");
    }

    /// <summary>Alias for ShowDiary, in case you wired a different name already.</summary>
    public void OpenDiaryFromMenu() => ShowDiary();

    /// <summary>Close diary and re-enable player controls.</summary>
    public void CloseDiary()
    {
        if (diaryPanel != null)
        {
            diaryPanel.SetActive(false);
            if (diaryCanvasGroup != null)
            {
                diaryCanvasGroup.blocksRaycasts = false;
                diaryCanvasGroup.interactable = false;
                diaryCanvasGroup.alpha = 0f;
            }
        }

        EnablePlayerControls();

        // 🔔 NEW: Re-open inventory when diary closes, as requested
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OpenInventoryUI();
        }

        // 🔔 Let quiz managers (e.g., SnugglesQuizManager) know we closed
        Debug.Log("[DEBUG_TRACE] [DiaryReaderUI] Invoking OnDiaryClosed event...");
        OnDiaryClosed?.Invoke();
    }

    /// <summary>Returns true if the diary is currently visible.</summary>
    public bool IsReaderOpen()
    {
        return diaryPanel != null && diaryPanel.activeSelf;
    }

    #endregion

    #region Page Refresh & Navigation

    /// <summary>Rebuilds page sprites from GlobalDiaryManager's collected IDs.</summary>
    public void RefreshPages()
    {
        currentPages.Clear();

        if (GlobalDiaryManager.Instance != null)
        {
            var ids = GlobalDiaryManager.Instance.GetCollectedIds();
            if (ids != null && ids.Count > 0)
            {
                foreach (var id in ids)
                {
                    var sprite = GlobalDiaryManager.Instance.GetSpriteForPageId(id);
                    if (sprite != null) currentPages.Add(sprite);
                    else Debug.LogWarning($"[DiaryReaderUI] No sprite registered for diary page id '{id}'.");
                }
            }
        }

        // Keep index in range
        currentPageIndex = Mathf.Clamp(currentPageIndex, 0, Mathf.Max(0, currentPages.Count - 1));

        // If open, make sure the UI reflects new content
        if (IsReaderOpen())
            DisplayCurrentPage();
    }

    /// <summary>Renders the current page or empty state.</summary>
    public void DisplayCurrentPage()
    {
        // Title (static)
        if (titleText != null) titleText.text = diaryTitle;

        // Handle empty state
        if (currentPages.Count == 0)
        {
            // Per-page objects hidden
            TogglePageObjects(-1);

            // Sprite preview cleared
            if (diaryPageImage != null) diaryPageImage.sprite = null;

            // Page number text
            if (pageNumberText != null) pageNumberText.text = "0/0";

            // Empty message
            if (emptyStateText != null)
            {
                emptyStateText.gameObject.SetActive(true);
                emptyStateText.text = emptyDiaryMessage;
            }

            return;
        }

        // Clamp index (safety)
        currentPageIndex = Mathf.Clamp(currentPageIndex, 0, currentPages.Count - 1);

        // Hide empty text
        if (emptyStateText != null) emptyStateText.gameObject.SetActive(false);

        // Toggle per-page objects (if provided)
        TogglePageObjects(currentPageIndex);

        // Single-image preview (optional)
        if (diaryPageImage != null)
            diaryPageImage.sprite = currentPages[currentPageIndex];

        // Page number
        if (pageNumberText != null)
            pageNumberText.text = $"{currentPageIndex + 1}/{currentPages.Count}";
    }

    public void NextPage()
    {
        if (currentPages.Count == 0) return;
        int next = Mathf.Clamp(currentPageIndex + 1, 0, currentPages.Count - 1);
        if (next != currentPageIndex)
        {
            currentPageIndex = next;
            DisplayCurrentPage();
        }
    }

    public void PreviousPage()
    {
        if (currentPages.Count == 0) return;
        int prev = Mathf.Clamp(currentPageIndex - 1, 0, currentPages.Count - 1);
        if (prev != currentPageIndex)
        {
            currentPageIndex = prev;
            DisplayCurrentPage();
        }
    }

    private void TogglePageObjects(int activeIndex)
    {
        if (pageObjects == null || pageObjects.Length == 0) return;

        for (int i = 0; i < pageObjects.Length; i++)
        {
            if (pageObjects[i] == null) continue;
            bool shouldBeActive = (i == activeIndex) && currentPages.Count > 0;
            pageObjects[i].SetActive(shouldBeActive);
        }
    }

    #endregion

    #region Player Control Gating

    private void DisablePlayerControls()
    {
        if (playerController == null)
            playerController = FindFirstObjectByType<JoystickPlayerController>();

        if (playerController != null) playerController.enabled = false;
        if (joystickObject != null) joystickObject.SetActive(false);

        // Pause Emily AI
        EmilyGhost emily = FindFirstObjectByType<EmilyGhost>();
        if (emily != null) emily.isPaused = true;
    }

    private void EnablePlayerControls()
    {
        if (playerController == null)
            playerController = FindFirstObjectByType<JoystickPlayerController>();

        if (playerController != null) playerController.enabled = true;
        if (joystickObject != null) joystickObject.SetActive(true);

        // Resume Emily AI
        EmilyGhost emily = FindFirstObjectByType<EmilyGhost>();
        if (emily != null) emily.isPaused = false;
    }

    #endregion

    #region Utilities

    private void EnsurePanelReference()
    {
        if (diaryPanel == null)
        {
            var fallback = GameObject.Find("DiaryPanel");
            if (fallback != null)
            {
                diaryPanel = fallback;
                diaryCanvasGroup = diaryPanel.GetComponent<CanvasGroup>() ?? diaryPanel.AddComponent<CanvasGroup>();
                Debug.Log("[DiaryReaderUI] Fallback: found DiaryPanel by name in ShowDiary().");
            }
        }
    }

    #endregion

    #region Context Menu (Editor Testing)

    [ContextMenu("Test: Open Diary")]
    private void _TestOpenDiary() => ShowDiary();

    [ContextMenu("Test: Close Diary")]
    private void _TestCloseDiary() => CloseDiary();

    #endregion
}