using UnityEngine;
using System.Text;

/// <summary>
/// Diagnostic tool for Room 5 frozen player issue.
/// Press D key to run full diagnostics.
/// Attach to any GameObject in Room 5 scene.
/// </summary>
public class Room05_DiagnosticTool : MonoBehaviour
{
    [Header("Settings")]
    public KeyCode diagnosticKey = KeyCode.D;
    public bool runOnStart = true;

    void Start()
    {
        if (runOnStart)
        {
            Invoke(nameof(RunDiagnostics), 0.5f);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(diagnosticKey))
        {
            RunDiagnostics();
        }
    }

    [ContextMenu("Run Diagnostics")]
    public void RunDiagnostics()
    {
        StringBuilder report = new StringBuilder();
        report.AppendLine("╔════════════════════════════════════════╗");
        report.AppendLine("║   ROOM 5 DIAGNOSTIC REPORT            ║");
        report.AppendLine("╚════════════════════════════════════════╝");
        report.AppendLine();

        // 1. Check Player
        report.AppendLine("【1】 PLAYER STATUS");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            report.AppendLine($"  ✓ Player found: {player.name}");
            report.AppendLine($"  Position: {player.transform.position}");
            report.AppendLine($"  Active: {player.activeInHierarchy}");

            JoystickPlayerController controller = player.GetComponent<JoystickPlayerController>();
            if (controller != null)
            {
                report.AppendLine($"  ✓ JoystickPlayerController: {(controller.enabled ? "ENABLED" : "❌ DISABLED")}");
            }
            else
            {
                report.AppendLine("  ❌ JoystickPlayerController: MISSING");
            }

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                report.AppendLine($"  ✓ Rigidbody2D: {(rb.simulated ? "ENABLED" : "❌ DISABLED")}");
                report.AppendLine($"    Velocity: {rb.linearVelocity}");
                report.AppendLine($"    Kinematic: {rb.isKinematic}");
            }
            else
            {
                report.AppendLine("  ❌ Rigidbody2D: MISSING");
            }

            Animator anim = player.GetComponent<Animator>();
            if (anim != null)
            {
                report.AppendLine($"  ✓ Animator: {(anim.enabled ? "ENABLED" : "❌ DISABLED")}");
            }
            else
            {
                report.AppendLine("  ⚠ Animator: MISSING");
            }
        }
        else
        {
            report.AppendLine("  ❌ PLAYER NOT FOUND!");
        }
        report.AppendLine();

        // 2. Check Joystick
        report.AppendLine("【2】 JOYSTICK STATUS");
        GameObject joystick = GameObject.Find("Joystick");
        if (joystick == null) joystick = GameObject.Find("FloatingJoystick");
        if (joystick == null) joystick = GameObject.Find("VariableJoystick");
        if (joystick == null) joystick = GameObject.Find("DynamicJoystick");

        if (joystick != null)
        {
            report.AppendLine($"  ✓ Joystick found: {joystick.name}");
            report.AppendLine($"  Active: {joystick.activeInHierarchy}");

            VirtualJoystick vj = joystick.GetComponent<VirtualJoystick>();
            if (vj != null)
            {
                report.AppendLine($"  ✓ VirtualJoystick: {(vj.enabled ? "ENABLED" : "❌ DISABLED")}");
            }
            else
            {
                report.AppendLine("  ❌ VirtualJoystick component: MISSING");
            }

            CanvasGroup cg = joystick.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                report.AppendLine($"  CanvasGroup alpha: {cg.alpha}");
                report.AppendLine($"  CanvasGroup interactable: {cg.interactable}");
            }
        }
        else
        {
            report.AppendLine("  ❌ JOYSTICK NOT FOUND!");
        }
        report.AppendLine();

        // 3. Check Room Controller
        report.AppendLine("【3】 ROOM CONTROLLER STATUS");
        if (Room05_DiningRoomController.Instance != null)
        {
            report.AppendLine("  ✓ Room05_DiningRoomController: FOUND");
            // Can't access private fields, but we can check public ones
            report.AppendLine($"  Emily Hunting: {Room05_DiningRoomController.Instance.isEmilyHunting}");
            report.AppendLine($"  Puzzle Completed: {Room05_DiningRoomController.Instance.puzzleCompleted}");
        }
        else
        {
            report.AppendLine("  ❌ Room05_DiningRoomController: NOT FOUND");
        }
        report.AppendLine();

        // 4. Check for Blocking UI
        report.AppendLine("【4】 BLOCKING UI CHECK");
        bool foundBlockingUI = false;

        // Check DialogueSystem
        if (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            report.AppendLine("  ⚠ DialogueSystemV2: ACTIVE (blocking)");
            foundBlockingUI = true;
        }

        // Check RecipeBook
        if (RecipeBookUI.Instance != null && RecipeBookUI.Instance.panel != null && RecipeBookUI.Instance.panel.activeSelf)
        {
            report.AppendLine("  ⚠ RecipeBookUI: OPEN (blocking)");
            foundBlockingUI = true;
        }

        // Check Inventory
        if (InventoryUI.Instance != null && InventoryUI.Instance.IsOpen)
        {
            report.AppendLine("  ⚠ InventoryUI: OPEN (may be blocking)");
            foundBlockingUI = true;
        }

        // Check Calendar
        CalendarViewer calendar = FindFirstObjectByType<CalendarViewer>();
        if (calendar != null && calendar.gameObject.activeSelf)
        {
            report.AppendLine("  ⚠ CalendarViewer: OPEN (blocking)");
            foundBlockingUI = true;
        }

        if (!foundBlockingUI)
        {
            report.AppendLine("  ✓ No blocking UI detected");
        }
        report.AppendLine();

        // 5. Check ScreenFader
        report.AppendLine("【5】 SCREENFADER STATUS");
        if (ScreenFader.Instance != null)
        {
            report.AppendLine("  ✓ ScreenFader: FOUND");
            report.AppendLine($"  Is Fading: {ScreenFader.Instance.IsFading()}");
        }
        else
        {
            report.AppendLine("  ❌ ScreenFader: NOT FOUND (will cause errors)");
        }
        report.AppendLine();

        // 6. Check for Missing Scripts
        report.AppendLine("【6】 MISSING SCRIPT CHECK");
        MonoBehaviour[] allScripts = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        int missingCount = 0;
        foreach (MonoBehaviour script in allScripts)
        {
            if (script == null)
            {
                missingCount++;
            }
        }
        if (missingCount > 0)
        {
            report.AppendLine($"  ⚠ Found {missingCount} missing script(s)");
            report.AppendLine("  Check Console for 'Missing Script' errors");
        }
        else
        {
            report.AppendLine("  ✓ No missing scripts detected");
        }
        report.AppendLine();

        // 7. Recommendations
        report.AppendLine("【7】 RECOMMENDATIONS");
        if (player == null)
        {
            report.AppendLine("  ❌ CRITICAL: Player not found - check scene setup");
        }
        else
        {
            JoystickPlayerController controller = player.GetComponent<JoystickPlayerController>();
            if (controller == null || !controller.enabled)
            {
                report.AppendLine("  ❌ CRITICAL: Player controller disabled - press E to fix");
            }
        }

        if (joystick == null)
        {
            report.AppendLine("  ❌ CRITICAL: Joystick not found - check scene setup");
        }

        if (ScreenFader.Instance == null)
        {
            report.AppendLine("  ⚠ WARNING: Add ScreenFader to scene");
        }

        if (foundBlockingUI)
        {
            report.AppendLine("  ⚠ WARNING: Close blocking UI - press R to resume");
        }

        report.AppendLine();
        report.AppendLine("╔════════════════════════════════════════╗");
        report.AppendLine("║   DIAGNOSTIC COMPLETE                 ║");
        report.AppendLine("╚════════════════════════════════════════╝");
        report.AppendLine();
        report.AppendLine("HOTKEYS:");
        report.AppendLine("  D = Run diagnostics");
        report.AppendLine("  E = Force enable player");
        report.AppendLine("  L = Log player state");
        report.AppendLine("  R = Resume from UI");

        Debug.Log(report.ToString());
    }
}
