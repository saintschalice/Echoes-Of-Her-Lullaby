# How to Change Spawn Position - Tagalog Guide

## SIMPLE GUIDE: Paano Baguhin ang Spawn Position

**WALANG KAILANGAN I-EDIT SA CODE!** Just move the GameObject sa Unity Editor.

---

## STEP-BY-STEP

### Step 1: Open the Scene
1. Sa Unity, open ang scene na gusto mong baguhin
   - Example: `Room05_DiningRoom`

### Step 2: Find the Spawn Point GameObject
1. Sa **Hierarchy**, hanapin ang spawn point
   - Usually named: `SpawnPoint`, `SpawnPoint_Main`, `PlayerSpawn`, etc.
   - Or nasa folder: `SpawnPoints` > `SpawnPoint_Main`

2. **Tip**: Kung hindi mo makita, gamitin ang search:
   - Click sa Hierarchy search box
   - Type: `spawn`
   - Lalabas lahat ng spawn points

### Step 3: Move the Spawn Point
1. **Select** ang SpawnPoint GameObject
2. Sa **Scene View**, makikita mo ang position niya (usually may icon/gizmo)
3. **Move** using any of these methods:

#### Method A: Drag in Scene View
- Click and drag ang GameObject sa Scene View
- I-position kung saan mo gusto mag-spawn si Lisa

#### Method B: Use Move Tool
- Press **W** key (Move tool)
- Drag ang arrows (X/Y axis) para i-move
- Or drag ang center para free move

#### Method C: Set Exact Position
- Sa **Inspector**, hanapin ang **Transform** component
- I-edit ang **Position** values:
  ```
  Position:
  ├─ X: [horizontal position]
  ├─ Y: [vertical position]
  └─ Z: 0 (always 0 for 2D)
  ```

### Step 4: Verify
1. **Play** ang scene
2. Si Lisa dapat mag-spawn sa new position
3. Kung mali pa rin, check kung tama ang spawn point na na-move mo

---

## EXAMPLE: Dining Room Spawn

### Current Problem:
Lisa spawns sa gilid (wrong position)

### Solution:
1. Open `Room05_DiningRoom` scene
2. Find `SpawnPoint_Main` (or similar)
3. Move it to center/entrance of room
4. Example position:
   ```
   Position:
   X: 0
   Y: -2
   Z: 0
   ```

### Visual Guide:
```
Before (spawning sa gilid):
┌─────────────────────┐
│                     │
│  [Table]            │
│                     │
│              Lisa ← │ Wrong!
└─────────────────────┘

After (spawning sa center):
┌─────────────────────┐
│                     │
│  [Table]            │
│      Lisa ←         │ Correct!
│                     │
└─────────────────────┘
```

---

## IMPORTANT SETTINGS

### Make Sure These Are Set:

#### In SpawnPoint Inspector:
```
RoomSpawnPoint Component:
├─ Room Name: "Room05_DiningRoom" (exact scene name!)
├─ Spawn Point ID: "Main" (or any unique name)
├─ Is Default Spawn Point: ✓ CHECKED (for main entrance)
└─ Match Rotation: ✗ (usually unchecked for 2D)
```

**CRITICAL**: 
- `Room Name` MUST match the exact scene name
- Only ONE spawn point should have `Is Default Spawn Point` checked per scene

---

## MULTIPLE SPAWN POINTS

Kung may multiple entrances sa room (e.g., from Kitchen, from Hallway):

### Setup:
```
SpawnPoints (folder)
├─ SpawnPoint_Main (default)
│   ├─ Room Name: "Room05_DiningRoom"
│   ├─ Spawn Point ID: "Main"
│   └─ Is Default: ✓ CHECKED
│
├─ SpawnPoint_FromKitchen
│   ├─ Room Name: "Room05_DiningRoom"
│   ├─ Spawn Point ID: "FromKitchen"
│   └─ Is Default: ✗ (unchecked)
│
└─ SpawnPoint_FromHallway
    ├─ Room Name: "Room05_DiningRoom"
    ├─ Spawn Point ID: "FromHallway"
    └─ Is Default: ✗ (unchecked)
```

### Usage:
- **Default spawn** (Main) - used when no specific spawn is set
- **FromKitchen** - used when RoomExit sets `targetSpawnPointID = "FromKitchen"`
- **FromHallway** - used when RoomExit sets `targetSpawnPointID = "FromHallway"`

---

## TESTING

### Quick Test:
1. Move spawn point to new position
2. **Save** the scene (Ctrl+S)
3. **Play** the scene
4. Lisa should spawn at new position

### Full Test (with transition):
1. Move spawn point in Dining Room
2. Save scene
3. Play from Kitchen scene
4. Walk to exit door
5. Transition to Dining Room
6. Lisa should spawn at new position

---

## COMMON ISSUES

### Issue 1: Lisa still spawns sa old position
**CAUSE**: Scene not saved
**FIX**: 
- Press **Ctrl+S** to save scene
- Or: File > Save

### Issue 2: Lisa spawns sa wrong spawn point
**CAUSE**: Multiple spawn points with `Is Default` checked
**FIX**:
- Only ONE spawn point should be default
- Uncheck others

### Issue 3: Lisa doesn't spawn at all
**CAUSE**: Room Name doesn't match scene name
**FIX**:
- Check `Room Name` field in RoomSpawnPoint component
- Must match EXACTLY: "Room05_DiningRoom" (case-sensitive!)

### Issue 4: Spawn point not visible in Scene View
**FIX**:
- Click **Gizmos** button (top right of Scene View)
- Make sure it's enabled
- Or just use Transform position values

---

## PRO TIPS

### Tip 1: Snap to Grid
- Enable **Grid Snapping**: Hold **Ctrl** while dragging
- Makes positioning cleaner

### Tip 2: Copy Position from Lisa
1. Play the scene
2. Move Lisa to desired spawn position
3. Copy her Transform position (right-click > Copy Component)
4. Stop playing
5. Paste to SpawnPoint Transform (right-click > Paste Component Values)

### Tip 3: Use Scene View Camera
- Position Scene View camera where you want spawn
- Select SpawnPoint
- Menu: GameObject > Align With View
- Adjusts position to match camera view

### Tip 4: Visual Marker
- Add a sprite/icon to SpawnPoint GameObject
- Makes it easier to see in Scene View
- Disable the sprite renderer in Inspector (so it doesn't show in game)

---

## ALL ROOMS SPAWN POINTS

### Checklist for All Scenes:

- [ ] **Room01_Foyer**
  - SpawnPoint_Main (entrance)
  
- [ ] **Room02_LivingRoom**
  - SpawnPoint_Main (from foyer)
  
- [ ] **Room03_Hallway**
  - SpawnPoint_Main (from living room)
  - SpawnPoint_FromUpstairs (optional)
  
- [ ] **Room04_Kitchen**
  - SpawnPoint_Main (from hallway)
  
- [ ] **Room05_DiningRoom**
  - SpawnPoint_Main (from kitchen) ← **FIX THIS ONE!**
  
- [ ] **Room06_ReturnToHallway**
  - SpawnPoint_Main (from dining room)
  
- [ ] **Room07_Lisa'sBedroom**
  - SpawnPoint_Main (from hallway)
  
- [ ] **Room08_Lisa'sBathroom**
  - SpawnPoint_Main (from bedroom)
  
- [ ] **Room09_Master'sBathroom**
  - SpawnPoint_Main (from hallway)

---

## SUMMARY

**To Change Spawn Position**:
1. Open scene
2. Find SpawnPoint GameObject
3. Move it to desired position (drag in Scene View)
4. Save scene (Ctrl+S)
5. Test

**No Code Changes Needed!** Just move the GameObject. 🎮

**For Dining Room**:
1. Open Room05_DiningRoom
2. Find SpawnPoint_Main
3. Move to center/entrance (not sa gilid)
4. Save and test

Yan lang! Super simple. 😊
