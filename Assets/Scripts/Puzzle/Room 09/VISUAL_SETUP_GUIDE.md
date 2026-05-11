# 📸 ROOM 09 - VISUAL STEP-BY-STEP GUIDE

## 🎯 PARA SA MGA VISUAL LEARNERS

Ito ang step-by-step guide with detailed descriptions para madali mong makita kung ano ang gagawin.

---

## 🎮 MIRROR 1: MEDICINE CABINET - VISUAL GUIDE

### **STEP 1: Create Panel**

**What you'll see**:
```
Hierarchy:
Canvas
└── MedicineC abinet_Panel (Panel)
    └── (empty for now)

Inspector (MedicineC abinet_Panel):
- Rect Transform: Anchor = Stretch
- Image: Color = Black, Alpha = 200
- Active: ✗ (unchecked)
```

**How it looks**: Full-screen dark semi-transparent panel

---

### **STEP 2: Add Title and Timer**

**What you'll see**:
```
Hierarchy:
Canvas
└── MedicineC abinet_Panel
    ├── Title_Text (TextMeshProUGUI)
    └── Timer_Text (TextMeshProUGUI)

Inspector (Title_Text):
- Text: "Medicine Cabinet"
- Font Size: 48
- Alignment: Center, Top
- Color: White

Inspector (Timer_Text):
- Text: "1:00"
- Font Size: 36
- Alignment: Right, Top
- Color: White
```

**How it looks**: 
- "Medicine Cabinet" at top center
- "1:00" at top right

---

### **STEP 3: Create Slots Container**

**What you'll see**:
```
Hierarchy:
Canvas
└── MedicineC abinet_Panel
    ├── Title_Text
    ├── Timer_Text
    └── Slots_Container (Panel)
        └── (empty - will add slots here)

Inspector (Slots_Container):
- Rect Transform: 
  - Width: 700
  - Height: 200
  - Position: Center of panel
- Horizontal Layout Group:
  - Spacing: 20
  - Child Alignment: Middle Center
  - Child Force Expand: Width ✓, Height ✓
```

**How it looks**: Invisible container in center (will hold 6 slots)

---

### **STEP 4: Create 6 Slots**

**What you'll see**:
```
Hierarchy:
Canvas
└── MedicineC abinet_Panel
    └── Slots_Container
        ├── Slot_1 (Image)
        ├── Slot_2 (Image)
        ├── Slot_3 (Image)
        ├── Slot_4 (Image)
        ├── Slot_5 (Image)
        └── Slot_6 (Image)

Inspector (each Slot):
- Image: Color = Dark Gray (50, 50, 50, 255)
- Size: Auto (handled by layout group)
```

**How it looks**: 6 dark gray rectangles in a horizontal row

---

### **STEP 5: Add Labels to Slots**

**What you'll see**:
```
Hierarchy:
Slot_1
└── Label (Text)

Inspector (Label):
- Text: "1" (then "2", "3", etc. for other slots)
- Font Size: 24
- Alignment: Top Left
- Color: White
```

**How it looks**: Each slot has a number in top-left corner

---

### **STEP 6: Create 6 Bottles**

**What you'll see**:
```
Hierarchy:
Canvas
└── MedicineC abinet_Panel
    ├── Slots_Container (with 6 slots)
    ├── Bottle_1973 (Image)
    ├── Bottle_1974 (Image)
    ├── Bottle_1975a (Image)
    ├── Bottle_1975b (Image)
    ├── Bottle_1976a (Image)
    └── Bottle_1976b (Image)

Inspector (Bottle_1973):
- Image: 
  - Color: White (or your bottle sprite)
  - Size: Width = 80, Height = 120
- Rect Transform:
  - Position: (-200, 100) [scattered]
- DraggableItem:
  - Item Id: "bottle_1973"
  - Puzzle Number: 1
  - Return To Original Position: ✓
```

**How it looks**: 6 white rectangles (or bottle sprites) scattered around panel

---

### **STEP 7: Add Year Labels to Bottles**

**What you'll see**:
```
Hierarchy:
Bottle_1973
└── Year_Label (Text)

Inspector (Year_Label):
- Text: "1973"
- Font Size: 18
- Alignment: Bottom Center
- Color: Black (or white if bottle is dark)
```

**How it looks**: Each bottle has year text at bottom

---

### **FINAL RESULT - Medicine Cabinet Panel**:

```
Visual Layout:

┌─────────────────────────────────────────────┐
│  Medicine Cabinet              1:00         │ ← Title & Timer
│                                             │
│  [1]  [2]  [3]  [4]  [5]  [6]              │ ← 6 Empty Slots
│                                             │
│     1973      1975a                         │
│                                             │
│  1974    1975b    1976a    1976b           │ ← 6 Bottles (scattered)
│                                             │
└─────────────────────────────────────────────┘
```

---

## 🎨 MAKING BOTTLES DRAGGABLE

### **Visual Checklist for Each Bottle**:

**In Hierarchy**:
```
✓ Bottle is child of Panel (NOT Slots_Container)
✓ Bottle has Image component
✓ Bottle has DraggableItem component
✓ Bottle has Text child for year label
```

**In Inspector (DraggableItem)**:
```
✓ Item Id is set (e.g., "bottle_1973")
✓ Puzzle Number is 1
✓ Return To Original Position is checked
```

**In Inspector (Image)**:
```
✓ Color is set (white or sprite assigned)
✓ Size is 80x120
✓ Raycast Target is checked (important!)
```

---

## 🧪 TESTING DRAG & DROP

### **What Should Happen**:

**1. Start Drag**:
```
- Tap/click bottle
- Bottle becomes semi-transparent (60% alpha)
- Bottle moves with finger/mouse
- Console shows: "Started dragging: bottle_1973"
```

**2. While Dragging**:
```
- Bottle follows finger/mouse smoothly
- Bottle is on top of everything
- Other UI elements are still visible
```

**3. Drop on Slot**:
```
- Release finger/mouse over a slot
- Bottle snaps to center of slot
- Bottle becomes fully opaque again
- Console shows: "bottle_1973 placed in Slot_1"
```

**4. Drop on Empty Space**:
```
- Release finger/mouse on empty area
- Bottle returns to original position
- Bottle becomes fully opaque again
- Console shows: "bottle_1973 returned to original position"
```

---

## 🐛 VISUAL TROUBLESHOOTING

### **Problem: Can't drag bottle**

**Check in Inspector**:
```
Bottle GameObject:
✓ Image component exists
✓ Image → Raycast Target is CHECKED
✓ DraggableItem component exists

Canvas:
✓ Graphic Raycaster component exists

Scene:
✓ EventSystem GameObject exists
```

**Visual Check**:
- Can you see the bottle in Game view?
- Is the bottle in front of other elements?
- Is the panel active when testing?

---

### **Problem: Bottle snaps back immediately**

**Check in Inspector**:
```
DraggableItem:
✓ Item Id is set correctly
✓ Puzzle Number is 1
✓ Return To Original Position is checked

Slot GameObject:
✓ Name contains "Slot" (e.g., "Slot_1")
✓ OR has tag "PuzzleSlot"
```

**Visual Check**:
- Are slots visible in Game view?
- Are slots in the right position?
- Are slots the right size?

---

### **Problem: Can't see bottles**

**Check in Hierarchy**:
```
✓ Bottles are children of Panel
✓ Bottles are BELOW Slots_Container (rendered on top)
✓ Bottles are active (checked)
```

**Check in Inspector**:
```
Bottle Image:
✓ Color alpha is 255 (fully opaque)
✓ Sprite is assigned OR color is set
✓ Size is not 0x0
```

**Visual Check**:
- Is the panel active?
- Are you in Game view (not Scene view)?
- Is the camera rendering the Canvas?

---

## 📋 COMPLETE VISUAL CHECKLIST

### **Hierarchy Structure**:
```
Canvas
├── MedicineC abinet_Panel ✓
│   ├── Title_Text ✓
│   ├── Timer_Text ✓
│   ├── Slots_Container ✓
│   │   ├── Slot_1 ✓
│   │   │   └── Label ✓
│   │   ├── Slot_2 ✓
│   │   ├── Slot_3 ✓
│   │   ├── Slot_4 ✓
│   │   ├── Slot_5 ✓
│   │   └── Slot_6 ✓
│   ├── Bottle_1973 ✓
│   │   └── Year_Label ✓
│   ├── Bottle_1974 ✓
│   ├── Bottle_1975a ✓
│   ├── Bottle_1975b ✓
│   ├── Bottle_1976a ✓
│   └── Bottle_1976b ✓
└── (other panels)
```

### **Component Checklist**:
```
Canvas:
✓ Canvas component
✓ Canvas Scaler component
✓ Graphic Raycaster component

EventSystem:
✓ EventSystem GameObject exists in scene
✓ Standalone Input Module component

Each Bottle:
✓ Rect Transform
✓ Image (with Raycast Target checked)
✓ DraggableItem (with Item Id and Puzzle Number set)
✓ Text child for label

Each Slot:
✓ Rect Transform
✓ Image
✓ Name contains "Slot"
```

---

## 🎯 QUICK VISUAL TEST

### **5-Second Test**:

1. **Play scene**
2. **Look for**:
   - Panel visible? ✓
   - Title visible? ✓
   - Timer visible? ✓
   - 6 slots visible? ✓
   - 6 bottles visible? ✓
3. **Try drag**:
   - Tap bottle → moves? ✓
   - Drop on slot → snaps? ✓
   - Drop on empty → returns? ✓

**If all ✓ = Working!**

---

## 💡 VISUAL TIPS

### **Color Coding (No Sprites)**:
```
Slots: Dark Gray (50, 50, 50)
Bottles: White (255, 255, 255)
Panel: Black (0, 0, 0, 200)
Text: White (255, 255, 255)
```

### **Size Reference**:
```
Panel: Full screen (stretch)
Slots: ~100x150 each
Bottles: 80x120 each
Text: 18-48 font size
```

### **Position Reference**:
```
Title: Top center (0, -50)
Timer: Top right (300, -50)
Slots: Center (0, 0)
Bottles: Scattered around center
```

---

## ✅ SUMMARY

### **What You Should See**:
1. Dark semi-transparent panel
2. Title at top
3. Timer at top right
4. 6 gray slots in a row
5. 6 white bottles scattered
6. Year labels on bottles

### **What Should Work**:
1. Tap bottle → becomes transparent
2. Drag bottle → follows finger
3. Drop on slot → snaps to center
4. Drop on empty → returns to start

### **If Not Working**:
1. Check Hierarchy structure
2. Check components exist
3. Check settings in Inspector
4. Check Console for errors

---

**VISUAL GUIDE COMPLETE!** 📸✨

Follow the visual descriptions and your drag-and-drop will work!

**KAYA MO YAN!** 💪🎨
