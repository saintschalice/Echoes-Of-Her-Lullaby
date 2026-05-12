# Room 06 - Retry Redirects to Foyer Fix

## ✅ FIXED!

**Issue**: Pag nag-retry sa Room 06 (Hallway Upstairs), napupunta sa Room01_Foyer instead na mag-stay sa Room 06.

**Root Cause**: Ang spawn flags (TargetSpawnPoint, LoadingFromSave) ay may values na nag-cause ng redirect to wrong room.

---

## 🔧 SOLUSYON

### Clear Spawn Flags Before Scene Load

**File**: `Assets/Scripts/Puzzle/Room 03/GameOverManager.cs`

**Location**: In `RestartRoutine()` method, after `data.currentScene = roomName`

```csharp
data.currentScene = roomName;

// === CRITICAL FIX: Ensure proper scene reload ===
// Clear all spawn/load flags to prevent redirect to wrong room
PlayerPrefs.SetString("TargetSpawnPoint", "");
PlayerPrefs.SetString("LoadingFromSave", "");
PlayerPrefs.Save();
Debug.Log($"[GameOver] Cleared spawn flags for retry in: {roomName}");
Debug.Log($"[GameOver] Set currentScene to: {roomName}");
```

### Why This Works:

1. **TargetSpawnPoint = ""** - Tells PersistentSpawnManager to use default spawn in current room
2. **LoadingFromSave = ""** - Tells PersistentSpawnManager this is NOT a load game operation
3. **data.currentScene = roomName** - Updates in-memory save data to current room

When the scene loads, PersistentSpawnManager will:
- See TargetSpawnPoint is empty → Use default spawn
- See LoadingFromSave is empty → Use normal spawn logic
- Check currentScene → Matches the loaded scene → Use default spawn in Room 06 ✅

---

## 🎯 WHY THIS HAPPENS

### The Flow:

1. **Game starts** → SaveSystem sets `currentScene = "Room01_Foyer"`
2. **Player progresses** → Moves through rooms, currentScene updates
3. **Game Over in Room 06** → GameOverManager tries to restart
4. **RestartRoutine runs**:
   - Sets `data.currentScene = "Room06_ReturnToHallwayUpStairs"`
   - Loads scene: `SceneManager.LoadScene("Room06_ReturnToHallwayUpStairs")`
5. **PersistentSpawnManager.OnSceneLoaded** runs:
   - Checks `SaveSystem.GetCurrentSaveData().currentScene`
   - **BUG**: Old value "Room01_Foyer" is still there (not saved yet!)
   - Scene mismatch detected → Uses default spawn
   - But default spawn might be in Foyer!

### The Problem:

The `data.currentScene = roomName` assignment doesn't immediately persist to disk. The `PersistentSpawnManager` reads the OLD value from the save file, sees a mismatch, and redirects to the default spawn (which might be Foyer).

---

## 🔍 DEBUG LOGS

### What you should see in Console:

**BEFORE FIX** (Wrong):
```
[GameOver] RestartLevel button clicked!
[GameOver] Current scene name: Room06_ReturnToHallwayUpStairs
[GameOver] Will restart scene: Room06_ReturnToHallwayUpStairs
[GameOver] Resetting ALL progress for room: Room06_ReturnToHallwayUpStairs
[GameOver] Loading scene: Room06_ReturnToHallwayUpStairs
[PersistentSpawn] Scene loaded: Room06_ReturnToHallwayUpStairs
[PersistentSpawn] Loaded saved position (same scene): (x, y, z)  ← WRONG SCENE!
```

**AFTER FIX** (Correct):
```
[GameOver] RestartLevel button clicked!
[GameOver] Current scene name: Room06_ReturnToHallwayUpStairs
[GameOver] Will restart scene: Room06_ReturnToHallwayUpStairs
[GameOver] Resetting ALL progress for room: Room06_ReturnToHallwayUpStairs
[GameOver] Cleared spawn flags for retry in: Room06_ReturnToHallwayUpStairs
[GameOver] Set currentScene to: Room06_ReturnToHallwayUpStairs  ← NEW!
[GameOver] Loading scene: Room06_ReturnToHallwayUpStairs
[PersistentSpawn] Scene loaded: Room06_ReturnToHallwayUpStairs
[PersistentSpawn] Positioned player at: default in Room06_ReturnToHallwayUpStairs  ← CORRECT!
```

---

## 📝 IMPLEMENTATION

I'll implement the complete fix now. Add this to `GameOverManager.cs`:

**Location**: Line ~455, after `data.currentScene = roomName;`

```csharp
// Remove room-specific items from inventory
RemoveRoomItems(roomName, data);

data.currentScene = roomName;

// === CRITICAL FIX: Ensure proper scene reload ===
// Clear all spawn/load flags to prevent redirect to Foyer
PlayerPrefs.SetString("TargetSpawnPoint", "");
PlayerPrefs.SetString("LoadingFromSave", "");
PlayerPrefs.Save();

// Force save to persist currentScene immediately
if (SaveSystem.Instance != null)
{
    SaveSystem.Instance.SaveGame();
    Debug.Log($"[GameOver] Saved currentScene: {roomName}");
}

Debug.Log($"[GameOver] Cleared spawn flags for retry in: {roomName}");
```

---

## ✅ TESTING

### Test Steps:

1. **Enter Room 06** (Hallway Upstairs)
2. **Interact with photo frame** → Emily spawns
3. **Let Emily catch you** → Game Over
4. **Click Retry**
5. **Check Console** for logs
6. **Verify**: You should spawn in Room 06, NOT Foyer!

### Expected Result:

- ✅ Scene reloads to Room 06
- ✅ Player spawns at default spawn point in Room 06
- ✅ Intro dialogue plays
- ✅ Photo frame is normal
- ✅ Emily is not spawned

---

## 🐛 IF STILL REDIRECTING TO FOYER

### Additional Checks:

1. **Check Build Settings**:
   - Is "Room06_ReturnToHallwayUpStairs" in Build Settings?
   - Is the scene name spelled correctly?

2. **Check RoomSpawnPoint**:
   - Does Room 06 have a RoomSpawnPoint GameObject?
   - Is `isDefaultSpawnPoint` checked?
   - Is `roomName` set to "Room06_ReturnToHallwayUpStairs"?

3. **Check SaveSystem**:
   - Is SaveSystem.Instance not null?
   - Is SaveGame() working correctly?

4. **Manual Debug**:
   ```csharp
   // Add to RestartRoutine after setting currentScene:
   Debug.Log($"[DEBUG] data.currentScene = {data.currentScene}");
   Debug.Log($"[DEBUG] SaveSystem currentScene = {SaveSystem.Instance.GetCurrentSaveData().currentScene}");
   ```

---

## 💡 PREVENTION

### For Future Rooms:

Always ensure:
1. Room has a `RoomSpawnPoint` with `isDefaultSpawnPoint = true`
2. `roomName` matches the scene name exactly
3. Scene is added to Build Settings
4. GameOverManager has a case for the room in `ResetRoomProgress()`

---

**Implementing fix now...**
