# Debug: Wrong Panel Opening

## 🐛 Problem: Toybox Panel Opens Instead of Curtain Panel

Based sa screenshot mo, ang Toybox sliding puzzle ang lumalabas instead of curtain panel.

---

## 🔍 Possible Causes

### Cause 1: Wrong Object Interacted
```
You clicked: Toybox
Expected: Window Curtains

Solution: Make sure you're clicking the correct object!
```

### Cause 2: Wrong Panel Assigned
```
Room07UIManager:
  Curtain Panel: [ToyboxPanel] ❌ WRONG!
  
Should be:
  Curtain Panel: [CurtainPanel] ✅ CORRECT!
```

### Cause 3: Panels Have Same Name
```
Both panels named "Panel"
Unity gets confused

Solution: Rename clearly:
  - CurtainPanel
  - ToyboxPanel
```

---

## ✅ Quick Fix Steps

### Step 1: Check Which Object You Clicked
```
1. Press Play
2. Look at Console when you interact
3. Should show: "[Room07] Showing Curtain Panel"
4. If shows: "[Room07] Showing Toybox Panel" = Wrong object!
```

### Step 2: Check Room07UIManager Assignment
```
1. Select Room07_Manager
2. Find Room07UIManager component
3. Check fields:
   Curtain Panel: Should be "CurtainPanel" GameObject
   Toybox Panel: Should be "ToyboxPanel" GameObject
   
4. If swapped, fix it:
   - Drag correct panels to correct fields
```

### Step 3: Check Panel Names in Hierarchy
```
Canvas
├─ CurtainPanel ← Should be named this
├─ TeaPartyPanel
├─ ToyboxPanel ← Should be named this
└─ DollhousePanel

If both named "Panel", rename them!
```

---

## 🧪 Test Each Panel

### Test Curtain Panel:
```
1. Press Play
2. Interact with Window Curtains object
3. Console: "[Room07] Showing Curtain Panel"
4. Should show: Curtain puzzle (not sliding puzzle!)
```

### Test Toybox Panel:
```
1. Press Play
2. Interact with Toybox object
3. Console: "[Room07] Showing Toybox Panel"
4. Should show: Sliding puzzle (8 tiles)
```

---

## 🔧 Fix Assignment

### If Panels Are Swapped:

**Current (WRONG):**
```
Room07UIManager:
  Curtain Panel: [ToyboxPanel] ❌
  Toybox Panel: [CurtainPanel] ❌
```

**Fixed (CORRECT):**
```
Room07UIManager:
  Curtain Panel: [CurtainPanel] ✅
  Toybox Panel: [ToyboxPanel] ✅
```

**How to Fix:**
```
1. Select Room07_Manager
2. Find Room07UIManager
3. Clear both fields (click X)
4. Drag CurtainPanel to "Curtain Panel" field
5. Drag ToyboxPanel to "Toybox Panel" field
6. Save scene
7. Test again
```

---

## 🎯 Verify Correct Setup

### In Hierarchy:
```
Canvas
├─ CurtainPanel (disabled)
│  └─ CurtainPuzzleUI script
├─ ToyboxPanel (disabled)
│  └─ ToyboxSlidingPuzzle script
```

### In Room07_Manager Inspector:
```
Room07UIManager:
  Curtain Panel: CurtainPanel ✅
  Tea Party Panel: TeaPartyPanel ✅
  Toybox Panel: ToyboxPanel ✅
  Dollhouse Panel: DollhousePanel ✅
```

### In Scene:
```
Window Curtains object:
  Room07_Interactable:
    My Type: WindowCurtains ✅
    UI Manager: Room07_Manager ✅

Toybox object:
  Room07_Interactable:
    My Type: Toybox ✅
    UI Manager: Room07_Manager ✅
```

---

## 🐛 Sliding Puzzle Tile Issue

Nakita ko rin sa screenshot: May white/blank tiles sa puzzle!

### Cause: Puzzle Image Not Assigned
```
ToyboxPanel → ToyboxSlidingPuzzle:
  Puzzle Image: None (Sprite) ❌
  
Should be:
  Puzzle Image: [Game Icon Sprite] ✅
```

### Fix:
```
1. Select ToyboxPanel
2. Find ToyboxSlidingPuzzle component
3. Find "Puzzle Image" field
4. Drag your game icon sprite
5. Test - tiles should show image pieces now
```

---

## 📝 Complete Checklist

### Panel Assignment:
- [ ] CurtainPanel exists and named correctly
- [ ] ToyboxPanel exists and named correctly
- [ ] CurtainPanel assigned to "Curtain Panel" field
- [ ] ToyboxPanel assigned to "Toybox Panel" field
- [ ] No swapped assignments

### Object Setup:
- [ ] Window Curtains has Type = WindowCurtains
- [ ] Toybox has Type = Toybox
- [ ] Both have UI Manager assigned

### Scripts:
- [ ] CurtainPanel has CurtainPuzzleUI script
- [ ] ToyboxPanel has ToyboxSlidingPuzzle script
- [ ] Puzzle Image assigned to ToyboxSlidingPuzzle

### Testing:
- [ ] Interact with Curtains → Curtain panel opens
- [ ] Interact with Toybox → Toybox panel opens
- [ ] Console shows correct panel name
- [ ] Sliding puzzle shows image (not white tiles)

---

## 🆘 Still Wrong Panel?

### Debug Steps:
```
1. Add this to Room07_Interactable.cs DoInteract():
   Debug.Log($"[Room07] Interacting with {myType}");

2. Press Play
3. Interact with object
4. Check Console:
   - Shows "WindowCurtains" = Correct object
   - Shows "Toybox" = Wrong object clicked!

3. If correct object but wrong panel:
   - Check Room07UIManager assignments
   - Make sure panels not swapped
```

---

## ✅ Expected Behavior

### Curtains:
```
Click Window Curtains
→ Console: "[Room07] Interacting with WindowCurtains"
→ Console: "[Room07] Showing Curtain Panel"
→ Panel shows: Curtain image with left/right buttons
```

### Toybox:
```
Click Toybox
→ Console: "[Room07] Interacting with Toybox"
→ Console: "[Room07] Showing Toybox Panel"
→ Panel shows: 3x3 sliding puzzle with game icon
```

---

**Check ang Room07UIManager assignments! Most likely swapped ang panels!** 🔧
