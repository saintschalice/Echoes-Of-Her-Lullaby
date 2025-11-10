// In RebakeOnStart.cs

using UnityEngine;
using System.Collections; // Add this

public class RebakeOnStart : MonoBehaviour
{
    private NavMesh2DCollector collector;
    public bool IsBakeComplete { get; private set; } = false; // Add this

    void Start()
    {
        collector = GetComponent<NavMesh2DCollector>();

        if (collector != null)
        {
            // We no longer auto-bake. We wait for another script to call it.
            StartCoroutine(BakeAfterDelay()); 
        }
    }

    // Make this public so the trigger can call it
    private IEnumerator BakeAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);

        Debug.Log("[RebakeOnStart] Rebaking NavMesh...");
        if (collector != null)
            collector.Bake2DNavMesh();

        yield return new WaitForSeconds(0.5f); // allow registration time

        Debug.Log("[RebakeOnStart] NavMesh rebake complete!");

        // Trigger Emily activation AFTER bake is safely registered
        if (PersistentEmilyManager.Instance != null)
            PersistentEmilyManager.Instance.ActivateEmily();
    }

}