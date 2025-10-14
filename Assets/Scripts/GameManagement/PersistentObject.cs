using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("If true, this object will persist across all scene loads")]
    public bool persist = true;

    void Awake()
    {
        if (persist)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}