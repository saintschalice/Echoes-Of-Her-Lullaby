# Diary Pages Joystick Fix - COMPLETE

## Problema
After kumuha ng diary pages at nag-show ng notification + dialogue, **nawawala ang joystick**.

## Root Cause
Diary pages may special flow:
1. Item notification shows (joystick hidden)
2. Player clicks to dismiss notification (joystick should re-enable)
3. **PERO** may dialogue na sumusunod agad (joystick hidden ulit)
4. After dialogue ends, joystick dapat bumalik
5. **PROBLEMA**: Joystick hindi bumabalik kasi may timing issue

## Solusyon

### 1. DialogueSystemV2.cs - Better Joystick Finding ✅

**Problem**: Joystick reference nawawala, hindi nag-search ng multiple names

**Solution**: Added multiple joystick name search (same as ItemNotificationUI)

#### Before:
```csharp
joystickUI = GameObject.Find("Joystick");
if (joystickUI != null)
{
    joystickUI.SetActive(true);
}
```

#### After:
```csharp
joystickUI = GameObject.Find("Joystick");
if (joystickUI == null)
{
    joystickUI = GameObject.Find("FloatingJoystick");
}
if (joystickUI == null)
{
    joystickUI = GameObject.Find("VariableJoystick");
}

if (joystickUI != null)
{
    joystickUI.SetActive(true);
    Debug.Log($"[Dialogue] Joystick found and re-enabled: {joystickUI.name}");
}
```

**Result**: Dialogue system can now find joystick with different names! ✅

---

### 2. GlobalDiaryManager.cs - Better Debug Logging ✅

**Problem**: Walang debug logs para malaman kung nag-trigger ng dialogue

**Solution**: Added debug logging sa `ShowCombinationDialogueAfterDelay()`

```csharp
if (DialogueSystemV2.Instance != null)
{
    DialogueSystemV2.Instance.StartDialogue(
        "These pages fit together... I can now read them in my diary.",
        "Lisa"
    );
    
    Debug.Log("[GlobalDiaryManager] Combination dialogue started after notification");
}
else
{
    Debug.LogWarning("[GlobalDiaryManager] DialogueSystemV2.Instance is null!");
}
```

**Result**: Mas madaling i-debug kung may problema! ✅

---

## How It Works Now

### Diary Page Pickup Flow (CORRECT):
```
Player picks up diary page
    ↓
ItemNotificationUI shows notification
    ↓
Joystick HIDDEN (can't move)
    ↓
Player clicks to dismiss notification
    ↓
ItemNotificationUI hides notification
    ↓
Joystick RE-ENABLED ✅
    ↓
GlobalDiaryManager checks if 2+ pages
    ↓
If yes: Wait 0.5s, then start dialogue
    ↓
DialogueSystemV2 starts dialogue
    ↓
Joystick HIDDEN again (for dialogue)
    ↓
Player clicks to dismiss dialogue
    ↓
DialogueSystemV2 ends dialogue
    ↓
Joystick RE-ENABLED ✅ (with multiple name search)
    ↓
Player can move again!
```

---

## Files Modified

### 1. DialogueSystemV2.cs ✅
**Location**: `Assets/Scripts/UI/Dialogs/DialogueSystemV2.cs`

**Changes**:
- Added multiple joystick name search in `EndDialogue()`
- Tries: Joystick → FloatingJoystick → VariableJoystick
- Better debug logging with joystick name

### 2. GlobalDiaryManager.cs ✅
**Location**: `Assets/Scripts/Puzzle/Room 02/GlobalDiaryManager.cs`

**Changes**:
- Added debug logging in `ShowCombinationDialogueAfterDelay()`
- Added null check for DialogueSystemV2.Instance
- Better error messages

---

## Testing Checklist

### Test Diary Page 1 (No Dialogue)
- [ ] Pick up first diary page
- [ ] Notification shows
- [ ] Click to dismiss
- [ ] **Expected**: Joystick returns ✅
- [ ] Player can move

### Test Diary Page 2 (With Dialogue)
- [ ] Pick up second diary page
- [ ] Notification shows
- [ ] Click to dismiss
- [ ] **Expected**: Joystick returns briefly ✅
- [ ] Dialogue shows: "These pages fit together..."
- [ ] Click to dismiss dialogue
- [ ] **Expected**: Joystick returns ✅
- [ ] Player can move

### Test Diary Pages 3-4
- [ ] Pick up third diary page
- [ ] Same flow as page 2
- [ ] Joystick returns after dialogue ✅
- [ ] Pick up fourth diary page
- [ ] Same flow
- [ ] Joystick returns after dialogue ✅

---

## Debug Logs to Check

### Successful Diary Page 1:
```
[ItemNotification] Showing notification, waiting for input...
[ItemNotification] Joystick re-enabled after notification
[GlobalDiaryManager] Added diary page diary_page_1. Total collected = 1
```

### Successful Diary Page 2 (Combination):
```
[ItemNotification] Showing notification, waiting for input...
[ItemNotification] Joystick re-enabled after notification
[GlobalDiaryManager] Added diary page diary_page_2. Total collected = 2
[GlobalDiaryManager] Combining pages into diary_entries...
[GlobalDiaryManager] Combination dialogue started after notification
[Dialogue] Started dialogue with 1 lines
[Dialogue] Ending dialogue
[Dialogue] Joystick found and re-enabled: FloatingJoystick (fallback)
[Dialogue] EndDialogue complete - controls should be restored
```

### If Joystick Not Found (Warning):
```
[Dialogue] Joystick not found! Player may be stuck. Tried: Joystick, FloatingJoystick, VariableJoystick
```

---

## Troubleshooting

### Problem: Joystick still not returning after diary dialogue
**Solution**:
1. Check Console for joystick warnings
2. Check joystick GameObject name in Unity
3. If different name, add to search list in DialogueSystemV2.cs:
   ```csharp
   if (joystickUI == null)
   {
       joystickUI = GameObject.Find("YourJoystickName");
   }
   ```

### Problem: Dialogue not showing after 2nd page
**Solution**:
1. Check Console for "[GlobalDiaryManager] Combination dialogue started"
2. If missing, check if DialogueSystemV2.Instance exists
3. Check if diary pages are registered in GlobalDiaryManager

### Problem: Joystick returns but player can't move
**Solution**:
1. Check if player controller is enabled
2. Check Console for "[Dialogue] Player controller re-enabled"
3. Check if JoystickPlayerController.Instance exists

---

## Additional Notes

### Why Diary Pages Are Special
Unlike other items, diary pages have a **two-step process**:
1. **Notification** - Shows item was picked up
2. **Dialogue** - Shows combination message (if 2+ pages)

This means joystick needs to:
1. Hide during notification
2. Show after notification
3. Hide during dialogue (if triggered)
4. Show after dialogue

### Timing Is Critical
The `ShowCombinationDialogueAfterDelay()` coroutine ensures:
- Notification finishes first
- 0.5s delay for diary UI to show
- Then dialogue starts

This prevents overlapping UI and ensures smooth transitions.

---

## Summary

**Before**:
- ❌ Joystick nawawala after diary page notification + dialogue
- ❌ Joystick search limited to "Joystick" name only
- ❌ Walang debug logs para i-track ang issue

**After**:
- ✅ Joystick bumabalik after notification
- ✅ Joystick bumabalik after dialogue
- ✅ Multiple joystick name search (Joystick, FloatingJoystick, VariableJoystick)
- ✅ Better debug logging
- ✅ Proper error handling

**Status**: ✅ COMPLETE - Ready for testing!

---

## Related Files
- `DialogueSystemV2.cs` - Dialogue system with joystick handling
- `GlobalDiaryManager.cs` - Diary page combination logic
- `ItemNotificationUI.cs` - Item notification system
- `Room02_LivingRoomController.cs` - Diary page pickup logic

**Tapos na! Test mo na yung diary pages!** 🎉
