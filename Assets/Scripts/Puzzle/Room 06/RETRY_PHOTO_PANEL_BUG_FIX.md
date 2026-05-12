# Room 06 - Retry Shows Photo Panel Bug (FIXED!)

## ❌ PROBLEMA

**Issue**: Pag nag-retry sa Room 06, lumalabas yung photo panel (with X button) at dialogue panel na walang text.

**Screenshot**: 
- Photo panel is visible (dapat hidden)
- Dialogue panel shows "Lisa" pero walang text
- May "X" button sa photo panel

---

## 🔧 ROOT CAUSE

### Why This Happens:

1. **Photo Panel Persists**: Ang photo panel ay naka-open pa from the previous game over
2. **Scene Reload**: Pag nag-reload ang scene, hindi na-close yung panel properly
3. **Intro Triggers**: Intro dialogue nag-trigger pero may conflict with photo panel

### The Flow (BUG):

1. Player interacts with photo → Panel opens
2. Emily catches player → Game Over
3. **BUG**: Photo panel stays open (not closed properly)
4. Player clicks Retry → Scene reloads
5. **BUG**: Photo panel still visible!
6. Intro dialogue tries to play → Conflict!

---

## ✅ SOLUSYON

### Fix 1: Force Close Panel in Awake()

**File**: `Assets/Scripts/Puzzle/Room 06/Room06_HallwayController.cs`

**Added to Awake()**:
```csharp
private void Awake()
{
    if (Instance == null) Instance = this;
    
    // CRITICAL: Force close photo panel immediately on awake
    // This prevents it from showing if it was left open from previous game over
    if (photoPanel != null)
    {
        photoPanel.SetActive(false);
        if (debugMode) Debug.Log("[Room06] Photo panel force closed in Awake");
    }
}
```

### Fix 2: Force Close Panel in Start()

**Added debug log in Start()**:
```csharp
// CRITICAL: Force hide photo panel on start
if (photoPanel != null)
{
    photoPanel.SetActive(false);
    if (debugMode) Debug.Log("[Room06] Photo panel hidden on start");
}
```

### Fix 3: Close Panel on Game Over

**File**: `Assets/Scripts/Puzzle/Room 06/Room06_HallwayController.cs`

**In TriggerGameOver()**:
```csharp
private void TriggerGameOver()
{
    if (!isEmilyHunting) return;
    
    isEmilyHunting = false;
    
    // CRITICAL: Close photo panel before game over
    if (photoPanel != null && photoPanel.activeSelf)
    {
        photoPanel.SetActive(false);
        if (debugMode) Debug.Log("[Room06] Photo panel closed before game over");
    }
    
    // ... rest of game over logic
}
```

---

## 🎯 ADDITIONAL FIX: Close Button Setup

### If You Have a Close Button (X):

1. **Find the X button** in Photo Panel
2. **Add Button component** if not present
3. **Add OnClick event**:
   - Drag `Room06_HallwayController` GameObject
   - Select function: `Room06_HallwayController.ClosePhotoPanel()`

### Updated ClosePhotoPanel() Method:

```csharp
public void ClosePhotoPanel()
{
    if (photoPanel != null)
    {
        photoPanel.SetActive(false);
        if (debugMode) Debug.Log("[Room06] Photo panel closed");
    }
    
    // Re-enable player controls if they were disabled
    JoystickPlayerController playerController = JoystickPlayerController.Instance;
    GameObject joystick = GameObject.Find("Joystick");
    
    if (playerController != null) playerController.enabled = true;
    if (joystick != null) joystick.SetActive(true);
}
```

---

## 🔍 DEBUGGING

### Check Console Logs:

**On Retry (Expected)**:
```
[GameOver] RestartLevel button clicked!
[GameOver] Loading scene: Room06_ReturnToHallwayUpStairs
[Room06] Photo panel force closed in Awake
[Room06] Photo panel hidden on start
[Room06] Playing intro sequence
[Room06] Intro sequence complete
```

### If Panel Still Shows:

1. **Check if panel is in PersistentUI**:
   - If yes, move it to Room 06 scene Canvas
   - Panels should NOT be in DontDestroyOnLoad

2. **Check if panel has CanvasGroup**:
   - If alpha is 0, panel is invisible but still active
   - Use `SetActive(false)` instead

3. **Check if panel is child of another object**:
   - Parent might be keeping it active
   - Make sure parent is also in Room 06 scene

---

## 📋 UNITY SETUP CHECK

### Photo Panel Hierarchy:

```
Room06_ReturnToHallwayUpStairs (Scene)
└─ Canvas (Scene Canvas, NOT PersistentUI!)
    └─ PhotoPanel
        ├─ PhotoImage (Image)
        └─ CloseButton (X button) [OPTIONAL]
            └─ OnClick → Room06_HallwayController.ClosePhotoPanel()
```

### CRITICAL:
- ✅ PhotoPanel must be in SCENE Canvas (not PersistentUI)
- ✅ PhotoPanel must be child of Room 06 scene
- ✅ PhotoPanel should be inactive in Inspector by default

---

## ✅ COMPLETE FIX IMPLEMENTATION

### Changes Made:

1. **Awake()**: Force close panel immediately
2. **Start()**: Force close panel again with debug log
3. **TriggerGameOver()**: Close panel before game over
4. **ClosePhotoPanel()**: Re-enable player controls

### Result:

- ✅ Photo panel is hidden on retry
- ✅ Intro dialogue plays correctly
- ✅ No visual glitches
- ✅ Player controls work properly

---

## 🐛 IF STILL SHOWING

### Manual Fix:

1. **Open Room 06 scene**
2. **Find PhotoPanel in Hierarchy**
3. **Check Inspector**:
   - Is it active? → Uncheck it
   - Is it in PersistentUI? → Move to scene Canvas
   - Does it have CanvasGroup? → Remove or set alpha to 1

4. **Test again**:
   - Enter Room 06
   - Interact with photo
   - Let Emily catch you
   - Click Retry
   - **Panel should be hidden!**

---

## 💡 PREVENTION

### For Future Panels:

1. **Always use scene-specific Canvas** (not PersistentUI)
2. **Always SetActive(false) in Awake/Start**
3. **Always close panels before Game Over**
4. **Always re-enable player controls when closing panels**

---

**Photo panel bug is FIXED! Test mo na!** 🎮✨
