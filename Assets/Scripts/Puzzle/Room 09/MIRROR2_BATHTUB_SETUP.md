# 🛁 MIRROR 2 - BATHTUB DRAIN PUZZLE SETUP

## 🎯 PUZZLE FLOW

```
1. Player interacts with Mirror 2
2. Panel opens showing bathtub WITH WATER
3. Player clicks DrainCover_Button
4. Water drains (sprite changes to EMPTY bathtub)
5. Dialogue: "Found torn notes in the drain!"
6. Player drags 4 note pieces to correct slots
7. Complete note reveals mother's suicide plan
8. Success → Mirror 2 complete!
```

---

## 📋 YOUR GAMEOBJECTS (From Screenshot)

### **BathtubDrain_Panel**:
```
├── Timer_Text
├── Bathtub_Image ← Changes sprite when drained!
├── DrainCover_Button ← Click to drain water
├── Assembly_Area
│   ├── Slot_1
│   ├── Slot_2
│   ├── Slot_3
│   └── Slot_4
├── Note_Piece_1 ← Draggable
├── Note_Piece_2 ← Draggable
├── Note_Piece_3 ← Draggable
└── Note_Piece_4 ← Draggable
```

---

## 🔧 UNITY SETUP

### **Step 1: Prepare Bathtub Sprites**

Kailangan mo ng **2 sprites** para sa bathtub:

```
1. bathtub_with_water.png ← Initial state
2. bathtub_empty.png ← After draining
```

**Import to Unity**:
```
1. Drag both sprites to Assets/Art/Sprites/
2. Set Texture Type: Sprite (2D and UI)
3. Apply
```

---

### **Step 2: Setup Bathtub_Image**

```
1. Select: Bathtub_Image
2. Inspector → Image Component
3. Source Image: bathtub_with_water
4. Preserve Aspect: ✓ Checked (optional)
```

---

### **Step 3: Setup DrainCover_Button**

```
1. Select: DrainCover_Button
2. Inspector → Button Component
3. Interactable: ✓ Checked
4. Transition: Color Tint (or your preference)
5. Add Image: drain_cover sprite
```

**Position**: Place over the drain area on bathtub image

---

### **Step 4: Setup Note Pieces**

For each note piece (Note_Piece_1 to Note_Piece_4):

```
1. Select: Note_Piece_1
2. Add Component → DraggableItem
3. Item Id: "Note_Piece_1" (exact match!)
4. Puzzle Number: 2
5. Detection Radius: 150
6. Return To Original Position: ✓ Checked
7. Fade While Dragging: ✓ Checked

Repeat for:
- Note_Piece_2 → Item Id: "Note_Piece_2"
- Note_Piece_3 → Item Id: "Note_Piece_3"
- Note_Piece_4 → Item Id: "Note_Piece_4"
```

---

### **Step 5: Setup Assembly Slots**

```
1. Select: Slot_1
2. Make sure it has Image component
3. Size: 150x150 (or larger for easier detection)
4. Name must be: "Slot_1" (exact!)

Repeat for Slot_2, Slot_3, Slot_4
```

---

### **Step 6: Setup Mirror2_BathtubDrain Script**

```
1. Create empty GameObject: "Mirror2_Controller"
2. Add Component → Mirror2_BathtubDrain
3. Inspector → Mirror2_BathtubDrain Component:
```

#### **Bathtub Sprites**:
```
Bathtub Image: Bathtub_Image
Bathtub With Water: bathtub_with_water sprite
Bathtub Without Water: bathtub_empty sprite
```

#### **UI References**:
```
Puzzle Panel: BathtubDrain_Panel
Timer Text: Timer_Text
Drain Cover Button: DrainCover_Button
Assembly Slots (Size: 4):
  - Element 0: Slot_1
  - Element 1: Slot_2
  - Element 2: Slot_3
  - Element 3: Slot_4
```

#### **Puzzle Settings**:
```
Time Limit: 90 (90 seconds)
```

#### **Audio**:
```
Drain Open Sound: (your drain open sound)
Water Drain Sound: (your water draining sound)
Paper Rustle Sound: (your paper sound)
Success Sound: (your success sound)
Emily Scream Sound: (your Emily scream)
```

#### **Success/Failure**:
```
Success Effect: (your success particle/glow)
Emily Jumpscare Panel: (your Emily jumpscare panel)
```

---

## 🎨 VISUAL LAYOUT

### **Panel Layout**:

```
┌────────────────────────────────────────┐
│  Bathtub Drain Puzzle    [Timer: 1:30]│
│                                        │
│         ┌──────────────────┐           │
│         │                  │           │
│         │   BATHTUB        │           │
│         │   (with water)   │           │
│         │                  │           │
│         │      [Drain]     │ ← Button  │
│         └──────────────────┘           │
│                                        │
│  Assembly Area:                        │
│  ┌────┐ ┌────┐ ┌────┐ ┌────┐          │
│  │ S1 │ │ S2 │ │ S3 │ │ S4 │          │
│  └────┘ └────┘ └────┘ └────┘          │
│                                        │
│  📄    📄    📄    📄                  │
│  P1    P2    P3    P4                  │
└────────────────────────────────────────┘
```

### **After Draining**:

```
┌────────────────────────────────────────┐
│  Bathtub Drain Puzzle    [Timer: 1:15]│
│                                        │
│         ┌──────────────────┐           │
│         │                  │           │
│         │   BATHTUB        │           │
│         │   (EMPTY!)       │           │
│         │                  │           │
│         │    [drained]     │           │
│         └──────────────────┘           │
│                                        │
│  Assembly Area:                        │
│  ┌────┐ ┌────┐ ┌────┐ ┌────┐          │
│  │ S1 │ │ S2 │ │ S3 │ │ S4 │          │
│  └────┘ └────┘ └────┘ └────┘          │
│                                        │
│  📄    📄    📄    📄                  │
│  P1    P2    P3    P4                  │
│  ↑ Drag these to slots above!          │
└────────────────────────────────────────┘
```

---

## 🎯 CORRECT NOTE ORDER

### **The Torn Note Message**:

```
"Tonight I end this child's suffering and mine - forever."
```

### **Correct Sequence**:

```
Slot_1 → Note_Piece_1 ("Tonight I")
Slot_2 → Note_Piece_2 ("end this child's")
Slot_3 → Note_Piece_3 ("suffering and")
Slot_4 → Note_Piece_4 ("mine - forever")
```

---

## 🎮 PLAYER EXPERIENCE

### **Step 1: Examine Bathtub**

```
Player: Interacts with Mirror 2
Panel: Opens showing bathtub with water
Dialogue: "The bathtub. Water rises and falls. Something's in the drain."
```

### **Step 2: Drain Water**

```
Player: Clicks DrainCover_Button
Sound: Drain opens (click sound)
Visual: Bathtub sprite changes to EMPTY
Sound: Water draining sound
Dialogue: "I remove the drain cover. Torn paper pieces... hidden in the pipes."
Dialogue: "A note. Torn into pieces. I need to reassemble it."
```

### **Step 3: Assemble Note**

```
Player: Drags Note_Piece_1 to Slot_1 ✅
Player: Drags Note_Piece_2 to Slot_2 ✅
Player: Drags Note_Piece_3 to Slot_3 ✅
Player: Drags Note_Piece_4 to Slot_4 ✅
```

### **Step 4: Success**

```
System: All 4 pieces in correct order!
Sound: Success sound
Dialogue: "The note is complete. Mother's handwriting."
Dialogue: "'Tonight I end this child's suffering and mine - forever.' A murder-suicide plan."
Panel: Closes
Player: Can move again
Mirror 2: Complete! ✅
```

---

## 🔊 AUDIO REQUIREMENTS

### **Sounds Needed**:

```
1. drain_open.wav - Click/mechanical sound
2. water_drain.wav - Water draining sound (2-3 seconds)
3. paper_rustle.wav - Paper placement sound
4. success.wav - Success chime
5. emily_scream.wav - Emily attack sound
```

---

## 🎨 SPRITE REQUIREMENTS

### **Bathtub Sprites**:

```
1. bathtub_with_water.png
   - Shows bathtub filled with water
   - Drain cover visible

2. bathtub_empty.png
   - Shows bathtub empty (no water)
   - Drain open/visible
```

### **Note Piece Sprites**:

```
1. note_piece_1.png - "Tonight I"
2. note_piece_2.png - "end this child's"
3. note_piece_3.png - "suffering and"
4. note_piece_4.png - "mine - forever"

Style: Torn paper edges, aged/yellowed paper
```

---

## 🐛 TESTING CHECKLIST

### **Test 1: Drain Button**

```
✅ Click DrainCover_Button
✅ Bathtub sprite changes to empty
✅ Drain sound plays
✅ Dialogue shows
✅ Button disappears after click
```

### **Test 2: Note Pieces**

```
✅ All 4 note pieces are draggable
✅ Note pieces have correct Item IDs
✅ Note pieces snap to slots
✅ Paper rustle sound plays
```

### **Test 3: Correct Order**

```
✅ Place Note_Piece_1 in Slot_1
✅ Place Note_Piece_2 in Slot_2
✅ Place Note_Piece_3 in Slot_3
✅ Place Note_Piece_4 in Slot_4
✅ Success dialogue plays
✅ Panel closes
✅ Mirror 2 marked complete
```

### **Test 4: Wrong Order**

```
✅ Place pieces in wrong order
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
Note_Piece_1
Note_Piece_2
Note_Piece_3
Note_Piece_4
```

**IMPORTANT**: Item IDs must match GameObject names EXACTLY!

---

## 🔍 CONSOLE MESSAGES

### **When Drain Clicked**:

```
[Mirror2] Drain cover clicked - draining water
[Mirror2] Bathtub sprite changed to empty
[Mirror2] Water drained! Now assemble the torn notes.
```

### **When Note Placed**:

```
[Mirror2] Note piece Note_Piece_1 placed in slot Slot_1
[Mirror2] Checking solution...
[Mirror2] Filled slots: 1/4
[Mirror2] Not all slots filled yet
```

### **When Puzzle Complete**:

```
[Mirror2] Filled slots: 4/4
[Mirror2] Slot 0: Expected=Note_Piece_1, Actual=Note_Piece_1
[Mirror2] Slot 1: Expected=Note_Piece_2, Actual=Note_Piece_2
[Mirror2] Slot 2: Expected=Note_Piece_3, Actual=Note_Piece_3
[Mirror2] Slot 3: Expected=Note_Piece_4, Actual=Note_Piece_4
[Mirror2] ✅ PUZZLE SOLVED!
```

---

## ✅ FINAL CHECKLIST

### **Sprites**:

- [ ] bathtub_with_water sprite imported
- [ ] bathtub_empty sprite imported
- [ ] 4 note piece sprites imported
- [ ] drain_cover sprite for button

### **GameObjects**:

- [ ] BathtubDrain_Panel exists
- [ ] Bathtub_Image has Image component
- [ ] DrainCover_Button has Button component
- [ ] 4 slots (Slot_1 to Slot_4) exist
- [ ] 4 note pieces (Note_Piece_1 to Note_Piece_4) exist

### **Components**:

- [ ] Mirror2_Controller has Mirror2_BathtubDrain script
- [ ] All note pieces have DraggableItem component
- [ ] All Item IDs match GameObject names
- [ ] All Puzzle Numbers = 2

### **References Assigned**:

- [ ] Bathtub Image assigned
- [ ] Both bathtub sprites assigned
- [ ] Drain Cover Button assigned
- [ ] All 4 slots assigned in array
- [ ] Timer Text assigned
- [ ] Audio clips assigned
- [ ] Emily Jumpscare Panel assigned

### **Testing**:

- [ ] Drain button works
- [ ] Sprite changes when drained
- [ ] Note pieces are draggable
- [ ] Correct order completes puzzle
- [ ] Timer counts down
- [ ] Timeout triggers Emily attack

---

**MIRROR 2 SETUP COMPLETE!** 🛁✅

**DRAIN WATER** → **FIND NOTES** → **ASSEMBLE MESSAGE** → **SUCCESS!** 🎉

