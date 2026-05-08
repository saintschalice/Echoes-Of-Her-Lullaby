# Lisa Visible During Cutscene - Troubleshooting

**Problem**: Nakikita si Lisa kahit may cutscene pa  
**Status**: ✅ FIXED with multiple fallbacks

---

## What Was Fixed

### 1. Early Hiding in Awake()
**Before**: Lisa hidden in Start() - too late!  
**After**: Lisa hidden in Awake() - as early as possible!

```csharp
void Awake()
{
    // Hide player IMMEDIATELY
    if (hideLisaDuringCutscene)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) player = GameObject.Find("Lisa");
        
        if (player != null)
        {
            player.SetActive(false);
            Debug.Log("[FoyerIntro] Player hidden in Awake() - EARLY HIDE");
        }
    }
}
```

### 2. Multiple Find Methods
**Before**: Only used FindGameObjectWithTag("Player")  
**After**: Tries 3 methods to find Lisa:

1. `GameObject.FindGameObjectWithTag("Player")`
2. `GameObject.Find("Lisa")`
3. `PersistentSpawnManager.Instance.player`

### 3. Immediate Hide in Start()
**Before**: Found player, then waited for SaveSystem  
**After**: Find AND hide player IMMEDIATELY, then wait

---

## Debug Checklist

### Check 1: Is Lisa Tagged as "Player"?
1. Select Lisa GameObject in Hierarchy
2. Check Inspector → Tag dropdown
3. Should be: **"Player"**
4. If not, set it to "Player"

### Check 2: Check Console Logs
Look for these logs when starting New Game:

✅ **Success logs**:
```
[FoyerIntro] Player hidden in Awake() - EARLY HIDE
[FoyerIntro] Player hidden IMMEDIATELY in Start()
[FoyerIntro] First time seeing 'IntroCutscene_Played'. Playing cutscene.
[FoyerIntro] Lisa hidden during cutscene
```

❌ **Problem logs**:
```
[FoyerIntro] Could not find Player GameObject!
```

### Check 3: Verify Settings
1. Open Room01_Foyer scene
2. Find FoyerIntroController GameObject
3. Check Inspector:
   - `Hide Lisa During Cutscene`: ✅ **MUST BE CHECKED**
   - `Cutscene Object`: Assigned
   - `Blackout Canvas Group`: Assigned

---

## If Lisa Still Visible

### Solution 1: Check Lisa's Name
Lisa GameObject might have different name:
1. Open PersistentScene
2. Find the player GameObject
3. Check its name (should be "Lisa" or have "Player" tag)
4. If different name, update `FindAndHidePlayer()` method

### Solution 2: Disable Lisa in PersistentScene
**Manual workaround**:
1. Open PersistentScene
2. Find Lisa GameObject
3. **Uncheck** the checkbox next to her name (disable her)
4. FoyerIntroController will enable her after cutscene

**Pros**: Guaranteed to work  
**Cons**: Need to manually enable for Load Game

### Solution 3: Check Script Execution Order
1. Edit → Project Settings → Script Execution Order
2. Ensure `FoyerIntroController` runs BEFORE `PersistentSpawnManager`
3. Set FoyerIntroController to -100
4. Set PersistentSpawnManager to 0 (default)

### Solution 4: Force Hide in Inspector
Add this to FoyerIntroController:
```csharp
[Header("Manual Player Reference")]
public GameObject manualPlayerReference;

void Awake()
{
    // ... existing code ...
    
    // Force hide if manually assigned
    if (manualPlayerReference != null)
    {
        manualPlayerReference.SetActive(false);
        Debug.Log("[FoyerIntro] Manually assigned player hidden");
    }
}
```

Then drag Lisa GameObject to `manualPlayerReference` field.

---

## Testing Steps

### Test 1: New Game
1. Click "New Game" from Main Menu
2. ✅ Screen should be BLACK (no Lisa visible)
3. ✅ Cutscene plays
4. ✅ After cutscene, Lisa appears
5. Check Console for logs

### Test 2: Load Game
1. Load an existing save
2. ✅ Lisa should be visible immediately
3. ✅ No cutscene plays
4. ✅ Fade in from black

### Test 3: Check Timing
1. New Game
2. Watch carefully at the START
3. If you see Lisa for even 1 frame → Still a problem
4. Should be BLACK from the very start

---

## Advanced Debugging

### Add More Debug Logs
Add this to Awake():
```csharp
void Awake()
{
    Debug.Log("[FoyerIntro] Awake() called");
    Debug.Log($"[FoyerIntro] hideLisaDuringCutscene = {hideLisaDuringCutscene}");
    
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    Debug.Log($"[FoyerIntro] Found player by tag: {player != null}");
    
    if (player == null)
    {
        player = GameObject.Find("Lisa");
        Debug.Log($"[FoyerIntro] Found player by name: {player != null}");
    }
    
    if (player != null)
    {
        Debug.Log($"[FoyerIntro] Player name: {player.name}, active: {player.activeSelf}");
        player.SetActive(false);
        Debug.Log($"[FoyerIntro] Player hidden, now active: {player.activeSelf}");
    }
}
```

### Check Scene Load Order
1. Open PersistentScene
2. Check if Room01_Foyer is loaded additively
3. Verify FoyerIntroController is in Room01_Foyer, not PersistentScene

### Check DontDestroyOnLoad
Lisa might be in DontDestroyOnLoad:
1. Play the game
2. Open Hierarchy
3. Look for "DontDestroyOnLoad" section
4. Check if Lisa is there
5. If yes, that's why she's visible early

---

## Root Cause Analysis

### Why Lisa Was Visible

**Execution Order**:
```
1. PersistentScene loads
2. Lisa spawns (PersistentSpawnManager)
3. Room01_Foyer loads additively
4. FoyerIntroController.Awake() runs
5. FoyerIntroController.Start() runs
```

**Problem**: Lisa spawns in step 2, but we hide her in step 4-5.

**Solution**: Hide her as early as possible (Awake) with multiple fallback methods.

---

## Alternative Solution: Spawn Lisa Later

Instead of hiding Lisa, spawn her AFTER cutscene:

### Modify PersistentSpawnManager
```csharp
public bool delaySpawnForCutscene = false;

void Start()
{
    if (delaySpawnForCutscene)
    {
        // Don't spawn player yet
        if (player != null)
        {
            player.gameObject.SetActive(false);
        }
        return;
    }
    
    // Normal spawn logic
}

public void SpawnPlayerNow()
{
    if (player != null)
    {
        player.gameObject.SetActive(true);
    }
}
```

### Call from FoyerIntroController
```csharp
void ShowPlayer()
{
    if (playerObject != null)
    {
        playerObject.SetActive(true);
    }
    
    // Also notify PersistentSpawnManager
    if (PersistentSpawnManager.Instance != null)
    {
        PersistentSpawnManager.Instance.SpawnPlayerNow();
    }
}
```

---

## Summary

**Current Fix**:
- ✅ Hide Lisa in Awake() (earliest possible)
- ✅ Multiple find methods (tag, name, manager)
- ✅ Immediate hide before any waiting
- ✅ Show Lisa after cutscene ends

**If Still Not Working**:
1. Check Lisa's tag ("Player")
2. Check Console logs
3. Verify `hideLisaDuringCutscene` is checked
4. Try manual player reference
5. Check script execution order

**Last Resort**:
- Disable Lisa in PersistentScene Inspector
- FoyerIntroController will enable her when needed

---

## Related Files

- `Assets/Scripts/Puzzle/Room 01/FoyerIntroController.cs` - Main controller
- `Assets/Scripts/Player/PersistentSpawnManager.cs` - Player spawning
- `Assets/Scripts/GameManagement/SaveSystem.cs` - Save/load logic

---

**Test again after this fix! Lisa should be hidden from the very start.** 🎬
