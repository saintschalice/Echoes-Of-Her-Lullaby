# Tea Party Puzzle - Unity Setup Guide

## 🎯 Overview

Drag and drop puzzle where player places Emily's Cup into the correct slot to complete the tea party ritual.

---

## 📋 What You Need

### UI Elements:
1. **Tea Party Panel** (main container)
2. **Background Image** (tea party scene)
3. **3 Tea Cups** (already placed on table)
4. **Emily's Cup Slot** (empty slot with highlight)
5. **Emily's Cup Draggable** (the cup player drags)
6. **Close Button** (optional)

---

## 🎨 Step-by-Step Setup

### Step 1: Create Tea Party Panel

```
Canvas
└── TeaParty_Panel (GameObject)
    ├── Background (Image)
    ├── TeaTable_Image (Image)
    ├── Cup1_Image (Image) - Already placed
    ├── Cup2_Image (Image) - Already placed
    ├── Cup3_Image (Image) - Already placed
    ├── EmilyCup_Slot (Image) - Empty slot with highlight
    ├── EmilyCup_Draggable (Image) - The cup to drag
    └── Close_Button (Button)
```

### Step 2: Setup Background

```
1. Right-click Canvas → UI → Panel
2. Rename to "TeaParty_Panel"
3. Set Anchor: Stretch (full screen)
4. Set Color: Semi-transparent black (0, 0, 0, 200)
```

### Step 3: Create Tea Table Scene

```
1. Right-click TeaParty_Panel → UI → Image
2. Rename to "TeaTable_Image"
3. Assign sprite: Your tea table background
4. Set size: 800x600 (or your preferred size)
5. Center it on screen
```

### Step 4: Add 3 Placed Cups

```
For each cup (Cup1, Cup2, Cup3):
1. Right-click TeaTable_Image → UI → Image
2. Rename to "Cup1_Image", "Cup2_Image", "Cup3_Image"
3. Assign sprite: Tea cup sprite
4. Set size: 100x100
5. Position them on the table (already placed)
```

### Step 5: Create Emily's Cup Slot

```
1. Right-click TeaTable_Image → UI → Image
2. Rename to "EmilyCup_Slot"
3. Assign sprite: Empty circle or highlight sprite
4. Set size: 120x120 (slightly larger than cup)
5. Position: Where Emily's cup should go
6. Set Color: Yellow or white (this will be the highlight)
```

**Important:** This is the TARGET where the cup should be placed.

### Step 6: Create Draggable Cup

```
1. Right-click TeaParty_Panel → UI → Image
2. Rename to "EmilyCup_Draggable"
3. Assign sprite: Emily's special cup sprite
4. Set size: 100x100
5. Position: Bottom of screen or side (starting position)
6. Add Component: Canvas Group
   - Interactable: ✓ (checked)
   - Block Raycasts: ✓ (checked)
```

**Important:** This is the cup the player will DRAG.

### Step 7: Add Close Button (Optional)

```
1. Right-click TeaParty_Panel → UI → Button
2. Rename to "Close_Button"
3. Position: Top-right corner
4. Set Text: "X" or "Close"
```

---

## 🔧 Script Setup

### Step 1: Add Script to Panel

```
1. Select TeaParty_Panel
2. Add Component → TeaPartyPuzzleUI
```

### Step 2: Assign References

```
TeaPartyPuzzleUI:
  UI References:
    Tea Party Panel: [Drag TeaParty_Panel here]
    Close Button: [Drag Close_Button here]
  
  Drag & Drop:
    Emily Cup Draggable: [Drag EmilyCup_Draggable here]
    Emily Cup Slot: [Drag EmilyCup_Slot here]
    Snap Distance: 50 (adjust as needed)
  
  Visual Feedback:
    Slot Highlight: [Drag EmilyCup_Slot here]
    Normal Color: White (255, 255, 255, 255)
    Highlight Color: Yellow (255, 255, 0, 255)
  
  Audio:
    Cup Place Sound: [Assign sound effect]
    Success Sound: [Assign success sound]
```

---

## 🎮 How It Works

### Gameplay Flow:

1. **Panel Opens**
   - Player sees tea table with 3 cups already placed
   - One empty slot (Emily's Cup Slot) is visible
   - Emily's Cup is at the bottom/side (draggable)

2. **Player Drags Cup**
   - Player clicks/touches Emily's Cup
   - Drags it towards the empty slot
   - Slot highlights when cup is near

3. **Cup Snaps to Slot**
   - If cup is close enough (within snap distance)
   - Cup snaps to slot position
   - Success sound plays

4. **Puzzle Complete**
   - Panel closes after 1 second
   - Memory Cutscene 1 plays
   - Cup removed from inventory

---

## 🎨 Visual Design Tips

### Tea Table Layout:
```
┌─────────────────────────┐
│   Tea Party Panel       │
├─────────────────────────┤
│                         │
│    [Cup1]  [Cup2]       │
│                         │
│    [Cup3]  [SLOT]       │
│         ↑               │
│    Emily's Slot         │
│                         │
│                         │
│    [Emily's Cup]        │
│    (Drag me!)           │
│                         │
└─────────────────────────┘
```

### Color Scheme:
```
Background: Dark semi-transparent
Table: Wooden texture
Cups: White/ceramic
Emily's Cup: Special color (pink, gold, etc.)
Slot Highlight: Yellow or glowing
```

### Sizes:
```
Panel: Full screen (stretch)
Table: 800x600
Regular Cups: 100x100
Emily's Cup: 100x100
Slot: 120x120 (slightly larger)
```

---

## ⚙️ Settings Explained

### Snap Distance:
```
50 = Tight (must be very close)
100 = Normal (recommended)
150 = Loose (easier to snap)
```

### Highlight Colors:
```
Normal Color: White (not highlighted)
Highlight Color: Yellow (when cup is near)
```

### Canvas Group (on Draggable Cup):
```
Interactable: ✓ (can be dragged)
Block Raycasts: ✓ (can receive input)
Ignore Parent Groups: ✗ (respect parent)
```

---

## 🧪 Testing

### Test 1: Drag Cup
```
1. Play Mode
2. Open Tea Party panel
3. Click/touch Emily's Cup
4. Drag it around
5. Should follow finger/mouse ✓
```

### Test 2: Highlight
```
1. Drag cup near slot
2. Slot should highlight (yellow) ✓
3. Drag cup away
4. Slot returns to normal (white) ✓
```

### Test 3: Snap to Slot
```
1. Drag cup close to slot
2. Release
3. Cup should snap to slot ✓
4. Success sound plays ✓
```

### Test 4: Return to Start
```
1. Drag cup away from slot
2. Release (not near slot)
3. Cup returns to start position ✓
```

### Test 5: Complete Puzzle
```
1. Drag cup to slot
2. Wait 1 second
3. Panel closes ✓
4. Cutscene plays ✓
5. Cup removed from inventory ✓
```

---

## 🐛 Troubleshooting

### Issue 1: Can't Drag Cup
```
Problem: Cup doesn't move when dragging

Check:
1. EmilyCup_Draggable has Canvas Group?
2. Canvas Group → Interactable is checked?
3. Canvas Group → Block Raycasts is checked?
4. EventTrigger is added by script (automatic)
```

### Issue 2: Slot Doesn't Highlight
```
Problem: Slot stays white, doesn't turn yellow

Check:
1. Slot Highlight assigned in Inspector?
2. Highlight Color is different from Normal Color?
3. Snap Distance is reasonable (50-150)?
```

### Issue 3: Cup Doesn't Snap
```
Problem: Cup returns to start instead of snapping

Check:
1. Snap Distance too small? Try 100
2. Emily Cup Slot assigned correctly?
3. Both cup and slot are RectTransforms?
```

### Issue 4: Panel Doesn't Close
```
Problem: Panel stays open after solving

Check:
1. Room07UIManager assigned in scene?
2. OnTeaPartySolved() method exists?
3. Check Console for errors
```

### Issue 5: Cup Starts at Wrong Position
```
Problem: Cup appears in wrong place

Fix:
1. Position EmilyCup_Draggable where you want it to start
2. Script saves this as cupStartPosition
3. Cup returns here if not snapped
```

---

## 🎯 Quick Setup Checklist

- [ ] TeaParty_Panel created (full screen)
- [ ] Background/table image added
- [ ] 3 cups placed on table (static)
- [ ] Emily's Cup Slot created (target)
- [ ] Emily's Cup Draggable created (movable)
- [ ] Canvas Group added to draggable cup
- [ ] TeaPartyPuzzleUI script added to panel
- [ ] All references assigned in Inspector
- [ ] Snap distance set (50-150)
- [ ] Colors set (normal and highlight)
- [ ] Close button added (optional)
- [ ] Tested dragging
- [ ] Tested highlighting
- [ ] Tested snapping
- [ ] Tested puzzle completion

---

## 📱 Mobile Considerations

### Touch Input:
```
✓ Works automatically with EventTrigger
✓ Drag with finger
✓ Release to drop
```

### Button Size:
```
Make draggable cup larger for mobile:
Desktop: 100x100
Mobile: 120x120 or 150x150
```

### Snap Distance:
```
Make snapping easier on mobile:
Desktop: 50
Mobile: 100-150
```

---

## 🎨 Visual Examples

### Example 1: Simple Layout
```
┌─────────────────────┐
│  Tea Party          │
├─────────────────────┤
│                     │
│  ☕ ☕ ☕ ⭕         │
│  (3 cups + slot)    │
│                     │
│       ☕            │
│   (Emily's Cup)     │
│                     │
└─────────────────────┘
```

### Example 2: Detailed Layout
```
┌─────────────────────────┐
│    Tea Party Ritual     │
├─────────────────────────┤
│                         │
│   🪑 Table 🪑          │
│                         │
│   ☕    ☕              │
│   Cup1  Cup2            │
│                         │
│   ☕    ⭕              │
│   Cup3  Emily's Slot    │
│                         │
│   Instructions:         │
│   "Place Emily's Cup"   │
│                         │
│        ☕               │
│    Emily's Cup          │
│    (Drag here)          │
│                         │
└─────────────────────────┘
```

---

## 🎉 Summary

### What You Need:
1. Panel with background
2. 3 static cups (already placed)
3. 1 empty slot (target)
4. 1 draggable cup (Emily's)
5. Script with references assigned

### How It Works:
1. Player drags Emily's Cup
2. Slot highlights when near
3. Cup snaps if close enough
4. Puzzle completes
5. Cutscene plays

### Key Settings:
- Snap Distance: 50-150
- Highlight Color: Yellow
- Canvas Group on draggable cup

---

**Setup the panel and test! Drag and drop should work smoothly!** 🎮✨

**Need sprites for cups and table!** 🎨☕
