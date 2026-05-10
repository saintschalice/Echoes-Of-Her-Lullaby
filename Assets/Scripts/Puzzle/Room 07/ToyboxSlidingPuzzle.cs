using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 8-Tile Sliding Puzzle (3x3 grid with one empty space)
/// Uses SWIPE controls - swipe in direction to move tiles
/// </summary>
public class ToyboxSlidingPuzzle : MonoBehaviour
{
    [Header("UI References")]
    public GameObject toyboxPanel;
    public Button closeButton;
    public Transform tilesParent; // Grid layout group
    
    [Header("Optional: Arrow Buttons (Backup Controls)")]
    public Button upButton;
    public Button downButton;
    public Button leftButton;
    public Button rightButton;

    [Header("Puzzle Settings")]
    public Sprite puzzleImage; // The game icon to split into tiles
    public int gridSize = 3; // 3x3 grid
    public float shuffleMoves = 20; // Number of random moves to shuffle

    [Header("Swipe Settings")]
    public float swipeThreshold = 50f; // Minimum distance for swipe
    public float swipeDeadzone = 0.3f; // Time to ignore after swipe

    [Header("Audio")]
    public AudioClip tileMoveSound;
    public AudioClip successSound;

    private List<TileButton> tiles = new List<TileButton>();
    private int emptyTileIndex;
    private bool isPuzzleSolved = false;
    private bool isInitialized = false;
    
    // Swipe detection
    private Vector2 swipeStartPos;
    private bool isSwiping = false;
    private float lastSwipeTime;

    [System.Serializable]
    public class TileButton
    {
        public GameObject gameObject;
        public Image image;
        public int currentIndex;
        public int correctIndex;
        public bool isEmpty;
    }

    void Start()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePuzzle);
        
        // Setup arrow button listeners (optional backup controls)
        // Note: In Unity UI, Row 0 is at TOP
        if (upButton != null)
            upButton.onClick.AddListener(() => OnArrowButtonPressed(Vector2Int.up)); // Changed from down
        
        if (downButton != null)
            downButton.onClick.AddListener(() => OnArrowButtonPressed(Vector2Int.down)); // Changed from up
        
        if (leftButton != null)
            leftButton.onClick.AddListener(() => OnArrowButtonPressed(Vector2Int.right));
        
        if (rightButton != null)
            rightButton.onClick.AddListener(() => OnArrowButtonPressed(Vector2Int.left));
    }
    
    void OnArrowButtonPressed(Vector2Int direction)
    {
        Debug.Log($"[ToyboxPuzzle] 🔘 Arrow button pressed: {direction}");
        MoveTileInDirection(direction);
    }

    void OnEnable()
    {
        PauseGame();
        isPuzzleSolved = false;

        if (!isInitialized)
        {
            InitializePuzzle();
            isInitialized = true;
        }
        
        ShufflePuzzle();
    }

    void Update()
    {
        if (isPuzzleSolved) return;
        
        DetectSwipe();
    }

    void DetectSwipe()
    {
        // Check if enough time passed since last swipe
        if (Time.time - lastSwipeTime < swipeDeadzone) return;

        // Mouse/Touch input - detect anywhere on screen
        if (Input.GetMouseButtonDown(0))
        {
            swipeStartPos = Input.mousePosition;
            isSwiping = true;
            Debug.Log($"[ToyboxPuzzle] ✋ Swipe started at: {swipeStartPos}");
        }
        else if (Input.GetMouseButtonUp(0) && isSwiping)
        {
            Vector2 swipeEndPos = Input.mousePosition;
            Vector2 swipeDelta = swipeEndPos - swipeStartPos;
            
            Debug.Log($"[ToyboxPuzzle] 🎯 Swipe ended at: {swipeEndPos}");
            Debug.Log($"[ToyboxPuzzle] 📊 Delta: ({swipeDelta.x:F1}, {swipeDelta.y:F1}), Magnitude: {swipeDelta.magnitude:F1}, Threshold: {swipeThreshold}");
            
            if (swipeDelta.magnitude >= swipeThreshold)
            {
                ProcessSwipe(swipeDelta);
                lastSwipeTime = Time.time;
            }
            else
            {
                Debug.LogWarning($"[ToyboxPuzzle] ❌ Swipe too short! Magnitude {swipeDelta.magnitude:F1} < Threshold {swipeThreshold}");
            }
            
            isSwiping = false;
        }
        
        // Also support touch input for mobile
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                swipeStartPos = touch.position;
                isSwiping = true;
                Debug.Log($"[ToyboxPuzzle] 📱 Touch started at: {swipeStartPos}");
            }
            else if (touch.phase == TouchPhase.Ended && isSwiping)
            {
                Vector2 swipeEndPos = touch.position;
                Vector2 swipeDelta = swipeEndPos - swipeStartPos;
                
                Debug.Log($"[ToyboxPuzzle] 📱 Touch ended at: {swipeEndPos}");
                Debug.Log($"[ToyboxPuzzle] 📊 Delta: ({swipeDelta.x:F1}, {swipeDelta.y:F1}), Magnitude: {swipeDelta.magnitude:F1}");
                
                if (swipeDelta.magnitude >= swipeThreshold && Time.time - lastSwipeTime >= swipeDeadzone)
                {
                    ProcessSwipe(swipeDelta);
                    lastSwipeTime = Time.time;
                }
                else if (swipeDelta.magnitude < swipeThreshold)
                {
                    Debug.LogWarning($"[ToyboxPuzzle] ❌ Touch swipe too short! {swipeDelta.magnitude:F1} < {swipeThreshold}");
                }
                
                isSwiping = false;
            }
        }
    }

    void ProcessSwipe(Vector2 swipeDelta)
    {
        float absX = Mathf.Abs(swipeDelta.x);
        float absY = Mathf.Abs(swipeDelta.y);
        
        Debug.Log($"[ToyboxPuzzle] Swipe detected: Delta=({swipeDelta.x:F1}, {swipeDelta.y:F1}), AbsX={absX:F1}, AbsY={absY:F1}");
        
        // Determine swipe direction - compare absolute values
        if (absX > absY)
        {
            // Horizontal swipe (X is dominant)
            Debug.Log($"[ToyboxPuzzle] HORIZONTAL swipe (absX {absX:F1} > absY {absY:F1})");
            
            if (swipeDelta.x > 0)
            {
                // Swipe RIGHT → Move tile from LEFT to empty space
                Debug.Log("[ToyboxPuzzle] Swipe RIGHT detected → Moving tile from LEFT");
                MoveTileInDirection(Vector2Int.left);
            }
            else
            {
                // Swipe LEFT → Move tile from RIGHT to empty space
                Debug.Log("[ToyboxPuzzle] Swipe LEFT detected → Moving tile from RIGHT");
                MoveTileInDirection(Vector2Int.right);
            }
        }
        else
        {
            // Vertical swipe (Y is dominant)
            Debug.Log($"[ToyboxPuzzle] VERTICAL swipe (absY {absY:F1} > absX {absX:F1})");
            
            if (swipeDelta.y > 0)
            {
                // Swipe UP → Move tile from BELOW to empty space
                // In Unity UI, Row 0 is at TOP, so "below" means higher row number
                Debug.Log("[ToyboxPuzzle] Swipe UP detected → Moving tile from BELOW (higher row)");
                MoveTileInDirection(Vector2Int.up); // Changed from down to up
            }
            else
            {
                // Swipe DOWN → Move tile from ABOVE to empty space
                // In Unity UI, Row 0 is at TOP, so "above" means lower row number
                Debug.Log("[ToyboxPuzzle] Swipe DOWN detected → Moving tile from ABOVE (lower row)");
                MoveTileInDirection(Vector2Int.down); // Changed from up to down
            }
        }
    }

    void MoveTileInDirection(Vector2Int direction)
    {
        // Get empty tile position
        int emptyRow = emptyTileIndex / gridSize;
        int emptyCol = emptyTileIndex % gridSize;
        
        Debug.Log($"[ToyboxPuzzle] Empty tile at Row:{emptyRow} Col:{emptyCol} (Index:{emptyTileIndex})");
        
        // Calculate tile to move position
        int tileRow = emptyRow + direction.y;
        int tileCol = emptyCol + direction.x;
        
        Debug.Log($"[ToyboxPuzzle] Trying to move tile from Row:{tileRow} Col:{tileCol} (Direction:{direction})");
        
        // Check if valid position
        if (tileRow < 0 || tileRow >= gridSize || tileCol < 0 || tileCol >= gridSize)
        {
            Debug.Log($"[ToyboxPuzzle] Invalid move - out of bounds (Row:{tileRow}, Col:{tileCol})");
            return;
        }
        
        int tileIndex = tileRow * gridSize + tileCol;
        
        Debug.Log($"[ToyboxPuzzle] Valid move! Moving tile at index {tileIndex} to empty space at {emptyTileIndex}");
        
        // Move the tile
        SwapTiles(tileIndex, emptyTileIndex);
        PlaySound(tileMoveSound);
        
        // Check if solved
        if (IsPuzzleSolved())
        {
            StartCoroutine(CompletePuzzle());
        }
    }

    void InitializePuzzle()
    {
        // Clear existing tiles first
        foreach (Transform child in tilesParent)
        {
            Destroy(child.gameObject);
        }
        tiles.Clear();

        // Create 9 tiles (8 image tiles + 1 empty)
        for (int i = 0; i < gridSize * gridSize; i++)
        {
            GameObject tileObj = new GameObject($"Tile_{i}");
            tileObj.transform.SetParent(tilesParent);
            tileObj.transform.localScale = Vector3.one;

            // Add Image component
            Image img = tileObj.AddComponent<Image>();

            TileButton tile = new TileButton
            {
                gameObject = tileObj,
                image = img,
                currentIndex = i,
                correctIndex = i,
                isEmpty = (i == gridSize * gridSize - 1)
            };

            if (tile.isEmpty)
            {
                img.color = Color.clear;
                emptyTileIndex = i;
            }
            else
            {
                img.sprite = CreateTileSprite(i);
            }

            tiles.Add(tile);
        }
        
        Debug.Log($"[ToyboxPuzzle] Initialized {tiles.Count} tiles with swipe controls");
    }

    Sprite CreateTileSprite(int tileIndex)
    {
        if (puzzleImage == null) return null;

        int row = tileIndex / gridSize;
        int col = tileIndex % gridSize;

        float tileWidth = 1f / gridSize;
        float tileHeight = 1f / gridSize;

        Rect rect = new Rect(
            col * tileWidth * puzzleImage.texture.width,
            (gridSize - 1 - row) * tileHeight * puzzleImage.texture.height,
            tileWidth * puzzleImage.texture.width,
            tileHeight * puzzleImage.texture.height
        );

        return Sprite.Create(puzzleImage.texture, rect, new Vector2(0.5f, 0.5f));
    }

    void ShufflePuzzle()
    {
        for (int i = 0; i < shuffleMoves; i++)
        {
            List<int> validMoves = GetValidMoves(emptyTileIndex);
            if (validMoves.Count > 0)
            {
                int randomMove = validMoves[Random.Range(0, validMoves.Count)];
                SwapTiles(emptyTileIndex, randomMove);
            }
        }
    }

    List<int> GetValidMoves(int emptyIndex)
    {
        List<int> validMoves = new List<int>();
        
        int emptyRow = emptyIndex / gridSize;
        int emptyCol = emptyIndex % gridSize;
        
        // Check all 4 directions
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        
        foreach (var dir in directions)
        {
            int newRow = emptyRow + dir.y;
            int newCol = emptyCol + dir.x;
            
            if (newRow >= 0 && newRow < gridSize && newCol >= 0 && newCol < gridSize)
            {
                validMoves.Add(newRow * gridSize + newCol);
            }
        }
        
        return validMoves;
    }

    void SwapTiles(int index1, int index2)
    {
        // Swap visual positions
        Transform temp = tiles[index1].gameObject.transform;
        int tempSiblingIndex = temp.GetSiblingIndex();
        temp.SetSiblingIndex(tiles[index2].gameObject.transform.GetSiblingIndex());
        tiles[index2].gameObject.transform.SetSiblingIndex(tempSiblingIndex);

        // Update current indices
        tiles[index1].currentIndex = index2;
        tiles[index2].currentIndex = index1;

        // Update empty tile index
        if (tiles[index1].isEmpty)
            emptyTileIndex = index2;
        else if (tiles[index2].isEmpty)
            emptyTileIndex = index1;

        // Swap in list
        TileButton tempTile = tiles[index1];
        tiles[index1] = tiles[index2];
        tiles[index2] = tempTile;
    }

    bool IsPuzzleSolved()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].currentIndex != tiles[i].correctIndex)
                return false;
        }
        return true;
    }

    IEnumerator CompletePuzzle()
    {
        isPuzzleSolved = true;
        PlaySound(successSound);

        yield return new WaitForSeconds(1f);

        Room07UIManager uiManager = FindFirstObjectByType<Room07UIManager>();
        if (uiManager != null)
        {
            uiManager.OnToyboxSolved();
        }

        ResumeGame();
    }

    void ClosePuzzle()
    {
        if (toyboxPanel != null)
            toyboxPanel.SetActive(false);

        ResumeGame();
    }

    void PauseGame()
    {
        EmilyGhost emily = FindFirstObjectByType<EmilyGhost>();
        if (emily != null) emily.isPaused = true;

        JoystickPlayerController player = FindFirstObjectByType<JoystickPlayerController>();
        if (player != null) player.enabled = false;

        GameObject joystick = GameObject.Find("Joystick");
        if (joystick != null) joystick.SetActive(false);
    }

    void ResumeGame()
    {
        EmilyGhost emily = FindFirstObjectByType<EmilyGhost>();
        if (emily != null) emily.isPaused = false;

        JoystickPlayerController player = FindFirstObjectByType<JoystickPlayerController>();
        if (player != null) player.enabled = true;

        GameObject joystick = GameObject.Find("Joystick");
        if (joystick != null) joystick.SetActive(true);
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
            AudioManager.Instance?.PlaySFX(clip);
    }
}
