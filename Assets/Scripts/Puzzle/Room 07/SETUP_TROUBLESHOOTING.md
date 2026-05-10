# Room 07 Setup Troubleshooting

## ❌ Problem: Wrong Panel Opens

**Symptom:** Interact with Cabinet, but Curtain Panel opens instead.

### Root Cause:
Wrong **ObjectType** assigned in Inspector!

---

## ✅ Solution: Check Inspector Settings

### Step 1: Select GameObject
```
In Hierarchy, select the GameObject you're having issues with
Example: Select "Cabinet" GameObject
```

### Step 2: Check Room07_Interactable Component
```
In Inspector, find Room07_Interactable component
Look at "My Type" dropdown
```

### Step 3: Verify Correct Type
```
Cabinet GameObject → My Type: Cabinet_Cup
Curtains GameObject → My Type: WindowCurtains
Bed GameObject → My Type: Bed
etc.
```

---

## 📋 Correct ObjectType for Each GameObject

### Environmental Objects:
```
Bed GameObject:
  My Type: Bed

Wall GameObject:
  My Type: WallDrawings

Bookshelf GameObject:
  My Type: Bookshelf

Chair GameObject:
  My Type: Chair

Closet GameObject:
  My Type: Closet

Reading Table GameObject:
  My Type: ReadingTable
```

### Puzzle Objects:
```
Window Curtains GameObject:
  My Type: WindowCurtains

Small Cabinet GameObject:
  My Type: Cabinet_Cup ← IMPORTANT!

Tea Party Spot GameObject:
  My Type: TeaParty

Toybox GameObject:
  My Type: Toybox

Toybox Icon GameObject (inside toybox):
  My Type: ToyboxIcon

Dollhouse GameObject:
  My Type: Dollhouse

Mirror GameObject:
  My Type: Mirror
```

---

## 🔍 How to Debug

### Method 1: Check Console Logs
```
1. Play Mode
2. Interact with object
3. Check Console (Ctrl+Shift+C)
4. Should see: "[Room07] Interacting with: Cabinet_Cup"
5. If wrong type shows, fix in Inspector
```

### Method 2: Check Inspector
```
1. Select GameObject
2. Room07_Interactable component
3. My Type dropdown
4. Change to correct type
5. Save scene (Ctrl+S)
```

---

## 🎯 Quick Fix Checklist

For each GameObject:
- [ ] Has Room07_Interactable component
- [ ] My Type is set correctly
- [ ] UI Manager is assigned (if needed)
- [ ] Scene is saved

---

## 📊 Common Mistakes

### Mistake 1: Copy-Paste Objects
```
Problem: Copied Cabinet, forgot to change My Type
Result: Both objects have same type

Fix: Always check My Type after copying
```

### Mistake 2: Prefab Override
```
Problem: Changed prefab, but instance not updated
Result: Old type still active

Fix: Apply prefab changes or update instance
```

### Mistake 3: Multiple Scripts
```
Problem: GameObject has multiple Room07_Interactable
Result: Both scripts run, wrong one triggers

Fix: Remove duplicate scripts
```

---

## 🧪 Testing Each Object

### Test Cabinet:
```
1. Interact with Cabinet
2. Console: "[Room07] Interacting with: Cabinet_Cup"
3. If curtains not opened: "The cabinet is locked..."
4. If curtains opened: Cabinet panel opens ✓
```

### Test Curtains:
```
1. Interact with Curtains
2. Console: "[Room07] Interacting with: WindowCurtains"
3. Dialogue shows
4. Curtain panel opens ✓
```

### Test Each Object:
```
For each GameObject:
1. Interact
2. Check Console log
3. Verify correct type shows
4. Verify correct action happens
```

---

## 🎮 Complete GameObject List

You need 13 GameObjects with Room07_Interactable:

1. **Bed** → ObjectType.Bed
2. **Wall** → ObjectType.WallDrawings
3. **Bookshelf** → ObjectType.Bookshelf
4. **Window Curtains** → ObjectType.WindowCurtains
5. **Small Cabinet** → ObjectType.Cabinet_Cup
6. **Tea Party Spot** → ObjectType.TeaParty
7. **Chair** → ObjectType.Chair
8. **Closet** → ObjectType.Closet
9. **Toybox** → ObjectType.Toybox
10. **Toybox Icon** → ObjectType.ToyboxIcon
11. **Dollhouse** → ObjectType.Dollhouse
12. **Reading Table** → ObjectType.ReadingTable
13. **Mirror** → ObjectType.Mirror

---

## ✅ Verification Steps

### Step 1: Check All GameObjects
```
For each of the 13 objects:
1. Select in Hierarchy
2. Check Room07_Interactable component exists
3. Check My Type is correct
4. Check UI Manager assigned (if needed)
```

### Step 2: Test Each Interaction
```
1. Play Mode
2. Interact with each object
3. Check Console for correct type
4. Verify correct action happens
```

### Step 3: Check Flow
```
1. Interact with objects in order
2. Verify dialogues show correctly
3. Verify panels open correctly
4. Verify items obtained correctly
```

---

## 🐛 Still Having Issues?

### Check These:

1. **Console Logs**
   ```
   Open Console (Ctrl+Shift+C)
   Look for: "[Room07] Interacting with: X"
   X should match what you clicked
   ```

2. **Inspector Settings**
   ```
   Select GameObject
   Room07_Interactable component
   My Type dropdown
   Should match GameObject name/purpose
   ```

3. **Scene Saved**
   ```
   After changing My Type
   Save scene (Ctrl+S)
   Test again
   ```

4. **No Duplicate Scripts**
   ```
   Select GameObject
   Should have only ONE Room07_Interactable
   Remove duplicates if any
   ```

---

## 📝 Example: Setting Up Cabinet

### Correct Setup:
```
GameObject Name: "SmallCabinet"

Components:
- Transform
- Collider (for interaction)
- Room07_Interactable
  └─ My Type: Cabinet_Cup ← CORRECT!
  └─ UI Manager: Room07_Manager
  └─ Required Item ID: (empty)
```

### Wrong Setup:
```
GameObject Name: "SmallCabinet"

Components:
- Transform
- Collider
- Room07_Interactable
  └─ My Type: WindowCurtains ← WRONG!
  
Result: Opens curtain panel instead of cabinet!
```

---

## 🎯 Quick Fix

If wrong panel opens:

1. **Find the GameObject** you interacted with
2. **Select it** in Hierarchy
3. **Check My Type** in Inspector
4. **Change to correct type**
5. **Save scene** (Ctrl+S)
6. **Test again**

---

**Always check My Type in Inspector!** 🔍

**Console logs show what type was triggered!** 📊✨
