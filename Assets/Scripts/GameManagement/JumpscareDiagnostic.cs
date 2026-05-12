using UnityEngine;

/// <summary>
/// Diagnostic script to test JumpscareManager
/// Add this to any GameObject and press J to test jumpscare
/// Press D to show diagnostic info
/// </summary>
public class JumpscareDiagnostic : MonoBehaviour
{
    [Header("Test Settings")]
    [Tooltip("Press this key to trigger test jumpscare")]
    public KeyCode testKey = KeyCode.J;
    
    [Tooltip("Press this key to show diagnostic info")]
    public KeyCode diagnosticKey = KeyCode.D;
    
    [Tooltip("Test message to show")]
    public string testMessage = "TEST JUMPSCARE";

    private void Update()
    {
        // Test jumpscare
        if (Input.GetKeyDown(testKey))
        {
            TestJumpscare();
        }
        
        // Show diagnostic
        if (Input.GetKeyDown(diagnosticKey))
        {
            ShowDiagnostic();
        }
    }

    private void TestJumpscare()
    {
        Debug.Log("=== JUMPSCARE TEST ===");
        
        if (JumpscareManager.Instance != null)
        {
            Debug.Log("✅ JumpscareManager found!");
            Debug.Log("🎬 Triggering test jumpscare...");
            JumpscareManager.Instance.TriggerJumpscare(testMessage);
        }
        else
        {
            Debug.LogError("❌ JumpscareManager.Instance is NULL!");
            Debug.LogError("Make sure JumpscareManager GameObject exists in scene!");
        }
    }

    private void ShowDiagnostic()
    {
        Debug.Log("=== JUMPSCARE DIAGNOSTIC ===");
        
        // Check JumpscareManager
        if (JumpscareManager.Instance != null)
        {
            Debug.Log("✅ JumpscareManager Instance: FOUND");
            
            JumpscareManager manager = JumpscareManager.Instance;
            
            // Use reflection to check private fields
            var type = typeof(JumpscareManager);
            
            // Check public fields
            Debug.Log($"Jumpscare Panel: {GetFieldStatus(manager, "jumpscarePanel")}");
            Debug.Log($"Jumpscare Image: {GetFieldStatus(manager, "jumpscareImage")}");
            Debug.Log($"Tilt Left Sprite: {GetFieldStatus(manager, "tiltLeftSprite")}");
            Debug.Log($"Tilt Right Sprite: {GetFieldStatus(manager, "tiltRightSprite")}");
            Debug.Log($"Center Sprite: {GetFieldStatus(manager, "centerSprite")}");
            Debug.Log($"Jumpscare Sound: {GetFieldStatus(manager, "jumpscareSound")}");
            Debug.Log($"Flash Image: {GetFieldStatus(manager, "flashImage")}");
        }
        else
        {
            Debug.LogError("❌ JumpscareManager Instance: NULL");
        }
        
        // Check GameOverManager
        if (GameOverManager.Instance != null)
        {
            Debug.Log("✅ GameOverManager Instance: FOUND");
        }
        else
        {
            Debug.LogWarning("⚠️ GameOverManager Instance: NULL");
        }
        
        // Check Canvas
        Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        Debug.Log($"📊 Total Canvases in scene: {canvases.Length}");
        
        foreach (Canvas canvas in canvases)
        {
            Debug.Log($"  - Canvas: {canvas.name}, Sort Order: {canvas.sortingOrder}, Render Mode: {canvas.renderMode}");
        }
        
        Debug.Log("============================");
    }

    private string GetFieldStatus(object obj, string fieldName)
    {
        var field = obj.GetType().GetField(fieldName);
        if (field != null)
        {
            var value = field.GetValue(obj);
            return value != null ? "✅ Assigned" : "❌ NULL";
        }
        return "⚠️ Field not found";
    }

    private void OnGUI()
    {
        // Show instructions on screen
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.yellow;
        
        GUI.Label(new Rect(10, 10, 500, 30), $"Press [{testKey}] to test jumpscare", style);
        GUI.Label(new Rect(10, 40, 500, 30), $"Press [{diagnosticKey}] to show diagnostic", style);
        
        // Show status
        style.fontSize = 16;
        if (JumpscareManager.Instance != null)
        {
            style.normal.textColor = Color.green;
            GUI.Label(new Rect(10, 70, 500, 30), "✅ JumpscareManager: READY", style);
        }
        else
        {
            style.normal.textColor = Color.red;
            GUI.Label(new Rect(10, 70, 500, 30), "❌ JumpscareManager: NOT FOUND", style);
        }
    }
}
