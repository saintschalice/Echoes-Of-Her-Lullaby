using UnityEngine;

public class LightingOptimizer : MonoBehaviour
{
    [Header("Performance")]
    public int maxLights = 3; // Limit active lights
    public float cullingDistance = 15f;

    private Light[] sceneLights;
    private Transform player;

    void Start()
    {
        // Updated method - no sorting needed for performance
        sceneLights = FindObjectsByType<Light>(FindObjectsSortMode.None);

        // Find player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player != null)
            OptimizeLights();
    }

    void OptimizeLights()
    {
        foreach (Light light in sceneLights)
        {
            if (light == null) continue;

            float distance = Vector3.Distance(player.position, light.transform.position);
            light.enabled = distance <= cullingDistance;
        }
    }
}