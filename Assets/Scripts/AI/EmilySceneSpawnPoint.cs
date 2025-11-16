using UnityEngine;

public class EmilySceneSpawnPoint : MonoBehaviour
{
    void Start()
    {
        var emily = FindFirstObjectByType<EmilyGhost>();
        if (emily != null)
            emily.transform.position = transform.position;
    }
}
