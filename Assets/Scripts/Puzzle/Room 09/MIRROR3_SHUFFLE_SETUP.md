# 📖 MIRROR 3 - SHUFFLE & REARRANGE PUZZLE

## 🎯 NEW CONCEPT

**Pages start IN slots but in RANDOM order!**

```
1. Panel opens
2. All 8 pages are ALREADY in slots
3. BUT in WRONG/RANDOM order
4. Player drags to rearrange them
5. Correct chronological order = Success!
```

---

## 📋 HIERARCHY SETUP

### **VanityTerror_Panel Structure**:

```
VanityTerror_Panel
├── Timer_Text
└── Slots_Container
    ├── Slot_1
    │   └── DiaryPage_X ← Page already inside!
    ├── Slot_2
    │   └── DiaryPage_Y ← Different page!
    ├── Slot_3
    │   └── DiaryPage_Z ← Random page!
    ├── Slot_4
    │   └── DiaryPage_...
    ├── Slot_5
    │   └── DiaryPage_...
    ├── Slot_6
    │   └── DiaryPage_...
    ├── Slot_7
    │   └── DiaryPage_...
    └── Slot_8
        └── DiaryPage_...
```

**Key Point**: Each slot has ONE page as a child (pre-placed)

---

## 🔧 UNITY SETUP

### **Step 1: Create Slots**

```
1. Create 8 slots (Slot_1 to Slot_8)
2. Each slot:
   - UI → Image
   - Size: 150x150
   - Add label: "1", "2", "3", etc.
```

### **Step 2: Create Pages INSIDE Slots**

```
For each slot, create a page as CHILD:

Slot_1:
  1. Right-click Slot_1
  2. UI → Image
  3. Name: "DiaryPage_1" (or any page)
  4. Add Component → DraggableItem
  5. Item Id: "DiaryPage_1"
  6. Puzzle Number: 3
  7. Detection Radius: 150

Repeat for all 8 slots!
```

**IMPORTANT**: 
- Each slot should have ONE page as child
- Pages can be in any order initially
- Script will shuffle them when puzzle starts!

---

## 🎯 HOW IT WORKS

### **Initial Setup** (In Unity Editor):

```
Slot_1 → DiaryPage_1 (child)
Slot_2 → DiaryPage_2 (child)
Slot_3 → DiaryPage_3 (child)
Slot_4 → DiaryPage_4 (child)
Slot_5 → DiaryPage_5 (child)
Slot_6 → DiaryPage_6 (child)
Slot_7 → DiaryPage_7 (child)
Slot_8 → DiaryPage_8 (child)
```

### **After StartPuzzle() Runs** (Shuffled):

```
Slot_1 → DiaryPage_5 (shuffled!)
Slot_2 → DiaryPage_2 (shuffled!)
Slot_3 → DiaryPage_7 (shuffled!)
Slot_4 → DiaryPage_1 (shuffled!)
Slot_5 → DiaryPage_8 (shuffled!)
Slot_6 → DiaryPage_3 (shuffled!)
Slot_7 → DiaryPage_4 (shuffled!)
Slot_8 → DiaryPage_6 (shuffled!)
```

### **Player's Goal** (Rearrange to Correct Order):

```
Slot_1 → DiaryPage_1 ✅
Slot_2 → DiaryPage_2 ✅
Slot_3 → DiaryPage_3 ✅
Slot_4 → DiaryPage_4 ✅
Slot_5 → DiaryPage_5 ✅
Slot_6 → DiaryPage_6 ✅
Slot_7 → DiaryPage_7 ✅
Slot_8 → DiaryPage_8 ✅
```

---

## 🎮 PLAYER EXPERIENCE

### **Step 1: Panel Opens**

```
Player: Interacts with Mirror 3
Panel: Opens
Shows: 8 slots with pages already in them
BUT: Pages are in wrong order!
Dialogue: "The vanity mirror. Diary pages scattered."
Dialogue: "I need to arrange them chronologically."
```

### **Step 2: Player Reads Pages**

```
Player: Reads content of each page
Player: Figures out chronological order
Example:
- "Lisa is defiant" = Early entry (Page 1)
- "Tonight I end this" = Late entry (Page 7)
```

### **Step 3: Player Rearranges**

```
Player: Drags DiaryPage_5 from Slot_1
Player: Drops it on Slot_5
System: Pages swap positions!

Player: Continues rearranging
Player: Until all pages in correct order
```

### **Step 4: Success**

```
System: All pages in correct chronological order!
Dialogue: "The timeline is complete."
Panel: Closes
Mirror 3: Complete! ✅
```

---

## 🔧 SCRIPT BEHAVIOR

### **ShufflePages() Method**:

```csharp
1. Find all DiaryPage GameObjects (children of slots)
2. Create a list of these pages
3. Shuffle the list randomly
4. Place shuffled pages back into slots
5. Update slotContents dictionary
```

### **Result**:

```
Every time puzzle starts:
- Pages are in DIFFERENT random order
- Player must rearrange them
- Replayability!
```

---

## 🎨 VISUAL LAYOUT

### **Panel Design**:

```
┌────────────────────────────────────────────────────┐
│  Vanity Terror Puzzle           [Timer: 1:30]     │
│                                                    │
│  ┌─────┐ ┌─────┐ ┌─────┐ ┌─────┐                 │
│  │  1  │ │  2  │ │  3  │ │  4  │                 │
│  ├─────┤ ├─────┤ ├─────┤ ├─────┤                 │
│  │ P5  │ │ P2  │ │ P7  │ │ P1  │ ← Wrong order!  │
│  └─────┘ └─────┘ └─────┘ └─────┘                 │
│                                                    │
│  ┌─────┐ ┌─────┐ ┌─────┐ ┌─────┐                 │
│  │  5  │ │  6  │ │  7  │ │  8  │                 │
│  ├─────┤ ├─────┤ ├─────┤ ├─────┤                 │
│  │ P8  │ │ P3  │ │ P4  │ │ P6  │ ← Wrong order!  │
│  └─────┘ └─────┘ └─────┘ └─────┘                 │
│                                                    │
│  💡 Drag pages to rearrange chronologically       │
└────────────────────────────────────────────────────┘
```

---

## 📖 DIARY PAGE CONTENT

### **Make Content Clearly Dated**:

```
DiaryPage_1:
"January 1975
Lisa is defiant again today.
I must maintain discipline."

DiaryPage_2:
"March 1975
The child refuses to obey.
Punishment is necessary."

DiaryPage_3:
"June 1975
Emily... I see her sometimes.
A demon protecting Lisa."

DiaryPage_4:
"September 1975
Doctor prescribed stronger meds.
This will help."

DiaryPage_5:
"December 1975
Lisa's defiance grows worse.
Emily grows stronger."

DiaryPage_6:
"March 1976
I cannot control them anymore.
Drastic measures needed."

DiaryPage_7:
"May 1976
Tonight. I will end this.
Both of them."

DiaryPage_8:
"June 1976
Rope. Pills. Knife.
Everything is ready. Tonight."
```

**Dates make it CLEAR what order they should be in!**

---

## 🔍 CONSOLE MESSAGES

### **When Puzzle Starts**:

```
[Mirror3] Starting Vanity Terror puzzle
[Mirror3] Shuffling diary pages...
[Mirror3] Placed DiaryPage_5 in Slot_1
[Mirror3] Placed DiaryPage_2 in Slot_2
[Mirror3] Placed DiaryPage_7 in Slot_3
...
[Mirror3] Pages shuffled! Player must rearrange them.
```

### **When Player Rearranges**:

```
[Mirror3] Diary page DiaryPage_1 placed in slot Slot_1
[Mirror3] Checking solution...
[Mirror3] Filled slots: 8/8
[Mirror3] Slot 0: Expected=DiaryPage_1, Actual=DiaryPage_1 ✅
[Mirror3] Slot 1: Expected=DiaryPage_2, Actual=DiaryPage_5 ❌
...
[Mirror3] ❌ Wrong order! Keep trying...
```

### **When Correct**:

```
[Mirror3] Slot 0: Expected=DiaryPage_1, Actual=DiaryPage_1 ✅
[Mirror3] Slot 1: Expected=DiaryPage_2, Actual=DiaryPage_2 ✅
...
[Mirror3] Slot 7: Expected=DiaryPage_8, Actual=DiaryPage_8 ✅
[Mirror3] ✅ PUZZLE SOLVED!
```

---

## 🐛 TESTING CHECKLIST

### **Test 1: Shuffle Works**

```
✅ Play scene
✅ Start puzzle
✅ Check Console for shuffle messages
✅ Pages should be in different positions each time
```

### **Test 2: Pages Are Draggable**

```
✅ All 8 pages can be dragged
✅ Pages swap when dropped on occupied slots
✅ Paper rustle sound plays
```

### **Test 3: Correct Order**

```
✅ Arrange all pages chronologically
✅ Success dialogue plays
✅ Panel closes
✅ Mirror 3 complete
```

### **Test 4: Replayability**

```
✅ Restart puzzle multiple times
✅ Pages should shuffle differently each time
✅ Never starts in correct order
```

---

## 📋 SETUP CHECKLIST

### **Hierarchy**:

- [ ] 8 slots created (Slot_1 to Slot_8)
- [ ] Each slot has ONE diary page as child
- [ ] All pages have DraggableItem component
- [ ] All Item IDs match GameObject names
- [ ] All Puzzle Numbers = 3

### **Script**:

- [ ] Mirror3_Controller has Mirror3_VanityTerror script
- [ ] All 8 slots assigned in array (in order!)
- [ ] Timer Text assigned
- [ ] Audio clips assigned

### **Testing**:

- [ ] Pages shuffle on puzzle start
- [ ] Pages are draggable
- [ ] Correct order completes puzzle
- [ ] Different shuffle each time

---

## ✅ ADVANTAGES

### **Why This Design is Better**:

```
✅ Pages already visible (no hunting for them)
✅ Clear objective (rearrange to correct order)
✅ Dates make solution obvious
✅ Replayable (different shuffle each time)
✅ Consistent with drag-drop mechanic
✅ No empty slots to confuse player
```

---

## 🎯 QUICK SETUP STEPS

### **1. Create Slots with Pages**:

```
For i = 1 to 8:
  Create Slot_i
  Inside Slot_i, create DiaryPage_i
  Add DraggableItem to DiaryPage_i
```

### **2. Assign to Script**:

```
Mirror3_Controller:
  Diary Slots: Slot_1 to Slot_8
```

### **3. Test**:

```
Play → Start puzzle → Pages shuffle!
```

---

**SHUFFLE SYSTEM COMPLETE!** 🔀✨

**PAGES START IN SLOTS** but in random order! 📖

**PLAYER REARRANGES** to correct chronological order! 🎯

**REPLAYABLE** - different shuffle each time! 🔄

