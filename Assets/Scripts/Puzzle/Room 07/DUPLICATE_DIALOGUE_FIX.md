# Room 07 - Duplicate Dialogue Fix

## ❌ PROBLEMA

May mga dialogues na nag-uulit (duplicate) sa Room 07:

1. **Tea Party** - Dialogue after cutscene
2. **Doll Pickup** - Possible duplicate sa cutscene
3. **Toybox** - Unused TOYBOX_LETTERS and TOYBOX_DOLL dialogues

---

## 🔧 SOLUSYON

### Fix 1: Remove Duplicate Tea Party Dialogue

**Problem**: After tea party cutscene, may dialogue sequence pa (TEA_PARTY_MEMORY_1, 2, 3) na baka nag-uulit.

**Solution**: Keep only ONE dialogue after cutscene - the completion message.

**File**: `Room07UIManager.cs`

**BEFORE** (May duplicate):
```csharp
System.Collections.IEnumerator TeaPartyCutsceneSequence()
{
    // Cutscene with fade
    yield return StartCoroutine(Room07_CutsceneController.Instance.PlayTeaPartyCutscene());
    
    yield return new WaitForSeconds(0.5f);
    
    // DUPLICATE: Memory dialogue sequence
    yield return StartCoroutine(ShowDialogueSequence(
        Room07_ShortDialogues_FINAL.TEA_PARTY_MEMORY_1,
        Room07_ShortDialogues_FINAL.TEA_PARTY_MEMORY_2,
        Room07_ShortDialogues_FINAL.TEA_PARTY_MEMORY_3
    ));
    
    yield return new WaitForSeconds(0.3f);
    
    // Completion message
    DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.TEA_PARTY_COMPLETE, "Lisa");
}
```

**AFTER** (No duplicate):
```csharp
System.Collections.IEnumerator TeaPartyCutsceneSequence()
{
    // Cutscene with fade (includes lullaby)
    yield return StartCoroutine(Room07_CutsceneController.Instance.PlayTeaPartyCutscene());
    
    yield return new WaitForSeconds(0.5f);
    
    // Only show completion message
    DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.TEA_PARTY_COMPLETE, "Lisa");
}
```

---

### Fix 2: Simplify Doll Pickup Sequence

**Problem**: Doll pickup calls `uiManager.PlayCutscene()` which might have duplicate dialogue.

**Solution**: Use the new cutscene controller directly.

**File**: `Room07_Interactable.cs`

**BEFORE**:
```csharp
System.Collections.IEnumerator PickupDollSequence()
{
    // Add item with notification
    InventoryManager.Instance?.AddItemWithNotification("emily_doll");

    // Wait for notification
    while (ItemNotificationUI.Instance != null && ItemNotificationUI.Instance.IsShowing())
    {
        yield return null;
    }

    yield return new WaitForSeconds(0.3f);

    // Play cutscene (might have duplicate dialogue)
    if (uiManager != null)
    {
        uiManager.PlayCutscene();
    }
}
```

**AFTER**:
```csharp
System.Collections.IEnumerator PickupDollSequence()
{
    // Add item with notification
    InventoryManager.Instance?.AddItemWithNotification("emily_doll");

    // Wait for notification
    while (ItemNotificationUI.Instance != null && ItemNotificationUI.Instance.IsShowing())
    {
        yield return null;
    }

    yield return new WaitForSeconds(0.3f);

    // Play doll cutscene with new controller (no duplicate dialogue)
    if (Room07_CutsceneController.Instance != null)
    {
        yield return StartCoroutine(Room07_CutsceneController.Instance.PlayDollCutscene());
    }
    
    // Optional: Show completion message
    DialogueSystemV2.Instance?.StartDialogue("I'll keep this safe. Emily would want me to.", "Lisa");
}
```

---

### Fix 3: Remove Unused Toybox Dialogues

**Problem**: TOYBOX_LETTERS and TOYBOX_DOLL dialogues exist but are never used, causing confusion.

**Solution**: Either use them or remove them.

**Option A: Use them** (Add to toybox interaction):
```csharp
case ObjectType.Toybox:
    if (!flow.hasCheckedCloset)
    {
        DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.TOYBOX_PREREQUISITE, "Lisa");
        return;
    }
    if (!flow.isToyboxSolved)
    {
        // Show letters dialogue before puzzle
        yield return StartCoroutine(ShowDialogueSequence(
            Room07_ShortDialogues_FINAL.TOYBOX_LETTERS_1,
            Room07_ShortDialogues_FINAL.TOYBOX_LETTERS_2,
            Room07_ShortDialogues_FINAL.TOYBOX_LETTERS_3
        ));
        
        DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.TOYBOX_LOCKED, "Lisa");
        uiManager.ShowToyboxPanel();
    }
    else if (!flow.hasEmilyDoll)
    {
        // Show doll dialogue when picking up
        yield return StartCoroutine(ShowDialogueSequence(
            Room07_ShortDialogues_FINAL.TOYBOX_DOLL_1,
            Room07_ShortDialogues_FINAL.TOYBOX_DOLL_2,
            Room07_ShortDialogues_FINAL.TOYBOX_DOLL_3
        ));
        
        flow.hasEmilyDoll = true;
        StartCoroutine(PickupDollSequence());
    }
    else
    {
        DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.TOYBOX_EMPTY, "Lisa");
    }
    break;
```

**Option B: Remove them** (If not needed):
- Delete TOYBOX_LETTERS_1, 2, 3 from dialogue file
- Delete TOYBOX_DOLL_1, 2, 3 from dialogue file
- Keep only TOYBOX_LOCKED, TOYBOX_SOLVED, TOYBOX_EMPTY

---

## 📋 COMPLETE FIX IMPLEMENTATION

### Update Room07UIManager.cs:

```csharp
System.Collections.IEnumerator TeaPartyCutsceneSequence()
{
    // Use new cutscene controller with fade transitions
    if (Room07_CutsceneController.Instance != null)
    {
        yield return StartCoroutine(Room07_CutsceneController.Instance.PlayTeaPartyCutscene());
    }
    else
    {
        // Fallback
        Debug.LogWarning("[Room07] Cutscene controller not found");
        if (blackScreenCutscene != null)
        {
            blackScreenCutscene.SetActive(true);
            yield return new WaitForSeconds(3f);
            blackScreenCutscene.SetActive(false);
        }
    }
    
    yield return new WaitForSeconds(0.5f);
    
    // Only show completion message (NO DUPLICATE MEMORY DIALOGUE)
    DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.TEA_PARTY_COMPLETE, "Lisa");
}
```

### Update Room07_Interactable.cs:

```csharp
System.Collections.IEnumerator PickupDollSequence()
{
    // Add item with notification
    InventoryManager.Instance?.AddItemWithNotification("emily_doll");

    // Wait for notification
    while (ItemNotificationUI.Instance != null && ItemNotificationUI.Instance.IsShowing())
    {
        yield return null;
    }

    yield return new WaitForSeconds(0.3f);

    // Play doll cutscene with new controller
    if (Room07_CutsceneController.Instance != null)
    {
        yield return StartCoroutine(Room07_CutsceneController.Instance.PlayDollCutscene());
    }
    else
    {
        // Fallback to old system
        if (uiManager != null)
        {
            uiManager.PlayCutscene();
        }
    }
}
```

---

## ✅ RESULT

After fixes:
- ✅ Tea party: Cutscene → Lullaby → Completion message (NO duplicate memory dialogue)
- ✅ Doll pickup: Notification → Cutscene → Lullaby (NO duplicate dialogue)
- ✅ Toybox: Clear flow, no unused dialogues

---

## 🎯 TESTING

### Tea Party Flow:
1. Complete tea party puzzle
2. **Fade to black**
3. **Show cutscene** (3s)
4. **Fade from black**
5. **Fade to black** (for lullaby)
6. **Play lullaby**
7. **Fade from black**
8. **Show ONLY completion message**: "The tea party is complete..."
9. ✅ **NO duplicate memory dialogue**

### Doll Pickup Flow:
1. Solve toybox puzzle
2. Interact with toybox again
3. **Show notification**: "Emily's Doll"
4. **Fade to black**
5. **Show cutscene** (2s)
6. **Fade from black**
7. **Fade to black** (for lullaby)
8. **Play lullaby**
9. **Fade from black**
10. ✅ **NO duplicate dialogue**

---

## 💡 RECOMMENDATION

**Best approach**: Remove duplicate dialogues entirely.

**Why?**:
- Cutscenes already tell the story visually
- Lullaby provides emotional impact
- Too much dialogue after cutscene is redundant
- Players want to continue gameplay, not read more text

**Keep only**:
- Brief completion messages
- Essential story beats
- Clear progression indicators

---

**Implement these fixes to remove duplicate dialogues!** 🎮✨
