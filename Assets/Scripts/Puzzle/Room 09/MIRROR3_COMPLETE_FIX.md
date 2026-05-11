# Mirror 3 - Complete Fix Guide

## Tatlong Problema:
1. ❌ **Lumabas ng slot** - Page napupunta sa Canvas root
2. ❌ **Walang switch** - Swap hindi gumagana
3. ❌ **Hindi nag-shuffle** - StartPuzzle() hindi natatawag

---

## FIX #1: Lumabas ng Slot (Pages Going to Canvas Root)

### Solution: Enable "Stay In Panel" Mode

Para sa lahat ng DiaryPage items:

1. **Select each DiaryPage** (DiaryPage_1 to DiaryPage_8)
2. **In Inspector**, find **DraggableItem** component
3. **Check ✅ "Stay In Panel"** checkbox
4. **Repeat for all 8 pages**

### What This Does:
- Pages stay within puzzle panel when dragging
- Pages don't move to Canvas root
- Easier to control and see

### Alternative: Manual Fix in Unity
If checkbox doesn't appear, update the script first then:
1. Play the game
2. Check Console - should see: `[DraggableItem] DiaryPage_1 staying in panel (stayInPanel=true)`

---

## FIX #2: Walang Switch (Swap Not Working)

### Possible Causes:

#### A. OnPagePlacedInSlot Not Being Called
**Check Console**: Should see:
```
[Mirror3] OnPagePlacedInSlot called: pageId='DiaryPage_1', targetSlot='Slot_2'
```

**If NOT appearing**:
- DraggableItem's Puzzle Number is wrong
- Should be **3** for Mirror 3

**Fix**:
1. Select each DiaryPage
2. Set **Puzzle Number = 3**

#### B. Pages Not Found in Slots
**Check Console**: Should see:
```
[Mirror3] Found dragged page 'DiaryPage_1' in slot 'Slot_1'
```

**If seeing "not found"**:
- Pages are not children of slots
- Hierarchy is wrong

**Fix**: Make sure hierarchy is:
```
Slot_1
└── DiaryPage_1  ← Must be CHILD!
```

#### C. Target Slot Page Not Detected
**Check Console**: Should see:
```
[Mirror3] Target slot 'Slot_2' already has page: 'DiaryPage_5'
[Mirror3] 🔄 SWAPPING: 'DiaryPage_1' ↔ 'DiaryPage_5'
```

**If NOT seeing swap message**:
- Target slot's page not detected
- Page names don't contain "DiaryPage"

**Fix**: Rename pages to include "DiaryPage" in name

---

## FIX #3: Hindi Nag-Shuffle (Shuffle Not Working)

### Cause: StartPuzzle() Not Being Called

### Solution A: Add Test Button

1. **Create UI Button**:
   - Right-click Canvas → UI → Button
   - Name it "StartMirror3Button"
   - Position it somewhere visible

2. **Add Script**:
   - Select the button
   - Add Component → **Mirror3_TestButton**

3. **Test**:
   - Play game
   - Click button
   - Check Console for shuffle messages

### Solution B: Manual Test in Inspector

1. **Play the game**
2. **In Hierarchy**, find GameObject with **Mirror3_VanityTerror**
3. **In Inspector**, right-click script name
4. **Click "Test Shuffle"**
5. **Check if pages shuffle**

### Solution C: Call from Interaction Script

If you have a mirror interaction script:

```csharp
public void OnMirror3Interact()
{
    Debug.Log("=== MIRROR 3 INTERACTION ===");
    
    Mirror3_VanityTerror mirror3 = FindObjectOfType<Mirror3_VanityTerror>();
    
    if (mirror3 != null)
    {
        Debug.Log("Found Mirror3, calling StartPuzzle()");
        mirror3.StartPuzzle();
    }
    else
    {
        Debug.LogError("Mirror3_VanityTerror NOT FOUND!");
    }
}
```

### Expected Console Output When Shuffle Works:
```
[Mirror3] ========== STARTING VANITY TERROR PUZZLE ==========
[Mirror3] ✅ Puzzle panel shown
[Mirror3] About to shuffle pages...
[Mirror3] ========== SHUFFLE PAGES START ==========
[Mirror3] Number of slots: 8
[Mirror3] Checking slot 0: Slot_1
[Mirror3]   - Found child: DiaryPage_1
[Mirror3]   ✅ Added DiaryPage_1 to shuffle list
... (continues for all 8)
[Mirror3] Total pages found: 8
[Mirror3] Shuffling pages...
[Mirror3] Slot_1 now has: DiaryPage_5  ← Random!
[Mirror3] Slot_2 now has: DiaryPage_1  ← Random!
[Mirror3] ========== SHUFFLE COMPLETE ==========
```

---

## Complete Setup Checklist

### Step 1: Hierarchy Setup
```
Mirror3_Panel
├── Timer_Text
├── Slots_Container
│   ├── Slot_1
│   │   └── DiaryPage_1
│   ├── Slot_2
│   │   └── DiaryPage_2
│   ├── Slot_3
│   │   └── DiaryPage_3
│   ├── Slot_4
│   │   └── DiaryPage_4
│   ├── Slot_5
│   │   └── DiaryPage_5
│   ├── Slot_6
│   │   └── DiaryPage_6
│   ├── Slot_7
│   │   └── DiaryPage_7
│   └── Slot_8
│       └── DiaryPage_8
└── StartButton (for testing)
```

### Step 2: Each DiaryPage Setup
For EACH DiaryPage (1-8):

1. **Image Component**:
   - ✅ Has Image component
   - Source Image assigned

2. **DraggableItem Component**:
   - Item Id = GameObject name (e.g., "DiaryPage_1")
   - Puzzle Number = **3**
   - Detection Radius = **150** (or higher)
   - Return To Original Position = ✅ checked
   - **Stay In Panel = ✅ checked** ← IMPORTANT!
   - Fade While Dragging = ✅ checked

3. **RectTransform**:
   - Anchored Position = (0, 0)
   - Scale = (1, 1, 1)

### Step 3: Mirror3_VanityTerror Setup

Select GameObject with Mirror3_VanityTerror:

1. **Puzzle Panel** = Mirror3_Panel
2. **Timer Text** = Timer_Text
3. **Diary Slots** array:
   - Size = **8**
   - Element 0 = Slot_1
   - Element 1 = Slot_2
   - Element 2 = Slot_3
   - Element 3 = Slot_4
   - Element 4 = Slot_5
   - Element 5 = Slot_6
   - Element 6 = Slot_7
   - Element 7 = Slot_8

4. **Audio Clips** (optional):
   - Paper Rustle Sound
   - Success Sound
   - Emily Scream Sound

5. **Success/Failure** (optional):
   - Success Effect
   - Emily Jumpscare Panel

### Step 4: Test Each Feature

#### Test 1: Shuffle
1. Play game
2. Trigger StartPuzzle() (button or Inspector)
3. **Expected**: Pages move to random slots
4. **Check Console**: Should see shuffle messages

#### Test 2: Drag
1. Try to drag a page
2. **Expected**: 
   - Page becomes semi-transparent
   - Page follows mouse/finger
   - Page STAYS in puzzle panel (doesn't go outside)
3. **Check Console**: `[DraggableItem] Started dragging: DiaryPage_X`

#### Test 3: Drop on Empty Slot
1. Drag a page to an empty slot
2. **Expected**: Page moves to that slot
3. **Check Console**: 
   ```
   [Mirror3] Target slot 'Slot_X' is empty, moving page
   [Mirror3] ✅ Moved 'DiaryPage_X' to 'Slot_X'
   ```

#### Test 4: Swap (Drop on Occupied Slot)
1. Drag a page to a slot that has another page
2. **Expected**: Both pages SWAP positions
3. **Check Console**:
   ```
   [Mirror3] Target slot 'Slot_2' already has page: 'DiaryPage_5'
   [Mirror3] 🔄 SWAPPING: 'DiaryPage_1' ↔ 'DiaryPage_5'
   [Mirror3] ✅ Moved 'DiaryPage_5' to 'Slot_1'
   [Mirror3] ✅ Moved 'DiaryPage_1' to 'Slot_2'
   ```

#### Test 5: Solve Puzzle
1. Arrange pages in correct order (DiaryPage_1 to DiaryPage_8)
2. **Expected**: 
   - Console shows: `[Mirror3] ✅ PUZZLE SOLVED!`
   - Success dialogue appears
   - Puzzle closes

---

## Common Mistakes

### ❌ Mistake 1: Stay In Panel Not Checked
```
DraggableItem:
- Stay In Panel: ☐ (unchecked)  ← WRONG!
```
**Result**: Pages go to Canvas root, hard to control

**Fix**: Check ✅ Stay In Panel for all DiaryPages

### ❌ Mistake 2: Wrong Puzzle Number
```
DraggableItem:
- Puzzle Number: 1  ← WRONG! Should be 3
```
**Result**: OnPagePlacedInSlot not called, no swap

**Fix**: Set Puzzle Number = 3

### ❌ Mistake 3: Pages Not Children of Slots
```
Slots_Container
├── Slot_1
├── DiaryPage_1  ← WRONG! Should be child of Slot_1
```
**Result**: Shuffle doesn't find pages, swap doesn't work

**Fix**: Drag DiaryPage_1 onto Slot_1 to make it a child

### ❌ Mistake 4: Slots Not Assigned
```
Mirror3_VanityTerror:
- Diary Slots: Size = 0  ← WRONG! Should be 8
```
**Result**: Nothing works

**Fix**: Set size to 8, assign all slots in order

---

## Debugging Steps

### If Shuffle Not Working:
1. Check Console for: `[Mirror3] ========== STARTING VANITY TERROR PUZZLE ==========`
2. If NOT there: StartPuzzle() not being called
3. If there but no shuffle: Check "Total pages found" - should be 8
4. If less than 8: Some slots don't have DiaryPage children

### If Swap Not Working:
1. Drag a page to another slot
2. Check Console for: `[Mirror3] OnPagePlacedInSlot called`
3. If NOT there: Puzzle Number is wrong (should be 3)
4. If there but no swap: Check for `🔄 SWAPPING` message
5. If no swap message: Target slot's page not detected

### If Pages Go Outside Panel:
1. Check DraggableItem component
2. Make sure **Stay In Panel = ✅ checked**
3. If still happening: Check Console for "staying in panel" message
4. If not there: Script not updated, reload Unity

---

## Quick Fix Summary

1. **All DiaryPages**: Set Puzzle Number = 3, Check Stay In Panel
2. **Mirror3_VanityTerror**: Assign all 8 slots in order
3. **Test**: Use button or Inspector to call StartPuzzle()
4. **Verify**: Check Console for detailed debug messages

Follow these steps and everything should work! 🎯
