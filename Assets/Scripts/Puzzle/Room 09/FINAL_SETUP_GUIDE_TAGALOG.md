# Room 09 - Final Setup Guide (Tagalog)

## Problema:
Nakikita pa rin yung puzzle items (bottles, notes, diary pages) kahit tapos na yung puzzle.

## Solusyon:
**Ilipat lahat ng items sa loob ng panel!**

---

## Paano Gumagana:

### Simple Rule:
```
Kung naka-hide yung panel → Naka-hide din lahat ng nasa loob
Kung naka-show yung panel → Naka-show din lahat ng nasa loob
```

### Example:
```
Mirror1_Panel (HIDDEN)
├── Timer_Text (HIDDEN)
├── Slot_1 (HIDDEN)
└── Antidepressants_1973 (HIDDEN)
```

**Lahat ng nasa loob ay sumusunod sa panel!**

---

## Setup Para sa Bawat Mirror:

### Mirror 1 - Medicine Cabinet

**Dapat nasa loob ng `Mirror1_Panel`**:
- Timer_Text
- Mistakes_Text
- Hint_Text
- Slot_1, Slot_2, Slot_3, Slot_4, Slot_5, Slot_6
- Antidepressants_1973
- Lithium_1974
- Valium_1975
- PainPills_1975
- SleepingPills_1976
- UnknownPills_1976

**Flow**:
1. Start → Walang nakikita
2. Interact → Lumabas lahat (bottles, slots, timer)
3. Solve → Nawawala lahat

---

### Mirror 2 - Bathtub Drain

**Dapat nasa loob ng `Mirror2_Panel`**:
- Timer_Text
- **Bathtub_Container**
  - Bathtub_Image
  - DrainCover_Button
- **NotePieces_Container** (or Assembly_Area)
  - Slot_1, Slot_2, Slot_3, Slot_4
  - Note_Piece_1
  - Note_Piece_2
  - Note_Piece_3
  - Note_Piece_4

**Flow**:
1. Start → Walang nakikita
2. Interact → Lumabas bathtub + button (walang notes pa)
3. Click button → Bathtub nawawala, notes lumalabas
4. Solve → Nawawala lahat

**IMPORTANTE**: 
- Bathtub at button ay nasa loob ng `Bathtub_Container`
- Notes at slots ay nasa loob ng `NotePieces_Container`
- Both containers ay nasa loob ng `Mirror2_Panel`

---

### Mirror 3 - Diary Arrangement

**Dapat nasa loob ng `Mirror3_Panel`**:
- Timer_Text
- Slot_1, Slot_2, Slot_3, Slot_4, Slot_5, Slot_6, Slot_7, Slot_8
- DiaryPage_1
- DiaryPage_2
- DiaryPage_3
- DiaryPage_4
- DiaryPage_5
- DiaryPage_6
- DiaryPage_7
- DiaryPage_8

**Flow**:
1. Start → Walang nakikita
2. Interact → Lumabas lahat (pages shuffled, slots, timer)
3. Solve → Nawawala lahat

---

## Paano Ilipat ang Items:

### Step-by-Step:

1. **Buksan ang Hierarchy window** (left side ng Unity)

2. **Hanapin ang panel** para sa mirror:
   - Mirror 1: `Mirror1_Panel`
   - Mirror 2: `Mirror2_Panel`
   - Mirror 3: `Mirror3_Panel`

3. **I-expand ang panel** (click arrow) para makita kung ano ang nasa loob

4. **Check kung nasaan ang items**:
   - Kung nasa loob na → OK! ✅
   - Kung nasa labas → Kailangan ilipat! ❌

5. **Ilipat ang items**:
   - Select lahat ng puzzle items (Shift+Click)
   - Drag sila papunta sa panel name
   - Drop sila sa panel
   - Verify: I-expand panel, dapat nandun na sila

---

## Visual Guide:

### ❌ MALI (Items Labas):
```
Canvas
├── Mirror1_Panel
│   └── Timer_Text
├── Antidepressants_1973  ← LABAS! Hindi matatago!
├── Lithium_1974  ← LABAS!
└── Valium_1975  ← LABAS!
```

**Problema**: Pag nag-hide yung panel, nandyan pa rin yung bottles!

---

### ✅ TAMA (Items Loob):
```
Canvas
└── Mirror1_Panel
    ├── Timer_Text
    ├── Antidepressants_1973  ← LOOB! Matatago!
    ├── Lithium_1974  ← LOOB!
    └── Valium_1975  ← LOOB!
```

**Result**: Pag nag-hide yung panel, nag-hide din yung bottles!

---

## Testing:

### Para sa Bawat Mirror:

**Test 1: Bago mag-start**
1. I-play yung scene
2. Expected: Walang nakikitang panels o items

**Test 2: Pag-interact**
1. I-interact yung mirror
2. Expected: Lumabas yung puzzle

**Test 3: Pag-solve**
1. Tapusin yung puzzle
2. Expected: Nawawala yung puzzle

**Test 4: After puzzle**
1. Lakad-lakad sa room
2. Expected: Walang nakikita pa rin

---

## Common Issues:

### Issue 1: "Items nandyan pa rin after puzzle"
**Cause**: Items ay nasa labas ng panel
**Fix**: Ilipat ang items sa loob ng panel

### Issue 2: "Items nawawala completely, kahit hindi pa nag-start"
**Cause**: Panel ay naka-disable sa Inspector
**Fix**: I-enable yung panel (pero dapat naka-hide pa rin dahil sa Start() method)

### Issue 3: "Walang lumalabas pag nag-interact"
**Cause**: Items ay deleted o naka-disable
**Fix**: I-enable yung items, siguraduhing nasa loob ng panel

### Issue 4: "Mirror 2 missing component error"
**Cause**: Walang `Mirror2_BathtubDrain` script sa Mirror 2 GameObject
**Fix**: Add component → Mirror2_BathtubDrain → Assign references

---

## Inspector Setup:

### Para sa Mirror 2 (Special Case):

**Mirror2_BathtubDrain Component**:

1. **Bathtub Sprites**:
   - Bathtub Image: Drag `Bathtub_Image` GameObject
   - Bathtub With Water: Drag sprite with water
   - Bathtub Without Water: Drag sprite without water

2. **UI References**:
   - Puzzle Panel: Drag `Mirror2_Panel`
   - Timer Text: Drag `Timer_Text`
   - Drain Cover Button: Drag `DrainCover_Button`
   - **Bathtub Container**: Drag `Bathtub_Container` ← IMPORTANTE!
   - **Note Pieces Container**: Drag `NotePieces_Container` ← IMPORTANTE!
   - Assembly Slots: Set size to 4, drag Slot_1 to Slot_4

3. **Puzzle Settings**:
   - Time Limit: 90

---

## Code Summary:

### Walang kailangan baguhin sa code!

Ang code ay simple lang:
- `Start()`: Hide panel
- `StartPuzzle()`: Show panel
- `PuzzleSuccess()`: Hide panel

**Yan lang!** Unity automatically hides/shows lahat ng children.

---

## Final Checklist:

Para sa bawat mirror:

- [ ] All items are CHILDREN of panel (check Hierarchy)
- [ ] Panel has correct script component
- [ ] All references assigned in Inspector
- [ ] Test: Items hide/show with panel

---

## Summary:

**Ang Solusyon**: Ilipat lahat ng puzzle items sa loob ng panel!

**Bakit?**: Kasi pag nag-hide yung panel, automatic nag-hide din lahat ng nasa loob.

**Walang ibang paraan**: Ito lang ang tamang solusyon. Ganyan talaga gumagana ang Unity.

**Quick Steps**:
1. Hanapin ang panel sa Hierarchy
2. I-expand para makita kung ano ang nasa loob
3. Kung nasa labas yung items, i-drag sila papunta sa loob
4. Test - dapat nag-hide na sila kasama ng panel

**Yan lang!** 🎯

---

## Need Help?

Kung may problema pa rin:

1. **Screenshot ng Hierarchy** - para makita kung tama ang setup
2. **Screenshot ng Inspector** - para makita kung assigned ang references
3. **Describe the problem** - ano ang nangyayari vs ano ang expected

Good luck! 🎮
