# 🎮 ROOM 09 - SIMPLE DRAG & DROP SETUP (NO PREFABS)

## 📋 WALANG PREFABS, WALANG PROBLEMA!

Ito ang simpleng paraan para gumawa ng drag-and-drop puzzles DIRECTLY sa Unity, walang prefabs needed!

---

## 🎯 MIRROR 1: MEDICINE CABINET (6 Bottles)

### **STEP 1: Create Panel**

```
1. In Canvas:
   - Right-click → UI → Panel
   - Name: "MedicineC abinet_Panel"
   - Anchor: Stretch (full screen)
   - Color: Black, Alpha: 200 (semi-transparent)
   - Active: ✗ (unchecked)
```

### **STEP 2: Create Title & Timer**

```
1. Inside Panel:
   - Right-click → UI → Text (TextMeshPro)
   - Name: "Title_Text"
   - Text: "Medicine Cabinet"
   - Font Size: 48
   - Position: Top center
   
2. Create Timer:
   - Right-click → UI → Text (TextMeshPro)
   - Name: "Timer_Text"
   - Text: "1:00"
   - Font Size: 36
   - Position: Top right
```

### **STEP 3: Create 6 Slots (Horizontal)**

```
1. Inside Panel:
   - Right-click → UI → Panel
   - Name: "Slots_Container"
   - Position: Center
   - Size: (700, 200)
   
2. Add Horizontal Layout Group:
   - Select Slots_Container
   - Add Component → Horizontal Layout Group
   - Spacing: 20
   - Child Alignment: Middle Center
   - Child Force Expand: Width ✓, Height ✓
```

### **STEP 4: Create 6 Empty Slots**

```
For each slot (repeat 6 times):

1. Inside Slots_Container:
   - Right-click → UI → Image
   - Name: "Slot_1" (then Slot_2, Slot_3, etc.)
   - Color: Dark gray (50, 50, 50, 255)
   - Size: Auto (layout group handles it)
   
2. Add Text Label:
   - Right-click Slot → UI → Text
   - Name: "Label"
   - Text: "1" (then "2", "3", etc.)
   - Font Size: 24
   - Position: Top left corner of slot
```

### **STEP 5: Create 6 Bottles (Draggable)**

```
For each bottle (repeat 6 times):

1. Inside Panel (NOT in Slots_Container):
   - Right-click → UI → Image
   - Name: "Bottle_1973" (then 1974, 1975a, 1975b, 1976a, 1976b)
   - Source Image: Your bottle sprite (or leave white for now)
   - Size: (80, 120)
   - Position: Random scattered positions
   
2. Add Text Label:
   - Right-click Bottle → UI → Text
   - Name: "Year_Label"
   - Text: "1973" (then "1974", etc.)
   - Font Size: 18
   - Position: Bottom of bottle
   - Color: White
   
3. Add DraggableItem Script:
   - Select Bottle
   - Add Component → DraggableItem
   - Item Id: "bottle_1973" (then "bottle_1974", etc.)
   - Puzzle Number: 1
   - Return To Original Position: ✓
```

### **STEP 6: Assign References**

```
1. Select Mirror1_MedicineC abinet GameObject

2. In Inspector, find Mirror1_MedicineCabinet component

3. Assign:
   - Puzzle Panel: Drag "MedicineC abinet_Panel"
   - Timer Text: Drag "Timer_Text"
   - Bottle Slots: (expand to 6)
     - Element 0: Drag "Slot_1"
     - Element 1: Drag "Slot_2"
     - Element 2: Drag "Slot_3"
     - Element 3: Drag "Slot_4"
     - Element 4: Drag "Slot_5"
     - Element 5: Drag "Slot_6"
```

---

## 🛁 MIRROR 2: BATHTUB DRAIN (4 Note Pieces)

### **STEP 1: Create Panel**

```
Same as Mirror 1, but:
- Name: "BathtubDrain_Panel"
- Title: "Bathtub"
```

### **STEP 2: Create Bathtub Image**

```
1. Inside Panel:
   - Right-click → UI → Image
   - Name: "Bathtub_Image"
   - Source Image: Your bathtub sprite
   - Size: (400, 300)
   - Position: Upper center
```

### **STEP 3: Create Drain Cover Button**

```
1. Inside Bathtub_Image:
   - Right-click → UI → Button
   - Name: "DrainCover_Button"
   - Position: Over drain area
   - Size: (80, 80)
   - Text: "Remove Cover"
```

### **STEP 4: Create 4 Assembly Slots (Vertical)**

```
1. Inside Panel:
   - Right-click → UI → Panel
   - Name: "Assembly_Container"
   - Position: Lower center
   - Size: (600, 250)
   
2. Add Vertical Layout Group:
   - Spacing: 10
   - Child Alignment: Upper Center
   
3. Create 4 slots:
   - Right-click Assembly_Container → UI → Image
   - Names: "Slot_1" to "Slot_4"
   - Color: Dark gray
   - Size: (550, 50) each
```

### **STEP 5: Create 4 Note Pieces**

```
For each piece (repeat 4 times):

1. Inside Panel:
   - Right-click → UI → Image
   - Name: "Note_Piece_1" (then 2, 3, 4)
   - Source Image: Your torn note sprite
   - Size: (500, 45)
   - Position: Scattered (initially hidden)
   - Active: ✗ (unchecked - shown after drain opened)
   
2. Add Text:
   - Right-click Note_Piece → UI → Text
   - Text: 
     - Piece 1: "Tonight I"
     - Piece 2: "end this child's"
     - Piece 3: "suffering and"
     - Piece 4: "mine forever"
   - Font Size: 16
   
3. Add DraggableItem:
   - Item Id: "piece1" (then "piece2", etc.)
   - Puzzle Number: 2
```

---

## 💄 MIRROR 3: VANITY TERROR (8 Diary Pages)

### **STEP 1: Create Panel**

```
Same pattern:
- Name: "VanityTerror_Panel"
- Title: "Mother's Diary"
- Timer: "1:30" (90 seconds)
```

### **STEP 2: Create 8 Numbered Slots (Grid)**

```
1. Inside Panel:
   - Right-click → UI → Panel
   - Name: "Slots_Container"
   - Position: Center
   - Size: (900, 600)
   
2. Add Grid Layout Group:
   - Cell Size: (200, 140)
   - Spacing: (10, 10)
   - Constraint: Fixed Column Count = 4
   - Child Alignment: Middle Center
   
3. Create 8 slots:
   - Right-click Slots_Container → UI → Image (repeat 8 times)
   - Names: "Slot_1" to "Slot_8"
   - Color: Dark gray
   - Each has number label: "1", "2", "3"... "8"
```

### **STEP 3: Create 8 Diary Pages**

```
For each page (repeat 8 times):

1. Inside Panel:
   - Right-click → UI → Image
   - Name: "DiaryPage_1" (then 2-8)
   - Color: Old paper color (230, 220, 200)
   - Size: (180, 120)
   - Position: Random scattered
   
2. Add Text (diary content):
   - Right-click DiaryPage → UI → Text
   - Font Size: 10-12 (small)
   - Text: [See diary content below]
   - Color: Dark brown
   
3. Add DraggableItem:
   - Item Id: "page1" (then "page2", etc.)
   - Puzzle Number: 3
```

**Diary Content**:
```
Page 1: "Child defied me at dinner..."
Page 2: "The defiance continues..."
Page 3: "I've increased discipline..."
Page 4: "Strange things happening..."
Page 5: "Supernatural events escalated..."
Page 6: "The presence grows bolder..."
Page 7: "I've made my preparations..."
Page 8: "Everything is ready..."
```

---

## 🔪 MIRROR 4: EVIDENCE SEQUENCE (4 Items)

### **STEP 1: Create Panel**

```
- Name: "EvidenceSequence_Panel"
- Title: "The Plan"
```

### **STEP 2: Create Mirror Image**

```
1. Inside Panel:
   - Right-click → UI → Image
   - Name: "Mirror_Image"
   - Source Image: Large mirror sprite
   - Size: (400, 500)
   - Position: Upper center
```

### **STEP 3: Create 4 Picture Frames**

```
1. Inside Panel:
   - Right-click → UI → Panel
   - Name: "Frames_Container"
   - Position: Below mirror
   - Size: (600, 150)
   
2. Add Horizontal Layout Group:
   - Spacing: 15
   
3. Create 4 frames:
   - Right-click Frames_Container → UI → Image (repeat 4 times)
   - Names: "Frame_1" to "Frame_4"
   - Source Image: Empty frame sprite
   - Size: (120, 120)
   - Each has number: "1", "2", "3", "4"
```

### **STEP 4: Create 4 Evidence Items**

```
For each item:

1. Rope:
   - Right-click Panel → UI → Image
   - Name: "Evidence_Rope"
   - Source Image: Rope sprite
   - Size: (100, 100)
   - Position: Scattered
   - DraggableItem: Item Id = "rope", Puzzle Number = 4

2. Pills:
   - Name: "Evidence_Pills"
   - Source Image: Pills sprite
   - Item Id = "pills"

3. Knife:
   - Name: "Evidence_Knife"
   - Source Image: Knife sprite
   - Item Id = "knife"

4. Towel:
   - Name: "Evidence_Towel"
   - Source Image: Towel sprite
   - Item Id = "towel"
```

### **STEP 5: Create Flashback Display**

```
1. Inside Panel:
   - Right-click → UI → Image
   - Name: "Flashback_Image"
   - Size: (300, 300)
   - Position: Center of mirror
   - Active: ✗ (unchecked - shows when item placed)
```

---

## 🎨 MAKING ITEMS DRAGGABLE

### **DraggableItem Script Setup**:

```
For EVERY draggable item (bottles, notes, pages, evidence):

1. Select the item GameObject

2. Add Component → DraggableItem

3. Set in Inspector:
   - Item Id: Unique identifier
     - Bottles: "bottle_1973", "bottle_1974", etc.
     - Notes: "piece1", "piece2", "piece3", "piece4"
     - Pages: "page1", "page2", ... "page8"
     - Evidence: "rope", "pills", "knife", "towel"
   
   - Puzzle Number: Which puzzle (1, 2, 3, or 4)
   
   - Return To Original Position: ✓ (checked)
```

---

## 🔧 SIMPLIFIED MIRROR SCRIPTS

Since you don't have prefabs, let me update the mirror scripts to work with direct UI setup:

### **Update Mirror1_MedicineCabinet**:

The script needs these references:
- `puzzlePanel` - The panel GameObject
- `timerText` - The timer text
- `bottleSlots` - Array of 6 slot GameObjects
- `bottles` - Array of 6 bottle GameObjects (the draggable ones)

You assign these in Inspector by dragging!

---

## 📋 COMPLETE SETUP CHECKLIST

### **For Each Puzzle**:

**Panel Setup**:
- [ ] Panel created (full screen, semi-transparent)
- [ ] Title text added
- [ ] Timer text added
- [ ] Panel starts inactive

**Slots Setup**:
- [ ] Container created with Layout Group
- [ ] Correct number of slots created
- [ ] Slots have proper size and color
- [ ] Slots have labels/numbers

**Items Setup**:
- [ ] All items created as UI Images
- [ ] Items have sprites (or placeholder colors)
- [ ] Items have text labels
- [ ] Items scattered in random positions
- [ ] DraggableItem script added to each
- [ ] Item Id set correctly
- [ ] Puzzle Number set correctly

**References**:
- [ ] Mirror GameObject has puzzle script
- [ ] Panel assigned
- [ ] Timer text assigned
- [ ] Slots array filled
- [ ] Items array filled (if needed)

---

## 🎯 QUICK START (Mirror 1 Example)

### **5-Minute Setup**:

```
1. Create Panel:
   Canvas → Panel → "MedicineC abinet_Panel"
   
2. Add Title:
   Panel → Text → "Medicine Cabinet"
   
3. Add Timer:
   Panel → Text → "1:00"
   
4. Create Slots Container:
   Panel → Panel → "Slots_Container"
   Add Horizontal Layout Group
   
5. Create 6 Slots:
   Slots_Container → Image (x6)
   Names: Slot_1 to Slot_6
   
6. Create 6 Bottles:
   Panel → Image (x6)
   Names: Bottle_1973, Bottle_1974, etc.
   Add DraggableItem to each
   Set Item Id: "bottle_1973", etc.
   Set Puzzle Number: 1
   
7. Assign References:
   Mirror1 GameObject → Inspector
   Drag panel, timer, slots
   
8. DONE!
```

---

## 💡 TIPS

### **No Sprites Yet?**
```
Use colored rectangles:
- Bottles: White rectangles with year text
- Notes: Beige rectangles with text
- Pages: Light brown rectangles with text
- Evidence: Different colors (brown, white, gray, red)
```

### **Testing Without Art**:
```
1. Use UI Images with solid colors
2. Add text labels for identification
3. Test drag-and-drop functionality
4. Replace with real sprites later
```

### **Positioning Items**:
```
Scattered positions for items:
- Top left: (-200, 150)
- Top right: (200, 150)
- Bottom left: (-200, -150)
- Bottom right: (200, -150)
- Center left: (-250, 0)
- Center right: (250, 0)
```

---

## 🐛 TROUBLESHOOTING

### **Items won't drag**:
```
Check:
- DraggableItem script attached
- Canvas has Graphic Raycaster
- EventSystem exists
- Item is child of Canvas
```

### **Items snap back immediately**:
```
Check:
- Slots have correct names
- Item Id matches expected values
- Puzzle Number is correct
```

### **Can't see items**:
```
Check:
- Items are active (checked)
- Items are in front (higher in hierarchy)
- Items have color/sprite assigned
- Panel is active when testing
```

---

## ✅ SUMMARY

### **No Prefabs Needed!**:
1. Create UI directly in Canvas
2. Use Images for items
3. Add DraggableItem script
4. Set Item Id and Puzzle Number
5. Assign references in Inspector

### **Drag & Drop Works By**:
1. DraggableItem detects drag
2. Moves item with finger/mouse
3. Checks what's underneath on release
4. If slot → place item
5. If not → return to original position

### **You Can**:
- Use placeholder colors/shapes
- Add real sprites later
- Test functionality immediately
- No prefabs required!

---

**SIMPLE SETUP, NO PREFABS!** 🎮✨

Just create UI elements directly and add DraggableItem script!

**KAYA MO YAN!** 💪🎨
