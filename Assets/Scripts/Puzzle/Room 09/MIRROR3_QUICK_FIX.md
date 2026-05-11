# Mirror 3 Quick Fix Guide - Hindi Nag-Shuffle at Hindi Nalalagay

## Problem 1: Hindi Nag-Shuffle ❌

### Cause:
Hindi natatawag yung `StartPuzzle()` method.

### Quick Test:
1. **Play the game**
2. **In Hierarchy**, hanapin yung GameObject na may **Mirror3_VanityTerror** component
3. **In Inspector**, right-click sa script name → Select **"Test Shuffle"**
4. Tingnan kung nag-shuffle yung pages

Kung nag-shuffle, ibig sabihin yung problema ay sa paano mo tinatawag yung puzzle.

### Solution A: Add Test Button

1. **Create a UI Button** sa scene:
   - Right-click sa Canvas → UI → Button
   - Name it "TestMirror3Button"

2. **Add the test script**:
   - Select the button
   - Add Component → **Mirror3_TestButton**

3. **Test**:
   - Play the game
   - Click the button
   - Check Console for messages

### Solution B: Call from Your Interaction Script

Kung may script ka na para sa mirror interaction, add this:

```csharp
// When player interacts with Mirror 3
public void OnMirror3Interact()
{
    Debug.Log("Player interacted with Mirror 3");
    
    Mirror3_VanityTerror mirror3 = FindObjectOfType<Mirror3_VanityTerror>();
    
    if (mirror3 != null)
    {
        mirror3.StartPuzzle();
    }
    else
    {
        Debug.LogError("Mirror3_VanityTerror not found!");
    }
}
```

---

## Problem 2: Hindi Nalalagay sa Slot ❌

### Possible Causes:

#### 1. Slots Not Detected
**Check Console**: Dapat may message na:
```
[DraggableItem] Found valid slot: Slot_1 (distance: 45.2)
```

**Kung wala**: Slots hindi na-detect.

**Fix**:
- Increase **Detection Radius** sa DraggableItem component
- Try 200 or 300 (mas malaki = mas madaling mag-snap)

#### 2. Slots Don't Have Proper Names
**Check**: Yung slots dapat may "Slot" sa name:
- ✅ Slot_1, Slot_2, Slot_3, etc.
- ✅ DiarySlot_1, DiarySlot_2, etc.
- ❌ Frame1, Box1, etc. (won't work)

**Fix**: Rename your slots to include "Slot"

#### 3. Slots Are Inside Container with Image
**Check**: Yung parent container ng slots, may Image component ba?

**Fix**:
1. Select the container (parent of all slots)
2. If may **Image** component:
   - Uncheck ✅ **Raycast Target**
   - This allows clicks to pass through

#### 4. Pages Can't Move Between Slots
**Check Console**: Dapat may message na:
```
[Mirror3] Diary page 'DiaryPage_1' placed in slot 'Slot_2'
```

**Kung wala**: Hindi natatawag yung `OnPagePlacedInSlot()`.

**Fix**: Check if DraggableItem has correct **Puzzle Number = 3**

---

## Complete Setup Checklist

### Hierarchy Structure:
```
Mirror3_Panel
├── Timer_Text
├── Slots_Container (NO Layout Group!)
│   ├── Slot_1
│   │   └── DiaryPage_1 (with DraggableItem)
│   ├── Slot_2
│   │   └── DiaryPage_2 (with DraggableItem)
│   ├── Slot_3
│   │   └── DiaryPage_3 (with DraggableItem)
│   ├── Slot_4
│   │   └── DiaryPage_4 (with DraggableItem)
│   ├── Slot_5
│   │   └── DiaryPage_5 (with DraggableItem)
│   ├── Slot_6
│   │   └── DiaryPage_6 (with DraggableItem)
│   ├── Slot_7
│   │   └── DiaryPage_7 (with DraggableItem)
│   └── Slot_8
│       └── DiaryPage_8 (with DraggableItem)
└── TestButton (optional, for testing)
```

### Each DiaryPage Must Have:
- ✅ **Image** component (to show the page)
- ✅ **DraggableItem** component with:
  - Item Id = GameObject name (e.g., "DiaryPage_1")
  - Puzzle Number = **3**
  - Detection Radius = **150** (or higher if not snapping)
  - Return To Original Position = ✅ checked
  - Fade While Dragging = ✅ checked

### Each Slot Must Have:
- ✅ Name contains "Slot" (e.g., Slot_1, Slot_2)
- ✅ **RectTransform** component
- ✅ Optional: **Image** component for visual (uncheck Raycast Target)

### Mirror3_VanityTerror Component Must Have:
- ✅ **Puzzle Panel** assigned
- ✅ **Timer Text** assigned
- ✅ **Diary Slots** array size = 8
- ✅ All 8 slots assigned **IN ORDER**:
  - Element 0 = Slot_1
  - Element 1 = Slot_2
  - Element 2 = Slot_3
  - ... (continue to Slot_8)

---

## Testing Steps

### Step 1: Test Shuffle
1. Play the game
2. Find Mirror3_VanityTerror in Hierarchy
3. Right-click script → "Test Shuffle"
4. **Expected**: Pages move to different slots randomly

### Step 2: Test Drag
1. Try to drag a page
2. **Expected**: Page becomes semi-transparent and follows mouse/finger

### Step 3: Test Drop
1. Drag a page over a slot
2. **Expected**: 
   - Console shows: `[DraggableItem] Found valid slot: Slot_X`
   - Page snaps to center of slot
   - Console shows: `[Mirror3] Diary page 'DiaryPage_X' placed in slot 'Slot_Y'`

### Step 4: Test Rearrange
1. Drag pages between different slots
2. **Expected**: Pages swap positions or move to new slots

### Step 5: Test Complete
1. Arrange pages in correct order (DiaryPage_1 to DiaryPage_8)
2. **Expected**: 
   - Console shows: `[Mirror3] ✅ PUZZLE SOLVED!`
   - Success dialogue appears
   - Puzzle closes

---

## Console Output Examples

### When Shuffle Works:
```
[Mirror3] ========== STARTING VANITY TERROR PUZZLE ==========
[Mirror3] ✅ Puzzle panel shown
[Mirror3] About to shuffle pages...
[Mirror3] ========== SHUFFLE PAGES START ==========
[Mirror3] Number of slots: 8
[Mirror3] Total pages found: 8
[Mirror3] Slot_1 now has: DiaryPage_5
[Mirror3] Slot_2 now has: DiaryPage_1
[Mirror3] ========== SHUFFLE COMPLETE ==========
```

### When Drag Works:
```
[DraggableItem] Started dragging: DiaryPage_1
[DraggableItem] Found valid slot: Slot_3 (distance: 45.2)
[Mirror3] ========================================
[Mirror3] Diary page 'DiaryPage_1' placed in slot 'Slot_3'
[Mirror3] This is Slot index 2 (Slot_3)
[Mirror3] Current arrangement:
[Mirror3]   Slot_1: DiaryPage_5
[Mirror3]   Slot_2: EMPTY
[Mirror3]   Slot_3: DiaryPage_1
[Mirror3] ========================================
```

---

## Common Mistakes

### ❌ Mistake 1: Pages are NOT children of slots
```
Slots_Container
├── Slot_1
├── DiaryPage_1  ← WRONG! Should be child of Slot_1
```

### ✅ Correct:
```
Slots_Container
├── Slot_1
│   └── DiaryPage_1  ← CORRECT!
```

### ❌ Mistake 2: Wrong Puzzle Number
```
DraggableItem component:
- Puzzle Number: 1  ← WRONG! Should be 3 for Mirror 3
```

### ✅ Correct:
```
DraggableItem component:
- Puzzle Number: 3  ← CORRECT!
```

### ❌ Mistake 3: Slots Not Assigned in Order
```
Diary Slots array:
- Element 0: Slot_3  ← WRONG! Should be Slot_1
- Element 1: Slot_1  ← WRONG! Should be Slot_2
```

### ✅ Correct:
```
Diary Slots array:
- Element 0: Slot_1  ← CORRECT!
- Element 1: Slot_2  ← CORRECT!
- Element 2: Slot_3  ← CORRECT!
```

---

## Still Not Working?

### Share These:
1. **Screenshot** of Hierarchy showing Mirror3_Panel expanded
2. **Screenshot** of Inspector for Mirror3_VanityTerror component
3. **Screenshot** of Inspector for one DiaryPage (showing DraggableItem)
4. **Console output** when you:
   - Start the puzzle
   - Try to drag a page
   - Drop the page

This will help identify the exact problem! 🔍
