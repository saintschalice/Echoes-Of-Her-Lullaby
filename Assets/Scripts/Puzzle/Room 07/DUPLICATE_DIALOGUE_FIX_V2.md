# Room 07 - Duplicate Dialogue Fix (Mirror Sequence)

## 🐛 PROBLEMA

May **duplicate dialogues** sa mirror interaction sequence. Umuulit ang dialogues dahil dalawang scripts ang nag-trigger ng dialogues:

### Before Fix:

**Room07_Interactable.cs** (TriggerMirrorSequence):
1. ✅ MIRROR_READY dialogues (3 parts)
2. ✅ MIRROR_JUMPSCARE dialogues (2 parts)
3. ✅ MIRROR_CHASE dialogue
4. Calls → `MirrorJumpscareSequence.TriggerJumpscare()`

**MirrorJumpscareSequence.cs** (JumpscareSequence):
1. ❌ "Let me check the mirror..." (DUPLICATE!)
2. Jumpscare visual + sound
3. ❌ "That lullaby..." (DUPLICATE!)
4. ❌ "The door is locked!..." (DUPLICATE!)
5. Start chase

**Result**: Sobrang daming dialogues! Confusing sa player!

---

## ✅ SOLUTION

**Removed duplicate dialogues** from `MirrorJumpscareSequence.cs` since proper dialogues are already handled by `Room07_Interactable.cs`.

### After Fix:

**Room07_Interactable.cs** (TriggerMirrorSequence):
1. ✅ MIRROR_READY dialogues (3 parts)
2. ✅ MIRROR_JUMPSCARE dialogues (2 parts)
3. ✅ MIRROR_CHASE dialogue
4. Calls → `MirrorJumpscareSequence.TriggerJumpscare()`

**MirrorJumpscareSequence.cs** (JumpscareSequence):
1. ✅ Jumpscare visual + sound
2. ✅ Spawn Emily
3. ✅ Camera shake
4. ✅ Play lullaby fragment 3
5. ✅ Lock bedroom door
6. ✅ Start chase (no dialogue)

**Result**: Clean dialogue flow! No duplicates!

---

## 📋 MIRROR SEQUENCE FLOW (FINAL)

### Complete Flow:

1. **Player interacts with mirror** (Room07_Interactable.cs)
   - Check if all puzzles complete
   - If not complete: Show hint about missing step
   - If complete: Continue to step 2

2. **Mirror Ready Dialogues** (Room07_Interactable.cs)
   - MIRROR_READY_1: "This mirror... it's the same one from my nightmares."
   - MIRROR_READY_2: "Every night, I see her reflection behind me..."
   - MIRROR_READY_3: "But she's not there when I turn around."

3. **Mirror Jumpscare Dialogues** (Room07_Interactable.cs)
   - MIRROR_JUMPSCARE_1: "Wait... something's different this time."
   - MIRROR_JUMPSCARE_2: "The reflection... it's moving on its own!"

4. **Chase Dialogue** (Room07_Interactable.cs)
   - MIRROR_CHASE: "She's here! I need to get out!"

5. **Jumpscare Sequence** (MirrorJumpscareSequence.cs)
   - Show jumpscare image
   - Play jumpscare sound
   - Spawn Emily behind Lisa
   - Camera shake
   - Hide jumpscare image

6. **Lullaby** (MirrorJumpscareSequence.cs)
   - Play lullaby fragment 3
   - Wait for lullaby to finish

7. **Lock Door & Start Chase** (MirrorJumpscareSequence.cs)
   - Lock bedroom door (prevent exit)
   - Re-enable player controls
   - Activate Emily's aggressive chase AI
   - Player must run to bathroom!

---

## 🎯 KEY CHANGES

### MirrorJumpscareSequence.cs:

**REMOVED**:
- ❌ "Let me check the mirror..." dialogue
- ❌ "That lullaby..." dialogue
- ❌ "The door is locked!..." dialogue
- ❌ Dialogue waiting loops

**KEPT**:
- ✅ Jumpscare visual effects
- ✅ Audio playback
- ✅ Emily spawn and AI activation
- ✅ Door locking
- ✅ Camera shake

**ADDED**:
- ✅ Wait for lullaby to finish playing
- ✅ Comment explaining dialogues are handled elsewhere

---

## 🔊 AUDIO FLOW

1. **Jumpscare sound** - When Emily appears
2. **Lullaby fragment 3** - After jumpscare
3. **Chase music** - During chase (handled by Emily AI)

---

## 🎮 PLAYER CONTROL FLOW

1. **Disabled** - During mirror dialogues (Room07_Interactable)
2. **Disabled** - During jumpscare sequence
3. **Disabled** - During lullaby playback
4. **ENABLED** - When chase starts (player can run!)

---

## ✅ TESTING CHECKLIST

### Test Mirror Interaction:

1. **Complete all puzzles** in Lisa's Bedroom
2. **Interact with mirror**
3. **Expected Flow**:
   - ✅ MIRROR_READY dialogues (3 parts)
   - ✅ MIRROR_JUMPSCARE dialogues (2 parts)
   - ✅ MIRROR_CHASE dialogue
   - ✅ Jumpscare visual + sound
   - ✅ Lullaby plays (full duration)
   - ✅ Player controls enabled
   - ✅ Emily starts chasing
   - ✅ Bedroom door locked
   - ✅ Can run to bathroom

4. **Check for duplicates**:
   - ❌ NO "Let me check the mirror..." dialogue
   - ❌ NO "That lullaby..." dialogue
   - ❌ NO "The door is locked!..." dialogue
   - ✅ Only proper MIRROR dialogues from Room07_ShortDialogues_FINAL

---

## 💡 NOTES

### Why This Fix Works:

1. **Single Source of Truth**: All dialogues come from `Room07_Interactable.cs`
2. **Clear Separation**: Interactable handles story, Jumpscare handles effects
3. **No Duplicates**: Each dialogue shows only once
4. **Better Flow**: Smooth transition from dialogue → jumpscare → chase

### Design Pattern:

```
Room07_Interactable.cs (Story Layer)
    ↓ Handles all dialogues
    ↓ Manages narrative flow
    ↓ Calls jumpscare when ready
    ↓
MirrorJumpscareSequence.cs (Effects Layer)
    ↓ Handles visual effects
    ↓ Manages audio playback
    ↓ Controls Emily AI
    ↓ Locks doors
```

---

## 🐛 TROUBLESHOOTING

### Issue: "Still seeing duplicate dialogues"

**Possible Causes**:
1. Old version of script still cached
2. Multiple MirrorJumpscareSequence components in scene

**Solution**:
1. Close Unity
2. Delete Library folder
3. Reopen Unity
4. Check scene for duplicate components

### Issue: "No dialogues at all"

**Possible Causes**:
1. Room07_ShortDialogues_FINAL not found
2. DialogueSystemV2 not in scene

**Solution**:
1. Check if Room07_ShortDialogues_FINAL.cs exists
2. Verify DialogueSystemV2 is in scene
3. Check Console for errors

### Issue: "Lullaby doesn't play"

**Possible Causes**:
1. Lullaby clip not assigned
2. Audio source not assigned

**Solution**:
1. Assign lullaby fragment 3 in Inspector
2. Assign music box audio source
3. Check audio source is not muted

---

## 📝 FILES MODIFIED

- `Assets/Scripts/Puzzle/Room 07/MirrorJumpscareSequence.cs`
  - Removed duplicate dialogues
  - Added lullaby wait duration
  - Improved comments

---

**Fix complete! No more duplicate dialogues sa mirror sequence!** 🎉

