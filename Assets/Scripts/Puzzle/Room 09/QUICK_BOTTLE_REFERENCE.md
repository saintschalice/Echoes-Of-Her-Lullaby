# 🎯 QUICK BOTTLE REFERENCE

## ✅ COPY-PASTE GUIDE

### **Bottle Item IDs** (Para sa Inspector)

```
Bottle_1973 → Item Id: bottle_1973
Bottle_1974 → Item Id: bottle_1974
Bottle_1975a → Item Id: bottle_1975a
Bottle_1975b → Item Id: bottle_1975b
Bottle_1976a → Item Id: bottle_1976a
Bottle_1976b → Item Id: bottle_1976b
```

**IMPORTANT**: 
- ✅ All lowercase
- ✅ Underscore between "bottle" and year
- ✅ Lowercase 'a' and 'b' for 1975/1976

---

## 🎯 SLOT MATCHING

```
Slot_1 (leftmost)  → ONLY accepts: bottle_1973
Slot_2             → ONLY accepts: bottle_1974
Slot_3             → ONLY accepts: bottle_1975a
Slot_4             → ONLY accepts: bottle_1975b
Slot_5             → ONLY accepts: bottle_1976a
Slot_6 (rightmost) → ONLY accepts: bottle_1976b
```

---

## 🔧 INSPECTOR SETUP

### **For Each Bottle GameObject**:

```
1. Select Bottle_1973
2. Inspector → DraggableItem Component
3. Item Id: bottle_1973
4. Puzzle Number: 1
5. Return To Original Position: ✓ Checked
6. Fade While Dragging: ✓ Checked
7. Drag Alpha: 0.6

Repeat for all 6 bottles!
```

---

## 📊 VISUAL DIAGRAM

```
LEFT                                                    RIGHT
┌────────┐ ┌────────┐ ┌────────┐ ┌────────┐ ┌────────┐ ┌────────┐
│ Slot_1 │ │ Slot_2 │ │ Slot_3 │ │ Slot_4 │ │ Slot_5 │ │ Slot_6 │
│        │ │        │ │        │ │        │ │        │ │        │
│ 1973   │ │ 1974   │ │ 1975a  │ │ 1975b  │ │ 1976a  │ │ 1976b  │
│        │ │        │ │        │ │        │ │        │ │        │
└────────┘ └────────┘ └────────┘ └────────┘ └────────┘ └────────┘
   ↑          ↑          ↑          ↑          ↑          ↑
   │          │          │          │          │          │
bottle_    bottle_    bottle_    bottle_    bottle_    bottle_
 1973       1974      1975a      1975b      1976a      1976b
```

---

## ✅ WHAT HAPPENS

### **Correct Placement**:
```
Drag bottle_1973 → Drop on Slot_1
✅ Bottle snaps to center of Slot_1
✅ Bottle becomes child of Slot_1
✅ Mistakes: 0/3 (stays same)
```

### **Wrong Placement**:
```
Drag bottle_1976b → Drop on Slot_1
❌ Bottle returns to original position
❌ Bottle stays child of Panel
❌ Mistakes: 0/3 → 1/3
```

---

## 🐛 TROUBLESHOOTING

### **Bottle doesn't stick even when correct?**

Check Item ID:
```
❌ WRONG: "bottle1973" (missing underscore)
❌ WRONG: "Bottle_1973" (uppercase B)
❌ WRONG: "bottle_1973 " (extra space)
✅ CORRECT: "bottle_1973"
```

### **All bottles rejected?**

Check Slot Array:
```
Mirror1_MedicineCabinet → Bottle Slots
Element 0: Slot_1 ← Must be in this order!
Element 1: Slot_2
Element 2: Slot_3
Element 3: Slot_4
Element 4: Slot_5
Element 5: Slot_6
```

---

**QUICK REFERENCE DONE!** ✅

