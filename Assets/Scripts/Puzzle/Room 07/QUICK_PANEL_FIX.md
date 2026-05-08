# Quick Panel Fix - Room 07

## 🎯 Problem: Only Toybox Panel Shows

### Most Likely Cause:
❌ **Other panels are NOT assigned to Room07UIManager**

---

## ✅ 2-Minute Fix

### Step 1: Select Room07_Manager
```
Hierarchy → Room07_Manager → Click
```

### Step 2: Find Room07UIManager Component
```
Inspector → Scroll to Room07UIManager
```

### Step 3: Check All Fields
```
Room07UIManager:
  Curtain Panel: [____] ← Empty? Assign it!
  Tea Party Panel: [____] ← Empty? Assign it!
  Toybox Panel: [ToyboxPanel] ← This one works!
  Dollhouse Panel: [____] ← Empty? Assign it!
  Black Screen Cutscene: [____] ← Empty? Assign it!
```

### Step 4: Assign Missing Panels
```
For each empty field:
1. Click the circle button (⊙) on the right
2. Double-click the correct panel from the list
   OR
1. Drag the panel from Hierarchy
2. Drop it in the field
```

### Step 5: Test
```
Press Play → Test each panel
```

---

## 📋 Quick Reference

### Panel Names in Hierarchy:
- `CurtainPanel` → Assign to "Curtain Panel" field
- `TeaPartyPanel` → Assign to "Tea Party Panel" field
- `ToyboxPanel` → Assign to "Toybox Panel" field
- `DollhousePanel` → Assign to "Dollhouse Panel" field
- `BlackScreenCutscene` → Assign to "Black Screen Cutscene" field

---

## 🧪 Quick Test

### Test Each Panel:
1. **Curtain** - Interact with Window Curtains → Panel should open
2. **Tea Party** - Get cup, interact with Tea Party Spot → Panel should open
3. **Toybox** - Interact with Toybox → Panel should open (already works!)
4. **Dollhouse** - Get doll, interact with Dollhouse → Panel should open

---

## 🔍 How to Check if Assigned

### In Inspector:
```
✅ CORRECT:
  Curtain Panel: [CurtainPanel]
  
❌ WRONG:
  Curtain Panel: None (GameObject)
```

---

## 🆘 If Panels Don't Exist

### Create Missing Panels:
```
1. Right-click Canvas → UI → Panel
2. Rename to correct name
3. Disable it (uncheck in Inspector)
4. Add correct script:
   - CurtainPanel → Add CurtainPuzzleUI
   - TeaPartyPanel → Add TeaPartyPuzzleUI
   - DollhousePanel → Add DollhousePuzzleUI
5. Assign to Room07UIManager
```

---

## ✅ Done!

**All panels should work now!** 🎉

**If still not working, read PANEL_TROUBLESHOOTING.md** 📖
