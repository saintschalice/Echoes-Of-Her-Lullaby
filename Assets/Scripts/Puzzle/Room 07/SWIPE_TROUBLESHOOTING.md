# Swipe Up/Down Troubleshooting Guide

## ❌ Problem: Swipe UP/DOWN Hindi Gumagana

Kung LEFT/RIGHT lang ang gumagana, pero UP/DOWN hindi, sundin ang guide na ito.

---

## 🔍 Step 1: Check Console Logs

### Ano ang dapat makita:

#### ✅ GOOD - Vertical Swipe Detected:
```
[ToyboxPuzzle] ✋ Swipe started at: (500, 200)
[ToyboxPuzzle] 🎯 Swipe ended at: (510, 400)
[ToyboxPuzzle] 📊 Delta: (10.0, 200.0), Magnitude: 200.2, Threshold: 50
[ToyboxPuzzle] Swipe detected: Delta=(10.0, 200.0), AbsX=10.0, AbsY=200.0
[ToyboxPuzzle] VERTICAL swipe (absY 200.0 > absX 10.0)
[ToyboxPuzzle] Swipe UP detected → Moving tile from BELOW
```

#### ❌ BAD - Horizontal Detected Instead:
```
[ToyboxPuzzle] ✋ Swipe started at: (500, 200)
[ToyboxPuzzle] 🎯 Swipe ended at: (600, 300)
[ToyboxPuzzle] 📊 Delta: (100.0, 100.0), Magnitude: 141.4, Threshold: 50
[ToyboxPuzzle] Swipe detected: Delta=(100.0, 100.0), AbsX=100.0, AbsY=100.0
[ToyboxPuzzle] HORIZONTAL swipe (absX 100.0 > absY 100.0)
[ToyboxPuzzle] Swipe RIGHT detected → Moving tile from LEFT
```
→ Swipe mo ay diagonal! X at Y ay pareho.

#### ❌ BAD - No Logs at All:
```
(walang logs)
```
→ Swipe hindi na-detect. Check if panel is active.

---

## 🎯 Step 2: Test Swipe Direction

### Test UP Swipe:
1. Put finger at **BOTTOM** of screen
2. Swipe **STRAIGHT UP** to top
3. Don't move left or right, only UP
4. Check Console logs

### Test DOWN Swipe:
1. Put finger at **TOP** of screen
2. Swipe **STRAIGHT DOWN** to bottom
3. Don't move left or right, only DOWN
4. Check Console logs

### What to Look For:
```
Delta: (X, Y)

For UP swipe:
✅ Y should be LARGE POSITIVE (e.g., 200)
✅ X should be SMALL (e.g., 10)
✅ AbsY > AbsX

For DOWN swipe:
✅ Y should be LARGE NEGATIVE (e.g., -200)
✅ X should be SMALL (e.g., -10)
✅ AbsY > AbsX
```

---

## 🔧 Step 3: Adjust Settings

### Option 1: Lower Swipe Threshold
```
Inspector → ToyboxSlidingPuzzle → Swipe Settings
Swipe Threshold: 30 (default is 50)
```
Mas madali ma-detect ang swipes.

### Option 2: Add Vertical Bias
Kung gusto mo mas sensitive sa vertical swipes, pwede natin i-adjust ang logic.

---

## 🎮 Step 4: Use Arrow Buttons (Backup)

Kung talaga hindi gumagana ang swipes, may backup controls na!

### Setup Arrow Buttons:

1. **Create 4 Buttons** sa Toybox Panel:
   - UP Button (↑)
   - DOWN Button (↓)
   - LEFT Button (←)
   - RIGHT Button (→)

2. **Assign sa Inspector**:
   ```
   ToyboxSlidingPuzzle:
     Optional: Arrow Buttons (Backup Controls)
       Up Button: [Assign UP button]
       Down Button: [Assign DOWN button]
       Left Button: [Assign LEFT button]
       Right Button: [Assign RIGHT button]
   ```

3. **Test**:
   - Click UP button → Tile from below moves
   - Click DOWN button → Tile from above moves
   - Click LEFT button → Tile from right moves
   - Click RIGHT button → Tile from left moves

---

## 🐛 Common Issues

### Issue 1: Swipe is Diagonal
```
Problem: Delta X and Y are similar
Example: Delta: (100, 100)

Fix: Swipe more straight
- For UP: X should be small, Y should be large positive
- For DOWN: X should be small, Y should be large negative
```

### Issue 2: UI Element Blocking
```
Problem: May ScrollRect, Button, or other UI blocking input

Fix: 
1. Check if may UI element na naka-block
2. Disable raycast target sa background images
3. Or use arrow buttons instead
```

### Issue 3: Swipe Too Short
```
Problem: Magnitude < Threshold
Example: Magnitude: 40, Threshold: 50

Fix: Lower threshold to 30
```

### Issue 4: Touch Input Not Working
```
Problem: Mouse works but touch doesn't

Fix: Updated code now supports both:
- Input.GetMouseButton (desktop)
- Input.touchCount (mobile)
```

---

## 📱 Mobile vs Desktop Testing

### Desktop (Mouse):
```
- Click and drag
- Should see "✋ Swipe started"
- Should see "🎯 Swipe ended"
```

### Mobile (Touch):
```
- Touch and drag
- Should see "📱 Touch started"
- Should see "📱 Touch ended"
```

---

## 🎯 Expected Console Output

### Perfect UP Swipe:
```
[ToyboxPuzzle] ✋ Swipe started at: (500, 600)
[ToyboxPuzzle] 🎯 Swipe ended at: (505, 200)
[ToyboxPuzzle] 📊 Delta: (5.0, -400.0), Magnitude: 400.0, Threshold: 50
[ToyboxPuzzle] Swipe detected: Delta=(5.0, -400.0), AbsX=5.0, AbsY=400.0
[ToyboxPuzzle] VERTICAL swipe (absY 400.0 > absX 5.0) ✅
[ToyboxPuzzle] Swipe DOWN detected → Moving tile from ABOVE ✅
[ToyboxPuzzle] Empty tile at Row:1 Col:1 (Index:4)
[ToyboxPuzzle] Trying to move tile from Row:0 Col:1 (Direction:(0, -1))
[ToyboxPuzzle] Valid move! Moving tile at index 1 to empty space at 4 ✅
```

### Perfect DOWN Swipe:
```
[ToyboxPuzzle] ✋ Swipe started at: (500, 200)
[ToyboxPuzzle] 🎯 Swipe ended at: (495, 600)
[ToyboxPuzzle] 📊 Delta: (-5.0, 400.0), Magnitude: 400.0, Threshold: 50
[ToyboxPuzzle] Swipe detected: Delta=(-5.0, 400.0), AbsX=5.0, AbsY=400.0
[ToyboxPuzzle] VERTICAL swipe (absY 400.0 > absX 5.0) ✅
[ToyboxPuzzle] Swipe UP detected → Moving tile from BELOW ✅
[ToyboxPuzzle] Empty tile at Row:1 Col:1 (Index:4)
[ToyboxPuzzle] Trying to move tile from Row:2 Col:1 (Direction:(0, 1))
[ToyboxPuzzle] Valid move! Moving tile at index 7 to empty space at 4 ✅
```

---

## 🎨 Visual Debug

### Swipe Direction Chart:
```
        ↑ UP
        |
        | Y+
        |
←LEFT---+---RIGHT→
  X-    |    X+
        |
        | Y-
        |
        ↓ DOWN
```

### Delta Values:
```
UP:    Delta: (0, +200)   → Y positive, large
DOWN:  Delta: (0, -200)   → Y negative, large
LEFT:  Delta: (-200, 0)   → X negative, large
RIGHT: Delta: (+200, 0)   → X positive, large
```

---

## ✅ Verification Checklist

Test each direction and check Console:

### UP Swipe:
- [ ] Swipe straight up (bottom to top)
- [ ] Console shows "VERTICAL swipe"
- [ ] Console shows "Swipe UP detected"
- [ ] Console shows "Moving tile from BELOW"
- [ ] Tile moves (if valid)

### DOWN Swipe:
- [ ] Swipe straight down (top to bottom)
- [ ] Console shows "VERTICAL swipe"
- [ ] Console shows "Swipe DOWN detected"
- [ ] Console shows "Moving tile from ABOVE"
- [ ] Tile moves (if valid)

### LEFT Swipe:
- [ ] Swipe straight left (right to left)
- [ ] Console shows "HORIZONTAL swipe"
- [ ] Console shows "Swipe LEFT detected"
- [ ] Console shows "Moving tile from RIGHT"
- [ ] Tile moves (if valid)

### RIGHT Swipe:
- [ ] Swipe straight right (left to right)
- [ ] Console shows "HORIZONTAL swipe"
- [ ] Console shows "Swipe RIGHT detected"
- [ ] Console shows "Moving tile from LEFT"
- [ ] Tile moves (if valid)

---

## 🚀 Quick Fixes

### Fix 1: Lower Threshold
```csharp
// In Inspector
Swipe Threshold: 30
```

### Fix 2: Use Arrow Buttons
```
Add 4 buttons (↑ ↓ ← →)
Assign to ToyboxSlidingPuzzle
Click to move tiles
```

### Fix 3: Check UI Blocking
```
Select all UI images in panel
Uncheck "Raycast Target" if not needed
```

### Fix 4: Test on Different Device
```
Desktop: Use mouse
Mobile: Use touch
Both should work now
```

---

## 📞 Still Not Working?

### Send me these Console logs:

1. **Swipe UP attempt:**
   ```
   Copy all logs starting with [ToyboxPuzzle]
   ```

2. **Swipe DOWN attempt:**
   ```
   Copy all logs starting with [ToyboxPuzzle]
   ```

3. **Settings:**
   ```
   Swipe Threshold: ?
   Swipe Deadzone: ?
   Grid Size: ?
   ```

---

## 🎉 Summary

### What Was Added:
- ✅ Better debug logs with emojis
- ✅ Shows AbsX and AbsY comparison
- ✅ Touch input support for mobile
- ✅ Arrow buttons as backup controls
- ✅ More detailed error messages

### How to Fix:
1. **Check Console** - See what's being detected
2. **Swipe Straight** - Don't diagonal
3. **Lower Threshold** - Make it 30 instead of 50
4. **Use Arrow Buttons** - Backup if swipes don't work

### Next Steps:
1. Open Console
2. Test all 4 directions
3. Copy logs here if still not working
4. Or use arrow buttons as alternative

---

**Test mo ulit with Console open! Makikita mo kung VERTICAL or HORIZONTAL ang na-detect.** 🔍

**If still not working, use arrow buttons!** 🎮✨
