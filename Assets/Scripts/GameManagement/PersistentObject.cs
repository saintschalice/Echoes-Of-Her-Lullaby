using UnityEngine;
using System.Collections.Generic;

public class PersistentObject : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("If true, this object will persist across all scene loads")]
    public bool persist = true;

    // Track which objects have already been marked as persistent
    private static HashSet<int> persistentObjects = new HashSet<int>();

    void Awake()
    {
        if (persist)
        {
            // CRITICAL FIX: Check if object is already in DontDestroyOnLoad scene
            if (gameObject.scene.name == "DontDestroyOnLoad")
            {
                Debug.Log($"[PersistentObject] {gameObject.name} already in DontDestroyOnLoad, skipping.");
                return;
            }

            int instanceID = gameObject.GetInstanceID();

            // Check if this specific object instance is already marked as persistent
            if (persistentObjects.Contains(instanceID))
            {
                // Already called DontDestroyOnLoad on this object, skip
                Debug.Log($"[PersistentObject] {gameObject.name} (ID: {instanceID}) already marked as persistent, skipping.");
                return;
            }

            // Mark this object as persistent
            DontDestroyOnLoad(gameObject);
            persistentObjects.Add(instanceID);
            Debug.Log($"[PersistentObject] {gameObject.name} (ID: {instanceID}) marked as persistent.");
        }
    }

    void OnDestroy()
    {
        // Clean up when object is destroyed
        if (persist)
        {
            int instanceID = gameObject.GetInstanceID();
            persistentObjects.Remove(instanceID);
        }
    }
}