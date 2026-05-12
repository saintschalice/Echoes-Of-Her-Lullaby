# Room 07 - Bathroom Door Setup Guide

## 🚪 OVERVIEW

Ang bathroom door sa Lisa's Bedroom ay **LOCKED** until lahat ng puzzles ay tapos na.

**Requirements**:
- ✅ All environmental checks complete
- ✅ All puzzles solved
- ✅ Mirror interaction NOT required (door unlocks before mirror)

---

## 🔧 UNITY SETUP

### Step 1: Create Bathroom Door GameObject

1. **Create Empty GameObject**: `BathroomDoor`
2. **Position**: Sa location ng bathroom door
3. **Add Collider2D**:
   - BoxCollider2D or CircleCollider2D
   - **Is Trigger**: ✅ CHECKED
   - **Size**: 1.5-2.0 (enough to detect player)

### Step 2: Add Script

1. **Add Component**: `Room07_BathroomDoor` script
2. **Configure in Inspector**:

```
Room07_BathroomDoor:
├─ Scene Settings:
│   └─ Bathroom Scene Name: "Room08_Lisa'sBathroom"
│
├─ Spawn Settings:
│   └─ Target Spawn Point ID: "" (or specific spawn point)
│
├─ Transition Settings:
│   ├─ Fade Out Duration: 0.8
│   ├─ Fade In Duration: 0.8
│   └─ Disable Player During Transition: ☑
│
├─ Lock Settings:
│   ├─ Locked Dialogue: "The door is locked. I need to finish what I came here for first."
│   └─ Unlocked Dialogue: "The door... it's open now. The bathroom. Where it all ended."
│
├─ Audio:
│   ├─ Locked Sound: [Assign locked door sound]
│   ├─ Unlock Sound: [Assign unlock sound]
│   └─ Door Open Sound: [Assign door open sound]
│
└─ Debug:
    └─ Debug Mode: ☑
```

---

## 🎯 HOW IT WORKS

### When Door is LOCKED:

**Conditions**: Hindi pa tapos lahat ng puzzles

**Flow**:
1. Player approaches door
2. **Check**: `Room07_FlowController.IsEverythingComplete()`
3. **Result**: FALSE (not complete)
4. **Play**: Locked sound
5. **Show**: Locked dialogue
6. **Player**: Cannot enter bathroom

**Locked Dialogue**:
> "The door is locked. I need to finish what I came here for first."

### When Door is UNLOCKED:

**Conditions**: Lahat ng puzzles tapos na

**Requirements**:
- ✅ Bed checked
- ✅ Wall checked
- ✅ Diary checked
- ✅ Curtains opened
- ✅ Cup obtained
- ✅ Tea party done
- ✅ Chair checked
- ✅ Closet checked
- ✅ Toybox solved
- ✅ Doll obtained
- ✅ Dollhouse done
- ✅ Reading table checked

**Flow (First Time)**:
1. Player approaches door
2. **Check**: `Room07_FlowController.IsEverythingComplete()`
3. **Result**: TRUE (complete!)
4. **Play**: Unlock sound
5. **Show**: Unlock dialogue
6. **Wait**: For dialogue to finish
7. **Play**: Door open sound
8. **Fade**: To black
9. **Load**: Bathroom scene
10. **Fade**: From black

**Unlock Dialogue**:
> "The door... it's open now. The bathroom. Where it all ended."

**Flow (Subsequent Times)**:
1. Player approaches door
2. Door is already unlocked
3. **Skip**: Unlock dialogue
4. **Play**: Door open sound
5. **Fade**: To black
6. **Load**: Bathroom scene
7. **Fade**: From black

---

## 📋 PUZZLE COMPLETION CHECKLIST

Ang door ay mag-unlock kapag **LAHAT** ng ito ay complete:

### Environmental Checks:
- [ ] **Bed** - hasCheckedBed
- [ ] **Wall Drawings** - hasCheckedWall
- [ ] **Diary** - hasCheckedDiary
- [ ] **Chair** - hasCheckedChair
- [ ] **Closet** - hasCheckedCloset
- [ ] **Reading Table** - hasCheckedReadingTable

### Puzzles:
- [ ] **Curtains** - areCurtainsOpened
- [ ] **Cabinet/Cup** - hasEmilyCup (obtained)
- [ ] **Tea Party** - isTeaPartyDone
- [ ] **Toybox** - isToyboxSolved
- [ ] **Doll** - hasEmilyDoll (obtained)
- [ ] **Dollhouse** - isDollhouseDone

**NOTE**: Mirror interaction is NOT required! Door unlocks BEFORE mirror.

---

## 🎨 VISUAL FEEDBACK

### In Scene View (Gizmos):

**When Locked** (puzzles incomplete):
- **Red wire box/circle** around door trigger

**When Unlocked** (puzzles complete):
- **Green wire box/circle** around door trigger

This helps you see at a glance if the door is unlocked!

---

## 🔊 AUDIO SETUP

### Required Audio Clips:

1. **Locked Sound**:
   - Sound when trying locked door
   - Example: Door rattle, lock sound
   - Duration: 0.5-1.0s

2. **Unlock Sound**:
   - Sound when door unlocks
   - Example: Lock clicking open
   - Duration: 1.0-2.0s

3. **Door Open Sound**:
   - Sound when door opens
   - Example: Door creaking open
   - Duration: 1.0-2.0s

---

## ✅ TESTING

### Test Locked Door:

1. **Start game** in Lisa's Bedroom
2. **Approach bathroom door** (without completing puzzles)
3. **Expected**:
   - ✅ Locked sound plays
   - ✅ Locked dialogue shows
   - ✅ Cannot enter bathroom
   - ✅ Red gizmo in Scene view

### Test Unlocked Door:

1. **Complete all puzzles** in Lisa's Bedroom
2. **Approach bathroom door**
3. **Expected (First Time)**:
   - ✅ Unlock sound plays
   - ✅ Unlock dialogue shows
   - ✅ Wait for dialogue to finish
   - ✅ Door open sound plays
   - ✅ Fade to black
   - ✅ Load bathroom scene
   - ✅ Fade from black
   - ✅ Green gizmo in Scene view

4. **Return to bedroom** (if possible)
5. **Approach door again**
6. **Expected (Subsequent Times)**:
   - ✅ No unlock dialogue (already shown)
   - ✅ Door open sound plays
   - ✅ Fade to black
   - ✅ Load bathroom scene
   - ✅ Fade from black

---

## 🐛 TROUBLESHOOTING

### Issue: "Door is always locked"

**Possible Causes**:
1. Not all puzzles complete
2. Room07_FlowController not found
3. IsEverythingComplete() returning false

**Solution**:
1. Enable Debug Mode in script
2. Check Console for puzzle completion status
3. Verify all flags are true in FlowController
4. Check each puzzle completion

### Issue: "Door is always unlocked"

**Possible Causes**:
1. IsEverythingComplete() check bypassed
2. Script not checking properly

**Solution**:
1. Check if script is attached to door
2. Verify IsDoorUnlocked() is being called
3. Check Console for debug logs

### Issue: "Dialogue doesn't show"

**Possible Causes**:
1. DialogueSystemV2 not found
2. Dialogue text empty

**Solution**:
1. Check if DialogueSystemV2 exists in scene
2. Verify dialogue text is assigned
3. Check Console for errors

### Issue: "No sound plays"

**Possible Causes**:
1. Audio clips not assigned
2. AudioManager not found

**Solution**:
1. Assign audio clips in Inspector
2. Check if AudioManager exists
3. Test audio clips directly

### Issue: "Scene doesn't load"

**Possible Causes**:
1. Scene name wrong
2. Scene not in Build Settings

**Solution**:
1. Verify bathroom scene name: "Room08_Lisa'sBathroom"
2. Check Build Settings → Add scene if missing
3. Check Console for scene load errors

---

## 💡 TIPS

### For Better Experience:

1. **Clear Feedback**:
   - Use distinct sounds for locked/unlocked
   - Make dialogue clear about requirements
   - Visual feedback (locked icon?) optional

2. **Smooth Transition**:
   - Fade duration: 0.8s recommended
   - Door open sound before fade
   - Unlock dialogue only first time

3. **Debug Mode**:
   - Enable during development
   - Shows all puzzle completion status
   - Helps identify missing requirements

### For Testing:

1. **Quick Test**:
   - Set all flags to true in FlowController
   - Test door unlock immediately
   - Reset flags after testing

2. **Full Test**:
   - Play through all puzzles
   - Verify door unlocks naturally
   - Test dialogue and sounds

---

## 📝 ALTERNATIVE: Manual Unlock

If you want to unlock the door at a specific point (e.g., after mirror interaction):

**Option 1**: Add public method to unlock:
```csharp
public void UnlockDoor()
{
    hasShownUnlockDialogue = false; // Reset to show dialogue
    // Door will unlock on next approach
}
```

**Option 2**: Check mirror interaction:
```csharp
private bool IsDoorUnlocked()
{
    Room07_FlowController flow = Room07_FlowController.Instance;
    if (flow == null) return false;
    
    // Unlock after mirror interaction
    return flow.hasInteractedWithMirror;
}
```

**Current Implementation**: Door unlocks when all puzzles complete (BEFORE mirror).

---

## 🎯 SCENE HIERARCHY

```
Room07_Lisa'sBedroom (Scene)
├─ BathroomDoor (GameObject)
│   ├─ BoxCollider2D (Is Trigger ✓)
│   └─ Room07_BathroomDoor (Script)
│
├─ Room07_FlowController (GameObject)
│   └─ Room07_FlowController (Script)
│
└─ ... (other room objects)
```

---

**Setup complete! Test the bathroom door!** 🚪✨
