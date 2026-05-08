# Dining Room Spawn Fix - Tagalog Guide

## PROBLEMA
Hindi nag-spawn si Lisa sa main spawn point ng Dining Room. Nag-spawn siya sa gilid instead of sa default position.

## ROOT CAUSE
Ang `RoomExit` script ay hindi nag-set ng `TargetSpawnPoint`, kaya ang `PersistentSpawnManager` ay gumagamit ng saved position (last position sa previous scene) instead of default spawn point.

---

## SOLUSYON

### Code Fix: RoomExit.cs
Added `targetSpawnPointID` field para ma-specify kung saan dapat mag-spawn si Lisa sa next scene.

**New Field**:
```csharp
[Header("Spawn Settings")]
[Tooltip("Leave empty to use default spawn point in next scene")]
public string targetSpawnPointID = "";
```

**New Logic**:
```csharp
// Set target spawn point for next scene (if specified)
if (!string.IsNullOrEmpty(targetSpawnPointID))
{
    PlayerPrefs.SetString("TargetSpawnPoint", targetSpawnPointID);
    Debug.Log($"[RoomExit] Set target spawn point: {targetSpawnPointID}");
}
else
{
    // Clear any previous spawn point to use default
    PlayerPrefs.SetString("TargetSpawnPoint", "");
    Debug.Log("[RoomExit] Using default spawn point in next scene");
}
```

---

## SETUP IN UNITY EDITOR

### Step 1: Find RoomExit GameObject
1. Open **Room04_KitchenDining** scene
2. Sa Hierarchy, hanapin ang **RoomExit** GameObject
   - Usually nasa exit door or transition area

### Step 2: Configure RoomExit Component
```
Inspector Settings:
├─ Next Scene Name: "Room05_DiningRoom"
├─ Target Spawn Point ID: "" (LEAVE EMPTY for default spawn)
│   └─ Or specify: "Main" kung may specific spawn point
├─ Fade Out Duration: 0.8
└─ Fade In Duration: 0.8
```

**IMPORTANT**: Leave `Target Spawn Point ID` **EMPTY** to use the default spawn point!

### Step 3: Verify Dining Room Spawn Points
1. Open **Room05_DiningRoom** scene
2. Sa Hierarchy, hanapin ang **SpawnPoints** (or similar)
3. Check kung may spawn point na:
   - `isDefaultSpawnPoint` = ✓ CHECKED
   - `roomName` = "Room05_DiningRoom"
   - `spawnPointID` = "Main" (or any name)

### Step 4: Test
1. Play from Kitchen scene
2. Walk to exit door
3. Should transition to Dining Room
4. **Lisa should spawn at default spawn point** (not sa gilid)

---

## HOW IT WORKS

### Before Fix:
```
Player exits Kitchen
    ↓
RoomExit triggers
    ↓
Saves current position (sa gilid ng kitchen)
    ↓
Loads Dining Room
    ↓
PersistentSpawnManager checks TargetSpawnPoint
    ↓
TargetSpawnPoint is empty
    ↓
Uses saved position (wrong!)
    ↓
Lisa spawns sa gilid ❌
```

### After Fix:
```
Player exits Kitchen
    ↓
RoomExit triggers
    ↓
Clears TargetSpawnPoint (sets to "")
    ↓
Loads Dining Room
    ↓
PersistentSpawnManager checks TargetSpawnPoint
    ↓
TargetSpawnPoint is empty (cleared)
    ↓
Uses DEFAULT spawn point ✓
    ↓
Lisa spawns sa main spawn point ✓
```

---

## ALTERNATIVE: Specify Spawn Point

Kung gusto mong mag-spawn si Lisa sa specific spawn point (not default):

### In RoomExit Inspector:
```
Target Spawn Point ID: "FromKitchen"
```

### In Dining Room Scene:
Create spawn point with:
```
Room Name: "Room05_DiningRoom"
Spawn Point ID: "FromKitchen"
Is Default Spawn Point: ✗ (unchecked)
```

---

## TESTING CHECKLIST

### Test 1: Kitchen to Dining Room
- [ ] Start in Kitchen scene
- [ ] Walk to exit door
- [ ] Fade out transition
- [ ] Scene loads
- [ ] **Lisa spawns at main spawn point** (center/default position)
- [ ] Fade in transition
- [ ] Lisa can move normally

### Test 2: Other Room Transitions
- [ ] Test all RoomExit objects in all scenes
- [ ] Verify each one has proper spawn point setup
- [ ] Check console logs for spawn point messages

### Console Logs to Check:
```
[RoomExit] Using default spawn point in next scene
[PersistentSpawn] Positioned player at: Main in Room05_DiningRoom
```

---

## COMMON ISSUES

### Issue 1: Lisa still spawns sa gilid
**CAUSE**: May naka-set na value sa `Target Spawn Point ID`
**FIX**: 
1. Select RoomExit GameObject
2. Clear ang `Target Spawn Point ID` field (leave empty)
3. Test again

### Issue 2: Lisa spawns sa wrong position
**CAUSE**: Walang default spawn point sa Dining Room
**FIX**:
1. Open Room05_DiningRoom scene
2. Find spawn point GameObject
3. Check `Is Default Spawn Point` checkbox
4. Set `Room Name` to "Room05_DiningRoom"

### Issue 3: Multiple default spawn points
**CAUSE**: May multiple spawn points na naka-check ang `Is Default Spawn Point`
**FIX**:
1. Only ONE spawn point should be default per scene
2. Uncheck others
3. Keep only the main entrance as default

---

## APPLY TO OTHER SCENES

This fix can be applied to ALL RoomExit objects:

### Scenes to Check:
- [ ] Room01_Foyer → Room02_LivingRoom
- [ ] Room02_LivingRoom → Room03_Hallway
- [ ] Room03_Hallway → Room04_Kitchen
- [ ] **Room04_Kitchen → Room05_DiningRoom** ✓ (fixed)
- [ ] Room05_DiningRoom → Room06_ReturnToHallway
- [ ] Room06 → Room07_Lisa'sBedroom
- [ ] Room07 → Room08_Lisa'sBathroom
- [ ] Room08 → Room09_Master'sBathroom

### For Each RoomExit:
1. Check if `Target Spawn Point ID` is empty (for default spawn)
2. Or set specific spawn point ID if needed
3. Test transition

---

## SUMMARY

**What Changed**:
- RoomExit.cs now has `targetSpawnPointID` field
- If empty, clears TargetSpawnPoint to use default spawn
- If specified, sets TargetSpawnPoint to specific spawn point

**What to Do**:
1. Leave `Target Spawn Point ID` empty in RoomExit (Kitchen)
2. Verify default spawn point exists in Dining Room
3. Test transition

**Result**:
- Lisa spawns at correct main spawn point
- No more spawning sa gilid
- Proper room transitions

Tapos na! 🎮
