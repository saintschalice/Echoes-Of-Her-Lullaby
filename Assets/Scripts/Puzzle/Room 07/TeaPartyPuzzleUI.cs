using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Tea Party Puzzle - Drag and drop Emily's Cup to complete the ritual
/// </summary>
public class TeaPartyPuzzleUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject teaPartyPanel;
    public Button closeButton;

    [Header("Drag & Drop")]
    public GameObject emilyCupDraggable; // The cup that can be dragged
    public Transform emilyCupSlot; // The target slot
    public float snapDistance = 50f; // Distance to snap to slot

    [Header("Visual Feedback")]
    public Image slotHighlight; // Highlight when cup is near
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    [Header("Audio")]
    public AudioClip cupPlaceSound;
    public AudioClip successSound;

    private bool isPuzzleSolved = false;
    private Vector3 cupStartPosition;
    private RectTransform cupRectTransform;

    void Start()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePuzzle);

        if (emilyCupDraggable != null)
        {
            cupRectTransform = emilyCupDraggable.GetComponent<RectTransform>();
            cupStartPosition = cupRectTransform.anchoredPosition;

            // Add drag handler
            EventTrigger trigger = emilyCupDraggable.GetComponent<EventTrigger>();
            if (trigger == null)
                trigger = emilyCupDraggable.AddComponent<EventTrigger>();

            // Drag event
            EventTrigger.Entry dragEntry = new EventTrigger.Entry();
            dragEntry.eventID = EventTriggerType.Drag;
            dragEntry.callback.AddListener((data) => { OnDragCup((PointerEventData)data); });
            trigger.triggers.Add(dragEntry);

            // End drag event
            EventTrigger.Entry endDragEntry = new EventTrigger.Entry();
            endDragEntry.eventID = EventTriggerType.EndDrag;
            endDragEntry.callback.AddListener((data) => { OnEndDragCup((PointerEventData)data); });
            trigger.triggers.Add(endDragEntry);
        }

        if (slotHighlight != null)
            slotHighlight.color = normalColor;
    }

    void OnEnable()
    {
        PauseGame();
        isPuzzleSolved = false;

        // Reset cup position
        if (cupRectTransform != null)
            cupRectTransform.anchoredPosition = cupStartPosition;

        if (slotHighlight != null)
            slotHighlight.color = normalColor;
    }

    void OnDragCup(PointerEventData data)
    {
        if (isPuzzleSolved) return;

        // Move cup with pointer
        cupRectTransform.anchoredPosition += data.delta;

        // Check distance to slot
        float distance = Vector2.Distance(cupRectTransform.anchoredPosition, emilyCupSlot.GetComponent<RectTransform>().anchoredPosition);

        // Highlight slot when near
        if (slotHighlight != null)
        {
            slotHighlight.color = distance < snapDistance ? highlightColor : normalColor;
        }
    }

    void OnEndDragCup(PointerEventData data)
    {
        if (isPuzzleSolved) return;

        // Check if cup is close enough to slot
        float distance = Vector2.Distance(cupRectTransform.anchoredPosition, emilyCupSlot.GetComponent<RectTransform>().anchoredPosition);

        if (distance < snapDistance)
        {
            // Snap to slot
            cupRectTransform.anchoredPosition = emilyCupSlot.GetComponent<RectTransform>().anchoredPosition;
            PlaySound(cupPlaceSound);

            // Puzzle solved!
            StartCoroutine(CompletePuzzle());
        }
        else
        {
            // Return to start position
            cupRectTransform.anchoredPosition = cupStartPosition;
            if (slotHighlight != null)
                slotHighlight.color = normalColor;
        }
    }

    IEnumerator CompletePuzzle()
    {
        isPuzzleSolved = true;

        if (slotHighlight != null)
            slotHighlight.color = highlightColor;

        PlaySound(successSound);

        yield return new WaitForSeconds(1f);

        // Close panel first
        if (teaPartyPanel != null)
            teaPartyPanel.SetActive(false);

        ResumeGame();

        // Notify UI Manager (this will trigger cutscene and dialogue)
        Room07UIManager uiManager = FindFirstObjectByType<Room07UIManager>();
        if (uiManager != null)
        {
            Debug.Log("[TeaPartyPuzzle] Puzzle completed! Notifying UI Manager...");
            uiManager.OnTeaPartySolved();
        }
        else
        {
            Debug.LogError("[TeaPartyPuzzle] Room07UIManager not found!");
        }
    }

    void ClosePuzzle()
    {
        if (teaPartyPanel != null)
            teaPartyPanel.SetActive(false);

        ResumeGame();
    }

    void PauseGame()
    {
        EmilyGhost emily = FindFirstObjectByType<EmilyGhost>();
        if (emily != null) emily.isPaused = true;

        JoystickPlayerController player = FindFirstObjectByType<JoystickPlayerController>();
        if (player != null) player.enabled = false;

        GameObject joystick = GameObject.Find("Joystick");
        if (joystick != null) joystick.SetActive(false);
    }

    void ResumeGame()
    {
        EmilyGhost emily = FindFirstObjectByType<EmilyGhost>();
        if (emily != null) emily.isPaused = false;

        JoystickPlayerController player = FindFirstObjectByType<JoystickPlayerController>();
        if (player != null) player.enabled = true;

        GameObject joystick = GameObject.Find("Joystick");
        if (joystick != null) joystick.SetActive(true);
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
            AudioManager.Instance?.PlaySFX(clip);
    }
}
