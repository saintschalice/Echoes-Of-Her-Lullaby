using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Enhanced diagnostic tool for jumpscare system
/// Press D for detailed diagnostic
/// Press J to test jumpscare
/// Press I to show instance info
/// </summary>
public class JumpscareDebugHelper : MonoBehaviour
{
    void Update()
    {
        // Press D for full diagnostic
        if (Input.GetKeyDown(KeyCode.D))
        {
            RunFullDiagnostic();
        }
        
        // Press J to test jumpscare
        if (Input.GetKeyDown(KeyCode.J))
        {
            TestJumpscare();
        }
        
        // Press I for instance info
        if (Input.GetKeyDown(KeyCode.I))
        {
            CheckInstance();
        }
    }
    
    void RunFullDiagnostic()
    {
        Debug.Log("╔════════════════════════════════════════════════════════════╗");
        Debug.Log("║         JUMPSCARE SYSTEM - FULL DIAGNOSTIC                 ║");
        Debug.Log("╚════════════════════════════════════════════════════════════╝");
        
        // 1. Check Instance
        Debug.Log("\n[1] CHECKING INSTANCE:");
        if (JumpscareManager.Instance == null)
        {
            Debug.LogError("   ❌ JumpscareManager.Instance is NULL!");
            Debug.LogError("   → JumpscareManager GameObject not in scene or Awake() not called");
            Debug.LogError("   → Check if JumpscareManager exists in PersistentScene");
            
            // Try to find it manually
            JumpscareManager found = FindFirstObjectByType<JumpscareManager>();
            if (found != null)
            {
                Debug.LogWarning("   ⚠️ Found JumpscareManager in scene but Instance is null!");
                Debug.LogWarning("   → This means Awake() hasn't run yet or was destroyed");
            }
            else
            {
                Debug.LogError("   ❌ No JumpscareManager found in scene at all!");
                Debug.LogError("   → You need to create JumpscareManager GameObject in PersistentScene");
            }
            
            Debug.Log("\n╔════════════════════════════════════════════════════════════╗");
            Debug.Log("║  DIAGNOSTIC STOPPED - Fix Instance issue first!            ║");
            Debug.Log("╚════════════════════════════════════════════════════════════╝");
            return;
        }
        
        Debug.Log("   ✅ JumpscareManager.Instance exists!");
        Debug.Log($"   → GameObject: {JumpscareManager.Instance.gameObject.name}");
        Debug.Log($"   → Scene: {JumpscareManager.Instance.gameObject.scene.name}");
        
        // 2. Check References
        Debug.Log("\n[2] CHECKING REFERENCES:");
        
        var manager = JumpscareManager.Instance;
        
        // Panel
        if (manager.jumpscarePanel == null)
        {
            Debug.LogError("   ❌ jumpscarePanel is NULL!");
            Debug.LogError("   → Assign JumpscarePanel in Inspector");
        }
        else
        {
            Debug.Log($"   ✅ jumpscarePanel: {manager.jumpscarePanel.name}");
            Debug.Log($"      → Active: {manager.jumpscarePanel.activeSelf}");
            Debug.Log($"      → Scene: {manager.jumpscarePanel.scene.name}");
            
            // Check if panel is in Canvas
            Canvas canvas = manager.jumpscarePanel.GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                Debug.Log($"      → Canvas: {canvas.name}");
                Debug.Log($"      → Canvas Sort Order: {canvas.sortingOrder}");
                if (canvas.sortingOrder < 1000)
                {
                    Debug.LogWarning("      ⚠️ Canvas sort order is low! Should be 1000+");
                }
            }
            else
            {
                Debug.LogError("      ❌ Panel is not child of Canvas!");
            }
        }
        
        // Image
        if (manager.jumpscareImage == null)
        {
            Debug.LogError("   ❌ jumpscareImage is NULL!");
            Debug.LogError("   → Assign JumpscareImage in Inspector");
        }
        else
        {
            Debug.Log($"   ✅ jumpscareImage: {manager.jumpscareImage.name}");
            Debug.Log($"      → Enabled: {manager.jumpscareImage.enabled}");
            Debug.Log($"      → GameObject Active: {manager.jumpscareImage.gameObject.activeSelf}");
        }
        
        // Sprites
        if (manager.tiltLeftSprite == null)
        {
            Debug.LogError("   ❌ tiltLeftSprite is NULL!");
        }
        else
        {
            Debug.Log($"   ✅ tiltLeftSprite: {manager.tiltLeftSprite.name}");
        }
        
        if (manager.tiltRightSprite == null)
        {
            Debug.LogError("   ❌ tiltRightSprite is NULL!");
        }
        else
        {
            Debug.Log($"   ✅ tiltRightSprite: {manager.tiltRightSprite.name}");
        }
        
        if (manager.centerSprite == null)
        {
            Debug.LogError("   ❌ centerSprite is NULL!");
        }
        else
        {
            Debug.Log($"   ✅ centerSprite: {manager.centerSprite.name}");
        }
        
        // Audio
        if (manager.jumpscareSound == null)
        {
            Debug.LogWarning("   ⚠️ jumpscareSound is NULL (optional but recommended)");
        }
        else
        {
            Debug.Log($"   ✅ jumpscareSound: {manager.jumpscareSound.name}");
            Debug.Log($"      → Length: {manager.jumpscareSound.length:F2}s");
        }
        
        // Flash Image
        if (manager.flashImage == null)
        {
            Debug.LogWarning("   ⚠️ flashImage is NULL (optional)");
        }
        else
        {
            Debug.Log($"   ✅ flashImage: {manager.flashImage.name}");
        }
        
        // 3. Count Issues
        Debug.Log("\n[3] SUMMARY:");
        int criticalIssues = 0;
        int warnings = 0;
        
        if (manager.jumpscarePanel == null) criticalIssues++;
        if (manager.jumpscareImage == null) criticalIssues++;
        if (manager.tiltLeftSprite == null) criticalIssues++;
        if (manager.tiltRightSprite == null) criticalIssues++;
        if (manager.centerSprite == null) criticalIssues++;
        
        if (manager.jumpscareSound == null) warnings++;
        if (manager.flashImage == null) warnings++;
        
        if (manager.jumpscarePanel != null)
        {
            Canvas canvas = manager.jumpscarePanel.GetComponentInParent<Canvas>();
            if (canvas != null && canvas.sortingOrder < 1000) warnings++;
        }
        
        Debug.Log($"   Critical Issues: {criticalIssues}");
        Debug.Log($"   Warnings: {warnings}");
        
        if (criticalIssues > 0)
        {
            Debug.LogError("\n   ❌ JUMPSCARE WILL NOT WORK!");
            Debug.LogError("   → Fix all critical issues (marked with ❌)");
            Debug.LogError("   → Assign missing references in Inspector");
        }
        else if (warnings > 0)
        {
            Debug.LogWarning("\n   ⚠️ JUMPSCARE MAY HAVE ISSUES");
            Debug.LogWarning("   → Fix warnings for best experience");
        }
        else
        {
            Debug.Log("\n   ✅ ALL CHECKS PASSED!");
            Debug.Log("   → Jumpscare should work correctly");
            Debug.Log("   → Press J to test jumpscare");
        }
        
        // 4. Check GameOverManager
        Debug.Log("\n[4] CHECKING GAME OVER MANAGER:");
        if (GameOverManager.Instance == null)
        {
            Debug.LogWarning("   ⚠️ GameOverManager.Instance is NULL");
            Debug.LogWarning("   → Jumpscare will play but game over won't show after");
        }
        else
        {
            Debug.Log("   ✅ GameOverManager.Instance exists");
        }
        
        Debug.Log("\n╔════════════════════════════════════════════════════════════╗");
        Debug.Log("║  DIAGNOSTIC COMPLETE                                       ║");
        Debug.Log("╚════════════════════════════════════════════════════════════╝");
    }
    
    void TestJumpscare()
    {
        Debug.Log("\n╔════════════════════════════════════════════════════════════╗");
        Debug.Log("║  TESTING JUMPSCARE                                         ║");
        Debug.Log("╚════════════════════════════════════════════════════════════╝");
        
        if (JumpscareManager.Instance == null)
        {
            Debug.LogError("❌ Cannot test - JumpscareManager.Instance is NULL!");
            Debug.LogError("→ Run diagnostic first (press D)");
            return;
        }
        
        Debug.Log("✅ Triggering test jumpscare...");
        JumpscareManager.Instance.TriggerJumpscare("TEST JUMPSCARE");
    }
    
    void CheckInstance()
    {
        Debug.Log("\n╔════════════════════════════════════════════════════════════╗");
        Debug.Log("║  INSTANCE CHECK                                            ║");
        Debug.Log("╚════════════════════════════════════════════════════════════╝");
        
        Debug.Log($"JumpscareManager.Instance: {(JumpscareManager.Instance != null ? "✅ EXISTS" : "❌ NULL")}");
        
        if (JumpscareManager.Instance != null)
        {
            Debug.Log($"GameObject: {JumpscareManager.Instance.gameObject.name}");
            Debug.Log($"Scene: {JumpscareManager.Instance.gameObject.scene.name}");
            Debug.Log($"Active: {JumpscareManager.Instance.gameObject.activeSelf}");
            Debug.Log($"Enabled: {JumpscareManager.Instance.enabled}");
        }
        else
        {
            // Try to find manually
            JumpscareManager found = FindFirstObjectByType<JumpscareManager>();
            if (found != null)
            {
                Debug.LogWarning("⚠️ Found JumpscareManager but Instance is null!");
                Debug.LogWarning($"GameObject: {found.gameObject.name}");
                Debug.LogWarning($"Scene: {found.gameObject.scene.name}");
                Debug.LogWarning("→ Awake() may not have run yet");
            }
            else
            {
                Debug.LogError("❌ No JumpscareManager found in scene!");
                Debug.LogError("→ Create JumpscareManager GameObject in PersistentScene");
            }
        }
        
        Debug.Log("\n╚════════════════════════════════════════════════════════════╝");
    }
    
    void OnGUI()
    {
        // Show instructions on screen
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 20;
        style.normal.textColor = Color.yellow;
        
        GUI.Label(new Rect(10, 10, 500, 30), "JUMPSCARE DEBUG:", style);
        
        style.fontSize = 16;
        style.normal.textColor = Color.white;
        
        GUI.Label(new Rect(10, 40, 500, 25), "Press D - Full Diagnostic", style);
        GUI.Label(new Rect(10, 65, 500, 25), "Press J - Test Jumpscare", style);
        GUI.Label(new Rect(10, 90, 500, 25), "Press I - Instance Info", style);
        
        // Show instance status
        if (JumpscareManager.Instance == null)
        {
            style.normal.textColor = Color.red;
            GUI.Label(new Rect(10, 120, 500, 25), "❌ JumpscareManager.Instance is NULL!", style);
        }
        else
        {
            style.normal.textColor = Color.green;
            GUI.Label(new Rect(10, 120, 500, 25), "✅ JumpscareManager.Instance OK", style);
        }
    }
}
