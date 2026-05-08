# Debug: Cup Not Hiding in Scene

## 🔍 Problem

Yung maliit na cup sa background (scene) hindi nawawala after kunin.

---

## ✅ Solution: Assign Cup GameObject

### Step 1: Find Cup in Hierarchy

```
1. Open Hierarchy window
2. Search for cup GameObject
   Possible names:
   - EmilyCup
   - Cup
   - TeaCup
   - SmallCup
   - Item_Cup
   etc.

3. This is the small cup you see in the background
   (NOT the big cup in the notification!)
```

---

### Step 2: Assign to CabinetItemPanel

```
1. Select Cabinet_Panel in Hierarchy
2. Look at Inspector
3. Find CabinetItemPanel component
4. Find "Scene References" section
5. Drag cup GameObject to "Cup In Scene" field
```

**Visual:**
```
Inspector:
┌─────────────────────────────┐
│ CabinetItemPanel            │
├─────────────────────────────┤
│ Scene References            │
│   Cup In Scene: [EMPTY]    │ ← Drag cup here!
│                             │
│ Item Display                │
│   Item Image: [assigned]   │
│   ...                       │
└─────────────────────────────┘
```

---

### Step 3: Check Console Logs

After assigning, test again and check Console:

```
✅ GOOD (Cup assigned):
[CabinetItemPanel] Panel opened. Cup In Scene assigned: True
[CabinetItemPanel] Cup in scene hidden!

❌ BAD (Cup NOT assigned):
[CabinetItemPanel] Panel opened. Cup In Scene assigned: False
[CabinetItemPanel] Cup In Scene is not assigned! Cup will not be hidden.
```

---

## 🎯 How to Find the Correct Cup

### Method 1: Search in Hierarchy
```
1. Click Hierarchy search box
2. Type "cup"
3. Look for cup GameObject (not UI elements)
4. Should have Sprite Renderer or Mesh Renderer
```

### Method 2: Click in Scene View
```
1. In Scene view, click the small cup
2. GameObject will be selected in Hierarchy
3. That's the one you need!
```

### Method 3: Check Parent
```
Cup might be child of:
- SmallCabinet
- Cabinet
- Items
- Interactables
etc.

Expand parent objects to find it
```

---

## 🐛 Common Mistakes

### Mistake 1: Assigned Wrong Object
```
❌ Assigned Cabinet GameObject
❌ Assigned UI Image
❌ Assigned Panel

✅ Should assign: Actual cup GameObject in scene
```

### Mistake 2: Cup is UI Element
```
If cup is part of UI (Canvas child):
- It's not a scene object
- It's part of the panel
- Don't need to hide it

Only hide 3D/2D scene objects!
```

### Mistake 3: Multiple Cups
```
If you have multiple cups:
- Assign the one in the cabinet
- Not the one in tea party
- Not the one in inventory
```

---

## 🧪 Testing Steps

### Test 1: Check Assignment
```
1. Select Cabinet_Panel
2. Inspector → CabinetItemPanel
3. Cup In Scene field should NOT be empty
4. Should show cup GameObject name
```

### Test 2: Check Console
```
1. Play Mode
2. Open cabinet panel
3. Check Console:
   "Cup In Scene assigned: True" ✓
```

### Test 3: Take Cup
```
1. Click cup in panel
2. Check Console:
   "Cup in scene hidden!" ✓
3. Look at scene - cup should disappear ✓
```

---

## 📊 Scene Structure

```
Hierarchy:
├── Room07_Manager
├── SmallCabinet (collider)
│   └── EmilyCup ← This one!
│       └── Sprite Renderer
│
Canvas:
└── Cabinet_Panel
    └── CabinetItemPanel
        └── Cup In Scene: [EmilyCup] ← Assign!
```

---

## 🎯 Quick Checklist

- [ ] Found cup GameObject in Hierarchy
- [ ] Cup has Sprite/Mesh Renderer
- [ ] Selected Cabinet_Panel
- [ ] Found CabinetItemPanel component
- [ ] Dragged cup to "Cup In Scene" field
- [ ] Field shows cup name (not empty)
- [ ] Saved scene (Ctrl+S)
- [ ] Tested in Play Mode
- [ ] Console shows "Cup In Scene assigned: True"
- [ ] Console shows "Cup in scene hidden!"
- [ ] Cup disappears from scene

---

## 💡 Alternative: Disable Instead of Hide

If hiding doesn't work, you can also try disabling the Sprite Renderer:

```csharp
// Instead of:
cupInScene.SetActive(false);

// Try:
SpriteRenderer sr = cupInScene.GetComponent<SpriteRenderer>();
if (sr != null) sr.enabled = false;
```

But current method should work if assigned correctly!

---

## 🔍 Debug Commands

Add these to check in Play Mode:

```csharp
// In Console, type:
Debug.Log(GameObject.Find("EmilyCup"));
// Should show the cup GameObject

Debug.Log(GameObject.Find("EmilyCup").activeSelf);
// Should show True before taking, False after
```

---

**Most common issue: Cup In Scene field is EMPTY!**

**Check Inspector - field should show cup name!** ✅

**Check Console logs to see if cup is assigned!** 🔍✨
