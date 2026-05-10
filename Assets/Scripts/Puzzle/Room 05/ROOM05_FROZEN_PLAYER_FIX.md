# Room 5 Frozen Player - Complete Fix Guide

## PROBLEMA
Pagpasok sa Room 5 (Dining Room), hindi makagalaw ang player. Completely frozen.

## ROOT CAUSES (Based on Console Errors)

### 1. DontDestroyOnLoad Assertion Error
```
Assertion failed: m_GameObjects.find(gameObject.GetEntityId()) == m_GameObjects.end()
```
**CAUSE**: Multiple objects calling DontDestroyOnLoad on the same GameObject
**STATUS**: ✅ FIXED in `PersistentObject.cs`

### 2. ScreenFader Not Found
```
[RoomExit] ScreenFader not found! Transitioning without fade.
```
**CAUSE**: Room 5 scene walang ScreenFader GameObject
**FIX**: Add ScreenFader to Room 5 scene (see below)

### 3. Missing Script Component
```
The referenced script (Unknown) on this Behaviour is missing!
```
**CAUSE**: May GameObject sa scene na may deleted/missing script component
**FIX**: Find and remove the missing component (see below)

---

## STEP-BY-STEP FIX

### STEP 1: Add ScreenFader to Room 5 Scene

1. **Open Scene**: `Assets/Scenes/Room05_DiningRoom.unity`

2. **Create ScreenFader GameObject**:
   - Right-click in Hierarchy → Create Empty
   - Name: `ScreenFader`
   - Add Component → `ScreenFader` script

3. **Setup ScreenFader UI**:
   - Right-click on ScreenFader → UI → Image
   - Name the Image: `FadeImage`
   - Set Image properties:
     - Anchor: Stretch (full screen)
     - Color: Black (R:0, G:0, B:0, A:255)
     - Raycast Target: ✓ (checked)

4. **Assign Reference**:
   - Select ScreenFader GameObject
   - In Inspector, drag `FadeImage` to the `Fade Image` field
   - Set `Default Fade Duration`: 1
   - Set `Fade In On Start`: ✓ (checked)
   - Set `Start Delay`: 0.2

5. **Add PersistentObject Component**:
   - Select ScreenFader GameObject
   - Add Component → `PersistentObject`
   - Set `Persist`: ✓ (checked)

6. **Set Canvas Order**:
   - Select the Canvas that contains FadeImage
   - Set `Sort Order`: 1000 (highest, para sa fade effect)

---

### STEP 2: Find and Remove Missing Script Components

1. **Open Console** (Ctrl+Shift+C)

2. **Click on the "Missing Script" error** - it will highlight the GameObject

3. **In Inspector**:
   - Look for components that say `Script (Missing)`
   - Click the ⚙️ (gear icon) next to it
   - Select "Remove Component"

4. **Common Places to Check**:
   - Player GameObject
   - Canvas objects
   - UI panels
   - Emily GameObject
   - Room Controller

---

### STEP 3: Verify Player Setup in Room 5

1. **Check Player Spawn Point**:
   ```
   GameObject: DefaultSpawn (or your spawn point name)
   Component: SpawnPoint
   - spawnPointName: "DefaultSpawn"
   ```

2. **Check Player GameObject**:
   - Tag: "Player"
   - Components required:
     - ✓ JoystickPlayerController (enabled)
     - ✓ Rigidbody2D
     - ✓ Animator
     - ✓ SpriteRenderer
     - ✓ Collider2D

3. **Check Joystick**:
   - GameObject name: "Joystick" or "DynamicJoystick"
   - Component: VirtualJoystick
   - Should be active in hierarchy

---

### STEP 4: Add Emergency Player Enable Script

Kung after ng Steps 1-3 ay hindi pa rin gumagana, add this emergency script:

1. **Create Script**: Already created as `Room05_ForceEnablePlayer.cs`

2. **Add to Scene**:
   - Open Room 5 scene
   - Create Empty GameObject: `PlayerEnabler`
   - Add Component → `Room05_ForceEnablePlayer`

3. **How it works**:
   - Auto-enables player after 1 second
   - Or press **E key** to manually enable
   - Logs everything to console for debugging

---

## TESTING CHECKLIST

After applying fixes, test these:

- [ ] Player can move immediately upon entering Room 5
- [ ] No console errors about DontDestroyOnLoad
- [ ] No console errors about ScreenFader
- [ ] No console errors about missing scripts
- [ ] Joystick appears and works
- [ ] Player controls respond to joystick input
- [ ] Scene transition has fade effect

---

## ADDITIONAL CHECKS

### If Player Still Frozen:

1. **Check Room05_DiningRoomController**:
   - Is `PauseGameForUI()` being called on Start?
   - Check `isGamePausedForUI` flag in Inspector (should be false)

2. **Check for Blocking UI**:
   - Press F1 to check if any UI is blocking
   - Check if RecipeBook panel is active (should be false)
   - Check if any dialogue is active

3. **Check Player Components in Inspector**:
   - JoystickPlayerController: ✓ enabled
   - Rigidbody2D: ✓ enabled, not kinematic
   - Collider2D: ✓ enabled

4. **Check Joystick**:
   - Active in hierarchy: ✓
   - VirtualJoystick component: ✓ enabled
   - Canvas: ✓ enabled

---

## DEBUG COMMANDS

Add these to `Room05_ForceEnablePlayer.cs` Update():

```csharp
// Press L to log player state
if (Input.GetKeyDown(KeyCode.L))
{
    Debug.Log("=== PLAYER STATE ===");
    Debug.Log($"Player exists: {player != null}");
    Debug.Log($"Controller enabled: {controller?.enabled}");
    Debug.Log($"Rigidbody2D: {rb != null}");
    Debug.Log($"Joystick exists: {joystick != null}");
    Debug.Log($"Joystick active: {joystick?.gameObject.activeSelf}");
}
```

---

## COMMON MISTAKES

❌ **WRONG**: Forgetting to set ScreenFader as DontDestroyOnLoad
✅ **RIGHT**: Add PersistentObject component with persist=true

❌ **WRONG**: ScreenFader Canvas has low Sort Order
✅ **RIGHT**: Canvas Sort Order = 1000 (highest)

❌ **WRONG**: Not removing missing script components
✅ **RIGHT**: Find and remove ALL missing components

❌ **WRONG**: Player spawns but controls not enabled
✅ **RIGHT**: Check JoystickPlayerController.enabled = true

---

## RELATED FILES

- `Assets/Scripts/Puzzle/Room 05/Room05_DiningRoomController.cs` - Main room logic
- `Assets/Scripts/Puzzle/Room 05/Room05_ForceEnablePlayer.cs` - Emergency fix
- `Assets/Scripts/GameManagement/ScreenFader.cs` - Fade transitions
- `Assets/Scripts/GameManagement/PersistentObject.cs` - DontDestroyOnLoad fix
- `Assets/Scripts/Player/JoystickPlayerController.cs` - Player movement

---

## NEXT STEPS

1. Apply STEP 1 (Add ScreenFader)
2. Apply STEP 2 (Remove missing scripts)
3. Test the game
4. If still frozen, apply STEP 4 (Emergency script)
5. Report back with console output

**Importante**: Save the scene after each step!
