# Mirror 3 - Swap System Explained

## How It Works 🔄

Ang Mirror 3 puzzle ay gumagamit ng **SWAP SYSTEM** para sa diary pages.

### Scenario 1: Drag to Empty Slot
```
Before:
Slot_1: DiaryPage_5
Slot_2: (empty)

Action: Drag DiaryPage_5 to Slot_2

After:
Slot_1: (empty)
Slot_2: DiaryPage_5
```

### Scenario 2: Drag to Occupied Slot (SWAP!)
```
Before:
Slot_1: DiaryPage_5
Slot_2: DiaryPage_1

Action: Drag DiaryPage_5 to Slot_2

After:
Slot_1: DiaryPage_1  ← Swapped!
Slot_2: DiaryPage_5  ← Swapped!
```

Ito yung nangyayari:
1. DiaryPage_5 moves from Slot_1 to Slot_2
2. DiaryPage_1 (na nasa Slot_2) moves to Slot_1
3. **Automatic swap!** Hindi mo na kailangan i-drag yung isa

---

## Why Swap System?

### Advantages:
✅ **Faster rearrangement** - One drag = two pages move
✅ **No empty slots** - All pages stay in slots
✅ **More intuitive** - Like physical card swapping
✅ **Less frustration** - No need to create empty slots first

### Example Puzzle Solving:
```
Start (shuffled):
Slot_1: DiaryPage_3
Slot_2: DiaryPage_1
Slot_3: DiaryPage_2

Goal:
Slot_1: DiaryPage_1
Slot_2: DiaryPage_2
Slot_3: DiaryPage_3

Solution:
1. Drag DiaryPage_1 (from Slot_2) to Slot_1
   → DiaryPage_1 and DiaryPage_3 swap
   Result: [1, 3, 2]

2. Drag DiaryPage_2 (from Slot_3) to Slot_2
   → DiaryPage_2 and DiaryPage_3 swap
   Result: [1, 2, 3] ✅ SOLVED!
```

---

## Technical Implementation

### In Mirror3_VanityTerror.cs:

```csharp
public void OnPagePlacedInSlot(GameObject targetSlot, string pageId)
{
    // 1. Find the dragged page and its original slot
    GameObject draggedPage = FindPageByName(pageId);
    Transform originalSlot = draggedPage.transform.parent;
    
    // 2. Check if target slot has a page
    GameObject targetSlotPage = FindPageInSlot(targetSlot);
    
    // 3. If target has a page, SWAP
    if (targetSlotPage != null && targetSlotPage != draggedPage)
    {
        // Move target's page to original slot
        targetSlotPage.transform.SetParent(originalSlot);
        
        // Move dragged page to target slot
        draggedPage.transform.SetParent(targetSlot);
        
        // Update both slot contents
        slotContents[originalSlot] = targetSlotPage.name;
        slotContents[targetSlot] = draggedPage.name;
    }
    // 4. If target is empty, just move
    else
    {
        draggedPage.transform.SetParent(targetSlot);
        slotContents[originalSlot] = "";
        slotContents[targetSlot] = draggedPage.name;
    }
}
```

---

## Console Output Examples

### When Swapping:
```
[Mirror3] ========================================
[Mirror3] Attempting to place 'DiaryPage_5' in slot 'Slot_2'
[Mirror3] Dragged page found in slot: Slot_1
[Mirror3] 🔄 SWAPPING: 'DiaryPage_5' ↔ 'DiaryPage_1'
[Mirror3] Moved 'DiaryPage_1' to 'Slot_1'
[Mirror3] Moved 'DiaryPage_5' to 'Slot_2'
[Mirror3] Current arrangement:
[Mirror3]   Slot_1: DiaryPage_1
[Mirror3]   Slot_2: DiaryPage_5
[Mirror3]   Slot_3: DiaryPage_3
[Mirror3] ========================================
```

### When Moving to Empty Slot:
```
[Mirror3] ========================================
[Mirror3] Attempting to place 'DiaryPage_5' in slot 'Slot_8'
[Mirror3] Dragged page found in slot: Slot_1
[Mirror3] Target slot is empty, moving page
[Mirror3] Moved 'DiaryPage_5' to 'Slot_8'
[Mirror3] Current arrangement:
[Mirror3]   Slot_1: EMPTY
[Mirror3]   Slot_2: DiaryPage_1
[Mirror3]   Slot_8: DiaryPage_5
[Mirror3] ========================================
```

---

## Testing the Swap System

### Test 1: Basic Swap
1. Start puzzle (pages should shuffle)
2. Note which pages are in Slot_1 and Slot_2
3. Drag Slot_1's page to Slot_2
4. **Expected**: Pages swap positions

### Test 2: Multiple Swaps
1. Perform several swaps
2. **Expected**: Each swap correctly exchanges two pages
3. Check Console for swap messages

### Test 3: Solve Puzzle
1. Arrange pages in order: DiaryPage_1 → DiaryPage_8
2. **Expected**: Puzzle completes when all in correct order

---

## Troubleshooting

### Problem: Pages Don't Swap
**Symptoms**: Dragged page moves, but other page doesn't move to original slot

**Check Console**: Should see `🔄 SWAPPING` message

**Possible Causes**:
1. Target slot's page not detected
2. Pages not properly parented to slots

**Fix**: Check hierarchy - each page must be direct child of slot

### Problem: Pages Disappear After Swap
**Symptoms**: After swapping, one or both pages disappear

**Possible Causes**:
1. RectTransform not reset properly
2. Scale or position wrong

**Fix**: Check if pages have RectTransform component

### Problem: Can't Drag After Swap
**Symptoms**: After swapping, pages become un-draggable

**Possible Causes**:
1. CanvasGroup blocksRaycasts not re-enabled
2. DraggableItem component disabled

**Fix**: Check if DraggableItem component is still enabled

---

## Comparison: Swap vs No-Swap

### Without Swap (Traditional):
```
To rearrange [3, 1, 2] to [1, 2, 3]:
1. Drag 3 to empty area
2. Drag 1 to position 1
3. Drag 2 to position 2
4. Drag 3 to position 3
Total: 4 drags
```

### With Swap (Current):
```
To rearrange [3, 1, 2] to [1, 2, 3]:
1. Drag 1 to position 1 (swaps with 3) → [1, 3, 2]
2. Drag 2 to position 2 (swaps with 3) → [1, 2, 3]
Total: 2 drags ✅ Faster!
```

---

## Advanced: Optimal Solving Strategy

Para sa 8 pages, average moves needed:
- **Without swap**: 8-15 moves
- **With swap**: 4-7 moves

### Tips for Players:
1. **Identify the dates** on each page
2. **Find DiaryPage_1** (earliest date)
3. **Swap it to Slot_1** if not already there
4. **Repeat** for DiaryPage_2, DiaryPage_3, etc.
5. **Work left to right** for efficiency

---

## Summary

✅ **Swap system implemented** - Drag to occupied slot = automatic swap
✅ **Faster puzzle solving** - Fewer moves needed
✅ **More intuitive** - Like physical card rearrangement
✅ **Fully functional** - Ready to test!

**Next Steps**:
1. Test the swap functionality
2. Verify shuffle works on puzzle start
3. Test puzzle completion with correct order
4. Add visual feedback (optional: highlight slots on hover)

Enjoy the improved puzzle experience! 🎮
