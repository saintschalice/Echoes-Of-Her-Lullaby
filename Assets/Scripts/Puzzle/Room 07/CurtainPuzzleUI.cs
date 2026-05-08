using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Curtain Puzzle - Player must open both left and right curtains
/// Uses 4 sprite states: Both Closed, Left Open, Right Open, Both Open
/// </summary>
public class CurtainPuzzleUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject curtainPanel;
    public Button leftCurtainButton;
    public Button rightCurtainButton;
    public Button closeButton;

    [Header("Curtain Sprites (4 States)")]
    public Image curtainImage; // Single image that changes sprites
    public Sprite bothClosedSprite;    // State 1: Both closed
    public Sprite leftOpenSprite;      // State 2: Left open, right closed
    public Sprite rightOpenSprite;     // State 3: Left closed, right open
    public Sprite bothOpenSprite;      // State 4: Both open

    [Header("Audio")]
    public AudioClip curtainOpenSound;

    private bool isLeftOpen = false;
    private bool isRightOpen = false;

    void Start()
    {
        // Setup button listeners
        if (leftCurtainButton != null)
            leftCurtainButton.onClick.AddListener(ToggleLeftCurtain);

        if (rightCurtainButton != null)
            rightCurtainButton.onClick.AddListener(ToggleRightCurtain);

        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePuzzle);

        // Initial state
        ResetCurtains();
    }

    void OnEnable()
    {
        // Pause game when panel opens
        PauseGame();
        ResetCurtains();
    }

    void ResetCurtains()
    {
        isLeftOpen = false;
        isRightOpen = false;
        UpdateCurtainSprite();
    }

    void ToggleLeftCurtain()
    {
        isLeftOpen = !isLeftOpen;
        PlaySound(curtainOpenSound);
        UpdateCurtainSprite();
        CheckCompletion();
    }

    void ToggleRightCurtain()
    {
        isRightOpen = !isRightOpen;
        PlaySound(curtainOpenSound);
        UpdateCurtainSprite();
        CheckCompletion();
    }

    void UpdateCurtainSprite()
    {
        if (curtainImage == null) return;

        // Determine which sprite to show based on state
        if (!isLeftOpen && !isRightOpen)
        {
            // State 1: Both closed
            curtainImage.sprite = bothClosedSprite;
        }
        else if (isLeftOpen && !isRightOpen)
        {
            // State 2: Left open, right closed
            curtainImage.sprite = leftOpenSprite;
        }
        else if (!isLeftOpen && isRightOpen)
        {
            // State 3: Left closed, right open
            curtainImage.sprite = rightOpenSprite;
        }
        else if (isLeftOpen && isRightOpen)
        {
            // State 4: Both open
            curtainImage.sprite = bothOpenSprite;
        }
    }

    void CheckCompletion()
    {
        if (isLeftOpen && isRightOpen)
        {
            StartCoroutine(CompletePuzzle());
        }
    }

    IEnumerator CompletePuzzle()
    {
        // Disable buttons to prevent further clicks
        if (leftCurtainButton != null) leftCurtainButton.interactable = false;
        if (rightCurtainButton != null) rightCurtainButton.interactable = false;

        yield return new WaitForSeconds(0.5f);

        // Notify UI Manager
        Room07UIManager uiManager = FindFirstObjectByType<Room07UIManager>();
        if (uiManager != null)
        {
            uiManager.OnCurtainsOpened();
        }

        ResumeGame();
    }

    void ClosePuzzle()
    {
        if (curtainPanel != null)
            curtainPanel.SetActive(false);

        ResumeGame();
    }

    void PauseGame()
    {
        // Pause Emily AI
        EmilyGhost emily = FindFirstObjectByType<EmilyGhost>();
        if (emily != null) emily.isPaused = true;

        // Disable player movement
        JoystickPlayerController player = FindFirstObjectByType<JoystickPlayerController>();
        if (player != null) player.enabled = false;

        // Hide joystick
        GameObject joystick = GameObject.Find("Joystick");
        if (joystick != null) joystick.SetActive(false);
    }

    void ResumeGame()
    {
        // Resume Emily AI
        EmilyGhost emily = FindFirstObjectByType<EmilyGhost>();
        if (emily != null) emily.isPaused = false;

        // Enable player movement
        JoystickPlayerController player = FindFirstObjectByType<JoystickPlayerController>();
        if (player != null) player.enabled = true;

        // Show joystick
        GameObject joystick = GameObject.Find("Joystick");
        if (joystick != null) joystick.SetActive(true);
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
            AudioManager.Instance?.PlaySFX(clip);
    }
}
