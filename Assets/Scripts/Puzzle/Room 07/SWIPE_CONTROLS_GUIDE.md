# Swipe Controls Guide - Sliding Puzzle

## ✅ UPDATED: Swipe Controls for 8-Tile Puzzle

### Old System (Click):
```
❌ Click tile → Tile moves to empty space
❌ Hard to use on mobile
❌ Need to click exact tile
```

### New System (Swipe):
```
✅ Swipe LEFT → Tile from RIGHT moves to empty
✅ Swipe RIGHT → Tile from LEFT moves to empty
✅ Swipe UP → Tile from BELOW moves to empty
✅ Swipe DOWN → Tile from ABOVE moves to empty
✅ Easy on mobile!
```

---

## 🎮 How Swipe Controls Work

### Swipe Direction Logic:
```
Empty space is here: [ ]

Swipe RIGHT →
  Tile from LEFT moves to empty
  [1] [ ] [2]  →  [ ] [1] [2]
  
Swipe LEFT ←
  Tile from RIGHT moves to empty
  [1] [ ] [2]  →  [1] [2] [ ]
  
Swipe DOWN ↓
  Tile from ABOVE moves to empty
  [1]          [ ]
  [ ]    →     [1]
  [2]          [2]
  
Swipe UP ↑
  Tile from BELOW moves to empty
  [1]          [1]
  [ ]    →     [2]
  [2]          [ ]
```

---

## 🎯 Swipe Settings

### In Inspector:
```
ToyboxSlidingPuzzle:
  Swipe Settings:
    Swipe Threshold: 50 ← Minimum distance for swipe
    Swipe Deadzone: 0.3 ← Time between swipes (seconds)
```

### Swipe Threshold:
- **50** = Normal sensitivity (recommended)
- **30** = More sensitive (easier to trigger)
- **100** = Less sensitive (need longer swipe)

### Swipe Deadzone:
- **0.3** = Normal speed (recommended)
- **0.1** = Fast (can swipe quickly)
- **0.5** = Slow (prevents accidental swipes)

---

## 🧪 Testing Swipe Controls

### Test 1: Horizontal Swipes
```
1. Open Toybox puzzle
2. Swipe RIGHT on screen
3. Tile from left should move
4. Swipe LEFT on screen
5. Tile from right should move
```

### Test 2: Vertical Swipes
```
1. Open Toybox puzzle
2. Swipe DOWN on screen
3. Tile from above should move
4. Swipe UP on screen
5. Tile from below should move
```

### Test 3: Invalid Swipes
```
1. Try to swipe when no tile can move
2. Should see: "[ToyboxPuzzle] Invalid move - out of bounds"
3. No tile moves (correct!)
```

### Test 4: Solve Puzzle
```
1. Use swipes to solve puzzle
2. When complete, panel closes
3. Dialogue: "The lock clicked..."
```

---

## 📊 Swipe Detection Flow

```
Player touches screen
  ↓
Record start position
  ↓
Player drags finger
  ↓
Player releases finger
  ↓
Calculate swipe distance
  ↓
Distance > Threshold?
  ↓ YES
Determine direction (horizontal or vertical)
  ↓
Calculate which tile to move
  ↓
Move tile to empty space
  ↓
Play sound
  ↓
Check if solved
```

---

## 🎨 Visual Guide

### Swipe RIGHT Example:
```
Before:
┌───┬───┬───┐
│ 1 │   │ 2 │  Empty in middle
├───┼───┼───┤
│ 3 │ 4 │ 5 │
├───┼───┼───┤
│ 6 │ 7 │ 8 │
└───┴───┴───┘

Player swipes RIGHT →

After:
┌───┬───┬───┐
│   │ 1 │ 2 │  Tile 1 moved right
├───┼───┼───┤
│ 3 │ 4 │ 5 │
├───┼───┼───┤
│ 6 │ 7 │ 8 │
└───┴───┴───┘
```

### Swipe DOWN Example:
```
Before:
┌───┬───┬───┐
│ 1 │ 2 │ 3 │
├───┼───┼───┤
│   │ 4 │ 5 │  Empty on left
├───┼───┼───┤
│ 6 │ 7 │ 8 │
└───┴───┴───┘

Player swipes DOWN ↓

After:
┌───┬───┬───┐
│   │ 2 │ 3 │  Tile 1 moved down
├───┼───┼───┤
│ 1 │ 4 │ 5 │
├───┼───┼───┤
│ 6 │ 7 │ 8 │
└───┴───┴───┘
```

---

## 🔧 Customization

### Make Swipes More Sensitive:
```
Swipe Threshold: 30 (lower = more sensitive)
Swipe Deadzone: 0.1 (lower = faster)
```

### Make Swipes Less Sensitive:
```
Swipe Threshold: 100 (higher = need longer swipe)
Swipe Deadzone: 0.5 (higher = slower)
```

### Disable Swipe Deadzone:
```
Swipe Deadzone: 0 (can swipe continuously)
Warning: May cause accidental moves!
```

---

## 🐛 Common Issues

### Issue 1: Swipes Not Detected
```
Problem: Swipe but nothing happens
Cause: Swipe too short
Fix: Increase swipe distance or lower threshold
```

### Issue 2: Only Left/Right Works, Not Up/Down
```
Problem: Vertical swipes don't work
Cause 1: UI elements blocking swipe input
Fix: Make sure you're swiping on the puzzle area, not on buttons

Cause 2: Swipe threshold too high
Fix: Lower swipe threshold to 30-40

Cause 3: Swipe not vertical enough
Fix: Swipe more vertically (straight up/down)

Debug: Check Console logs:
  - "Swipe detected: (x, y)" shows swipe direction
  - If x > y, it's horizontal
  - If y > x, it's vertical
```

### Issue 3: Wrong Tile Moves
```
Problem: Unexpected tile moves
Cause: Swipe direction calculation
Fix: Check swipe threshold and direction logic
```

### Issue 4: Too Sensitive
```
Problem: Accidental swipes trigger
Cause: Threshold too low
Fix: Increase swipe threshold to 50-100
```

### Issue 5: Too Slow
```
Problem: Can't swipe fast enough
Cause: Deadzone too high
Fix: Lower swipe deadzone to 0.1-0.2
```

---

## 📱 Mobile vs Desktop

### Mobile (Touch):
```
✅ Swipe with finger
✅ Natural gesture
✅ Easy to use
✅ Fast gameplay
```

### Desktop (Mouse):
```
✅ Click and drag
✅ Works same as touch
✅ Swipe threshold applies
✅ Can use mouse or trackpad
```

---

## 🎓 Pro Tips

1. **Swipe Anywhere** - Don't need to swipe on specific tile
2. **Direction Matters** - Swipe opposite of where you want tile to go
3. **Practice** - Try a few swipes to get the feel
4. **Fast Swipes** - Short, quick swipes work best
5. **Visual Feedback** - Watch tiles move to understand direction

---

## 🎯 Swipe Direction Cheat Sheet

```
Want to move tile RIGHT?
  → Swipe LEFT ←

Want to move tile LEFT?
  → Swipe RIGHT →

Want to move tile DOWN?
  → Swipe UP ↑

Want to move tile UP?
  → Swipe DOWN ↓

Remember: Swipe OPPOSITE of where tile should go!
```

---

## ✅ Verification

### Test Checklist:
- [ ] Swipe right moves tile from left
- [ ] Swipe left moves tile from right
- [ ] Swipe up moves tile from below
- [ ] Swipe down moves tile from above
- [ ] Invalid swipes don't move tiles
- [ ] Sound plays on valid move
- [ ] Puzzle can be solved with swipes
- [ ] Works on mobile and desktop

---

## 🎉 Summary

### What Changed:
- ✅ Removed click controls
- ✅ Added swipe detection
- ✅ Swipe in direction to move tiles
- ✅ Configurable sensitivity
- ✅ Deadzone to prevent spam
- ✅ Works on mobile and desktop

### How to Use:
1. Open Toybox puzzle
2. Swipe in any direction
3. Tile moves to empty space
4. Solve puzzle with swipes!

---

**Swipe controls are now active! Test mo na!** 🎮📱

**Swipe LEFT/RIGHT/UP/DOWN to move tiles!** ✨
