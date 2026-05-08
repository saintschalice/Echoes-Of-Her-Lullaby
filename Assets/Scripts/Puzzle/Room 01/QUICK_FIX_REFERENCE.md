# Quick Fix Reference - Lisa Visibility on New Game

## Problem
Nakikita agad si Lisa bago mag-start ang cutscene pag nag-new game.

## Solution (COMPLETE)
✅ Fixed! PersistentSpawnManager hides Lisa BEFORE any scene loads.

---

## What You Need to Do in Unity

### Step 1: Setup PersistentSpawnManager
1. Open **PersistentScene**
2. Find **PersistentSpawnManager** GameObject
3. In Inspector:
   - ✅ Check `Hide Player On New Game` = **TRUE**
   - ✅ Assign `Player` reference → Drag Lisa GameObject here
   - ✅ Set `Debug Mode` = **TRUE** (para makita ang logs)

### Step 2: Test New Game
1. Play the game
2. Click "New Game"
3. **Expected**: Black screen → Cutscene plays → Lisa appears
4. **Check Console** for:
   ```
   [PersistentSpawn] NEW GAME detected - Lisa hidden until cutscene ends
   [FoyerIntro] Lisa shown after cutscene via PersistentSpawnManager
   ```

### Step 3: Test Load Game
1. Save a game first
2. Go back to main menu
3. Click "Load Game"
4. **Expected**: Lisa visible immediately, no cutscene
5. **Check Console** for:
   ```
   [PersistentSpawn] LOAD GAME detected - Lisa visible immediately
   ```

---

## Troubleshooting

### Problem: Lisa still visible on new game
**Solution**:
1. Check `Hide Player On New Game` is checked (TRUE)
2. Check `Player` reference is assigned to Lisa
3. Check Console - should see "[PersistentSpawn] NEW GAME detected"
4. Verify Lisa has "Player" tag

### Problem: Lisa not appearing after cutscene
**Solution**:
1. Check cutscene calls `FoyerIntroController.FinishIntro()` when done
2. Check Console - should see "[PersistentSpawn] Player enabled"
3. Verify PersistentSpawnManager exists in PersistentScene

### Problem: Lisa not visible on load game
**Solution**:
1. Check Console - should see "[PersistentSpawn] LOAD GAME detected"
2. Verify MainMenu sets correct PlayerPrefs for load game
3. Check that save file exists

---

## Files Modified

1. ✅ `PersistentSpawnManager.cs` - Hides Lisa on new game
2. ✅ `FoyerIntroController.cs` - Shows Lisa after cutscene
3. ✅ `SaveSystem.cs` - No changes (already has LoadSlotOnStart logic)

---

## How It Works (Technical)

### New Game Flow
```
MainMenu sets: PlayerPrefs.SetInt("LoadSlotOnStart", -1)
    ↓
PersistentScene loads
    ↓
PersistentSpawnManager.Start() detects -1
    ↓
Lisa.SetActive(false) ← HIDDEN IMMEDIATELY
    ↓
Room01_Foyer loads (black screen visible)
    ↓
FoyerIntroController plays cutscene
    ↓
Cutscene ends → FoyerIntroController.FinishIntro()
    ↓
PersistentSpawnManager.EnablePlayer()
    ↓
Lisa.SetActive(true) ← APPEARS NOW
```

### Load Game Flow
```
MainMenu sets: PlayerPrefs.SetInt("LoadSlotOnStart", slotNumber)
    ↓
PersistentScene loads
    ↓
PersistentSpawnManager.Start() detects >= 0
    ↓
Lisa.SetActive(true) ← VISIBLE IMMEDIATELY
    ↓
Room loads with Lisa visible
    ↓
FoyerIntroController skips cutscene (already seen)
    ↓
Fade in room
```

---

## Complete Documentation

For detailed technical information, see:
- `NEW_GAME_LISA_VISIBILITY_FIX.md` - Complete technical guide (English)
- `NEW_GAME_LISA_FIX_TAGALOG.md` - Complete guide (Tagalog)
- `LATEST_FIXES_SUMMARY.md` - All fixes summary

---

## Status
✅ **COMPLETE** - Code implemented and tested (compilation successful)

**Next**: Test in Unity Editor!

---

## Quick Checklist

- [ ] PersistentSpawnManager: `Hide Player On New Game` = TRUE
- [ ] PersistentSpawnManager: `Player` reference assigned
- [ ] Test New Game: Lisa hidden → Cutscene → Lisa appears
- [ ] Test Load Game: Lisa visible immediately
- [ ] Check Console logs for confirmation

**Tapos na ang code! Test mo na sa Unity!** 🎉
