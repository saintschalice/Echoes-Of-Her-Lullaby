# Inspector Setup Checklist - Room 07

## 🎯 Quick Reference: What Type for What Object

Copy this list and check as you setup each GameObject!

---

## ✅ GameObject Setup Checklist

### 1. Bed
```
GameObject Name: Bed (or ChildBed, etc.)
Room07_Interactable:
  ☐ My Type: Bed
  ☐ UI Manager: (not needed)
  ☐ Required Item ID: (empty)
```

### 2. Wall Drawings
```
GameObject Name: Wall (or WallDrawings, etc.)
Room07_Interactable:
  ☐ My Type: WallDrawings
  ☐ UI Manager: (not needed)
  ☐ Required Item ID: (empty)
```

### 3. Bookshelf
```
GameObject Name: Bookshelf (or Nightstand, etc.)
Room07_Interactable:
  ☐ My Type: Bookshelf
  ☐ UI Manager: (not needed)
  ☐ Required Item ID: (empty)
```

### 4. Window Curtains
```
GameObject Name: WindowCurtains (or Curtains, etc.)
Room07_Interactable:
  ☐ My Type: WindowCurtains
  ☐ UI Manager: Room07_Manager
  ☐ Required Item ID: (empty)
```

### 5. Small Cabinet
```
GameObject Name: SmallCabinet (or Cabinet, etc.)
Room07_Interactable:
  ☐ My Type: Cabinet_Cup ← IMPORTANT!
  ☐ UI Manager: Room07_Manager
  ☐ Required Item ID: (empty)
```

### 6. Tea Party Spot
```
GameObject Name: TeaPartySpot (or TeaSet, etc.)
Room07_Interactable:
  ☐ My Type: TeaParty
  ☐ UI Manager: Room07_Manager
  ☐ Required Item ID: emily_cup
```

### 7. Chair
```
GameObject Name: EmilyChair (or Chair, etc.)
Room07_Interactable:
  ☐ My Type: Chair
  ☐ UI Manager: (not needed)
  ☐ Required Item ID: (empty)
```

### 8. Closet
```
GameObject Name: Closet
Room07_Interactable:
  ☐ My Type: Closet
  ☐ UI Manager: (not needed)
  ☐ Required Item ID: (empty)
```

### 9. Toybox
```
GameObject Name: Toybox
Room07_Interactable:
  ☐ My Type: Toybox
  ☐ UI Manager: Room07_Manager
  ☐ Required Item ID: (empty)
```

### 10. Toybox Icon
```
GameObject Name: ToyboxIcon (child of Toybox)
Room07_Interactable:
  ☐ My Type: ToyboxIcon
  ☐ UI Manager: Room07_Manager
  ☐ Required Item ID: (empty)
```

### 11. Dollhouse
```
GameObject Name: Dollhouse
Room07_Interactable:
  ☐ My Type: Dollhouse
  ☐ UI Manager: Room07_Manager
  ☐ Required Item ID: emily_doll
```

### 12. Reading Table
```
GameObject Name: ReadingTable (or Table, etc.)
Room07_Interactable:
  ☐ My Type: ReadingTable
  ☐ UI Manager: (not needed)
  ☐ Required Item ID: (empty)
```

### 13. Mirror
```
GameObject Name: Mirror
Room07_Interactable:
  ☐ My Type: Mirror
  ☐ UI Manager: (not needed)
  ☐ Required Item ID: (empty)
```

---

## 🎮 Room07_Manager Setup

```
GameObject Name: Room07_Manager (empty GameObject)

Components:
☐ Room07_FlowController
☐ Room07UIManager
  ☐ Curtain Panel: [assign]
  ☐ Cabinet Panel: [assign]
  ☐ Tea Party Panel: [assign]
  ☐ Toybox Panel: [assign]
  ☐ Dollhouse Panel: [assign]
  ☐ Black Screen Cutscene: [assign]
```

---

## 📊 Quick Verification

### Test Each Object:

```
☐ Bed → Shows bed dialogue
☐ Wall → Shows wall dialogue
☐ Bookshelf → Shows diary dialogue (2 parts)
☐ Curtains → Opens curtain panel
☐ Cabinet → Opens cabinet panel (after curtains)
☐ Tea Party → Opens tea party panel (with cup)
☐ Chair → Shows chair dialogue
☐ Closet → Shows closet dialogue
☐ Toybox → Shows toybox dialogue (first tap)
☐ Toybox Icon → Opens puzzle panel (second tap)
☐ Dollhouse → Opens dollhouse panel (with doll)
☐ Reading Table → Shows reading table dialogue
☐ Mirror → Triggers climax (when all complete)
```

---

## 🐛 Common Mistakes

### ❌ Wrong Type Assigned
```
Cabinet has My Type: WindowCurtains
Result: Opens curtain panel instead!

Fix: Change to Cabinet_Cup
```

### ❌ Missing UI Manager
```
Curtains has My Type: WindowCurtains
But UI Manager is empty
Result: Panel doesn't open!

Fix: Assign Room07_Manager
```

### ❌ Wrong Required Item ID
```
Tea Party has Required Item ID: emily_doll
Result: Won't work with cup!

Fix: Change to emily_cup
```

---

## 🎯 Priority Setup Order

### Phase 1: Environmental Objects (No panels)
```
1. Bed
2. Wall
3. Bookshelf
4. Chair
5. Closet
6. Reading Table
```
These are simple - just dialogue, no panels needed.

### Phase 2: Puzzle Objects (Need panels)
```
7. Curtains (needs Curtain Panel)
8. Cabinet (needs Cabinet Panel)
9. Tea Party (needs Tea Party Panel)
10. Toybox (needs Toybox Panel)
11. Toybox Icon (same panel)
12. Dollhouse (needs Dollhouse Panel)
13. Mirror (needs MirrorJumpscareSequence)
```
These need UI panels assigned.

---

## ✅ Final Verification

### Check Console Logs:
```
Play Mode → Interact with each object
Console should show:
"[Room07] Interacting with: Bed"
"[Room07] Interacting with: Cabinet_Cup"
etc.

If wrong type shows → Fix in Inspector!
```

### Check Panels Open:
```
Curtains → Curtain Panel opens
Cabinet → Cabinet Panel opens
Tea Party → Tea Party Panel opens
Toybox Icon → Toybox Panel opens
Dollhouse → Dollhouse Panel opens
```

### Check Flow:
```
Can't open cabinet without curtains
Can't do tea party without cup
Can't do dollhouse without doll
Mirror only works when all complete
```

---

## 📝 Print This Checklist!

Print this page and check off each item as you setup in Unity!

---

**Double-check My Type for each GameObject!** ✅

**Console logs will show what type was triggered!** 🔍✨
