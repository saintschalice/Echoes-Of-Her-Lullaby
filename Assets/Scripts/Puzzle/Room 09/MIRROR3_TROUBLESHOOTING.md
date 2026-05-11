# Mirror 3 Troubleshooting Guide

## Problem 1: Hindi Nag-Shuffle ❌

### Possible Causes:

#### 1. Hindi Natatawag yung StartPuzzle()
**Check**: Tingnan yung Console, dapat may message na:
```
[Mirror3] ========== STARTING VANITY TERROR PUZZLE ==========
```

**Kung wala**: Hindi natatawag yung `StartPuzzle()` method.

**Fix**: 
- Check kung paano mo tinatawag yung puzzle
- Dapat may script na tumatawag ng `Mirror3_VanityTerror.Instance.StartPuzzle()`
- O kaya may button na naka-assign sa `StartPuzzle()` method

#### 2. Walang DiaryPage sa Slots
**Check**: Tingnan yung Console, dapat may message na:
```
[Mirror3] Total pages found: 8
```

**Kung less than 8**: May slots na walang DiaryPage child.

**Fix**:
1. Sa Hierarchy, expand yung Mirror3 panel
2. Expand each Slot_1 to Slot_8
3. Make sure EACH slot has ONE DiaryPage as child:
   ```
   Slot_1
   └── DiaryPage_1  ← Must be CHILD of slot!
   Slot_2
   └── DiaryPage_2
   ... (continue for all 8)
   ```

#### 3. Wrong GameObject Names
**Check**: Tingnan yung Console, dapat may message na:
```
[Mirror3]   ✅ Added DiaryPage_1 to shuffle list
```

**Kung wala**: Yung GameObject names ay hindi naglalaman ng "DiaryPage".

**Fix**: Rename your GameObjects to include "DiaryPage":
- ✅ DiaryPage_1
- ✅ DiaryPage_2
- ✅ Page_DiaryPage_3
- ❌ Page1 (won't work)
- ❌ Diary1 (won't work)

#### 4. Slots Not Assigned in Inspector
**Check**: Select yung GameObject na may Mirror3_VanityTerror component.

**Fix**:
1. Sa Inspector, hanapin yung **Diary Slots** array
2. Set size to **8**
3. Drag each slot (Slot_1 to Slot_8) into the array **IN ORDER**:
   - Element 0 = Slot_1
   - Element 1 = Slot_2
   - Element 2 = Slot_3
   - ... (continue to Slot_8)

---

## Problem 2: Napupunta sa Taas yung Item ⬆️

### Cause:
Yung dragged item ay lumalabas sa puzzle panel kasi nag-reparent to Canvas root.

### Fix Applied:
✅ Updated `DraggableItem.cs` - items now stay within puzzle panel

### Additional Fixes Needed:

#### 1. Add Mask to Puzzle Panel
Para hindi lumabas yung items sa panel:

1. Select yung **Mirror3_Panel** (or main puzzle panel)
2. Add Component → **Mask**
3. Check ✅ **Show Mask Graphic** (optional)

#### 2. Check Panel Hierarchy
Make sure structure is:
```
Mirror3_Panel (with Mask component)
├── Slots_Container
│   ├── Slot_1
│   │   └── DiaryPage_1
│   ├── Slot_2
│   │   └── DiaryPage_2
│   ... (continue)
```

#### 3. Disable Layout Groups
If slots are moving when you drag:

1. Select **Slots_Container**
2. If may **Horizontal Layout Group** or **Vertical Layout Group**:
   - Uncheck ✅ **Enabled**
   - OR remove component completely

---

## Testing Checklist

### Before Starting Puzzle:
- [ ] All 8 slots have DiaryPage children
- [ ] All DiaryPages have DraggableItem component
- [ ] All DraggableItem have Puzzle Number = 3
- [ ] All DiaryPages have correct Item ID (matches GameObject name)
- [ ] Diary Slots array has 8 elements assigned in order

### When Starting Puzzle:
- [ ] Console shows: `[Mirror3] ========== STARTING VANITY TERROR PUZZLE ==========`
- [ ] Console shows: `[Mirror3] Total pages found: 8`
- [ ] Console shows: `[Mirror3] ========== SHUFFLE COMPLETE ==========`
- [ ] Pages are in DIFFERENT positions than before

### When Dragging:
- [ ] Item becomes semi-transparent
- [ ] Item follows finger/mouse
- [ ] Item stays WITHIN puzzle panel (doesn't go outside)
- [ ] Item snaps to slot when dropped nearby

### When Dropped:
- [ ] Console shows: `[Mirror3] Diary page DiaryPage_X placed in slot Slot_Y`
- [ ] Item centers in slot
- [ ] Item stays in slot (doesn't move)

---

## Debug Commands

### Check if StartPuzzle() is being called:
Add this to your interaction script:
```csharp
Debug.Log("About to call Mirror3 StartPuzzle");
Mirror3_VanityTerror mirror3 = FindObjectOfType<Mirror3_VanityTerror>();
if (mirror3 != null)
{
    mirror3.StartPuzzle();
}
else
{
    Debug.LogError("Mirror3_VanityTerror not found!");
}
```

### Manual Test in Inspector:
1. Play the game
2. In Hierarchy, find the GameObject with Mirror3_VanityTerror
3. In Inspector, right-click on the script name
4. Click **"StartPuzzle"** to manually trigger

---

## Common Setup Mistakes

### ❌ Wrong: Pages are siblings of slots
```
Slots_Container
├── Slot_1
├── Slot_2
├── DiaryPage_1  ← WRONG! Not a child of slot
├── DiaryPage_2  ← WRONG!
```

### ✅ Correct: Pages are children of slots
```
Slots_Container
├── Slot_1
│   └── DiaryPage_1  ← CORRECT! Child of slot
├── Slot_2
│   └── DiaryPage_2  ← CORRECT!
```

### ❌ Wrong: Multiple pages in one slot
```
Slot_1
├── DiaryPage_1
└── DiaryPage_2  ← WRONG! Only ONE page per slot
```

### ✅ Correct: One page per slot
```
Slot_1
└── DiaryPage_1  ← CORRECT! Only one page
```

---

## Expected Console Output

When puzzle starts correctly, you should see:
```
[Mirror3] ========== STARTING VANITY TERROR PUZZLE ==========
[Mirror3] ✅ Puzzle panel shown
[Mirror3] About to shuffle pages...
[Mirror3] ========== SHUFFLE PAGES START ==========
[Mirror3] Number of slots: 8
[Mirror3] Checking slot 0: Slot_1
[Mirror3]   - Found child: DiaryPage_1
[Mirror3]   ✅ Added DiaryPage_1 to shuffle list
[Mirror3] Checking slot 1: Slot_2
[Mirror3]   - Found child: DiaryPage_2
[Mirror3]   ✅ Added DiaryPage_2 to shuffle list
... (continues for all 8 slots)
[Mirror3] Total pages found: 8
[Mirror3] Shuffling pages...
[Mirror3] Pages shuffled! New order:
[Mirror3] Slot_1 now has: DiaryPage_5
[Mirror3] Slot_2 now has: DiaryPage_1
[Mirror3] Slot_3 now has: DiaryPage_7
... (random order)
[Mirror3] ========== SHUFFLE COMPLETE ==========
```

If you see this, shuffle is working! 🎉

---

## Still Not Working?

### Share These Details:
1. Screenshot of Hierarchy showing slots and pages
2. Screenshot of Inspector for Mirror3_VanityTerror component
3. Console output when starting puzzle
4. Which step in the checklist failed

This will help identify the exact problem! 🔍
