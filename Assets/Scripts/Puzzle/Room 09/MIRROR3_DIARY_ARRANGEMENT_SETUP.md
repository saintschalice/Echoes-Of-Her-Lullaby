# 📖 MIRROR 3 - DIARY ARRANGEMENT PUZZLE

## 🎯 PUZZLE CONCEPT

**Simple arrangement puzzle** - just like Mirror 2!

```
Player sees 8 diary pages scattered
Drag them to 8 numbered slots
Arrange in chronological order (1 → 8)
Complete timeline = Success!
```

---

## 📋 HIERARCHY SETUP

### **VanityTerror_Panel Structure**:

```
VanityTerror_Panel
├── Timer_Text
├── DiaryPages_Container
│   ├── DiaryPage_1 (draggable)
│   ├── DiaryPage_2 (draggable)
│   ├── DiaryPage_3 (draggable)
│   ├── DiaryPage_4 (draggable)
│   ├── DiaryPage_5 (draggable)
│   ├── DiaryPage_6 (draggable)
│   ├── DiaryPage_7 (draggable)
│   └── DiaryPage_8 (draggable)
└── Slots_Container
    ├── Slot_1 (labeled "1")
    ├── Slot_2 (labeled "2")
    ├── Slot_3 (labeled "3")
    ├── Slot_4 (labeled "4")
    ├── Slot_5 (labeled "5")
    ├── Slot_6 (labeled "6")
    ├── Slot_7 (labeled "7")
    └── Slot_8 (labeled "8")
```

---

## 🔧 UNITY SETUP

### **Step 1: Create Diary Pages**

For each page (DiaryPage_1 to DiaryPage_8):

```
1. Create UI → Image
2. Name: "DiaryPage_1" (etc.)
3. Add Component → DraggableItem
4. Item Id: "DiaryPage_1" (match GameObject name!)
5. Puzzle Number: 3
6. Detection Radius: 150
7. Return To Original Position: ✓ Checked
8. Fade While Dragging: ✓ Checked
```

### **Step 2: Create Slots**

For each slot (Slot_1 to Slot_8):

```
1. Create UI → Image
2. Name: "Slot_1" (etc.)
3. Size: 150x150 (or larger)
4. Add child Text: "1" (slot number label)
5. Background: Light color or border
```

### **Step 3: Position Elements**

#### **Diary Pages** (Scattered):
```
Arrange in a natural scattered pattern
Example positions:

DiaryPage_1: (-300, 200)
DiaryPage_2: (-100, 250)
DiaryPage_3: (100, 220)
DiaryPage_4: (300, 180)
DiaryPage_5: (-280, -50)
DiaryPage_6: (-80, -100)
DiaryPage_7: (120, -80)
DiaryPage_8: (300, -120)
```

#### **Slots** (Organized):
```
Arrange in 2 rows of 4:

Row 1: Slot_1, Slot_2, Slot_3, Slot_4
Row 2: Slot_5, Slot_6, Slot_7, Slot_8

Or in a single row if space allows
```

---

## 🔧 SCRIPT SETUP

### **Mirror3_VanityTerror Component**:

```
Create GameObject: "Mirror3_Controller"
Add Component → Mirror3_VanityTerror

Inspector:
✅ Puzzle Panel: VanityTerror_Panel
✅ Timer Text: Timer_Text
✅ Diary Slots (8):
   - Element 0: Slot_1
   - Element 1: Slot_2
   - Element 2: Slot_3
   - Element 3: Slot_4
   - Element 4: Slot_5
   - Element 5: Slot_6
   - Element 6: Slot_7
   - Element 7: Slot_8
✅ Time Limit: 90

Audio:
✅ Paper Rustle Sound: (your sound)
✅ Success Sound: (your sound)
✅ Emily Scream Sound: (your sound)

Success/Failure:
✅ Success Effect: (your effect)
✅ Emily Jumpscare Panel: (your panel)
```

---

## 🎯 CORRECT ORDER

### **Chronological Timeline**:

```
Slot_1 → DiaryPage_1 (earliest entry)
Slot_2 → DiaryPage_2
Slot_3 → DiaryPage_3
Slot_4 → DiaryPage_4
Slot_5 → DiaryPage_5
Slot_6 → DiaryPage_6
Slot_7 → DiaryPage_7
Slot_8 → DiaryPage_8 (latest entry)
```

---

## 📖 DIARY CONTENT SUGGESTIONS

### **Timeline of Mother's Descent**:

```
Page 1: "Lisa is defiant again. I must maintain discipline."
Page 2: "The child refuses to obey. Punishment is necessary."
Page 3: "Emily... I see her sometimes. A demon protecting Lisa."
Page 4: "The doctor prescribed stronger medication. Good."
Page 5: "Lisa's defiance grows worse. Emily grows stronger."
Page 6: "I cannot control them anymore. Drastic measures needed."
Page 7: "Tonight. I will end this. Both of them."
Page 8: "Rope. Pills. Knife. Everything is ready. Tonight."
```

---

## 🎨 VISUAL LAYOUT

### **Panel Design**:

```
┌────────────────────────────────────────────────────┐
│  Vanity Terror Puzzle           [Timer: 1:30]     │
│                                                    │
│  📄    📄    📄    📄                              │
│   1     2     3     4    ← Scattered diary pages  │
│                                                    │
│      📄    📄    📄    📄                          │
│       5     6     7     8                          │
│                                                    │
│  Assembly Slots:                                   │
│  ┌───┐ ┌───┐ ┌───┐ ┌───┐                          │
│  │ 1 │ │ 2 │ │ 3 │ │ 4 │                          │
│  └───┘ └───┘ └───┘ └───┘                          │
│  ┌───┐ ┌───┐ ┌───┐ ┌───┐                          │
│  │ 5 │ │ 6 │ │ 7 │ │ 8 │                          │
│  └───┘ └───┘ └───┘ └───┘                          │
│                                                    │
│  💡 Arrange diary pages chronologically            │
└────────────────────────────────────────────────────┘
```

---

## 🎮 PLAYER EXPERIENCE

### **Step 1: Examine Vanity**

```
Player: Interacts with Mirror 3
Panel: Opens
Shows: 8 scattered diary pages + 8 numbered slots
Dialogue: "The vanity mirror. Diary pages scattered around it."
Dialogue: "Mother's diary. Fragments of her descent. I need to put them in order."
```

### **Step 2: Arrange Pages**

```
Player: Reads page contents
Player: Drags DiaryPage_1 to Slot_1 ✅
Player: Drags DiaryPage_2 to Slot_2 ✅
... continues for all 8 pages
```

### **Step 3: Success**

```
System: All 8 pages in correct order!
Dialogue: "The timeline is complete. I can see it all now."
Dialogue: "Her defiance... my defiance. The discipline sessions. Emily protecting me. Mother's final plan."
Panel: Closes
Mirror 3: Complete! ✅
```

---

## 🐛 TESTING CHECKLIST

### **Test 1: Initial State**

```
✅ Panel opens
✅ 8 diary pages visible and scattered
✅ 8 slots visible and numbered
✅ Timer starts at 1:30
```

### **Test 2: Dragging**

```
✅ All 8 pages are draggable
✅ Pages snap to slots
✅ Paper rustle sound plays
✅ Can rearrange pages
```

### **Test 3: Correct Order**

```
✅ Place all 8 pages in correct order (1→8)
✅ Success dialogue plays
✅ Panel closes
✅ Mirror 3 marked complete
```

### **Test 4: Wrong Order**

```
✅ Place pages in wrong order
✅ Nothing happens (can rearrange)
✅ No success until correct order
```

### **Test 5: Timeout**

```
✅ Wait for timer to reach 0:00
✅ Emily jumpscare appears
✅ Emily attack dialogue plays
✅ Scene reloads (Game Over)
```

---

## 📋 ITEM ID REFERENCE

### **Copy-Paste for Inspector**:

```
DiaryPage_1
DiaryPage_2
DiaryPage_3
DiaryPage_4
DiaryPage_5
DiaryPage_6
DiaryPage_7
DiaryPage_8
```

**IMPORTANT**: Item IDs must match GameObject names EXACTLY!

---

## 🎨 DIARY PAGE DESIGN

### **Visual Style**:

```
- Aged paper texture
- Handwritten font
- Torn/weathered edges
- Sepia or yellowed color
- Visible text excerpt
- Page number visible (1-8)
```

### **Size**:

```
Width: 120-150
Height: 150-180
(Tall rectangle, like a diary page)
```

---

## 🔍 CONSOLE MESSAGES

### **When Page Placed**:

```
[Mirror3] Diary page DiaryPage_1 placed in slot Slot_1
[Mirror3] Checking solution...
[Mirror3] Filled slots: 1/8
[Mirror3] Not all slots filled yet
```

### **When Puzzle Complete**:

```
[Mirror3] Filled slots: 8/8
[Mirror3] Slot 0: Expected=DiaryPage_1, Actual=DiaryPage_1
[Mirror3] Slot 1: Expected=DiaryPage_2, Actual=DiaryPage_2
...
[Mirror3] Slot 7: Expected=DiaryPage_8, Actual=DiaryPage_8
[Mirror3] ✅ PUZZLE SOLVED!
```

---

## ✅ FINAL CHECKLIST

### **GameObjects**:

- [ ] 8 diary pages (DiaryPage_1 to DiaryPage_8)
- [ ] 8 slots (Slot_1 to Slot_8)
- [ ] All pages have DraggableItem component
- [ ] All Item IDs match GameObject names
- [ ] All Puzzle Numbers = 3

### **Script References**:

- [ ] Mirror3_Controller has Mirror3_VanityTerror script
- [ ] All 8 slots assigned in array (in order!)
- [ ] Timer Text assigned
- [ ] Audio clips assigned
- [ ] Emily Jumpscare Panel assigned

### **Testing**:

- [ ] Pages are draggable
- [ ] Slots detect pages
- [ ] Correct order completes puzzle
- [ ] Timer counts down
- [ ] Timeout triggers Emily attack

---

## 🎯 COMPARISON WITH MIRROR 2

### **Mirror 2** (Bathtub):
```
- 4 torn note pieces
- Reassemble mother's suicide note
- 90 seconds
```

### **Mirror 3** (Vanity):
```
- 8 diary pages
- Arrange chronologically
- 90 seconds
```

**Same mechanic, different content!** ✅

---

**MIRROR 3 SETUP COMPLETE!** 📖✨

**SIMPLE ARRANGEMENT** - just drag and drop! 🎯

**8 PAGES** in chronological order! 📄

**SAME AS MIRROR 2** - consistent gameplay! ⚖️

