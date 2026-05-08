# Cabinet Panel - Simple Setup Guide

## 🎯 Ano Ito?

Panel na lalabas pag i-interact ang Small Cabinet. May cup image na pwedeng i-click para makuha.

---

## 🎨 Step 1: Create Panel

### 1.1 Create Main Panel
```
Hierarchy:
Canvas
└── Cabinet_Panel (Panel)
```

**How:**
1. Right-click Canvas
2. UI → Panel
3. Rename to "Cabinet_Panel"
4. Set Anchor: Stretch (full screen)
5. Color: Black (0, 0, 0, 200) - semi-transparent

---

### 1.2 Add Cabinet Image
```
Cabinet_Panel
└── Cabinet_Image (Image)
```

**How:**
1. Right-click Cabinet_Panel
2. UI → Image
3. Rename to "Cabinet_Image"
4. Assign sprite: Open cabinet sprite
5. Size: 600x800
6. Position: Center

---

### 1.3 Add Cup Image (CLICKABLE!)
```
Cabinet_Panel
└── Cabinet_Image
    └── Item_Image (Image)
```

**How:**
1. Right-click Cabinet_Image
2. UI → Image
3. Rename to "Item_Image"
4. Size: 200x200
5. Position: Inside cabinet (where cup should be)
6. **✓ Raycast Target: CHECKED** ← IMPORTANT!

---

### 1.4 Add Item Name Text
```
Cabinet_Panel
└── Cabinet_Image
    └── ItemName_Text (Text)
```

**How:**
1. Right-click Cabinet_Image
2. UI → Text
3. Rename to "ItemName_Text"
4. Position: Below Item_Image
5. Font Size: 32
6. Alignment: Center
7. Color: White

---

### 1.5 Add Item Description Text
```
Cabinet_Panel
└── Cabinet_Image
    └── ItemDescription_Text (Text)
```

**How:**
1. Right-click Cabinet_Image
2. UI → Text
3. Rename to "ItemDescription_Text"
4. Position: Below ItemName_Text
5. Font Size: 20
6. Alignment: Center
7. Color: Light Gray

---

### 1.6 Add Close Button (Optional)
```
Cabinet_Panel
└── Close_Button (Button)
```

**How:**
1. Right-click Cabinet_Panel
2. UI → Button
3. Rename to "Close_Button"
4. Position: Top-right corner
5. Size: 60x60
6. Text: "X"

---

## 🔧 Step 2: Add Script

### 2.1 Add CabinetItemPanel Script
```
1. Select Cabinet_Panel
2. Add Component
3. Search: CabinetItemPanel
4. Click to add
```

---

### 2.2 Assign References

```
Select Cabinet_Panel
Inspector → CabinetItemPanel:

UI References:
  Cabinet Panel: [Drag Cabinet_Panel here]
  Close Button: [Drag Close_Button here]

Item Display:
  Item Image: [Drag Item_Image here] ← IMPORTANT!
  Item Name Text: [Drag ItemName_Text here]
  Item Description Text: [Drag ItemDescription_Text here]

Item to Give:
  Item Id: emily_cup

Visual Feedback:
  Normal Color: White (255, 255, 255, 255)
  Hover Color: Yellow (255, 255, 0, 255)

Audio:
  Open Sound: [Assign cabinet open sound]
  Take Sound: [Assign item pickup sound]
```

---

## 🔗 Step 3: Connect to Manager

### 3.1 Assign to Room07UIManager
```
1. Select Room07_Manager (in Hierarchy)
2. Inspector → Room07UIManager
3. Find "Cabinet Panel" field
4. Drag Cabinet_Panel to this field
```

---

## 🎮 Step 4: Setup Cabinet GameObject

### 4.1 Create Cabinet GameObject
```
1. Create empty GameObject "SmallCabinet"
2. Add Collider (for interaction)
3. Add Component → Room07_Interactable
```

### 4.2 Set Inspector Values
```
Select SmallCabinet
Inspector → Room07_Interactable:

My Type: Cabinet_Cup ← IMPORTANT!
UI Manager: [Drag Room07_Manager here]
Required Item ID: (empty)
```

---

## ✅ Complete Structure

```
Canvas
└── Cabinet_Panel (Panel)
    ├── Cabinet_Image (Image)
    │   ├── Item_Image (Image) ← Click to take!
    │   ├── ItemName_Text (Text)
    │   └── ItemDescription_Text (Text)
    └── Close_Button (Button)

Hierarchy (Scene):
└── SmallCabinet (GameObject)
    └── Room07_Interactable
        └── My Type: Cabinet_Cup
```

---

## 🧪 Testing

### Test 1: Open Panel
```
1. Play Mode
2. Open curtains first
3. Interact with SmallCabinet
4. Cabinet_Panel should open ✓
5. Should show cup image ✓
```

### Test 2: Hover Effect
```
1. Move mouse over cup
2. Cup turns yellow ✓
3. Move mouse away
4. Cup returns white ✓
```

### Test 3: Click to Take
```
1. Click cup image
2. Panel closes ✓
3. Dialogue shows ✓
4. Notification shows ✓
5. Cup added to inventory ✓
```

---

## 🐛 Common Issues

### Issue 1: Can't Click Cup
```
Problem: Click cup but nothing happens

Fix:
1. Select Item_Image
2. Inspector → Image component
3. ✓ Raycast Target: MUST BE CHECKED
```

### Issue 2: Panel Doesn't Open
```
Problem: Interact with cabinet, nothing happens

Fix:
1. Select SmallCabinet GameObject
2. Check My Type: Should be Cabinet_Cup
3. Check UI Manager: Should be assigned
4. Check curtains opened first
```

### Issue 3: No Hover Effect
```
Problem: Cup doesn't turn yellow

Fix:
1. Check Raycast Target is checked
2. Check Normal Color and Hover Color are different
3. Check Item_Image is assigned in script
```

---

## 📊 Visual Layout

```
┌─────────────────────────┐
│  Cabinet Panel      [X] │
├─────────────────────────┤
│                         │
│   ┌─────────────┐       │
│   │  Cabinet    │       │
│   │  (Open)     │       │
│   │             │       │
│   │   ┌─────┐   │       │
│   │   │ 🍵  │ ← Click!  │
│   │   └─────┘   │       │
│   │             │       │
│   │ Emily's Cup │       │
│   │             │       │
│   │ A small     │       │
│   │ porcelain   │       │
│   │ cup...      │       │
│   │             │       │
│   └─────────────┘       │
│                         │
└─────────────────────────┘
```

---

## ✅ Quick Checklist

- [ ] Cabinet_Panel created
- [ ] Cabinet_Image added
- [ ] Item_Image added (cup sprite)
- [ ] Item_Image → Raycast Target CHECKED
- [ ] ItemName_Text added
- [ ] ItemDescription_Text added
- [ ] Close_Button added
- [ ] CabinetItemPanel script added to panel
- [ ] All references assigned in Inspector
- [ ] Item Id set to "emily_cup"
- [ ] Panel assigned in Room07UIManager
- [ ] SmallCabinet GameObject created
- [ ] SmallCabinet → My Type: Cabinet_Cup
- [ ] SmallCabinet → UI Manager assigned
- [ ] Tested opening panel
- [ ] Tested clicking cup
- [ ] Tested notification

---

## 🎯 Key Points

1. **Item_Image must have Raycast Target CHECKED** - Para ma-click!
2. **My Type must be Cabinet_Cup** - Para tama ang panel na bubuksan
3. **Curtains must be opened first** - Bago ma-access ang cabinet
4. **Click cup directly** - Walang button, click lang ang cup!

---

**Follow these steps exactly!** 🎮✨

**Most important: Raycast Target CHECKED!** ✅
