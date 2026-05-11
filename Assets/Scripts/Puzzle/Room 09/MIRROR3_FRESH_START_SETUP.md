# Mirror 3 - FRESH START Setup Guide

## Bagong Simula! 🎯

Gawa tayo ng **SUPER SIMPLE** puzzle:
- ✅ Drag and drop lang
- ✅ Automatic swap
- ✅ Auto-complete pag tama na
- ✅ Walang complications!

---

## Step 1: Clean Up Old Components

### Remove These (if present):
1. `Mirror3_VanityTerror` component
2. `Mirror3_VanityTerror_Simple` component
3. `DraggableItem` components from all DiaryPages

**How**: Select each component → Right-click → Remove Component

---

## Step 2: Create Simple Hierarchy

```
Mirror3_Panel
├── Timer_Text
├── Slot_1 (empty GameObject with RectTransform)
├── Slot_2
├── Slot_3
├── Slot_4
├── Slot_5
├── Slot_6
├── Slot_7
├── Slot_8
├── DiaryPage_1 (Image with your diary page sprite)
├── DiaryPage_2
├── DiaryPage_3
├── DiaryPage_4
├── DiaryPage_5
├── DiaryPage_6
├── DiaryPage_7
└── DiaryPage_8
```

**IMPORTANT**: 
- Slots are EMPTY GameObjects (just RectTransform)
- Pages are SIBLINGS of slots (not children!)
- Pages have Image component with sprite

---

## Step 3: Setup Slots

For each Slot (Slot_1 to Slot_8):

1. **Create Empty GameObject**:
   - Right-click Mirror3_Panel → Create Empty
   - Name it "Slot_1" (then Slot_2, etc.)

2. **Add RectTransform** (automatic for UI objects)

3. **Position them** in a grid or row:
   - Example: 2 rows of 4 slots each
   - Or: 1 row of 8 slots

4. **Size**: Make them big enough for pages (e.g., 150x150)

---

## Step 4: Setup Pages

For each DiaryPage (DiaryPage_1 to DiaryPage_8):

1. **Create Image**:
   - Right-click Mirror3_Panel → UI → Image
   - Name it "DiaryPage_1" (then DiaryPage_2, etc.)

2. **Assign Sprite**:
   - In Inspector, assign your diary page sprite

3. **Size**: Same as slots (e.g., 150x150)

4. **Initial Position**: Place each page in its corresponding slot position
   - DiaryPage_1 at Slot_1 position
   - DiaryPage_2 at Slot_2 position
   - etc.

**NOTE**: Pages are NOT children of slots! They're siblings!

---

## Step 5: Add Script

1. **Select Mirror3_Panel** (or create a new GameObject for the script)

2. **Add Component** → `Mirror3_DiaryArrangement`

3. **Assign References**:

### Puzzle Panel:
- Drag Mirror3_Panel here

### Timer Text:
- Drag Timer_Text here

### Slots Array:
- Set Size = 8
- Drag Slot_1 to Element 0
- Drag Slot_2 to Element 1
- ... continue to Slot_8 → Element 7

### Pages Array:
- Set Size = 8
- Drag DiaryPage_1 to Element 0
- Drag DiaryPage_2 to Element 1
- ... continue to DiaryPage_8 → Element 7

### Settings:
- Time Limit = 90
- Snap Distance = 200 (adjust if needed)

### Audio (optional):
- Swap Sound
- Success Sound
- Fail Sound

---

## Step 6: Update Room09_Interactable

On your Mirror 3 GameObject:

1. **Keep** `Room09_Interactable` component
2. **Set** Mirror Number = 3

The script already supports the new component!

---

## How It Works:

### 1. Start Puzzle:
- Pages randomize to different positions
- Timer starts (90 seconds)

### 2. Drag & Drop:
- Click and hold a page
- Drag it to another slot
- Release to drop

### 3. Automatic Swap:
- If you drop on a slot with another page
- Both pages SWAP positions automatically!

### 4. Win Condition:
- Arrange pages in order: DiaryPage_1 → DiaryPage_8
- When correct order is achieved: PUZZLE COMPLETE!

---

## Testing:

### Test 1: Start Puzzle
1. Play game
2. Interact with Mirror 3
3. **Expected**: Pages randomize, timer starts

### Test 2: Drag
1. Click and hold a page
2. **Expected**: Page becomes semi-transparent, follows cursor

### Test 3: Swap
1. Drag a page to another page's position
2. Release
3. **Expected**: Both pages swap positions
4. **Console**: `[Mirror3] Swapping: Page X ↔ Page Y`

### Test 4: Win
1. Arrange pages in correct order (1→8)
2. **Expected**: Success dialogue, puzzle closes
3. **Console**: `[Mirror3] ✅ PUZZLE SOLVED!`

---

## Advantages of This Approach:

| Feature | Old System | New System |
|---------|-----------|------------|
| Setup | Complex, many components | Simple, one script |
| Hierarchy | Pages must be children of slots | Pages are siblings |
| Drag Logic | External DraggableItem | Built-in |
| Swap Logic | Complex, error-prone | Simple, reliable |
| Debugging | Many error points | Few error points |
| Components Needed | 3+ per page | 1 total |

---

## Troubleshooting:

### Problem: Pages don't drag
**Check**:
- Pages have Image component?
- Pages array assigned in Inspector?

### Problem: Swap doesn't work
**Check Console**: Should see "Swapping" message

**If not**:
- Slots array assigned correctly?
- Snap Distance too small? (increase to 300)

### Problem: Puzzle doesn't complete
**Check**:
- Are pages named exactly "DiaryPage_1" to "DiaryPage_8"?
- Are they in correct order in Pages array?

---

## Quick Setup Checklist:

- [ ] Remove old components
- [ ] Create 8 empty slots (Slot_1 to Slot_8)
- [ ] Create 8 page images (DiaryPage_1 to DiaryPage_8)
- [ ] Position slots in grid/row
- [ ] Position pages at slot positions initially
- [ ] Add Mirror3_DiaryArrangement script
- [ ] Assign Puzzle Panel
- [ ] Assign Timer Text
- [ ] Assign all 8 Slots to array (in order!)
- [ ] Assign all 8 Pages to array (in order!)
- [ ] Test!

---

## Summary:

✅ **Super simple** - One script, minimal setup
✅ **No external components** - Everything built-in
✅ **Easy to debug** - Clear console messages
✅ **Reliable** - Tested drag & swap logic
✅ **Auto-complete** - Detects correct order automatically

**Just 3 things**:
1. Create slots and pages
2. Assign arrays in Inspector
3. Play!

That's it! 🎉
