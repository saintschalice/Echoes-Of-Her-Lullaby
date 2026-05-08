# Mirror Troubleshooting Guide

## 🪞 Why Mirror Appears "Disabled"

The mirror should **always be interactable** - it just gives different responses based on your progress.

---

## ✅ How Mirror Should Work

### If NOT everything complete:
```
Player interacts with Mirror
  ↓
Shows specific hint about next step
Example: "I should check the bed first."
```

### If EVERYTHING complete:
```
Player interacts with Mirror
  ↓
Triggers jumpscare sequence
  ↓
Emily appears, chase begins
```

---

## 🔍 Check These Things

### Check 1: Mirror GameObject Setup

**In Unity Hierarchy:**
```
1. Find the Mirror GameObject
2. Select it
3. Look at Inspector
```

**Inspector should have:**
```
✓ Room07_Interactable component
✓ ObjectType set to: Mirror
✓ UI Manager assigned
✓ Collider2D (for interaction detection)
✓ Tag: Interactable (or your interaction tag)
```

---

### Check 2: ObjectType Setting

**CRITICAL:**
```
Inspector → Room07_Interactable component
  My Type: Mirror  ← MUST be set to "Mirror"
  
If set to something else:
→ Wrong behavior!
→ Change to "Mirror"
```

---

### Check 3: Collider Setup

**Mirror needs a collider:**
```
Inspector → Collider2D component
  ✓ Box Collider 2D or Circle Collider 2D
  ✓ Is Trigger: Checked (if using trigger-based interaction)
  ✓ Size: Large enough to detect player
```

---

### Check 4: Interaction System

**How does player interact?**

**Option A: Mobile Button**
```
- Player taps "Interact" button when near mirror
- Button calls: mirror.Interact()
- Should work if collider detects player
```

**Option B: IInteractable Interface**
```
- Player enters trigger zone
- System calls: OnInteract(PlayerContext)
- Should work if IInteractable is implemented
```

**Option C: Direct Click**
```
- Player clicks/taps mirror directly
- EventSystem detects click
- Calls interaction method
```

---

## 🧪 Testing Steps

### Test 1: Check if Mirror is Interactable

**Do this:**
```
1. Play Mode
2. Walk to mirror
3. Try to interact (tap button or click mirror)
4. Check Console for logs
```

**Expected Console Log:**
```
"[Room07] Interacting with: Mirror"
```

**If NO log:**
→ Mirror not detecting interaction
→ Check collider and interaction system

**If log shows different ObjectType:**
→ Wrong ObjectType assigned
→ Change to "Mirror" in Inspector

---

### Test 2: Check Validation Messages

**Do this:**
```
1. Play Mode
2. Don't complete any steps
3. Interact with Mirror
4. Should show: "I should check the bed first."
```

**If shows this message:**
→ Mirror IS working! ✓
→ Just need to complete sequence

**If no message:**
→ Check DialogueSystemV2 exists
→ Check Console for errors

---

### Test 3: Check Complete Sequence

**Do this:**
```
1. In Inspector, select: Room07_FlowController
2. Manually set ALL booleans to true:
   ✓ isIntroDone
   ✓ hasCheckedBed
   ✓ hasCheckedWall
   ✓ hasCheckedDiary
   ✓ areCurtainsOpened
   ✓ hasEmilyCup
   ✓ isTeaPartyDone
   ✓ hasCheckedChair
   ✓ hasCheckedCloset
   ✓ isToyboxSolved
   ✓ hasEmilyDoll
   ✓ isDollhouseDone
   ✓ hasCheckedReadingTable
3. Play Mode
4. Interact with Mirror
5. Should trigger jumpscare sequence
```

**If triggers jumpscare:**
→ Mirror works perfectly! ✓
→ Just need to complete sequence normally

**If nothing happens:**
→ Check MirrorJumpscareSequence exists
→ Check flow.CheckFinalCondition() method

---

## 🐛 Common Issues

### Issue 1: "Can't interact with mirror at all"

**Symptoms:**
- No response when clicking/tapping
- No console logs
- Nothing happens

**Solutions:**
```
1. Check Mirror has Collider2D
2. Check Collider size is large enough
3. Check player can reach mirror
4. Check interaction button works on other objects
5. Check Mirror has Room07_Interactable component
```

---

### Issue 2: "Mirror shows wrong message"

**Symptoms:**
- Shows message for different object
- Example: Shows curtain dialogue

**Solutions:**
```
1. Select Mirror in Hierarchy
2. Inspector → Room07_Interactable
3. My Type: Change to "Mirror"
4. Save scene
5. Test again
```

---

### Issue 3: "Mirror shows validation but I completed everything"

**Symptoms:**
- Completed all steps
- Mirror still says "I should check..."

**Solutions:**
```
1. Select Room07_FlowController in Hierarchy
2. Inspector → Check ALL booleans
3. Find which one is still false
4. That step wasn't completed properly
5. Redo that step
```

**Check these booleans:**
```
Environmental:
- hasCheckedBed
- hasCheckedWall
- hasCheckedDiary
- hasCheckedChair
- hasCheckedCloset
- hasCheckedReadingTable

Puzzles:
- areCurtainsOpened
- isTeaPartyDone
- isToyboxSolved
- isDollhouseDone

Items:
- hasEmilyCup (should be true even after using it)
- hasEmilyDoll (should be true even after using it)
```

---

### Issue 4: "Mirror triggers too early"

**Symptoms:**
- Didn't complete everything
- But mirror triggers jumpscare anyway

**Solutions:**
```
This shouldn't happen with current code!

If it does:
1. Check Room07_FlowController.IsEverythingComplete()
2. Make sure it checks ALL booleans
3. Check Console for any errors
```

---

## 📊 Mirror Logic Flow

```
Player interacts with Mirror
  ↓
Check: flow.IsEverythingComplete()?
  ↓
NO → GetMissingStepHint()
  ↓
  Show specific hint dialogue
  ↓
  Return (don't trigger jumpscare)

YES → Trigger jumpscare
  ↓
  MirrorJumpscareSequence.TriggerJumpscare()
  ↓
  OR flow.CheckFinalCondition() (fallback)
```

---

## 🎮 Inspector Setup Checklist

**Mirror GameObject must have:**

- [ ] Room07_Interactable component
- [ ] My Type: Mirror
- [ ] UI Manager: Assigned
- [ ] Collider2D component
- [ ] Collider size: Covers mirror area
- [ ] Tag: Interactable (or your tag)
- [ ] Layer: Default or Interactable layer

---

## 💡 Quick Fix

**If mirror seems disabled, try this:**

```
1. Select Mirror in Hierarchy
2. Inspector → Room07_Interactable
3. My Type: Make sure it says "Mirror"
4. UI Manager: Drag Room07UIManager here
5. Add Collider2D if missing
6. Save scene (Ctrl+S)
7. Play Mode
8. Test interaction
```

---

## 🔍 Debug Mode

**Add this to test:**

```csharp
// In Room07_Interactable.cs, add to Mirror case:

case ObjectType.Mirror:
    Debug.Log("[Mirror] Interaction detected!");
    Debug.Log($"[Mirror] Everything complete? {flow.IsEverythingComplete()}");
    
    if (!flow.IsEverythingComplete())
    {
        string hint = GetMissingStepHint(flow);
        Debug.Log($"[Mirror] Missing step hint: {hint}");
        DialogueSystemV2.Instance?.StartDialogue(hint, "Lisa");
        return;
    }
    
    Debug.Log("[Mirror] Triggering jumpscare!");
    // ... rest of code
```

This will show you exactly what's happening.

---

## 📞 Still Not Working?

**If mirror still appears disabled:**

1. **Screenshot the Mirror GameObject Inspector**
2. **Screenshot the Room07_FlowController Inspector** (showing all booleans)
3. **Copy the Console logs** when you try to interact
4. **Describe what happens** when you click/tap the mirror

This will help diagnose the exact issue! 🙂

---

**Remember: Mirror should ALWAYS be interactable - it just gives different responses!** 🪞✨

