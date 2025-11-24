using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DemoEndTrigger : MonoBehaviour
{
    [Tooltip("Ensures this triggers only once.")]
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            if (DemoEndUI.Instance != null)
            {
                Debug.Log("[DemoEndTrigger] Player finished the demo!");
                hasTriggered = true;
                DemoEndUI.Instance.ShowDemoEnd();
            }
            else
            {
                Debug.LogError("[DemoEndTrigger] DemoEndUI not found in scene!");
            }
        }
    }
}