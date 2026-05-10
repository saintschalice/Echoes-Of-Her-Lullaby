# Emily Not Spawning - Troubleshooting Guide

## Problema
**Hindi nag-spawn si Emily sa kitchen.**

## Possible Causes & Solutions

### 1. Emily Prefab Not Assigned ❌
**Check**: KitchenChaseTrigger Inspector
- Select **KitchenChaseTrigger** GameObject in kitchen scene
- Check **"Emily Prefab"** field
- **Should have**: Emily prefab assigned (NOT empty/None)

**Fix**: Drag Emily prefab from Project to "Emily Prefab" field

---

### 2. Emily Spawn Point Not Assigned ❌
**Check**: KitchenChaseTrigger Inspector
- Select **KitchenChaseTrigger** GameObject
- Check **"Emily Spawn Point"** field
- **Should have**: Transform assigned (NOT empty/None)

**Fix**: 
1. Create Empty GameObject → Name it "EmilySpawnPoint"
2. Position it where Emily should appear
3. Drag to "Emily Spawn Point" field

---

### 3. Trigger Already Fired ❌
**Check**: KitchenChaseTrigger Inspector (during Play mode)
- **"Has Triggered"** = TRUE means trigger already fired
- Won't trigger again until scene reloads

**Fix**: 
- Restart scene
- OR check if `emilyIntroDone` is already true in KitchenRoomController

---

### 4. KitchenRoomController Missing ❌
**Check**: Kitchen scene
- Look for GameObject with **KitchenRoomController** component
- Should exist in scene

**Fix**: 
1. Create Empty GameObject → Name it "KitchenRoomController"
2. Add Component → KitchenRoomController
3. Assign all required fields

---

### 5. EmilyRespawnHelper Interfering ❌
**Check**: Emily GameObject
- If **EmilyRespawnHelper** component is attached
- It might be respawning Emily at wrong time

**Fix**: 
- **REMOVE** EmilyRespawnHelper component from Emily
- The trigger script now handles everything

---

### 6. Emily Already in Scene ❌
**Check**: Hierarchy during Play mode
- Search for "Emily" GameObject
- If exists, trigger will use existing Emily

**Fix**: 
- This is actually correct behavior!
- Check Console logs:
  ```
  [KitchenChaseTrigger] Emily already exists in scene. Moving to spawn point...
  ```

---

## Debug Checklist

### Before Playing:
- [ ] KitchenChaseTrigger exists in scene
- [ ] Emily Prefab is assigned
- [ ] Emily Spawn Point is assigned
- [ ] KitchenRoomController exists in scene
- [ ] Trigger collider is set to "Is Trigger" = TRUE
- [ ] Trigger collider overlaps player path

### During Play:
- [ ] Check Console for "[KitchenChaseTrigger] Player entered trigger"
- [ ] Check Console for spawn/existing Emily messages
- [ ] Check if Emily GameObject appears in Hierarchy
- [ ] Check Emily's position (should be at spawn point)

### Expected Console Logs (First Time):
```
[KitchenChaseTrigger] Player entered trigger. Handing off to KitchenRoomController.
[KitchenChaseTrigger] No existing Emily found. Spawning new Emily from prefab.
[KitchenController] Spawning new Emily instance from prefab
```

### Expected Console Logs (With Existing Emily):
```
[KitchenChaseTrigger] Player entered trigger. Handing off to KitchenRoomController.
[KitchenChaseTrigger] Emily already exists in scene. Moving to spawn point...
[KitchenController] Using existing Emily instance
```

---

## Common Errors & Fixes

### Error: "Emily Prefab is NULL!"
**Cause**: Prefab not assigned in Inspector  
**Fix**: Assign Emily prefab to KitchenChaseTrigger

### Error: "Emily Prefab or Spawn Point is missing!"
**Cause**: Missing references  
**Fix**: Assign both prefab and spawn point

### Error: "KitchenRoomController not found!"
**Cause**: Controller doesn't exist in scene  
**Fix**: Add KitchenRoomController to scene

### Error: "Failed to instantiate Emily!"
**Cause**: Prefab is corrupted or invalid  
**Fix**: Check Emily prefab in Project, recreate if needed

---

## Quick Fix Steps

### If Emily Not Spawning:

1. **Check Inspector** (KitchenChaseTrigger):
   - Emily Prefab = Assigned ✅
   - Emily Spawn Point = Assigned ✅

2. **Check Console** (during play):
   - Look for error messages
   - Look for "[KitchenChaseTrigger]" logs

3. **Remove EmilyRespawnHelper**:
   - Select Emily GameObject
   - Remove EmilyRespawnHelper component (if exists)

4. **Test Again**:
   - Play game
   - Enter kitchen
   - Emily should spawn ✅

---

## Alternative: Manual Emily Setup

If trigger still doesn't work, you can place Emily manually:

1. **Drag Emily Prefab** into kitchen scene
2. **Position** at desired spawn location
3. **Disable** Emily GameObject in Inspector
4. **In KitchenChaseTrigger**:
   - Keep trigger logic
   - It will find and enable existing Emily

---

## Files to Check

### Scripts:
- `KitchenChaseTrigger.cs` - Trigger logic
- `KitchenRoomController.cs` - Intro sequence
- `EmilyGhost.cs` - Emily AI

### Scene Objects:
- KitchenChaseTrigger GameObject
- EmilySpawnPoint GameObject
- KitchenRoomController GameObject
- Emily Prefab (in Project)

---

## Summary

**Most Common Issue**: Emily Prefab not assigned in Inspector

**Quick Fix**: 
1. Select KitchenChaseTrigger
2. Assign Emily Prefab
3. Assign Emily Spawn Point
4. Remove EmilyRespawnHelper (if exists)
5. Test

**Tapos na dapat!** 🎉

If still not working, check Console for specific error messages and follow the troubleshooting steps above.
