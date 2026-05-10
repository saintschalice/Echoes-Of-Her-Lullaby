# Panel Troubleshooting Guide - Room 07

## ❌ PROBLEMA: Yung 8-Tile Puzzle Panel Lang ang Lumalabas

### Possible Causes:
1. ❌ Iba pang panels ay NULL (hindi naka-assign)
2. ❌ Panels ay naka-disable permanently
3. ❌ UI Manager walang reference sa panels
4. ❌ Panels ay nasa ibang Canvas

---

## ✅ SOLUTION: Check Panel References

### Step 1: Check Room07UIManager
```
1. Select Room07_Manager sa Hierarchy
2. Tingnan ang Room07UIManager component
3. Check kung lahat ng panels ay naka-assign:
   ☑ Curtain Panel: [CurtainPanel GameObject]
   ☑ Tea Party Panel: [TeaPartyPanel GameObject]
   ☑ Toybox Panel: [ToyboxPanel GameObject]
   ☑ Dollhouse Panel: [DollhousePanel GameObject]
   ☑ Black Screen Cutscene: [BlackScreenCutscene GameObject]
```

### Step 2: Check Console for Errors
```
Press Play → Try to open each panel
Console should show:
  "[Room07] Showing Curtain Panel" ✅
  OR
  "[Room07] Curtain Panel is NULL!" ❌

Kung may NULL error:
  → Panel is not assigned!
  → Need to assign in Inspector
```

---

## 🔧 How to Fix

### Fix 1: Assign Missing Panels

**If Curtain Panel is NULL:**
```
1. Select Room07_Manager
2. Find Room07UIManager component
3. Find "Curtain Panel" field
4. Drag CurtainPanel GameObject from Hierarchy
5. Click Apply
```

**Repeat for all panels:**
- Tea Party Panel
- Toybox Panel
- Dollhouse Panel
- Black Screen Cutscene

### Fix 2: Create Missing Panels

**If panel doesn't exist in scene:**
```
1. Right-click Canvas → UI → Panel
2. Rename to correct name (e.g., "CurtainPanel")
3. Set to full screen (Anchor: Stretch-Stretch)
4. Add appropriate script:
   - CurtainPanel → CurtainPuzzleUI
   - TeaPartyPanel → TeaPartyPuzzleUI
   - ToyboxPanel → ToyboxSlidingPuzzle
   - DollhousePanel → DollhousePuzzleUI
5. Disable panel (uncheck in Inspector)
6. Assign to Room07UIManager
```

---

## 🧪 Testing Each Panel

### Test 1: Curtain Panel
```
1. Press Play
2. Interact with Window Curtains
3. Should show curtain panel
4. Check Console: "[Room07] Showing Curtain Panel"
```

### Test 2: Tea Party Panel
```
1. Press Play
2. Get Emily's Cup first (from cabinet)
3. Interact with Tea Party Spot
4. Should show tea party panel
5. Check Console: "[Room07] Showing Tea Party Panel"
```

### Test 3: Toybox Panel
```
1. Press Play
2. Interact with Toybox
3. Should show sliding puzzle panel
4. Check Console: "[Room07] Showing Toybox Panel"
```

### Test 4: Dollhouse Panel
```
1. Press Play
2. Get Emily Doll first (from toybox after solving)
3. Interact with Dollhouse
4. Should show dollhouse panel
5. Check Console: "[Room07] Showing Dollhouse Panel"
```

---

## 📋 Panel Setup Checklist

### For Each Panel:

#### CurtainPanel
- [ ] GameObject exists in Canvas
- [ ] Name: "CurtainPanel"
- [ ] Has CurtainPuzzleUI script
- [ ] Initially disabled (unchecked)
- [ ] Assigned to Room07UIManager
- [ ] Has left/right curtain buttons
- [ ] Has curtain images (open/closed)

#### TeaPartyPanel
- [ ] GameObject exists in Canvas
- [ ] Name: "TeaPartyPanel"
- [ ] Has TeaPartyPuzzleUI script
- [ ] Initially disabled
- [ ] Assigned to Room07UIManager
- [ ] Has draggable cup
- [ ] Has cup slot

#### ToyboxPanel
- [ ] GameObject exists in Canvas
- [ ] Name: "ToyboxPanel"
- [ ] Has ToyboxSlidingPuzzle script
- [ ] Initially disabled
- [ ] Assigned to Room07UIManager
- [ ] Has tiles parent with Grid Layout
- [ ] Has puzzle image assigned

#### DollhousePanel
- [ ] GameObject exists in Canvas
- [ ] Name: "DollhousePanel"
- [ ] Has DollhousePuzzleUI script
- [ ] Initially disabled
- [ ] Assigned to Room07UIManager
- [ ] Has draggable doll
- [ ] Has doll slot

---

## 🔍 Visual Verification

### In Hierarchy:
```
Canvas
├── CurtainPanel (disabled)
├── TeaPartyPanel (disabled)
├── ToyboxPanel (disabled)
├── DollhousePanel (disabled)
└── BlackScreenCutscene (disabled)
```

### In Inspector (Room07_Manager):
```
Room07UIManager:
  Curtain Panel: [CurtainPanel] ✓
  Tea Party Panel: [TeaPartyPanel] ✓
  Toybox Panel: [ToyboxPanel] ✓
  Dollhouse Panel: [DollhousePanel] ✓
  Black Screen Cutscene: [BlackScreenCutscene] ✓
```

---

## 🐛 Common Issues

### Issue 1: Panel Exists But Not Assigned
```
Symptom: Panel is in Hierarchy but NULL error in Console
Cause: Not assigned to Room07UIManager
Fix: Drag panel to Room07UIManager field
```

### Issue 2: Panel Always Visible
```
Symptom: Panel is always showing
Cause: Panel is enabled in Inspector
Fix: Uncheck panel in Inspector (disable it)
```

### Issue 3: Panel Behind Other UI
```
Symptom: Panel shows but can't interact
Cause: Wrong sorting order or Canvas layer
Fix: 
  - Set Canvas Sorting Order to high number (100+)
  - Or move panel to top of Hierarchy
```

### Issue 4: Multiple Panels Show at Once
```
Symptom: Multiple panels visible simultaneously
Cause: HideAllPanels() not working
Fix: Check if all panels are assigned to UIManager
```

### Issue 5: Panel Shows Then Immediately Hides
```
Symptom: Panel flashes then disappears
Cause: Another script is hiding it
Fix: Check Console for errors or conflicting scripts
```

---

## 🎯 Quick Debug Steps

### Step 1: Check Assignment
```
Select Room07_Manager
→ Room07UIManager component
→ Check all 5 fields
→ All should have GameObjects assigned
→ None should say "None (GameObject)"
```

### Step 2: Check Existence
```
Look at Hierarchy
→ Expand Canvas
→ Should see all 5 panels
→ All should be disabled (grayed out)
```

### Step 3: Test One by One
```
Test Curtain Panel:
  1. Interact with Window Curtains
  2. Check Console
  3. Check if panel appears

Repeat for each panel
```

### Step 4: Enable Debug
```
Console will now show:
  "[Room07] Showing [Panel Name]" = Success
  "[Room07] [Panel Name] is NULL!" = Not assigned
```

---

## 📊 Panel Priority Order

Panels should open in this order during gameplay:

```
1. Curtain Panel (first puzzle)
   ↓
2. Tea Party Panel (after getting cup)
   ↓
3. Toybox Panel (sliding puzzle)
   ↓
4. Dollhouse Panel (after getting doll)
   ↓
5. Mirror Jumpscare (after all puzzles)
```

---

## ✅ Verification Checklist

### Before Testing:
- [ ] All 5 panels exist in Canvas
- [ ] All 5 panels are disabled
- [ ] All 5 panels have correct scripts
- [ ] All 5 panels assigned to Room07UIManager
- [ ] Room07_Manager exists in scene

### During Testing:
- [ ] Curtain panel opens when interacting with curtains
- [ ] Tea party panel opens when interacting with tea spot (with cup)
- [ ] Toybox panel opens when interacting with toybox
- [ ] Dollhouse panel opens when interacting with dollhouse (with doll)
- [ ] No NULL errors in Console

### After Testing:
- [ ] Panels close properly after solving
- [ ] Game resumes after closing panels
- [ ] No panels stuck open
- [ ] No multiple panels open at once

---

## 🆘 Emergency Fix

### If Nothing Works:

**Option 1: Recreate Panels**
```
1. Delete all panels from Canvas
2. Follow UNITY_SETUP_GUIDE_TAGALOG.md
3. Recreate panels step by step
4. Assign to Room07UIManager
5. Test each panel
```

**Option 2: Use Toybox Panel as Template**
```
Since Toybox panel works:
1. Duplicate ToyboxPanel
2. Rename to CurtainPanel
3. Remove ToyboxSlidingPuzzle script
4. Add CurtainPuzzleUI script
5. Configure components
6. Assign to Room07UIManager
7. Test

Repeat for other panels
```

**Option 3: Check Other Rooms**
```
1. Open Room 02 or Room 04 scene
2. Check how their panels are setup
3. Copy the structure
4. Apply to Room 07
```

---

## 📞 Get Help

### Provide These Info:
1. **Screenshot of Hierarchy** (showing Canvas and panels)
2. **Screenshot of Room07UIManager** (showing all fields)
3. **Console Log** (copy all text)
4. **Which panels work?** (e.g., "Only Toybox works")
5. **Which panels don't work?** (e.g., "Curtain, Tea Party, Dollhouse")

### Check These First:
- [ ] All panels exist in Hierarchy?
- [ ] All panels assigned to UIManager?
- [ ] All panels have correct scripts?
- [ ] All panels are disabled initially?
- [ ] Console shows NULL errors?

---

## 🎓 Pro Tips

1. **Name Consistently** - Use exact names (CurtainPanel, not Curtain_Panel)
2. **Disable Initially** - All panels should start disabled
3. **Test Individually** - Test one panel at a time
4. **Check Console** - Always check for NULL errors
5. **Use Prefabs** - Once working, make prefabs for reuse

---

**Fix the NULL references and all panels should work!** 🎮✨

**Key: Make sure ALL panels are assigned to Room07UIManager!** ✅
