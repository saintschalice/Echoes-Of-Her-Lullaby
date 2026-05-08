# Fade In/Fade Out Transitions - Complete Guide

**Status**: ✅ IMPLEMENTED  
**Feature**: Automatic fade transitions kada lipat ng room

---

## Overview

Lahat ng room transitions ay may fade in/fade out animation na:
- **Fade Out** - Screen nag-fade to black bago mag-load ng bagong scene
- **Fade In** - Screen nag-fade from black pagkatapos mag-load ng scene

---

## System Components

### 1. ScreenFader (Core System)
**Location**: `Assets/Scripts/GameManagement/ScreenFader.cs`

**Features**:
- Singleton pattern (DontDestroyOnLoad)
- Automatic fade in on scene start
- Customizable fade duration
- Blocks raycasts during fade (prevents accidental clicks)

**Key Methods**:
```csharp
ScreenFader.Instance.FadeOut(duration, onComplete);  // Fade to black
ScreenFader.Instance.FadeIn(duration, onComplete);   // Fade from black
ScreenFader.Instance.SetInstantBlack();              // Instant black
ScreenFader.Instance.SetInstantClear();              // Instant clear
```

**Settings**:
- `fadeImage` - UI Image component (usually full-screen black image)
- `defaultFadeDuration` - Default fade time (1 second)
- `fadeColor` - Color to fade to (default: black)
- `fadeInOnStart` - Auto fade in when scene loads (default: true)
- `startDelay` - Delay before auto fade in (default: 0.2s)

---

## Updated Scripts

### 1. ✅ RoomExit.cs (Trigger-based Transitions)
**Location**: `Assets/Scripts/Puzzle/Room 04/RoomExit.cs`

**What Changed**:
- Added fade out before scene load
- Disables player movement during transition
- Stops player velocity
- Saves player position
- Notifies SaveSystem

**New Settings**:
```csharp
[Header("Transition Settings")]
public float fadeOutDuration = 0.8f;
public float fadeInDuration = 0.8f;
public bool disablePlayerDuringTransition = true;
```

**Flow**:
```
Player enters trigger
    ↓
Disable player movement
    ↓
Fade out (0.8s)
    ↓
Save player position
    ↓
Load new scene
    ↓
Fade in (automatic in ScreenFader.Start())
```

---

### 2. ✅ LockedDoor.cs (Interactable Doors)
**Location**: `Assets/Scripts/GameManagement/LockedDoor.cs`

**What Changed**:
- Added fade transition option
- Disables player during transition
- Waits for door animation before fade

**New Settings**:
```csharp
[Header("Transition")]
public float fadeOutDuration = 0.8f;
public float fadeInDuration = 0.8f;
public bool useFadeTransition = true;
```

**Flow**:
```
Player interacts with door
    ↓
Check if unlocked/has key
    ↓
Play door open animation (0.5s)
    ↓
Disable player movement
    ↓
Fade out (0.8s)
    ↓
Load new scene
    ↓
Fade in (automatic)
```

---

### 3. ✅ UnifiedDoorInteraction.cs (Already Has Fade)
**Location**: `Assets/Scripts/GameManagement/UnifiedDoorInteraction.cs`

**Status**: Already implemented!

**Settings**:
```csharp
[Header("Transition")]
public float transitionDelay = 1.5f;
public bool useFadeTransition = true;
```

**Flow**:
```
Player interacts
    ↓
Check lock/requirements
    ↓
Play animations
    ↓
Fade out (60% of transitionDelay)
    ↓
Load scene
    ↓
Fade in (automatic)
```

---

## Unity Setup

### ScreenFader GameObject Setup

1. **Create ScreenFader** (if not exists):
   - Create Empty GameObject: `ScreenFader`
   - Add Component: `ScreenFader.cs`

2. **Create Fade Image**:
   - Right-click ScreenFader → UI → Image
   - Name it: `FadeImage`
   - Set to full screen:
     - Anchor: Stretch (all corners)
     - Left/Right/Top/Bottom: 0
   - Set color: Black (RGB: 0,0,0, Alpha: 255)
   - Set Raycast Target: ✅ (checked)

3. **Assign References**:
   - Select ScreenFader GameObject
   - Drag FadeImage to `Fade Image` field
   - Set `Default Fade Duration`: 1
   - Set `Fade Color`: Black
   - Check `Fade In On Start`: ✅
   - Set `Start Delay`: 0.2

4. **Canvas Settings**:
   - ScreenFader should be child of a Canvas
   - Canvas Render Mode: Screen Space - Overlay
   - Canvas Sort Order: 9999 (highest, so it's always on top)

5. **Important**:
   - ScreenFader must be in EVERY scene, OR
   - Use DontDestroyOnLoad (already implemented in code)

---

## Testing Checklist

### Basic Fade Test
- [ ] Start game → Screen fades in from black
- [ ] Walk to door/exit → Screen fades to black
- [ ] New scene loads → Screen fades in from black

### RoomExit (Trigger) Test
- [ ] Walk into trigger zone
- [ ] Player stops moving
- [ ] Screen fades to black (0.8s)
- [ ] New scene loads
- [ ] Screen fades in from black

### LockedDoor Test
- [ ] Interact with locked door → Shows locked message (no fade)
- [ ] Get key and interact → Door unlocks
- [ ] Door opens → Animation plays
- [ ] Screen fades to black (0.8s)
- [ ] New scene loads
- [ ] Screen fades in from black

### UnifiedDoorInteraction Test
- [ ] Interact with door
- [ ] Check requirements
- [ ] Door opens
- [ ] Screen fades to black
- [ ] New scene loads
- [ ] Screen fades in from black

---

## Customization

### Change Fade Duration
```csharp
// In RoomExit or LockedDoor Inspector:
fadeOutDuration = 1.5f;  // Slower fade (1.5 seconds)
fadeOutDuration = 0.5f;  // Faster fade (0.5 seconds)
```

### Change Fade Color
```csharp
// In ScreenFader Inspector:
fadeColor = Color.white;  // Fade to white instead of black
```

### Disable Fade for Specific Door
```csharp
// In LockedDoor Inspector:
useFadeTransition = false;  // No fade, instant transition
```

### Custom Fade Timing
```csharp
// In your script:
ScreenFader.Instance.FadeOut(2.0f, () => {
    // This runs after fade completes
    SceneManager.LoadScene("NextScene");
});
```

---

## Troubleshooting

### Problem: No Fade Happens
**Cause**: ScreenFader.Instance is null  
**Fix**: 
1. Check if ScreenFader GameObject exists in scene
2. Check if ScreenFader script is attached
3. Check Console for errors

### Problem: Fade Image Not Visible
**Cause**: Canvas sort order too low  
**Fix**: 
1. Select Canvas
2. Set Sort Order to 9999
3. Ensure FadeImage is child of Canvas

### Problem: Fade Blocks UI Forever
**Cause**: Fade didn't complete properly  
**Fix**: 
1. Check if fadeImage.raycastTarget is being set to false after fade
2. Call `ScreenFader.Instance.SetInstantClear()` to force clear

### Problem: Player Can Move During Fade
**Cause**: Player controller not disabled  
**Fix**: 
1. Check `disablePlayerDuringTransition = true` in RoomExit
2. Verify JoystickPlayerController is being found

### Problem: Fade In Doesn't Happen
**Cause**: `fadeInOnStart` is disabled  
**Fix**: 
1. Select ScreenFader GameObject
2. Check `Fade In On Start` ✅
3. Adjust `Start Delay` if needed

---

## Advanced Usage

### Manual Fade Control
```csharp
// Fade out, do something, then fade in
StartCoroutine(CustomFadeSequence());

IEnumerator CustomFadeSequence()
{
    // Fade to black
    ScreenFader.Instance.FadeOut(1.0f);
    yield return new WaitForSeconds(1.0f);
    
    // Do something (teleport player, change scene, etc.)
    player.transform.position = newPosition;
    
    // Fade back in
    ScreenFader.Instance.FadeIn(1.0f);
    yield return new WaitForSeconds(1.0f);
}
```

### Fade with Callback
```csharp
ScreenFader.Instance.FadeOut(1.0f, () => {
    Debug.Log("Fade out complete!");
    // Load scene or do something
});
```

### Check if Fading
```csharp
if (ScreenFader.Instance.IsFading())
{
    Debug.Log("Currently fading, wait...");
}
```

---

## Performance Notes

- Fade uses `Time.unscaledDeltaTime` so it works even when game is paused
- Raycast blocking prevents accidental clicks during transition
- Fade image is disabled when not in use (alpha = 0)
- No performance impact when not fading

---

## Summary

✅ **RoomExit.cs** - Added fade transitions for trigger-based exits  
✅ **LockedDoor.cs** - Added fade transitions for interactable doors  
✅ **UnifiedDoorInteraction.cs** - Already has fade transitions  
✅ **ScreenFader.cs** - Core system (already exists)  

**Lahat ng room transitions ay may smooth fade in/fade out na!** 🎬✨

---

## Related Files

- `Assets/Scripts/GameManagement/ScreenFader.cs` - Core fade system
- `Assets/Scripts/Puzzle/Room 04/RoomExit.cs` - Trigger-based transitions
- `Assets/Scripts/GameManagement/LockedDoor.cs` - Interactable doors
- `Assets/Scripts/GameManagement/UnifiedDoorInteraction.cs` - Unified door system
- `Assets/Scripts/Puzzle/Room 07/Room07_RugTransition.cs` - Room 7 specific
- `Assets/Scripts/Puzzle/Room 06.2/HallwayDoorInteraction.cs` - Hallway doors

---

## Next Steps

1. ✅ Test all room transitions
2. ✅ Verify fade durations feel good
3. ✅ Adjust timing if needed
4. ✅ Ensure ScreenFader exists in all scenes
