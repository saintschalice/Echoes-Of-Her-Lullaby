# Mirror 3 - SIMPLE VERSION Setup

## Mas Simple na Approach! 🎯

Ito yung **bagong version** na:
- ✅ **NO DraggableItem component needed** - automatic na!
- ✅ **Self-contained** - lahat ng logic nasa isang script
- ✅ **Automatic swap** - built-in na
- ✅ **Automatic shuffle** - built-in na
- ✅ **Walang complications** - plug and play!

---

## Setup Steps

### Step 1: Remove Old Components

Para sa lahat ng DiaryPage (1-8):
1. Select the DiaryPage
2. **Remove DraggableItem component** (if present)
3. That's it! The new script will add what's needed automatically

### Step 2: Setup Hierarchy

Make sure hierarchy is:
```
Mirror3_Panel
├── Timer_Text
├── Slots_Container  ← IMPORTANT: This is the parent!
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
```

**IMPORTANT**: 
- All slots must be children of **Slots_Container**
- Each slot must have ONE DiaryPage child
- Slot names must contain "Slot"
- Page names must contain "DiaryPage"

### Step 3: Add New Script

1. **Find or create** a GameObject for the puzzle (can be Mirror3_Panel itself)
2. **Remove** old `Mirror3_VanityTerror` component (if present)
3. **Add Component** → `Mirror3_VanityTerror_Simple`

### Step 4: Assign References

In Inspector for **Mirror3_VanityTerror_Simple**:

1. **Puzzle Panel** = Mirror3_Panel (the main panel)
2. **Timer Text** = Timer_Text
3. **Slots Container** = Slots_Container ← IMPORTANT!
4. **Audio Clips** (optional):
   - Paper Rustle Sound
   - Success Sound
   - Emily Scream Sound
5. **Success/Failure** (optional):
   - Success Effect
   - Emily Jumpscare Panel

### Step 5: Test!

1. Play the game
2. Call `StartPuzzle()` (via button or Inspector)
3. **Expected**:
   - Pages shuffle automatically
   - Can drag pages
   - Pages swap when dropped on each other
   - Puzzle solves when in correct order

---

## What This Script Does Automatically

### 1. Setup (On Start)
- Finds all slots in Slots_Container
- Finds all pages in slots
- Adds DiaryPage component to each page automatically
- Sets up drag handlers

### 2. Shuffle (On StartPuzzle)
- Collects all pages
- Shuffles them randomly
- Places them in random slots

### 3. Drag & Drop
- Pages can be dragged
- Pages become semi-transparent while dragging
- Pages snap to nearest slot

### 4. Swap
- If dropped on occupied slot: AUTOMATIC SWAP
- If dropped on empty slot: MOVE
- If dropped on nothing: RETURN to original position

### 5. Check Solution
- After each move, checks if pages are in correct order
- If correct: Puzzle completes!

---

## Advantages Over Old System

| Feature | Old System | New System |
|---------|-----------|------------|
| Setup | Manual DraggableItem on each page | Automatic |
| Puzzle Number | Must set to 3 manually | Not needed |
| Stay In Panel | Must check manually | Automatic |
| Detection Radius | Must configure | Built-in (200) |
| Swap Logic | Complex, error-prone | Simple, reliable |
| Finding Pages | Searches everywhere | Knows exact location |
| Debugging | Many error points | Few error points |

---

## Testing Checklist

### ✅ Test 1: Script Setup
1. Play game
2. Check Console: `[Mirror3Simple] Setup complete: 8 slots found`
3. If not 8: Check hierarchy, make sure all slots are in Slots_Container

### ✅ Test 2: Shuffle
1. Call StartPuzzle()
2. Check Console: `[Mirror3Simple] Shuffling pages...`
3. **Expected**: Pages move to random positions
4. Check Console: `[Mirror3Simple] Shuffle complete!`

### ✅ Test 3: Drag
1. Try to drag a page
2. **Expected**: 
   - Page becomes semi-transparent
   - Page follows mouse/finger
   - Console: `[Mirror3Simple] Started dragging: DiaryPage_X`

### ✅ Test 4: Swap
1. Drag a page to another slot with a page
2. **Expected**: Both pages swap positions
3. Console: `[Mirror3Simple] 🔄 SWAPPING: DiaryPage_X ↔ DiaryPage_Y`

### ✅ Test 5: Solve
1. Arrange pages in order (DiaryPage_1 to DiaryPage_8)
2. **Expected**: 
   - Console: `[Mirror3Simple] ✅ PUZZLE SOLVED!`
   - Success dialogue
   - Puzzle closes

---

## Troubleshooting

### Problem: "Setup complete: 0 slots found"
**Cause**: Slots_Container not assigned or wrong

**Fix**:
1. Make sure Slots_Container is assigned in Inspector
2. Make sure all slots are children of Slots_Container
3. Make sure slot names contain "Slot"

### Problem: Pages don't shuffle
**Cause**: StartPuzzle() not called

**Fix**: Add a button or call manually from Inspector

### Problem: Can't drag pages
**Cause**: Pages don't have Image component

**Fix**: Add Image component to each DiaryPage

### Problem: Swap doesn't work
**Cause**: Detection radius too small or slots too far

**Fix**: In script, change `minDistance = 200f` to higher value (e.g., 300f)

---

## Migration from Old System

If you already have the old system setup:

1. **Backup your scene** (just in case)
2. **Select all DiaryPages** (Shift+Click)
3. **Remove DraggableItem component** from all
4. **Remove old Mirror3_VanityTerror** component
5. **Add Mirror3_VanityTerror_Simple** component
6. **Assign Slots_Container** in Inspector
7. **Test!**

---

## Summary

✅ **Simpler setup** - Less components to configure
✅ **More reliable** - Fewer things that can go wrong
✅ **Self-contained** - Everything in one script
✅ **Automatic** - Handles drag, swap, shuffle automatically
✅ **Easier to debug** - Clear console messages

**Just 3 things needed**:
1. Correct hierarchy (Slots_Container → Slots → Pages)
2. Assign Slots_Container in Inspector
3. Call StartPuzzle()

That's it! 🎉
