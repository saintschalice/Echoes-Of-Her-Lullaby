# Mirror 2 - Correct Flow (Bathtub Drain)

## Flow ng Puzzle:

### 1. Bago mag-interact
- **Walang nakikita** - panel hidden

### 2. Pag-interact sa Mirror 2
- **Lumabas**: Bathtub with water + Drain button
- **Hindi pa lumalabas**: Notes (torn pages)

### 3. Pag-click ng Drain Button
- Water drains (sprite change)
- Dialogue
- **Bathtub disappears**
- **Notes appear** (4 torn pages + slots)

### 4. Pag-solve ng puzzle
- Success dialogue
- **Lahat nawawala** - panel hidden

---

## Unity Hierarchy Setup:

### Correct Structure:
```
Mirror2_Panel (puzzlePanel)
├── Timer_Text
├── Bathtub_Container
│   ├── Bathtub_Image
│   └── DrainCover_Button
└── NotePieces_Container (or Assembly_Area)
    ├── Slot_1
    ├── Slot_2
    ├── Slot_3
    ├── Slot_4
    ├── Note_Piece_1
    ├── Note_Piece_2
    ├── Note_Piece_3
    └── Note_Piece_4
```

**IMPORTANTE**: 
- Lahat ng items (bathtub, button, notes, slots) ay **nasa loob ng Mirror2_Panel**
- Bathtub at button ay nasa loob ng `Bathtub_Container`
- Notes at slots ay nasa loob ng `NotePieces_Container`

---

## How It Works:

### Start() Method:
```csharp
// Hide ENTIRE panel (including bathtub and notes)
puzzlePanel.SetActive(false);
```

### StartPuzzle() Method:
```csharp
// Show panel
puzzlePanel.SetActive(true);

// Show bathtub, hide notes
bathtubContainer.SetActive(true);
notePiecesContainer.SetActive(false);
```

### OnDrainCoverClicked() → DrainWaterSequence():
```csharp
// Hide bathtub
bathtubContainer.SetActive(false);

// Show notes
notePiecesContainer.SetActive(true);
```

### PuzzleSuccess():
```csharp
// Hide entire panel (bathtub and notes both hidden)
puzzlePanel.SetActive(false);
```

---

## Testing:

### Test 1: Initial State
1. Load scene
2. **Expected**: Walang nakikita (panel hidden)

### Test 2: Interact with Mirror
1. Click mirror
2. **Expected**: Lumabas bathtub + button
3. **Expected**: Walang notes pa

### Test 3: Click Drain Button
1. Click button
2. **Expected**: Water drains
3. **Expected**: Bathtub disappears
4. **Expected**: Notes appear

### Test 4: Solve Puzzle
1. Arrange notes correctly
2. **Expected**: Success dialogue
3. **Expected**: Lahat nawawala (panel hidden)

### Test 5: After Puzzle
1. Walk around
2. **Expected**: Walang nakikita pa rin

---

## Common Issues:

### Issue 1: Notes visible from start
**Cause**: `NotePieces_Container` is active in Inspector
**Fix**: Uncheck `NotePieces_Container` in Inspector (or let code handle it)

### Issue 2: Bathtub doesn't disappear after clicking button
**Cause**: `Bathtub_Container` not assigned in Inspector
**Fix**: Assign `Bathtub_Container` reference in Mirror2_BathtubDrain component

### Issue 3: Notes don't appear after draining
**Cause**: `NotePieces_Container` not assigned in Inspector
**Fix**: Assign `NotePieces_Container` reference in Mirror2_BathtubDrain component

### Issue 4: Items still visible after puzzle
**Cause**: Items are NOT children of panel
**Fix**: Move all items inside `Mirror2_Panel` (see hierarchy above)

---

## Inspector Setup:

### Mirror2_BathtubDrain Component:

**Bathtub Sprites**:
- Bathtub Image: `Bathtub_Image` (Image component)
- Bathtub With Water: Sprite with water
- Bathtub Without Water: Sprite without water (empty)

**UI References**:
- Puzzle Panel: `Mirror2_Panel`
- Timer Text: `Timer_Text`
- Drain Cover Button: `DrainCover_Button`
- Bathtub Container: `Bathtub_Container` ← IMPORTANTE!
- Note Pieces Container: `NotePieces_Container` ← IMPORTANTE!
- Assembly Slots: Array of 4 slots (Slot_1 to Slot_4)

**Puzzle Settings**:
- Time Limit: 90

**Audio**: (optional)
- Drain Open Sound
- Water Drain Sound
- Paper Rustle Sound
- Success Sound
- Emily Scream Sound

**Success/Failure**:
- Success Effect
- Emily Jumpscare Panel

---

## Summary:

**Key Points**:
1. Panel starts HIDDEN (walang nakikita)
2. Interact → Panel SHOWS with bathtub (walang notes pa)
3. Click button → Bathtub HIDES, notes SHOW
4. Solve → Panel HIDES (lahat nawawala)

**Critical Setup**:
- All items must be CHILDREN of `Mirror2_Panel`
- Bathtub/button inside `Bathtub_Container`
- Notes/slots inside `NotePieces_Container`
- Both containers inside `Mirror2_Panel`

Yan lang! 🎯
