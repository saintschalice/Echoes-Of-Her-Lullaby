using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Dollhouse Puzzle - Drag Emily Doll into the dollhouse
/// </summary>
public class DollhousePuzzleUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dollhousePanel;
    public Button closeButton;

    [Header("Drag & Drop")]
    public GameObject emilyDollDraggable; // The doll that can be dragged
    public Transform dollSlot; // The target slot in dollhouse
    public float snapDistance = 50f;

    [Header("Visual Feedback")]
    public Image slotHighlight;
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    [Header("Audio")]
    public AudioClip dollPlaceSound;
    public AudioClip successSound;

    private bool isPuzzleSolved = false;
    private Vector3 dollStartPosition;
    private RectTransform dollRectTransform;

    void Start()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePuzzle);

        if (emilyDollDraggable != null)
        {
            dollRectTransform = emilyDollDraggable.GetComponent<RectTransform>();
            dollStartPosition = dollRectTransform.anchoredPosition;

            // Add drag handler
            EventTrigger trigger = emilyDollDraggable.GetComponent<EventTrigger>();
            if (trigger == null)
                trigger = emilyDollDraggable.AddComponent<EventTrigger>();

            // Drag event
            EventTrigger.Entry dragEntry = new EventTrigger.Entry();
            dragEntry.eventID = EventTriggerType.Drag;
            dragEntry.callback.AddListener((data) => { OnDragDoll((PointerEventData)data); });
            trigger.triggers.Add(dragEntry);

            // End drag event
            EventTrigger.Entry endDragEntry = new EventTrigger.Entry();
            endDragEntry.eventID = EventTriggerType.EndDrag;
            endDragEntry.callback.AddListener((data) => { OnEndDragDoll((PointerEventData)data); });
            trigger.triggers.Add(endDragEntry);
        }

        if (slotHighlight != null)
            slotHighlight.color = normalColor;
    }

    void OnEnable()
    {
        PauseGame();
        isPuzzleSolved = false;

        // Reset doll position
        if (dollRectTransform != null)
            dollRectTransform.anchoredPosition = dollStartPosition;

        if (slotHighlight != null)
            slotHighlight.color = normalColor;
    }

    void OnDragDoll(PointerEventData data)
    {
        if (isPuzzleSolved) return;

        // Move doll with pointer
        dollRectTransform.anchoredPosition += data.delta;

        // Check distance to slot
        float distance = Vector2.Distance(dollRectTransform.anchoredPosition, dollSlot.GetComponent<RectTransform>().anchoredPosition);

        // Highlight slot when near
        if (slotHighlight != null)
        {
            slotHighlight.color = distance < snapDistance ? highlightColor : normalColor;
        }
    }

    void OnEndDragDoll(PointerEventData data)
    {
        if (isPuzzleSolved) return;

        // Check if doll is close enough to slot
        float distance = Vector2.Distance(dollRectTransform.anchoredPosition, dollSlot.GetComponent<RectTransform>().anchoredPosition);

        if (distance < snapDistance)
        {
            // Snap to slot
            dollRectTransform.anchoredPosition = dollSlot.GetComponent<RectTransform>().anchoredPosition;
            PlaySound(dollPlaceSound);

            // Puzzle solved!
            StartCoroutine(CompletePuzzle());
        }
        else
        {
            // Return to start position
            dollRectTransform.anchoredPosition = dollStartPosition;
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

        // Notify UI Manager
        Room07UIManager uiManager = FindFirstObjectByType<Room07UIManager>();
        if (uiManager != null)
        {
            uiManager.OnDollhouseSolved();
        }

        ResumeGame();
    }

    void ClosePuzzle()
    {
        if (dollhousePanel != null)
            dollhousePanel.SetActive(false);

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
