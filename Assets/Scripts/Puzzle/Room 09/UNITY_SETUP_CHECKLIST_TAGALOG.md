# ✅ ROOM 09 - COMPLETE UNITY SETUP CHECKLIST (TAGALOG)

## 🎯 LAHAT NG KAILANGAN MONG GAWIN SA UNITY

Ito ang **COMPLETE STEP-BY-STEP CHECKLIST** para sa Room 09. Sundin lang ito at gagana ang lahat!

---

## 📋 PRE-REQUISITES (Dapat Meron Na)

### **Sa Scene**:
- [ ] Canvas GameObject (with Graphic Raycaster component)
- [ ] EventSystem GameObject
- [ ] Player GameObject (with PlayerInteractionController)
- [ ] Joystick (virtual joystick for mobile)
- [ ] Interact Button (bottom right of screen)

### **Check Kung Meron**:
```
1. Hierarchy → Search "Canvas"
   - Canvas component ✓
   - Canvas Scaler ✓
   - Graphic Raycaster ✓ (IMPORTANT!)

2. Hierarchy → Search "EventSystem"
   - EventSystem component ✓
   - Standalone Input Module ✓

3. Hierarchy → Search "Player"
   - PlayerInteractionController ✓
   - Collider2D ✓
   - Tag: "Player" ✓
```

**Kung wala, create muna yan!**

---

## 🪞 PART 1: SETUP MIRRORS (4 Mirrors)

### **For Each Mirror (Repeat 4 times)**:

#### **STEP 1: Create Mirror GameObject**

```
1. Hierarchy → Right-click → Create Empty
2. Name: "Mirror1_MedicineCabinet" (or Mirror2, Mirror3, Mirror4)
3. Position: Kung saan mo gusto sa room
```

#### **STEP 2: Add Sprite**

```
1. Select mirror GameObject
2. Add Component → Sprite Renderer
3. Sprite: Drag your mirror sprite
4. Order in Layer: 1 (para visible)
```

**Kung wala pang sprite**: Use white square muna, palit later

#### **STEP 3: Add Collider (IMPORTANT!)**

```
1. Select mirror GameObject
2. Add Component → Box Collider 2D (or Circle Collider 2D)
3. Settings:
   - Is Trigger: ✓ CHECKED (very important!)
   - Size: Cover mirror sprite + extra space
   - Make it generous para madaling ma-detect
```

#### **STEP 4: Add Room09_Interactable Script**

```
1. Select mirror GameObject
2. Add Component → Room09_Interactable
3. In Inspector:
   - Mirror Number: 1 (or 2, 3, 4 depending on mirror)
```

#### **STEP 5: Add Puzzle Script**

```
For Mirror 1:
- Add Component → Mirror1_MedicineCabinet

For Mirror 2:
- Add Component → Mirror2_BathtubDrain

For Mirror 3:
- Add Component → Mirror3_VanityTerror

For Mirror 4:
- Add Component → Mirror4_EvidenceSequence
```

**Wag mo pa i-assign ang references! Gagawin natin later.**

---

## 🎨 PART 2: CREATE UI PANELS (4 Panels)

### **MIRROR 1: MEDICINE CABINET PANEL**

#### **STEP 1: Create Panel**

```
1. Canvas → Right-click → UI → Panel
2. Name: "MedicineCabinet_Panel"
3. Inspector:
   - Anchor: Stretch (full screen)
   - Color: Black, Alpha: 200
   - Active: ✗ (UNCHECKED - starts hidden)
```

#### **STEP 2: Add Title**

```
1. MedicineCabinet_Panel → Right-click → UI → Text - TextMeshPro
2. Name: "Title_Text"
3. Inspector:
   - Text: "Medicine Cabinet"
   - Font Size: 48
   - Alignment: Center, Top
   - Color: White
   - Position: (0, -50) from top
```

#### **STEP 3: Add Timer**

```
1. MedicineCabinet_Panel → Right-click → UI → Text - TextMeshPro
2. Name: "Timer_Text"
3. Inspector:
   - Text: "1:00"
   - Font Size: 36
   - Alignment: Right, Top
   - Color: White
   - Position: (300, -50) from top right
```

#### **STEP 4: Create Slots Container**

```
1. MedicineCabinet_Panel → Right-click → UI → Panel
2. Name: "Slots_Container"
3. Inspector:
   - Width: 700
   - Height: 200
   - Position: Center (0, 0)
4. Add Component → Horizontal Layout Group
   - Spacing: 20
   - Child Alignment: Middle Center
   - Child Force Expand: Width ✓, Height ✓
```

#### **STEP 5: Create 6 Slots**

```
For each slot (repeat 6 times):

1. Slots_Container → Right-click → UI → Image
2. Name: "Slot_1" (then Slot_2, Slot_3, Slot_4, Slot_5, Slot_6)
3. Inspector:
   - Color: Dark Gray (50, 50, 50, 255)
   - Size: Auto (layout group handles it)

4. Add label:
   - Slot → Right-click → UI → Text
   - Name: "Label"
   - Text: "1" (then "2", "3", etc.)
   - Font Size: 24
   - Position: Top left corner
```

#### **STEP 6: Create 6 Bottles (Draggable Items)**

```
For each bottle (repeat 6 times):

1. MedicineCabinet_Panel → Right-click → UI → Image
2. Name: "Bottle_1973" (then 1974, 1975a, 1975b, 1976a, 1976b)
3. Inspector:
   - Color: White (or assign sprite)
   - Size: Width = 80, Height = 120
   - Position: Scattered (see positions below)
   - Raycast Target: ✓ CHECKED (important!)

4. Add year label:
   - Bottle → Right-click → UI → Text
   - Name: "Year_Label"
   - Text: "1973" (then "1974", etc.)
   - Font Size: 18
   - Position: Bottom center

5. Add Component → DraggableItem
   - Item Id: "bottle_1973" (then "bottle_1974", etc.)
   - Puzzle Number: 1
   - Return To Original Position: ✓
   - Fade While Dragging: ✓
   - Drag Alpha: 0.6
```

**Scattered Positions for Bottles**:
```
Bottle_1973: (-200, 100)
Bottle_1974: (200, 100)
Bottle_1975a: (-200, -100)
Bottle_1975b: (200, -100)
Bottle_1976a: (-250, 0)
Bottle_1976b: (250, 0)
```

#### **STEP 7: Assign References to Mirror1 Script**

```
1. Select Mirror1_MedicineCabinet GameObject (in scene, not panel)
2. Inspector → Mirror1_MedicineCabinet component
3. Assign:
   - Puzzle Panel: Drag "MedicineCabinet_Panel"
   - Timer Text: Drag "Timer_Text"
   - Bottle Slots: (expand to 6)
     - Element 0: Drag "Slot_1"
     - Element 1: Drag "Slot_2"
     - Element 2: Drag "Slot_3"
     - Element 3: Drag "Slot_4"
     - Element 4: Drag "Slot_5"
     - Element 5: Drag "Slot_6"
```

**MIRROR 1 COMPLETE!** ✅

---

### **MIRROR 2: BATHTUB DRAIN PANEL**

#### **STEP 1: Create Panel**

```
1. Canvas → Right-click → UI → Panel
2. Name: "BathtubDrain_Panel"
3. Settings: Same as Mirror 1 (full screen, black, alpha 200, inactive)
```

#### **STEP 2: Add Title & Timer**

```
Same as Mirror 1:
- Title: "Bathtub"
- Timer: "1:00"
```

#### **STEP 3: Create Bathtub Image**

```
1. BathtubDrain_Panel → Right-click → UI → Image
2. Name: "Bathtub_Image"
3. Inspector:
   - Source Image: Your bathtub sprite (or white rectangle)
   - Size: (400, 300)
   - Position: Upper center (0, 100)
```

#### **STEP 4: Create Drain Cover Button**

```
1. Bathtub_Image → Right-click → UI → Button
2. Name: "DrainCover_Button"
3. Inspector:
   - Position: Over drain area (center of bathtub)
   - Size: (80, 80)
   - Text: "Remove Cover"
```

#### **STEP 5: Create 4 Assembly Slots**

```
1. BathtubDrain_Panel → Right-click → UI → Panel
2. Name: "Assembly_Container"
3. Inspector:
   - Width: 600
   - Height: 250
   - Position: Lower center (0, -150)
4. Add Component → Vertical Layout Group
   - Spacing: 10
   - Child Alignment: Upper Center

5. Create 4 slots:
   - Assembly_Container → Right-click → UI → Image (repeat 4 times)
   - Names: "Slot_1", "Slot_2", "Slot_3", "Slot_4"
   - Color: Dark Gray
   - Size: (550, 50) each
```

#### **STEP 6: Create 4 Note Pieces**

```
For each piece (repeat 4 times):

1. BathtubDrain_Panel → Right-click → UI → Image
2. Name: "Note_Piece_1" (then 2, 3, 4)
3. Inspector:
   - Color: Beige (230, 220, 200) or assign sprite
   - Size: (500, 45)
   - Position: Scattered
   - Active: ✗ (UNCHECKED - shown after drain opened)
   - Raycast Target: ✓

4. Add text:
   - Note_Piece → Right-click → UI → Text
   - Text:
     - Piece 1: "Tonight I"
     - Piece 2: "end this child's"
     - Piece 3: "suffering and"
     - Piece 4: "mine forever"
   - Font Size: 16

5. Add Component → DraggableItem
   - Item Id: "piece1" (then "piece2", "piece3", "piece4")
   - Puzzle Number: 2
   - Return To Original Position: ✓
```

#### **STEP 7: Assign References to Mirror2 Script**

```
1. Select Mirror2_BathtubDrain GameObject
2. Inspector → Mirror2_BathtubDrain component
3. Assign:
   - Puzzle Panel: Drag "BathtubDrain_Panel"
   - Timer Text: Drag "Timer_Text"
   - Drain Cover Button: Drag "DrainCover_Button"
   - Assembly Slots: (expand to 4)
     - Element 0-3: Drag Slot_1 to Slot_4
   - Note Pieces: (expand to 4)
     - Element 0-3: Drag Note_Piece_1 to Note_Piece_4
```

**MIRROR 2 COMPLETE!** ✅

---

### **MIRROR 3: VANITY TERROR PANEL**

#### **STEP 1: Create Panel**

```
1. Canvas → Right-click → UI → Panel
2. Name: "VanityTerror_Panel"
3. Settings: Same (full screen, black, alpha 200, inactive)
```

#### **STEP 2: Add Title & Timer**

```
- Title: "Mother's Diary"
- Timer: "1:30" (90 seconds)
```

#### **STEP 3: Create 8 Numbered Slots (Grid)**

```
1. VanityTerror_Panel → Right-click → UI → Panel
2. Name: "Slots_Container"
3. Inspector:
   - Width: 900
   - Height: 600
   - Position: Center (0, 0)
4. Add Component → Grid Layout Group
   - Cell Size: (200, 140)
   - Spacing: (10, 10)
   - Constraint: Fixed Column Count = 4
   - Child Alignment: Middle Center

5. Create 8 slots:
   - Slots_Container → Right-click → UI → Image (repeat 8 times)
   - Names: "Slot_1" to "Slot_8"
   - Color: Dark Gray
   - Each has number label: "1", "2", "3"... "8"
```

#### **STEP 4: Create 8 Diary Pages**

```
For each page (repeat 8 times):

1. VanityTerror_Panel → Right-click → UI → Image
2. Name: "DiaryPage_1" (then 2-8)
3. Inspector:
   - Color: Old paper (230, 220, 200)
   - Size: (180, 120)
   - Position: Random scattered
   - Raycast Target: ✓

4. Add text:
   - DiaryPage → Right-click → UI → Text
   - Font Size: 10-12
   - Text: [Diary content - see below]
   - Color: Dark brown

5. Add Component → DraggableItem
   - Item Id: "page1" (then "page2" to "page8")
   - Puzzle Number: 3
   - Return To Original Position: ✓
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

#### **STEP 5: Assign References to Mirror3 Script**

```
1. Select Mirror3_VanityTerror GameObject
2. Inspector → Mirror3_VanityTerror component
3. Assign:
   - Puzzle Panel: Drag "VanityTerror_Panel"
   - Timer Text: Drag "Timer_Text"
   - Numbered Slots: (expand to 8)
     - Element 0-7: Drag Slot_1 to Slot_8
   - Diary Pages: (expand to 8)
     - Element 0-7: Drag DiaryPage_1 to DiaryPage_8
```

**MIRROR 3 COMPLETE!** ✅

---

### **MIRROR 4: EVIDENCE SEQUENCE PANEL**

#### **STEP 1: Create Panel**

```
1. Canvas → Right-click → UI → Panel
2. Name: "EvidenceSequence_Panel"
3. Settings: Same (full screen, black, alpha 200, inactive)
```

#### **STEP 2: Add Title & Timer**

```
- Title: "The Plan"
- Timer: "1:00"
```

#### **STEP 3: Create Mirror Image**

```
1. EvidenceSequence_Panel → Right-click → UI → Image
2. Name: "Mirror_Image"
3. Inspector:
   - Source Image: Large mirror sprite
   - Size: (400, 500)
   - Position: Upper center (0, 100)
```

#### **STEP 4: Create 4 Picture Frames**

```
1. EvidenceSequence_Panel → Right-click → UI → Panel
2. Name: "Frames_Container"
3. Inspector:
   - Width: 600
   - Height: 150
   - Position: Below mirror (0, -200)
4. Add Component → Horizontal Layout Group
   - Spacing: 15
   - Child Alignment: Middle Center

5. Create 4 frames:
   - Frames_Container → Right-click → UI → Image (repeat 4 times)
   - Names: "Frame_1" to "Frame_4"
   - Source Image: Empty frame sprite (or white border)
   - Size: (120, 120)
   - Each has number label: "1", "2", "3", "4"
```

#### **STEP 5: Create 4 Evidence Items**

```
For each item:

1. Rope:
   - EvidenceSequence_Panel → Right-click → UI → Image
   - Name: "Evidence_Rope"
   - Source Image: Rope sprite (or brown rectangle)
   - Size: (100, 100)
   - Position: Scattered
   - Raycast Target: ✓
   - Add DraggableItem: Item Id = "rope", Puzzle Number = 4

2. Pills:
   - Name: "Evidence_Pills"
   - Source Image: Pills sprite (or white circle)
   - Item Id = "pills"

3. Knife:
   - Name: "Evidence_Knife"
   - Source Image: Knife sprite (or gray triangle)
   - Item Id = "knife"

4. Towel:
   - Name: "Evidence_Towel"
   - Source Image: Towel sprite (or red rectangle)
   - Item Id = "towel"
```

#### **STEP 6: Create Flashback Display**

```
1. EvidenceSequence_Panel → Right-click → UI → Image
2. Name: "Flashback_Image"
3. Inspector:
   - Size: (300, 300)
   - Position: Center of mirror (0, 100)
   - Active: ✗ (UNCHECKED - shows when item placed)
```

#### **STEP 7: Assign References to Mirror4 Script**

```
1. Select Mirror4_EvidenceSequence GameObject
2. Inspector → Mirror4_EvidenceSequence component
3. Assign:
   - Puzzle Panel: Drag "EvidenceSequence_Panel"
   - Timer Text: Drag "Timer_Text"
   - Picture Frames: (expand to 4)
     - Element 0-3: Drag Frame_1 to Frame_4
   - Evidence Items: (expand to 4)
     - Element 0: Drag "Evidence_Rope"
     - Element 1: Drag "Evidence_Pills"
     - Element 2: Drag "Evidence_Knife"
     - Element 3: Drag "Evidence_Towel"
   - Flashback Image: Drag "Flashback_Image"
   - Flashback Sprites: (assign your 4 flashback images)
```

**MIRROR 4 COMPLETE!** ✅

---

## 🎮 PART 3: FINAL CHECKS

### **Check Each Mirror**:

```
For Mirror 1, 2, 3, and 4:

1. Mirror GameObject:
   - [ ] Has Sprite Renderer
   - [ ] Has Collider2D (Is Trigger: ✓)
   - [ ] Has Room09_Interactable (Mirror Number set)
   - [ ] Has puzzle script (Mirror1, Mirror2, Mirror3, or Mirror4)
   - [ ] All references assigned in Inspector

2. Panel:
   - [ ] Exists in Canvas
   - [ ] Has title and timer
   - [ ] Has slots/frames
   - [ ] Has draggable items
   - [ ] Starts inactive (Active: ✗)

3. Draggable Items:
   - [ ] All have DraggableItem script
   - [ ] Item Id is set correctly
   - [ ] Puzzle Number is set correctly
   - [ ] Raycast Target is checked
   - [ ] Return To Original Position is checked
```

### **Check Scene**:

```
- [ ] Canvas has Graphic Raycaster
- [ ] EventSystem exists
- [ ] Player has PlayerInteractionController
- [ ] Interact button exists and works
- [ ] All 4 mirrors in scene
- [ ] All 4 panels in Canvas
```

---

## 🧪 PART 4: TESTING

### **Test Each Mirror**:

```
1. Play scene
2. Walk player near mirror
3. Tap Interact button (or tap mirror directly)
4. Panel should open
5. Try dragging items
6. Items should follow finger/mouse
7. Drop on slot - should snap
8. Drop on empty - should return
9. Complete puzzle - should show success
10. Panel should close
```

### **Test All 4 Mirrors**:

```
- [ ] Mirror 1 opens and works
- [ ] Mirror 2 opens and works
- [ ] Mirror 3 opens and works
- [ ] Mirror 4 opens and works
- [ ] All drag-and-drop works
- [ ] Timers count down
- [ ] Success dialogues show
- [ ] Panels close after success
```

---

## 🐛 TROUBLESHOOTING

### **Problem: Can't interact with mirror**

**Check**:
1. Mirror has Collider2D with Is Trigger ✓
2. Mirror has Room09_Interactable script
3. Mirror Number is set
4. Player has PlayerInteractionController
5. Interact button exists

**Fix**: Add missing components

---

### **Problem: Can't drag items**

**Check**:
1. Item has DraggableItem script
2. Item has Image component with Raycast Target ✓
3. Canvas has Graphic Raycaster
4. EventSystem exists
5. Item Id and Puzzle Number are set

**Fix**: Add missing components, check settings

---

### **Problem: Items snap back immediately**

**Check**:
1. Slot names contain "Slot" or "Frame"
2. Item Id matches expected values
3. Puzzle Number is correct
4. Slots are in correct array order

**Fix**: Rename slots, check Item Ids

---

### **Problem: Panel doesn't open**

**Check**:
1. Panel is assigned in mirror script
2. Panel exists in Canvas
3. StartPuzzle() method is being called
4. No errors in Console

**Fix**: Assign panel reference, check Console

---

### **Problem: Timer doesn't show**

**Check**:
1. Timer Text is assigned in mirror script
2. Timer Text exists in panel
3. Timer Text is TextMeshProUGUI (not regular Text)

**Fix**: Assign timer reference, use TextMeshPro

---

## ✅ FINAL CHECKLIST

### **Before Testing**:

- [ ] All 4 mirrors created in scene
- [ ] All 4 mirrors have colliders (Is Trigger: ✓)
- [ ] All 4 mirrors have Room09_Interactable
- [ ] All 4 mirrors have puzzle scripts
- [ ] All 4 panels created in Canvas
- [ ] All panels have title and timer
- [ ] All panels have slots/frames
- [ ] All draggable items created
- [ ] All items have DraggableItem script
- [ ] All Item Ids set correctly
- [ ] All Puzzle Numbers set correctly
- [ ] All references assigned in Inspector
- [ ] Canvas has Graphic Raycaster
- [ ] EventSystem exists
- [ ] Player has PlayerInteractionController

### **After Testing**:

- [ ] All mirrors can be interacted with
- [ ] All panels open correctly
- [ ] All items can be dragged
- [ ] Items snap to slots correctly
- [ ] Items return if dropped on empty space
- [ ] Puzzles detect completion
- [ ] Success dialogues show
- [ ] Panels close after success
- [ ] No errors in Console

---

## 🎉 SUMMARY

### **What You Created**:

1. **4 Mirror GameObjects** (in scene)
   - With colliders, scripts, and references

2. **4 UI Panels** (in Canvas)
   - With titles, timers, slots, and items

3. **Draggable Items** (bottles, notes, pages, evidence)
   - With DraggableItem scripts and settings

4. **Complete Interaction System**
   - Walk near mirror → Interact button → Panel opens → Drag items → Complete puzzle

### **Total Items Created**:

- 4 mirrors (scene objects)
- 4 panels (UI)
- 6 bottles (Mirror 1)
- 4 note pieces (Mirror 2)
- 8 diary pages (Mirror 3)
- 4 evidence items (Mirror 4)
- **Total: 26 draggable items!**

### **How It Works**:

```
Player walks near mirror
    ↓
PlayerInteractionController detects Room09_Interactable
    ↓
Interact button activates
    ↓
Player taps button
    ↓
Room09_Interactable.OnInteract() called
    ↓
Mirror script StartPuzzle() called
    ↓
Panel opens, items appear
    ↓
Player drags items to slots
    ↓
DraggableItem detects slot
    ↓
Notifies mirror script
    ↓
Mirror script checks solution
    ↓
Success → Dialogue → Panel closes
```

---

**COMPLETE SETUP GUIDE!** ✅🎮

Follow this checklist and everything will work!

**KAYA MO YAN!** 💪✨🎨

**NO PREFABS NEEDED!** Just create UI directly in Unity!
