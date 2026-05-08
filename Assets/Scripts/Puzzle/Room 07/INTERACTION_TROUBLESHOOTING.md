# Room 07 Interaction Troubleshooting Guide

## ❌ Problem: Hindi Ko Ma-Interact ang Lahat ng Items

### ✅ FIXED: Room07_Interactable Script
Nag-update na ako ng script para mag-implement ng `IInteractable` interface.

---

## 🔍 Checklist: Bakit Hindi Gumagana ang Interaction?

### 1. Check ang Collider
- [ ] May **Collider2D** ba ang object? (Box Collider 2D o Circle Collider 2D)
- [ ] **Is Trigger** ba ay naka-check?
- [ ] Tama ba ang size ng collider? (Hindi masyadong maliit)

**Paano I-check:**
1. Select ang object sa Hierarchy
2. Tingnan sa Inspector kung may Collider2D component
3. Click ang object sa Scene view - dapat makita ang green outline (collider bounds)

**Paano I-fix:**
```
1. Select object
2. Add Component → Box Collider 2D
3. Check "Is Trigger"
4. Adjust size para sakto sa sprite
```

---

### 2. Check ang Layer
- [ ] Tama ba ang Layer ng object?
- [ ] Hindi ba naka-set sa "Ignore Raycast"?

**Paano I-check:**
1. Select ang object
2. Tingnan ang Layer dropdown sa taas ng Inspector
3. Dapat "Default" o custom layer na hindi ignored

**Paano I-fix:**
```
1. Select object
2. Layer dropdown → Default
```

---

### 3. Check ang Script
- [ ] May **Room07_Interactable** script ba?
- [ ] Naka-assign ba ang **UI Manager**?
- [ ] Tama ba ang **Object Type**?

**Paano I-check:**
1. Select ang object
2. Tingnan sa Inspector kung may Room07_Interactable component
3. Check kung may laman ang "UI Manager" field
4. Check kung tama ang "My Type" dropdown

**Paano I-fix:**
```
1. Select object
2. Add Component → Room07_Interactable
3. Drag Room07_Manager sa "UI Manager" field
4. Set "My Type" to correct type (e.g., Bed, Mirror, etc.)
```

---

### 4. Check ang Player Interaction System
- [ ] May **PlayerInteraction** script ba ang Player?
- [ ] May **Interaction Range** ba?
- [ ] Tama ba ang **Interaction Layer**?

**Paano I-check:**
1. Select ang Player sa Hierarchy
2. Tingnan kung may PlayerInteraction o similar script
3. Check ang interaction range (dapat at least 2-3 units)

**Paano I-fix:**
```
1. Select Player
2. Check kung may interaction system
3. Adjust interaction range kung masyadong maliit
```

---

### 5. Check ang Input System
- [ ] Gumagana ba ang interact button? (E key o mobile button)
- [ ] May **EventSystem** ba sa scene?

**Paano I-check:**
1. Press Play
2. Lumapit sa object
3. Press E (o tap mobile button)
4. Check Console kung may error

**Paano I-fix:**
```
1. Check kung may EventSystem sa Hierarchy
2. Kung wala: Right-click → UI → Event System
3. Test ulit
```

---

## 🛠️ Common Fixes

### Fix #1: Collider Masyadong Maliit
```
Problem: Kailangan sobrang lapit para ma-interact
Solution:
1. Select object
2. Sa Collider2D component, i-adjust ang Size
3. Make it bigger (e.g., Size: 2, 2)
```

### Fix #2: Walang UI Manager Reference
```
Problem: May error sa Console: "NullReferenceException: uiManager"
Solution:
1. Select LAHAT ng Room07_Interactable objects
2. Drag Room07_Manager sa "UI Manager" field
3. Apply to all
```

### Fix #3: Wrong Object Type
```
Problem: Wrong dialogue lumalabas
Solution:
1. Select object
2. Check "My Type" dropdown
3. Set to correct type:
   - Bed → Bed
   - Mirror → Mirror
   - etc.
```

### Fix #4: Player Interaction Range Masyadong Maliit
```
Problem: Kailangan sobrang lapit sa object
Solution:
1. Select Player
2. Find PlayerInteraction script (o similar)
3. Increase "Interaction Range" to 2.5 or 3
```

---

## 🧪 Testing Each Object

### Test Checklist:
Lumapit sa bawat object at i-press ang E (o interact button):

**Environmental Objects:**
- [ ] Bed → "Child's bed has two pillow indentations..."
- [ ] Wall Drawings → "Crayon drawings show two figures..."
- [ ] Diary → "Child's diary: Emily came to me..."
- [ ] Chair → "Small chair marked Emily's Chair..."
- [ ] Closet → "Scratches inside the closet..."
- [ ] Reading Table → "Fairy tale books..."

**Puzzle Objects:**
- [ ] Window Curtains → Opens curtain panel
- [ ] Small Cabinet → (after curtains) Gets cup
- [ ] Tea Party Spot → Opens tea party panel
- [ ] Toybox → Opens sliding puzzle
- [ ] Dollhouse → Opens dollhouse panel
- [ ] Mirror → Triggers jumpscare (if all puzzles done)

---

## 🔧 Debug Mode

### Enable Debug Logs:
Ang Room07_Interactable ay may debug logs na:
```
[Room07] Focused on Bed
[Room07] Blurred from Bed
```

**Paano Gamitin:**
1. Press Play
2. Lumapit sa object
3. Tingnan ang Console
4. Kung walang log, hindi nag-trigger ang OnFocus
5. Check collider at layer

---

## 📝 Quick Fix Script

Kung gusto mo i-test kung gumagana ang script mismo:

1. Select any Room07_Interactable object
2. Sa Inspector, sa Room07_Interactable component
3. Right-click sa component header → "Edit Script"
4. Sa DoInteract() method, i-add sa unang line:
```csharp
Debug.Log($"[Room07] Interacting with {myType}");
```
5. Save
6. Test - dapat may log sa Console

---

## 🆘 Still Not Working?

### Last Resort Checklist:
1. **Restart Unity** - Sometimes kailangan i-reload ang scripts
2. **Recompile Scripts** - Assets → Reimport All
3. **Check Console** - May error ba?
4. **Check Player** - Gumagalaw ba? May interaction system ba?
5. **Check Scene** - Tama ba ang scene na binuksan?

### Common Errors:

**Error: "NullReferenceException: Object reference not set"**
- **Cause:** May hindi naka-assign na reference
- **Fix:** Check lahat ng fields sa Inspector

**Error: "MissingReferenceException: The object of type 'Room07UIManager' has been destroyed"**
- **Cause:** Nawala ang Room07_Manager
- **Fix:** Check kung nandoon pa ang Room07_Manager sa Hierarchy

**Error: No error, pero walang nangyayari**
- **Cause:** Collider issue o layer issue
- **Fix:** Check collider size at layer settings

---

## ✅ Verification Steps

### Step 1: Visual Check
1. Select object sa Hierarchy
2. Tingnan sa Scene view
3. Dapat may **green outline** (collider)
4. Kung walang green outline = walang collider

### Step 2: Inspector Check
1. Select object
2. Check components:
   - ✅ Transform
   - ✅ Sprite Renderer (optional)
   - ✅ Collider2D (Is Trigger = true)
   - ✅ Room07_Interactable (UI Manager assigned)

### Step 3: Runtime Check
1. Press Play
2. Lumapit sa object
3. Tingnan ang Console
4. Dapat may "[Room07] Focused on..." log

### Step 4: Interaction Check
1. Press E (o interact button)
2. Dapat may dialogue o panel na lumalabas
3. Check Console kung may error

---

## 📞 Need More Help?

Kung after lahat ng steps na ito ay hindi pa rin gumagana:

1. **Take Screenshot** ng:
   - Inspector ng object
   - Console errors
   - Scene view (showing collider)

2. **Check kung:**
   - Tama ba ang scene (Room07_Lisa'sBedroom)
   - May Player ba sa scene
   - May Room07_Manager ba

3. **Try Simple Test:**
   - Gumawa ng bagong empty object
   - Add Box Collider 2D (Is Trigger = true)
   - Add Room07_Interactable
   - Set type to Bed
   - Assign UI Manager
   - Test kung gumagana

Kung gumagana ang simple test, ibig sabihin may problema sa original objects. Kailangan i-recreate.

---

## 🎯 Summary

**Most Common Issues:**
1. ❌ Walang collider → ✅ Add Collider2D
2. ❌ Is Trigger = false → ✅ Check Is Trigger
3. ❌ Walang UI Manager → ✅ Assign Room07_Manager
4. ❌ Wrong Object Type → ✅ Set correct type
5. ❌ Collider masyadong maliit → ✅ Increase size

**Quick Fix:**
```
1. Select ALL Room07_Interactable objects
2. Check kung lahat may:
   - Collider2D (Is Trigger = true)
   - UI Manager assigned
   - Correct Object Type
3. Test one by one
```

Good luck! 🎮
