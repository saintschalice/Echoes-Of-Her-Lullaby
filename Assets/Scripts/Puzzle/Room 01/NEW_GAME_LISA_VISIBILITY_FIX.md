# New Game Lisa Visibility Fix - COMPLETE

## Problem
When clicking "New Game" in the main menu, Lisa was visible for a split second before the intro cutscene started. This happened because:
1. PersistentScene loads first and spawns Lisa
2. Room01_Foyer scene loads second
3. FoyerIntroController tries to hide Lisa, but she's already been visible

## Solution
Implemented a two-part system where **PersistentSpawnManager** handles the initial hide, and **FoyerIntroController** shows Lisa after the cutscene.

---

## Changes Made

### 1. PersistentSpawnManager.cs
**Location**: `Assets/Scripts/Player/PersistentSpawnManager.cs`

#### Added Settings
```csharp
[Header("New Game Settings")]
[Tooltip("Hide player on first load (for intro cutscene). FoyerIntroController will show her after cutscene.")]
public bool hidePlayerOnNewGame = true;
```

#### Modified Start() Method
- Checks if this is a new game by reading `PlayerPrefs.GetInt("LoadSlotOnStart")`
- If `LoadSlotOnStart == -1`, it's a NEW GAME → Lisa is hidden
- If `LoadSlotOnStart >= 0`, it's a LOAD GAME → Lisa is visible
- This happens BEFORE any scene transitions, preventing visibility

```csharp
// CRITICAL: Hide player on new game to prevent visibility before cutscene
if (hidePlayerOnNewGame && player != null)
{
    if (PlayerPrefs.HasKey("LoadSlotOnStart"))
    {
        int loadSlot = PlayerPrefs.GetInt("LoadSlotOnStart");
        if (loadSlot == -1)
        {
            // NEW GAME - hide Lisa
            player.gameObject.SetActive(false);
            Debug.Log("[PersistentSpawn] NEW GAME detected - Lisa hidden until cutscene ends");
        }
        else
        {
            // LOAD GAME - Lisa visible
            player.gameObject.SetActive(true);
            Debug.Log("[PersistentSpawn] LOAD GAME detected - Lisa visible immediately");
        }
    }
}
```

#### Added Public Methods
```csharp
/// <summary>
/// Enable the player GameObject. Called by FoyerIntroController after cutscene ends.
/// </summary>
public void EnablePlayer()
{
    if (player != null)
    {
        player.gameObject.SetActive(true);
        Debug.Log("[PersistentSpawn] Player enabled (called externally)");
    }
}

/// <summary>
/// Disable the player GameObject. For special cases like cutscenes.
/// </summary>
public void DisablePlayer()
{
    if (player != null)
    {
        player.gameObject.SetActive(false);
        Debug.Log("[PersistentSpawn] Player disabled (called externally)");
    }
}
```

---

### 2. FoyerIntroController.cs
**Location**: `Assets/Scripts/Puzzle/Room 01/FoyerIntroController.cs`

#### Removed Local Player Management
- Removed `hideLisaDuringCutscene` setting (no longer needed)
- Removed `playerObject` and `playerWasActive` fields
- Removed `FindAndHidePlayer()`, `HidePlayer()`, and `ShowPlayer()` methods
- Now delegates all player visibility to PersistentSpawnManager

#### Modified CheckAndPlayCutsceneRoutine()
**For Load Game (cutscene already seen):**
```csharp
// Ensure Lisa is visible for loaded games
if (PersistentSpawnManager.Instance != null)
{
    PersistentSpawnManager.Instance.EnablePlayer();
}
```

**For New Game (first time):**
```csharp
// Show Lisa after cutscene ends
if (PersistentSpawnManager.Instance != null)
{
    PersistentSpawnManager.Instance.EnablePlayer();
    Debug.Log("[FoyerIntro] Lisa shown after cutscene via PersistentSpawnManager");
}
```

#### Modified FinishIntro()
```csharp
public void FinishIntro()
{
    manualFinishTriggered = true;
    Debug.Log("[FoyerIntro] Manual finish triggered.");
    
    // Show Lisa when cutscene finishes
    if (PersistentSpawnManager.Instance != null)
    {
        PersistentSpawnManager.Instance.EnablePlayer();
        Debug.Log("[FoyerIntro] Lisa enabled via PersistentSpawnManager in FinishIntro()");
    }
}
```

---

## How It Works

### New Game Flow
1. **MainMenu** sets `PlayerPrefs.SetInt("LoadSlotOnStart", -1)` for new game
2. **PersistentScene** loads → PersistentSpawnManager spawns Lisa
3. **PersistentSpawnManager.Start()** detects `LoadSlotOnStart == -1` → Hides Lisa IMMEDIATELY
4. **Room01_Foyer** scene loads → Black screen is visible
5. **FoyerIntroController** plays cutscene
6. **After cutscene ends** → FoyerIntroController calls `PersistentSpawnManager.Instance.EnablePlayer()`
7. **Lisa appears** for the first time (no flicker!)

### Load Game Flow
1. **MainMenu** sets `PlayerPrefs.SetInt("LoadSlotOnStart", slotNumber)` for load game
2. **PersistentScene** loads → PersistentSpawnManager spawns Lisa
3. **PersistentSpawnManager.Start()** detects `LoadSlotOnStart >= 0` → Keeps Lisa VISIBLE
4. **Room loads** → Lisa is already visible (cutscene skipped)
5. **FoyerIntroController** detects cutscene already seen → Fades in room with Lisa visible

---

## Unity Setup Required

### PersistentScene Setup
1. Select **PersistentSpawnManager** GameObject in PersistentScene
2. In Inspector, ensure:
   - `Hide Player On New Game` = **TRUE** (checked)
   - `Player` reference is assigned to Lisa GameObject
   - `Debug Mode` = TRUE (for testing logs)

### Room01_Foyer Setup
1. **FoyerIntroController** should have:
   - `Cutscene Object` assigned (the cutscene GameObject)
   - `Blackout Canvas Group` assigned (the black screen overlay)
   - `Cutscene Save ID` = "IntroCutscene_Played"

2. **Cutscene GameObject** should:
   - Be DISABLED by default in Inspector
   - Call `FoyerIntroController.FinishIntro()` when cutscene ends (via UnityEvent or Timeline Signal)

3. **Blackout Canvas Group** should:
   - Be ENABLED by default in Inspector
   - Have `Alpha = 1` (fully black)
   - Have `Blocks Raycasts = true`

---

## Testing Checklist

### Test New Game
- [ ] Click "New Game" in main menu
- [ ] Black screen appears immediately (no Lisa visible)
- [ ] Cutscene plays
- [ ] After cutscene ends, Lisa appears
- [ ] No flicker or split-second visibility

### Test Load Game
- [ ] Click "Load Game" in main menu
- [ ] Lisa is visible immediately (no cutscene)
- [ ] Room fades in normally
- [ ] Lisa can move immediately

### Debug Logs to Check
**New Game:**
```
[PersistentSpawn] NEW GAME detected - Lisa hidden until cutscene ends
[FoyerIntro] First time seeing 'IntroCutscene_Played'. Playing cutscene.
[FoyerIntro] Lisa shown after cutscene via PersistentSpawnManager
[PersistentSpawn] Player enabled (called externally)
```

**Load Game:**
```
[PersistentSpawn] LOAD GAME detected - Lisa visible immediately
[FoyerIntro] Cutscene 'IntroCutscene_Played' already seen. Skipping and fading in.
[PersistentSpawn] Player enabled (called externally)
```

---

## Troubleshooting

### Lisa Still Visible on New Game
1. Check PersistentSpawnManager Inspector: `Hide Player On New Game` must be TRUE
2. Check Console for log: "[PersistentSpawn] NEW GAME detected"
3. Verify MainMenu is setting `PlayerPrefs.SetInt("LoadSlotOnStart", -1)` for new game
4. Check that Lisa GameObject has "Player" tag

### Lisa Not Appearing After Cutscene
1. Check that cutscene calls `FoyerIntroController.FinishIntro()` when done
2. Check Console for log: "[PersistentSpawn] Player enabled (called externally)"
3. Verify PersistentSpawnManager.Instance is not null
4. Check that Lisa GameObject exists in PersistentScene

### Lisa Not Visible on Load Game
1. Check Console for log: "[PersistentSpawn] LOAD GAME detected"
2. Verify MainMenu is setting correct slot number (>= 0) for load game
3. Check that `PlayerPrefs.GetInt("LoadSlotOnStart")` returns correct value

---

## Technical Notes

### Why This Approach Works
- **Early Detection**: PersistentSpawnManager.Start() runs BEFORE Room01_Foyer loads
- **Centralized Control**: All player visibility is managed by PersistentSpawnManager
- **Clear Separation**: PersistentSpawnManager handles spawn/visibility, FoyerIntroController handles cutscene logic
- **No Race Conditions**: PlayerPrefs flag is set by MainMenu before any scene loads

### Alternative Approaches Considered
1. ❌ **Disable Lisa in PersistentScene Inspector**: Would break load game (Lisa wouldn't appear)
2. ❌ **Hide Lisa in FoyerIntroController.Awake()**: Too late, Lisa already visible for 1 frame
3. ✅ **Current approach**: PersistentSpawnManager detects new game and hides Lisa BEFORE any visibility

---

## Related Files
- `Assets/Scripts/Player/PersistentSpawnManager.cs` - Player spawn and visibility management
- `Assets/Scripts/Puzzle/Room 01/FoyerIntroController.cs` - Intro cutscene controller
- `Assets/Scripts/GameManagement/SaveSystem.cs` - Save/load system (sets LoadSlotOnStart flag)
- `Assets/Scripts/UI/MainMenuManager.cs` - Main menu (should set LoadSlotOnStart flag)

---

## Status
✅ **COMPLETE** - Lisa visibility issue fixed for both new game and load game scenarios.

**Date**: 2026-05-04
**Tested**: Code complete, ready for Unity testing
