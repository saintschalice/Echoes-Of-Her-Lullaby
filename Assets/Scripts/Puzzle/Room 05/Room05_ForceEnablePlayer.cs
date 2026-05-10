using UnityEngine;

/// <summary>
/// Emergency script to force enable player controls in Room 5.
/// Attach to any GameObject in Room05_DiningRoom scene.
/// Press E key to force enable player.
/// </summary>
public class Room05_ForceEnablePlayer : MonoBehaviour
{
    [Header("Debug Key")]
    public KeyCode enableKey = KeyCode.E;

    void Start()
    {
        // Auto-enable player after 1 second
        Invoke(nameof(ForceEnablePlayer), 1f);
    }

    void Update()
    {
        if (Input.GetKeyDown(enableKey))
        {
            ForceEnablePlayer();
        }

        // Press L to log current player state
        if (Input.GetKeyDown(KeyCode.L))
        {
            LogPlayerState();
        }

        // Press R to force resume from Room Controller
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Room05_DiningRoomController.Instance != null)
            {
                Room05_DiningRoomController.Instance.ResumeGameFromUI();
                Debug.Log("[ForceEnable] ✓ ResumeGameFromUI() called");
            }
        }
    }

    void LogPlayerState()
    {
        Debug.Log("=== PLAYER STATE DEBUG ===");

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            JoystickPlayerController controller = player.GetComponent<JoystickPlayerController>();
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            Animator anim = player.GetComponent<Animator>();

            Debug.Log($"Player exists: ✓");
            Debug.Log($"Controller exists: {(controller != null ? "✓" : "✗")}");
            Debug.Log($"Controller enabled: {(controller != null ? controller.enabled : false)}");
            Debug.Log($"Rigidbody2D exists: {(rb != null ? "✓" : "✗")}");
            Debug.Log($"Rigidbody2D simulated: {(rb != null ? rb.simulated : false)}");
            Debug.Log($"Animator exists: {(anim != null ? "✓" : "✗")}");
            Debug.Log($"Animator enabled: {(anim != null ? anim.enabled : false)}");
        }
        else
        {
            Debug.LogError("Player not found!");
        }

        GameObject joystick = GameObject.Find("Joystick");
        if (joystick == null) joystick = GameObject.Find("FloatingJoystick");
        if (joystick == null) joystick = GameObject.Find("VariableJoystick");

        if (joystick != null)
        {
            Debug.Log($"Joystick exists: ✓ ({joystick.name})");
            Debug.Log($"Joystick active: {joystick.activeSelf}");
            VirtualJoystick vj = joystick.GetComponent<VirtualJoystick>();
            Debug.Log($"VirtualJoystick component: {(vj != null ? "✓" : "✗")}");
            Debug.Log($"VirtualJoystick enabled: {(vj != null ? vj.enabled : false)}");
        }
        else
        {
            Debug.LogWarning("Joystick not found!");
        }

        if (Room05_DiningRoomController.Instance != null)
        {
            Debug.Log("Room Controller exists: ✓");
        }
        else
        {
            Debug.LogWarning("Room Controller not found!");
        }

        Debug.Log("=========================");
    }

    [ContextMenu("Force Enable Player")]
    public void ForceEnablePlayer()
    {
        Debug.Log("=== FORCE ENABLING PLAYER ===");

        // 1. Find and enable player controller
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            JoystickPlayerController controller = player.GetComponent<JoystickPlayerController>();
            if (controller != null)
            {
                controller.enabled = true;
                Debug.Log("[ForceEnable] Player controller enabled");
            }

            // Enable Rigidbody2D
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.simulated = true;
                rb.linearVelocity = Vector2.zero;
                Debug.Log("[ForceEnable] Rigidbody2D enabled");
            }
        }
        else
        {
            Debug.LogError("[ForceEnable] Player not found!");
        }

        // 2. Find and enable joystick
        GameObject joystick = GameObject.Find("Joystick");
        if (joystick == null) joystick = GameObject.Find("FloatingJoystick");
        if (joystick == null) joystick = GameObject.Find("VariableJoystick");

        if (joystick != null)
        {
            joystick.SetActive(true);
            CanvasGroup cg = joystick.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
            Debug.Log($"[ForceEnable] Joystick enabled: {joystick.name}");
        }
        else
        {
            Debug.LogWarning("[ForceEnable] Joystick not found!");
        }

        // 3. Resume from UI pause if needed
        if (Room05_DiningRoomController.Instance != null)
        {
            Room05_DiningRoomController.Instance.ResumeGameFromUI();
            Debug.Log("[ForceEnable] Room controller resumed");
        }

        // 4. Close any blocking UI
        CloseBlockingUI();

        Debug.Log("=== PLAYER ENABLED ===");
        Debug.Log("Try moving now!");
    }

    void CloseBlockingUI()
    {
        // Close calendar if open
        CalendarViewer calendar = FindFirstObjectByType<CalendarViewer>();
        if (calendar != null && calendar.gameObject.activeSelf)
        {
            calendar.gameObject.SetActive(false);
            Debug.Log("[ForceEnable] Closed calendar");
        }

        // Close cabinet UI if open
        if (Room05_DiningRoomController.Instance != null)
        {
            Room05_DiningRoomController.Instance.CloseCabinetUI();
            Room05_DiningRoomController.Instance.CloseTableUI();
            Room05_DiningRoomController.Instance.CloseCalendarUI();
            Debug.Log("[ForceEnable] Closed all Room 5 UI");
        }

        // Close recipe book if somehow still open
        if (RecipeBookUI.Instance != null)
        {
            RecipeBookUI.Instance.CloseBook();
            Debug.Log("[ForceEnable] Closed recipe book");
        }
    }
}
