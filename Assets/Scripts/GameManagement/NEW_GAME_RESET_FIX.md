# NEW GAME RESET FIX - COMPLETE PROGRESS RESET

## 🐛 Issue
When clicking "New Game" from the main menu, the game was not properly resetting all progress. Players would start with:
- Previous inventory items
- Completed puzzles still marked as done
- Room-specific progress flags still set
- Dialogue triggers still active

This made it impossible to experience a true fresh start.

---

## ✅ Fix Applied

### 1. SaveSystem.cs - Added Complete Progress Reset

**New Method**: `ClearAllGameProgress()`

This method clears ALL PlayerPrefs keys used throughout the game, including:

#### Room 01 (Foyer):
- `FoyerIntro_Played`
- `Foyer_MailPickedUp`

#### Room 02 (Living Room):
- `R02_TVInteracted`
- `R02_PianoInteracted`
- `R02_LullabyPlayed`
- `R02_MrSnugglesFixed`
- `R02_SmallKeyObtained`

#### Room 03 (Hallway):
- `R03_ClosetUsed`

#### Room 04 (Kitchen):
- `kitchen_cookie_puzzle_bridge`
- `kitchen_cookie_puzzle_dough`
- `kitchen_cookie_puzzle_oven`
- `kitchen_cookie_puzzle_cookies`
- `kitchen_cookie_puzzle_recipe`
- `kitchen_cookie_puzzle_floorboard`
- `emily_kitchen_intro`
- `Room04_Bridge_Completed`
- `Room04_Bridge_Fixed`

#### Room 05 (Dining Room):
- `R05_Calendar`
- `R05_Cabinet`
- `R05_HasSpoon`
- `R05_SpoonPlaced`
- `R05_FirstHide`
- `R05_Chairs`
- `R05_ChildChair`
- `R05_MotherChair`
- `R05_FatherChair`

#### Room 06 (Return Hallway):
- `R06_IntroPlayed`
- `R06_PhotoInteracted`

#### Room 07 (Lisa's Bedroom):
- `R07_IntroPlayed`
- `R07_ToyboxOpened`
- `R07_SlidingPuzzleSolved`
- `R07_TeaPartyComplete`
- `R07_CabinetUnlocked`
- `R07_AllPuzzlesComplete`

#### Room 08 (Lisa's Bathroom):
- `R08_IntroPlayed`
- `R08_MedicineCabinetOpened`
- `R08_BathtubInteracted`
- `R08_AllEvidenceCollected`
- `R08_MirrorQTEComplete`

#### General Progress:
- `LoadSlotOnStart`
- `HasSeenIntro`
- `CurrentChapter`
- All dialogue triggers (100 possible keys)

---

### 2. InventoryManager.cs - Added Inventory Clear

**New Method**: `ClearAllItems()`

This method:
- Clears all items from the current save data's inventory list
- Refreshes the UI to show empty inventory
- Logs the clear operation for debugging

---

### 3. Integration in CreateNewGame()

The `CreateNewGame()` method now:
1. **First**: Calls `ClearAllGameProgress()` to wipe all PlayerPrefs
2. **Then**: Creates fresh GameSaveData with default values
3. **Finally**: Sets spawn position to Room01_Foyer default spawn

When the new game loads:
- SaveSystem.Start() detects the new game flag
- Calls CreateNewGame()
- Calls InventoryManager.ClearAllItems() to empty inventory
- Loads Room01_Foyer with fresh state

---

## 🎮 How It Works Now

### Player Flow:
1. Player clicks "New Game" in Main Menu
2. MainMenuManager sets `PlayerPrefs.SetInt("LoadSlotOnStart", -1)`
3. Scene loads to PersistentScene
4. SaveSystem.Start() detects the -1 flag
5. SaveSystem calls `CreateNewGame()`
6. `CreateNewGame()` calls `ClearAllGameProgress()`
7. ALL PlayerPrefs are deleted
8. Fresh GameSaveData is created
9. InventoryManager clears all items
10. Player spawns in Room01_Foyer with completely fresh state

---

## 📋 What Gets Reset

### ✅ Inventory:
- All items removed
- Empty inventory on start
- No carried-over items from previous playthrough

### ✅ Room Progress:
- All puzzles reset to unsolved
- All doors reset to locked
- All interactables reset to initial state
- All room-specific flags cleared

### ✅ Story Progress:
- All dialogue triggers reset
- All cutscenes will play again
- All intro sequences will trigger
- Emily intro sequences reset

### ✅ Puzzle States:
- Cookie puzzle (Room 04) reset
- Cabinet codes (Room 05, 07) reset
- Bridge placement (Room 04) reset
- Tea party (Room 07) reset
- All puzzle completion flags cleared

### ✅ Collectibles:
- Lullaby fragments reset to 0
- Memory fragments cleared
- All examined objects cleared

---

## 🔧 Technical Details

### Files Modified:

1. **SaveSystem.cs**:
   - Added `ClearAllGameProgress()` method
   - Modified `CreateNewGame()` to call clear method
   - Modified `Start()` to clear inventory on new game

2. **InventoryManager.cs**:
   - Added `ClearAllItems()` method
   - Clears inventory list from save data
   - Refreshes UI after clear

3. **MainMenuManager.cs**:
   - Already working correctly
   - Sets flag for new game (-1)
   - SaveSystem handles the rest

---

## 🧪 Testing Checklist

### To Test New Game Reset:

1. **Start a game and make progress**:
   - [ ] Collect some items
   - [ ] Complete a puzzle
   - [ ] Trigger some dialogues
   - [ ] Visit multiple rooms

2. **Return to Main Menu**:
   - [ ] Use pause menu or game over
   - [ ] Return to main menu

3. **Start New Game**:
   - [ ] Click "New Game" button
   - [ ] Wait for scene to load

4. **Verify Fresh Start**:
   - [ ] Inventory is empty
   - [ ] Spawn in Room01_Foyer at correct position
   - [ ] Intro cutscene plays again
   - [ ] All puzzles are unsolved
   - [ ] All doors are locked
   - [ ] No items in inventory
   - [ ] Dialogue triggers work again

---

## 🎯 Expected Behavior

### Before Fix:
- ❌ New game kept old inventory items
- ❌ Puzzles remained solved
- ❌ Doors stayed unlocked
- ❌ Dialogue didn't replay
- ❌ Room progress carried over

### After Fix:
- ✅ New game has empty inventory
- ✅ All puzzles reset to unsolved
- ✅ All doors reset to locked
- ✅ All dialogues replay
- ✅ Complete fresh start

---

## 📝 Notes for Developers

### Adding New Rooms:
When adding new rooms (Room 09, 10, etc.), remember to add their PlayerPrefs keys to `ClearAllGameProgress()`:

```csharp
// Room 09
PlayerPrefs.DeleteKey("R09_IntroPlayed");
PlayerPrefs.DeleteKey("R09_Mirror1Complete");
PlayerPrefs.DeleteKey("R09_Mirror2Complete");
// ... etc

// Room 10
PlayerPrefs.DeleteKey("R10_IntroPlayed");
PlayerPrefs.DeleteKey("R10_MirrorUnlocked");
// ... etc
```

### Best Practices:
1. **Always use PlayerPrefs for room-specific progress**
2. **Always add new keys to ClearAllGameProgress()**
3. **Test new game reset after adding new features**
4. **Document all PlayerPrefs keys used**

---

## 🚨 Important Notes

### PlayerPrefs vs SaveSystem:
- **PlayerPrefs**: Used for room-specific progress (puzzles, doors, flags)
- **SaveSystem**: Used for global progress (inventory, story, position)
- **Both must be cleared** for complete reset

### Why Two Systems:
- PlayerPrefs persist across save slots (room states)
- SaveSystem is per-save-slot (player progress)
- New game must clear both to ensure fresh start

---

## 🔍 Debugging

### If New Game Still Has Old Data:

1. **Check Console Logs**:
   - Look for "[SaveSystem] Clearing ALL game progress..."
   - Look for "[SaveSystem] All PlayerPrefs cleared for new game"
   - Look for "[InventoryManager] All items cleared for new game"

2. **Verify PlayerPrefs**:
   - Open Unity → Edit → Clear All PlayerPrefs
   - This manually clears everything for testing

3. **Check Save Files**:
   - Delete save files manually if needed
   - Location: `Application.persistentDataPath/Saves/`

4. **Verify Scene Load**:
   - Ensure PersistentScene loads correctly
   - Check that Room01_Foyer loads after

---

## ✅ Status

**Fix Status**: ✅ COMPLETE

**Files Modified**: 2
- SaveSystem.cs
- InventoryManager.cs

**Testing Status**: Ready for testing

**Compatibility**: Works with existing save system

---

## 📞 Support

If new game reset still doesn't work:
1. Check console for error messages
2. Verify all PlayerPrefs keys are in ClearAllGameProgress()
3. Test with fresh Unity build
4. Clear PlayerPrefs manually for testing

---

**Last Updated**: [Current Date]
**Fixed By**: AI Assistant
**Issue**: New Game not resetting progress
**Solution**: Complete PlayerPrefs and inventory clear on new game
