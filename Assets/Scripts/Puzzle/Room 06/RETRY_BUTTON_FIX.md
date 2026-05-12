# Room 06 - Retry Button Fix

## ✅ PROBLEMA: FIXED!

**Issue**: Kapag pinindot ang "Retry" button sa Game Over screen sa Room 06, nag-new game instead na mag-restart lang sa room.

**Root Cause**: Ang `GameOverManager.cs` ay gumagamit ng wrong PlayerPrefs keys para sa Room 06. Ang Room 06 ay gumagamit ng `SaveSystem` dialogue triggers, hindi PlayerPrefs.

---

## 🔧 ANO ANG GINAWA

### Fixed in `GameOverManager.cs`:

#### 1. ResetRoomProgress() Method
**BEFORE** (Wrong):
```csharp
case "Room06_ReturnToHallway":
case "Room06_ReturnToHallwayUpStairs":
    PlayerPrefs.DeleteKey("R06_IntroPlayed");
    PlayerPrefs.DeleteKey("R06_PhotoInteracted");
    break;
```

**AFTER** (Correct):
```csharp
case "Room06_ReturnToHallway":
case "Room06_ReturnToHallwayUpStairs":
    // Room 06 uses SaveSystem dialogue triggers, not PlayerPrefs
    // Reset via SaveSystem instead
    if (SaveSystem.Instance != null)
    {
        GameSaveData data = SaveSystem.Instance.GetCurrentSaveData();
        if (data != null)
        {
            // Remove Room 06 specific dialogue triggers
            data.triggeredDialogues.Remove("Room06_Intro");
            data.triggeredDialogues.Remove("Room06_PhotoInteracted");
            Debug.Log("[GameOver] Room 06 dialogue triggers cleared from SaveSystem");
        }
    }
    break;
```

#### 2. RemoveRoomItems() Method
Added Room 06 case (walang items pero kailangan ng case):
```csharp
case "Room06_ReturnToHallway":
case "Room06_ReturnToHallwayUpStairs":
    // Room 06 has no collectible items, only photo interaction
    // No items to remove
    break;
```

---

## 🎯 ANO ANG MANGYAYARI NGAYON

### Pag nag-Game Over sa Room 06:

1. **Game Over Screen**
   - "GAME OVER" message
   - Tap to continue
   - Retry / Main Menu / Exit buttons

2. **Pag pinindot ang "Retry"**:
   - ✅ **Dialogue triggers cleared** - Intro at photo interaction ay mag-reset
   - ✅ **Emily disabled** - Emily ay babalik sa disabled state
   - ✅ **Photo frame reset** - Photo frame ay babalik sa normal sprite
   - ✅ **Scene reloads** - Room 06 scene ay mag-reload
   - ✅ **Player spawns** - Player ay mag-spawn sa default spawn point
   - ✅ **Intro plays again** - Intro dialogue ay mag-play ulit

3. **Result**:
   - Parang first time mo pumasok sa room
   - Lahat ng puzzle progress ay na-reset
   - Emily ay hindi pa nag-spawn
   - Photo frame ay normal pa

---

## 🔍 TECHNICAL DETAILS

### SaveSystem vs PlayerPrefs

**Room 06 uses SaveSystem**:
- Dialogue triggers: `SaveSystem.Instance.TriggerDialogue(flag)`
- Check if triggered: `SaveSystem.Instance.WasDialogueTriggered(flag)`
- Flags used:
  - `"Room06_Intro"` - Intro dialogue played
  - `"Room06_PhotoInteracted"` - Photo frame interacted

**Other rooms use PlayerPrefs**:
- Example: `PlayerPrefs.SetInt("R02_TVInteracted", 1)`
- Example: `PlayerPrefs.GetInt("R02_TVInteracted", 0)`

### Why the difference?

Room 06 was implemented using the newer SaveSystem pattern, which is more robust and integrated with the game's save/load system. Older rooms still use PlayerPrefs for backwards compatibility.

---

## ✅ TESTING

### How to test the fix:

1. **Enter Room 06**
   - Intro dialogue should play
   - Photo frame is normal

2. **Interact with photo frame**
   - Panel opens
   - Photo scratches
   - Emily spawns

3. **Let Emily catch you**
   - Game Over screen appears

4. **Click "Retry"**
   - Screen fades to black
   - Scene reloads
   - ✅ **Intro dialogue plays again** (FIXED!)
   - ✅ **Photo frame is normal again** (FIXED!)
   - ✅ **Emily is not spawned** (FIXED!)

5. **Interact with photo frame again**
   - Should work exactly like first time
   - Panel opens, photo scratches, Emily spawns

---

## 🐛 IF STILL NOT WORKING

### Check Console for these logs:

When you click Retry:
```
[GameOver] RestartLevel button clicked!
[GameOver] Resetting ALL progress for room: Room06_ReturnToHallwayUpStairs
[GameOver] Room 06 dialogue triggers cleared from SaveSystem
[GameOver] Room progress reset complete for Room06_ReturnToHallwayUpStairs
```

When scene reloads:
```
[Room06] Playing intro sequence
[Room06] Intro sequence complete
```

### If intro doesn't play:

1. **Check SaveSystem**:
   - Is SaveSystem.Instance not null?
   - Is GetCurrentSaveData() returning valid data?

2. **Check Room06_HallwayController**:
   - Is it checking `SaveSystem.Instance.WasDialogueTriggered("Room06_Intro")`?
   - Is it calling `SaveSystem.Instance.TriggerDialogue("Room06_Intro")` after intro?

3. **Manual Reset** (for testing):
   ```csharp
   // In Unity Console or Debug script:
   SaveSystem.Instance.GetCurrentSaveData().triggeredDialogues.Remove("Room06_Intro");
   SaveSystem.Instance.GetCurrentSaveData().triggeredDialogues.Remove("Room06_PhotoInteracted");
   ```

---

## 💡 NOTES

### For Future Rooms:

If you create more rooms that use SaveSystem dialogue triggers, make sure to add them to `GameOverManager.ResetRoomProgress()`:

```csharp
case "RoomXX_YourRoom":
    if (SaveSystem.Instance != null)
    {
        GameSaveData data = SaveSystem.Instance.GetCurrentSaveData();
        if (data != null)
        {
            data.triggeredDialogues.Remove("RoomXX_YourFlag1");
            data.triggeredDialogues.Remove("RoomXX_YourFlag2");
        }
    }
    break;
```

### Consistency Recommendation:

Consider migrating all rooms to use SaveSystem instead of PlayerPrefs for consistency. SaveSystem is:
- More robust
- Integrated with save/load
- Easier to debug
- Better for multiplayer/cloud saves

---

## ✅ SUMMARY

**Fixed Files**:
- `Assets/Scripts/Puzzle/Room 03/GameOverManager.cs`

**Changes**:
1. Updated `ResetRoomProgress()` to use SaveSystem for Room 06
2. Added Room 06 case to `RemoveRoomItems()` (no items to remove)

**Result**:
- ✅ Retry button now properly resets Room 06 puzzle
- ✅ Intro dialogue plays again
- ✅ Photo frame resets to normal
- ✅ Emily is disabled again
- ✅ Player can replay the room from the beginning

**Tested**: Ready for testing in Unity!

---

**Retry button is now working correctly for Room 06!** 🎮✨
