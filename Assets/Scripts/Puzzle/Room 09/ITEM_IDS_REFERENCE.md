# 🏷️ ROOM 09 - ITEM IDS QUICK REFERENCE

## 📋 COMPLETE LIST OF ITEM IDS AND PUZZLE NUMBERS

Use this as reference when setting up DraggableItem scripts!

---

## 🪞 MIRROR 1: MEDICINE CABINET

**Puzzle Number**: `1`

### **6 Bottles**:

| Item GameObject | Item Id | Puzzle Number | Year |
|----------------|---------|---------------|------|
| Bottle_1973 | `bottle_1973` | 1 | 1973 |
| Bottle_1974 | `bottle_1974` | 1 | 1974 |
| Bottle_1975a | `bottle_1975a` | 1 | 1975 |
| Bottle_1975b | `bottle_1975b` | 1 | 1975 |
| Bottle_1976a | `bottle_1976a` | 1 | 1976 |
| Bottle_1976b | `bottle_1976b` | 1 | 1976 |

**Correct Order**: 1973 → 1974 → 1975a → 1975b → 1976a → 1976b

---

## 🛁 MIRROR 2: BATHTUB DRAIN

**Puzzle Number**: `2`

### **4 Note Pieces**:

| Item GameObject | Item Id | Puzzle Number | Text Content |
|----------------|---------|---------------|--------------|
| Note_Piece_1 | `piece1` | 2 | "Tonight I" |
| Note_Piece_2 | `piece2` | 2 | "end this child's" |
| Note_Piece_3 | `piece3` | 2 | "suffering and" |
| Note_Piece_4 | `piece4` | 2 | "mine forever" |

**Correct Order**: piece1 → piece2 → piece3 → piece4

**Complete Note**: "Tonight I end this child's suffering and mine forever"

---

## 💄 MIRROR 3: VANITY TERROR

**Puzzle Number**: `3`

### **8 Diary Pages**:

| Item GameObject | Item Id | Puzzle Number | Content Summary |
|----------------|---------|---------------|-----------------|
| DiaryPage_1 | `page1` | 3 | "Child defied me at dinner..." |
| DiaryPage_2 | `page2` | 3 | "The defiance continues..." |
| DiaryPage_3 | `page3` | 3 | "I've increased discipline..." |
| DiaryPage_4 | `page4` | 3 | "Strange things happening..." |
| DiaryPage_5 | `page5` | 3 | "Supernatural events escalated..." |
| DiaryPage_6 | `page6` | 3 | "The presence grows bolder..." |
| DiaryPage_7 | `page7` | 3 | "I've made my preparations..." |
| DiaryPage_8 | `page8` | 3 | "Everything is ready..." |

**Correct Order**: page1 → page2 → page3 → page4 → page5 → page6 → page7 → page8

---

## 🔪 MIRROR 4: EVIDENCE SEQUENCE

**Puzzle Number**: `4`

### **4 Evidence Items**:

| Item GameObject | Item Id | Puzzle Number | Description | Flashback |
|----------------|---------|---------------|-------------|-----------|
| Evidence_Rope | `rope` | 4 | Rope used to restrain | Mother buying rope at hardware store |
| Evidence_Pills | `pills` | 4 | Pills to sedate | Mother crushing pills with mortar |
| Evidence_Knife | `knife` | 4 | Knife for murder | Mother sharpening kitchen knife |
| Evidence_Towel | `towel` | 4 | Bloody towel for cleanup | Mother preparing cleanup materials |

**Correct Order**: rope → pills → knife → towel

**Sequence Meaning**: 
1. Rope (restrain child)
2. Pills (sedate child)
3. Knife (murder child)
4. Towel (cleanup evidence)

---

## 🎯 SLOT NAMING CONVENTIONS

### **For Detection to Work**:

Slots must have one of these in their name:
- "Slot" (e.g., "Slot_1", "Slot_2")
- "Frame" (e.g., "Frame_1", "Frame_2")
- OR have tag "PuzzleSlot"

**Examples**:
```
✅ GOOD:
- Slot_1
- Slot_2
- Frame_1
- BottleSlot_1
- AssemblySlot_1

❌ BAD:
- Position1
- Place1
- Container1
```

---

## 📝 DRAGGABLEITEM SCRIPT SETTINGS

### **For Each Item**:

```csharp
DraggableItem Component:
├─ Item Id: [see tables above]
├─ Puzzle Number: [1, 2, 3, or 4]
├─ Return To Original Position: ✓ (checked)
├─ Fade While Dragging: ✓ (checked)
└─ Drag Alpha: 0.6
```

### **Example Setup**:

**Bottle_1973**:
```
Item Id: bottle_1973
Puzzle Number: 1
Return To Original Position: ✓
Fade While Dragging: ✓
Drag Alpha: 0.6
```

**Note_Piece_1**:
```
Item Id: piece1
Puzzle Number: 2
Return To Original Position: ✓
Fade While Dragging: ✓
Drag Alpha: 0.6
```

**DiaryPage_1**:
```
Item Id: page1
Puzzle Number: 3
Return To Original Position: ✓
Fade While Dragging: ✓
Drag Alpha: 0.6
```

**Evidence_Rope**:
```
Item Id: rope
Puzzle Number: 4
Return To Original Position: ✓
Fade While Dragging: ✓
Drag Alpha: 0.6
```

---

## ⚠️ IMPORTANT NOTES

### **Item Id Rules**:

1. **Must be lowercase** (e.g., "bottle_1973" not "Bottle_1973")
2. **Must match exactly** what the puzzle script expects
3. **No spaces** (use underscore: "note_piece_1" not "note piece 1")
4. **Unique per puzzle** (no duplicate IDs)

### **Puzzle Number Rules**:

1. **Must be 1, 2, 3, or 4** (corresponding to mirror number)
2. **All items in same puzzle must have same number**
3. **Used to notify correct puzzle script**

### **Common Mistakes**:

❌ Wrong Item Id: "Bottle1973" (should be "bottle_1973")
❌ Wrong Puzzle Number: 0 (should be 1-4)
❌ Missing underscore: "piece1" vs "piece_1" (depends on script)
❌ Uppercase: "ROPE" (should be "rope")

---

## 🔍 QUICK LOOKUP

### **Need to find Item Id?**

**Mirror 1 (Medicine Cabinet)**:
- bottle_1973, bottle_1974, bottle_1975a, bottle_1975b, bottle_1976a, bottle_1976b

**Mirror 2 (Bathtub Drain)**:
- piece1, piece2, piece3, piece4

**Mirror 3 (Vanity Terror)**:
- page1, page2, page3, page4, page5, page6, page7, page8

**Mirror 4 (Evidence Sequence)**:
- rope, pills, knife, towel

### **Need to find Puzzle Number?**

- Mirror 1 = Puzzle Number **1**
- Mirror 2 = Puzzle Number **2**
- Mirror 3 = Puzzle Number **3**
- Mirror 4 = Puzzle Number **4**

---

## 📋 COPY-PASTE REFERENCE

### **Mirror 1 Items**:
```
bottle_1973
bottle_1974
bottle_1975a
bottle_1975b
bottle_1976a
bottle_1976b
```

### **Mirror 2 Items**:
```
piece1
piece2
piece3
piece4
```

### **Mirror 3 Items**:
```
page1
page2
page3
page4
page5
page6
page7
page8
```

### **Mirror 4 Items**:
```
rope
pills
knife
towel
```

---

## ✅ VERIFICATION CHECKLIST

### **Before Testing**:

- [ ] All Item Ids are lowercase
- [ ] All Item Ids match this reference
- [ ] All Puzzle Numbers are 1-4
- [ ] All items in same puzzle have same Puzzle Number
- [ ] No typos in Item Ids
- [ ] No spaces in Item Ids
- [ ] All slots have "Slot" or "Frame" in name

### **During Testing**:

- [ ] Console shows correct Item Id when dragging
- [ ] Console shows correct slot name when dropping
- [ ] Items snap to slots correctly
- [ ] Puzzle detects completion

---

## 🎉 SUMMARY

**Total Items**: 22 draggable items
- 6 bottles (Mirror 1)
- 4 note pieces (Mirror 2)
- 8 diary pages (Mirror 3)
- 4 evidence items (Mirror 4)

**Puzzle Numbers**: 1, 2, 3, 4

**Item Id Format**: lowercase, underscores, no spaces

**Slot Names**: Must contain "Slot" or "Frame"

---

**QUICK REFERENCE COMPLETE!** 🏷️✨

Use this when setting up DraggableItem scripts!

**KAYA MO YAN!** 💪📝
