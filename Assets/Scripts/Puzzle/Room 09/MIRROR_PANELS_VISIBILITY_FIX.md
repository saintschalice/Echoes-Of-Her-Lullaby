# All Mirrors - Panel Visibility Fix

## Problem:
Puzzle elements (bottles, pages, etc.) are visible BEFORE and AFTER puzzles.

---

## Root Cause:

### Issue 1: Items Outside Panel
Kung yung puzzle items (bottles, pages, etc.) ay **HINDI nasa loob** ng puzzle panel, hindi sila matatago pag nag-hide yung panel.

### Issue 2: No Start() Method
Kung walang `Start()` method na nag-hide ng panel, visible agad yung panel from the beginning.

---

## Solution for Each Mirror:

### ✅ Mirror 1 (Medicine Cabinet)

**Fixed**: Added `Start()` method

**Hierarchy Check**:
```
Mirror1_Panel (puzzlePanel) ← This should hide/show
├── Timer_Text
├── Mistakes_Text
├── Hint_Text
├── Slot_1
├── Slot_2
├── Slot_3
├── Slot_4
├── Slot_5
├── Slot_6
├── Antidepressants_1973  ← Must be CHILD of panel!
├── Lithium_1974
├── Valium_1975
├── PainPills_1975
├── SleepingPills_1976
└── UnknownPills_1976
```

**If bottles are OUTSIDE panel**:
1. Select all 6 bottles
2. Drag them ONTO Mirror1_Panel to make them children
3. Test - bottles should hide with panel

---

### ✅ Mirror 2 (Bathtub Drain)

**Status**: Already has `Start()` method ✅

**Hierarchy Check**:
```
Mirror2_Panel (puzzlePanel)
├── Timer_Text
├── Bathtub_Container
│   ├── Bathtub_Image
│   └── DrainCover_Button
└── NotePieces_Container
    ├── Slot_1
    ├── Slot_2
    ├── Slot_3
    ├── Slot_4
    ├── Note_Piece_1  ← Must be CHILD of container!
    ├── Note_Piece_2
    ├── Note_Piece_3
    └── Note_Piece_4
```

**Error**: "Mirror 2 missing Mirror2_BathtubDrain component!"
- This means your Mirror 2 GameObject doesn't have the script
- **Fix**: Add `Mirror2_BathtubDrain` component to Mirror 2 GameObject

---

### ✅ Mirror 3 (Diary Arrangement)

**Check if has Start() method**:

If using `Mirror3_DiaryArrangement`:
```csharp
void Start()
{
    if (puzzlePanel != null) puzzlePanel.SetActive(false);
    
    // Initialize - each page starts in its corresponding slot
    for (int i = 0; i < 8; i++)
    {
        slotToPage[i] = i;
    }
}
```

**Hierarchy Check**:
```
Mirror3_Panel (puzzlePanel)
├── Timer_Text
├── Slot_1
├── Slot_2
├── ... (8 slots total)
├── DiaryPage_1  ← Must be CHILD of panel!
├── DiaryPage_2
├── ... (8 pages total)
```

---

### ✅ Mirror 4 (Evidence Sequence)

**Check if has Start() method** - if not, add it:
```csharp
void Start()
{
    if (puzzlePanel != null) puzzlePanel.SetActive(false);
}
```

---

## Quick Fix Checklist:

### For Each Mirror:

- [ ] **Check Hierarchy**:
  - All puzzle items (bottles, pages, notes) are CHILDREN of puzzle panel
  - NOT siblings or outside

- [ ] **Check Script**:
  - Has `Start()` method
  - `Start()` hides the panel: `puzzlePanel.SetActive(false)`

- [ ] **Check Component**:
  - Mirror GameObject has the correct component:
    - Mirror 1: `Mirror1_MedicineCabinet`
    - Mirror 2: `Mirror2_BathtubDrain`
    - Mirror 3: `Mirror3_DiaryArrangement` (or other version)
    - Mirror 4: `Mirror4_EvidenceSequence`

- [ ] **Test**:
  - Scene loads → Panel HIDDEN
  - Interact → Panel SHOWS
  - Complete → Panel HIDES

---

## Common Mistakes:

### ❌ Mistake 1: Items Outside Panel
```
Canvas
├── Mirror1_Panel
│   └── Timer_Text
├── Antidepressants_1973  ← WRONG! Outside panel
├── Lithium_1974  ← WRONG!
```

**Fix**: Move items INSIDE panel

### ❌ Mistake 2: No Start() Method
```csharp
public class Mirror1_MedicineCabinet : MonoBehaviour
{
    // No Start() method!
    
    public void StartPuzzle()
    {
        puzzlePanel.SetActive(true);
    }
}
```

**Fix**: Add Start() method

### ❌ Mistake 3: Panel Active in Inspector
- Panel's checkbox is CHECKED in Inspector
- Even with Start() hiding it, it flickers visible first

**Fix**: Uncheck panel in Inspector (Start() will keep it hidden)

---

## Testing Each Mirror:

### Test 1: Initial State
1. Load scene
2. Walk around room
3. **Expected**: NO puzzle panels visible
4. **Expected**: NO bottles/pages/notes visible

### Test 2: Start Puzzle
1. Interact with mirror
2. **Expected**: Panel appears
3. **Expected**: Puzzle items appear

### Test 3: Complete Puzzle
1. Solve the puzzle
2. **Expected**: Success dialogue
3. **Expected**: Panel HIDES
4. **Expected**: Items DISAPPEAR

### Test 4: Walk Around After
1. After completing puzzle
2. Walk around room
3. **Expected**: Panel stays HIDDEN
4. **Expected**: Items stay HIDDEN

---

## Debugging:

### If items still visible after puzzle:

**Check 1**: Are items children of panel?
```
Select item → Look at Inspector → Check Transform parent
```

**Check 2**: Is panel actually hiding?
```
Play game → Complete puzzle → Check Hierarchy
Is puzzlePanel active? Should be FALSE
```

**Check 3**: Are there duplicate items?
```
Search Hierarchy for item name
Are there 2 copies? (one in panel, one outside?)
```

### If panel visible at start:

**Check 1**: Does script have Start()?
```
Open script → Search for "void Start()"
```

**Check 2**: Is Start() hiding panel?
```
void Start()
{
    if (puzzlePanel != null) puzzlePanel.SetActive(false); ← This line?
}
```

**Check 3**: Is panel checked in Inspector?
```
Select panel → Look at checkbox next to name
Should be UNCHECKED (or Start() will uncheck it)
```

---

## Summary:

✅ **Mirror 1**: Added Start() method
✅ **Mirror 2**: Already has Start() method
✅ **Mirror 3**: Check if has Start() method
✅ **Mirror 4**: Check if has Start() method

**Key Rule**: All puzzle items must be CHILDREN of puzzle panel!

If items are outside panel, they won't hide when panel hides! 🎯
