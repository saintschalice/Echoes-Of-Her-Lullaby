# 🎯 BOTTLE TO SLOT MATCHING - EXACT GUIDE

## ✅ TAMANG BOTTLES SA BAWAT SLOT

### **SLOT 1** (Leftmost/Unang Slot)
```
✅ ACCEPTS: bottle_1973
❌ REJECTS: lahat ng iba
```

### **SLOT 2** (Second Slot)
```
✅ ACCEPTS: bottle_1974
❌ REJECTS: lahat ng iba
```

### **SLOT 3** (Third Slot)
```
✅ ACCEPTS: bottle_1975a
❌ REJECTS: lahat ng iba
```

### **SLOT 4** (Fourth Slot)
```
✅ ACCEPTS: bottle_1975b
❌ REJECTS: lahat ng iba
```

### **SLOT 5** (Fifth Slot)
```
✅ ACCEPTS: bottle_1976a
❌ REJECTS: lahat ng iba
```

### **SLOT 6** (Rightmost/Huling Slot)
```
✅ ACCEPTS: bottle_1976b
❌ REJECTS: lahat ng iba
```

---

## 📊 VISUAL DIAGRAM

```
┌─────────┐  ┌─────────┐  ┌─────────┐  ┌─────────┐  ┌─────────┐  ┌─────────┐
│ SLOT 1  │  │ SLOT 2  │  │ SLOT 3  │  │ SLOT 4  │  │ SLOT 5  │  │ SLOT 6  │
├─────────┤  ├─────────┤  ├─────────┤  ├─────────┤  ├─────────┤  ├─────────┤
│         │  │         │  │         │  │         │  │         │  │         │
│ bottle_ │  │ bottle_ │  │ bottle_ │  │ bottle_ │  │ bottle_ │  │ bottle_ │
│  1973   │  │  1974   │  │ 1975a   │  │ 1975b   │  │ 1976a   │  │ 1976b   │
│         │  │         │  │         │  │         │  │         │  │         │
└─────────┘  └─────────┘  └─────────┘  └─────────�┘  └─────────┘  └─────────┘
```

---

## 🔧 UNITY SETUP - ENSURE BOTTLES "STICK" TO SLOTS

### **Problem**: Bottle returns to original position even when correct

### **Solution**: Make sure DraggableItem places bottle INSIDE slot

---

## 📝 CURRENT CODE CHECK

### **DraggableItem.cs - PlaceInSlot() Method**

Tignan mo kung ganito ang code:

```csharp
private void PlaceInSlot(GameObject slot)
{
    // Move to slot
    transform.SetParent(slot.transform);
    rectTransform.anchoredPosition = Vector2.zero;
    
    Debug.Log($"[DraggableItem] {itemId} placed in {slot.name}");
}
```

**This should make the bottle "stick" inside the slot!**

---

## 🐛 IF BOTTLE DOESN'T STICK

### **Check 1: Is Validation Returning True?**

Console should show:
```
[Mirror1] ✅ CORRECT! bottle_1973 belongs in slot 0
[DraggableItem] bottle_1973 placed in Slot_1
```

If you see:
```
[Mirror1] ❌ WRONG! Slot 0 expects bottle_1973, got bottle_1973
```

**Problem**: Item ID doesn't match exactly!

---

### **Check 2: Verify Item IDs**

Select each bottle GameObject in Unity:

```
Bottle_1973:
  Inspector → DraggableItem
  Item Id: "bottle_1973"  ← MUST BE EXACTLY THIS (with underscore!)

Bottle_1974:
  Item Id: "bottle_1974"

Bottle_1975a:
  Item Id: "bottle_1975a"  ← Note: lowercase 'a'

Bottle_1975b:
  Item Id: "bottle_1975b"  ← Note: lowercase 'b'

Bottle_1976a:
  Item Id: "bottle_1976a"

Bottle_1976b:
  Item Id: "bottle_1976b"
```

**COMMON MISTAKES**:
- ❌ "bottle1973" (missing underscore)
- ❌ "bottle_1975A" (uppercase A instead of lowercase a)
- ❌ "Bottle_1973" (uppercase B)
- ❌ "bottle_1973 " (extra space at end)

---

### **Check 3: Verify Slot Names**

Select each slot GameObject in Unity:

```
Slot_1 ✅ (exactly this name)
Slot_2 ✅
Slot_3 ✅
Slot_4 ✅
Slot_5 ✅
Slot_6 ✅
```

**COMMON MISTAKES**:
- ❌ "Slot1" (missing underscore)
- ❌ "slot_1" (lowercase s)
- ❌ "Slot 1" (space instead of underscore)

---

### **Check 4: Verify Slot Array in Inspector**

Select **Mirror1_MedicineCabinet** GameObject:

```
Inspector → Mirror1_MedicineCabinet Component
→ Bottle Slots (Size: 6)

Element 0: Slot_1  ← MUST be in this order!
Element 1: Slot_2
Element 2: Slot_3
Element 3: Slot_4
Element 4: Slot_5
Element 5: Slot_6
```

**Order matters!** Element 0 = Slot_1, Element 1 = Slot_2, etc.

---

## 🎯 TESTING GUIDE

### **Test 1: Correct Bottle in Correct Slot**

```
1. Play scene
2. Interact with Mirror 1
3. Drag bottle_1973 to Slot_1
4. Expected:
   ✅ Bottle snaps to center of Slot_1
   ✅ Bottle becomes child of Slot_1 in Hierarchy
   ✅ Console: "✅ CORRECT! bottle_1973 belongs in slot 0"
   ✅ Console: "bottle_1973 placed in Slot_1"
   ✅ Mistakes counter stays 0/3
```

### **Test 2: Wrong Bottle in Slot**

```
1. Drag bottle_1976b to Slot_1
2. Expected:
   ✅ Bottle returns to original position
   ✅ Bottle stays as child of Panel (not Slot_1)
   ✅ Console: "❌ WRONG! Slot 0 expects bottle_1973, got bottle_1976b"
   ✅ Console: "placement rejected - returning to original position"
   ✅ Mistakes counter: 0/3 → 1/3
```

### **Test 3: All 6 Bottles Correct**

```
1. Place bottle_1973 in Slot_1 ✅
2. Place bottle_1974 in Slot_2 ✅
3. Place bottle_1975a in Slot_3 ✅
4. Place bottle_1975b in Slot_4 ✅
5. Place bottle_1976a in Slot_5 ✅
6. Place bottle_1976b in Slot_6 ✅
7. Expected:
   ✅ All bottles stay in slots
   ✅ Console: "🎉🎉🎉 ALL BOTTLES CORRECT! PUZZLE SOLVED! 🎉🎉🎉"
   ✅ Success dialogue plays
   ✅ Panel closes
```

---

## 🔍 HIERARCHY CHECK

### **Before Placing Bottles**:

```
MedicineCabinet_Panel
├── Slots_Container
│   ├── Slot_1 (empty)
│   ├── Slot_2 (empty)
│   ├── Slot_3 (empty)
│   ├── Slot_4 (empty)
│   ├── Slot_5 (empty)
│   └── Slot_6 (empty)
├── Bottle_1973 ← Child of Panel
├── Bottle_1974 ← Child of Panel
├── Bottle_1975a ← Child of Panel
├── Bottle_1975b ← Child of Panel
├── Bottle_1976a ← Child of Panel
└── Bottle_1976b ← Child of Panel
```

### **After Placing bottle_1973 in Slot_1 (CORRECT)**:

```
MedicineCabinet_Panel
├── Slots_Container
│   ├── Slot_1
│   │   └── Bottle_1973 ← NOW CHILD OF SLOT! ✅
│   ├── Slot_2 (empty)
│   ├── Slot_3 (empty)
│   ├── Slot_4 (empty)
│   ├── Slot_5 (empty)
│   └── Slot_6 (empty)
├── Bottle_1974 ← Still child of Panel
├── Bottle_1975a
├── Bottle_1975b
├── Bottle_1976a
└── Bottle_1976b
```

**Key Point**: Correct bottle becomes **child of slot** in Hierarchy!

### **After Trying to Place bottle_1976b in Slot_1 (WRONG)**:

```
MedicineCabinet_Panel
├── Slots_Container
│   ├── Slot_1
│   │   └── Bottle_1973 (still here)
│   ├── Slot_2 (empty)
│   └── ...
├── Bottle_1974
├── Bottle_1975a
├── Bottle_1975b
├── Bottle_1976a
└── Bottle_1976b ← STAYS AS CHILD OF PANEL! ✅
```

**Key Point**: Wrong bottle stays **child of Panel** (returns to original position)!

---

## 📋 QUICK REFERENCE TABLE

| Slot GameObject | Slot Index | Accepts ONLY | Item ID (exact) |
|----------------|------------|--------------|-----------------|
| Slot_1         | 0          | bottle_1973  | "bottle_1973"   |
| Slot_2         | 1          | bottle_1974  | "bottle_1974"   |
| Slot_3         | 2          | bottle_1975a | "bottle_1975a"  |
| Slot_4         | 3          | bottle_1975b | "bottle_1975b"  |
| Slot_5         | 4          | bottle_1976a | "bottle_1976a"  |
| Slot_6         | 5          | bottle_1976b | "bottle_1976b"  |

---

## 🔧 IF BOTTLE STILL DOESN'T STICK

### **Debug Steps**:

1. **Clear Console**
2. **Play Scene**
3. **Drag bottle_1973 to Slot_1**
4. **Check Console Messages**:

```
Expected messages:
✅ "[DraggableItem] Started dragging: bottle_1973"
✅ "[DraggableItem] bottle_1973 dropped on Slot_1"
✅ "[Mirror1] 🍾 Validating bottle bottle_1973 for slot Slot_1"
✅ "[Mirror1] ✅ CORRECT! bottle_1973 belongs in slot 0"
✅ "[DraggableItem] bottle_1973 placed in Slot_1"
```

5. **Check Hierarchy**:
   - Bottle_1973 should now be child of Slot_1

6. **If bottle returns to Panel**:
   - Check Item ID (must be exactly "bottle_1973")
   - Check Console for "❌ WRONG!" message
   - Check if validation is returning false

---

## 🎯 VALIDATION LOGIC

### **How It Works**:

```csharp
// In Mirror1_MedicineCabinet.cs
public bool ValidateAndPlaceBottle(GameObject slot, string bottleId)
{
    // Get slot index (0-5)
    int slotIndex = GetSlotIndex(slot);
    
    // Expected bottles for each slot
    string[] correctOrder = { 
        "bottle_1973",  // Slot 0 (Slot_1)
        "bottle_1974",  // Slot 1 (Slot_2)
        "bottle_1975a", // Slot 2 (Slot_3)
        "bottle_1975b", // Slot 3 (Slot_4)
        "bottle_1976a", // Slot 4 (Slot_5)
        "bottle_1976b"  // Slot 5 (Slot_6)
    };
    
    // Check if bottle matches expected bottle for this slot
    if (bottleId == correctOrder[slotIndex])
    {
        // CORRECT! Accept placement
        return true; // DraggableItem will place bottle in slot
    }
    else
    {
        // WRONG! Reject placement
        mistakeCount++;
        return false; // DraggableItem will return bottle to original position
    }
}
```

---

## ✅ FINAL CHECKLIST

### **Before Testing**:

- [ ] All bottle Item IDs are correct (with underscore, lowercase a/b)
- [ ] All slot names are correct (Slot_1 to Slot_6)
- [ ] Bottle Slots array has 6 elements in correct order
- [ ] Bottles are children of Panel (not Slots_Container)
- [ ] Slots are children of Slots_Container
- [ ] DraggableItem → Puzzle Number = 1 on all bottles
- [ ] DraggableItem → Return To Original Position = ✓ Checked

### **During Testing**:

- [ ] Correct bottle snaps to slot
- [ ] Correct bottle becomes child of slot in Hierarchy
- [ ] Wrong bottle returns to original position
- [ ] Wrong bottle stays child of Panel in Hierarchy
- [ ] Mistakes counter updates on wrong placement
- [ ] Console shows validation messages

### **Success Criteria**:

- [ ] All 6 bottles can be placed in correct slots
- [ ] Wrong bottles are rejected
- [ ] Puzzle completes when all 6 correct
- [ ] 3 wrong placements = Emily attack

---

**MATCHING COMPLETE!** ✅

**SLOT 1** → bottle_1973 ✅
**SLOT 2** → bottle_1974 ✅
**SLOT 3** → bottle_1975a ✅
**SLOT 4** → bottle_1975b ✅
**SLOT 5** → bottle_1976a ✅
**SLOT 6** → bottle_1976b ✅

**CORRECT BOTTLES STICK** to slots! 🎯
**WRONG BOTTLES RETURN** to original position! ↩️

