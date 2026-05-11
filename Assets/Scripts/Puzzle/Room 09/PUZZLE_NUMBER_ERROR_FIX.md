# ❌ Fix: Invalid Puzzle Number Error

## Error Message:
```
[DraggableItem] Invalid puzzle number: 7
```

## What This Means:
May DraggableItem component na may MALING puzzle number sa Inspector.

## Valid Puzzle Numbers:
- **1** = Mirror 1 (Medicine Cabinet)
- **2** = Mirror 2 (Bathtub Drain)
- **3** = Mirror 3 (Vanity Terror)
- **4** = Mirror 4 (Evidence Sequence)

## How to Fix:

### Step 1: Find the Problem GameObject
Tingnan mo yung Console sa Unity. After the error, makikita mo:
```
[DraggableItem] ❌ INVALID PUZZLE NUMBER on GameObject 'DiaryPage_1'!
[DraggableItem] Item ID: 'DiaryPage_1', Puzzle Number: 7
```

Yung GameObject name ay nakalagay dun (example: `DiaryPage_1`)

### Step 2: Select the GameObject
1. Sa Unity Hierarchy, hanapin yung GameObject na nakalagay sa error
2. Click mo para ma-select

### Step 3: Check the Inspector
1. Tingnan yung **DraggableItem** component
2. Hanapin yung field na **Puzzle Number**
3. Tingnan kung ano yung value (example: 7)

### Step 4: Change to Correct Number
Palitan mo yung Puzzle Number based sa puzzle:

**Para sa Medicine Cabinet (Mirror 1)**:
- Bottles: Antidepressants_1973, Lithium_1974, etc.
- Puzzle Number = **1**

**Para sa Bathtub Drain (Mirror 2)**:
- Note pieces: Note_Piece_1, Note_Piece_2, etc.
- Puzzle Number = **2**

**Para sa Vanity Terror (Mirror 3)**:
- Diary pages: DiaryPage_1, DiaryPage_2, etc.
- Puzzle Number = **3**

**Para sa Evidence Sequence (Mirror 4)**:
- Evidence items: Photo_1, Letter_1, etc.
- Puzzle Number = **4**

### Step 5: Save and Test
1. Save yung scene (Ctrl+S)
2. Test ulit yung puzzle
3. Error dapat mawala na

---

## Common Causes:

### 1. Copy-Paste Error
Nag-copy ka ng GameObject from another puzzle, hindi mo na-update yung puzzle number.

**Fix**: Update yung Puzzle Number sa Inspector

### 2. Typo sa Inspector
Nag-type ka ng wrong number (like 7 instead of 3)

**Fix**: Double-check yung number, dapat 1-4 lang

### 3. Multiple DraggableItem Components
May dalawang DraggableItem component sa same GameObject

**Fix**: Remove yung extra component, dapat isa lang

---

## Quick Reference Table:

| Puzzle | Puzzle Number | Item Examples |
|--------|--------------|---------------|
| Mirror 1: Medicine Cabinet | **1** | Antidepressants_1973, Lithium_1974, Valium_1975, PainPills_1975, SleepingPills_1976, UnknownPills_1976 |
| Mirror 2: Bathtub Drain | **2** | Note_Piece_1, Note_Piece_2, Note_Piece_3, Note_Piece_4 |
| Mirror 3: Vanity Terror | **3** | DiaryPage_1, DiaryPage_2, DiaryPage_3, DiaryPage_4, DiaryPage_5, DiaryPage_6, DiaryPage_7, DiaryPage_8 |
| Mirror 4: Evidence Sequence | **4** | Photo_1, Letter_1, Newspaper_1, etc. |

---

## Prevention Tips:

1. **Use Prefabs**: Create a prefab for each puzzle's draggable items with correct puzzle number
2. **Name Convention**: Name your items clearly (DiaryPage_1, Note_Piece_1, etc.)
3. **Double-Check**: After creating items, verify puzzle number in Inspector
4. **Test Early**: Test each puzzle as you build it to catch errors early

---

## Still Getting Error?

### Check All Items in Puzzle:
1. Go to your puzzle panel in Hierarchy
2. Expand all children
3. Find ALL GameObjects with DraggableItem component
4. Verify each one has correct Puzzle Number (1-4)

### Use Search:
1. In Hierarchy, search for "DraggableItem" (type: t:DraggableItem)
2. Check each result
3. Fix any with wrong puzzle number

---

## Updated Error Message:
After the fix, the error message now shows:
- GameObject name
- Item ID
- Current puzzle number
- Valid puzzle numbers

This makes it easier to find and fix the problem! 🎯
