using UnityEngine;
using System.Collections;

/// <summary>
/// Automatic fix for Room 5 frozen player issue.
/// Attach to any GameObject in Room 5 scene.
/// Runs automatically on scene load.
/// </summary>
public class Room05_AutoFix : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Delay before running auto-fix (seconds)")]
    public float fixDelay = 0.5f;

    [Header("Debug")]
    public bool showDebugLogs = true;

    void Start()
    {
        if (showDebugLogs)
        {
            Debug.Log("[Room05_AutoFix] Starting auto-fix sequence...");
        }

        StartCoroutine(AutoFixSequence());
    }

    IEnumerator AutoFixSequence()
    {
        // Wait for scene to fully load
        yield return new WaitForSeconds(fixDelay);

        if (showDebugLogs)
        {
            Debug.Log("[Room05_AutoFix] Running auto-fix...");
        }

        bool fixedSomething = false;

        // Fix 1: Enable Player Controller
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            JoystickPlayerController controller = player.GetComponent<JoystickPlayerController>();
            if (controller != null && !controller.enabled)
            {
                controller.enabled = true;
                fixedSomething = true;
                if (showDebugLogs)
                {
                    Debug.Log("[Room05_AutoFix] ✓ Enabled JoystickPlayerController");
                }
            }

            // Fix Rigidbody2D
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                if (!rb.simulated)
                {
                    rb.simulated = true;
                    fixedSomething = true;
                    if (showDebugLogs)
                    {
                        Debug.Log("[Room05_AutoFix] ✓ Enabled Rigidbody2D simulation");
                    }
                }

                if (rb.isKinematic)
                {
                    rb.isKinematic = false;
                    fixedSomething = true;
                    if (showDebugLogs)
                    {
                        Debug.Log("[Room05_AutoFix] ✓ Disabled Rigidbody2D kinematic");
                    }
                }

                // Reset velocity
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            // Fix Animator
            Animator anim = player.GetComponent<Animator>();
            if (anim != null && !anim.enabled)
            {
                anim.enabled = true;
                fixedSomething = true;
                if (showDebugLogs)
                {
                    Debug.Log("[Room05_AutoFix] ✓ Enabled Animator");
                }
            }
        }
        else
        {
            Debug.LogError("[Room05_AutoFix] ❌ Player not found!");
        }

        // Fix 2: Enable Joystick
        GameObject joystick = FindJoystick();
        if (joystick != null)
        {
            if (!joystick.activeSelf)
            {
                joystick.SetActive(true);
                fixedSomething = true;
                if (showDebugLogs)
                {
                    Debug.Log($"[Room05_AutoFix] ✓ Enabled Joystick: {joystick.name}");
                }
            }

            VirtualJoystick vj = joystick.GetComponent<VirtualJoystick>();
            if (vj != null && !vj.enabled)
            {
                vj.enabled = true;
                fixedSomething = true;
                if (showDebugLogs)
                {
                    Debug.Log("[Room05_AutoFix] ✓ Enabled VirtualJoystick component");
                }
            }

            // Fix CanvasGroup
            CanvasGroup cg = joystick.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                if (cg.alpha < 1f || !cg.interactable || !cg.blocksRaycasts)
                {
                    cg.alpha = 1f;
                    cg.interactable = true;
                    cg.blocksRaycasts = true;
                    fixedSomething = true;
                    if (showDebugLogs)
                    {
                        Debug.Log("[Room05_AutoFix] ✓ Fixed Joystick CanvasGroup");
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("[Room05_AutoFix] ⚠ Joystick not found!");
        }

        // Fix 3: Resume from UI pause
        if (Room05_DiningRoomController.Instance != null)
        {
            Room05_DiningRoomController.Instance.ResumeGameFromUI();
            fixedSomething = true;
            if (showDebugLogs)
            {
                Debug.Log("[Room05_AutoFix] ✓ Called ResumeGameFromUI()");
            }
        }

        // Fix 4: Close any blocking UI
        CloseBlockingUI();

        // Final report
        if (fixedSomething)
        {
            if (showDebugLogs)
            {
                Debug.Log("[Room05_AutoFix] ✅ Auto-fix complete! Player should be able to move now.");
            }
        }
        else
        {
            if (showDebugLogs)
            {
                Debug.Log("[Room05_AutoFix] ℹ No issues detected. Everything looks good!");
            }
        }

        // Wait a bit more, then verify
        yield return new WaitForSeconds(0.5f);
        VerifyFix();
    }

    GameObject FindJoystick()
    {
        // Try multiple common joystick names
        string[] joystickNames = {
            "Joystick",
            "DynamicJoystick",
            "FloatingJoystick",
            "VariableJoystick",
            "FixedJoystick"
        };

        foreach (string name in joystickNames)
        {
            GameObject obj = GameObject.Find(name);
            if (obj != null)
            {
                return obj;
            }
        }

        // Try finding by component
        VirtualJoystick vj = FindFirstObjectByType<VirtualJoystick>();
        if (vj != null)
        {
            return vj.gameObject;
        }

        return null;
    }

    void CloseBlockingUI()
    {
        bool closedSomething = false;

        // Close calendar if open
        CalendarViewer calendar = FindFirstObjectByType<CalendarViewer>();
        if (calendar != null && calendar.gameObject.activeSelf)
        {
            calendar.gameObject.SetActive(false);
            closedSomething = true;
            if (showDebugLogs)
            {
                Debug.Log("[Room05_AutoFix] ✓ Closed CalendarViewer");
            }
        }

        // Close Room 5 UI panels
        if (Room05_DiningRoomController.Instance != null)
        {
            Room05_DiningRoomController.Instance.CloseCabinetUI();
            Room05_DiningRoomController.Instance.CloseTableUI();
            Room05_DiningRoomController.Instance.CloseCalendarUI();
        }

        // Close recipe book if somehow open
        if (RecipeBookUI.Instance != null && RecipeBookUI.Instance.panel != null && RecipeBookUI.Instance.panel.activeSelf)
        {
            RecipeBookUI.Instance.CloseBook();
            closedSomething = true;
            if (showDebugLogs)
            {
                Debug.Log("[Room05_AutoFix] ✓ Closed RecipeBookUI");
            }
        }

        // Close inventory if open
        if (InventoryUI.Instance != null && InventoryUI.Instance.IsOpen)
        {
            InventoryUI.Instance.CloseInventory();
            closedSomething = true;
            if (showDebugLogs)
            {
                Debug.Log("[Room05_AutoFix] ✓ Closed InventoryUI");
            }
        }

        if (closedSomething && showDebugLogs)
        {
            Debug.Log("[Room05_AutoFix] ✓ Closed blocking UI");
        }
    }

    void VerifyFix()
    {
        if (!showDebugLogs) return;

        Debug.Log("[Room05_AutoFix] === VERIFICATION ===");

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            JoystickPlayerController controller = player.GetComponent<JoystickPlayerController>();
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

            bool playerOK = controller != null && controller.enabled && rb != null && rb.simulated;
            Debug.Log($"[Room05_AutoFix] Player: {(playerOK ? "✅ OK" : "❌ PROBLEM")}");
        }

        GameObject joystick = FindJoystick();
        if (joystick != null)
        {
            VirtualJoystick vj = joystick.GetComponent<VirtualJoystick>();
            bool joystickOK = joystick.activeSelf && vj != null && vj.enabled;
            Debug.Log($"[Room05_AutoFix] Joystick: {(joystickOK ? "✅ OK" : "❌ PROBLEM")}");
        }
        else
        {
            Debug.Log("[Room05_AutoFix] Joystick: ❌ NOT FOUND");
        }

        Debug.Log("[Room05_AutoFix] === END VERIFICATION ===");
        Debug.Log("[Room05_AutoFix] If player still can't move, press E key to force enable.");
    }
}
