# Visual Setup Guide - Room 07 Interactables

## 🎯 Correct Setup Example

### Bed Object Setup (Example)

```
Hierarchy:
└── Bed
    ├── Sprite (optional child)
    └── Collider visualization (green outline in Scene view)

Inspector View:
┌─────────────────────────────────────┐
│ Bed                                 │
├─────────────────────────────────────┤
│ Tag: Untagged    Layer: Default     │
├─────────────────────────────────────┤
│ Transform                           │
│   Position: (0, 0, 0)              │
│   Rotation: (0, 0, 0)              │
│   Scale: (1, 1, 1)                 │
├─────────────────────────────────────┤
│ Sprite Renderer (optional)          │
│   Sprite: [bed_sprite]             │
├─────────────────────────────────────┤
│ Box Collider 2D                     │
│   ☑ Is Trigger                     │ ← IMPORTANTE!
│   Size: (2, 2)                     │
│   Offset: (0, 0)                   │
├─────────────────────────────────────┤
│ Room07_Interactable                 │
│   My Type: Bed                     │ ← Tama ang type!
│   UI Manager: [Room07_Manager]     │ ← May reference!
│   Required Item ID: (empty)        │
│   Interaction Prompt: Press E...   │
└─────────────────────────────────────┘
```

---

## ✅ Checklist Para sa Bawat Object

### Visual Indicators:
1. **Green Outline** sa Scene view = May collider ✅
2. **No outline** = Walang collider ❌
3. **Blue outline** = Collider pero hindi trigger ⚠️

### Inspector Checklist:
```
☑ Transform component
☑ Collider2D component
  ☑ Is Trigger = TRUE
  ☑ Size is reasonable (not too small)
☑ Room07_Interactable component
  ☑ My Type = Correct type
  ☑ UI Manager = Room07_Manager assigned
```

---

## 🔧 Step-by-Step Setup (Visual)

### Step 1: Create Object
```
Right-click Hierarchy
→ Create Empty
→ Name: "Bed"
```

### Step 2: Add Collider
```
Select "Bed"
→ Inspector → Add Component
→ Search: "Box Collider 2D"
→ Click to add
```

### Step 3: Enable Trigger
```
In Box Collider 2D component:
→ Find "Is Trigger" checkbox
→ ☑ Check it!
```

### Step 4: Adjust Collider Size
```
In Box Collider 2D component:
→ Size X: 2
→ Size Y: 2
(Adjust based on your sprite size)
```

### Step 5: Add Script
```
Select "Bed"
→ Inspector → Add Component
→ Search: "Room07_Interactable"
→ Click to add
```

### Step 6: Configure Script
```
In Room07_Interactable component:
→ My Type dropdown: Select "Bed"
→ UI Manager field: Drag "Room07_Manager" from Hierarchy
```

### Step 7: Verify
```
Scene View:
→ Select object
→ Should see GREEN outline (collider bounds)

Inspector:
→ Is Trigger = ☑ Checked
→ My Type = Correct
→ UI Manager = Assigned
```

---

## 🎨 Scene View Visual Guide

### What You Should See:

```
Scene View (with object selected):

    ┌─────────────────┐
    │                 │  ← Green outline (collider)
    │   [Bed Sprite]  │
    │                 │
    └─────────────────┘
         ↑
    Gizmo (move tool)
```

### What's Wrong:

```
NO GREEN OUTLINE = No collider!
→ Add Box Collider 2D

BLUE OUTLINE = Collider but not trigger!
→ Check "Is Trigger"

RED OUTLINE = Collider on wrong layer!
→ Set Layer to Default
```

---

## 📊 Object Type Reference

### Environmental Objects:
| Object Name        | My Type       | Has Sprite? |
|-------------------|---------------|-------------|
| Bed               | Bed           | Yes         |
| WallDrawings      | WallDrawings  | Yes         |
| Nightstand_Diary  | Diary         | Yes         |
| EmilyChair        | Chair         | Yes         |
| Closet            | Closet        | Yes         |
| ReadingTable      | ReadingTable  | Yes         |

### Puzzle Objects:
| Object Name       | My Type        | Has Sprite? | Extra Components |
|------------------|----------------|-------------|------------------|
| WindowCurtains   | WindowCurtains | Yes         | -                |
| SmallCabinet     | Cabinet_Cup    | Yes         | -                |
| TeaPartySpot     | TeaParty       | Optional    | -                |
| Toybox           | Toybox         | Yes         | Audio Source     |
| Dollhouse        | Dollhouse      | Yes         | -                |
| Mirror           | Mirror         | Yes         | -                |

---

## 🧪 Testing Visual Guide

### Test 1: Collider Visibility
```
1. Select object in Hierarchy
2. Look at Scene view
3. Should see green box/circle around object
4. If no green outline → Add collider!
```

### Test 2: Interaction Range
```
1. Press Play
2. Move player near object
3. Player should be within 2-3 units
4. Press E to interact
```

### Test 3: Console Feedback
```
1. Press Play
2. Move near object
3. Console should show:
   "[Room07] Focused on Bed"
4. Move away:
   "[Room07] Blurred from Bed"
5. Press E:
   Dialogue should appear
```

---

## 🎯 Quick Visual Checklist

### In Scene View:
- [ ] Can see object sprite/icon
- [ ] Can see green collider outline when selected
- [ ] Collider size covers the sprite reasonably
- [ ] Object is in the room (not outside bounds)

### In Inspector:
- [ ] Transform position is reasonable
- [ ] Collider2D exists
- [ ] Is Trigger is checked
- [ ] Room07_Interactable exists
- [ ] My Type is set correctly
- [ ] UI Manager shows "Room07_Manager"

### In Hierarchy:
- [ ] Object name is clear (e.g., "Bed", not "GameObject")
- [ ] Object is not inside another object accidentally
- [ ] Object is in the scene root (not hidden in folders)

---

## 🔍 Common Visual Problems

### Problem 1: Can't See Collider
```
Symptom: No green outline in Scene view
Cause: No collider component
Fix: Add Box Collider 2D
```

### Problem 2: Collider Too Small
```
Symptom: Green outline is tiny
Cause: Collider size is too small (e.g., 0.1, 0.1)
Fix: Increase size to 2, 2 or bigger
```

### Problem 3: Object Not Visible
```
Symptom: Can't find object in Scene view
Cause: Object is far away or scale is 0
Fix: 
  - Double-click object in Hierarchy (focuses camera)
  - Check Transform scale (should be 1, 1, 1)
```

### Problem 4: Wrong Layer Color
```
Symptom: Collider is blue or red, not green
Cause: Wrong collider type or layer
Fix:
  - Use Box Collider 2D (not 3D)
  - Set Layer to Default
  - Check "Is Trigger"
```

---

## 📸 Screenshot Checklist

If you need help, take screenshots of:

1. **Hierarchy View** - showing all objects
2. **Inspector View** - of the problematic object
3. **Scene View** - showing collider (or lack of it)
4. **Console** - showing any errors
5. **Game View** - showing the issue in play mode

---

## ✅ Final Verification

### Before Testing:
```
For EACH object:
1. Select in Hierarchy ✓
2. See green outline in Scene ✓
3. Check "Is Trigger" in Inspector ✓
4. Check "My Type" is correct ✓
5. Check "UI Manager" is assigned ✓
```

### During Testing:
```
For EACH object:
1. Press Play ✓
2. Walk to object ✓
3. See focus log in Console ✓
4. Press E ✓
5. See dialogue/panel ✓
```

---

## 🎓 Pro Tips

1. **Use Prefabs** - Setup one object correctly, then duplicate
2. **Color Code** - Use different colors for different object types
3. **Name Clearly** - Use descriptive names (not "GameObject (1)")
4. **Group Objects** - Put all interactables in a folder
5. **Test Often** - Don't wait until all objects are done

---

Kung sundin mo ang visual guide na ito, dapat gumagana na ang lahat ng interactions! 🎮✨
