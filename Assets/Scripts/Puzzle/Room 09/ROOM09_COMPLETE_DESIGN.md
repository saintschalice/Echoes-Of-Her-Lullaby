# 🛁 ROOM 09 - MASTER BEDROOM'S BATHROOM - COMPLETE DESIGN

## 📋 OVERVIEW

Room 09 is the **climactic puzzle room** with 4 mirror puzzles that reveal the complete truth about mother's murder-suicide plan. Emily manifests at full power and attacks if puzzles aren't solved in time.

---

## 🎯 ROOM OBJECTIVES

1. ✅ Emerge from broken mirror (cut by glass)
2. ✅ Survive Emily's full manifestation
3. ✅ Solve 4 mirror puzzles to reconstruct timeline
4. ✅ Witness Emily's breakdown
5. ✅ Unlock master bedroom door
6. ✅ Enter final room for climactic revelation

---

## 🎮 COMPLETE FLOW

```
ENTRY
  ↓
Lisa climbs through broken mirror
Blood everywhere (cut by glass)
  ↓
Door slams shut - TRAPPED
  ↓
Emily manifests at FULL POWER
Solid, terrifying, reality warps
  ↓
4 MIRROR PUZZLES (solve all to progress)
├─ Mirror 1: Medicine Cabinet (chronological bottles)
├─ Mirror 2: Bathtub Drain (reassemble note)
├─ Mirror 3: Vanity Terror (diary timeline)
└─ Mirror 4: Evidence Sequence (murder plan order)
  ↓
ALL MIRRORS COMPLETE
  ↓
Mother's voice: "Tonight I end this child's defiance forever"
  ↓
EMILY'S BREAKDOWN
"Every time I saved you, I became more like her!"
Emily collapses, becomes translucent
  ↓
MASTER BEDROOM DOOR UNLOCKS
  ↓
Emily's final words:
"The mirror in there... it will show you everything I tried to hide"
  ↓
ENTER MASTER BEDROOM
→ Room 10 (Final Revelation)
```

---

## 🪞 MIRROR PUZZLE 1: MEDICINE CABINET

### **Concept:**
Arrange 6 prescription bottles chronologically by year

### **Bottles:**
1. **Antidepressants 1973** (oldest)
2. **Lithium 1974**
3. **Valium 1975**
4. **Pain Pills 1975**
5. **Sleeping Pills 1976**
6. **Unknown Pills 1976** (newest)

### **Correct Solution:**
```
Slot 1: Antidepressants 1973
Slot 2: Lithium 1974
Slot 3: Valium 1975
Slot 4: Pain Pills 1975
Slot 5: Sleeping Pills 1976
Slot 6: Unknown Pills 1976
```

### **Mechanics:**
- 6 bottles spawn in random order
- Player drags bottles to correct slots
- Must be in exact chronological order
- Time limit: 60 seconds
- Failure: Emily jumpscare → Game Over

### **Success:**
- Mirror shows mother's face
- Flashback: Increasing dosages over time
- Reveals: Mother planning for years

### **UI Panel:**
```
Medicine_Cabinet_Panel
├─ Background (semi-transparent black)
├─ Title_Text: "Medicine Cabinet"
├─ Timer_Text: "0:60"
├─ Bottle_Slots (6 slots, horizontal)
│   ├─ Slot_1 (empty frame)
│   ├─ Slot_2 (empty frame)
│   ├─ Slot_3 (empty frame)
│   ├─ Slot_4 (empty frame)
│   ├─ Slot_5 (empty frame)
│   └─ Slot_6 (empty frame)
└─ Bottle_Container (spawned bottles)
```

---

## 🛁 MIRROR PUZZLE 2: BATHTUB DRAIN

### **Concept:**
Remove drain cover, find torn note pieces, reassemble suicide note

### **Note Pieces (4):**
1. "Tonight I"
2. "end this child's"
3. "suffering and"
4. "mine forever"

### **Complete Note:**
"Tonight I end this child's suffering and mine - forever"

### **Mechanics:**
- Click bathtub → Water level changes
- Click drain cover → Removes, reveals note pieces
- 4 torn pieces appear
- Drag pieces to assembly area
- Must form complete sentence
- Time limit: 60 seconds
- Failure: Emily's face in water → Game Over

### **Success:**
- Complete note appears
- Reveals: Murder-suicide plan
- Mother's handwriting visible

### **UI Panel:**
```
Bathtub_Drain_Panel
├─ Background
├─ Title_Text: "Bathtub"
├─ Timer_Text: "0:60"
├─ Bathtub_Image (with drain)
├─ Drain_Cover_Button (clickable)
├─ Note_Pieces (4 draggable pieces)
└─ Assembly_Area (4 slots for pieces)
```

---

## 💄 MIRROR PUZZLE 3: VANITY TERROR

### **Concept:**
Arrange 8 diary page fragments in chronological order

### **Diary Pages:**

**Page 1:** "Child defied me at dinner. Refused to sit properly, knocked over her milk deliberately. The defiance grows stronger each day."

**Page 2:** "The defiance continues. Found the child talking to herself again. She claims someone named Emily tells her to disobey me. I need to increase the discipline sessions."

**Page 3:** "I've increased discipline sessions, but the child screams without breaking. Her invisible friend seems to make her braver, more resistant to correction."

**Page 4:** "Now strange things are happening in the house. Doors slamming, cold spots, objects moving. The child smiles when these incidents occur."

**Page 5:** "The supernatural events have escalated. I see shapes in the corners now. The child's imaginary friend is becoming real through her rebellion."

**Page 6:** "The presence grows bolder when I punish the child. It protects her, makes my discipline completely ineffective. I must find a permanent solution to this problem."

**Page 7:** "I've made my preparations and acquired what I need from town. The child suspects nothing. Her invisible protector won't be able to save her from what's coming."

**Page 8:** "Everything is ready. Tomorrow night I end this. The child will sleep in my room - she won't escape, and neither will her ghostly guardian."

### **Correct Order:**
Pages 1 → 2 → 3 → 4 → 5 → 6 → 7 → 8

### **Mechanics:**
- 8 diary pages scattered around vanity
- Player drags pages to numbered slots (1-8)
- Must be in exact chronological order
- Time limit: 90 seconds (longer, more complex)
- Failure: Emily's screaming face fills mirror → Game Over

### **Success:**
- Complete timeline visible
- Shows mother's descent into madness
- Reveals: Emily protecting Lisa, mother's final plan

### **UI Panel:**
```
Vanity_Terror_Panel
├─ Background
├─ Title_Text: "Mother's Diary"
├─ Timer_Text: "1:30"
├─ Mirror_Image (vanity mirror)
├─ Page_Slots (8 numbered slots)
│   ├─ Slot_1
│   ├─ Slot_2
│   ├─ Slot_3
│   ├─ Slot_4
│   ├─ Slot_5
│   ├─ Slot_6
│   ├─ Slot_7
│   └─ Slot_8
└─ Page_Container (scattered pages)
```

---

## 🔪 MIRROR PUZZLE 4: EVIDENCE SEQUENCE

### **Concept:**
Arrange 4 evidence items in correct order showing mother's murder plan progression

### **Evidence Items:**
1. **Rope** (restraint)
2. **Pills** (sedation)
3. **Knife** (murder weapon)
4. **Bloody Towel** (cleanup)

### **Correct Sequence:**
```
Frame 1: Rope (restrain child)
Frame 2: Pills (sedate child)
Frame 3: Knife (murder)
Frame 4: Bloody Towel (cleanup evidence)
```

### **Mechanics:**
- 4 evidence items scattered around bathroom
- Large mirror with 4 empty picture frames below
- Drag items into frames left-to-right
- Each correct placement shows flashback
- Time limit: 60 seconds
- Failure: Emily jumpscare → Game Over

### **Success:**
- Complete sequence shows mother's plan
- Flashbacks: Mother acquiring each item
- Reveals: Premeditated murder plan

### **Flashbacks:**
- Rope: Mother buying rope at hardware store
- Pills: Mother crushing pills into powder
- Knife: Mother sharpening kitchen knife
- Towel: Mother preparing cleanup supplies

### **UI Panel:**
```
Evidence_Sequence_Panel
├─ Background
├─ Title_Text: "The Plan"
├─ Timer_Text: "0:60"
├─ Large_Mirror_Image
├─ Picture_Frames (4 empty frames)
│   ├─ Frame_1 (Rope)
│   ├─ Frame_2 (Pills)
│   ├─ Frame_3 (Knife)
│   └─ Frame_4 (Bloody Towel)
└─ Evidence_Container (4 draggable items)
```

---

## 👻 EMILY'S MANIFESTATION

### **Visual:**
- Full-body Emily sprite
- Solid, not translucent
- Terrifying expression
- Fills entire bathroom
- Reality warps around her (visual effects)

### **Behavior:**
- Always visible during puzzles
- Attacks if time runs out
- Screams/jumpscares on failure
- Breaks down after all puzzles complete

### **Breakdown Sequence:**
1. Emily becomes translucent
2. Collapses to floor
3. Water rises around her
4. Whispers final words
5. Remains powerless

---

## 🎨 UI PANELS NEEDED

### **1. Medicine Cabinet Panel**
- 6 bottle slots
- Timer
- Draggable bottles
- Success effect

### **2. Bathtub Drain Panel**
- Bathtub image
- Drain cover button
- 4 note piece slots
- Assembly area

### **3. Vanity Terror Panel**
- Mirror image
- 8 numbered page slots
- Scattered diary pages
- Timer

### **4. Evidence Sequence Panel**
- Large mirror
- 4 picture frames
- 4 evidence items
- Flashback images

### **5. Emily Jumpscare Panel**
- Full-screen Emily face
- Screaming animation
- Red vignette effect
- Game over text

---

## 🔊 AUDIO REQUIREMENTS

### **Ambient:**
- Tense music (looping)
- Water dripping sounds
- Emily's breathing
- Reality warping sounds

### **Puzzle Sounds:**
- Bottle clink (placing bottles)
- Paper rustle (diary pages)
- Water drain sound
- Item pickup sound

### **Emily Sounds:**
- Scream (jumpscare)
- Whisper (final words)
- Breathing (ambient)
- Attack sound

### **Success Sounds:**
- Puzzle complete chime
- Mirror activation sound
- Door unlock sound

---

## 📊 PROGRESSION FLAGS

```csharp
// Room state
bool isIntroDone
bool isDoorLocked

// Mirror puzzles
bool mirror1Complete // Medicine Cabinet
bool mirror2Complete // Bathtub Drain
bool mirror3Complete // Vanity Terror
bool mirror4Complete // Evidence Sequence

// Emily state
bool emilyHasCollapsed
bool canEnterMasterBedroom
```

---

## 🎮 GAMEPLAY FLOW DETAILED

### **Phase 1: Entry (1 minute)**
```
1. Lisa climbs through broken mirror
2. Blood drips from cuts
3. Door slams shut
4. Emily manifests at full power
5. Player can move
```

### **Phase 2: Puzzle Solving (5-10 minutes)**
```
Player can interact with 4 mirrors in any order:
├─ Mirror 1: Medicine Cabinet (60s time limit)
├─ Mirror 2: Bathtub Drain (60s time limit)
├─ Mirror 3: Vanity Terror (90s time limit)
└─ Mirror 4: Evidence Sequence (60s time limit)

Each puzzle:
1. Click mirror to start
2. Panel opens
3. Timer starts
4. Solve puzzle
5. Success → Mirror complete
6. Failure → Emily attack → Game Over
```

### **Phase 3: All Mirrors Complete (2 minutes)**
```
1. All 4 mirrors solved
2. Mother's voice echoes
3. Emily's breakdown sequence
4. Emily collapses
5. Door unlocks
6. Emily's final words
```

### **Phase 4: Master Bedroom Entry (30 seconds)**
```
1. Player approaches door
2. Final dialogue
3. Door opens
4. Scene transition → Room 10
```

---

## ✅ TESTING CHECKLIST

### **Entry Sequence:**
- [ ] Lisa enters from broken mirror
- [ ] Blood effects visible
- [ ] Door slams and locks
- [ ] Emily appears at full power
- [ ] Intro dialogue plays
- [ ] Player can move after intro

### **Mirror 1 (Medicine Cabinet):**
- [ ] Click mirror → Panel opens
- [ ] 6 bottles spawn randomly
- [ ] Bottles are draggable
- [ ] Timer counts down
- [ ] Correct order → Success
- [ ] Wrong order + timeout → Emily attack
- [ ] Success dialogue plays
- [ ] Mirror marked complete

### **Mirror 2 (Bathtub Drain):**
- [ ] Click bathtub → Panel opens
- [ ] Drain cover clickable
- [ ] 4 note pieces appear
- [ ] Pieces are draggable
- [ ] Correct assembly → Success
- [ ] Timeout → Emily attack
- [ ] Success dialogue plays
- [ ] Mirror marked complete

### **Mirror 3 (Vanity Terror):**
- [ ] Click vanity → Panel opens
- [ ] 8 diary pages scattered
- [ ] Pages are draggable
- [ ] 8 numbered slots visible
- [ ] Correct order → Success
- [ ] Timeout → Emily attack
- [ ] Success dialogue plays
- [ ] Mirror marked complete

### **Mirror 4 (Evidence Sequence):**
- [ ] Click mirror → Panel opens
- [ ] 4 evidence items visible
- [ ] Items are draggable
- [ ] 4 picture frames visible
- [ ] Correct sequence → Success
- [ ] Each placement shows flashback
- [ ] Timeout → Emily attack
- [ ] Success dialogue plays
- [ ] Mirror marked complete

### **All Mirrors Complete:**
- [ ] Completion sequence triggers
- [ ] Mother's voice plays
- [ ] Emily breakdown sequence
- [ ] Emily becomes translucent
- [ ] Emily collapses
- [ ] Final words play
- [ ] Door unlocks
- [ ] Player can enter master bedroom

### **Master Bedroom Entry:**
- [ ] Click door → Final dialogue
- [ ] Scene transitions to Room 10
- [ ] Progress saved

---

## 🐛 COMMON ISSUES

### **Puzzles too hard:**
- Increase time limits
- Add visual hints
- Show correct positions on hover

### **Emily attacks too often:**
- Increase time limits
- Add checkpoint system
- Allow puzzle retries

### **Drag-and-drop not working:**
- Check EventSystem exists
- Check Canvas has GraphicRaycaster
- Check draggable components

### **Panels not showing:**
- Check panel initially inactive
- Check references assigned
- Check Canvas exists

---

## 💡 DESIGN TIPS

### **Difficulty Balance:**
- Mirror 1: Easy (chronological order)
- Mirror 2: Medium (reassemble note)
- Mirror 3: Hard (8 pages, longer)
- Mirror 4: Medium (logical sequence)

### **Time Limits:**
- Mirror 1: 60 seconds
- Mirror 2: 60 seconds
- Mirror 3: 90 seconds (more complex)
- Mirror 4: 60 seconds

### **Player Experience:**
- Can solve in any order
- Each puzzle reveals part of story
- Tension builds with each completion
- Climactic payoff when all complete

---

## 📝 SUMMARY

### **Scripts Created:**
1. ✅ Room09_Dialogues.cs
2. ✅ Room09_FlowController.cs
3. ✅ Mirror1_MedicineCabinet.cs
4. ⏳ Mirror2_BathtubDrain.cs (need to create)
5. ⏳ Mirror3_VanityTerror.cs (need to create)
6. ⏳ Mirror4_EvidenceSequence.cs (need to create)

### **Panels Needed:**
1. Medicine Cabinet Panel (6 bottle slots)
2. Bathtub Drain Panel (4 note pieces)
3. Vanity Terror Panel (8 diary pages)
4. Evidence Sequence Panel (4 evidence items)
5. Emily Jumpscare Panel (full screen)

### **GameObjects Needed:**
1. Room09_FlowController
2. Emily_Manifestation (full power sprite)
3. Master_Bedroom_Door
4. 4 Mirror trigger objects
5. Audio sources

---

**COMPLEX BUT EPIC!** 🎮✨

This is the climactic puzzle room - make it memorable! 💖
