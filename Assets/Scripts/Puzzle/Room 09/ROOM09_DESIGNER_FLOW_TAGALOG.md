# 🎮 ROOM 09 - FLOW PARA SA DESIGNER

## 📋 OVERVIEW

Ito yung **climactic puzzle room** - yung pinaka-intense na room bago yung final revelation. May 4 mirror puzzles na kailangan i-solve para ma-unlock yung master bedroom door.

---

## 🎯 MAIN CONCEPT

**Lisa ay trapped sa master bathroom kasama si Emily na naka-full power manifestation. Kailangan niyang i-solve yung 4 mirror puzzles para makita yung complete truth about sa murder-suicide plan ng mother niya. Pag hindi niya na-solve in time, si Emily ay mag-attack at game over.**

---

## 🎬 COMPLETE FLOW (STEP-BY-STEP)

### **PHASE 1: ENTRY SEQUENCE (1 minute)**

```
1. Lisa climbs through broken mirror
   - Galing sa Room 08 (Lisa's Bathroom)
   - Broken mirror sa wall
   - Lisa climbs through

2. Blood everywhere
   - Glass shards cut Lisa
   - Blood drips from wounds
   - Blood trail on floor

3. Door slams shut
   - Master bathroom door slams
   - LOCKED - walang escape
   - Sound effect: SLAM!

4. Emily manifests
   - Emily appears - FULL POWER
   - Solid, hindi translucent
   - Terrifying, fills entire room
   - Reality warps around her
   - Visual effects: distortion, glitching

5. Dialogue sequence
   Lisa: "I climb through the broken mirror. Glass shards cut deep."
   Lisa: "Blood... everywhere. My blood."
   Lisa: "The door slams shut behind me. I'm locked in."
   Lisa: "I'm locked in here with her... and she's not holding back anymore."
   Lisa: "Emily... she's here. Fully manifested. Solid. Terrifying."
   Lisa: "The entire bathroom warps around her desperation. Reality itself bends."
   Lisa: "She's not holding back. I need to solve these mirrors before she breaks completely."

6. Player can move
   - After dialogue, player gains control
   - Emily remains visible (menacing)
   - 4 mirrors available to interact
```

---

### **PHASE 2: PUZZLE SOLVING (5-10 minutes)**

Player can solve puzzles in **ANY ORDER**. Each puzzle has its own panel and timer.

---

#### **🪞 MIRROR 1: MEDICINE CABINET**

**Concept:** Arrange 6 prescription bottles chronologically by year

**Setup:**
```
Medicine Cabinet sa wall
6 prescription bottles scattered
Each bottle may label: "Medicine Name YEAR"
```

**Bottles:**
1. Antidepressants 1973
2. Lithium 1974
3. Valium 1975
4. Pain Pills 1975
5. Sleeping Pills 1976
6. Unknown Pills 1976

**Gameplay:**
```
1. Player clicks Medicine Cabinet
   ↓
2. Panel opens (full screen or large)
   ↓
3. 6 bottles appear in RANDOM order
   ↓
4. Timer starts: 60 seconds
   ↓
5. Player drags bottles to 6 slots (left to right)
   ↓
6. Must arrange chronologically: 1973 → 1974 → 1975 → 1975 → 1976 → 1976
   ↓
7. SUCCESS:
   - Mirror glows
   - Shows mother's face
   - Dialogue: "The bottles align... the mirror shows mother's face."
   - Dialogue: "Increasing dosages. Year after year. She was planning this for so long."
   - Panel closes
   - Mirror 1 COMPLETE ✅
   
8. FAILURE (timeout):
   - Emily JUMPSCARE
   - Full screen Emily face
   - Scream sound
   - Dialogue: "Emily's face fills my vision. Screaming. Furious."
   - Dialogue: "I can't... I can't think... Everything goes dark."
   - GAME OVER → Reload scene
```

**UI Panel:**
```
Medicine_Cabinet_Panel
├─ Background (dark, semi-transparent)
├─ Title: "Medicine Cabinet"
├─ Timer: "0:60" (counts down)
├─ 6 Empty Slots (horizontal row)
│   [Slot 1] [Slot 2] [Slot 3] [Slot 4] [Slot 5] [Slot 6]
└─ 6 Bottles (draggable, random positions)
```

---

#### **🛁 MIRROR 2: BATHTUB DRAIN**

**Concept:** Remove drain cover, find torn note pieces, reassemble suicide note

**Setup:**
```
Bathtub sa room
Drain cover visible
Water level changes
```

**Gameplay:**
```
1. Player clicks Bathtub
   ↓
2. Panel opens
   ↓
3. Bathtub image with drain cover
   ↓
4. Player clicks drain cover
   ↓
5. Drain cover removes
   ↓
6. 4 torn note pieces appear
   ↓
7. Timer starts: 60 seconds
   ↓
8. Player drags pieces to assembly area
   ↓
9. Must form complete sentence:
   "Tonight I end this child's suffering and mine - forever"
   
   Pieces:
   - "Tonight I"
   - "end this child's"
   - "suffering and"
   - "mine forever"
   ↓
10. SUCCESS:
    - Complete note appears
    - Mother's handwriting visible
    - Dialogue: "The note is complete. Mother's handwriting."
    - Dialogue: "'Tonight I end this child's suffering and mine - forever.' A murder-suicide plan."
    - Panel closes
    - Mirror 2 COMPLETE ✅
    
11. FAILURE (timeout):
    - Emily's face appears in water
    - Jumpscare
    - GAME OVER → Reload scene
```

**UI Panel:**
```
Bathtub_Drain_Panel
├─ Background
├─ Title: "Bathtub"
├─ Timer: "0:60"
├─ Bathtub Image (with drain)
├─ Drain Cover Button (clickable)
├─ 4 Note Pieces (draggable)
└─ Assembly Area (4 slots in order)
```

---

#### **💄 MIRROR 3: VANITY TERROR**

**Concept:** Arrange 8 diary page fragments in chronological order

**Setup:**
```
Vanity mirror sa wall
8 diary pages scattered around vanity
Each page may 1-2 sentences
```

**Diary Pages (in correct order):**

**Page 1:** "Child defied me at dinner. Refused to sit properly, knocked over her milk deliberately. The defiance grows stronger each day."

**Page 2:** "The defiance continues. Found the child talking to herself again. She claims someone named Emily tells her to disobey me. I need to increase the discipline sessions."

**Page 3:** "I've increased discipline sessions, but the child screams without breaking. Her invisible friend seems to make her braver, more resistant to correction."

**Page 4:** "Now strange things are happening in the house. Doors slamming, cold spots, objects moving. The child smiles when these incidents occur."

**Page 5:** "The supernatural events have escalated. I see shapes in the corners now. The child's imaginary friend is becoming real through her rebellion."

**Page 6:** "The presence grows bolder when I punish the child. It protects her, makes my discipline completely ineffective. I must find a permanent solution to this problem."

**Page 7:** "I've made my preparations and acquired what I need from town. The child suspects nothing. Her invisible protector won't be able to save her from what's coming."

**Page 8:** "Everything is ready. Tomorrow night I end this. The child will sleep in my room - she won't escape, and neither will her ghostly guardian."

**Gameplay:**
```
1. Player clicks Vanity Mirror
   ↓
2. Panel opens
   ↓
3. 8 diary pages scattered (random positions)
   ↓
4. Timer starts: 90 seconds (mas mahaba kasi 8 pages)
   ↓
5. Player drags pages to numbered slots (1-8)
   ↓
6. Must be in exact chronological order
   ↓
7. SUCCESS:
   - Complete timeline visible
   - Shows mother's descent into madness
   - Dialogue: "The timeline is complete. I can see it all now."
   - Dialogue: "Her defiance... my defiance. The discipline sessions. Emily protecting me. Mother's final plan."
   - Panel closes
   - Mirror 3 COMPLETE ✅
   
8. FAILURE (timeout):
   - Emily's screaming face fills mirror
   - Jumpscare
   - GAME OVER → Reload scene
```

**UI Panel:**
```
Vanity_Terror_Panel
├─ Background
├─ Title: "Mother's Diary"
├─ Timer: "1:30"
├─ Mirror Image (vanity)
├─ 8 Numbered Slots
│   [1] [2] [3] [4] [5] [6] [7] [8]
└─ 8 Diary Pages (draggable, scattered)
```

---

#### **🔪 MIRROR 4: EVIDENCE SEQUENCE**

**Concept:** Arrange 4 evidence items in correct order showing murder plan progression

**Setup:**
```
Large mirror sa wall
4 empty picture frames below mirror
4 evidence items scattered around bathroom
```

**Evidence Items:**
1. **Rope** (restraint)
2. **Pills** (sedation)
3. **Knife** (murder weapon)
4. **Bloody Towel** (cleanup)

**Correct Sequence:**
```
Frame 1: Rope (restrain child first)
Frame 2: Pills (sedate child)
Frame 3: Knife (murder)
Frame 4: Bloody Towel (cleanup evidence)
```

**Gameplay:**
```
1. Player clicks Large Mirror
   ↓
2. Panel opens
   ↓
3. 4 evidence items visible (scattered)
   ↓
4. 4 empty picture frames below mirror
   ↓
5. Timer starts: 60 seconds
   ↓
6. Player drags items into frames (left to right)
   ↓
7. Each CORRECT placement shows flashback:
   - Rope: Mother buying rope at store
   - Pills: Mother crushing pills
   - Knife: Mother sharpening knife
   - Towel: Mother preparing cleanup
   ↓
8. SUCCESS:
   - All 4 items in correct sequence
   - Complete murder plan revealed
   - Dialogue: "The sequence is complete. Each item shows a flashback."
   - Dialogue: "Restraint. Sedation. Murder. Cleanup. She had it all planned out."
   - Panel closes
   - Mirror 4 COMPLETE ✅
   
9. FAILURE (timeout):
   - Emily jumpscare
   - GAME OVER → Reload scene
```

**UI Panel:**
```
Evidence_Sequence_Panel
├─ Background
├─ Title: "The Plan"
├─ Timer: "0:60"
├─ Large Mirror Image
├─ 4 Picture Frames (empty)
│   [Frame 1] [Frame 2] [Frame 3] [Frame 4]
└─ 4 Evidence Items (draggable)
    [Rope] [Pills] [Knife] [Towel]
```

---

### **PHASE 3: ALL MIRRORS COMPLETE (2 minutes)**

```
Pag na-solve na lahat ng 4 mirrors:

1. Automatic trigger
   ↓
2. Player stops moving (cutscene)
   ↓
3. Dialogue: "All four mirrors show the complete story. The truth I tried to forget."
   ↓
4. Mother's voice echoes (creepy)
   Mother: "Tonight I end this child's defiance forever."
   ↓
5. EMILY'S BREAKDOWN SEQUENCE:
   
   a. Emily starts becoming translucent
      - Fade out effect
      - Losing power
   
   b. Dialogue:
      Lisa: "Emily's power... it's breaking. She's becoming translucent."
      Emily: "Every time I saved you, I became more like her!"
      Lisa: "She's exhausted. Collapsing. The water rises around her."
   
   c. Emily collapses to floor
      - Animation: Emily falls
      - Water rises around her
      - She becomes very translucent (almost invisible)
   
   d. Emily's final words (whisper):
      Emily: "The mirror in there... it will show you everything I tried to hide."
      Emily: "I'm sorry, Lisa. I couldn't protect you from the truth."
   
   e. Door unlocks
      - Sound effect: CLICK
      - Dialogue: "The master bedroom door... it's unlocking."
   ↓
6. Player can move again
   ↓
7. Can now interact with Master Bedroom Door
```

---

### **PHASE 4: MASTER BEDROOM ENTRY (30 seconds)**

```
1. Player walks to Master Bedroom Door
   ↓
2. Player clicks door
   ↓
3. Final dialogue:
   Lisa: "The master bedroom. Where it all ended."
   Lisa: "Emily lies collapsed in the flooded bathroom behind me. Powerless."
   Lisa: "I open the door. The final truth awaits."
   ↓
4. Door opens (animation)
   ↓
5. Fade to black
   ↓
6. Load next scene: Room 10 (Master Bedroom)
```

---

## 🎨 VISUAL REQUIREMENTS

### **Emily Manifestation:**
- Full-body Emily sprite
- **Solid** (hindi translucent)
- Terrifying expression
- Large (fills screen or significant portion)
- Always visible during puzzles
- Visual effects: reality distortion, glitching

### **Blood Effects:**
- Blood on Lisa's hands
- Blood drips on floor
- Blood trail from mirror
- Particle effects (optional)

### **Mirror Effects:**
- Glow when complete
- Flashback images
- Mother's face appearance
- Visual distortion

### **Water Effects:**
- Water level rises in bathtub
- Water on floor (flooding)
- Emily's reflection in water

---

## 🔊 AUDIO REQUIREMENTS

### **Ambient:**
- Tense music (looping, intense)
- Water dripping
- Emily's breathing
- Reality warping sounds

### **Puzzle Sounds:**
- Bottle clink (placing bottles)
- Paper rustle (diary pages)
- Water drain sound
- Item pickup/place sound

### **Emily Sounds:**
- Scream (jumpscare)
- Whisper (final words)
- Breathing (ambient)
- Attack sound

### **Success Sounds:**
- Puzzle complete chime
- Mirror activation
- Door unlock

---

## 📊 DIFFICULTY SETTINGS

### **Time Limits:**
- Mirror 1: 60 seconds (easy)
- Mirror 2: 60 seconds (medium)
- Mirror 3: 90 seconds (hard - 8 pages)
- Mirror 4: 60 seconds (medium)

### **Adjustments:**
- Pwede i-increase yung time kung masyadong mahirap
- Pwede mag-add ng hints (visual cues)
- Pwede mag-add ng checkpoint system

---

## 🎮 PLAYER EXPERIENCE

### **Tension Build:**
```
Low → Medium → High → CLIMAX

Entry: Trapped, Emily appears (HIGH tension)
  ↓
Puzzles: Time pressure, Emily watching (SUSTAINED tension)
  ↓
Each puzzle complete: Brief relief
  ↓
All complete: CLIMAX - Emily breakdown, truth revealed
  ↓
Master bedroom: Anticipation for final revelation
```

### **Story Revelation:**
```
Each puzzle reveals part of the truth:

Mirror 1: Mother planning for years (medication)
Mirror 2: Murder-suicide plan (note)
Mirror 3: Timeline of madness (diary)
Mirror 4: Execution plan (evidence)

Combined: Complete picture of that night
```

---

## 💡 DESIGN TIPS

### **Para sa Artist:**
1. Emily dapat **terrifying** - ito yung full power niya
2. Blood effects dapat visible pero hindi sobrang graphic
3. Mirror effects dapat mystical/supernatural
4. Bathroom dapat claustrophobic (trapped feeling)

### **Para sa UI Designer:**
1. Panels dapat clear at easy to understand
2. Draggable items dapat obvious
3. Timer dapat visible at color-coded (white → yellow → red)
4. Success/failure feedback dapat immediate

### **Para sa Sound Designer:**
1. Tense music dapat looping, building tension
2. Emily's sounds dapat creepy pero hindi annoying
3. Puzzle sounds dapat satisfying
4. Jumpscare sound dapat shocking pero hindi too loud

---

## ✅ TESTING CHECKLIST

### **Entry:**
- [ ] Lisa enters from broken mirror
- [ ] Blood effects visible
- [ ] Door slams and locks
- [ ] Emily appears
- [ ] Dialogue plays correctly
- [ ] Player can move after

### **Each Mirror Puzzle:**
- [ ] Panel opens when clicked
- [ ] Timer starts
- [ ] Items are draggable
- [ ] Correct solution → Success
- [ ] Timeout → Emily attack
- [ ] Success dialogue plays
- [ ] Panel closes
- [ ] Mirror marked complete

### **All Mirrors Complete:**
- [ ] Triggers automatically
- [ ] Mother's voice plays
- [ ] Emily breakdown sequence
- [ ] Door unlocks
- [ ] Player can enter master bedroom

### **Master Bedroom Entry:**
- [ ] Final dialogue plays
- [ ] Scene transitions correctly
- [ ] Progress saved

---

## 📝 SUMMARY PARA SA DESIGNER

### **Main Concept:**
Lisa trapped sa bathroom with full-power Emily. Must solve 4 mirror puzzles to reveal complete truth and unlock master bedroom.

### **4 Puzzles:**
1. **Medicine Cabinet** - Arrange 6 bottles chronologically
2. **Bathtub Drain** - Reassemble torn suicide note
3. **Vanity Terror** - Order 8 diary pages
4. **Evidence Sequence** - Arrange 4 murder plan items

### **Key Features:**
- Time pressure (60-90 seconds per puzzle)
- Emily attacks if timeout (game over)
- Can solve in any order
- Each reveals part of story
- All complete → Emily breakdown → Door unlocks

### **Mood:**
Intense, claustrophobic, terrifying, climactic

### **Duration:**
5-10 minutes total (depending on player skill)

---

**EPIC CLIMACTIC ROOM!** 🎮✨

Ito yung pinaka-intense na puzzle room - make it memorable! 💖
