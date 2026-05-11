# 🐛 ROOM 09 - DRAG & DROP TROUBLESHOOTING GUIDE

## 🎯 COMMON PROBLEMS AND SOLUTIONS

Kung may problema sa drag-and-drop system, check dito!

---

## ❌ PROBLEM 1: Can't Drag Items

### **Symptoms**:
- Items don't move when tapped/clicked
- No response when touching items
- Items are "stuck"

### **Possible Causes & Solutions**:

#### **Cause 1: Missing DraggableItem Script**

**Check**:
```
1. Select item GameObject
2. Inspector → Look for DraggableItem component
```

**Fix**:
```
1. Select item
2. Add Component → DraggableItem
3. Set Item Id and Puzzle Number
```

---

#### **Cause 2: Raycast Target Unchecked**

**Check**:
```
1. Select item GameObject
2. Inspector → Image component
3. Look at "Raycast Target" checkbox
```

**Fix**:
```
1. Select item
2. Image component → Raycast Target: ✓ (CHECK IT)
```

**Why**: Unity needs Raycast Target checked to detect touch/click events!

---

#### **Cause 3: No Graphic Raycaster on Canvas**

**Check**:
```
1. Select Canvas GameObject
2. Inspector → Look for Graphic Raycaster component
```

**Fix**:
```
1. Select Canvas
2. Add Component → Graphic Raycaster
```

**Why**: Canvas needs Graphic Raycaster to detect UI interactions!

---

#### **Cause 4: No EventSystem in Scene**

**Check**:
```
1. Hierarchy → Search "EventSystem"
2. Should find one EventSystem GameObject
```

**Fix**:
```
1. GameObject → UI → Event System
2. Should auto-create EventSystem
```

**Why**: EventSystem handles all input events in Unity UI!

---

#### **Cause 5: Item Not Child of Canvas**

**Check**:
```
1. Hierarchy → Find item GameObject
2. Check if it's under Canvas (or a child of Canvas)
```

**Fix**:
```
1. Drag item GameObject under Canvas
2. Or under a Panel that's under Canvas
```

**Why**: UI elements must be children of Canvas to work!

---

## ❌ PROBLEM 2: Items Snap Back Immediately

### **Symptoms**:
- Item moves but returns to start position instantly
- Item doesn't stay in slot
- "Dropped on nothing" message in Console

### **Possible Causes & Solutions**:

#### **Cause 1: Slot Name Doesn't Contain "Slot" or "Frame"**

**Check**:
```
1. Select slot GameObject
2. Look at name in Inspector
```

**Fix**:
```
1. Rename slot to include "Slot" or "Frame"
   - Good: "Slot_1", "BottleSlot_1", "Frame_1"
   - Bad: "Position1", "Place1", "Container1"
```

**Why**: DraggableItem script looks for "Slot" or "Frame" in name!

---

#### **Cause 2: Wrong Item Id**

**Check**:
```
1. Select item GameObject
2. Inspector → DraggableItem → Item Id
3. Compare with ITEM_IDS_REFERENCE.md
```

**Fix**:
```
1. Set correct Item Id (see ITEM_IDS_REFERENCE.md)
   - Mirror 1: bottle_1973, bottle_1974, etc.
   - Mirror 2: piece1, piece2, piece3, piece4
   - Mirror 3: page1, page2, ... page8
   - Mirror 4: rope, pills, knife, towel
```

**Why**: Puzzle script expects specific Item Ids!

---

#### **Cause 3: Wrong Puzzle Number**

**Check**:
```
1. Select item GameObject
2. Inspector → DraggableItem → Puzzle Number
```

**Fix**:
```
1. Set correct Puzzle Number:
   - Mirror 1 items: Puzzle Number = 1
   - Mirror 2 items: Puzzle Number = 2
   - Mirror 3 items: Puzzle Number = 3
   - Mirror 4 items: Puzzle Number = 4
```

**Why**: DraggableItem notifies wrong puzzle script if number is wrong!

---

#### **Cause 4: Puzzle Script Not Found**

**Check Console**:
```
Look for: "[DraggableItem] Mirror1_MedicineCabinet not found!"
```

**Fix**:
```
1. Find mirror GameObject in scene (not panel!)
2. Add Component → Mirror1_MedicineCabinet (or Mirror2, 3, 4)
3. Make sure script is attached to mirror, not panel
```

**Why**: DraggableItem needs to find puzzle script to notify it!

---

## ❌ PROBLEM 3: Items Don't Snap to Slots

### **Symptoms**:
- Item drops but doesn't center in slot
- Item stays where dropped
- No visual feedback

### **Possible Causes & Solutions**:

#### **Cause 1: Slot Doesn't Have RectTransform**

**Check**:
```
1. Select slot GameObject
2. Inspector → Should have RectTransform component
```

**Fix**:
```
1. Slot must be a UI element (Image, Panel, etc.)
2. If not, create new UI Image for slot
```

**Why**: DraggableItem uses RectTransform to position items!

---

#### **Cause 2: Slot Is Behind Item**

**Check**:
```
1. Hierarchy → Check order
2. Slots should be ABOVE items in hierarchy
```

**Fix**:
```
1. Drag slots higher in hierarchy
2. Or drag items lower
3. Items should be rendered on top
```

**Why**: Raycast hits the first object, which should be the item!

---

## ❌ PROBLEM 4: Puzzle Doesn't Detect Completion

### **Symptoms**:
- All items placed correctly
- Nothing happens
- No success dialogue

### **Possible Causes & Solutions**:

#### **Cause 1: Items in Wrong Order**

**Check**:
```
1. Verify correct order:
   - Mirror 1: 1973, 1974, 1975a, 1975b, 1976a, 1976b
   - Mirror 2: piece1, piece2, piece3, piece4
   - Mirror 3: page1, page2, page3, page4, page5, page6, page7, page8
   - Mirror 4: rope, pills, knife, towel
```

**Fix**:
```
1. Rearrange items in correct order
2. Check ITEM_IDS_REFERENCE.md for correct sequence
```

---

#### **Cause 2: Slots Not Assigned in Inspector**

**Check**:
```
1. Select mirror GameObject (in scene)
2. Inspector → Mirror script component
3. Check if slots array is filled
```

**Fix**:
```
1. Expand slots array
2. Set size to correct number (6, 4, 8, or 4)
3. Drag each slot GameObject to array
4. Make sure order matches (Slot_1 to Element 0, etc.)
```

**Why**: Puzzle script needs slot references to check solution!

---

#### **Cause 3: Item Ids Don't Match**

**Check Console**:
```
Look for debug messages showing Item Ids
Compare with expected values
```

**Fix**:
```
1. Check Item Id spelling (lowercase, underscores)
2. Check for typos
3. Verify against ITEM_IDS_REFERENCE.md
```

---

## ❌ PROBLEM 5: Items Disappear When Dragged

### **Symptoms**:
- Item vanishes when dragging starts
- Item reappears when dropped
- Can't see item while dragging

### **Possible Causes & Solutions**:

#### **Cause 1: Drag Alpha Too Low**

**Check**:
```
1. Select item GameObject
2. Inspector → DraggableItem → Drag Alpha
```

**Fix**:
```
1. Set Drag Alpha to 0.6 or higher
2. Or uncheck "Fade While Dragging"
```

---

#### **Cause 2: Item Moved Behind Other UI**

**Check**:
```
1. While dragging, check Hierarchy
2. Item should be at bottom (rendered on top)
```

**Fix**:
```
This is handled automatically by DraggableItem
If not working, check Canvas sort order
```

---

## ❌ PROBLEM 6: Multiple Items in Same Slot

### **Symptoms**:
- Can place multiple items in one slot
- Items overlap
- Puzzle accepts wrong solution

### **Solution**:

This is expected behavior! The puzzle script checks if items are in CORRECT slots, not if slots are full.

**To prevent**: Add logic to puzzle scripts to check if slot is already occupied.

---

## ❌ PROBLEM 7: Can't Drag on Mobile

### **Symptoms**:
- Works in Unity editor
- Doesn't work on mobile device
- Touch not detected

### **Possible Causes & Solutions**:

#### **Cause 1: Input System Not Setup**

**Check**:
```
Edit → Project Settings → Player → Other Settings
Look at "Active Input Handling"
```

**Fix**:
```
Set to "Both" or "Input System Package (New)"
```

---

#### **Cause 2: Canvas Render Mode Wrong**

**Check**:
```
1. Select Canvas
2. Inspector → Canvas component → Render Mode
```

**Fix**:
```
Set to "Screen Space - Overlay" for mobile
```

---

#### **Cause 3: Touch Not Enabled**

**Check**:
```
Edit → Project Settings → Player → Resolution and Presentation
Look at "Default Orientation"
```

**Fix**:
```
Set to "Landscape Left" or "Landscape Right" for mobile
Enable "Multithreaded Rendering" if needed
```

---

## 🔍 DEBUGGING TIPS

### **Enable Debug Logs**:

DraggableItem script already has debug logs! Check Console for:

```
[DraggableItem] Started dragging: bottle_1973
[DraggableItem] bottle_1973 dropped on Slot_1
[DraggableItem] bottle_1973 placed in Slot_1
[DraggableItem] bottle_1973 returned to original position
```

### **Common Console Messages**:

**"Started dragging: [itemId]"**
- ✅ Good! Drag detected

**"[itemId] dropped on [slotName]"**
- ✅ Good! Slot detected

**"[itemId] placed in [slotName]"**
- ✅ Good! Item placed successfully

**"[itemId] dropped on nothing"**
- ⚠️ No slot detected - check slot names

**"[itemId] returned to original position"**
- ⚠️ Item not placed - check slot detection

**"Mirror1_MedicineCabinet not found!"**
- ❌ Puzzle script missing - add to mirror GameObject

---

## 📋 COMPLETE TROUBLESHOOTING CHECKLIST

### **If Drag-and-Drop Not Working**:

- [ ] Item has DraggableItem script
- [ ] Item has Image component
- [ ] Image → Raycast Target is CHECKED
- [ ] Item Id is set correctly (lowercase, no spaces)
- [ ] Puzzle Number is set correctly (1-4)
- [ ] Canvas has Graphic Raycaster
- [ ] EventSystem exists in scene
- [ ] Item is child of Canvas (or child of Panel under Canvas)
- [ ] Slots have "Slot" or "Frame" in name
- [ ] Slots are UI elements (Image, Panel, etc.)
- [ ] Mirror GameObject has puzzle script
- [ ] Slots are assigned in mirror script Inspector
- [ ] Panel is active when testing
- [ ] No errors in Console

---

## 🎯 QUICK FIXES

### **Can't drag at all?**
```
1. Check Canvas → Graphic Raycaster ✓
2. Check EventSystem exists ✓
3. Check Item → Image → Raycast Target ✓
4. Check Item → DraggableItem script ✓
```

### **Items snap back?**
```
1. Check slot names contain "Slot" or "Frame"
2. Check Item Id is correct (see ITEM_IDS_REFERENCE.md)
3. Check Puzzle Number is correct (1-4)
4. Check mirror has puzzle script
```

### **Puzzle doesn't complete?**
```
1. Check items are in correct order
2. Check slots are assigned in Inspector
3. Check Item Ids match expected values
4. Check Console for errors
```

---

## 🆘 STILL NOT WORKING?

### **Step-by-Step Debug Process**:

**1. Test Basic Drag**:
```
1. Create simple test:
   - Canvas → Panel → Image (test item)
   - Add DraggableItem to image
   - Set Item Id: "test"
   - Set Puzzle Number: 1
2. Play scene
3. Try dragging test item
4. If works: Problem is with your setup
5. If doesn't work: Check Canvas/EventSystem
```

**2. Check Console**:
```
1. Play scene
2. Try dragging
3. Read Console messages
4. Look for errors or warnings
5. Fix errors first
```

**3. Verify References**:
```
1. Select mirror GameObject
2. Inspector → Check all references
3. Make sure nothing is "None" or "Missing"
4. Reassign if needed
```

**4. Test One Puzzle at a Time**:
```
1. Start with Mirror 1 only
2. Get it working completely
3. Then move to Mirror 2
4. Don't try to fix all at once
```

---

## ✅ VERIFICATION TESTS

### **Test 1: Basic Drag**
```
1. Play scene
2. Tap/click item
3. Item should become semi-transparent
4. Item should follow mouse/finger
✅ PASS: Drag system works
❌ FAIL: Check Canvas/EventSystem/Raycast Target
```

### **Test 2: Slot Detection**
```
1. Drag item over slot
2. Release
3. Item should snap to center of slot
✅ PASS: Slot detection works
❌ FAIL: Check slot names, Item Id, Puzzle Number
```

### **Test 3: Puzzle Completion**
```
1. Place all items in correct order
2. Success dialogue should show
3. Panel should close
✅ PASS: Puzzle logic works
❌ FAIL: Check slots assigned, Item Ids correct
```

---

## 🎉 SUMMARY

### **Most Common Issues**:

1. **Raycast Target unchecked** (90% of "can't drag" problems)
2. **No Graphic Raycaster on Canvas** (80% of "can't drag" problems)
3. **Wrong slot names** (70% of "items snap back" problems)
4. **Wrong Item Ids** (60% of "puzzle doesn't complete" problems)
5. **Slots not assigned** (50% of "puzzle doesn't complete" problems)

### **Quick Fix Order**:

1. Check Raycast Target ✓
2. Check Graphic Raycaster ✓
3. Check EventSystem ✓
4. Check slot names
5. Check Item Ids
6. Check Puzzle Numbers
7. Check references assigned

---

**TROUBLESHOOTING GUIDE COMPLETE!** 🐛✨

Most problems can be fixed by checking these common issues!

**KAYA MO YAN!** 💪🔧

**CHECK CONSOLE LOGS!** They tell you what's wrong!
