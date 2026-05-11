# Room 09 Mirror Puzzles - Current Status

## ✅ MIRROR 1: Medicine Cabinet (COMPLETE)
**Status**: Fully implemented and tested
**Script**: `Mirror1_MedicineCabinet.cs`

### Features:
- 6 bottles with specific placement validation
- Each slot only accepts ONE correct bottle
- 3-strikes system (3 mistakes = Emily attack)
- Visual hints (mistakes counter, hint text)
- Detection radius: 150 (configurable)
- Time limit: 90 seconds

### Correct Bottle Order:
1. Slot_1 → Antidepressants_1973
2. Slot_2 → Lithium_1974
3. Slot_3 → Valium_1975
4. Slot_4 → PainPills_1975
5. Slot_5 → SleepingPills_1976
6. Slot_6 → UnknownPills_1976

### Setup Complete:
- ✅ Script compiled without errors
- ✅ Validation system working
- ✅ 3-strikes system implemented
- ✅ Detection radius increased for easier placement
- ✅ Item IDs match GameObject names

---

## ✅ MIRROR 2: Bathtub Drain (SCRIPT COMPLETE - NEEDS UNITY SETUP)
**Status**: Script ready, needs Unity configuration
**Script**: `Mirror2_BathtubDrain.cs`

### Features:
- Click drain button → water drains (sprite change)
- Dialogue appears after draining
- Bathtub container hides, torn pages container shows
- 4 note pieces to assemble in correct order
- Time limit: 90 seconds

### Flow:
1. Player clicks `DrainCover_Button`
2. Bathtub sprite changes from water → empty
3. Dialogue: "I found torn notes in the bathtub!"
4. `Bathtub_Container` hides
5. `NotePieces_Container` shows with 4 torn pages
6. Player drags pages to slots in correct order

### Correct Note Order:
1. Slot_1 → Note_Piece_1
2. Slot_2 → Note_Piece_2
3. Slot_3 → Note_Piece_3
4. Slot_4 → Note_Piece_4

### Unity Setup Needed:
1. **Fix Button Clickability Issue**:
   - Check if `Bathtub_Container` has Image component
   - If yes: Uncheck "Raycast Target" on the Image
   - OR: Remove Image component from container
   - OR: Move `DrainCover_Button` outside container as sibling

2. **Assign References in Inspector**:
   - Bathtub Image (the Image component showing bathtub)
   - Bathtub With Water sprite
   - Bathtub Without Water sprite
   - Drain Cover Button
   - Bathtub Container (parent of bathtub + button)
   - Note Pieces Container (parent of torn pages)
   - Assembly Slots array (4 slots)

3. **Setup Note Pieces**:
   - Each note piece needs DraggableItem component
   - Item Id = GameObject name (Note_Piece_1, etc.)
   - Puzzle Number = 2

### Troubleshooting:
- If button not clickable: See `BUTTON_NOT_CLICKABLE_FIX.md`
- Test button with `TestButtonClick.cs` script

---

## ✅ MIRROR 3: Vanity Terror (SCRIPT COMPLETE - NEEDS UNITY SETUP)
**Status**: Script ready with shuffle system, needs Unity configuration
**Script**: `Mirror3_VanityTerror.cs`

### Features:
- 8 diary pages START in slots but SHUFFLED randomly
- Player rearranges to chronological order (1→8)
- Pages shuffle when puzzle starts
- Time limit: 90 seconds

### How It Works:
1. In Unity: Create 8 diary pages as CHILDREN of slots
2. When `StartPuzzle()` runs: Pages shuffle randomly
3. Player drags pages between slots to rearrange
4. Correct order: DiaryPage_1 → DiaryPage_2 → ... → DiaryPage_8

### Correct Page Order:
1. Slot_1 → DiaryPage_1 (earliest date)
2. Slot_2 → DiaryPage_2
3. Slot_3 → DiaryPage_3
4. Slot_4 → DiaryPage_4
5. Slot_5 → DiaryPage_5
6. Slot_6 → DiaryPage_6
7. Slot_7 → DiaryPage_7
8. Slot_8 → DiaryPage_8 (latest date)

### Unity Setup Needed:
1. **Create Hierarchy**:
   ```
   Mirror3_Panel
   ├── Slot_1
   │   └── DiaryPage_1 (child of slot!)
   ├── Slot_2
   │   └── DiaryPage_2
   ├── Slot_3
   │   └── DiaryPage_3
   ... (continue for all 8)
   ```

2. **Setup Each Diary Page**:
   - Add DraggableItem component
   - Item Id = GameObject name (DiaryPage_1, etc.)
   - Puzzle Number = 3
   - Detection Radius = 150

3. **Assign References in Inspector**:
   - Puzzle Panel
   - Timer Text
   - Diary Slots array (8 slots in order: Slot_1 to Slot_8)
   - Audio clips
   - Success effect
   - Emily jumpscare panel

4. **Add Dates to Pages**:
   - Each page should have visible date text
   - Make dates obvious so player knows chronological order
   - Example: "January 1973", "March 1973", "June 1973", etc.

### How Shuffle Works:
- `ShufflePages()` finds all DiaryPage children of slots
- Shuffles them using Fisher-Yates algorithm
- Redistributes to slots randomly
- Player must rearrange to correct order

---

## 📋 NEXT STEPS

### For Mirror 2 (Bathtub):
1. ✅ Fix button clickability (check container Image component)
2. ✅ Assign all references in Inspector
3. ✅ Test button click → water drain → sprite change
4. ✅ Test container swap (bathtub hides, pages show)
5. ✅ Test note piece assembly

### For Mirror 3 (Vanity):
1. ✅ Create 8 diary pages as children of slots
2. ✅ Add DraggableItem to each page
3. ✅ Assign all references in Inspector
4. ✅ Test shuffle on puzzle start
5. ✅ Test drag-and-drop between slots
6. ✅ Test correct order completion

---

## 🔧 COMMON ISSUES & FIXES

### Button Not Clickable (Mirror 2):
**Problem**: DrainCover_Button not responding to clicks
**Cause**: Parent container has Image component blocking raycasts
**Fix**: 
- Option 1: Uncheck "Raycast Target" on Bathtub_Container's Image
- Option 2: Remove Image component from Bathtub_Container
- Option 3: Move button outside container

### Slots Moving (All Mirrors):
**Problem**: Slots move when items placed
**Cause**: Horizontal/Vertical Layout Group on container
**Fix**: Disable or remove Layout Group component

### Items Not Snapping to Slots:
**Problem**: Items don't snap to slots when dropped nearby
**Cause**: Detection radius too small
**Fix**: Increase `detectionRadius` in DraggableItem (try 150-200)

### Wrong Item Accepted in Slot:
**Problem**: Any item can be placed in any slot
**Cause**: Puzzle script not validating placement
**Fix**: 
- Mirror 1: Already validates (3-strikes system)
- Mirror 2 & 3: Currently accept any placement (validation can be added if needed)

---

## 📝 DOCUMENTATION FILES

- `MIRROR2_UPDATED_FLOW.md` - Mirror 2 flow and setup
- `BUTTON_NOT_CLICKABLE_FIX.md` - Button troubleshooting
- `MIRROR3_SHUFFLE_SETUP.md` - Mirror 3 shuffle system
- `ITEM_IDS_REFERENCE.md` - All item IDs for puzzles
- `DETECTION_RADIUS_SETUP.md` - How to adjust detection

---

## ✅ COMPILATION STATUS

**All scripts compile without errors!**
- ✅ Mirror1_MedicineCabinet.cs - No errors
- ✅ Mirror2_BathtubDrain.cs - No errors
- ✅ Mirror3_VanityTerror.cs - No errors
- ✅ DraggableItem.cs - No errors

**Ready for Unity setup and testing!**
