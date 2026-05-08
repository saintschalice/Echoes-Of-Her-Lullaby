# Debug Logs Added to Swipe Controls

## ✅ What Was Done

Added comprehensive debug logging to `ToyboxSlidingPuzzle.cs` to help diagnose swipe detection issues.

---

## 🔍 Debug Logs Added

### 1. Swipe Start Detection
```csharp
Debug.Log($"[ToyboxPuzzle] Swipe started at: {swipeStartPos}");
```
Shows when player starts swiping and the starting position.

### 2. Swipe End Detection
```csharp
Debug.Log($"[ToyboxPuzzle] Swipe ended. Delta: {swipeDelta}, Magnitude: {swipeDelta.magnitude}, Threshold: {swipeThreshold}");
```
Shows:
- Swipe delta (X, Y movement)
- Swipe magnitude (total distance)
- Threshold required

### 3. Swipe Too Short
```csharp
Debug.Log($"[ToyboxPuzzle] Swipe too short (magnitude {swipeDelta.magnitude} < threshold {swipeThreshold})");
```
Shows when swipe doesn't meet minimum distance.

### 4. Swipe Direction Detected
```csharp
Debug.Log("[ToyboxPuzzle] Swipe RIGHT detected → Moving tile from LEFT");
Debug.Log("[ToyboxPuzzle] Swipe LEFT detected → Moving tile from RIGHT");
Debug.Log("[ToyboxPuzzle] Swipe UP detected → Moving tile from BELOW");
Debug.Log("[ToyboxPuzzle] Swipe DOWN detected → Moving tile from ABOVE");
```
Shows which direction was detected and which tile will move.

### 5. Empty Tile Position
```csharp
Debug.Log($"[ToyboxPuzzle] Empty tile at Row:{emptyRow} Col:{emptyCol} (Index:{emptyTileIndex})");
```
Shows where the empty space is located.

### 6. Tile Movement Attempt
```csharp
Debug.Log($"[ToyboxPuzzle] Trying to move tile from Row:{tileRow} Col:{tileCol} (Direction:{direction})");
```
Shows which tile is trying to move.

### 7. Invalid Move
```csharp
Debug.Log($"[ToyboxPuzzle] Invalid move - out of bounds (Row:{tileRow}, Col:{tileCol})");
```
Shows when there's no tile in that direction.

### 8. Valid Move
```csharp
Debug.Log($"[ToyboxPuzzle] Valid move! Moving tile at index {tileIndex} to empty space at {emptyTileIndex}");
```
Shows successful tile movement.

---

## 🎯 How to Use Debug Logs

### Step 1: Open Console
```
Unity → Window → General → Console
Or press: Ctrl+Shift+C (Windows) / Cmd+Shift+C (Mac)
```

### Step 2: Play Game
```
Enter Play Mode
Open Toybox puzzle
```

### Step 3: Swipe and Watch Console
```
Swipe on puzzle
Watch Console logs appear in real-time
```

---

## 📊 Example Console Output

### Successful UP Swipe:
```
[ToyboxPuzzle] Swipe started at: (500, 200)
[ToyboxPuzzle] Swipe ended. Delta: (10, 180), Magnitude: 180.3, Threshold: 50
[ToyboxPuzzle] Swipe detected: (10, 180) (magnitude: 180.3)
[ToyboxPuzzle] Swipe UP detected → Moving tile from BELOW
[ToyboxPuzzle] Empty tile at Row:2 Col:1 (Index:7)
[ToyboxPuzzle] Trying to move tile from Row:3 Col:1 (Direction:(0, 1))
[ToyboxPuzzle] Invalid move - out of bounds (Row:3, Col:1)
```
→ No tile below empty space (empty is at bottom row)

### Successful RIGHT Swipe:
```
[ToyboxPuzzle] Swipe started at: (400, 300)
[ToyboxPuzzle] Swipe ended. Delta: (150, 20), Magnitude: 151.3, Threshold: 50
[ToyboxPuzzle] Swipe detected: (150, 20) (magnitude: 151.3)
[ToyboxPuzzle] Swipe RIGHT detected → Moving tile from LEFT
[ToyboxPuzzle] Empty tile at Row:1 Col:1 (Index:4)
[ToyboxPuzzle] Trying to move tile from Row:1 Col:0 (Direction:(-1, 0))
[ToyboxPuzzle] Valid move! Moving tile at index 3 to empty space at 4
```
→ Tile successfully moved!

### Failed Swipe (Too Short):
```
[ToyboxPuzzle] Swipe started at: (500, 300)
[ToyboxPuzzle] Swipe ended. Delta: (20, 30), Magnitude: 36.1, Threshold: 50
[ToyboxPuzzle] Swipe too short (magnitude 36.1 < threshold 50)
```
→ Swipe didn't meet minimum distance

---

## 🐛 Troubleshooting with Logs

### Problem: No Logs Appear
```
Cause: Swipe not detected at all
Check:
- Is puzzle panel active?
- Is ToyboxSlidingPuzzle script enabled?
- Are you in Play Mode?
```

### Problem: "Swipe too short" Always Shows
```
Cause: Swipe threshold too high
Fix: Lower threshold to 30-40 in Inspector
```

### Problem: Wrong Direction Detected
```
Cause: Swipe is diagonal
Fix: Swipe more straight (up/down or left/right)

Check Delta values:
- Horizontal: X should be much larger than Y
- Vertical: Y should be much larger than X
```

### Problem: "Invalid move - out of bounds"
```
Cause: No tile in that direction
This is NORMAL! It means:
- Empty space is at edge
- No tile to move from that direction
- Try swiping different direction
```

---

## 🎮 Testing Guide

### Test All 4 Directions:

#### Test UP:
```
1. Swipe straight UP
2. Check Console:
   - Should see "Swipe UP detected"
   - Should see "Moving tile from BELOW"
3. If empty at bottom row:
   - Will show "Invalid move - out of bounds" (normal!)
4. If tile below:
   - Will show "Valid move!" and tile moves
```

#### Test DOWN:
```
1. Swipe straight DOWN
2. Check Console:
   - Should see "Swipe DOWN detected"
   - Should see "Moving tile from ABOVE"
3. If empty at top row:
   - Will show "Invalid move - out of bounds" (normal!)
4. If tile above:
   - Will show "Valid move!" and tile moves
```

#### Test LEFT:
```
1. Swipe straight LEFT
2. Check Console:
   - Should see "Swipe LEFT detected"
   - Should see "Moving tile from RIGHT"
3. If empty at right edge:
   - Will show "Invalid move - out of bounds" (normal!)
4. If tile to right:
   - Will show "Valid move!" and tile moves
```

#### Test RIGHT:
```
1. Swipe straight RIGHT
2. Check Console:
   - Should see "Swipe RIGHT detected"
   - Should see "Moving tile from LEFT"
3. If empty at left edge:
   - Will show "Invalid move - out of bounds" (normal!)
4. If tile to left:
   - Will show "Valid move!" and tile moves
```

---

## 📈 Understanding Delta Values

### Swipe Delta = (End Position - Start Position)

#### Horizontal Swipes:
```
Swipe RIGHT:
Start: (200, 300)
End:   (400, 310)
Delta: (200, 10)
→ X is large positive, Y is small
→ Detected as RIGHT ✅

Swipe LEFT:
Start: (400, 300)
End:   (200, 290)
Delta: (-200, -10)
→ X is large negative, Y is small
→ Detected as LEFT ✅
```

#### Vertical Swipes:
```
Swipe UP:
Start: (300, 200)
End:   (310, 400)
Delta: (10, 200)
→ Y is large positive, X is small
→ Detected as UP ✅

Swipe DOWN:
Start: (300, 400)
End:   (290, 200)
Delta: (-10, -200)
→ Y is large negative, X is small
→ Detected as DOWN ✅
```

#### Diagonal Swipes (Problematic):
```
Diagonal:
Start: (200, 200)
End:   (400, 400)
Delta: (200, 200)
→ X and Y are equal
→ Could be detected as either!
→ Depends on which is slightly bigger
```

---

## 🎯 Key Insights from Logs

### What to Look For:

1. **Swipe Magnitude**
   - Should be > threshold (default 50)
   - If too low, increase swipe distance or lower threshold

2. **Delta Values**
   - For UP/DOWN: Y should be much larger than X
   - For LEFT/RIGHT: X should be much larger than Y

3. **Direction Detection**
   - Should match your swipe direction
   - If wrong, swipe more straight

4. **Empty Tile Position**
   - Shows where empty space is
   - Helps understand why moves are invalid

5. **Valid vs Invalid Moves**
   - Invalid is normal when no tile in that direction
   - Valid means tile successfully moved

---

## 🎉 Summary

### Debug Logs Help You:
- ✅ See if swipes are detected
- ✅ Understand swipe direction
- ✅ Diagnose why moves fail
- ✅ Tune swipe sensitivity
- ✅ Verify all 4 directions work

### How to Use:
1. Open Console
2. Play game and open puzzle
3. Swipe and watch logs
4. Adjust settings based on logs

### Common Fixes:
- Swipe too short → Lower threshold
- Wrong direction → Swipe more straight
- No detection → Check panel is active
- Invalid moves → Normal if no tile there

---

**Open Console and test! Makikita mo lahat ng nangyayari.** 🔍✨

**All 4 directions (UP/DOWN/LEFT/RIGHT) are supported!** 🎮
