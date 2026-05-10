# ROOM 10: MASTER BEDROOM - ASSETS & FLOW SUMMARY

## 🎯 ROOM OVERVIEW
Room 10 is the **FINAL REVELATION ROOM** where Lisa confronts the ultimate truth. This is the emotional climax and ending of the entire game. Lisa sees the truth in the mirror, learns about Emily's past, forgives her, and finally finds peace.

---

## 📦 ASSETS NEEDED

### 🖼️ SPRITES (Scene Objects)

#### Main Scene:
- [ ] **Master Bedroom Background** - Large, ominous bedroom
- [ ] **Emily Manifestation** - Solid, visible Emily (most solid form in game)
- [ ] **Truth Mirror** - Large ornate mirror (centerpiece of room)
- [ ] **Mirror Glow Effect** - Glowing particle effect or sprite overlay
- [ ] **Child's Bed** - Small bed next to mother's bed
- [ ] **Mother's Bed** - Large bed (evidence of struggle)
- [ ] **Diary** - Mother's final diary on nightstand or floor
- [ ] **Music Box** - Ornate music box (contains Lullaby Fragment #4)
- [ ] **Overturned Furniture** - Evidence of violent struggle
- [ ] **Blood Stains** - On floor, walls, furniture

#### Flashback Images (9 Required):
- [ ] **Flashback 1**: Mother entering room with pillow
- [ ] **Flashback 2**: Young Lisa in bed, terrified, mother approaching
- [ ] **Flashback 3**: Emily's spirit entering Lisa's body (ghostly overlay)
- [ ] **Flashback 4**: Possessed Lisa's body moving (Emily's will controlling)
- [ ] **Flashback 5**: Mother trying to smother Lisa, Lisa fighting back
- [ ] **Flashback 6**: Lisa's small hands around mother's throat
- [ ] **Flashback 7**: Emily's ghostly form overlapping Lisa's body
- [ ] **Flashback 8**: Mother struggling, falling, going still
- [ ] **Flashback 9**: Emily leaving Lisa's body, Lisa collapsing, blood everywhere

---

### 🎨 UI PANELS

#### Flashback Panel (Full-Screen)
```
Components:
- Black Background (full screen, alpha 0.9)
- Flashback Image Display (center, 800x600 or larger)
- Dialogue Text (bottom, TextMeshProUGUI)
- Fade In/Out Animation
```

**Layout**:
```
┌─────────────────────────────────────────┐
│                                         │
│                                         │
│         [Flashback Image]               │
│                                         │
│                                         │
│─────────────────────────────────────────│
│                                         │
│  "Dialogue text appears here..."        │
│                                         │
└─────────────────────────────────────────┘
```

---

### 🎵 AUDIO

#### Music Tracks (3 Required):
- [ ] **Tense Music** - Dark, ominous (Intro, Emily blocks, Exploration)
- [ ] **Lullaby Clip** - Emily's melody, peaceful (Music box, Mirror sequence)
- [ ] **Peaceful Music** - Calm, resolution (Emily's departure, Epilogue)

#### Sound Effects:
- [ ] **Footsteps** - Lisa walking
- [ ] **Door Open** - Lisa entering room
- [ ] **Music Box Wind** - Music box winding up
- [ ] **Music Box Play** - Lullaby playing
- [ ] **Mirror Glow** - Magical sound when mirror activates
- [ ] **Reality Distortion** - Warping, bending sounds
- [ ] **Emily Whisper** - Emily's voice effects
- [ ] **Fade Sound** - Emily fading away
- [ ] **Heartbeat** - Tension moments
- [ ] **Ambient Wind** - Supernatural atmosphere

---

### 📝 INVENTORY ITEM

#### Lullaby Fragment #4
```
Item Name: Lullaby Fragment #4
Description: The final piece of Emily's lullaby. A music box melody that's been in my head my whole life.
Sprite: Music box icon or musical note
Category: Key Item
```

**Add to Inventory Database**

---

## 🎮 COMPLETE FLOW SEQUENCE

### PHASE 1: ENTRY (1 minute)
```
AUTOMATIC SEQUENCE:

1. Lisa enters master bedroom
   - Player controls DISABLED
   - Camera focuses on Lisa

2. Entry Dialogues:
   - "This room... it feels like the center of everything."
   - "All the pain, all the secrets lead here."

3. Mirror Magnetism:
   - "The mirror... I feel drawn to it."
   - "I need to look into it. Something's calling me."

4. Player controls RE-ENABLED
   - Can now explore room

MUSIC: Tense Music starts
```

---

### PHASE 2: EMILY BLOCKS (30 seconds)
```
AUTOMATIC SEQUENCE:

1. Emily manifests (solid, blocking mirror)
   - Emily sprite appears in front of mirror
   - Most visible she's been in entire game

2. Emily's Dialogues:
   - "Emily appears. More solid than ever. Blocking the mirror."
   - (Emily) "I've been practicing what to tell you for decades."
   - (Emily) "But every word tastes like ash now that you're here."

3. Player controls RE-ENABLED
   - Can explore room
   - Cannot access mirror yet (Emily blocking)

MUSIC: Tense Music continues
```

---

### PHASE 3: EXPLORATION (2-3 minutes)
```
PLAYER CHOICE - Can interact with objects in any order:

REQUIREMENTS TO PROGRESS:
- Must examine Bed OR Diary (sets hasExaminedRoom = true)
- Must find Music Box (sets hasFoundLullaby = true)

┌─────────────────────────────────────────────────┐
│  INTERACTABLE OBJECTS:                          │
│                                                 │
│  1. BED                                         │
│  2. DIARY                                       │
│  3. MUSIC BOX                                   │
│  4. MIRROR (locked until requirements met)      │
└─────────────────────────────────────────────────┘
```

#### OBJECT 1: BED
```
INTERACTION:
1. Player clicks Bed
2. Player controls DISABLED
3. Examination dialogues play:
   - "Evidence of violent struggle everywhere. Furniture overturned. Blood stains."
   - "A small child's bed... next to mother's bed."
   - "A child slept here... with her mother. That child was me."
4. Player controls RE-ENABLED
5. hasExaminedRoom = TRUE

RESULT: Requirement 1 met
```

#### OBJECT 2: DIARY
```
INTERACTION:
1. Player clicks Diary
2. Player controls DISABLED
3. Examination dialogues play:
   - "Mother's final diary entry."
   - "'Tonight I end the child's defiance. She will learn obedience, or she will not learn at all.'"
4. Player controls RE-ENABLED
5. hasExaminedRoom = TRUE

RESULT: Requirement 1 met
```

#### OBJECT 3: MUSIC BOX
```
INTERACTION:
1. Player clicks Music Box
2. Player controls DISABLED
3. Music box dialogues play:
   - "A music box. Emily's melody."
   - [Lullaby plays - 2 second pause]
   - "This song... it's been in my head my whole life."
   - "Emily sang this to me. To calm me. To protect me."
4. NOTIFICATION: "Lullaby Fragment #4 added to inventory"
5. Player controls RE-ENABLED
6. hasFoundLullaby = TRUE

MUSIC: Switches from Tense to Lullaby
RESULT: Requirement 2 met
```

---

### PHASE 4: MIRROR UNLOCK (1 minute)
```
AUTOMATIC SEQUENCE (triggers when both requirements met):

1. Reality Distortion:
   - Player controls DISABLED
   - Visual effects: screen shake, color shift, vignette
   - "The room... it's changing. Temperature drops. Time slows."
   - "Shadows move incorrectly. Emily's desperation warps reality itself."

2. Emily's Breakdown:
   - (Emily) "She was going to kill you that night. I couldn't let her."
   - (Emily) "But what I did... what I made you do..."
   - (Emily) "I possessed you. A child. I used your hands."

3. Mirror Unlocks:
   - Mirror glow effect ACTIVATES
   - canAccessMirror = TRUE
   - Player controls RE-ENABLED

MUSIC: Lullaby continues
VISUAL: Mirror glows, indicating it can be clicked
```

---

### PHASE 5: MIRROR APPROACH (1.5 minutes)
```
INTERACTION:
Player clicks Mirror

SEQUENCE:
1. Player controls DISABLED

2. Lisa Approaches:
   - "I move toward the mirror. Emily tries to stop me."
   - "I need to know. I need to remember."

3. Emily Desperate:
   - (Emily) "Please, Lisa. You don't need to see this."
   - (Emily) "I wanted to protect you from this memory forever."

4. Emily Accepts:
   - (Emily) "I always knew this day would come."
   - (Emily) "I just hoped I'd found better words by now."
   - (Emily) "Look into the mirror, Lisa. See what I did. See what we did."

5. Mirror Activates:
   - "The mirror glows. Images form. The past comes alive."
   - "I see... that night. The night everything changed."

MUSIC: Lullaby continues
VISUAL: Mirror glows intensely
```

---

### PHASE 6: FLASHBACK SEQUENCE (2 minutes)
```
FULL-SCREEN FLASHBACK:

1. Flashback Panel appears (covers entire screen)
2. Player controls DISABLED
3. 9 images shown in sequence with dialogues:

IMAGE 1: Mother with pillow
- "Mother enters the room. She's holding something. A pillow."

IMAGE 2: Young Lisa terrified
- "I'm in bed. Small. Terrified. She approaches."

IMAGE 3: Emily enters Lisa
- "Emily's spirit... she enters me. Possesses me."

IMAGE 4: Possessed Lisa moves
- "My small body moves on its own. Emily's will, my hands."

IMAGE 5: Fighting back
- "Mother tries to smother me. Emily makes me fight back."

IMAGE 6: Hands on throat
- "My hands... around mother's throat. But they're not my hands."

IMAGE 7: Emily overlapping
- "Emily's ghostly form overlaps my small body. We move as one."

IMAGE 8: Mother falls
- "Mother struggles. Falls. Goes still."

IMAGE 9: Emily leaves, Lisa collapses
- "Emily leaves my body. I collapse. Blood everywhere."

4. Flashback Panel closes
5. Return to bedroom scene

MUSIC: Lullaby continues (emotional)
TIMING: Each image displays for 3 seconds + dialogue time
```

---

### PHASE 7: UNDERSTANDING (3 minutes)
```
DIALOGUE SEQUENCE:

1. Lisa Processes (2 dialogues):
   - "I... I killed her. We killed her."
   - "No. Emily killed her. Using me. To save me."

2. Lisa Confronts (2 dialogues):
   - "You possessed me. Made me kill my own mother."
   - "To save me from her. But at what cost?"

3. Emily Explains (5 dialogues):
   - (Emily) "She was going to kill you. I had no choice."
   - (Emily) "I was a child once too. Killed by my own mother."
   - (Emily) "When I died, I became... this. A protector. A guardian."
   - (Emily) "I found you. Felt your pain. Your fear. It was my pain. My fear."
   - (Emily) "I couldn't let another child die the way I did."

4. Lisa Responds (4 dialogues):
   - "You saved me. But you made me a killer."
   - "I've lived my whole life not knowing. Not remembering."
   - "The nightmares. The fear. The feeling that something was wrong."
   - "It was all real. It all happened."

5. Emily Apologizes (4 dialogues):
   - (Emily) "I'm sorry. I'm so sorry, Lisa."
   - (Emily) "I saved your life, but I stole your innocence."
   - (Emily) "I made you forget, hoping you could live a normal life."
   - (Emily) "But the truth always finds a way back."

MUSIC: Lullaby continues
PLAYER: Controls remain DISABLED
TOTAL: 17 dialogues
```

---

### PHASE 8: FORGIVENESS (1 minute)
```
DIALOGUE SEQUENCE:

1. Lisa Forgives (3 dialogues):
   - "You did what you had to do. To save a child."
   - "You've been protecting me ever since. Carrying this burden alone."
   - "I forgive you, Emily. And I thank you."

2. Emily's Relief (2 dialogues):
   - (Emily) "Thank you. I've waited so long to hear those words."
   - (Emily) "I can finally... let go."

MUSIC: Switches to Peaceful Music
PLAYER: Controls remain DISABLED
EMOTIONAL PEAK: This is the catharsis
```

---

### PHASE 9: EMILY'S DEPARTURE (1.5 minutes)
```
SEQUENCE:

1. Emily Fades Dialogues (3 dialogues):
   - "Emily begins to fade. Her form becoming light."
   - (Emily) "You don't need me anymore, Lisa. You're strong enough now."
   - (Emily) "Live your life. Be free. Remember me, but don't let me haunt you."

2. Emily Sprite Fades:
   - 3-second alpha fade (1.0 → 0.0)
   - Smooth transition
   - Light particle effects (optional)

3. Final Goodbye (2 dialogues):
   - "Goodbye, Emily. My protector. My friend."
   - "Thank you for saving me. Thank you for everything."

4. Emily disappears completely
   - emilyHasFaded = TRUE

MUSIC: Peaceful Music continues
VISUAL: Emily fades to nothing
EMOTIONAL: Bittersweet goodbye
```

---

### PHASE 10: EPILOGUE & ENDING (1.5 minutes)
```
FINAL SEQUENCE:

1. Epilogue Dialogues (3 dialogues):
   - "The house is quiet now. The truth revealed. The burden lifted."
   - "I can finally leave this place. Leave the past behind."
   - "I survived. We both survived. And now, we can both rest."

2. Fade to Black:
   - 2-second fade to black screen

3. Save Game:
   - SaveSystem.Instance?.MarkPuzzleSolved("game_complete")

4. Scene Transition:
   - SceneManager.LoadScene("EndingScene")
   - OR load credits
   - OR return to main menu with completion flag

MUSIC: Peaceful Music fades out
PLAYER: Controls remain DISABLED
GAME: COMPLETE
```

---

## ⏱️ TIMING BREAKDOWN

```
Phase 1: Entry                    1:00
Phase 2: Emily Blocks             0:30
Phase 3: Exploration              2:00-3:00
Phase 4: Mirror Unlock            1:00
Phase 5: Mirror Approach          1:30
Phase 6: Flashback                2:00
Phase 7: Understanding            3:00
Phase 8: Forgiveness              1:00
Phase 9: Departure                1:30
Phase 10: Epilogue                1:30
                                ─────────
TOTAL PLAYTIME:                  14-15 minutes
```

---

## 🎯 PROGRESSION REQUIREMENTS

### To Unlock Mirror:
```
┌─────────────────────────────────────┐
│  REQUIREMENT 1:                     │
│  hasExaminedRoom = TRUE             │
│  (Click Bed OR Diary)               │
└─────────────────────────────────────┘
              +
┌─────────────────────────────────────┐
│  REQUIREMENT 2:                     │
│  hasFoundLullaby = TRUE             │
│  (Click Music Box)                  │
└─────────────────────────────────────┘
              ║
              ▼
┌─────────────────────────────────────┐
│  RESULT:                            │
│  canAccessMirror = TRUE             │
│  Mirror glow activates              │
│  Can click mirror                   │
└─────────────────────────────────────┘
```

---

## 🎨 VISUAL EFFECTS

### Required Effects:
- [ ] **Mirror Glow** - Particle system or glowing sprite (when unlocked)
- [ ] **Reality Distortion** - Screen shake, color shift, vignette (unlock phase)
- [ ] **Emily Fade** - Smooth alpha fade 1.0 → 0.0 over 3 seconds
- [ ] **Flashback Transitions** - Fade in/out between flashback images
- [ ] **Final Fade** - Fade to black at ending (2 seconds)

### Optional Effects:
- [ ] **Light Particles** - When Emily fades
- [ ] **Screen Vignette** - During tense moments
- [ ] **Color Grading** - Desaturate during flashback
- [ ] **Bloom Effect** - On mirror glow
- [ ] **Dust Particles** - Ambient atmosphere

---

## 🎮 PLAYER CONTROLS TIMELINE

```
PHASE               PLAYER CONTROLS    JOYSTICK
─────────────────────────────────────────────────
Entry               DISABLED           Hidden
Emily Blocks        ENABLED            Visible
Exploration         ENABLED            Visible
Mirror Unlock       DISABLED           Hidden
(Brief moment)      ENABLED            Visible
Mirror Approach     DISABLED           Hidden
Flashback           DISABLED           Hidden
Understanding       DISABLED           Hidden
Forgiveness         DISABLED           Hidden
Departure           DISABLED           Hidden
Epilogue            DISABLED           Hidden
```

**Summary**: Player only has control during exploration phase

---

## 📊 DIALOGUE COUNT

```
Phase 1: Entry                    4 dialogues
Phase 2: Emily Blocks             3 dialogues
Phase 3: Examination
  - Bed                           3 dialogues
  - Diary                         2 dialogues
  - Music Box                     4 dialogues
Phase 4: Unlock                   5 dialogues
Phase 5: Approach                 9 dialogues
Phase 6: Flashback                9 dialogues
Phase 7: Understanding           17 dialogues
Phase 8: Forgiveness              5 dialogues
Phase 9: Departure                5 dialogues
Phase 10: Epilogue                3 dialogues
                                ─────────────
TOTAL:                           60+ dialogues
```

**All dialogues are 1-2 sentences as required**

---

## 🔧 UNITY SETUP CHECKLIST

### Scene Objects:
- [ ] Room10_FlowController (empty + script)
- [ ] Emily_Manifestation (sprite + SpriteRenderer)
- [ ] TruthMirror (sprite + Room10_Interactable + collider)
- [ ] MirrorGlow (particle/sprite, initially disabled)
- [ ] Bed (sprite + Room10_Interactable + collider)
- [ ] Diary (sprite + Room10_Interactable + collider)
- [ ] MusicBox (sprite + Room10_Interactable + AudioSource + collider)
- [ ] BackgroundMusic (AudioSource)

### UI:
- [ ] Canvas (if not exists)
- [ ] FlashbackPanel (Panel, initially disabled)
  - [ ] Background (Black Image, full screen)
  - [ ] FlashbackImage (Image component)
  - [ ] DialogueText (TextMeshProUGUI)

### Inspector Setup (Room10_FlowController):
- [ ] Assign all GameObjects
- [ ] Assign 9 flashback images with dialogues
- [ ] Assign 3 audio clips
- [ ] Set nextSceneName

### Inventory:
- [ ] Add "Lullaby Fragment #4" to database

---

## 🎯 SUCCESS CRITERIA

### Story Must:
- [ ] Reveal complete truth about possession and murder
- [ ] Show Emily's backstory (she was also killed by her mother)
- [ ] Provide emotional catharsis through forgiveness
- [ ] Give satisfying closure to Lisa's journey
- [ ] End game properly

### Technical Must:
- [ ] All 60+ dialogues play correctly
- [ ] All 9 flashback images display
- [ ] Music switches correctly (tense → lullaby → peaceful)
- [ ] Emily fades smoothly
- [ ] Scene transitions to ending
- [ ] Game completion is saved

### Emotional Must:
- [ ] Build tension (entry, Emily blocks)
- [ ] Allow discovery (exploration)
- [ ] Deliver revelation (flashback)
- [ ] Process emotions (understanding)
- [ ] Provide catharsis (forgiveness)
- [ ] Give closure (departure, epilogue)

---

## 📝 IMPLEMENTATION PRIORITY

### Phase 1 (Core - Day 1):
1. Create all scene objects
2. Create flashback panel UI
3. Implement Room10_FlowController basic flow
4. Test intro and exploration

### Phase 2 (Interactions - Day 2):
1. Implement all interactable objects
2. Implement progression requirements
3. Implement mirror unlock
4. Test progression flow

### Phase 3 (Flashback - Day 3):
1. Create/assign 9 flashback images
2. Implement flashback sequence
3. Test flashback display
4. Adjust timing

### Phase 4 (Dialogue - Day 4):
1. Implement all dialogue sequences
2. Test dialogue flow
3. Adjust pacing
4. Test player control management

### Phase 5 (Polish - Day 5):
1. Add visual effects
2. Add sound effects
3. Implement Emily fade
4. Test music switching
5. Create ending scene
6. Final testing

---

## 🎊 EMOTIONAL PACING GUIDE

```
TENSION CURVE:

High ┤                    ╱╲
     │                   ╱  ╲
     │                  ╱    ╲
     │                 ╱      ╲
     │        ╱╲      ╱        ╲
     │       ╱  ╲    ╱          ╲___
     │      ╱    ╲  ╱                ╲___
Low  ┤_____╱      ╲╱                     ╲___
     └─────────────────────────────────────────
     Entry  Explore  Unlock  Flashback  Forgive  Depart

EMOTIONAL BEATS:
1. Tension (Entry, Emily blocks)
2. Investigation (Exploration)
3. Revelation (Mirror unlock, Approach)
4. Shock (Flashback)
5. Understanding (Dialogue)
6. Catharsis (Forgiveness)
7. Peace (Departure)
8. Closure (Epilogue)
```

---

## 🎮 PLAYER EXPERIENCE GOALS

### Player Should Feel:
1. **Drawn** - Magnetism toward mirror
2. **Blocked** - Emily preventing access
3. **Curious** - Exploring room for clues
4. **Unlocked** - Progress when requirements met
5. **Shocked** - Truth revealed in flashback
6. **Understanding** - Processing what happened
7. **Compassion** - Empathy for Emily
8. **Catharsis** - Release through forgiveness
9. **Peace** - Calm after storm
10. **Closure** - Satisfying ending

---

## 📋 TESTING CHECKLIST

### Basic Tests:
- [ ] Scene loads without errors
- [ ] Intro plays correctly
- [ ] Can examine bed/diary
- [ ] Can find music box
- [ ] Lullaby Fragment #4 added to inventory
- [ ] Mirror unlocks after requirements
- [ ] Mirror glow appears

### Sequence Tests:
- [ ] Can click mirror to start sequence
- [ ] All 9 flashback images display
- [ ] All dialogues play in order
- [ ] Emily fades smoothly
- [ ] Music switches correctly
- [ ] Scene transitions to ending

### Polish Tests:
- [ ] Visual effects work
- [ ] Sound effects play
- [ ] Timing feels right
- [ ] Pacing is good
- [ ] Emotional impact achieved
- [ ] Game completion saved

---

**ROOM 10 ESTIMATED PLAYTIME**: 14-15 minutes

**THIS IS THE FINAL ROOM - MAKE IT UNFORGETTABLE!** 🎮✨
