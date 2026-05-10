# Cabinet Panel Setup Guide

## 🎯 Overview

Simple panel that displays Emily's Cup - **click the cup image directly to take it**. No "Take" button needed!

### Features:
- Click cup image to obtain item
- Hover effect (cup turns yellow)
- Automatic dialogue → notification sequence
- Gray out when already taken

---

## 📋 What You Need

### UI Elements:
1. **Cabinet_Panel** - Main container
2. **Item_Image** - Shows cup sprite (CLICKABLE!)
3. **ItemName_Text** - Shows "Emily's Cup"
4. **ItemDescription_Text** - Shows description
5. **Close_Button** - Button to close panel (optional)

**Note:** No "Take" button needed - just click the cup!

---

## 🎨 Step-by-Step Setup

### Step 1: Create Cabinet Panel

```
Canvas
└── Cabinet_Panel (GameObject)
    ├── Background (Image) - Dark semi-transparent
    ├── Cabinet_Image (Image) - Cabinet visual
    ├── Item_Container (GameObject)
    │   ├── Item_Image (Image) - Cup sprite
    │   ├── ItemName_Text (Text) - "Emily's Cup"
    │   └── ItemDescription_Text (Text) - Description
    ├── Take_Button (Button) - "Take" button
    └── Close_Button (Button) - "X" button
```

### Step 2: Setup Background

```
1. Right-click Canvas → UI → Panel
2. Rename to "Cabinet_Panel"
3. Set Anchor: Stretch (full screen)
4. Set Color: Black (0, 0, 0, 200) - semi-transparent
```

### Step 3: Add Cabinet Visual

```
1. Right-click Cabinet_Panel → UI → Image
2. Rename to "Cabinet_Image"
3. Assign sprite: Open cabinet sprite
4. Set size: 600x800
5. Center on screen
```

### Step 4: Create Item Container

```
1. Right-click Cabinet_Image → Create Empty
2. Rename to "Item_Container"
3. Position: Center of cabinet
```

### Step 5: Add Item Image

```
1. Right-click Item_Container → UI → Image
2. Rename to "Item_Image"
3. Set size: 200x200
4. Position: Top of container
5. Sprite will be assigned by script from database
```

### Step 6: Add Item Name Text

```
1. Right-click Item_Container → UI → Text
2. Rename to "ItemName_Text"
3. Position: Below Item_Image
4. Font Size: 32
5. Alignment: Center
6. Color: White
7. Text will be set by script
```

### Step 7: Add Item Description Text

```
1. Right-click Item_Container → UI → Text
2. Rename to "ItemDescription_Text"
3. Position: Below ItemName_Text
4. Font Size: 20
5. Alignment: Center
6. Color: Light Gray
7. Best Fit: ✓ (optional)
8. Text will be set by script
```

### Step 8: Add Take Button

```
1. Right-click Cabinet_Panel → UI → Button
2. Rename to "Take_Button"
3. Position: Bottom center
4. Set size: 200x60
5. Set Text: "Take"
6. Font Size: 28
```

### Step 9: Add Close Button (Optional)

```
1. Right-click Cabinet_Panel → UI → Button
2. Rename to "Close_Button"
3. Position: Top-right corner
4. Set size: 60x60
5. Set Text: "X"
6. Font Size: 32
```

---

## 🔧 Script Setup

### Step 1: Add Script to Panel

```
1. Select Cabinet_Panel
2. Add Component → CabinetItemPanel
```

### Step 2: Assign References

```
CabinetItemPanel:
  UI References:
    Cabinet Panel: [Drag Cabinet_Panel here]
    Take Button: [Drag Take_Button here]
    Close Button: [Drag Close_Button here]
  
  Item Display:
    Item Image: [Drag Item_Image here]
    Item Name Text: [Drag ItemName_Text here]
    Item Description Text: [Drag ItemDescription_Text here]
  
  Item to Give:
    Item Id: emily_cup
  
  Audio:
    Open Sound: [Assign cabinet open sound]
    Take Sound: [Assign item pickup sound]
```

---

## 🎮 How It Works

### Flow:

1. **Player Opens Cabinet**
   - Curtains must be opened first
   - Cabinet panel opens
   - Shows Emily's Cup with description

2. **Player Clicks "Take"**
   - Panel closes
   - Dialogue shows: "Found Emily's Cup..."
   - Wait for dialogue to finish
   - Item notification shows
   - Player taps to continue
   - Cup added to inventory

3. **If Already Taken**
   - Button shows "Already Taken"
   - Button is disabled
   - Can still close panel

---

## 🎨 Visual Layout

```
┌─────────────────────────┐
│  Cabinet Panel      [X] │
├─────────────────────────┤
│                         │
│   ┌─────────────┐       │
│   │             │       │
│   │  Cabinet    │       │
│   │  (Open)     │       │
│   │             │       │
│   │   ┌─────┐   │       │
│   │   │ Cup │   │       │
│   │   │ 🍵  │   │       │
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
│      [Take Item]        │
│                         │
└─────────────────────────┘
```

---

## 📊 Item Data (From Database)

The script automatically loads item data from ItemDatabase:

```
Item ID: emily_cup
Item Name: Emily's Cup
Description: A small porcelain cup with delicate floral patterns...
Icon: Cup sprite (assign in database)
Is Key Item: true
Required For Puzzle: tea_party
```

---

## 🧪 Testing

### Test 1: Open Panel
```
1. Play Mode
2. Open curtains first
3. Interact with cabinet
4. Cabinet panel should open ✓
5. Should show cup image, name, description ✓
```

### Test 2: Take Item
```
1. Click "Take" button
2. Panel closes ✓
3. Dialogue shows ✓
4. Wait for dialogue
5. Notification shows ✓
6. Tap to continue
7. Cup added to inventory ✓
```

### Test 3: Already Taken
```
1. Take cup once
2. Open cabinet again
3. Button should say "Already Taken" ✓
4. Button should be disabled ✓
```

### Test 4: Close Without Taking
```
1. Open cabinet panel
2. Click "X" button
3. Panel closes ✓
4. Cup NOT added to inventory ✓
5. Can open again later ✓
```

---

## 🐛 Troubleshooting

### Issue 1: Item Data Not Showing
```
Problem: Name/description blank, no image

Check:
1. ItemDatabase assigned in InventoryManager?
2. Item "emily_cup" exists in database?
3. Item has name, description, icon assigned?
```

### Issue 2: Button Doesn't Work
```
Problem: Click "Take" but nothing happens

Check:
1. Button has onClick listener (added by script)?
2. Script Start() method ran?
3. Check Console for errors
```

### Issue 3: No Notification
```
Problem: Item added but no notification shows

Check:
1. ItemNotificationUI exists in scene?
2. AddItemWithNotification() method used?
3. Check InventoryManager setup
```

### Issue 4: Can Take Multiple Times
```
Problem: Can take cup multiple times

Check:
1. itemTaken flag set to true?
2. flow.hasEmilyCup set to true?
3. Check OnEnable() checks HasItem()
```

---

## ✅ Quick Checklist

- [ ] Cabinet_Panel created (full screen)
- [ ] Cabinet_Image added (visual)
- [ ] Item_Image added (cup sprite)
- [ ] ItemName_Text added
- [ ] ItemDescription_Text added
- [ ] Take_Button added
- [ ] Close_Button added (optional)
- [ ] CabinetItemPanel script added
- [ ] All references assigned
- [ ] Item ID set to "emily_cup"
- [ ] Tested opening panel
- [ ] Tested taking item
- [ ] Tested notification
- [ ] Tested "already taken" state

---

## 📱 Mobile Considerations

### Button Size:
```
Desktop: 200x60
Mobile: 250x80 (larger for touch)
```

### Text Size:
```
Item Name: 32-40
Description: 20-24
Button Text: 28-32
```

### Touch Area:
```
Make sure buttons have enough padding
Minimum touch target: 60x60 pixels
```

---

## 🎉 Summary

### What You Need:
1. Panel with cabinet visual
2. Item display (image, name, description)
3. Take button
4. Close button (optional)
5. Script with references

### How It Works:
1. Panel opens showing cup
2. Player clicks "Take"
3. Dialogue shows
4. Notification shows
5. Cup added to inventory

### Key Points:
- Item data loaded from database
- Proper dialogue → notification sequence
- Prevents taking multiple times
- Shows "Already Taken" if taken

---

**Setup the panel and add items to database!** 🎮✨

**Use Room07_ItemDatabaseSetup script to add all items!** 📦
