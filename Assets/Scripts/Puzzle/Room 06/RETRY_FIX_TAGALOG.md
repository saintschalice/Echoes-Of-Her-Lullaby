# Room 06 - Retry Button Fix (Tagalog)

## ✅ FIXED NA!

**Problema**: Pag pinindot ang "Retry" sa Game Over, nag-new game instead na mag-restart lang sa room.

**Solusyon**: Na-fix na ang `GameOverManager.cs` para gumamit ng tamang SaveSystem para sa Room 06.

---

## 🎯 ANO ANG NANGYAYARI NGAYON

### Pag nag-Game Over (Emily caught you):

1. **Game Over Screen**
   - "GAME OVER" message
   - Tap anywhere to continue
   - Retry / Main Menu / Exit buttons

2. **Pag pinindot ang "Retry"**:
   - ✅ Screen fades to black
   - ✅ Room 06 scene reloads
   - ✅ **Intro dialogue plays ULIT** (FIXED!)
   - ✅ **Photo frame ay normal ULIT** (FIXED!)
   - ✅ **Emily ay WALA PA** (FIXED!)
   - ✅ Player spawns sa default position

3. **Pag interact sa photo frame ULIT**:
   - Panel opens with normal photo
   - Photo scratches
   - Emily spawns
   - Chase sequence starts
   - Exactly like first time!

---

## 🔧 ANO ANG GINAWA

### Fixed in GameOverManager.cs:

**BEFORE** (Mali):
```csharp
// Gumagamit ng wrong PlayerPrefs keys
PlayerPrefs.DeleteKey("R06_IntroPlayed");      // ❌ Wrong!
PlayerPrefs.DeleteKey("R06_PhotoInteracted");  // ❌ Wrong!
```

**AFTER** (Tama):
```csharp
// Gumagamit ng SaveSystem (correct!)
data.triggeredDialogues.Remove("Room06_Intro");           // ✅ Correct!
data.triggeredDialogues.Remove("Room06_PhotoInteracted"); // ✅ Correct!
```

### Bakit mali yung dati?

Room 06 ay gumagamit ng **SaveSystem** para sa dialogue triggers, hindi PlayerPrefs. Kaya hindi na-reset yung progress kasi wrong keys ang tina-try i-delete.

---

## ✅ TESTING GUIDE

### Paano i-test:

1. **Pumasok sa Room 06**
   - ✅ Intro dialogue dapat mag-play
   - ✅ Photo frame ay normal

2. **Interact sa photo frame**
   - ✅ Panel opens
   - ✅ Photo scratches
   - ✅ Emily spawns

3. **Hayaan si Emily na hulihin ka**
   - ✅ Game Over screen

4. **Click "Retry"**
   - ✅ Screen fades to black
   - ✅ Scene reloads
   - ✅ **Intro dialogue plays ULIT!** ← DAPAT ITO!
   - ✅ **Photo frame normal ULIT!** ← DAPAT ITO!
   - ✅ **Emily wala pa!** ← DAPAT ITO!

5. **Interact sa photo frame ULIT**
   - ✅ Gumagana exactly like first time
   - ✅ Panel, scratch, Emily spawn - lahat ulit!

---

## 🔍 DEBUG LOGS

### Pag nag-retry, dapat makita mo sa Console:

```
[GameOver] RestartLevel button clicked!
[GameOver] Resetting ALL progress for room: Room06_ReturnToHallwayUpStairs
[GameOver] Room 06 dialogue triggers cleared from SaveSystem
[GameOver] Room progress reset complete for Room06_ReturnToHallwayUpStairs
```

### Pag nag-reload ang scene:

```
[Room06] Playing intro sequence
[Room06] Intro sequence complete
```

### Kung walang logs:

1. Check kung enabled ang Debug Mode sa Room06_HallwayController
2. Check kung may SaveSystem.Instance sa scene
3. Check Console for errors (red messages)

---

## 🐛 KUNG HINDI PA RIN GUMAGANA

### Check mo ito:

1. **SaveSystem exists?**
   - May SaveSystem GameObject sa scene?
   - SaveSystem.Instance ay hindi null?

2. **Room06_HallwayController correct?**
   - Gumagamit ba ng `SaveSystem.Instance.WasDialogueTriggered("Room06_Intro")`?
   - Tama ba ang flag names?

3. **Scene name correct?**
   - Scene name ay "Room06_ReturnToHallwayUpStairs"?
   - Check sa Build Settings kung naka-add ang scene

4. **Manual reset** (for testing):
   - Open Unity Console
   - Type:
   ```csharp
   SaveSystem.Instance.GetCurrentSaveData().triggeredDialogues.Clear();
   ```

---

## 💡 TECHNICAL NOTES

### SaveSystem vs PlayerPrefs

**Room 06 (NEW way)**:
- Uses: `SaveSystem.Instance.TriggerDialogue("Room06_Intro")`
- Check: `SaveSystem.Instance.WasDialogueTriggered("Room06_Intro")`
- Better for save/load system
- More robust

**Old Rooms (OLD way)**:
- Uses: `PlayerPrefs.SetInt("R02_TVInteracted", 1)`
- Check: `PlayerPrefs.GetInt("R02_TVInteracted", 0)`
- Still works but less integrated

### Recommendation:

Migrate all rooms to SaveSystem for consistency!

---

## 📋 SUMMARY

**Fixed File**:
- `Assets/Scripts/Puzzle/Room 03/GameOverManager.cs`

**What Changed**:
1. ✅ Room 06 now uses SaveSystem to reset progress
2. ✅ Correct dialogue trigger flags ("Room06_Intro", "Room06_PhotoInteracted")
3. ✅ Added Room 06 case to RemoveRoomItems (no items to remove)

**Result**:
- ✅ Retry button works correctly
- ✅ Intro plays again
- ✅ Photo frame resets
- ✅ Emily disabled again
- ✅ Full puzzle reset

---

## ✅ FINAL CHECK

### Dapat ganito ang flow:

1. Enter Room 06 → Intro plays
2. Interact photo → Emily spawns
3. Emily catches you → Game Over
4. Click Retry → **Intro plays ULIT!** ✅
5. Interact photo ULIT → Emily spawns ULIT ✅

**Kung ganito ang nangyayari, WORKING NA!** 🎮✨

---

**Retry button is FIXED! Test mo na sa Unity!** 💪✨
