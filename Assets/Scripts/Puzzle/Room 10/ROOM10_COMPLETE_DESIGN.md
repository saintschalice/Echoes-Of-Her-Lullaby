# ROOM 10: MASTER BEDROOM - COMPLETE TECHNICAL DESIGN

## Overview
Room 10 is the **FINAL REVELATION ROOM** where Lisa confronts the ultimate truth about her past. This is the emotional climax of the game where all mysteries are resolved.

---

## Core Scripts

### 1. Room10_Dialogues.cs
**Purpose**: Contains all dialogue strings for the final revelation sequence

**Dialogue Categories**:
- Entry dialogues (Lisa enters room)
- Mirror magnetism (drawn to mirror)
- Emily's manifestation (blocking mirror)
- Room examination (bed, diary)
- Reality distortion (Emily's desperation)
- Lullaby fragment #4 (music box)
- Mirror approach (Lisa moves toward truth)
- Emily's acceptance (lets Lisa see)
- Flashback sequence (9 parts showing possession and murder)
- Final understanding (Lisa processes truth)
- Emily's explanation (her backstory)
- Lisa's response (processing emotions)
- Emily's apology (regret and sorrow)
- Forgiveness (Lisa forgives Emily)
- Emily's departure (fading away)
- Epilogue (Lisa can finally leave)

**Total Dialogues**: 60+ dialogue strings

---

### 2. Room10_FlowController.cs
**Purpose**: Main controller managing the entire revelation sequence

**Key Features**:
- **Story Milestones**: Tracks progression (intro, examined room, found lullaby, approached mirror, seen flashback, forgiven)
- **Emily State**: Manages Emily's solid manifestation and fading
- **Mirror System**: Controls mirror access and glow effects
- **Flashback System**: Plays full-screen flashback sequence with images
- **Music System**: Switches between tense, lullaby, and peaceful music
- **Scene Transition**: Loads ending scene after completion

**Flow Sequence**:
1. **Intro** → Lisa enters, Emily blocks mirror
2. **Examination** → Player examines bed and diary
3. **Lullaby** → Player finds music box (Lullaby Fragment #4)
4. **Unlock** → Mirror access unlocked after examination + lullaby
5. **Approach** → Lisa approaches mirror, Emily tries to stop
6. **Acceptance** → Emily accepts, lets Lisa see truth
7. **Flashback** → 9-part flashback showing possession and murder
8. **Understanding** → Lisa and Emily discuss what happened
9. **Forgiveness** → Lisa forgives Emily
10. **Departure** → Emily fades away peacefully
11. **Epilogue** → Lisa can finally leave the house
12. **End** → Load ending scene/credits

**Methods**:
- `PlayIntroSequence()` - Entry dialogues
- `OnRoomExamined()` - Called when bed/diary examined
- `OnLullabyFound()` - Called when music box found
- `CheckProgression()` - Checks if mirror can be unlocked
- `UnlockMirrorAccess()` - Unlocks mirror after requirements met
- `ApproachMirror()` - Triggers mirror sequence
- `MirrorApproachSequence()` - Approach and acceptance dialogues
- `PlayFlashbackSequence()` - Shows 9-part flashback
- `FinalUnderstandingSequence()` - Understanding and explanation
- `ForgivenessSequence()` - Forgiveness dialogues
- `EmilyDepartureSequence()` - Emily fades away
- `EpilogueSequence()` - Final dialogues and scene transition

---

### 3. Room10_Interactable.cs
**Purpose**: Handles interactions with room objects

**Interactable Types**:
1. **Bed** - Shows evidence of child sleeping with mother
2. **Diary** - Mother's final diary entry (murder plan)
3. **Music Box** - Contains Lullaby Fragment #4, plays Emily's melody
4. **Mirror** - The truth-revealing mirror (triggers main sequence)

**Interaction Flow**:
- **Bed/Diary**: Show examination dialogues → Mark room as examined
- **Music Box**: Show dialogues → Play lullaby → Add to inventory → Mark as found
- **Mirror**: Check if unlocked → Trigger mirror sequence OR show hint

**Key Methods**:
- `HandleInteraction()` - Routes to correct examination method
- `ExamineBed()` - Bed examination sequence
- `ExamineDiary()` - Diary examination sequence
- `ExamineMusicBox()` - Music box sequence (plays lullaby)
- `ApproachMirror()` - Triggers mirror revelation

---

## Unity Setup Requirements

### Scene Objects Needed

#### 1. Room10_FlowController GameObject
**Components**:
- Room10_FlowController script

**Inspector Settings**:
```
Story Milestones: (all false initially)
- isIntroDone: false
- hasExaminedRoom: false
- hasFoundLullaby: false
- hasApproachedMirror: false
- hasSeenFlashback: false
- hasForgiven: false

Emily State:
- emilyManifestation: [Drag Emily GameObject]
- emilyHasFaded: false

Mirror:
- truthMirror: [Drag Mirror GameObject]
- mirrorGlowEffect: [Drag Glow Effect GameObject]
- canAccessMirror: false

Flashback:
- flashbackPanel: [Drag Flashback Panel UI]
- flashbackImages: [Array of 9 FlashbackImage entries]
  - Each entry has: image (Sprite), dialogue (string), displayDuration (3f)

Music Box:
- musicBox: [Drag Music Box GameObject]
- lullabyClip: [Drag Lullaby Audio Clip]

Audio:
- ambientAudio: [Drag AudioSource]
- tenseMusicClip: [Drag Tense Music]
- peacefulMusicClip: [Drag Peaceful Music]

Scene Transition:
- nextSceneName: "EndingScene" (or "MainMenu")
```

#### 2. Emily Manifestation GameObject
**Components**:
- SpriteRenderer (for fading effect)
- Sprite: Solid Emily sprite (more visible than previous rooms)

**Position**: In front of mirror, blocking access

#### 3. Truth Mirror GameObject
**Components**:
- SpriteRenderer (mirror sprite)
- Room10_Interactable script (type: Mirror)
- Collider2D (for clicking)

**Child Object**: Glow Effect
- Particle System or Sprite with glow shader
- Initially disabled, enabled when mirror unlocked

#### 4. Bed GameObject
**Components**:
- SpriteRenderer (bed sprite showing two beds - child and mother)
- Room10_Interactable script (type: Bed)
- Collider2D (for clicking)

#### 5. Diary GameObject
**Components**:
- SpriteRenderer (diary sprite)
- Room10_Interactable script (type: Diary)
- Collider2D (for clicking)

**Position**: On nightstand or floor

#### 6. Music Box GameObject
**Components**:
- SpriteRenderer (music box sprite)
- Room10_Interactable script (type: MusicBox)
- AudioSource (for playing lullaby)
- Collider2D (for clicking)

**Inspector Settings**:
- lullabyClip: [Drag Lullaby Audio Clip]

#### 7. Flashback Panel (UI)
**Hierarchy**:
```
Canvas
└── FlashbackPanel
    ├── Background (Black Image, full screen)
    ├── FlashbackImage (Image component for showing sprites)
    └── DialogueText (TextMeshProUGUI for flashback narration)
```

**Setup**:
- Initially disabled
- Covers entire screen when active
- Shows flashback images in sequence

#### 8. Audio Sources
**Background Music AudioSource**:
- Plays tense music initially
- Switches to lullaby when music box found
- Switches to peaceful music during Emily's departure

---

## Flashback System

### Flashback Images (9 total)
You need to create or assign 9 sprites showing the possession and murder sequence:

1. **Image 1**: Mother entering room with pillow
2. **Image 2**: Young Lisa in bed, terrified
3. **Image 3**: Emily's spirit entering Lisa's body (ghostly overlay)
4. **Image 4**: Possessed Lisa's body moving (Emily's will)
5. **Image 5**: Mother trying to smother Lisa, Lisa fighting back
6. **Image 6**: Lisa's hands around mother's throat (not her will)
7. **Image 7**: Emily's ghostly form overlapping Lisa's body
8. **Image 8**: Mother falling, going still
9. **Image 9**: Emily leaving Lisa's body, Lisa collapsing

### Flashback Panel Setup
1. Create UI Canvas (if not exists)
2. Add Panel GameObject named "FlashbackPanel"
3. Add black background Image (full screen, alpha 0.9)
4. Add Image component for showing flashback sprites
5. Add TextMeshProUGUI for dialogue text
6. Disable panel initially
7. Drag to Room10_FlowController inspector

---

## Progression Requirements

### To Unlock Mirror Access:
1. ✅ Intro sequence completed
2. ✅ Room examined (bed OR diary)
3. ✅ Lullaby Fragment #4 found (music box)

### After Mirror Unlocked:
- Mirror glow effect activates
- Emily's breakdown dialogues play
- Player can interact with mirror to trigger final sequence

---

## Audio Requirements

### Music Tracks Needed:
1. **Tense Music** - Plays during intro and examination phase
2. **Lullaby Clip** - Emily's melody (plays when music box found)
3. **Peaceful Music** - Plays during Emily's departure and epilogue

### Audio Flow:
- Start: Tense Music (loop)
- Music Box Found: Switch to Lullaby (loop)
- Emily Departs: Switch to Peaceful Music (loop)
- End: Fade out during scene transition

---

## Item Database Entry

### Lullaby Fragment #4
```
Item Name: Lullaby Fragment #4
Description: The final piece of Emily's lullaby. A music box melody that's been in my head my whole life.
Sprite: [Music box icon or musical note]
Category: Key Item
```

**Add to InventoryManager database**

---

## Save System Integration

### Save Point:
- Game completion saved after epilogue sequence
- `SaveSystem.Instance?.MarkPuzzleSolved("game_complete")`

### What to Save:
- Game completion flag
- Final scene reached
- All lullaby fragments collected (4/4)

---

## Scene Transition

### After Epilogue:
1. Fade to black (2 seconds)
2. Save game completion
3. Load ending scene: `SceneManager.LoadScene(nextSceneName)`

### Ending Scene Options:
- **Credits Scene** - Show game credits
- **Ending Cutscene** - Final cinematic
- **Main Menu** - Return to main menu with "Completed" indicator
- **Thank You Screen** - Thank player for playing

---

## Visual Effects

### Recommended Effects:
1. **Mirror Glow** - Particle system or glowing sprite when unlocked
2. **Emily Fade** - Smooth alpha fade from 1.0 to 0.0 over 3 seconds
3. **Reality Distortion** - Screen shake, color shift, vignette effect
4. **Flashback Transition** - Fade to white/black between flashback images
5. **Final Fade** - Fade to black at end before scene transition

---

## Testing Checklist

### Sequence Testing:
- [ ] Intro plays correctly on scene start
- [ ] Can examine bed (shows dialogues, marks examined)
- [ ] Can examine diary (shows dialogues, marks examined)
- [ ] Can find music box (plays lullaby, adds to inventory)
- [ ] Mirror unlocks after bed/diary + music box
- [ ] Mirror glow effect appears when unlocked
- [ ] Can interact with mirror to trigger sequence
- [ ] Approach sequence plays correctly
- [ ] Flashback shows all 9 images with dialogues
- [ ] Understanding sequence plays correctly
- [ ] Forgiveness sequence plays correctly
- [ ] Emily fades out smoothly
- [ ] Epilogue plays correctly
- [ ] Scene transitions to ending

### Audio Testing:
- [ ] Tense music plays at start
- [ ] Lullaby plays when music box found
- [ ] Peaceful music plays during departure
- [ ] All audio transitions are smooth

### UI Testing:
- [ ] Flashback panel covers screen
- [ ] Flashback images display correctly
- [ ] Dialogue text is readable
- [ ] Player controls disabled during sequences
- [ ] Player controls re-enabled after sequences

---

## Common Issues & Solutions

### Issue: Mirror won't unlock
**Solution**: Check that both `hasExaminedRoom` and `hasFoundLullaby` are true

### Issue: Flashback images not showing
**Solution**: Ensure flashbackImages array is populated in inspector with 9 entries

### Issue: Emily won't fade
**Solution**: Ensure Emily GameObject has SpriteRenderer component

### Issue: Music doesn't switch
**Solution**: Check that all audio clips are assigned in inspector

### Issue: Scene won't transition
**Solution**: Verify nextSceneName matches actual scene name in Build Settings

---

## Performance Notes

- Flashback sequence is dialogue-heavy (60+ dialogues total)
- Each dialogue waits for player click before continuing
- Total sequence can take 10-15 minutes depending on player reading speed
- Consider adding "Skip" option for replays (optional)

---

## Emotional Pacing

This room is the **emotional climax** of the game. Pacing is critical:

1. **Tension** (Intro) - Build anticipation
2. **Investigation** (Examination) - Let player discover clues
3. **Revelation** (Mirror) - Show the truth
4. **Understanding** (Dialogue) - Process emotions
5. **Resolution** (Forgiveness) - Emotional catharsis
6. **Peace** (Departure) - Calm after storm
7. **Closure** (Epilogue) - Satisfying ending

**Key**: Don't rush. Let each moment breathe. This is the payoff for the entire game.

---

## Next Steps for Designer

1. ✅ Read START_HERE.md for quick setup guide
2. ✅ Read ROOM10_DESIGNER_FLOW_TAGALOG.md for detailed Tagalog explanation
3. ✅ Create/assign all GameObjects in Unity scene
4. ✅ Assign all references in inspectors
5. ✅ Create or assign 9 flashback images
6. ✅ Test each sequence individually
7. ✅ Test full playthrough from start to end
8. ✅ Adjust timing/pacing as needed
9. ✅ Add visual effects for polish
10. ✅ Create ending scene

---

**STATUS**: ✅ COMPLETE - All scripts created, ready for Unity implementation
