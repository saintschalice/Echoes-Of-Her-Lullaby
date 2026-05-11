# All Mirrors - Visibility Flow

## Simple Rule:

**Bago puzzle**: Walang nakikita (panel hidden)
**Habang puzzle**: May nakikita (panel shown)
**After puzzle**: Walang nakikita (panel hidden)

---

## Mirror 1 - Medicine Cabinet

### Flow:
1. **Start**: Walang nakikita
2. **Interact**: Lumabas 6 bottles + 6 slots + timer
3. **Solve**: Lahat nawawala

### Hierarchy:
```
Mirror1_Panel
├── Timer_Text
├── Mistakes_Text
├── Hint_Text
├── Slot_1 to Slot_6
├── Antidepressants_1973
├── Lithium_1974
├── Valium_1975
├── PainPills_1975
├── SleepingPills_1976
└── UnknownPills_1976
```

### Code:
- **Start()**: `puzzlePanel.SetActive(false)` ← Hide everything
- **StartPuzzle()**: `puzzlePanel.SetActive(true)` ← Show everything
- **PuzzleSuccess()**: `puzzlePanel.SetActive(false)` ← Hide everything

---

## Mirror 2 - Bathtub Drain

### Flow:
1. **Start**: Walang nakikita
2. **Interact**: Lumabas bathtub + button (walang notes pa)
3. **Click button**: Bathtub disappears, notes appear
4. **Solve**: Lahat nawawala

### Hierarchy:
```
Mirror2_Panel
├── Timer_Text
├── Bathtub_Container
│   ├── Bathtub_Image
│   └── DrainCover_Button
└── NotePieces_Container
    ├── Slot_1 to Slot_4
    ├── Note_Piece_1
    ├── Note_Piece_2
    ├── Note_Piece_3
    └── Note_Piece_4
```

### Code:
- **Start()**: `puzzlePanel.SetActive(false)` ← Hide everything
- **StartPuzzle()**: 
  - `puzzlePanel.SetActive(true)` ← Show panel
  - `bathtubContainer.SetActive(true)` ← Show bathtub
  - `notePiecesContainer.SetActive(false)` ← Hide notes
- **OnDrainCoverClicked()**: 
  - `bathtubContainer.SetActive(false)` ← Hide bathtub
  - `notePiecesContainer.SetActive(true)` ← Show notes
- **PuzzleSuccess()**: `puzzlePanel.SetActive(false)` ← Hide everything

---

## Mirror 3 - Diary Arrangement

### Flow:
1. **Start**: Walang nakikita
2. **Interact**: Lumabas 8 diary pages (shuffled) + 8 slots + timer
3. **Solve**: Lahat nawawala

### Hierarchy:
```
Mirror3_Panel
├── Timer_Text
├── Slot_1 to Slot_8
├── DiaryPage_1
├── DiaryPage_2
├── DiaryPage_3
├── DiaryPage_4
├── DiaryPage_5
├── DiaryPage_6
├── DiaryPage_7
└── DiaryPage_8
```

### Code:
- **Start()**: `puzzlePanel.SetActive(false)` ← Hide everything
- **StartPuzzle()**: `puzzlePanel.SetActive(true)` ← Show everything
- **PuzzleSuccess()**: `puzzlePanel.SetActive(false)` ← Hide everything

---

## Why This Works:

### Unity's SetActive() Rule:
```
Panel.SetActive(false)
├── Hides the panel
└── Hides ALL children of the panel
```

### Example:
```
Mirror1_Panel (SetActive false)
├── Timer_Text ← HIDDEN (child)
├── Slot_1 ← HIDDEN (child)
└── Antidepressants_1973 ← HIDDEN (child)
```

**Lahat ng nasa loob ng panel ay matatago!**

---

## Common Mistake:

### ❌ MALI (Items Outside):
```
Canvas
├── Mirror1_Panel ← Hidden
│   └── Timer_Text ← Hidden
├── Antidepressants_1973 ← STILL VISIBLE! (not a child)
└── Lithium_1974 ← STILL VISIBLE!
```

**Result**: Panel nag-hide, pero bottles nandyan pa rin!

### ✅ TAMA (Items Inside):
```
Canvas
└── Mirror1_Panel ← Hidden
    ├── Timer_Text ← Hidden
    ├── Antidepressants_1973 ← Hidden
    └── Lithium_1974 ← Hidden
```

**Result**: Panel nag-hide, bottles nag-hide din!

---

## Testing All Mirrors:

### For Each Mirror:

**Test 1: Initial State**
- Load scene
- **Expected**: Walang nakikita

**Test 2: Start Puzzle**
- Interact with mirror
- **Expected**: Lumabas yung puzzle items

**Test 3: Complete Puzzle**
- Solve the puzzle
- **Expected**: Lahat nawawala

**Test 4: After Puzzle**
- Walk around
- **Expected**: Walang nakikita pa rin

---

## Quick Fix Checklist:

Para sa bawat mirror:

- [ ] **Check Hierarchy**: All items are CHILDREN of panel
- [ ] **Check Start()**: Panel is hidden at start
- [ ] **Check StartPuzzle()**: Panel is shown when puzzle starts
- [ ] **Check PuzzleSuccess()**: Panel is hidden when puzzle completes
- [ ] **Test**: Items hide/show with panel

---

## Summary:

**The Golden Rule**:
> **All puzzle items must be CHILDREN of the puzzle panel!**

**The Flow**:
1. Start → Panel hidden → Items hidden
2. Interact → Panel shown → Items shown
3. Complete → Panel hidden → Items hidden

**No Special Code Needed**:
- Just `SetActive(true/false)` on the panel
- Unity automatically hides/shows all children

Yan lang! 🎯
