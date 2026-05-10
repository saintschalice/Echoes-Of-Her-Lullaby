# PersistentSceneHider Setup Guide - FINAL SOLUTION

## Problema
Nakikita lahat ng objects sa PersistentScene (Lisa, joystick, UI) bago mag-start ang cutscene.

## Solusyon
**PersistentSceneHider** - I-hide lahat ng visible objects sa PersistentScene during cutscene!

---

## Unity Setup (SIMPLE!)

### Step 1: Create PersistentSceneHider GameObject
1. Open **PersistentScene**
2. Right-click in Hierarchy → Create Empty
3. Rename to **"PersistentSceneHider"**
4. Add Component → **PersistentSceneHider.cs**

### Step 2: Assign References
Sa PersistentSceneHider Inspector:

#### Lisa Reference
- Drag **Lisa** GameObject → `Lisa` field

#### PersistentUI Reference
- Drag **PersistentUI** GameObject → `Persistent UI` field
  - Ito yung parent ng joystick, inventory, etc.
  - Kung walang parent, drag yung joystick mismo

#### Other Objects (Optional)
- Kung may ibang objects na dapat i-hide (e.g., minimap, health bar)
- I-add sa `Other Objects To Hide` array

#### Settings
- ✅ Check `Hide On New Game` = **TRUE**
- ✅ Check `Debug Mode` = **TRUE** (para makita ang logs)

---

## Example Setup

```
PersistentScene
├── AudioManager
├── PersistentManagers
├── PersistentUI  ← DRAG THIS to "Persistent UI" field
│   ├── FloatingJoystick
│   ├── InventoryPanel
│   └── NotificationPanel
├── Lisa  ← DRAG THIS to "Lisa" field
├── SceneInitializer
├── EventSystem
├── Main Camera
└── PersistentSceneHider  ← NEW! Add PersistentSceneHider.cs here
```

---

## How It Works

### New Game Flow
```
MainMenu sets: LoadSlotOnStart = -1
    ↓
PersistentScene loads
    ↓
PersistentSceneHider.Start() detects -1 (new game)
    ↓
HideAllObjects() called
    ↓
Lisa.SetActive(false)
PersistentUI.SetActive(false)
    ↓
Room01_Foyer loads (black screen visible)
    ↓
Cutscene plays (nothing visible except cutscene)
    ↓
Cutscene ends → FoyerIntroController.FinishIntro()
    ↓
PersistentSceneHider.ShowAllObjects()
    ↓
Lisa.SetActive(true)
PersistentUI.SetActive(true)
    ↓
Game starts! (Lisa + UI visible)
```

### Load Game Flow
```
MainMenu sets: LoadSlotOnStart = slot number
    ↓
PersistentScene loads
    ↓
PersistentSceneHider.Start() detects >= 0 (load game)
    ↓
ShowAllObjects() called
    ↓
Lisa.SetActive(true)
PersistentUI.SetActive(true)
    ↓
Room loads with everything visible
    ↓
No cutscene (already seen)
```

---

## What Gets Hidden

### Automatically Hidden on New Game:
✅ **Lisa** - Player character  
✅ **PersistentUI** - Joystick, inventory, notifications, etc.  
✅ **Other Objects** - Any additional objects you specify  

### NOT Hidden:
❌ **AudioManager** - Still plays music/sounds  
❌ **PersistentManagers** - SaveSystem, etc. still work  
❌ **EventSystem** - Still processes input  
❌ **Main Camera** - Still renders the scene  

---

## Testing Checklist

### Test New Game
- [ ] Click "New Game" in main menu
- [ ] **Expected**: Black screen only (no Lisa, no joystick, no UI)
- [ ] Cutscene plays
- [ ] After cutscene: Lisa appears, joystick appears, UI appears
- [ ] Check Console for:
  ```
  [PersistentHider] NEW GAME - All persistent objects hidden
  [FoyerIntro] All persistent objects shown after cutscene
  ```

### Test Load Game
- [ ] Save a game first
- [ ] Go back to main menu
- [ ] Click "Load Game"
- [ ] **Expected**: Lisa + UI visible immediately
- [ ] No cutscene plays
- [ ] Check Console for:
  ```
  [PersistentHider] LOAD GAME - All persistent objects visible
  ```

---

## Troubleshooting

### Problem: Lisa/UI still visible on new game
**Solution**:
1. Check PersistentSceneHider Inspector:
   - `Hide On New Game` must be TRUE
   - `Lisa` reference must be assigned
   - `Persistent UI` reference must be assigned
2. Check Console - should see "[PersistentHider] NEW GAME - All persistent objects hidden"
3. Verify MainMenu sets `PlayerPrefs.SetInt("LoadSlotOnStart", -1)` for new game

### Problem: Lisa/UI not appearing after cutscene
**Solution**:
1. Check that cutscene calls `FoyerIntroController.FinishIntro()` when done
2. Check Console - should see "[FoyerIntro] All persistent objects shown"
3. Verify PersistentSceneHider.Instance is not null

### Problem: Joystick not in PersistentUI
**Solution**:
If joystick is separate GameObject:
1. Add joystick to `Other Objects To Hide` array
2. Or move joystick under PersistentUI parent

---

## Code Reference

### PersistentSceneHider.cs
**Location**: `Assets/Scripts/Puzzle/Room 01/PersistentSceneHider.cs`

**Key Methods**:
- `HideAllObjects()` - Hides Lisa, UI, and other objects
- `ShowAllObjects()` - Shows Lisa, UI, and other objects
- `AreObjectsHidden()` - Check if objects are currently hidden

### FoyerIntroController.cs
**Location**: `Assets/Scripts/Puzzle/Room 01/FoyerIntroController.cs`

**Integration**:
- Calls `PersistentSceneHider.Instance.ShowAllObjects()` after cutscene
- Works for both new game and load game

---

## Advantages of This Approach

✅ **Simple** - Just assign references in Inspector  
✅ **Centralized** - One script controls all visibility  
✅ **Flexible** - Easy to add more objects to hide  
✅ **Clean** - No complex finding/searching for objects  
✅ **Reliable** - Works every time, no race conditions  
✅ **Debuggable** - Clear logs show what's happening  

---

## Alternative: Manual Disable in Inspector

If you prefer even simpler approach:

### Option A: Disable Objects Manually
1. Open PersistentScene
2. Disable Lisa GameObject (uncheck in Inspector)
3. Disable PersistentUI GameObject (uncheck in Inspector)
4. FoyerIntroController will enable them after cutscene

**Pros**: No script needed  
**Cons**: Must remember to enable for load game

### Option B: Use PersistentSceneHider (RECOMMENDED)
**Pros**: Automatic detection, handles both new game and load game  
**Cons**: Requires setup (but only once!)

---

## Files Modified/Created

### New Files
✅ `PersistentSceneHider.cs` - Main script for hiding/showing objects

### Modified Files
✅ `FoyerIntroController.cs` - Calls PersistentSceneHider instead of individual objects

### No Longer Needed
❌ `PersistentSpawnManager.cs` hide logic - Can be removed if you want
❌ Individual joystick finding code - Handled by PersistentSceneHider

---

## Summary

**Before**: Lisa, joystick, UI all visible before cutscene  
**After**: Everything hidden during cutscene, shown after  

**Setup Time**: 2 minutes  
**Complexity**: Low  
**Reliability**: High  

**Status**: ✅ COMPLETE - Ready for Unity testing!

---

## Quick Setup Checklist

- [ ] Create PersistentSceneHider GameObject in PersistentScene
- [ ] Add PersistentSceneHider.cs component
- [ ] Assign Lisa reference
- [ ] Assign PersistentUI reference
- [ ] Check "Hide On New Game" = TRUE
- [ ] Check "Debug Mode" = TRUE
- [ ] Test new game (nothing visible during cutscene)
- [ ] Test load game (everything visible immediately)

**Tapos na! Test mo na sa Unity!** 🎉
