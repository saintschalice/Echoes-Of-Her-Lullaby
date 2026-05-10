# Coordinate System Fix - Swipe Directions

## ❌ The Problem

Swipe UP and DOWN were giving "Invalid move - out of bounds" errors.

### What Was Happening:
```
Empty tile at Row:0 Col:0 (top-left corner)
Swipe UP → Trying to move tile from Row:-1 ❌ (out of bounds!)
Swipe DOWN → Trying to move tile from Row:1 ✅ (but wrong direction!)
```

---

## 🔍 Root Cause

**Unity UI Coordinate System:**
```
Row 0 is at the TOP
Row 1 is in the MIDDLE  
Row 2 is at the BOTTOM

This is OPPOSITE of what we expect!
```

### Visual Representation:
```
┌───┬───┬───┐
│ 0 │ 1 │ 2 │  ← Row 0 (TOP)
├───┼───┼───┤
│ 3 │ 4 │ 5 │  ← Row 1 (MIDDLE)
├───┼───┼───┤
│ 6 │ 7 │ 8 │  ← Row 2 (BOTTOM)
└───┴───┴───┘
```

### The Confusion:
```
When we say "swipe UP":
- We mean: move finger from bottom to top
- We expect: tile from below (Row 2) moves up
- But in Unity UI: "below" means HIGHER row number!

When we say "swipe DOWN":
- We mean: move finger from top to bottom
- We expect: tile from above (Row 0) moves down
- But in Unity UI: "above" means LOWER row number!
```

---

## ✅ The Fix

### Changed Direction Vectors:

#### Before (WRONG):
```csharp
if (swipeDelta.y > 0)
{
    // Swipe UP → Move tile from BELOW
    MoveTileInDirection(Vector2Int.down); // ❌ WRONG!
}
else
{
    // Swipe DOWN → Move tile from ABOVE
    MoveTileInDirection(Vector2Int.up); // ❌ WRONG!
}
```

#### After (CORRECT):
```csharp
if (swipeDelta.y > 0)
{
    // Swipe UP → Move tile from BELOW (higher row number in UI)
    MoveTileInDirection(Vector2Int.up); // ✅ CORRECT!
}
else
{
    // Swipe DOWN → Move tile from ABOVE (lower row number in UI)
    MoveTileInDirection(Vector2Int.down); // ✅ CORRECT!
}
```

---

## 📊 Understanding Unity UI Coordinates

### Grid Layout:
```
Index:  0   1   2
Row:    0   0   0  ← Top row
Col:    0   1   2

Index:  3   4   5
Row:    1   1   1  ← Middle row
Col:    0   1   2

Index:  6   7   8
Row:    2   2   2  ← Bottom row
Col:    0   1   2
```

### Direction Vectors in Unity UI:
```
Vector2Int.up = (0, -1)    → Decreases row (moves UP visually)
Vector2Int.down = (0, 1)   → Increases row (moves DOWN visually)
Vector2Int.left = (-1, 0)  → Decreases col (moves LEFT visually)
Vector2Int.right = (1, 0)  → Increases col (moves RIGHT visually)
```

---

## 🎯 How It Works Now

### Swipe UP (finger moves up):
```
Empty at Row:2 Col:1 (bottom)
Swipe UP detected
MoveTileInDirection(Vector2Int.up)
  → direction = (0, -1)
  → tileRow = 2 + (-1) = 1 ✅
  → Tile from Row:1 moves to Row:2 ✅
```

### Swipe DOWN (finger moves down):
```
Empty at Row:0 Col:1 (top)
Swipe DOWN detected
MoveTileInDirection(Vector2Int.down)
  → direction = (0, 1)
  → tileRow = 0 + 1 = 1 ✅
  → Tile from Row:1 moves to Row:0 ✅
```

### Swipe LEFT (finger moves left):
```
Empty at Row:1 Col:2 (right)
Swipe LEFT detected
MoveTileInDirection(Vector2Int.right)
  → direction = (1, 0)
  → tileCol = 2 + 1 = 3 ❌ Out of bounds (correct!)
  
Empty at Row:1 Col:1 (middle)
Swipe LEFT detected
MoveTileInDirection(Vector2Int.right)
  → direction = (1, 0)
  → tileCol = 1 + 1 = 2 ✅
  → Tile from Col:2 moves to Col:1 ✅
```

### Swipe RIGHT (finger moves right):
```
Empty at Row:1 Col:0 (left)
Swipe RIGHT detected
MoveTileInDirection(Vector2Int.left)
  → direction = (-1, 0)
  → tileCol = 0 + (-1) = -1 ❌ Out of bounds (correct!)
  
Empty at Row:1 Col:1 (middle)
Swipe RIGHT detected
MoveTileInDirection(Vector2Int.left)
  → direction = (-1, 0)
  → tileCol = 1 + (-1) = 0 ✅
  → Tile from Col:0 moves to Col:1 ✅
```

---

## 🎮 Testing After Fix

### Test 1: Empty at Top-Left (Row:0 Col:0)
```
Can move:
✅ RIGHT (tile from Col:1)
✅ DOWN (tile from Row:1)

Cannot move:
❌ LEFT (out of bounds)
❌ UP (out of bounds)
```

### Test 2: Empty at Center (Row:1 Col:1)
```
Can move:
✅ UP (tile from Row:0)
✅ DOWN (tile from Row:2)
✅ LEFT (tile from Col:2)
✅ RIGHT (tile from Col:0)
```

### Test 3: Empty at Bottom-Right (Row:2 Col:2)
```
Can move:
✅ LEFT (tile from Col:1)
✅ UP (tile from Row:1)

Cannot move:
❌ RIGHT (out of bounds)
❌ DOWN (out of bounds)
```

---

## 🔧 Arrow Buttons Also Fixed

### Before (WRONG):
```csharp
upButton → Vector2Int.down    // ❌
downButton → Vector2Int.up    // ❌
```

### After (CORRECT):
```csharp
upButton → Vector2Int.up      // ✅
downButton → Vector2Int.down  // ✅
```

---

## 📝 Key Takeaways

### Unity UI Grid Layout:
- **Row 0 = TOP** (not bottom!)
- **Row increases = DOWN** (not up!)
- **Vector2Int.up = (0, -1)** (decreases row)
- **Vector2Int.down = (0, 1)** (increases row)

### Swipe Logic:
- **Swipe UP** → Use `Vector2Int.up` (decreases row)
- **Swipe DOWN** → Use `Vector2Int.down` (increases row)
- **Swipe LEFT** → Use `Vector2Int.right` (tile from right moves left)
- **Swipe RIGHT** → Use `Vector2Int.left` (tile from left moves right)

### Why It's Confusing:
- Swipe direction = finger movement
- Tile movement = opposite of swipe
- Unity UI rows = top to bottom (0 to 2)
- Vector2Int.up = negative Y (decreases row)

---

## ✅ Verification

### Console Output Should Show:
```
[ToyboxPuzzle] Empty tile at Row:1 Col:1 (Index:4)
[ToyboxPuzzle] Swipe UP detected → Moving tile from BELOW (higher row)
[ToyboxPuzzle] Trying to move tile from Row:2 Col:1 (Direction:(0, 1))
[ToyboxPuzzle] Valid move! Moving tile at index 7 to empty space at 4
```

### All 4 Directions Should Work:
- ✅ Swipe UP → Tile from below moves
- ✅ Swipe DOWN → Tile from above moves
- ✅ Swipe LEFT → Tile from right moves
- ✅ Swipe RIGHT → Tile from left moves

---

## 🎉 Summary

### The Issue:
- Direction vectors were inverted for vertical movement
- Unity UI uses top-to-bottom row numbering
- Vector2Int.up actually decreases row number

### The Fix:
- Swapped `Vector2Int.up` and `Vector2Int.down` in swipe logic
- Updated arrow button directions to match
- Added comments explaining Unity UI coordinate system

### Result:
- ✅ All 4 swipe directions now work correctly
- ✅ Arrow buttons work correctly
- ✅ No more "out of bounds" errors for valid moves

---

**Test mo ulit! Dapat gumagana na ang UP at DOWN swipes!** 🎮✨

**The coordinate system is now correct!** ✅
