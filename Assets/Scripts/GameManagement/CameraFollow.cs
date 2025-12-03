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
    [Tooltip("If true, the script automatically finds tilemaps in the scene. If false, it uses the list below.")]
    public bool autoScanForTilemaps = true;
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
        StartCoroutine(RefreshBoundariesDelayed());
    }

    System.Collections.IEnumerator RefreshBoundariesDelayed()
    {
        yield return new WaitForEndOfFrame();

        if (useTilemapBoundaries)
        {
            UpdateTilemapBoundaries();
        }
        else if (useManualBoundaries)
        {
            SetManualBoundaries();
        }

        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }

        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;

            if (useTilemapBoundaries || useManualBoundaries)
            {
                if (roomSmallerThanCamera)
                {
                    desiredPosition = lockedCameraPosition;
                }
                else
                {
                    desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
                    desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
                }
            }

            transform.position = desiredPosition;
            velocity = Vector3.zero;
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
        StartCoroutine(RefreshBoundariesDelayed());
        lastSceneName = SceneManager.GetActiveScene().name;
    }

    void Update()
    {
        Camera cam = GetComponent<Camera>();
        float currentHalfWidth = cam.orthographicSize * cam.aspect;

        if (Mathf.Abs(currentHalfWidth - cameraHalfWidth) > 0.1f)
        {
            CalculateCameraSize();
            if (useTilemapBoundaries)
            {
                SetBoundariesFromTilemap();
            }
        }

        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != lastSceneName)
        {
            lastSceneName = currentScene;
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
    }

    void UpdateTilemapBoundaries()
    {
        // Only auto-find if the checkbox is checked
        if (autoScanForTilemaps)
        {
            AutoFindTilemaps();
        }

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
                string tName = tilemap.gameObject.name.ToLower();

                // Exclude UI, Overlay, AND Extras from boundary calculations
                if (!tName.Contains("ui") &&
                    !tName.Contains("overlay") &&
                    !tName.Contains("extras")) // <--- ADDED THIS LINE
                {
                    validTilemaps.Add(tilemap);
                    // Debug.Log($"[CameraFollow] Found tilemap: {tilemap.gameObject.name}");
                }
            }
        }

        boundaryTilemaps = validTilemaps.ToArray();
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
        float tilemapWidth = tilemapBounds.size.x;
        float tilemapHeight = tilemapBounds.size.y;

        bool xTooSmall = tilemapWidth <= (cameraHalfWidth * 2 + boundaryPadding * 2);
        bool yTooSmall = tilemapHeight <= (cameraHalfHeight * 2 + boundaryPadding * 2);

        roomSmallerThanCamera = xTooSmall || yTooSmall;

        if (roomSmallerThanCamera)
        {
            lockedCameraPosition = new Vector3(
                tilemapBounds.center.x,
                tilemapBounds.center.y,
                offset.z
            );
            minX = maxX = tilemapBounds.center.x;
            minY = maxY = tilemapBounds.center.y;
        }
        else
        {
            minX = tilemapBounds.min.x + cameraHalfWidth + boundaryPadding;
            maxX = tilemapBounds.max.x - cameraHalfWidth - boundaryPadding;
            minY = tilemapBounds.min.y + cameraHalfHeight + boundaryPadding;
            maxY = tilemapBounds.max.y - cameraHalfHeight - boundaryPadding;

            if (minX > maxX)
            {
                float centerX = tilemapBounds.center.x;
                minX = maxX = centerX;
            }
            if (minY > maxY)
            {
                float centerY = tilemapBounds.center.y;
                minY = maxY = centerY;
            }
        }
    }

    void SetManualBoundaries()
    {
        minX = manualMinX;
        maxX = manualMaxX;
        minY = manualMinY;
        maxY = manualMaxY;
        roomSmallerThanCamera = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition;

        if (roomSmallerThanCamera)
        {
            desiredPosition = lockedCameraPosition;
        }
        else
        {
            desiredPosition = target.position + offset;

            if (useTilemapBoundaries || useManualBoundaries)
            {
                desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
                desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
            }
        }

        if (useSmoothing)
        {
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        }
    }

    public void SetBoundaries(float newMinX, float newMaxX, float newMinY, float newMaxY)
    {
        minX = newMinX;
        maxX = newMaxX;
        minY = newMinY;
        maxY = newMaxY;
        useManualBoundaries = true;
        useTilemapBoundaries = false;
        roomSmallerThanCamera = false;
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

    void OnDrawGizmosSelected()
    {
        if (useTilemapBoundaries || useManualBoundaries)
        {
            Gizmos.color = roomSmallerThanCamera ? Color.red : Color.cyan;
            Vector3 center = new Vector3((minX + maxX) * 0.5f, (minY + maxY) * 0.5f, transform.position.z);
            Vector3 size = new Vector3(Mathf.Max(0.1f, maxX - minX), Mathf.Max(0.1f, maxY - minY), 0);
            Gizmos.DrawWireCube(center, size);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, new Vector3(cameraHalfWidth * 2, cameraHalfHeight * 2, 0));
        }
    }
}