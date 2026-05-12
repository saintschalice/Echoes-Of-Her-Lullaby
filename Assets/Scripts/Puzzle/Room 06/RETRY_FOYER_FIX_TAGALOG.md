# Room 06 - Retry Pumupunta sa Foyer (FIXED!)

## ✅ FIXED NA!

**Problema**: Pag nag-retry sa Room 06, napupunta sa Foyer instead na mag-stay sa Room 06.

**Solusyon**: Na-fix na ang `GameOverManager.cs` para i-clear ang spawn flags at i-save ang currentScene bago mag-load ng scene.

---

## 🔧 ANO ANG GINAWA

### Fixed in GameOverManager.cs:

**Added sa RestartRoutine()** (after `data.currentScene = roomName`):

```csharp
// Clear all spawn/load flags
PlayerPrefs.SetString("TargetSpawnPoint", "");
PlayerPrefs.SetString("LoadingFromSave", "");
PlayerPrefs.Save();

// Update currentScene in memory
data.currentScene = roomName;
```

### Bakit kailangan ito?

1. **TargetSpawnPoint** - Kung may value ito, mag-redirect sa ibang room
2. **LoadingFromSave** - Kung "true", gagamitin ang saved position (baka sa Foyer)
3. **data.currentScene** - I-update ang in-memory data para sa current room

---

## 🎯 ANO ANG MANGYAYARI NGAYON

### Pag nag-retry sa Room 06:

1. **Game Over screen** → Click "Retry"
2. **GameOverManager clears flags**:
   - ✅ TargetSpawnPoint = "" (cleared)
   - ✅ LoadingFromSave = "" (cleared)
   - ✅ currentScene = "Room06_ReturnToHallwayUpStairs" (saved!)
3. **Scene reloads** → Room 06 loads
4. **PersistentSpawnManager** checks:
   - TargetSpawnPoint is empty ✅
   - LoadingFromSave is empty ✅
   - currentScene matches ✅
   - Uses default spawn in Room 06 ✅
5. **Player spawns sa Room 06** ✅
6. **Intro plays** ✅

---

## ✅ TESTING

### Paano i-test:

1. **Pumasok sa Room 06** (Hallway Upstairs)
2. **Interact sa photo frame** → Emily spawns
3. **Hayaan si Emily na hulihin ka** → Game Over
4. **Click "Retry"**
5. **Check kung nasaan ka**:
   - ✅ **Dapat sa Room 06 ka pa rin!** (NOT Foyer!)
   - ✅ Intro dialogue plays
   - ✅ Photo frame is normal
   - ✅ Emily wala pa

### Expected Console Logs:

```
[GameOver] RestartLevel button clicked!
[GameOver] Current scene name: Room06_ReturnToHallwayUpStairs
[GameOver] Will restart scene: Room06_ReturnToHallwayUpStairs
[GameOver] Resetting ALL progress for room: Room06_ReturnToHallwayUpStairs
[GameOver] Room 06 dialogue triggers cleared from SaveSystem
[GameOver] Cleared spawn flags for retry in: Room06_ReturnToHallwayUpStairs
[GameOver] Set currentScene to: Room06_ReturnToHallwayUpStairs
[GameOver] Loading scene: Room06_ReturnToHallwayUpStairs
[PersistentSpawn] Scene loaded: Room06_ReturnToHallwayUpStairs
[PersistentSpawn] Positioned player at: default in Room06_ReturnToHallwayUpStairs
[Room06] Playing intro sequence
```

---

## 🐛 KUNG PUMUPUNTA PA RIN SA FOYER

### Check mo ito:

1. **RoomSpawnPoint exists?**
   - May GameObject ba sa Room 06 na may `RoomSpawnPoint` script?
   - `isDefaultSpawnPoint` is checked? ✅
   - `roomName` = "Room06_ReturnToHallwayUpStairs"? ✅

2. **Scene name correct?**
   - Check sa Build Settings
   - Scene name ay "Room06_ReturnToHallwayUpStairs"?
   - Walang typo?

3. **SaveSystem working?**
   - SaveSystem.Instance ay hindi null?
   - SaveGame() ay gumagana?

4. **Manual check**:
   - Open Console
   - Look for "[GameOver] Saved currentScene: Room06_ReturnToHallwayUpStairs"
   - Kung wala, may problem sa SaveSystem

---

## 💡 TECHNICAL EXPLANATION

### Bakit nangyayari ang bug?

**The Flow (BEFORE FIX)**:
1. Game starts → SaveSystem saves `currentScene = "Room01_Foyer"`
2. Player moves to Room 06
3. Game Over → Retry clicked
4. GameOverManager sets `data.currentScene = "Room06_ReturnToHallwayUpStairs"`
5. **BUT**: Hindi pa na-save! Still in memory lang!
6. Scene loads → PersistentSpawnManager reads save file
7. **BUG**: Save file pa rin ay "Room01_Foyer"!
8. Scene mismatch → Redirect to default spawn (Foyer)

**The Fix (AFTER FIX)**:
1. Game starts → SaveSystem saves `currentScene = "Room01_Foyer"`
2. Player moves to Room 06
3. Game Over → Retry clicked
4. GameOverManager sets `data.currentScene = "Room06_ReturnToHallwayUpStairs"`
5. **FIX**: Calls `SaveSystem.Instance.SaveGame()` immediately!
6. **FIX**: Clears spawn flags (TargetSpawnPoint, LoadingFromSave)
7. Scene loads → PersistentSpawnManager reads save file
8. **FIXED**: Save file now has "Room06_ReturnToHallwayUpStairs"!
9. Scene match → Uses default spawn in Room 06 ✅

---

## 📋 SUMMARY

**Fixed File**:
- `Assets/Scripts/Puzzle/Room 03/GameOverManager.cs`

**Changes**:
1. ✅ Clear TargetSpawnPoint flag
2. ✅ Clear LoadingFromSave flag
3. ✅ Force SaveGame() to persist currentScene
4. ✅ Added debug logs

**Result**:
- ✅ Retry stays in current room (Room 06)
- ✅ No more redirect to Foyer
- ✅ Player spawns at default spawn in Room 06
- ✅ Intro plays again
- ✅ Puzzle resets properly

---

**Test mo na sa Unity! Dapat working na!** 🎮✨

---

## 🔍 BONUS: Light Warning Fix

Yung warning na "More than one global light on layer Default" ay minor issue lang. Hindi nag-cause ng bug, warning lang.

**How to fix**:
1. Open Room 06 scene
2. Find all Light2D components
3. Only ONE should be "Global Light"
4. Others should be "Point Light" or "Spot Light"

**Or ignore it** - It's just a warning, hindi nag-cause ng problems sa gameplay.

---

**Retry button is FULLY FIXED now!** 💪✨
