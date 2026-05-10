# ROOM 09: MASTER BEDROOM'S BATHROOM - ASSETS & FLOW SUMMARY

## 🎯 ROOM OVERVIEW
Room 09 is where Lisa emerges from a broken mirror into the Master Bedroom's bathroom. She must solve 4 mirror puzzles while Emily attacks at full power. This is the most intense puzzle room before the final revelation.

---

## 📦 ASSETS NEEDED

### 🖼️ SPRITES (Scene Objects)

#### Main Scene:
- [ ] **Bathroom Background** - Master bedroom's private bathroom
- [ ] **Broken Mirror** (entry point) - Shattered mirror Lisa climbs through
- [ ] **Blood Drips** - Blood from Lisa's cuts on glass shards
- [ ] **Emily Full Power** - Solid, terrifying Emily sprite (most visible form)
- [ ] **Door** - Locked bathroom door

#### Mirror 1: Medicine Cabinet
- [ ] **Medicine Cabinet** - Cabinet sprite (closed/open states)
- [ ] **6 Prescription Bottles** - Individual bottle sprites with labels:
  - "Valium 1975"
  - "Lithium 1974"
  - "Sleeping Pills 1976"
  - "Antidepressants 1973"
  - "Pain Pills 1975"
  - "Unknown Pills 1976"

#### Mirror 2: Bathtub Drain
- [ ] **Bathtub** - Bathtub sprite
- [ ] **Drain Cover** - Removable drain cover
- [ ] **Water** - Water level sprite (rises/falls)
- [ ] **4 Torn Note Pieces** - Individual note piece sprites:
  - "Tonight I"
  - "end this child's"
  - "suffering and"
  - "mine forever"
- [ ] **Emily Face in Water** - Emily's reflection in water (jumpscare)

#### Mirror 3: Vanity Terror
- [ ] **Vanity Mirror** - Large vanity with mirror
- [ ] **8 Diary Page Fragments** - Individual page sprites with text
- [ ] **Emily Screaming Face** - Emily's face in mirror reflection (jumpscare)

#### Mirror 4: Evidence Sequence
- [ ] **Large Mirror** - Mirror with 4 empty picture frames
- [ ] **4 Evidence Items** - Individual item sprites:
  - Rope
  - Knife
  - Pills
  - Bloody Towel
- [ ] **4 Flashback Images** - Brief flashback for each item

---

### 🎨 UI PANELS (4 Puzzle Panels)

#### Panel 1: Medicine Cabinet Panel
```
Components:
- Background (semi-transparent dark overlay)
- Cabinet Image (top)
- 6 Bottle Slots (horizontal arrangement)
- 6 Draggable Bottles (scattered below)
- Timer Display (60 seconds)
- Close Button (X)
- Instructions Text
```

#### Panel 2: Bathtub Drain Panel
```
Components:
- Background (semi-transparent dark overlay)
- Bathtub Image (center)
- Drain Cover (clickable to remove)
- 4 Note Piece Slots (horizontal arrangement)
- 4 Draggable Note Pieces (scattered)
- Water Level Indicator
- Timer Display (60 seconds)
- Close Button (X)
- Instructions Text
```

#### Panel 3: Vanity Terror Panel
```
Components:
- Background (semi-transparent dark overlay)
- Vanity Mirror Image (top)
- 8 Page Slots (numbered 1-8)
- 8 Draggable Diary Pages (scattered)
- Timer Display (90 seconds)
- Close Button (X)
- Instructions Text
```

#### Panel 4: Evidence Sequence Panel
```
Components:
- Background (semi-transparent dark overlay)
- Large Mirror Image (top)
- 4 Picture Frame Slots (horizontal)
- 4 Draggable Evidence Items (scattered)
- Timer Display (60 seconds)
- Close Button (X)
- Instructions Text
```

---

### 🎵 AUDIO

#### Music:
- [ ] **Intense Battle Music** - High tension, fast-paced (plays throughout room)
- [ ] **Success Jingle** - Short success sound (when puzzle solved)
- [ ] **Failure Sound** - Ominous sound (when Emily attacks)

#### Sound Effects:
- [ ] **Glass Breaking** - Lisa climbing through broken mirror
- [ ] **Blood Drip** - Blood dripping sounds
- [ ] **Door Slam** - Bathroom door locking
- [ ] **Emily Scream** - Emily's attack sound
- [ ] **Bottle Clink** - Bottles being moved
- [ ] **Water Splash** - Bathtub water sounds
- [ ] **Paper Rustle** - Diary pages being moved
- [ ] **Item Pickup** - Evidence items being picked up
- [ ] **Tick Tock** - Timer countdown (last 10 seconds)
- [ ] **Door Unlock** - Master bedroom door opening

---

### 📝 TEXT CONTENT

#### Diary Page Fragments (8 pages):
```
Page 1: "Child defied me at dinner. Refused to sit properly, knocked over her milk deliberately. The defiance grows stronger each day."

Page 2: "The defiance continues. Found the child talking to herself again. She claims someone named Emily tells her to disobey me. I need to increase the discipline sessions."

Page 3: "I've increased discipline sessions, but the child screams without breaking. Her invisible friend seems to make her braver, more resistant to correction."

Page 4: "Now strange things are happening in the house. Doors slamming, cold spots, objects moving. The child smiles when these incidents occur."

Page 5: "The supernatural events have escalated. I see shapes in the corners now. The child's imaginary friend is becoming real through her rebellion."

Page 6: "The presence grows bolder when I punish the child. It protects her, makes my discipline completely ineffective. I must find a permanent solution to this problem."

Page 7: "I've made my preparations and acquired what I need from town. The child suspects nothing. Her invisible protector won't be able to save her from what's coming."

Page 8: "Everything is ready. Tomorrow night I end this. The child will sleep in my room - she won't escape, and neither will her ghostly guardian."
```

---

## 🎮 COMPLETE FLOW SEQUENCE

### PHASE 1: EMERGENCY ENTRY
```
1. Lisa climbs through broken mirror
2. Cuts herself on glass shards
3. Blood drips everywhere
4. Emily's violent response triggered
5. Bathroom door SLAMS shut (locked)
6. Dialogue: "I'm locked in here with her... and she's not holding back anymore."
```

**Player State**: Can move, can interact with mirrors

---

### PHASE 2: EMILY'S UNRESTRAINED FURY
```
1. Emily manifests at FULL POWER
2. Solid, terrifying, fills entire bathroom
3. Reality warps around her
4. Dialogue: Emily's most desperate warnings
5. Multi-stage puzzle begins
```

**Player State**: Can interact with 4 mirrors in any order

---

### PHASE 3: MIRROR PUZZLES (Any Order)

#### MIRROR 1: Medicine Cabinet Sequence
```
INTERACTION:
1. Player clicks Medicine Cabinet
2. Panel opens with 6 bottles scattered
3. Timer starts: 60 seconds

PUZZLE:
- Drag bottles to slots in chronological order (left to right)
- Correct order: 1973, 1974, 1975 (Valium), 1975 (Pain), 1976 (Sleep), 1976 (Unknown)

SUCCESS:
- Mirror shows mother increasing dosage over time
- Success sound plays
- Panel closes
- Mirror 1 marked complete

FAILURE (Timeout):
- Emily jumpscares
- Game Over screen
```

#### MIRROR 2: Bathtub Drain Puzzle
```
INTERACTION:
1. Player clicks Bathtub
2. Panel opens with bathtub and drain
3. Timer starts: 60 seconds

PUZZLE:
- Click drain cover to remove
- 4 torn note pieces appear
- Drag pieces to slots to form complete note
- Correct order: "Tonight I" + "end this child's" + "suffering and" + "mine forever"

VISUAL FEEDBACK:
- Emily's face appears in water surface (creepy)

SUCCESS:
- Complete note reads: "Tonight I end this child's suffering and mine - forever"
- Reveals mother's murder-suicide plan
- Success sound plays
- Panel closes
- Mirror 2 marked complete

FAILURE (Timeout):
- Emily's face in water attacks
- Game Over screen
```

#### MIRROR 3: Vanity Terror Sequence
```
INTERACTION:
1. Player clicks Vanity Mirror
2. Panel opens with 8 diary pages scattered
3. Timer starts: 90 seconds (longer due to complexity)

PUZZLE:
- Drag diary pages to numbered slots (1-8)
- Must arrange in chronological order
- Each page shows escalating abuse and final plan

VISUAL FEEDBACK:
- Emily's screaming face fills mirror reflection

SUCCESS:
- All 8 pages in correct sequence
- Reveals complete timeline of mother's deteriorating mental state
- Success sound plays
- Panel closes
- Mirror 3 marked complete

FAILURE (Timeout):
- Emily screams from mirror
- Game Over screen
```

#### MIRROR 4: Evidence Sequence Puzzle
```
INTERACTION:
1. Player clicks Large Mirror
2. Panel opens with 4 empty frames and 4 items
3. Timer starts: 60 seconds

PUZZLE:
- Drag evidence items to frames in correct order (left to right)
- Correct order: Rope → Pills → Knife → Bloody Towel
- Shows progression of mother's murder plan

VISUAL FEEDBACK:
- Each correct placement shows brief flashback image

SUCCESS:
- All 4 items in correct sequence
- Reveals mother's complete murder plan
- Success sound plays
- Panel closes
- Mirror 4 marked complete

FAILURE (Timeout):
- Emily jumpscare
- Game Over screen
```

---

### PHASE 4: ALL PUZZLES COMPLETE
```
WHEN ALL 4 MIRRORS SOLVED:

1. Emily's complete breakdown
   - Dialogue: "Every time I saved you, I became more like her!"

2. Truth sequence unlocked
   - All four mirrors show complete story
   - Mother's voice echoes: "Tonight I end this child's defiance forever."

3. Master bedroom door UNLOCKS
   - Door unlock sound plays
   - Glow effect on door

4. Emily's exhaustion
   - Emily's power breaks from overexertion
   - Becomes translucent
   - Dialogue: "The mirror in there... it will show you everything I tried to hide."

5. Player can exit
   - Click door to proceed to Room 10
```

---

## 🎯 PUZZLE SOLUTIONS REFERENCE

### Mirror 1: Medicine Cabinet
```
Slot 1: Antidepressants 1973
Slot 2: Lithium 1974
Slot 3: Valium 1975
Slot 4: Pain Pills 1975
Slot 5: Sleeping Pills 1976
Slot 6: Unknown Pills 1976
```

### Mirror 2: Bathtub Drain
```
Slot 1: "Tonight I"
Slot 2: "end this child's"
Slot 3: "suffering and"
Slot 4: "mine forever"
```

### Mirror 3: Vanity Terror
```
Slot 1: Page 1 (Child defied me at dinner...)
Slot 2: Page 2 (The defiance continues...)
Slot 3: Page 3 (I've increased discipline...)
Slot 4: Page 4 (Now strange things...)
Slot 5: Page 5 (The supernatural events...)
Slot 6: Page 6 (The presence grows bolder...)
Slot 7: Page 7 (I've made my preparations...)
Slot 8: Page 8 (Everything is ready...)
```

### Mirror 4: Evidence Sequence
```
Frame 1: Rope (restraint)
Frame 2: Pills (sedation)
Frame 3: Knife (murder weapon)
Frame 4: Bloody Towel (cleanup)
```

---

## ⏱️ TIMING SPECIFICATIONS

### Puzzle Timers:
- **Mirror 1**: 60 seconds
- **Mirror 2**: 60 seconds
- **Mirror 3**: 90 seconds (more complex)
- **Mirror 4**: 60 seconds

### Total Possible Time:
- **Minimum**: 4 minutes (if all solved quickly)
- **Maximum**: 4.5 minutes (using full time)
- **Average**: 5-7 minutes (including exploration and retries)

---

## 🎨 VISUAL EFFECTS

### Required Effects:
- [ ] **Blood Drip Animation** - Blood dripping from Lisa's cuts
- [ ] **Reality Distortion** - Screen warping, color shifts
- [ ] **Emily Glow** - Emily's solid form glowing with power
- [ ] **Mirror Glow** - Unsolved mirrors glow to indicate interactability
- [ ] **Success Flash** - Flash of light when puzzle solved
- [ ] **Failure Shake** - Screen shake when Emily attacks
- [ ] **Door Glow** - Door glows when all puzzles complete
- [ ] **Water Ripple** - Water rippling in bathtub
- [ ] **Timer Warning** - Timer flashes red in last 10 seconds

---

## 🎮 PLAYER CONTROLS

### During Exploration:
- **Movement**: Enabled (can walk around bathroom)
- **Interaction**: Click mirrors to open puzzle panels
- **Joystick**: Visible and active

### During Puzzle:
- **Movement**: Disabled (focused on puzzle)
- **Interaction**: Drag and drop items
- **Joystick**: Hidden
- **Close Button**: Can close panel (puzzle progress saved)

### After All Puzzles:
- **Movement**: Enabled
- **Interaction**: Click door to exit
- **Joystick**: Visible and active

---

## 📊 DIFFICULTY BALANCE

### Mirror 1 (Medicine Cabinet):
- **Difficulty**: Medium
- **Items**: 6 bottles
- **Challenge**: Chronological ordering with duplicate years
- **Time**: 60 seconds (10 seconds per bottle)

### Mirror 2 (Bathtub Drain):
- **Difficulty**: Easy-Medium
- **Items**: 4 note pieces
- **Challenge**: Form coherent sentence
- **Time**: 60 seconds (15 seconds per piece)

### Mirror 3 (Vanity Terror):
- **Difficulty**: Hard
- **Items**: 8 diary pages
- **Challenge**: Long text, chronological ordering
- **Time**: 90 seconds (11 seconds per page)

### Mirror 4 (Evidence Sequence):
- **Difficulty**: Medium
- **Items**: 4 evidence items
- **Challenge**: Logical sequence (plan progression)
- **Time**: 60 seconds (15 seconds per item)

---

## 🔧 UNITY SETUP CHECKLIST

### Scene Objects:
- [ ] Room09_FlowController (empty GameObject + script)
- [ ] Emily_FullPower (sprite + animator)
- [ ] BrokenMirror_Entry (sprite)
- [ ] BathroomDoor (sprite + collider)
- [ ] MedicineCabinet (sprite + Mirror1 script + collider)
- [ ] Bathtub (sprite + Mirror2 script + collider)
- [ ] VanityMirror (sprite + Mirror3 script + collider)
- [ ] LargeMirror (sprite + Mirror4 script + collider)

### UI Panels:
- [ ] MedicineCabinetPanel (Canvas panel)
- [ ] BathtubDrainPanel (Canvas panel)
- [ ] VanityTerrorPanel (Canvas panel)
- [ ] EvidenceSequencePanel (Canvas panel)

### Audio:
- [ ] BackgroundMusic (AudioSource)
- [ ] SFXAudioSource (AudioSource)

---

## 📝 IMPLEMENTATION PRIORITY

### Phase 1 (Core):
1. Create all 4 puzzle panels
2. Implement drag-and-drop system
3. Implement timer system
4. Implement win/lose conditions

### Phase 2 (Integration):
1. Connect puzzles to flow controller
2. Implement Emily's reactions
3. Implement door unlock
4. Test all 4 puzzles

### Phase 3 (Polish):
1. Add visual effects
2. Add sound effects
3. Add Emily jumpscares
4. Balance difficulty/timing

---

## 🎯 SUCCESS CRITERIA

### Player Must:
- [ ] Solve all 4 mirror puzzles
- [ ] Complete each within time limit
- [ ] Survive Emily's attacks (no game overs)
- [ ] Unlock master bedroom door
- [ ] Proceed to Room 10

### Game Must:
- [ ] Track puzzle completion (4/4)
- [ ] Show clear feedback for success/failure
- [ ] Provide fair time limits
- [ ] Allow puzzles in any order
- [ ] Save progress between puzzles

---

**ROOM 09 ESTIMATED PLAYTIME**: 5-7 minutes (including exploration and puzzle solving)

**NEXT ROOM**: Room 10 (Master Bedroom - Final Revelation)
