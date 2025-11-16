using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public sealed class EmilySpawnTrigger : MonoBehaviour
{
    public EmilyGhost emilyPrefab;               // drag the prefab here
    public Transform spawnPoint;                // where she appears
    EmilyGhost _instance;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        if (_instance != null) return;           // already awake

        _instance = Instantiate(emilyPrefab, spawnPoint.position, Quaternion.identity);

        _instance.transform.position = new Vector3(
            spawnPoint.position.x,
            spawnPoint.position.y,
            0f
        );

        _instance.gameObject.SetActive(true);    // wake up!
        Destroy(gameObject);                     // one-shot trigger

        Debug.Log("[EMILY SPAWN] Spawned at " + spawnPoint.position);

    }
}
