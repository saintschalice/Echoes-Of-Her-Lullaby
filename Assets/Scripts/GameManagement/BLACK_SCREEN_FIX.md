# Black Screen After Scene Transition - Fix

## PROBLEMA
Pagkalabas ng Living Room (o any room), nagiging black screen. Hindi nag-fade in.

---

## ROOT CAUSE

### Issue: ScreenFader Start() Only Runs Once

**Problem**:
```csharp
void Awake()
{
    DontDestroyOnLoad(gameObject); // ScreenFader persists
}

void Start()
{
    if (fadeInOnStart)
    {
        FadeIn(); // Only runs ONCE when first created!
    }
}
```

**What Happens**:
1. ✅ First scene loads → ScreenFader created → Start() runs → Fades in
2. ❌ Player exits to next scene → ScreenFader persists → Start() DOESN'T run → Stays black!

**Why**: `Start()` only runs once per GameObject lifetime. Since ScreenFader uses `DontDestroyOnLoad`, it never gets destroyed, so `Start()` never runs again.

---

## ✅ SOLUTION

### Subscribe to Scene Load Events

Added `SceneManager.sceneLoaded` event listener:

```csharp
using UnityEngine.SceneManagement; // Added

void Awake()
{
    // ... existing code ...
    
    // CRITICAL FIX: Subscribe to scene loaded event
    SceneManager.sceneLoaded += OnSceneLoaded;
}

void OnDestroy()
{
    // Unsubscribe when destroyed
    SceneManager.sceneLoaded -= OnSceneLoaded;
}

// CRITICAL FIX: Fade in automatically when a new scene loads
void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    if (fadeInOnSceneLoad && fadeImage != null)
    {
        Debug.Log($"[ScreenFader] Scene loaded: {scene.name}, fading in...");
        
        // Make sure we start from black
        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1f);
        
        // Fade in after a short delay
        Invoke(nameof(FadeInOnStart), startDelay);
    }
}
```

---

## HOW IT WORKS NOW

### Scene Transition Flow:

1. **Player exits room**:
   ```
   RoomExit.OnTriggerEnter2D()
   → ScreenFader.FadeOut() (fade to black)
   → SceneManager.LoadScene()
   ```

2. **New scene loads**:
   ```
   SceneManager.sceneLoaded event fires
   → ScreenFader.OnSceneLoaded() called
   → Sets fadeImage to black
   → Invokes FadeIn() after delay
   → Screen fades from black to clear ✅
   ```

3. **Player can see and move**:
   ```
   ✅ Screen is clear
   ✅ Player controls enabled
   ✅ No black screen
   ```

---

## TESTING

### Test 1: Living Room → Hallway
```
1. Start in Living Room
2. Walk to exit
3. Expected: Fade to black
4. Expected: Load next scene
5. Expected: Fade from black to clear ✅
6. Expected: Can see and move
```

### Test 2: Multiple Transitions
```
1. Room 1 → Room 2 (should fade in)
2. Room 2 → Room 3 (should fade in)
3. Room 3 → Room 4 (should fade in)
4. All transitions should work ✅
```

### Test 3: First Scene Load
```
1. Start game from main menu
2. Load first scene
3. Expected: Fade in works ✅
```

---

## CONFIGURATION

### In Inspector (ScreenFader GameObject):

```
ScreenFader Component:
├── Fade Settings
│   ├── Fade Image: [Assign FadeImage]
│   ├── Default Fade Duration: 1
│   └── Fade Color: Black
├── Auto Fade In on Start
│   ├── Fade In On Start: ✓ (checked)
│   └── Start Delay: 0.2
└── Auto Fade on Scene Load
    └── Fade In On Scene Load: ✓ (checked) ⭐ NEW
```

**Important**: Make sure `Fade In On Scene Load` is checked!

---

## RELATED ISSUES

### Issue 1: ScreenFader Not Found
**Symptom**: Console error "ScreenFader not found!"
**Fix**: Add ScreenFader to scene (see SCREENFADER_ROOM05_SETUP.md)

### Issue 2: Fade Image Not Assigned
**Symptom**: Black screen, no fade
**Fix**: Assign FadeImage to ScreenFader component

### Issue 3: Canvas Sort Order Too Low
**Symptom**: Fade doesn't cover everything
**Fix**: Set Canvas Sort Order to 1000

---

## FILES MODIFIED

✅ `Assets/Scripts/GameManagement/ScreenFader.cs`
- Added `using UnityEngine.SceneManagement`
- Added `fadeInOnSceneLoad` bool
- Added `OnSceneLoaded()` method
- Added `OnDestroy()` to unsubscribe
- Subscribes to `SceneManager.sceneLoaded` event

---

## COMMON MISTAKES

### ❌ WRONG: Relying on Start() for DontDestroyOnLoad objects
```csharp
void Start()
{
    FadeIn(); // Only runs once!
}
```

### ✅ RIGHT: Using scene load events
```csharp
void Awake()
{
    SceneManager.sceneLoaded += OnSceneLoaded;
}

void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    FadeIn(); // Runs every scene load!
}
```

### ❌ WRONG: Forgetting to unsubscribe
```csharp
// Missing OnDestroy()
// Memory leak!
```

### ✅ RIGHT: Unsubscribe in OnDestroy
```csharp
void OnDestroy()
{
    SceneManager.sceneLoaded -= OnSceneLoaded;
}
```

---

## DEBUG COMMANDS

### Check if ScreenFader Exists
```csharp
Debug.Log(ScreenFader.Instance != null);
```

### Check Fade Image Alpha
```csharp
Debug.Log(ScreenFader.Instance.fadeImage.color.a);
// 0 = transparent (clear)
// 1 = opaque (black)
```

### Manually Fade In
```csharp
ScreenFader.Instance.FadeIn();
```

### Check Scene Load Events
```csharp
// In ScreenFader.OnSceneLoaded():
Debug.Log($"Scene loaded: {scene.name}");
```

---

## VERIFICATION CHECKLIST

After applying fix:

- [ ] ScreenFader exists in scene (or DontDestroyOnLoad)
- [ ] Fade Image assigned
- [ ] Canvas Sort Order = 1000
- [ ] Fade In On Scene Load = ✓ (checked)
- [ ] Test scene transition (should fade in)
- [ ] No black screen after transition
- [ ] Console shows "Scene loaded: [name], fading in..."

---

## SUMMARY

### What Was Wrong
- ScreenFader's Start() only ran once
- After first scene, Start() never ran again
- Screen stayed black after transitions

### What Was Fixed
- Added SceneManager.sceneLoaded event listener
- OnSceneLoaded() runs every time a scene loads
- Automatically fades in on every scene transition

### Expected Behavior
- ✅ Fade out when exiting room
- ✅ Load new scene
- ✅ Fade in automatically
- ✅ Player can see and move

---

## ADDITIONAL NOTES

### Why Not Use OnEnable()?
```csharp
void OnEnable()
{
    FadeIn(); // This would run too often!
}
```
**Problem**: OnEnable() runs when:
- GameObject is enabled
- Component is enabled
- Scene loads
- GameObject is activated

This could cause unwanted fades.

### Why Scene Load Events?
```csharp
SceneManager.sceneLoaded += OnSceneLoaded;
```
**Benefits**:
- ✅ Runs exactly when scene loads
- ✅ Provides scene info
- ✅ Reliable timing
- ✅ Standard Unity pattern

---

**Status**: ✅ FIXED
**Developer**: Jhon Jellar Z. Miranda
**Date**: May 4, 2026
**Bug**: Black screen after scene transition
**Fix**: Added scene load event listener to ScreenFader
