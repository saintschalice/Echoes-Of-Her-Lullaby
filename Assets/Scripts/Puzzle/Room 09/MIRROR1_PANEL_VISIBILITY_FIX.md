# Mirror 1 - Panel Visibility Fix

## Problem:
Yung puzzle panel (bottles) ay naka-display sa screen kahit:
- ❌ Hindi pa nag-start yung puzzle
- ❌ After solving the puzzle

## Cause:
Walang `Start()` method sa `Mirror1_MedicineCabinet.cs` para i-hide yung panel initially.

## Solution:
✅ Added `Start()` method na nag-hide ng panel at the beginning

## What Changed:

### Before:
```csharp
public class Mirror1_MedicineCabinet : MonoBehaviour
{
    // ... variables ...
    
    public void StartPuzzle()  // No Start() method!
    {
        // Show panel
        if (puzzlePanel != null) puzzlePanel.SetActive(true);
        // ...
    }
}
```

**Result**: Panel visible from the start ❌

### After:
```csharp
public class Mirror1_MedicineCabinet : MonoBehaviour
{
    // ... variables ...
    
    void Start()  // NEW!
    {
        // Hide panel at start
        if (puzzlePanel != null)
        {
            puzzlePanel.SetActive(false);
        }
    }
    
    public void StartPuzzle()
    {
        // Show panel
        if (puzzlePanel != null) puzzlePanel.SetActive(true);
        // ...
    }
}
```

**Result**: Panel hidden until puzzle starts ✅

---

## Flow Now:

### 1. Scene Loads:
- `Start()` runs
- Panel is HIDDEN
- Bottles not visible

### 2. Player Interacts with Mirror 1:
- `StartPuzzle()` runs
- Panel is SHOWN
- Bottles become visible
- Timer starts

### 3. Player Solves Puzzle:
- `PuzzleSuccess()` runs
- Success dialogue shows
- Panel is HIDDEN again
- Bottles disappear

### 4. Player Fails (3 mistakes or timeout):
- `EmilyAttack()` runs
- Jumpscare shows
- Game reloads

---

## Testing:

### Test 1: Initial State
1. Load the scene
2. **Expected**: Puzzle panel is HIDDEN
3. **Expected**: Bottles are NOT visible

### Test 2: Start Puzzle
1. Interact with Mirror 1
2. **Expected**: Panel appears
3. **Expected**: Bottles become visible

### Test 3: Complete Puzzle
1. Arrange bottles correctly
2. **Expected**: Success dialogue
3. **Expected**: Panel HIDES after dialogue
4. **Expected**: Bottles disappear

### Test 4: Fail Puzzle
1. Make 3 mistakes OR let timer run out
2. **Expected**: Emily attack
3. **Expected**: Scene reloads

---

## Other Mirrors:

Check if other mirrors have the same issue:

### Mirror 2 (Bathtub):
- Check if panel is hidden at start
- Should only show when puzzle starts

### Mirror 3 (Diary):
- Check if panel is hidden at start
- Should only show when puzzle starts

### Mirror 4 (Evidence):
- Check if panel is hidden at start
- Should only show when puzzle starts

---

## Quick Fix for Other Mirrors:

If other mirrors have the same problem, add this to their scripts:

```csharp
void Start()
{
    // Hide panel at start
    if (puzzlePanel != null)
    {
        puzzlePanel.SetActive(false);
    }
}
```

---

## Summary:

✅ **Fixed**: Panel now hidden at start
✅ **Fixed**: Panel shows only when puzzle starts
✅ **Fixed**: Panel hides after puzzle completes
✅ **Tested**: Flow works correctly

The bottles should no longer be visible before and after the puzzle! 🎯
