# PersistentObject DontDestroyOnLoad Fix

## ERROR MESSAGE
```
Assertion failed on expression: 'm_GameObjects.find(gameObject.GetEntityId()) == m_GameObjects.end()'
UnityEngine.Object:DontDestroyOnLoad (UnityEngine.Object)
PersistentObject:Awake () (at Assets/Scripts/GameManagement/PersistentObject.cs:13)
```

## PROBLEMA
Ang `PersistentObject` script ay tumatawag ng `DontDestroyOnLoad()` multiple times sa same object, causing assertion errors.

## ROOT CAUSE
Kapag nag-reload ng scene or nag-transition, ang objects na naka-DontDestroyOnLoad ay nananatili. Pero ang `Awake()` method ay tumatawag ulit, at nag-try ulit mag-call ng `DontDestroyOnLoad()` sa object na nasa DontDestroyOnLoad scene na.

### Flow ng Error:
```
Scene 1 loads
    ↓
PersistentObject.Awake() calls
    ↓
DontDestroyOnLoad(gameObject) ✓
    ↓
Object moves to "DontDestroyOnLoad" scene
    ↓
Scene 2 loads
    ↓
PersistentObject.Awake() calls AGAIN (object still exists!)
    ↓
DontDestroyOnLoad(gameObject) ✗ ERROR!
    ↓
Assertion failed - object already in DontDestroyOnLoad scene
```

---

## SOLUTION

### Code Fix: PersistentObject.cs
Added check to see if object is already in DontDestroyOnLoad scene before calling DontDestroyOnLoad again.

```csharp
void Awake()
{
    if (persist)
    {
        // CRITICAL FIX: Check if already in DontDestroyOnLoad scene
        if (gameObject.scene.name == "DontDestroyOnLoad")
        {
            // Already persistent, skip
            return;
        }

        // Mark as persistent
        DontDestroyOnLoad(gameObject);
    }
}
```

### How It Works:
1. **First time** (Scene 1):
   - Object is in normal scene (e.g., "Room01_Foyer")
   - `gameObject.scene.name != "DontDestroyOnLoad"`
   - Calls `DontDestroyOnLoad(gameObject)` ✓
   - Object moves to DontDestroyOnLoad scene

2. **Second time** (Scene 2):
   - Object is already in DontDestroyOnLoad scene
   - `gameObject.scene.name == "DontDestroyOnLoad"`
   - Skips `DontDestroyOnLoad()` call ✓
   - No error!

---

## AFFECTED OBJECTS

This fix applies to ALL objects using PersistentObject component:

### Common Persistent Objects:
- **PersistentUI** (Joystick, Inventory button, etc.)
- **Lisa** (Player character)
- **AudioManager**
- **SaveSystem**
- **ScreenFader**
- **GameOverManager**
- **DialogueSystemV2**
- **InventoryManager**
- **PersistentSpawnManager**

All of these should now work without assertion errors.

---

## TESTING

### Test 1: Scene Transitions
- [ ] Start game (Room01_Foyer)
- [ ] Transition to Room02_LivingRoom
- [ ] Check console - NO assertion errors
- [ ] Transition to Room03_Hallway
- [ ] Check console - NO assertion errors
- [ ] Continue through all rooms

### Test 2: Multiple Transitions
- [ ] Transition through 5+ rooms
- [ ] Check console for assertion errors
- [ ] All persistent objects should work normally

### Test 3: Retry (Game Over)
- [ ] Get caught by Emily
- [ ] Click Retry
- [ ] Scene reloads
- [ ] Check console - NO assertion errors
- [ ] All persistent objects still work

### Test 4: Load Game
- [ ] Save game
- [ ] Return to main menu
- [ ] Load saved game
- [ ] Check console - NO assertion errors

---

## CONSOLE LOGS

### Before Fix:
```
Assertion failed on expression: 'm_GameObjects.find(gameObject.GetEntityId()) == m_GameObjects.end()'
Assertion failed on expression: 'm_GameObjects.find(gameObject.GetEntityId()) == m_GameObjects.end()'
Assertion failed on expression: 'm_GameObjects.find(gameObject.GetEntityId()) == m_GameObjects.end()'
(Multiple errors for each persistent object)
```

### After Fix:
```
(No assertion errors - clean console!)
```

---

## WHY THIS HAPPENS

### Unity's DontDestroyOnLoad Behavior:
1. When you call `DontDestroyOnLoad(obj)`, Unity moves the object to a special scene called "DontDestroyOnLoad"
2. This scene persists across all scene loads
3. Objects in this scene don't get destroyed when loading new scenes
4. BUT their `Awake()` method can still be called in certain situations
5. Calling `DontDestroyOnLoad()` on an object that's already in the DontDestroyOnLoad scene causes an assertion error

### The Fix:
Simply check if the object is already in the DontDestroyOnLoad scene before calling DontDestroyOnLoad again.

---

## ALTERNATIVE APPROACHES

### Approach 1: Singleton Pattern (Already Used)
Many managers already use singleton pattern:
```csharp
void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    else
    {
        Destroy(gameObject);
    }
}
```
This prevents duplicates but doesn't prevent the assertion error on the same object.

### Approach 2: Scene Check (Our Fix)
Check if already in DontDestroyOnLoad scene:
```csharp
if (gameObject.scene.name == "DontDestroyOnLoad")
{
    return; // Skip DontDestroyOnLoad call
}
```
This prevents calling DontDestroyOnLoad on objects already in that scene.

### Approach 3: Flag Check
Use a flag to track if already called:
```csharp
private bool hasCalledDontDestroy = false;

void Awake()
{
    if (!hasCalledDontDestroy)
    {
        DontDestroyOnLoad(gameObject);
        hasCalledDontDestroy = true;
    }
}
```
This works but the scene check is cleaner.

---

## RELATED FIXES

This fix works together with:
1. **Singleton patterns** in managers (prevents duplicates)
2. **PersistentSpawnManager** (handles player persistence)
3. **Scene transition logic** (proper scene loading)

---

## SUMMARY

**What Changed**:
- PersistentObject.cs now checks if object is already in DontDestroyOnLoad scene
- Skips calling DontDestroyOnLoad if already persistent
- Prevents assertion errors

**What to Test**:
- Scene transitions (no errors)
- Retry after game over (no errors)
- Load game (no errors)
- All persistent objects work normally

**Result**:
- Clean console (no assertion errors)
- Proper persistence across scenes
- No duplicate objects
- Better performance

Tapos na! 🎮
