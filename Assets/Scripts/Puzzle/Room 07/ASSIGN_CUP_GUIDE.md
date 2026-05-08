# How to Assign Cup to Hide It

## 🎯 The Yellow Cup You See

Yung yellow cup sa taas ng screen ay **scene object** - actual GameObject sa scene, hindi part ng UI panel.

---

## ✅ Step-by-Step Fix

### Step 1: Find the Cup GameObject

**Method 1: Click it in Scene View**
```
1. Switch to Scene view (not Game view)
2. Click the yellow cup
3. GameObject will be selected in Hierarchy
4. Note the name (e.g., "EmilyCup", "Cup", etc.)
```

**Method 2: Search in Hierarchy**
```
1. Click Hierarchy search box
2. Type: cup
3. Look for GameObject with Sprite Renderer
4. Should be the yellow cup
```

**Method 3: Check Common Locations**
```
Hierarchy might be:
- EmilyCup
- Cup
- SmallCabinet/Cup
- Items/EmilyCup
- Interactables/Cup
etc.
```

---

### Step 2: Assign to CabinetItemPanel

```
1. In Hierarchy, select: Cabinet_Panel
   (This is the UI panel, not the cup!)

2. Look at Inspector window

3. Find: CabinetItemPanel component

4. Find section: "Scene References"

5. You'll see: Cup In Scene: None (GameObject)

6. Drag the cup GameObject from Step 1 here

7. Should now show: Cup In Scene: EmilyCup (GameObject)
```

---

### Step 3: Save and Test

```
1. Save scene (Ctrl+S or Cmd+S)

2. Enter Play Mode

3. Open cabinet panel

4. Click cup in panel

5. Yellow cup should disappear! ✓
```

---

## 📊 Visual Guide

### Before Assignment:
```
Inspector (Cabinet_Panel selected):
┌─────────────────────────────────┐
│ CabinetItemPanel                │
├─────────────────────────────────┤
│ Scene References                │
│   Cup In Scene: None (GameObject) ← EMPTY!
└─────────────────────────────────┘

Result: Cup stays visible ❌
```

### After Assignment:
```
Inspector (Cabinet_Panel selected):
┌─────────────────────────────────┐
│ CabinetItemPanel                │
├─────────────────────────────────┤
│ Scene References                │
│   Cup In Scene: EmilyCup ✓      │ ← ASSIGNED!
└─────────────────────────────────┘

Result: Cup disappears ✓
```

---

## 🎮 What to Select

### ❌ WRONG - Don't select these:
- Cabinet_Panel (UI panel)
- Item_Image (UI image inside panel)
- Canvas
- Any UI element

### ✅ CORRECT - Select this:
- The yellow cup GameObject in the scene
- Has Sprite Renderer component
- Visible in Scene view
- Part of game world, not UI

---

## 🔍 How to Verify

### Check 1: GameObject Type
```
Select the cup
Inspector should show:
- Transform (not RectTransform)
- Sprite Renderer (not Image)
- Maybe Collider

If shows RectTransform or Image:
→ Wrong object! That's UI, not scene object
```

### Check 2: Parent
```
Scene objects are usually under:
- Scene root
- Environment
- Items
- Interactables

UI objects are under:
- Canvas
- Panels
→ Don't use these!
```

### Check 3: After Assignment
```
Inspector → CabinetItemPanel:
  Cup In Scene: Should show GameObject name
  
If still shows "None":
→ Assignment didn't work, try again
```

---

## 🧪 Testing

### Test 1: Check Assignment
```
1. Select Cabinet_Panel
2. Inspector → CabinetItemPanel
3. Cup In Scene field should NOT be "None"
4. Should show cup name
```

### Test 2: Check Console
```
1. Play Mode
2. Open cabinet
3. Console should show:
   "[CabinetItemPanel] Panel opened. Cup In Scene assigned: True"
   
If shows False:
→ Not assigned correctly!
```

### Test 3: Take Cup
```
1. Click cup in panel
2. Console should show:
   "[CabinetItemPanel] Cup in scene hidden!"
3. Yellow cup should disappear
```

---

## 🎯 Common Issues

### Issue 1: Can't Find Cup
```
Try:
1. In Scene view, zoom out
2. Look for yellow cup sprite
3. Click it
4. Check Hierarchy for selected object
```

### Issue 2: Multiple Cups
```
If you have multiple cups:
- Find the one in the cabinet area
- The one that's visible before taking
- Not the one in tea party area
```

### Issue 3: Assignment Not Saving
```
After assigning:
1. Save scene (Ctrl+S)
2. Check field still shows cup name
3. If reverts to None: Scene not saved
```

---

## 💡 Quick Checklist

- [ ] Found yellow cup GameObject in scene
- [ ] Cup has Sprite Renderer (not Image)
- [ ] Cup has Transform (not RectTransform)
- [ ] Selected Cabinet_Panel (UI)
- [ ] Found CabinetItemPanel component
- [ ] Found "Cup In Scene" field
- [ ] Dragged cup GameObject to field
- [ ] Field shows cup name (not None)
- [ ] Saved scene (Ctrl+S)
- [ ] Tested in Play Mode
- [ ] Console shows "assigned: True"
- [ ] Cup disappears after taking

---

## 🎬 Video Steps (Imagine This)

```
1. [Click yellow cup in scene]
   → Hierarchy highlights "EmilyCup"

2. [Keep it selected, find Cabinet_Panel]
   → Click Cabinet_Panel in Hierarchy

3. [Look at Inspector]
   → Find CabinetItemPanel component
   → Find "Cup In Scene" field

4. [Drag EmilyCup from Hierarchy]
   → Drop on "Cup In Scene" field
   → Field now shows "EmilyCup"

5. [Save scene]
   → Ctrl+S or File → Save

6. [Test]
   → Play Mode
   → Take cup
   → Cup disappears! ✓
```

---

**The yellow cup in your screenshot MUST be assigned to "Cup In Scene" field!**

**Select Cabinet_Panel → Inspector → Drag cup GameObject to field!** ✅

**Save scene after assigning!** 💾✨
