# 🛁 MIRROR 2 - UPDATED FLOW (BATHTUB DISAPPEARS)

## 🎯 NEW FLOW

```
1. Player interacts with Mirror 2
2. Panel opens → Shows BATHTUB WITH WATER + Drain Button
3. Player clicks DrainCover_Button
4. Water drains (sprite changes to empty)
5. Dialogue: "Found torn notes in drain!"
6. BATHTUB DISAPPEARS ✨
7. TORN PAGES APPEAR ✨ (4 pieces)
8. Player drags and assembles torn pages
9. Success → Mirror 2 complete!
```

---

## 📋 HIERARCHY SETUP

### **BathtubDrain_Panel Structure**:

```
BathtubDrain_Panel
├── Timer_Text
├── Bathtub_Container ← NEW! Will hide after draining
│   ├── Bathtub_Image (with water sprite)
│   └── DrainCover_Button
├── NotePieces_Container ← NEW! Will show after draining
│   ├── Note_Piece_1 (draggable)
│   ├── Note_Piece_2 (draggable)
│   ├── Note_Piece_3 (draggable)
│   └── Note_Piece_4 (draggable)
└── Assembly_Area
    ├── Slot_1
    ├── Slot_2
    ├── Slot_3
    └── Slot_4
```

---

## 🔧 UNITY SETUP

### **Step 1: Create Bathtub_Container**

```
1. In BathtubDrain_Panel, create Empty GameObject
2. Name: "Bathtub_Container"
3. Move Bathtub_Image INSIDE Bathtub_Container
4. Move DrainCover_Button INSIDE Bathtub_Container

Result:
Bathtub_Container
├── Bathtub_Image
└── DrainCover_Button
```

### **Step 2: Create NotePieces_Container**

```
1. In BathtubDrain_Panel, create Empty GameObject
2. Name: "NotePieces_Container"
3. Move all note pieces INSIDE NotePieces_Container
4. Set Active: ✗ UNCHECKED (starts hidden!)

Result:
NotePieces_Container (inactive)
├── Note_Piece_1
├── Note_Piece_2
├── Note_Piece_3
└── Note_Piece_4
```

### **Step 3: Position Containers**

#### **Bathtub_Container**:
```
Position: Center of panel
Size: Large (takes most of panel space)
```

#### **NotePieces_Container**:
```
Position: Same as Bathtub_Container (will replace it)
Layout: Arrange 4 note pieces in a scattered pattern
```

---

## 🔧 SCRIPT SETUP

### **Mirror2_BathtubDrain Component**:

```
Select: Mirror2_Controller GameObject
Inspector → Mirror2_BathtubDrain:

Bathtub Sprites:
✅ Bathtub Image: Bathtub_Image
✅ Bathtub With Water: bathtub_with_water sprite
✅ Bathtub Without Water: bathtub_empty sprite

UI References:
✅ Puzzle Panel: BathtubDrain_Panel
✅ Timer Text: Timer_Text
✅ Drain Cover Button: DrainCover_Button
✅ Bathtub Container: Bathtub_Container ← NEW!
✅ Note Pieces Container: NotePieces_Container ← NEW!
✅ Assembly Slots (4): Slot_1, Slot_2, Slot_3, Slot_4

Puzzle Settings:
✅ Time Limit: 90

Audio:
✅ Drain Open Sound: (your sound)
✅ Water Drain Sound: (your sound)
✅ Paper Rustle Sound: (your sound)
✅ Success Sound: (your sound)
✅ Emily Scream Sound: (your sound)

Success/Failure:
✅ Success Effect: (your effect)
✅ Emily Jumpscare Panel: (your panel)
```

---

## 🎨 VISUAL FLOW

### **Phase 1: Bathtub Visible**

```
┌────────────────────────────────────────┐
│  Bathtub Drain Puzzle    [Timer: 1:30]│
│                                        │
│         ┌──────────────────┐           │
│         │                  │           │
│         │   BATHTUB        │           │
│         │   (with water)   │           │
│         │                  │           │
│         │   [Drain Button] │ ← Click!  │
│         └──────────────────┘           │
│                                        │
│  Assembly Area:                        │
│  ┌────┐ ┌────┐ ┌────┐ ┌────┐          │
│  │ S1 │ │ S2 │ │ S3 │ │ S4 │          │
│  └────┘ └────┘ └────┘ └────┘          │
└────────────────────────────────────────┘
```

### **Phase 2: Water Draining (Transition)**

```
┌────────────────────────────────────────┐
│  Bathtub Drain Puzzle    [Timer: 1:28]│
│                                        │
│         ┌──────────────────┐           │
│         │                  │           │
│         │   BATHTUB        │           │
│         │   (EMPTY!)       │           │
│         │                  │           │
│         │    [drained]     │           │
│         └──────────────────┘           │
│                                        │
│  Dialogue: "Found torn notes!"         │
└────────────────────────────────────────┘
```

### **Phase 3: Torn Pages Visible**

```
┌────────────────────────────────────────┐
│  Bathtub Drain Puzzle    [Timer: 1:25]│
│                                        │
│         📄      📄                     │
│      Note_1  Note_2                    │
│                                        │
│         📄      📄                     │
│      Note_3  Note_4                    │
│                                        │
│  Assembly Area:                        │
│  ┌────┐ ┌────┐ ┌────┐ ┌────┐          │
│  │ S1 │ │ S2 │ │ S3 │ │ S4 │          │
│  └────┘ └────┘ └────┘ └────┘          │
│  ↑ Drag notes here!                    │
└────────────────────────────────────────┘
```

---

## 🎮 PLAYER EXPERIENCE

### **Step 1: See Bathtub**

```
Player: Interacts with Mirror 2
Panel: Opens
Shows: Bathtub with water + Drain button
Dialogue: "The bathtub. Water rises and falls."
```

### **Step 2: Click Drain Button**

```
Player: Clicks DrainCover_Button
Sound: Click sound
Visual: Sprite changes to empty bathtub
Sound: Water draining sound (2 seconds)
```

### **Step 3: Dialogue**

```
Dialogue: "I remove the drain cover."
Dialogue: "Torn paper pieces... hidden in the pipes."
Dialogue: "A note. Torn into pieces. I need to reassemble it."
```

### **Step 4: Bathtub Disappears, Pages Appear**

```
Visual: Bathtub_Container fades out / disappears
Visual: NotePieces_Container fades in / appears
Shows: 4 torn note pieces scattered
Player: Can now drag pieces to slots
```

### **Step 5: Assemble Notes**

```
Player: Drags Note_Piece_1 to Slot_1 ✅
Player: Drags Note_Piece_2 to Slot_2 ✅
Player: Drags Note_Piece_3 to Slot_3 ✅
Player: Drags Note_Piece_4 to Slot_4 ✅
Success: Puzzle complete!
```

---

## 🎨 LAYOUT TIPS

### **Bathtub_Container Layout**:

```
- Bathtub_Image: Large, centered
- DrainCover_Button: Positioned over drain area
- Takes up most of panel space
```

### **NotePieces_Container Layout**:

```
- 4 note pieces scattered naturally
- Not in perfect grid (looks more realistic)
- Example positions:

Note_Piece_1: (-150, 100)
Note_Piece_2: (50, 120)
Note_Piece_3: (-100, -50)
Note_Piece_4: (100, -80)
```

### **Assembly_Area Layout**:

```
- 4 slots in a row at bottom
- Clear space for assembly
- Labeled or numbered (optional)
```

---

## 🔊 AUDIO TIMING

### **Sequence**:

```
0.0s: Click drain button
0.0s: Play drain_open sound (click)
0.5s: Play water_drain sound (2-3 seconds)
0.5s: Change sprite to empty
2.0s: Dialogue starts
4.0s: Bathtub disappears
4.0s: Torn pages appear
```

---

## 🐛 TESTING CHECKLIST

### **Test 1: Initial State**

```
✅ Panel opens
✅ Bathtub_Container is visible
✅ NotePieces_Container is hidden
✅ Bathtub shows water sprite
✅ Drain button is visible
```

### **Test 2: Drain Button**

```
✅ Click drain button
✅ Drain sound plays
✅ Water drain sound plays
✅ Sprite changes to empty
✅ Button disappears
```

### **Test 3: Dialogue**

```
✅ Dialogue shows after draining
✅ "Found torn notes" message
✅ "Need to reassemble" message
```

### **Test 4: Container Swap**

```
✅ After dialogue, Bathtub_Container disappears
✅ NotePieces_Container appears
✅ 4 torn pages are visible
✅ Torn pages are draggable
```

### **Test 5: Assembly**

```
✅ Can drag note pieces to slots
✅ Correct order completes puzzle
✅ Success dialogue plays
✅ Panel closes
```

---

## 📋 QUICK SETUP CHECKLIST

### **Hierarchy**:

- [ ] Created Bathtub_Container
- [ ] Moved Bathtub_Image inside Bathtub_Container
- [ ] Moved DrainCover_Button inside Bathtub_Container
- [ ] Created NotePieces_Container
- [ ] Moved all 4 note pieces inside NotePieces_Container
- [ ] Set NotePieces_Container to inactive (unchecked)

### **Script References**:

- [ ] Assigned Bathtub Container
- [ ] Assigned Note Pieces Container
- [ ] Assigned all other references

### **Testing**:

- [ ] Bathtub visible at start
- [ ] Note pieces hidden at start
- [ ] Drain button works
- [ ] Sprite changes
- [ ] Dialogue plays
- [ ] Bathtub disappears after dialogue
- [ ] Note pieces appear after dialogue
- [ ] Can assemble notes

---

## ✅ WHAT CHANGED

### **OLD FLOW**:
```
Bathtub → Drain → Sprite change → Assemble notes
(Bathtub stays visible)
```

### **NEW FLOW**:
```
Bathtub → Drain → Sprite change → Dialogue → 
Bathtub DISAPPEARS → Note pieces APPEAR → Assemble notes
```

### **Why Better**:
```
✅ More realistic (picked up the notes from drain)
✅ Cleaner UI (bathtub doesn't clutter assembly area)
✅ Better visual flow (clear transition)
✅ More immersive (feels like collecting evidence)
```

---

**UPDATED FLOW COMPLETE!** ✅

**BATHTUB DISAPPEARS** after draining! 🛁➡️❌

**TORN PAGES APPEAR** for assembly! 📄✨

**CLEANER UI** and better flow! 🎨

