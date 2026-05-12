using UnityEngine;

/// <summary>
/// Quick test script - Press T to test jumpscare
/// Check Console for detailed info
/// </summary>
public class QuickJumpscareTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("========================================");
            Debug.Log("QUICK JUMPSCARE TEST - Press T");
            Debug.Log("========================================");
            
            // Check if Instance exists
            if (JumpscareManager.Instance == null)
            {
                Debug.LogError("❌ JumpscareManager.Instance is NULL!");
                Debug.LogError("→ JumpscareManager GameObject not found or Awake() not called");
                
                // Try to find it manually
                JumpscareManager found = FindFirstObjectByType<JumpscareManager>();
                if (found != null)
                {
                    Debug.LogWarning("⚠️ Found JumpscareManager in scene but Instance is null!");
                    Debug.LogWarning($"→ GameObject: {found.gameObject.name}");
                    Debug.LogWarning($"→ Scene: {found.gameObject.scene.name}");
                    Debug.LogWarning($"→ Active: {found.gameObject.activeSelf}");
                    Debug.LogWarning($"→ Enabled: {found.enabled}");
                }
                else
                {
                    Debug.LogError("❌ No JumpscareManager found in any scene!");
                }
                return;
            }
            
            Debug.Log("✅ JumpscareManager.Instance EXISTS!");
            Debug.Log($"→ GameObject: {JumpscareManager.Instance.gameObject.name}");
            Debug.Log($"→ Scene: {JumpscareManager.Instance.gameObject.scene.name}");
            
            // Check references
            Debug.Log("\nChecking References:");
            Debug.Log($"Panel: {(JumpscareManager.Instance.jumpscarePanel != null ? "✅" : "❌ NULL")}");
            Debug.Log($"Image: {(JumpscareManager.Instance.jumpscareImage != null ? "✅" : "❌ NULL")}");
            Debug.Log($"Tilt Left: {(JumpscareManager.Instance.tiltLeftSprite != null ? "✅" : "❌ NULL")}");
            Debug.Log($"Tilt Right: {(JumpscareManager.Instance.tiltRightSprite != null ? "✅" : "❌ NULL")}");
            Debug.Log($"Center: {(JumpscareManager.Instance.centerSprite != null ? "✅" : "❌ NULL")}");
            
            // Trigger test
            Debug.Log("\n🎬 TRIGGERING TEST JUMPSCARE...");
            JumpscareManager.Instance.TriggerJumpscare("TEST - Press T");
        }
    }
    
    void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 24;
        style.normal.textColor = Color.yellow;
        
        GUI.Label(new Rect(10, 10, 400, 30), "Press T to Test Jumpscare", style);
        
        style.fontSize = 18;
        if (JumpscareManager.Instance == null)
        {
            style.normal.textColor = Color.red;
            GUI.Label(new Rect(10, 45, 500, 25), "❌ JumpscareManager.Instance is NULL!", style);
        }
        else
        {
            style.normal.textColor = Color.green;
            GUI.Label(new Rect(10, 45, 500, 25), "✅ JumpscareManager Ready", style);
        }
    }
}
