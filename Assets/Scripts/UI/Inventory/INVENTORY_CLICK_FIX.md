# Inventory Click Not Working Fix - Tagalog Guide

## PROBLEMA
Hindi natatap ang items sa inventory pag nasa Kitchen scene na.

## POSSIBLE CAUSES
1. **CanvasGroup blocksRaycasts = false** - naka-disable ang raycasts
2. **Higher sorting order Canvas** - may UI na naka-block sa inventory
3. **GraphicRaycaster disabled** - walang raycaster sa canvas
4. **EventSystem missing** - walang event system sa scene

---

## QUICK FIX 1: Check CanvasGroup

### In Unity Editor:
1. **Open Kitchen scene**
2. **Find InventoryPanel** sa Hierarchy
   - Usually nasa: `PersistentUI > InventoryPanel`
3. **Check CanvasGroup component**:
   ```
   CanvasGroup:
   ├─ Alpha: 1
   ├─ Interactable: ✓ CHECKED
   └─ Blocks Raycasts: ✓ CHECKED (CRITICAL!)
   ```

**IMPORTANT**: Ang `Blocks Raycasts` ay DAPAT naka-check!

---

## QUICK FIX 2: Check Canvas Sorting Order

### Problem: May UI na naka-block
Kitchen scene might have a Canvas with higher sorting order than inventory.

### Solution:
1. **Find all Canvases** sa Kitchen scene
2. **Check sorting orders**:
   ```
   Expected:
   ├─ PersistentUI Canvas: 100 (inventory)
   ├─ DialogueCanvas: 90 (below inventory)
   ├─ GameOverCanvas: 200 (above inventory - OK)
   └─ Kitchen UI: Should be < 100
   ```

3. **If Kitchen has Canvas > 100**:
   - Select the Canvas
   - Set Sorting Order to < 100 (e.g., 50)

---

## QUICK FIX 3: Add Debug Script

### Step 1: Add Debugger
1. **Select InventoryPanel** sa Hierarchy
2. **Add Component**: `InventoryClickDebugger`
3. **Enable Debug**: ✓ checked

### Step 2: Test
1. **Play** the Kitchen scene
2. **Press D key** - shows debug info sa console
3. **Click on inventory** - shows what was clicked

### Step 3: Read Console
Look for these messages:
```
[Debug] CanvasGroup - Alpha: 1, Interactable: True, BlocksRaycasts: True
[Debug] GraphicRaycaster enabled: True
[Click Debug] Clicked at (x, y), hit X UI elements
```

**If BlocksRaycasts: False** → That's the problem!
**If hit 0 UI elements** → EventSystem or Raycaster issue

---

## QUICK FIX 4: Ensure GraphicRaycaster

### Check:
1. **Find PersistentUI Canvas** (parent of InventoryPanel)
2. **Check components**:
   ```
   Canvas
   ├─ Render Mode: Screen Space - Overlay
   ├─ Sorting Order: 100
   └─ Components:
       ├─ Canvas ✓
       ├─ Canvas Scaler ✓
       └─ Graphic Raycaster ✓ (MUST HAVE!)
   ```

### If Missing GraphicRaycaster:
1. Select Canvas
2. **Add Component**: `Graphic Raycaster`
3. Test again

---

## QUICK FIX 5: Check EventSystem

### Verify EventSystem Exists:
1. **Hierarchy** → Search: `EventSystem`
2. Should find: `EventSystem` GameObject

### If Missing:
1. **Right-click** Hierarchy
2. **UI** → **Event System**
3. Test again

---

## CODE FIX (If needed)

### If CanvasGroup keeps resetting to blocksRaycasts = false:

Check `InventoryUI.cs` → `SetVisible()` method:
```csharp
void SetVisible(bool visible)
{
    if (inventoryPanel == null) return;

    CanvasGroup canvasGroup = inventoryPanel.GetComponent<CanvasGroup>();
    if (canvasGroup != null)
    {
        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible; // ← Should be TRUE when visible!
    }
}
```

This should already be correct. If not, update it.

---

## TESTING CHECKLIST

### Test in Kitchen:
- [ ] Open inventory (press I or click button)
- [ ] Inventory panel appears
- [ ] **Single tap** on item → Shows tooltip
- [ ] **Double tap** on item → Uses item
- [ ] Items respond to taps
- [ ] No console errors

### Test in Other Rooms:
- [ ] Test in Foyer - works?
- [ ] Test in Living Room - works?
- [ ] Test in Hallway - works?
- [ ] **Test in Kitchen** - works? ← Main issue

---

## COMMON ISSUES

### Issue 1: Works in other rooms, not in Kitchen
**CAUSE**: Kitchen has blocking UI or Canvas
**FIX**: 
- Check Kitchen scene for extra Canvases
- Lower their sorting order to < 100

### Issue 2: Can't click anything in inventory
**CAUSE**: blocksRaycasts = false
**FIX**:
- Select InventoryPanel
- Check CanvasGroup component
- Enable "Blocks Raycasts"

### Issue 3: Inventory doesn't open at all
**CAUSE**: Different issue (not click-related)
**FIX**:
- Check InventoryUI.Instance exists
- Check toggle button is connected
- Check for blocking dialogues

### Issue 4: Single tap works, double tap doesn't
**CAUSE**: DOUBLE_TAP_THRESHOLD too short
**FIX**:
- Open InventorySlot.cs
- Change `DOUBLE_TAP_THRESHOLD` from 0.3f to 0.5f

---

## MANUAL FIX IN SCENE

### If you need to manually fix in Kitchen scene:

1. **Open Room04_KitchenDining scene**
2. **Find PersistentUI** (should be in DontDestroyOnLoad, but check scene too)
3. **Select InventoryPanel**
4. **Inspector** → CanvasGroup:
   - Alpha: 1
   - Interactable: ✓
   - Blocks Raycasts: ✓
5. **Save scene**
6. **Test**

---

## DEBUG COMMANDS

### In Play Mode (Kitchen):
- **Press D** - Show inventory debug info
- **Click anywhere** - Show what UI was hit
- **Press I** - Toggle inventory

### Console Filters:
```
[Debug]
[Click Debug]
[InventorySlot]
[InventoryUI]
```

---

## SUMMARY

**Most Likely Cause**:
- CanvasGroup `blocksRaycasts` is false
- Or Kitchen has blocking Canvas with high sorting order

**Quick Fix**:
1. Select InventoryPanel
2. Check CanvasGroup → Blocks Raycasts ✓
3. Check Canvas sorting orders (Inventory should be ~100)
4. Test

**If Still Not Working**:
1. Add InventoryClickDebugger script
2. Press D in Play Mode
3. Read console output
4. Fix based on debug info

Yan lang! 🎮
