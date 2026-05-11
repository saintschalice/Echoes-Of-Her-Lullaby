# 🔍 MIRROR 1 - DEBUG GUIDE

## 🎯 WHAT TO CHECK IN CONSOLE

Pag nag-test ka, tignan mo ang Console messages para makita kung ano ang nangyayari!

---

## 📊 CONSOLE MESSAGES YOU SHOULD SEE

### **When You Drag a Bottle**:

```
[DraggableItem] Started dragging: bottle_1973
```

### **When You Drop on a Slot**:

```
[DraggableItem] bottle_1973 dropped on Slot_1
[DraggableItem] bottle_1973 placed in Slot_1
[Mirror1] 🍾 Bottle bottle_1973 placed in slot Slot_1
[Mirror1] Current slot contents:
  Slot 0 (Slot_1): bottle_1973
  Slot 1 (Slot_2): EMPTY
  Slot 2 (Slot_3): EMPTY
  Slot 3 (Slot_4): EMPTY
  Slot 4 (Slot_5): EMPTY
  Slot 5 (Slot_6): EMPTY
[Mirror1] ═══════════════════════════════════
[Mirror1] Checking solution...
[Mirror1] 📊 Filled slots: 1/6
[Mirror1] ⏳ Not all slots filled yet. Waiting for more bottles...
[Mirror1] ═══════════════════════════════════
```

### **After Placing All 6 Bottles (CORRECT Order)**:

```
[Mirror1] ═══════════════════════════════════
[Mirror1] Checking solution...
[Mirror1] 📊 Filled slots: 6/6
[Mirror1] ✅ All 6 slots are filled! Checking order...
[Mirror1] ✅ Slot 0: Expected=bottle_1973, Actual=bottle_1973
[Mirror1] ✅ Slot 1: Expected=bottle_1974, Actual=bottle_1974
[Mirror1] ✅ Slot 2: Expected=bottle_1975a, Actual=bottle_1975a
[Mirror1] ✅ Slot 3: Expected=bottle_1975b, Actual=bottle_1975b
[Mirror1] ✅ Slot 4: Expected=bottle_1976a, Actual=bottle_1976a
[Mirror1] ✅ Slot 5: Expected=bottle_1976b, Actual=bottle_1976b
[Mirror1] ═══════════════════════════════════
[Mirror1] 🎉🎉🎉 ALL BOTTLES CORRECT! PUZZLE SOLVED! 🎉🎉🎉
[Mirror1] ═══════════════════════════════════
```

### **After Placing All 6 Bottles (WRONG Order)**:

```
[Mirror1] ═══════════════════════════════════
[Mirror1] Checking solution...
[Mirror1] 📊 Filled slots: 6/6
[Mirror1] ✅ All 6 slots are filled! Checking order...
[Mirror1] ❌ Slot 0: Expected=bottle_1973, Actual=bottle_1976b
[Mirror1] ❌ Slot 1: Expected=bottle_1974, Actual=bottle_1973
[Mirror1] ❌ Slot 2: Expected=bottle_1975a, Actual=bottle_1974
[Mirror1] ❌ Slot 3: Expected=bottle_1975b, Actual=bottle_1975a
[Mirror1] ❌ Slot 4: Expected=bottle_1976a, Actual=bottle_1975b
[Mirror1] ❌ Slot 5: Expected=bottle_1976b, Actual=bottle_1976a
[Mirror1] ═══════════════════════════════════
[Mirror1] ❌ Bottles not in correct order. Keep trying...
[Mirror1] 💡 Hint: Arrange bottles chronologically (1973 → 1976)
[Mirror1] ═══════════════════════════════════
```

---

## 🐛 COMMON PROBLEMS

### **Problem 1: Puzzle Completes After 1 Bottle**

**Console Shows**:
```
[Mirror1] 📊 Filled slots: 1/6
[Mirror1] 🎉🎉🎉 ALL BOTTLES CORRECT! PUZZLE SOLVED! 🎉🎉🎉
```

**Cause**: CheckSolution() is not checking filled slots correctly

**Fix**: Make sure you have the updated Mirror1_MedicineCabinet.cs

---

### **Problem 2: Slot Not Tracked**

**Console Shows**:
```
[Mirror1] ❌ Slot Slot_1 is not in our tracked slots!
```

**Cause**: Slot GameObject is not in the bottleSlots array

**Fix**:
1. Select Mirror1_MedicineCabinet GameObject
2. Inspector → Mirror1_MedicineCabinet component
3. Bottle Slots → Make sure all 6 slots are assigned
4. Drag Slot_1, Slot_2, Slot_3, Slot_4, Slot_5, Slot_6 to array

---

### **Problem 3: Bottle ID Mismatch**

**Console Shows**:
```
[Mirror1] ❌ Slot 0: Expected=bottle_1973, Actual=bottle1973
```

**Cause**: Item Id doesn't match expected format (missing underscore)

**Fix**:
1. Select bottle GameObject
2. Inspector → DraggableItem component
3. Item Id: Make sure it's "bottle_1973" (with underscore!)
4. Check all bottles:
   - bottle_1973 ✅
   - bottle_1974 ✅
   - bottle_1975a ✅
   - bottle_1975b ✅
   - bottle_1976a ✅
   - bottle_1976b ✅

---

### **Problem 4: Slot Contents Not Updating**

**Console Shows**:
```
[Mirror1] Current slot contents:
  Slot 0 (Slot_1): EMPTY
  Slot 1 (Slot_2): EMPTY
  ... (all empty even after placing bottles)
```

**Cause**: DraggableItem not calling OnBottlePlacedInSlot()

**Fix**:
1. Check DraggableItem → Puzzle Number is 1
2. Check slot names contain "Slot" (e.g., "Slot_1", "Slot_2")
3. Check Console for "[DraggableItem] bottle_1973 dropped on Slot_1"

---

## 📋 TESTING CHECKLIST

### **Test 1: Place 1 Bottle**

```
Expected Console:
✅ "Started dragging: bottle_1973"
✅ "bottle_1973 dropped on Slot_1"
✅ "Bottle bottle_1973 placed in slot Slot_1"
✅ "Filled slots: 1/6"
✅ "Not all slots filled yet. Waiting..."
❌ Should NOT see "PUZZLE SOLVED!"
```

### **Test 2: Place All 6 (Correct Order)**

```
Expected Console:
✅ "Filled slots: 6/6"
✅ "All 6 slots are filled! Checking order..."
✅ All slots show ✅
✅ "🎉🎉🎉 ALL BOTTLES CORRECT! PUZZLE SOLVED! 🎉🎉🎉"
✅ Success dialogue shows
✅ Panel closes
```

### **Test 3: Place All 6 (Wrong Order)**

```
Expected Console:
✅ "Filled slots: 6/6"
✅ "All 6 slots are filled! Checking order..."
✅ Some slots show ❌
✅ "❌ Bottles not in correct order. Keep trying..."
❌ Should NOT see "PUZZLE SOLVED!"
❌ Panel should stay open
```

---

## 🔍 HOW TO DEBUG

### **Step 1: Open Console**

```
Unity → Window → General → Console
```

### **Step 2: Clear Console**

```
Click "Clear" button (top left of Console)
```

### **Step 3: Play Scene**

```
Click Play button
```

### **Step 4: Interact with Mirror**

```
Walk near mirror → Press E → Panel opens
```

### **Step 5: Drag Bottles**

```
Drag bottle_1973 to Slot_1
Watch Console messages
```

### **Step 6: Check Messages**

```
Look for:
✅ "Started dragging: bottle_1973"
✅ "bottle_1973 dropped on Slot_1"
✅ "Bottle bottle_1973 placed in slot Slot_1"
✅ "Filled slots: 1/6"
✅ "Not all slots filled yet. Waiting..."
```

### **Step 7: Continue Placing**

```
Place all 6 bottles
Watch Console for each placement
Check if puzzle completes correctly
```

---

## 📊 CORRECT BOTTLE ORDER

```
Slot_1 (Slot 0): bottle_1973
Slot_2 (Slot 1): bottle_1974
Slot_3 (Slot 2): bottle_1975a
Slot_4 (Slot 3): bottle_1975b
Slot_5 (Slot 4): bottle_1976a
Slot_6 (Slot 5): bottle_1976b
```

---

## ✅ WHAT TO LOOK FOR

### **GOOD Signs** ✅:

```
✅ "Filled slots: X/6" (increases with each bottle)
✅ "Not all slots filled yet. Waiting..." (when < 6)
✅ "All 6 slots are filled! Checking order..." (when = 6)
✅ Slot contents show correct bottles
✅ Puzzle only completes when all 6 correct
```

### **BAD Signs** ❌:

```
❌ "PUZZLE SOLVED!" after placing 1 bottle
❌ "Slot X is not in our tracked slots!"
❌ Slot contents always show "EMPTY"
❌ Item Id doesn't match (e.g., "bottle1973" vs "bottle_1973")
❌ Puzzle completes with wrong order
```

---

## 🆘 STILL NOT WORKING?

### **Send Me These Console Messages**:

1. Clear Console
2. Play scene
3. Place 1 bottle
4. Copy ALL Console messages
5. Send to me

**I need to see**:
- DraggableItem messages
- Mirror1 messages
- Any errors or warnings

---

**DEBUG GUIDE COMPLETE!** 🔍✨

**CHECK CONSOLE** to see what's happening!

**LOOK FOR**: "Filled slots: X/6" messages!
