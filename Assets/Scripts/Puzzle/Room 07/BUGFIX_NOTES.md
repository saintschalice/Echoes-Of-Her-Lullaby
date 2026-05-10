# Bug Fix: EmilyGhost isAggressive Error

## Error Message
```
Assets\Scripts\Puzzle\Room 07\MirrorJumpscareSequence.cs(136,23): 
error CS1061: 'EmilyGhost' does not contain a definition for 'isAggressive'
```

## Root Cause
The `MirrorJumpscareSequence.cs` script was trying to set a property `isAggressive` that doesn't exist in the `EmilyGhost` class.

## Solution
Instead of adding a new property, we use the existing EmilyGhost system:

### Before (Broken):
```csharp
emily.isPaused = false;
emily.isAggressive = true; // ❌ Property doesn't exist
```

### After (Fixed):
```csharp
emily.isPaused = false;

// Make Emily chase faster and more aggressively
emily.huntSpeed = 3.5f; // Increase hunt speed for chase
emily.lostLOSTime = 5f; // Takes longer to lose sight of player

// Force Emily into Hunt state
emily.SetStateExternal(EmilyGhost.State.Hunt);
```

## How It Works Now

### EmilyGhost Properties Used:
1. **`huntSpeed`** - Speed when chasing player
   - Default: 0.5 (normal patrol/hunt)
   - Chase: 3.5 (much faster, scary!)

2. **`lostLOSTime`** - Time before losing sight of player
   - Default: 1.8 seconds
   - Chase: 5 seconds (harder to escape)

3. **`SetStateExternal(State.Hunt)`** - Forces Emily into Hunt mode
   - Makes Emily actively pursue the player
   - Bypasses normal state transitions

4. **`isPaused`** - Pauses/unpauses Emily's AI
   - Set to `false` to resume chasing

## Testing
After the mirror jumpscare:
- ✅ Emily should chase at 3.5 speed (much faster than normal)
- ✅ Emily should stay in Hunt mode
- ✅ Emily should be harder to lose (5 second LOS time)
- ✅ Player must escape to bathroom

## Files Modified
- `Assets/Scripts/Puzzle/Room 07/MirrorJumpscareSequence.cs` - Fixed chase activation
- `Assets/Scripts/Puzzle/Room 07/ROOM07_DEVELOPMENT_GUIDE.md` - Updated documentation

## No Changes Needed To:
- `Assets/Scripts/AI/EmilyGhost.cs` - Already has all needed functionality
