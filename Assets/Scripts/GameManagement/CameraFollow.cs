using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target; // Lisa (player character)
    public float followSpeed = 5f;
    public Vector3 offset = new Vector3(0, 0, -10);

    [Header("Tilemap Boundary Settings")]
    public bool useTilemapBoundaries = true;
    public Tilemap[] boundaryTilemaps;
    public float boundaryPadding = 1f;

    [Header("Manual Boundary Override")]
    public bool useManualBoundaries = false;
    public float manualMinX = -10f;
    public float manualMaxX = 10f;
    public float manualMinY = -5f;
    public float manualMaxY = 5f;

    [Header("Smoothing")]
    public bool useSmoothing = true;
    public float smoothTime = 0.3f;

    // Internal variables
    private Vector3 velocity = Vector3.zero;
    private float cameraHalfHeight;
    private float cameraHalfWidth;
    private Bounds tilemapBounds;

    // Public boundary properties (for external access)
    public float minX { get; private set; }
    public float maxX { get; private set; }
    public float minY { get; private set; }
    public float maxY { get; private set; }

    void Start()
    {
        // Find target if not assigned
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }

        // Calculate camera dimensions
        CalculateCameraSize();

        // Set up boundaries
        if (useTilemapBoundaries)
        {
            UpdateTilemapBoundaries();
        }
        else if (useManualBoundaries)
        {
            SetManualBoundaries();
        }
    }

    void Update()
    {
        // Recalculate camera size if aspect ratio changes (for testing different ratios)
        Camera cam = GetComponent<Camera>();
        float currentHalfWidth = cam.orthographicSize * cam.aspect;

        if (Mathf.Abs(currentHalfWidth - cameraHalfWidth) > 0.1f)
        {
            Debug.Log($"Aspect ratio changed from {cameraHalfWidth / cam.orthographicSize:F2} to {cam.aspect:F2}");
            CalculateCameraSize();

            if (useTilemapBoundaries)
            {
                SetBoundariesFromTilemap();
            }
        }
    }

    void CalculateCameraSize()
    {
        Camera cam = GetComponent<Camera>();
        if (cam.orthographic)
        {
            cameraHalfHeight = cam.orthographicSize;
            cameraHalfWidth = cameraHalfHeight * cam.aspect;
        }
        else
        {
            float distance = Mathf.Abs(transform.position.z);
            cameraHalfHeight = distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            cameraHalfWidth = cameraHalfHeight * cam.aspect;
        }

        Debug.Log($"Camera size calculated - Aspect: {cam.aspect:F2}, Half Width: {cameraHalfWidth:F2}, Half Height: {cameraHalfHeight:F2}");
    }

    void UpdateTilemapBoundaries()
    {
        if (boundaryTilemaps == null || boundaryTilemaps.Length == 0)
        {
            // Auto-find tilemaps if none assigned
            AutoFindTilemaps();
        }

        if (boundaryTilemaps.Length > 0)
        {
            tilemapBounds = CalculateCombinedTilemapBounds();
            SetBoundariesFromTilemap();
        }
    }

    void AutoFindTilemaps()
    {
        Tilemap[] allTilemaps = FindObjectsOfType<Tilemap>();
        System.Collections.Generic.List<Tilemap> validTilemaps = new System.Collections.Generic.List<Tilemap>();

        foreach (Tilemap tilemap in allTilemaps)
        {
            // Only include tilemaps that have tiles (non-empty)
            if (tilemap.cellBounds.size.x > 0 && tilemap.cellBounds.size.y > 0)
            {
                // Exclude UI or overlay tilemaps (you can customize this filter)
                if (!tilemap.gameObject.name.ToLower().Contains("ui") &&
                    !tilemap.gameObject.name.ToLower().Contains("overlay"))
                {
                    validTilemaps.Add(tilemap);
                }
            }
        }

        boundaryTilemaps = validTilemaps.ToArray();
        Debug.Log($"Auto-found {boundaryTilemaps.Length} tilemaps for camera boundaries");
    }

    Bounds CalculateCombinedTilemapBounds()
    {
        if (boundaryTilemaps.Length == 0) return new Bounds();

        Bounds combinedBounds = GetTilemapWorldBounds(boundaryTilemaps[0]);

        for (int i = 1; i < boundaryTilemaps.Length; i++)
        {
            Bounds tileBounds = GetTilemapWorldBounds(boundaryTilemaps[i]);
            combinedBounds.Encapsulate(tileBounds);
        }

        return combinedBounds;
    }

    Bounds GetTilemapWorldBounds(Tilemap tilemap)
    {
        BoundsInt cellBounds = tilemap.cellBounds;
        Vector3 min = tilemap.CellToWorld(new Vector3Int(cellBounds.xMin, cellBounds.yMin, 0));
        Vector3 max = tilemap.CellToWorld(new Vector3Int(cellBounds.xMax, cellBounds.yMax, 0));
        return new Bounds((min + max) * 0.5f, max - min);
    }

    void SetBoundariesFromTilemap()
    {
        // Calculate boundaries ensuring camera doesn't show empty space
        minX = tilemapBounds.min.x + cameraHalfWidth + boundaryPadding;
        maxX = tilemapBounds.max.x - cameraHalfWidth - boundaryPadding;
        minY = tilemapBounds.min.y + cameraHalfHeight + boundaryPadding;
        maxY = tilemapBounds.max.y - cameraHalfHeight - boundaryPadding;

        // Prevent invalid boundaries
        if (minX > maxX)
        {
            float centerX = tilemapBounds.center.x;
            minX = centerX - 0.1f;
            maxX = centerX + 0.1f;
        }
        if (minY > maxY)
        {
            float centerY = tilemapBounds.center.y;
            minY = centerY - 0.1f;
            maxY = centerY + 0.1f;
        }

        Debug.Log($"Tilemap boundaries set: X({minX:F2} to {maxX:F2}), Y({minY:F2} to {maxY:F2})");
        Debug.Log($"Tilemap bounds: min({tilemapBounds.min.x:F2}, {tilemapBounds.min.y:F2}), max({tilemapBounds.max.x:F2}, {tilemapBounds.max.y:F2})");
        Debug.Log($"Camera half-width: {cameraHalfWidth:F2}, boundary padding: {boundaryPadding:F2}");
    }

    void SetManualBoundaries()
    {
        minX = manualMinX;
        maxX = manualMaxX;
        minY = manualMinY;
        maxY = manualMaxY;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Calculate desired position
        Vector3 desiredPosition = target.position + offset;

        // Apply boundaries
        if (useTilemapBoundaries || useManualBoundaries)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        }

        // Move camera
        if (useSmoothing)
        {
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        }
    }

    // Public methods for external control
    public void SetBoundaries(float newMinX, float newMaxX, float newMinY, float newMaxY)
    {
        minX = newMinX;
        maxX = newMaxX;
        minY = newMinY;
        maxY = newMaxY;
        useManualBoundaries = true;
        useTilemapBoundaries = false;
    }

    public void RefreshTilemapBoundaries()
    {
        if (useTilemapBoundaries)
        {
            UpdateTilemapBoundaries();
        }
    }

    public void SwitchToTilemapMode(Tilemap[] newTilemaps = null)
    {
        useTilemapBoundaries = true;
        useManualBoundaries = false;

        if (newTilemaps != null)
            boundaryTilemaps = newTilemaps;

        UpdateTilemapBoundaries();
    }

    // Visualize boundaries in Scene view
    void OnDrawGizmosSelected()
    {
        if (useTilemapBoundaries || useManualBoundaries)
        {
            Gizmos.color = Color.cyan;
            Vector3 center = new Vector3((minX + maxX) * 0.5f, (minY + maxY) * 0.5f, transform.position.z);
            Vector3 size = new Vector3(maxX - minX, maxY - minY, 0);
            Gizmos.DrawWireCube(center, size);

            // Draw camera bounds
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, new Vector3(cameraHalfWidth * 2, cameraHalfHeight * 2, 0));
        }
    }
}