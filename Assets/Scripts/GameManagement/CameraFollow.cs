using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target;
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

    [Header("Auto-Refresh")]
    public bool autoRefreshOnSceneLoad = true;

    // Internal variables
    private Vector3 velocity = Vector3.zero;
    private float cameraHalfHeight;
    private float cameraHalfWidth;
    private Bounds tilemapBounds;
    private bool roomSmallerThanCamera = false;
    private Vector3 lockedCameraPosition;
    private string lastSceneName = "";

    // Public boundary properties
    public float minX { get; private set; }
    public float maxX { get; private set; }
    public float minY { get; private set; }
    public float maxY { get; private set; }

    void Awake()
    {
        if (autoRefreshOnSceneLoad)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[CameraFollow] Scene loaded: {scene.name}, refreshing boundaries...");

        // Wait a frame for tilemaps to be ready
        StartCoroutine(RefreshBoundariesDelayed());
    }

    System.Collections.IEnumerator RefreshBoundariesDelayed()
    {
        // Wait a frame for tilemaps/player to be ready
        yield return new WaitForEndOfFrame();

        // 1. Calculate and set boundaries
        if (useTilemapBoundaries)
        {
            UpdateTilemapBoundaries();
        }
        else if (useManualBoundaries)
        {
            SetManualBoundaries();
        }

        // 2. Ensure target is found again, in case it loaded late
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }

        // 3. Snap camera to the target's initial position + offset
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;

            // Apply boundary constraints for the initial snap,
            // crucial if the player spawns near an edge.
            if (useTilemapBoundaries || useManualBoundaries)
            {
                if (roomSmallerThanCamera)
                {
                    // Use the locked center position
                    desiredPosition = lockedCameraPosition;
                }
                else
                {
                    // Clamp to the calculated boundaries
                    desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
                    desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
                }
            }

            // Snap the position immediately
            transform.position = desiredPosition;
            velocity = Vector3.zero; // Reset velocity to prevent immediate "snap back" due to smoothing

            //Debug.Log($"[CameraFollow] Initial snap position: {transform.position}");
        }
    }

    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }

        CalculateCameraSize();

        // NEW: Call the delayed refresh here to handle initial positioning
        // and boundaries after the first frame.
        StartCoroutine(RefreshBoundariesDelayed());
        // ---------------------------------------------------------------------

        // Removed the old boundary/manual boundary checks here, 
        // as they are now handled inside RefreshBoundariesDelayed

        lastSceneName = SceneManager.GetActiveScene().name;
    }

    void Update()
    {
        Camera cam = GetComponent<Camera>();
        float currentHalfWidth = cam.orthographicSize * cam.aspect;

        if (Mathf.Abs(currentHalfWidth - cameraHalfWidth) > 0.1f)
        {
            //Debug.Log($"[CameraFollow] Aspect ratio changed from {cameraHalfWidth / cam.orthographicSize:F2} to {cam.aspect:F2}");
            CalculateCameraSize();

            if (useTilemapBoundaries)
            {
                SetBoundariesFromTilemap();
            }
        }

        // Check if scene changed (for persistent cameras)
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != lastSceneName)
        {
            lastSceneName = currentScene;
            Debug.Log($"[CameraFollow] Scene changed to: {currentScene}");
            if (useTilemapBoundaries)
            {
                UpdateTilemapBoundaries();
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

       // Debug.Log($"[CameraFollow] Camera size - Half Width: {cameraHalfWidth:F2}, Half Height: {cameraHalfHeight:F2}");
    }

    void UpdateTilemapBoundaries()
    {
        // Always refresh tilemaps from current scene
        AutoFindTilemaps();

        if (boundaryTilemaps != null && boundaryTilemaps.Length > 0)
        {
            tilemapBounds = CalculateCombinedTilemapBounds();
            SetBoundariesFromTilemap();
        }
        else
        {
            Debug.LogWarning("[CameraFollow] No tilemaps found! Camera will be unbounded.");
        }
    }

    void AutoFindTilemaps()
    {
        Tilemap[] allTilemaps = FindObjectsOfType<Tilemap>();
        System.Collections.Generic.List<Tilemap> validTilemaps = new System.Collections.Generic.List<Tilemap>();

        foreach (Tilemap tilemap in allTilemaps)
        {
            if (tilemap.cellBounds.size.x > 0 && tilemap.cellBounds.size.y > 0)
            {
                // Exclude UI and overlay tilemaps
                if (!tilemap.gameObject.name.ToLower().Contains("ui") &&
                    !tilemap.gameObject.name.ToLower().Contains("overlay"))
                {
                    validTilemaps.Add(tilemap);
                    Debug.Log($"[CameraFollow] Found tilemap: {tilemap.gameObject.name} ({tilemap.cellBounds.size})");
                }
            }
        }

        boundaryTilemaps = validTilemaps.ToArray();
        Debug.Log($"[CameraFollow] Auto-found {boundaryTilemaps.Length} valid tilemaps for boundaries");
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

        //Debug.Log($"[CameraFollow] Combined tilemap bounds: Center({combinedBounds.center}), Size({combinedBounds.size})");
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
        float tilemapWidth = tilemapBounds.size.x;
        float tilemapHeight = tilemapBounds.size.y;

        // Check if room is smaller than camera view
        bool xTooSmall = tilemapWidth <= (cameraHalfWidth * 2 + boundaryPadding * 2);
        bool yTooSmall = tilemapHeight <= (cameraHalfHeight * 2 + boundaryPadding * 2);

        roomSmallerThanCamera = xTooSmall || yTooSmall;

        if (roomSmallerThanCamera)
        {
            // Lock camera to center of room
            lockedCameraPosition = new Vector3(
                tilemapBounds.center.x,
                tilemapBounds.center.y,
                offset.z
            );

            //Debug.Log($"[CameraFollow] Room smaller than camera! Locking at center: {lockedCameraPosition}");

            // Set boundaries to center
            minX = maxX = tilemapBounds.center.x;
            minY = maxY = tilemapBounds.center.y;
        }
        else
        {
            // Normal boundary calculation
            minX = tilemapBounds.min.x + cameraHalfWidth + boundaryPadding;
            maxX = tilemapBounds.max.x - cameraHalfWidth - boundaryPadding;
            minY = tilemapBounds.min.y + cameraHalfHeight + boundaryPadding;
            maxY = tilemapBounds.max.y - cameraHalfHeight - boundaryPadding;

            // Safety check
            if (minX > maxX)
            {
                float centerX = tilemapBounds.center.x;
                minX = maxX = centerX;
               // Debug.LogWarning($"[CameraFollow] X boundaries invalid, centering at {centerX}");
            }
            if (minY > maxY)
            {
                float centerY = tilemapBounds.center.y;
                minY = maxY = centerY;
                Debug.LogWarning($"[CameraFollow] Y boundaries invalid, centering at {centerY}");
            }

          //  Debug.Log($"[CameraFollow] Boundaries set: X({minX:F2} to {maxX:F2}), Y({minY:F2} to {maxY:F2})");
        }
    }

    void SetManualBoundaries()
    {
        minX = manualMinX;
        maxX = manualMaxX;
        minY = manualMinY;
        maxY = manualMaxY;
        roomSmallerThanCamera = false;
        //Debug.Log($"[CameraFollow] Using manual boundaries: X({minX} to {maxX}), Y({minY} to {maxY})");
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition;

        if (roomSmallerThanCamera)
        {
            // Lock camera to center of small room
            desiredPosition = lockedCameraPosition;
        }
        else
        {
            // Normal follow behavior with boundaries
            desiredPosition = target.position + offset;

            if (useTilemapBoundaries || useManualBoundaries)
            {
                desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
                desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
            }
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

    // Public methods
    public void SetBoundaries(float newMinX, float newMaxX, float newMinY, float newMaxY)
    {
        minX = newMinX;
        maxX = newMaxX;
        minY = newMinY;
        maxY = newMaxY;
        useManualBoundaries = true;
        useTilemapBoundaries = false;
        roomSmallerThanCamera = false;
       // Debug.Log($"[CameraFollow] Manual boundaries set: X({minX} to {maxX}), Y({minY} to {maxY})");
    }

    public void RefreshTilemapBoundaries()
    {
        if (useTilemapBoundaries)
        {
           // Debug.Log("[CameraFollow] Manually refreshing tilemap boundaries...");
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

    void OnDrawGizmosSelected()
    {
        if (useTilemapBoundaries || useManualBoundaries)
        {
            // Draw boundaries
            Gizmos.color = roomSmallerThanCamera ? Color.red : Color.cyan;
            Vector3 center = new Vector3((minX + maxX) * 0.5f, (minY + maxY) * 0.5f, transform.position.z);
            Vector3 size = new Vector3(Mathf.Max(0.1f, maxX - minX), Mathf.Max(0.1f, maxY - minY), 0);
            Gizmos.DrawWireCube(center, size);

            // Draw camera bounds
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, new Vector3(cameraHalfWidth * 2, cameraHalfHeight * 2, 0));
        }
    }
}