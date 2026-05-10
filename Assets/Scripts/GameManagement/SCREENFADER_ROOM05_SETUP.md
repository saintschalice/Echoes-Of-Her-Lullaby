# ScreenFader Setup for Room 5 - Visual Guide

## WHY IS THIS NEEDED?

The console error:
```
[RoomExit] ScreenFader not found! Transitioning without fade.
```

This means Room 5 scene is missing the ScreenFader GameObject, which causes:
- ❌ No fade transitions between scenes
- ❌ Potential player control issues
- ❌ Console errors

---

## STEP-BY-STEP SETUP

### STEP 1: Create ScreenFader GameObject

1. **Open Scene**: `Assets/Scenes/Room05_DiningRoom.unity`

2. **Create GameObject**:
   ```
   Hierarchy → Right-click → Create Empty
   Name: ScreenFader
   ```

3. **Position**:
   ```
   Transform:
   - Position: (0, 0, 0)
   - Rotation: (0, 0, 0)
   - Scale: (1, 1, 1)
   ```

---

### STEP 2: Add ScreenFader Component

1. **Select ScreenFader GameObject**

2. **Add Component**:
   ```
   Inspector → Add Component → ScreenFader
   ```

3. **Configure ScreenFader**:
   ```
   ScreenFader Component:
   - Fade Image: [Will assign in Step 3]
   - Default Fade Duration: 1
   - Fade Color: Black (R:0, G:0, B:0, A:255)
   - Fade In On Start: ✓ (checked)
   - Start Delay: 0.2
   ```

---

### STEP 3: Create Fade UI

1. **Create Canvas**:
   ```
   ScreenFader → Right-click → UI → Canvas
   Name: FadeCanvas
   ```

2. **Configure Canvas**:
   ```
   Canvas Component:
   - Render Mode: Screen Space - Overlay
   - Pixel Perfect: ✗ (unchecked)
   - Sort Order: 1000 (IMPORTANT: Must be highest!)
   
   Canvas Scaler:
   - UI Scale Mode: Scale With Screen Size
   - Reference Resolution: 1920 x 1080
   - Match: 0.5
   ```

3. **Create Image**:
   ```
   FadeCanvas → Right-click → UI → Image
   Name: FadeImage
   ```

4. **Configure FadeImage**:
   ```
   Rect Transform:
   - Anchors: Stretch (full screen)
     - Min: (0, 0)
     - Max: (1, 1)
   - Left: 0
   - Top: 0
   - Right: 0
   - Bottom: 0
   
   Image Component:
   - Source Image: None (leave empty)
   - Color: Black (R:0, G:0, B:0, A:255)
   - Raycast Target: ✓ (checked)
   - Material: None
   ```

---

### STEP 4: Link FadeImage to ScreenFader

1. **Select ScreenFader GameObject**

2. **Drag FadeImage**:
   ```
   In Inspector:
   - Find "Fade Image" field in ScreenFader component
   - Drag "FadeImage" GameObject from Hierarchy
   - Drop it into the "Fade Image" field
   ```

3. **Verify**:
   ```
   ScreenFader Component should now show:
   - Fade Image: FadeImage (Image)
   ```

---

### STEP 5: Add PersistentObject Component

1. **Select ScreenFader GameObject**

2. **Add Component**:
   ```
   Inspector → Add Component → PersistentObject
   ```

3. **Configure**:
   ```
   PersistentObject Component:
   - Persist: ✓ (checked)
   ```

**Why?** This makes ScreenFader persist across scene transitions (DontDestroyOnLoad).

---

### STEP 6: Final Hierarchy Structure

Your hierarchy should look like this:

```
Room05_DiningRoom
├── ScreenFader
│   └── FadeCanvas (Canvas)
│       └── FadeImage (Image)
├── Player
├── Joystick
├── Room05_DiningRoomController
└── ... (other objects)
```

---

## VERIFICATION

### In Editor (Before Playing)

1. **Select ScreenFader**:
   - ScreenFader component: ✓
   - PersistentObject component: ✓
   - Fade Image field: Assigned to FadeImage

2. **Select FadeImage**:
   - Color: Black (R:0, G:0, B:0, A:255)
   - Anchors: Stretch
   - Raycast Target: ✓

3. **Select FadeCanvas**:
   - Sort Order: 1000

### In Play Mode

1. **Enter Play Mode**
2. **Check Console**: Should see:
   ```
   [PersistentObject] ScreenFader (ID: xxxxx) marked as persistent.
   ```
3. **No errors** about ScreenFader not found

---

## COMMON MISTAKES

### ❌ WRONG: Canvas Sort Order Too Low
```
Canvas → Sort Order: 0
```
**Problem**: Fade won't cover everything
**Fix**: Set Sort Order to 1000

### ❌ WRONG: FadeImage Not Stretched
```
Rect Transform → Anchors: Center
```
**Problem**: Fade won't cover full screen
**Fix**: Set Anchors to Stretch (full screen)

### ❌ WRONG: Forgot to Assign Fade Image
```
ScreenFader → Fade Image: None
```
**Problem**: Fade won't work
**Fix**: Drag FadeImage to Fade Image field

### ❌ WRONG: Forgot PersistentObject
```
ScreenFader has no PersistentObject component
```
**Problem**: ScreenFader destroyed on scene change
**Fix**: Add PersistentObject component

### ❌ WRONG: FadeImage Color Not Black
```
Image → Color: White or other color
```
**Problem**: Fade looks wrong
**Fix**: Set Color to Black (R:0, G:0, B:0, A:255)

---

## TESTING

### Test 1: Fade In on Scene Load
1. Play the game
2. Enter Room 5
3. **Expected**: Screen fades from black to clear
4. **Duration**: ~1 second

### Test 2: Fade Out on Scene Exit
1. In Room 5, go to exit
2. Trigger scene transition
3. **Expected**: Screen fades from clear to black
4. **Duration**: ~1 second

### Test 3: No Console Errors
1. Play the game
2. Enter and exit Room 5 multiple times
3. **Expected**: No ScreenFader errors in console

---

## TROUBLESHOOTING

### Issue: "ScreenFader not found" still appears
**Check**:
- ScreenFader GameObject exists in scene
- ScreenFader component is attached
- PersistentObject component is attached
- Scene is saved

### Issue: Fade doesn't cover full screen
**Check**:
- FadeImage anchors are set to Stretch
- FadeImage Left/Top/Right/Bottom are all 0
- Canvas is Screen Space - Overlay

### Issue: Fade is wrong color
**Check**:
- FadeImage color is Black (R:0, G:0, B:0, A:255)
- ScreenFader fadeColor is Black

### Issue: Fade blocks UI after fading in
**Check**:
- Raycast Target is checked (should be)
- ScreenFader script handles raycast blocking automatically

### Issue: ScreenFader destroyed on scene change
**Check**:
- PersistentObject component is attached
- Persist is checked
- No duplicate ScreenFaders in other scenes

---

## COPY TO OTHER ROOMS

Once ScreenFader works in Room 5, you can copy it to other rooms:

### Option 1: Create Prefab (Recommended)
1. Drag ScreenFader from Hierarchy to Project
2. Save as Prefab: `ScreenFader.prefab`
3. Drag prefab into other scenes

### Option 2: Copy-Paste
1. Select ScreenFader in Room 5
2. Ctrl+C (copy)
3. Open another scene
4. Ctrl+V (paste)

**Note**: Only ONE ScreenFader should exist at runtime (PersistentObject handles this).

---

## ALTERNATIVE: Use Existing ScreenFader

If ScreenFader already exists in another scene:

1. **Don't create a new one** in Room 5
2. **ScreenFader persists** across scenes (DontDestroyOnLoad)
3. **Just verify** it exists in at least one scene

**To check**:
```
Play the game → Check Hierarchy → Look for ScreenFader in DontDestroyOnLoad
```

---

## RELATED FILES

- `ScreenFader.cs` - Main fade logic
- `PersistentObject.cs` - DontDestroyOnLoad handler
- `RoomExit.cs` - Uses ScreenFader for transitions
- `SceneTransitionManager.cs` - Scene loading with fades

---

## SUMMARY

✅ **Required Components**:
1. ScreenFader GameObject
2. ScreenFader component
3. PersistentObject component
4. FadeCanvas (Canvas)
5. FadeImage (Image)

✅ **Critical Settings**:
- Canvas Sort Order: 1000
- FadeImage Anchors: Stretch
- FadeImage Color: Black
- Persist: Checked

✅ **Expected Result**:
- No console errors
- Smooth fade transitions
- Player controls work correctly

---

## QUICK CHECKLIST

Before closing Unity:

- [ ] ScreenFader GameObject created
- [ ] ScreenFader component added
- [ ] PersistentObject component added
- [ ] FadeCanvas created
- [ ] FadeImage created and configured
- [ ] FadeImage assigned to ScreenFader
- [ ] Canvas Sort Order = 1000
- [ ] FadeImage color = Black
- [ ] Scene saved
- [ ] Tested in Play Mode
- [ ] No console errors

**Done!** ✅
