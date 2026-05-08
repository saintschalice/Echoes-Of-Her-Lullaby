# Tea Party Puzzle - Setup Guide (Tagalog)

## 🎯 Ano Ito?

Drag and drop puzzle kung saan ilalagay ng player ang Emily's Cup sa tamang slot.

---

## 📋 Kailangan Mo

### 6 na UI Elements:

1. **TeaParty_Panel** - Main container (full screen)
2. **TeaTable_Image** - Background ng tea table
3. **3 Tea Cups** - Naka-place na sa table (Cup1, Cup2, Cup3)
4. **EmilyCup_Slot** - Empty slot na may highlight (target)
5. **EmilyCup_Draggable** - Yung cup na i-drag ng player
6. **Close_Button** - Para isara (optional)

---

## 🎨 Step-by-Step Setup

### Step 1: Create Panel

```
1. Right-click Canvas → UI → Panel
2. Rename to "TeaParty_Panel"
3. Set Anchor: Stretch (buong screen)
4. Set Color: Black (0, 0, 0, 200) - semi-transparent
```

### Step 2: Add Tea Table Background

```
1. Right-click TeaParty_Panel → UI → Image
2. Rename to "TeaTable_Image"
3. Assign sprite: Tea table background mo
4. Set size: 800x600
5. Center sa screen
```

### Step 3: Add 3 Cups (Already Placed)

```
Para sa bawat cup:

Cup 1:
1. Right-click TeaTable_Image → UI → Image
2. Rename to "Cup1_Image"
3. Assign sprite: Tea cup sprite
4. Set size: 100x100
5. Position: Top-left ng table

Cup 2:
1. Same steps
2. Rename to "Cup2_Image"
3. Position: Top-right ng table

Cup 3:
1. Same steps
2. Rename to "Cup3_Image"
3. Position: Bottom-left ng table
```

### Step 4: Create Emily's Cup Slot (Target)

```
1. Right-click TeaTable_Image → UI → Image
2. Rename to "EmilyCup_Slot"
3. Assign sprite: Circle or highlight sprite
4. Set size: 120x120 (mas malaki ng konti sa cup)
5. Position: Bottom-right ng table (empty slot)
6. Set Color: Yellow (255, 255, 0, 255)
```

**Important:** Ito yung TARGET kung saan ilalagay ang cup!

### Step 5: Create Draggable Cup

```
1. Right-click TeaParty_Panel → UI → Image
2. Rename to "EmilyCup_Draggable"
3. Assign sprite: Emily's special cup sprite
4. Set size: 100x100
5. Position: Bottom ng screen (starting position)
6. Add Component: Canvas Group
   - Interactable: ✓ (checked)
   - Block Raycasts: ✓ (checked)
```

**Important:** Ito yung cup na i-DRAG ng player!

### Step 6: Add Script

```
1. Select TeaParty_Panel
2. Add Component → TeaPartyPuzzleUI
```

### Step 7: Assign References

```
TeaPartyPuzzleUI:
  UI References:
    Tea Party Panel: [I-drag ang TeaParty_Panel dito]
    Close Button: [I-drag ang Close_Button dito]
  
  Drag & Drop:
    Emily Cup Draggable: [I-drag ang EmilyCup_Draggable dito]
    Emily Cup Slot: [I-drag ang EmilyCup_Slot dito]
    Snap Distance: 100 (adjust kung gusto mo)
  
  Visual Feedback:
    Slot Highlight: [I-drag ang EmilyCup_Slot dito]
    Normal Color: White (255, 255, 255, 255)
    Highlight Color: Yellow (255, 255, 0, 255)
  
  Audio:
    Cup Place Sound: [Assign sound effect]
    Success Sound: [Assign success sound]
```

---

## 🎮 Paano Gumagana

### Flow:

1. **Panel Opens**
   - Makikita ng player ang tea table
   - May 3 cups na naka-place na
   - May 1 empty slot (yellow highlight)
   - Emily's Cup ay nasa baba (pwede i-drag)

2. **Player Drags Cup**
   - Click/touch sa Emily's Cup
   - I-drag papunta sa empty slot
   - Mag-highlight ang slot pag malapit na

3. **Cup Snaps to Slot**
   - Pag malapit na (within snap distance)
   - Automatic na mag-snap sa slot
   - May sound effect

4. **Puzzle Complete**
   - Panel closes after 1 second
   - Memory Cutscene 1 plays
   - Cup removed from inventory

---

## 🎨 Visual Layout

```
┌─────────────────────────┐
│   Tea Party Panel       │
├─────────────────────────┤
│                         │
│   Tea Table:            │
│                         │
│   ☕        ☕          │
│   Cup1     Cup2         │
│                         │
│   ☕        ⭕          │
│   Cup3     SLOT         │
│            (Empty)      │
│                         │
│                         │
│        ☕               │
│    Emily's Cup          │
│    (I-drag dito!)       │
│                         │
└─────────────────────────┘
```

---

## ⚙️ Settings

### Snap Distance:
```
50 = Mahigpit (kailangan sobrang lapit)
100 = Normal (recommended)
150 = Maluwag (madaling mag-snap)
```

### Colors:
```
Normal Color: White (hindi highlighted)
Highlight Color: Yellow (pag malapit na ang cup)
```

---

## 🧪 Testing

### Test 1: Drag Cup
```
1. Play Mode
2. Open Tea Party panel
3. Click sa Emily's Cup
4. I-drag
5. Dapat sumusunod sa mouse/finger ✓
```

### Test 2: Highlight
```
1. I-drag ang cup malapit sa slot
2. Dapat mag-yellow ang slot ✓
3. I-drag palayo
4. Balik sa white ang slot ✓
```

### Test 3: Snap
```
1. I-drag ang cup malapit sa slot
2. Release
3. Dapat mag-snap sa slot ✓
4. May sound ✓
```

### Test 4: Return to Start
```
1. I-drag ang cup malayo sa slot
2. Release (hindi malapit)
3. Babalik sa starting position ✓
```

### Test 5: Complete
```
1. I-drag ang cup sa slot
2. Wait 1 second
3. Panel closes ✓
4. Cutscene plays ✓
```

---

## 🐛 Common Problems

### Problem 1: Hindi Ma-drag ang Cup
```
Check:
1. May Canvas Group ba ang EmilyCup_Draggable?
2. Interactable is checked?
3. Block Raycasts is checked?
```

### Problem 2: Hindi Nag-highlight ang Slot
```
Check:
1. Slot Highlight assigned sa Inspector?
2. Highlight Color is yellow?
3. Snap Distance is 100?
```

### Problem 3: Hindi Nag-snap ang Cup
```
Check:
1. Snap Distance too small? Try 100
2. Emily Cup Slot assigned?
3. Both are RectTransforms?
```

### Problem 4: Hindi Nagsasara ang Panel
```
Check:
1. Room07UIManager assigned?
2. Check Console for errors
```

---

## ✅ Quick Checklist

- [ ] TeaParty_Panel created
- [ ] TeaTable_Image added (background)
- [ ] Cup1_Image added (top-left)
- [ ] Cup2_Image added (top-right)
- [ ] Cup3_Image added (bottom-left)
- [ ] EmilyCup_Slot added (bottom-right, yellow)
- [ ] EmilyCup_Draggable added (bottom, movable)
- [ ] Canvas Group added to draggable cup
- [ ] TeaPartyPuzzleUI script added
- [ ] All references assigned
- [ ] Snap distance = 100
- [ ] Colors set (white and yellow)
- [ ] Tested dragging
- [ ] Tested snapping
- [ ] Tested completion

---

## 🎯 Key Points

### Dalawang Cup Objects:
1. **EmilyCup_Slot** = TARGET (yellow circle, hindi gumagalaw)
2. **EmilyCup_Draggable** = MOVABLE (yung i-drag ng player)

### Canvas Group:
- Kailangan sa draggable cup
- Interactable: ✓
- Block Raycasts: ✓

### Snap Distance:
- 100 is good default
- Adjust kung masyadong mahirap o madali

---

## 📱 Mobile Tips

### Touch Input:
- Automatic gumagana
- Drag with finger
- Release to drop

### Sizes:
- Desktop: 100x100
- Mobile: 120x120 (mas malaki para madaling i-tap)

### Snap Distance:
- Desktop: 50-100
- Mobile: 100-150 (mas maluwag)

---

## 🎉 Summary

### Kailangan:
1. Panel (full screen)
2. Table background
3. 3 static cups
4. 1 slot (target, yellow)
5. 1 draggable cup (Emily's)
6. Script with references

### Paano:
1. Drag Emily's Cup
2. Slot highlights pag malapit
3. Cup snaps pag close enough
4. Puzzle complete
5. Cutscene plays

---

**Setup mo na! Drag and drop lang!** 🎮✨

**Kailangan mo ng sprites para sa cups at table!** 🎨☕
