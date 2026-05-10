# ScreenFader Setup Guide - Step by Step

**Para sa**: Fade in/fade out transitions kada lipat ng room  
**Kailangan**: ScreenFader GameObject sa PersistentScene

---

## Saan Ilalagay ang ScreenFader?

### ✅ ANSWER: Sa **PersistentScene** (DontDestroyOnLoad)

**Bakit?**
- ScreenFader ay persistent across all scenes
- Kailangan niya mag-survive kada scene change
- Dapat laging available para sa fade transitions

---

## Step-by-Step Setup

### Step 1: Open PersistentScene

1. Sa Unity, open ang **PersistentScene**
2. Ito ang scene na may:
   - Lisa (Player)
   - Main Camera
   - PersistentSpawnManager
   - GameManager
   - SaveSystem
   - Etc.

---

### Step 2: Create ScreenFader GameObject

1. **Right-click** sa Hierarchy (sa PersistentScene)
2. Create Empty GameObject
3. Name it: **`ScreenFader`**
4. Position: (0, 0, 0) - doesn't matter, UI lang naman

---

### Step 3: Add ScreenFader Script

1. Select ang **ScreenFader** GameObject
2. Sa Inspector, click **Add Component**
3. Search: **`ScreenFader`**
4. Click to add the script

---

### Step 4: Create Canvas (if wala pa)

**Check muna kung may Canvas na sa PersistentScene:**
- Kung may **PersistentUI** Canvas na → Use that
- Kung wala → Create new Canvas

**To create new Canvas:**
1. Right-click sa Hierarchy
2. UI → Canvas
3. Name it: **`PersistentUI`** or **`ScreenFaderCanvas`**
4. Canvas settings:
   - Render Mode: **Screen Space - Overlay**
   - Pixel Perfect: ✅ (optional)
   - Sort Order: **9999** (para laging nasa top)

---

### Step 5: Create Fade Image

1. **Right-click** sa Canvas (or ScreenFader GameObject)
2. UI → Image
3. Name it: **`FadeImage`**

**Configure FadeImage:**

#### A. RectTransform (para full screen)
- Anchor Presets: Click **Stretch** (bottom-right icon)
- Or manually set:
  - Anchor Min: (0, 0)
  - Anchor Max: (1, 1)
  - Left: 0
  - Right: 0
  - Top: 0
  - Bottom: 0

#### B. Image Component
- Source Image: **None** (or any 1x1 white sprite)
- Color: **Black** (R:0, G:0, B:0, A:255)
- Raycast Target: ✅ **Checked** (important!)

---

### Step 6: Assign References

1. Select **ScreenFader** GameObject
2. Sa Inspector, sa **ScreenFader** component:

**Drag and drop:**
- **Fade Image**: Drag ang `FadeImage` GameObject dito
- **Default Fade Duration**: `1` (1 second)
- **Fade Color**: Black (0, 0, 0, 255)
- **Fade In On Start**: ✅ **Checked**
- **Start Delay**: `0.2` (0.2 seconds)

---

### Step 7: Hierarchy Structure

Dapat ganito ang structure:

```
PersistentScene
├── Lisa (Player)
├── Main Camera
├── PersistentSpawnManager
├── GameManager
├── SaveSystem
├── PersistentUI (Canvas) ← Sort Order: 9999
│   ├── ScreenFader
│   │   └── FadeImage ← Full screen black image
│   ├── Joystick
│   ├── InventoryButton
│   └── ... (other UI)
```

**OR** (if separate Canvas):

```
PersistentScene
├── Lisa (Player)
├── Main Camera
├── ... (other objects)
├── ScreenFaderCanvas (Canvas) ← Sort Order: 9999
│   └── FadeImage ← Full screen black image
├── ScreenFader ← Script component (sibling of Canvas)
```

---

## Visual Guide

### Inspector Settings - ScreenFader Component

```
┌─────────────────────────────────────┐
│ ScreenFader (Script)                │
├─────────────────────────────────────┤
│ Fade Settings                       │
│ ├─ Fade Image: [FadeImage]         │ ← Drag FadeImage here
│ ├─ Default Fade Duration: 1        │
│ └─ Fade Color: ■ Black             │
│                                     │
│ Auto Fade In on Start               │
│ ├─ Fade In On Start: ✅            │
│ └─ Start Delay: 0.2                │
└─────────────────────────────────────┘
```

### Inspector Settings - FadeImage

```
┌─────────────────────────────────────┐
│ Image (Script)                      │
├─────────────────────────────────────┤
│ Source Image: None                  │
│ Color: ■ Black (0,0,0,255)         │
│ Material: None                      │
│ Raycast Target: ✅                  │ ← IMPORTANT!
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│ Rect Transform                      │
├─────────────────────────────────────┤
│ Anchors: Stretch (all corners)      │
│ Left: 0    Right: 0                 │
│ Top: 0     Bottom: 0                │
└─────────────────────────────────────┘
```

---

## Testing

### Test 1: Scene Start Fade In
1. Play ang PersistentScene
2. ✅ Screen dapat mag-fade in from black
3. ✅ After 0.2s delay, fade in starts
4. ✅ After 1 second, fully visible

### Test 2: Manual Fade Out
1. Sa Console, type:
   ```csharp
   ScreenFader.Instance.FadeOut(1.0f);
   ```
2. ✅ Screen dapat mag-fade to black

### Test 3: Manual Fade In
1. Sa Console, type:
   ```csharp
   ScreenFader.Instance.FadeIn(1.0f);
   ```
2. ✅ Screen dapat mag-fade from black

### Test 4: Room Transition
1. Play the game
2. Walk to a door/exit
3. ✅ Screen fades to black
4. ✅ New room loads
5. ✅ Screen fades in from black

---

## Troubleshooting

### Problem: Walang Fade
**Symptoms**: Screen doesn't fade, instant transitions  
**Causes**:
1. ScreenFader.Instance is null
2. FadeImage not assigned
3. Canvas disabled

**Fix**:
1. Check if ScreenFader GameObject exists
2. Check if FadeImage is assigned in Inspector
3. Check if Canvas is enabled
4. Check Console for errors

---

### Problem: Fade Image Not Visible
**Symptoms**: Fade doesn't cover screen  
**Causes**:
1. Canvas Sort Order too low
2. FadeImage not full screen
3. FadeImage color is transparent

**Fix**:
1. Set Canvas Sort Order to 9999
2. Set FadeImage anchors to Stretch
3. Set FadeImage color to Black (A:255)

---

### Problem: Fade Blocks UI Forever
**Symptoms**: Screen stays black, can't interact  
**Causes**:
1. Fade didn't complete
2. Raycast Target not being disabled

**Fix**:
1. Check if fade duration is reasonable (not 0 or negative)
2. Call `ScreenFader.Instance.SetInstantClear()` to force clear
3. Check if `fadeImage.raycastTarget` is being set to false after fade

---

### Problem: Multiple ScreenFaders
**Symptoms**: Fade happens twice, or errors about duplicate instances  
**Causes**:
1. ScreenFader exists in multiple scenes
2. ScreenFader not using DontDestroyOnLoad

**Fix**:
1. Remove ScreenFader from all other scenes
2. Keep only ONE ScreenFader in PersistentScene
3. Verify script has DontDestroyOnLoad in Awake()

---

## Advanced: Multiple Scenes

### If you have multiple starting scenes:

**Option 1: PersistentScene Always Loads First (Recommended)**
- MainMenu → PersistentScene → Room scenes
- ScreenFader stays in PersistentScene
- All transitions use ScreenFader.Instance

**Option 2: ScreenFader in Every Scene**
- Add ScreenFader to MainMenu scene
- Add ScreenFader to PersistentScene
- Singleton pattern ensures only one exists
- First one loaded survives (DontDestroyOnLoad)

---

## Code Examples

### Fade Out Then Load Scene
```csharp
IEnumerator TransitionToScene(string sceneName)
{
    // Fade out
    ScreenFader.Instance.FadeOut(0.8f);
    yield return new WaitForSeconds(0.8f);
    
    // Load scene
    SceneManager.LoadScene(sceneName);
    
    // Fade in happens automatically in ScreenFader.Start()
}
```

### Fade Out with Callback
```csharp
ScreenFader.Instance.FadeOut(1.0f, () => {
    Debug.Log("Fade out complete!");
    SceneManager.LoadScene("NextScene");
});
```

### Check if Fading
```csharp
if (ScreenFader.Instance.IsFading())
{
    Debug.Log("Currently fading, please wait...");
    return;
}
```

### Instant Black (for cutscenes)
```csharp
ScreenFader.Instance.SetInstantBlack();
// ... play cutscene ...
ScreenFader.Instance.FadeIn(1.0f);
```

---

## Summary Checklist

Setup checklist:
- [ ] ScreenFader GameObject created in PersistentScene
- [ ] ScreenFader script added
- [ ] Canvas exists with Sort Order 9999
- [ ] FadeImage created (full screen, black)
- [ ] FadeImage assigned to ScreenFader script
- [ ] Fade In On Start is checked
- [ ] Tested fade in on scene start
- [ ] Tested room transitions

---

## Related Files

- `Assets/Scripts/GameManagement/ScreenFader.cs` - Main script
- `Assets/Scripts/Puzzle/Room 04/RoomExit.cs` - Uses fade
- `Assets/Scripts/GameManagement/LockedDoor.cs` - Uses fade
- `Assets/Scripts/GameManagement/UnifiedDoorInteraction.cs` - Uses fade

---

## Quick Reference

**Location**: PersistentScene  
**GameObject**: ScreenFader  
**Child**: FadeImage (full screen black)  
**Canvas Sort Order**: 9999  
**Script**: ScreenFader.cs  

**Settings**:
- Fade Duration: 1 second
- Fade Color: Black
- Fade In On Start: ✅
- Start Delay: 0.2s

**Usage**:
```csharp
ScreenFader.Instance.FadeOut(duration);
ScreenFader.Instance.FadeIn(duration);
```

---

**Tapos na! Ready na ang ScreenFader para sa smooth transitions!** 🎬✨
