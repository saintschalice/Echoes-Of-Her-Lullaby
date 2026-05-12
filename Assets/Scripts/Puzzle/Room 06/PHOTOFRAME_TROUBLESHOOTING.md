# Room 06 - Photo Frame Interaction Troubleshooting

## ✅ CODE STATUS
**Scripts are CORRECT and match Room 07 pattern!**
- `Room06_PhotoFrameInteractable.cs` - ✅ Implements IInteractable correctly
- `Room06_HallwayController.cs` - ✅ Complete flow implementation

---

## 🔍 UNITY SETUP CHECKLIST

### 1. Photo Frame GameObject Setup
**GameObject Name**: `PhotoFrame` (or similar)

**Required Components**:
- ✅ `SpriteRenderer` - To show the photo sprite
- ✅ `Collider2D` (BoxCollider2D or CircleCollider2D)
  - **Is Trigger**: ✅ CHECKED (IMPORTANT!)
  - **Size**: 1.5 to 2.0 (adjust to cover sprite area)
- ✅ `Room06_PhotoFrameInteractable` script attached

**Layer**: Should be on a layer that player can interact with (usually `Default` or `Interactable`)

---

### 2. Room06_HallwayController Setup
**GameObject Name**: `Room06_Controller` (or similar)

**Inspector Settings to Assign**:

#### Photo Frame Section:
- **Photo Frame**: Drag the PhotoFrame GameObject here
- **Normal Photo Sprite**: Assign normal family photo sprite
- **Scratched Photo Sprite**: Assign bloody/scratched photo sprite

#### Photo Panel UI Section:
- **Photo Panel**: Drag the UI Panel GameObject (Canvas child)
- **Photo Panel Image**: Drag the Image component inside the panel

#### Emily Configuration:
- **Emily Game Object**: Drag Emily GameObject
- **Emily Spawn Point**: Create empty GameObject named "Emily_Spawn_Point" and drag here
- **Emily Chase Speed**: 4.5 (adjust as needed)
- **Catch Distance**: 1.0

#### Audio:
- **Scratch Sound**: Assign scratch/horror sound effect
- **Emily Spawn Sound**: Assign jumpscare sound
- **Chase Music Loop**: Assign chase music
- **Room Audio Source**: Drag AudioSource component

---

### 3. Player Interaction System Check

**Verify Player Has**:
- `PlayerInteractionHandler` script (or similar)
- Collider2D (trigger detection)
- Correct layer setup

**Check Interaction Button**:
- Mobile: Interaction button should appear when near photo frame
- If button doesn't appear, check `PlayerInteractionHandler` script

---

### 4. Common Issues & Fixes

#### Issue: "Can't interact with photo frame"
**Possible Causes**:
1. ❌ Collider2D on PhotoFrame is NOT set as trigger
   - **Fix**: Select PhotoFrame → Inspector → Collider2D → Check "Is Trigger"

2. ❌ Collider2D size is too small
   - **Fix**: Increase size to 1.5-2.0 to cover sprite area

3. ❌ Room06_HallwayController not in scene
   - **Fix**: Create empty GameObject, add script, assign all references

4. ❌ Photo Frame GameObject not assigned in controller
   - **Fix**: Drag PhotoFrame GameObject to controller's "Photo Frame" field

5. ❌ Player interaction system not detecting interactables
   - **Fix**: Check player has `PlayerInteractionHandler` and correct layer setup

#### Issue: "Interaction button doesn't appear"
**Possible Causes**:
1. ❌ Player's interaction detection range too small
   - **Fix**: Increase detection radius in PlayerInteractionHandler

2. ❌ PhotoFrame on wrong layer
   - **Fix**: Set PhotoFrame to `Default` or `Interactable` layer

3. ❌ IInteractable not being detected
   - **Fix**: Verify `Room06_PhotoFrameInteractable` script is attached

#### Issue: "Panel doesn't show"
**Possible Causes**:
1. ❌ Photo Panel not assigned in controller
   - **Fix**: Assign UI Panel GameObject in Inspector

2. ❌ Photo Panel Image not assigned
   - **Fix**: Assign Image component inside panel

3. ❌ Sprites not assigned
   - **Fix**: Assign both normal and scratched sprites

---

### 5. Testing Steps

1. **Enter Room 06**
   - Intro dialogue should play automatically
   - Player should be able to move after dialogue

2. **Approach Photo Frame**
   - Interaction button should appear (mobile)
   - Debug log: "[PhotoFrame] Player focused on photo frame"

3. **Click Interaction Button**
   - Debug log: "[PhotoFrame] OnInteract called!"
   - Debug log: "[Room06] Photo frame interacted"
   - Panel should open with normal photo

4. **Watch Sequence**
   - Normal photo shows (1.5s)
   - Scratch sound plays
   - Photo transitions to scratched version
   - Panel auto-closes (1.0s)
   - World photo frame changes to bloody sprite
   - Lisa's reaction dialogue
   - Emily spawns and hunts

---

### 6. Debug Mode

**Enable Debug Logs**:
- Select `Room06_HallwayController` in scene
- Check "Debug Mode" in Inspector
- Select `PhotoFrame` GameObject
- Check "Debug Mode" in `Room06_PhotoFrameInteractable`

**Expected Console Logs**:
```
[Room06] Playing intro sequence
[Room06] Intro sequence complete
[PhotoFrame] Player focused on photo frame
[PhotoFrame] OnInteract called!
[Room06] Photo frame interacted
[Room06] Photo panel opened - showing normal photo
[Room06] Photo scratched in panel!
[Room06] Photo panel closed automatically
[Room06] World photo frame changed to bloody version
[Room06] Spawning Emily!
[Room06] Emily hunting! Speed: 4.5
```

---

### 7. Quick Fix Checklist

If photo frame still not working, check in this order:

1. ✅ PhotoFrame has Collider2D with "Is Trigger" checked
2. ✅ PhotoFrame has `Room06_PhotoFrameInteractable` script
3. ✅ Room06_HallwayController exists in scene
4. ✅ PhotoFrame GameObject assigned in controller
5. ✅ All sprites assigned (normal + scratched)
6. ✅ Photo Panel and Image assigned
7. ✅ Emily and spawn point assigned
8. ✅ Debug mode enabled to see logs
9. ✅ Player can interact with other objects (test in Room 07)
10. ✅ Scene is saved after setup

---

## 📝 SCENE HIERARCHY EXAMPLE

```
Room06_ReturnToHallwayUpStairs (Scene)
├── Room06_Controller (Empty GameObject)
│   └── Room06_HallwayController script
├── PhotoFrame (Sprite GameObject)
│   ├── SpriteRenderer (normal photo sprite)
│   ├── BoxCollider2D (Is Trigger ✓, Size: 1.5)
│   └── Room06_PhotoFrameInteractable script
├── Emily_Spawn_Point (Empty GameObject)
│   └── Transform (position where Emily spawns)
├── Emily (GameObject)
│   ├── NavMeshAgent
│   ├── EmilyGhost script
│   └── (Initially disabled)
└── Canvas
    └── PhotoPanel (Panel)
        └── PhotoImage (Image component)
```

---

## 🎯 FINAL VERIFICATION

**Before Testing**:
1. Save scene
2. Enable debug mode on both scripts
3. Check all references assigned
4. Verify collider is trigger
5. Test in Play mode

**If Still Not Working**:
- Check Console for error messages
- Verify player interaction system works in other rooms
- Try increasing collider size to 2.5
- Ensure PhotoFrame is not on "Ignore Raycast" layer

---

**Scripts are correct! Issue is likely in Unity setup/references.**
