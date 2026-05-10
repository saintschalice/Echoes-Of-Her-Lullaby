# Arrow Buttons Setup Guide (Backup Controls)

## 🎮 Alternative Control Method

Kung hindi talaga gumagana ang swipe UP/DOWN, pwede gumamit ng arrow buttons!

---

## 📋 Setup Steps

### Step 1: Create Arrow Buttons

Sa Toybox Panel, create 4 buttons:

```
Toybox Panel
├── Tiles Grid (existing)
├── Close Button (existing)
└── Arrow Controls (NEW)
    ├── UP Button (↑)
    ├── DOWN Button (↓)
    ├── LEFT Button (←)
    └── RIGHT Button (→)
```

### Step 2: Design Buttons

#### Option A: Text Buttons
```
UP Button:
  Text: "↑"
  Font Size: 40
  
DOWN Button:
  Text: "↓"
  Font Size: 40
  
LEFT Button:
  Text: "←"
  Font Size: 40
  
RIGHT Button:
  Text: "→"
  Font Size: 40
```

#### Option B: Image Buttons
```
Use arrow sprites/icons
Size: 60x60 pixels each
```

### Step 3: Layout Buttons

#### Layout 1: Cross Pattern (Recommended)
```
        [↑]
    [←] [↓] [→]
```

#### Layout 2: Row Pattern
```
[↑] [↓] [←] [→]
```

#### Layout 3: Corners
```
[↑]         [→]


[←]         [↓]
```

### Step 4: Assign to Script

1. Select **Room07_Manager** (or object with ToyboxSlidingPuzzle)
2. Find **ToyboxSlidingPuzzle** component
3. Expand **Optional: Arrow Buttons (Backup Controls)**
4. Drag buttons:
   - **Up Button** → UP button GameObject
   - **Down Button** → DOWN button GameObject
   - **Left Button** → LEFT button GameObject
   - **Right Button** → RIGHT button GameObject

---

## 🎯 How It Works

### Button Logic:
```
UP Button (↑):
  → Moves tile from BELOW to empty space
  → Same as swiping UP

DOWN Button (↓):
  → Moves tile from ABOVE to empty space
  → Same as swiping DOWN

LEFT Button (←):
  → Moves tile from RIGHT to empty space
  → Same as swiping LEFT

RIGHT Button (→):
  → Moves tile from LEFT to empty space
  → Same as swiping RIGHT
```

---

## 🎨 Example UI Layout

### Full Panel Structure:
```
┌─────────────────────────────┐
│     Toybox Puzzle Panel     │
├─────────────────────────────┤
│                             │
│   ┌───┬───┬───┐             │
│   │ 1 │ 2 │ 3 │  Tiles      │
│   ├───┼───┼───┤             │
│   │ 4 │ 5 │ 6 │             │
│   ├───┼───┼───┤             │
│   │ 7 │ 8 │   │             │
│   └───┴───┴───┘             │
│                             │
│       [↑]                   │
│   [←] [↓] [→]  Controls     │
│                             │
│         [Close]             │
└─────────────────────────────┘
```

---

## 🔧 Unity Setup (Detailed)

### 1. Create Arrow Controls Container
```
Right-click Toybox Panel
→ Create Empty
→ Rename to "Arrow Controls"
→ Add Horizontal Layout Group (optional)
```

### 2. Create UP Button
```
Right-click Arrow Controls
→ UI → Button
→ Rename to "UP_Button"
→ Set Text to "↑"
→ Adjust size and position
```

### 3. Create DOWN Button
```
Right-click Arrow Controls
→ UI → Button
→ Rename to "DOWN_Button"
→ Set Text to "↓"
→ Adjust size and position
```

### 4. Create LEFT Button
```
Right-click Arrow Controls
→ UI → Button
→ Rename to "LEFT_Button"
→ Set Text to "←"
→ Adjust size and position
```

### 5. Create RIGHT Button
```
Right-click Arrow Controls
→ UI → Button
→ Rename to "RIGHT_Button"
→ Set Text to "→"
→ Adjust size and position
```

### 6. Assign to Script
```
Select Room07_Manager
→ ToyboxSlidingPuzzle component
→ Optional: Arrow Buttons (Backup Controls)
  → Up Button: Drag UP_Button
  → Down Button: Drag DOWN_Button
  → Left Button: Drag LEFT_Button
  → Right Button: Drag RIGHT_Button
```

---

## 🎮 Testing

### Test Each Button:

1. **Play Mode**
2. **Open Toybox Puzzle**
3. **Click UP Button (↑)**
   - Console: `[ToyboxPuzzle] 🔘 Arrow button pressed: (0, 1)`
   - Tile from below should move (if valid)

4. **Click DOWN Button (↓)**
   - Console: `[ToyboxPuzzle] 🔘 Arrow button pressed: (0, -1)`
   - Tile from above should move (if valid)

5. **Click LEFT Button (←)**
   - Console: `[ToyboxPuzzle] 🔘 Arrow button pressed: (1, 0)`
   - Tile from right should move (if valid)

6. **Click RIGHT Button (→)**
   - Console: `[ToyboxPuzzle] 🔘 Arrow button pressed: (-1, 0)`
   - Tile from left should move (if valid)

---

## 🎨 Styling Tips

### Button Colors:
```
Normal: Light gray
Highlighted: White
Pressed: Dark gray
Disabled: Very light gray
```

### Button Size:
```
Desktop: 60x60 pixels
Mobile: 80x80 pixels (larger for touch)
```

### Button Spacing:
```
Gap between buttons: 10-20 pixels
```

### Font:
```
Font: Arial or similar
Size: 40-50
Color: Black or dark gray
```

---

## 🔄 Swipe + Buttons (Both Work!)

You can use **BOTH** swipe and buttons:
- **Swipe** for fast gameplay
- **Buttons** for precise control

Both methods work simultaneously!

---

## 🐛 Troubleshooting

### Buttons Not Responding:
```
Check:
1. Buttons assigned in Inspector?
2. Buttons have Button component?
3. Buttons are active (not disabled)?
4. Panel is active when testing?
```

### Wrong Tile Moves:
```
Check Console:
[ToyboxPuzzle] 🔘 Arrow button pressed: (X, Y)
[ToyboxPuzzle] Empty tile at Row:X Col:Y
[ToyboxPuzzle] Trying to move tile from Row:X Col:Y

If "Invalid move - out of bounds":
→ Normal! No tile in that direction.
```

### Buttons Don't Show:
```
Check:
1. Arrow Controls is child of Toybox Panel?
2. Buttons are active in hierarchy?
3. Canvas Scaler settings correct?
```

---

## 📱 Mobile Considerations

### Button Size:
```
Make buttons LARGER for mobile:
- Desktop: 60x60
- Mobile: 80x80 or 100x100
```

### Button Position:
```
Place buttons where thumbs can reach:
- Bottom of screen
- Sides of screen
- Not in center (blocks view)
```

### Touch Feedback:
```
Add visual feedback:
- Scale animation on press
- Color change on press
- Sound effect on press
```

---

## ✅ Verification Checklist

- [ ] 4 buttons created (↑ ↓ ← →)
- [ ] Buttons assigned in Inspector
- [ ] UP button moves tile from below
- [ ] DOWN button moves tile from above
- [ ] LEFT button moves tile from right
- [ ] RIGHT button moves tile from left
- [ ] Console shows button press logs
- [ ] Buttons work in Play Mode
- [ ] Buttons styled and positioned well

---

## 🎉 Summary

### What You Get:
- ✅ 4 arrow buttons for tile movement
- ✅ Works alongside swipe controls
- ✅ Easy to use on mobile and desktop
- ✅ Visual feedback in Console
- ✅ Same logic as swipe controls

### When to Use:
- Swipe controls not working
- Need precise control
- Prefer button controls
- Accessibility option

### Setup Time:
- 5-10 minutes to create and assign buttons

---

**Arrow buttons are a reliable backup if swipes don't work!** 🎮

**Both swipe and buttons work together!** ✨
