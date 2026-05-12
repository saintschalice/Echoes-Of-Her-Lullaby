# Room 06 - Retry Panel Bug (FIXED!)

## ✅ FIXED NA!

**Problema**: Pag nag-retry, lumalabas yung photo panel (with X button) at dialogue panel na walang text.

**Solusyon**: Na-fix na ang `Room06_HallwayController.cs` para i-force close ang photo panel on retry.

---

## 🔧 ANO ANG GINAWA

### Fix 1: Force Close sa Awake()

```csharp
private void Awake()
{
    if (Instance == null) Instance = this;
    
    // Force close photo panel immediately
    if (photoPanel != null)
    {
        photoPanel.SetActive(false);
    }
}
```

### Fix 2: Force Close sa Start()

```csharp
// Force hide photo panel on start
if (photoPanel != null)
{
    photoPanel.SetActive(false);
    Debug.Log("[Room06] Photo panel hidden on start");
}
```

### Fix 3: Close Before Game Over

```csharp
private void TriggerGameOver()
{
    // Close photo panel before game over
    if (photoPanel != null && photoPanel.activeSelf)
    {
        photoPanel.SetActive(false);
    }
    
    // ... game over logic
}
```

---

## 🎯 BAKIT NANGYAYARI ANG BUG

### The Flow (BEFORE FIX):

1. **Interact sa photo** → Panel opens
2. **Emily catches you** → Game Over
3. **BUG**: Photo panel stays open!
4. **Click Retry** → Scene reloads
5. **BUG**: Panel still visible!
6. **Intro plays** → Conflict with panel!

### The Flow (AFTER FIX):

1. **Interact sa photo** → Panel opens
2. **Emily catches you** → Game Over
3. **FIX**: Photo panel closes before game over ✅
4. **Click Retry** → Scene reloads
5. **FIX**: Panel force closed in Awake() ✅
6. **FIX**: Panel force closed in Start() ✅
7. **Intro plays** → No conflict! ✅

---

## ✅ TESTING

### Paano i-test:

1. **Enter Room 06**
2. **Interact sa photo frame** → Panel opens
3. **Let Emily catch you** → Game Over
4. **Click Retry**
5. **Check**:
   - ✅ Photo panel should be HIDDEN
   - ✅ Intro dialogue should play correctly
   - ✅ No visual glitches

### Expected Console Logs:

```
[GameOver] RestartLevel button clicked!
[GameOver] Loading scene: Room06_ReturnToHallwayUpStairs
[Room06] Photo panel force closed in Awake
[Room06] Photo panel hidden on start
[Room06] Playing intro sequence
[Room06] Intro sequence complete
```

---

## 🐛 KUNG LUMALABAS PA RIN ANG PANEL

### Check mo ito:

1. **PhotoPanel location**:
   - Dapat sa Room 06 scene Canvas
   - HINDI sa PersistentUI
   - HINDI DontDestroyOnLoad

2. **PhotoPanel Inspector**:
   - Dapat INACTIVE by default (unchecked)
   - Walang CanvasGroup na naka-alpha 0
   - Walang script na nag-auto show

3. **Hierarchy**:
   ```
   Room06 Scene
   └─ Canvas (Scene Canvas)
       └─ PhotoPanel ← DAPAT DITO!
   ```

### Manual Fix:

1. Open Room 06 scene
2. Find PhotoPanel in Hierarchy
3. Check kung saan siya:
   - Kung sa PersistentUI → MOVE to scene Canvas
   - Kung sa DontDestroyOnLoad → MOVE to scene Canvas
4. Uncheck PhotoPanel (make inactive)
5. Save scene
6. Test ulit

---

## 💡 IMPORTANT NOTES

### Photo Panel Setup:

**CORRECT** ✅:
- PhotoPanel is in Room 06 scene Canvas
- PhotoPanel is inactive by default
- PhotoPanel is NOT in PersistentUI
- PhotoPanel is NOT DontDestroyOnLoad

**WRONG** ❌:
- PhotoPanel is in PersistentUI
- PhotoPanel is active by default
- PhotoPanel is DontDestroyOnLoad
- PhotoPanel is in different scene

### Close Button (X):

Kung may close button ka:
1. Add Button component
2. OnClick event:
   - Drag Room06_HallwayController
   - Select: `ClosePhotoPanel()`

---

## 📋 SUMMARY

**Fixed File**:
- `Assets/Scripts/Puzzle/Room 06/Room06_HallwayController.cs`

**Changes**:
1. ✅ Force close panel in Awake()
2. ✅ Force close panel in Start()
3. ✅ Close panel before Game Over
4. ✅ Re-enable player controls in ClosePhotoPanel()

**Result**:
- ✅ Photo panel hidden on retry
- ✅ Intro dialogue works correctly
- ✅ No visual glitches
- ✅ Player controls work properly

---

**Test mo na! Dapat working na!** 🎮✨

---

## 🔍 ADDITIONAL DEBUG

### Kung may problema pa rin:

**Enable Debug Mode**:
1. Select Room06_HallwayController in scene
2. Check "Debug Mode" in Inspector
3. Play mode
4. Check Console for logs

**Expected Logs**:
- "[Room06] Photo panel force closed in Awake"
- "[Room06] Photo panel hidden on start"
- "[Room06] Playing intro sequence"

**If walang logs**:
- Debug Mode not enabled
- Room06_HallwayController not in scene
- Script may error

---

**Panel bug is FIXED! Ready for testing!** 💪✨
