# Fix: Puzzle Items Not Hiding After Completion

## Problem:
Puzzle items (bottles, notes, diary pages) stay visible after completing the puzzle.

## Root Cause:
**Items are NOT children of the puzzle panel!**

When you call `puzzlePanel.SetActive(false)`, only the panel and its CHILDREN hide. Items outside the panel stay visible.

---

## How to Fix (For Each Mirror):

### Step 1: Find the Puzzle Panel

For each mirror, find the main panel GameObject:
- Mirror 1: `Mirror1_Panel` or `MedicineCabinet_Panel`
- Mirror 2: `Mirror2_Panel` or `Bathtub_Panel`
- Mirror 3: `Mirror3_Panel` or `Diary_Panel`
- Mirror 4: `Mirror4_Panel` or `Evidence_Panel`

### Step 2: Check Current Hierarchy

Select the panel and expand it. Check if ALL puzzle items are children:

**❌ WRONG (Items Outside)**:
```
Canvas
├── Mirror2_Panel
│   ├── Timer_Text
│   └── Slot_1
├── Note_Piece_1  ← OUTSIDE! Won't hide!
├── Note_Piece_2  ← OUTSIDE!
├── Note_Piece_3  ← OUTSIDE!
└── Note_Piece_4  ← OUTSIDE!
```

**✅ CORRECT (Items Inside)**:
```
Canvas
└── Mirror2_Panel
    ├── Timer_Text
    ├── Slot_1
    ├── Slot_2
    ├── Slot_3
    ├── Slot_4
    ├── Note_Piece_1  ← INSIDE! Will hide!
    ├── Note_Piece_2  ← INSIDE!
    ├── Note_Piece_3  ← INSIDE!
    └── Note_Piece_4  ← INSIDE!
```

### Step 3: Move Items Inside Panel

1. **Select all puzzle items** (Shift+Click or Ctrl+Click)
   - For Mirror 1: All 6 bottles
   - For Mirror 2: All 4 note pieces
   - For Mirror 3: All 8 diary pages
   - For Mirror 4: All evidence items

2. **Drag them ONTO the puzzle panel** in Hierarchy
   - This makes them children of the panel

3. **Verify**: Expand the panel - items should be nested inside

---

## For Each Mirror:

### Mirror 1 (Medicine Cabinet)

**Items to move INSIDE `Mirror1_Panel`**:
- Antidepressants_1973
- Lithium_1974
- Valium_1975
- PainPills_1975
- SleepingPills_1976
- UnknownPills_1976

**Also inside**:
- All 6 slots (Slot_1 to Slot_6)
- Timer_Text
- Mistakes_Text
- Hint_Text

---

### Mirror 2 (Bathtub Drain)

**Items to move INSIDE `Mirror2_Panel`**:
- Note_Piece_1
- Note_Piece_2
- Note_Piece_3
- Note_Piece_4

**Also inside**:
- Bathtub_Container (with bathtub image and button)
- NotePieces_Container (parent of slots)
- All 4 slots (Slot_1 to Slot_4)
- Timer_Text

**Note**: The note pieces can be inside `NotePieces_Container` which is inside `Mirror2_Panel`

---

### Mirror 3 (Diary Arrangement)

**Items to move INSIDE `Mirror3_Panel`**:
- DiaryPage_1
- DiaryPage_2
- DiaryPage_3
- DiaryPage_4
- DiaryPage_5
- DiaryPage_6
- DiaryPage_7
- DiaryPage_8

**Also inside**:
- All 8 slots (Slot_1 to Slot_8)
- Timer_Text

---

### Mirror 4 (Evidence Sequence)

**Items to move INSIDE `Mirror4_Panel`**:
- All evidence items (photos, letters, etc.)

**Also inside**:
- All frames/slots
- Timer_Text

---

## Testing After Fix:

### Test 1: Before Puzzle
1. Load scene
2. **Expected**: Panel is HIDDEN
3. **Expected**: Items are HIDDEN (because they're children of hidden panel)

### Test 2: During Puzzle
1. Interact with mirror
2. **Expected**: Panel is SHOWN
3. **Expected**: Items are SHOWN (because they're children of shown panel)

### Test 3: After Puzzle
1. Complete the puzzle
2. **Expected**: Panel is HIDDEN
3. **Expected**: Items are HIDDEN (because they're children of hidden panel)

---

## Why This Happens:

### Unity's SetActive() Behavior:
```csharp
// When you call:
puzzlePanel.SetActive(false);

// Unity does:
// 1. Hide the panel GameObject
// 2. Hide ALL children of the panel
// 3. Does NOT hide siblings or parents
```

### Example:
```
Panel (SetActive false)
├── Child1  ← HIDDEN (child of panel)
├── Child2  ← HIDDEN (child of panel)
└── Child3  ← HIDDEN (child of panel)

Sibling  ← STILL VISIBLE (not a child!)
```

---

## Quick Check Command:

In Unity, you can verify hierarchy:

1. **Select a puzzle item** (e.g., Note_Piece_1)
2. **Look at Inspector → Transform**
3. **Check "Parent"** field
4. **Should say**: "Mirror2_Panel" (or whatever your panel is named)
5. **If it says**: "Canvas" or something else → WRONG! Move it inside panel

---

## Common Mistakes:

### ❌ Mistake 1: Items at Canvas Level
```
Canvas
├── Mirror2_Panel
├── Note_Piece_1  ← WRONG!
```
**Fix**: Drag Note_Piece_1 onto Mirror2_Panel

### ❌ Mistake 2: Items in Wrong Panel
```
Canvas
├── Mirror1_Panel
│   └── Note_Piece_1  ← WRONG! This is Mirror 2's item!
└── Mirror2_Panel
```
**Fix**: Move Note_Piece_1 to Mirror2_Panel

### ❌ Mistake 3: Slots Inside, Items Outside
```
Mirror2_Panel
├── Slot_1  ← Inside
├── Slot_2  ← Inside
Note_Piece_1  ← WRONG! Outside!
```
**Fix**: Move Note_Piece_1 inside Mirror2_Panel

---

## Summary:

**The Golden Rule**: 
> **ALL puzzle items must be CHILDREN (or descendants) of the puzzle panel!**

If items are outside the panel, they won't hide when the panel hides.

**Quick Fix**:
1. Select all puzzle items
2. Drag onto puzzle panel
3. Test - items should now hide with panel

That's it! 🎯
