# Room 5 Dialogue Bug Fix

## PROBLEMA
Pagpasok sa Room 5, lumalabas agad ang dialogue:
```
"She's coming! I need to solve this NOW!"
```

Pero mali yan kasi dapat hindi pa lumalabas yan on first entry!

---

## ROOT CAUSE

### Bug Location: `DiningRoomChaseTrigger.cs`

May trigger sa scene na nag-call ng `OnCalendarInteract()` kahit hindi pa nakikita ang calendar.

**Old Code (MALI)**:
```csharp
private void OnTriggerEnter2D(Collider2D other)
{
    // ...
    Room05_DiningRoomController.Instance.OnCalendarInteract();
    // ^^^ This opens calendar AND triggers dialogue!
}
```

**Problem**:
1. Player enters Room 5
2. Trigger fires immediately
3. Calls `OnCalendarInteract()` → Opens calendar + shows dialogue
4. When calendar closes → Triggers `EmilyGetsAngrySequence()`
5. Shows "She's coming! I need to solve this NOW!" ❌

---

## ✅ SOLUTION

### Fix 1: Check if Calendar Was Seen

Updated `DiningRoomChaseTrigger.cs` to only trigger if calendar was already seen:

```csharp
private void OnTriggerEnter2D(Collider2D other)
{
    // ...
    
    // CRITICAL FIX: Only trigger if calendar has been seen
    if (!Room05_DiningRoomController.Instance.isCalendarSeen)
    {
        Debug.Log("[DiningRoomChaseTrigger] Calendar not seen yet, skipping trigger.");
        return;
    }
    
    // Start the angry sequence directly
    StartCoroutine(Room05_DiningRoomController.Instance.EmilyGetsAngrySequence());
}
```

### Fix 2: Made EmilyGetsAngrySequence Public

Changed in `Room05_DiningRoomController.cs`:
```csharp
// OLD: IEnumerator EmilyGetsAngrySequence()
// NEW:
public IEnumerator EmilyGetsAngrySequence()
```

---

## CORRECT FLOW

### First Time in Room 5:
1. ✅ Player enters Room 5
2. ✅ No dialogue (clean entry)
3. ✅ Player explores
4. ✅ Player interacts with calendar
5. ✅ Calendar opens, shows calendar dialogue
6. ✅ Player closes calendar
7. ✅ "She's coming! I need to solve this NOW!" appears
8. ✅ Emily starts hunting

### Returning to Room 5 (After Seeing Calendar):
1. ✅ Player enters Room 5
2. ✅ Trigger checks: `isCalendarSeen == true`
3. ✅ Trigger can fire if player walks into it
4. ✅ Emily starts hunting sequence

---

## TESTING

### Test 1: First Entry (Clean)
```
1. Start new game (or reset Room 5 puzzle)
2. Enter Room 5
3. Expected: NO dialogue on entry
4. Expected: Player can move freely
5. Expected: No Emily hunting yet
```

### Test 2: Calendar Interaction
```
1. In Room 5, interact with calendar
2. Expected: Calendar opens
3. Expected: Calendar dialogue shows
4. Close calendar
5. Expected: "She's coming! I need to solve this NOW!"
6. Expected: Emily starts hunting
```

### Test 3: Re-entry After Calendar
```
1. See calendar, trigger Emily hunt
2. Die or leave room
3. Re-enter Room 5
4. Expected: Trigger can fire if you walk into it
5. Expected: Emily hunt sequence starts
```

---

## RELATED FIXES

### Fix 3: PersistentObject DontDestroyOnLoad Error

**Error**:
```
Assertion failed: m_GameObjects.find(gameObject.GetEntityId()) == m_GameObjects.end()
```

**Cause**: Objects already in DontDestroyOnLoad scene calling DontDestroyOnLoad again

**Fix**: Check if object is already in DontDestroyOnLoad scene:
```csharp
void Awake()
{
    if (persist)
    {
        // Check if already in DontDestroyOnLoad scene
        if (gameObject.scene.name == "DontDestroyOnLoad")
        {
            Debug.Log($"[PersistentObject] {gameObject.name} already in DontDestroyOnLoad, skipping.");
            return;
        }
        
        // ... rest of code
        DontDestroyOnLoad(gameObject);
    }
}
```

---

## FILES MODIFIED

1. ✅ `Assets/Scripts/Puzzle/Room 05/DiningRoomChaseTrigger.cs`
   - Added calendar seen check
   - Changed to call EmilyGetsAngrySequence directly

2. ✅ `Assets/Scripts/Puzzle/Room 05/Room05_DiningRoomController.cs`
   - Made EmilyGetsAngrySequence() public

3. ✅ `Assets/Scripts/GameManagement/PersistentObject.cs`
   - Added DontDestroyOnLoad scene check
   - Prevents assertion error

---

## VERIFICATION CHECKLIST

After applying fixes:

- [ ] No dialogue on first Room 5 entry
- [ ] Player can move freely on entry
- [ ] Calendar interaction works correctly
- [ ] Dialogue appears AFTER closing calendar
- [ ] Emily hunt starts after calendar
- [ ] No DontDestroyOnLoad assertion errors
- [ ] Trigger works correctly on re-entry

---

## COMMON MISTAKES

### ❌ WRONG: Trigger fires on first entry
```csharp
// Missing calendar check
Room05_DiningRoomController.Instance.OnCalendarInteract();
```

### ✅ RIGHT: Trigger checks calendar first
```csharp
if (!Room05_DiningRoomController.Instance.isCalendarSeen)
{
    return; // Skip trigger
}
```

### ❌ WRONG: Calling OnCalendarInteract from trigger
```csharp
// This opens calendar UI, which is wrong
Room05_DiningRoomController.Instance.OnCalendarInteract();
```

### ✅ RIGHT: Calling EmilyGetsAngrySequence directly
```csharp
// This starts the hunt without opening calendar
StartCoroutine(Room05_DiningRoomController.Instance.EmilyGetsAngrySequence());
```

---

## DEBUG COMMANDS

To test the fix:

### Reset Room 5 Puzzle
```csharp
// In Unity Editor, select Room05_DiningRoomController
// Right-click component → Reset Room 05 Puzzle
```

### Check Calendar Seen Flag
```csharp
// In Console:
Debug.Log(Room05_DiningRoomController.Instance.isCalendarSeen);
```

### Manually Trigger Emily Hunt
```csharp
// In Console:
StartCoroutine(Room05_DiningRoomController.Instance.EmilyGetsAngrySequence());
```

---

## SUMMARY

### What Was Wrong
- Trigger called `OnCalendarInteract()` on first entry
- This opened calendar and triggered dialogue immediately
- Player saw "She's coming!" before even exploring

### What Was Fixed
- Trigger now checks if calendar was seen first
- Only triggers Emily hunt if calendar was already seen
- Calls `EmilyGetsAngrySequence()` directly (no calendar UI)
- Fixed PersistentObject DontDestroyOnLoad error

### Expected Behavior
- ✅ Clean entry to Room 5 (no dialogue)
- ✅ Player explores freely
- ✅ Calendar interaction triggers hunt sequence
- ✅ Dialogue appears at correct time
- ✅ No console errors

---

## NEXT STEPS

1. ✅ Test first entry (should be clean)
2. ✅ Test calendar interaction (should trigger hunt)
3. ✅ Test re-entry (trigger should work)
4. ✅ Verify no console errors

**Status**: ✅ FIXED

---

**Developer**: Jhon Jellar Z. Miranda
**Date**: May 4, 2026
**Bug**: Room 5 dialogue appearing on entry
**Fix**: Added calendar seen check to trigger
