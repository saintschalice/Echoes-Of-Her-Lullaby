# ROOM 10: MASTER BEDROOM - START HERE 🎮

## Quick Overview
This is the **FINAL ROOM** of the game where Lisa discovers the truth about her past. All mysteries are revealed, Emily's story is told, and the game reaches its emotional conclusion.

---

## What You Have

### ✅ 3 Scripts (Ready to Use)
1. **Room10_Dialogues.cs** - All dialogue strings (60+)
2. **Room10_FlowController.cs** - Main controller
3. **Room10_Interactable.cs** - Object interactions

### ✅ 4 Documentation Files
1. **START_HERE.md** (this file) - Quick start
2. **ROOM10_SUMMARY.md** - Package overview
3. **ROOM10_COMPLETE_DESIGN.md** - Full technical details
4. **ROOM10_DESIGNER_FLOW_TAGALOG.md** - Detailed Tagalog guide

---

## Setup Order (Follow This!)

### STEP 1: Read Documentation (5 minutes)
1. ✅ Read this file first (START_HERE.md)
2. ✅ Read ROOM10_SUMMARY.md for overview
3. ✅ Read ROOM10_DESIGNER_FLOW_TAGALOG.md for detailed flow
4. ✅ Keep ROOM10_COMPLETE_DESIGN.md open for reference

### STEP 2: Create GameObjects (15 minutes)

#### A. Main Controller
```
1. Create empty GameObject: "Room10_FlowController"
2. Add script: Room10_FlowController.cs
3. Leave inspector empty for now (we'll fill it later)
```

#### B. Emily
```
1. Create GameObject: "Emily_Manifestation"
2. Add SpriteRenderer
3. Assign Emily sprite (solid, visible)
4. Position in front of mirror
```

#### C. Mirror
```
1. Create GameObject: "TruthMirror"
2. Add SpriteRenderer (mirror sprite)
3. Add Room10_Interactable script
4. Set type: Mirror
5. Add BoxCollider2D or CircleCollider2D
```

#### D. Mirror Glow (Child of Mirror)
```
1. Create child GameObject: "MirrorGlow"
2. Add Particle System OR Sprite with glow effect
3. Disable this GameObject initially
```

#### E. Bed
```
1. Create GameObject: "Bed"
2. Add SpriteRenderer (bed sprite)
3. Add Room10_Interactable script
4. Set type: Bed
5. Add Collider2D
```

#### F. Diary
```
1. Create GameObject: "Diary"
2. Add SpriteRenderer (diary sprite)
3. Add Room10_Interactable script
4. Set type: Diary
5. Add Collider2D
```

#### G. Music Box
```
1. Create GameObject: "MusicBox"
2. Add SpriteRenderer (music box sprite)
3. Add Room10_Interactable script
4. Set type: MusicBox
5. Add AudioSource component
6. Add Collider2D
```

#### H. Background Music
```
1. Create empty GameObject: "BackgroundMusic"
2. Add AudioSource component
3. Set Loop: true
4. Set Play On Awake: false
```

### STEP 3: Create UI (10 minutes)

#### Flashback Panel
```
1. Find or create Canvas in scene
2. Right-click Canvas → UI → Panel
3. Rename to "FlashbackPanel"
4. Set to full screen (anchor: stretch, offsets: 0)
5. Set color: Black, Alpha: 0.9

Inside FlashbackPanel:
6. Add UI → Image, rename to "FlashbackImage"
   - Set to center of screen
   - Set size: 800x600 (or your preferred size)
   
7. Add UI → Text - TextMeshPro, rename to "DialogueText"
   - Position at bottom of screen
   - Set font size: 24
   - Set alignment: Center
   - Set color: White

8. Disable FlashbackPanel GameObject
```

### STEP 4: Assign References (10 minutes)

#### In Room10_FlowController Inspector:

**Story Milestones** (leave all unchecked):
- All booleans should be FALSE initially

**Emily State**:
- emilyManifestation: Drag "Emily_Manifestation" GameObject
- emilyHasFaded: FALSE

**Mirror**:
- truthMirror: Drag "TruthMirror" GameObject
- mirrorGlowEffect: Drag "MirrorGlow" GameObject
- canAccessMirror: FALSE

**Flashback**:
- flashbackPanel: Drag "FlashbackPanel" GameObject
- flashbackImages: Set size to 9 (we'll fill this later)

**Music Box**:
- musicBox: Drag "MusicBox" GameObject
- lullabyClip: Drag your lullaby audio clip

**Audio**:
- ambientAudio: Drag "BackgroundMusic" GameObject
- tenseMusicClip: Drag tense music audio clip
- peacefulMusicClip: Drag peaceful music audio clip

**Scene Transition**:
- nextSceneName: Type "EndingScene" (or your ending scene name)

#### In MusicBox Inspector:
- lullabyClip: Drag your lullaby audio clip

### STEP 5: Prepare Flashback Images (20 minutes)

You need **9 images** showing the possession and murder sequence:

1. Mother entering with pillow
2. Young Lisa in bed, terrified
3. Emily's spirit entering Lisa
4. Possessed Lisa moving
5. Mother trying to smother, Lisa fighting
6. Lisa's hands around mother's throat
7. Emily overlapping Lisa's body
8. Mother falling
9. Emily leaving, Lisa collapsing

**In Room10_FlowController Inspector**:
- Expand "Flashback Images" array
- Set size: 9
- For each entry (0-8):
  - Assign sprite (flashback image)
  - Copy dialogue from Room10_Dialogues.cs (FLASHBACK_1 through FLASHBACK_9)
  - Set displayDuration: 3

### STEP 6: Add to Inventory Database (5 minutes)

Open your InventoryManager or Item Database:
```
Item Name: Lullaby Fragment #4
Description: The final piece of Emily's lullaby. A music box melody that's been in my head my whole life.
Sprite: [Music box icon or musical note]
Category: Key Item
```

### STEP 7: Test Basic Setup (5 minutes)

1. Play the scene
2. Check if intro dialogues play
3. Check if player controls work after intro
4. Try clicking bed, diary, music box
5. Check if dialogues appear

**If errors appear**: Check ROOM10_COMPLETE_DESIGN.md troubleshooting section

---

## How It Works (Quick Version)

### Phase 1: Entry
- Lisa enters, sees mirror, Emily blocks it
- Player controls enabled after intro

### Phase 2: Exploration
- Player clicks **Bed** or **Diary** → Examination dialogues
- Player clicks **Music Box** → Gets Lullaby Fragment #4

### Phase 3: Unlock
- After examining room + finding music box → Mirror unlocks
- Mirror glow effect activates

### Phase 4: Revelation
- Player clicks **Mirror** → Final sequence begins
- Flashback shows 9 images (possession and murder)
- Understanding dialogues (Lisa and Emily talk)
- Forgiveness (Lisa forgives Emily)
- Departure (Emily fades away)
- Epilogue (Lisa can leave)
- Scene transitions to ending

---

## Testing Checklist

### Basic Tests:
- [ ] Scene loads without errors
- [ ] Intro dialogues play
- [ ] Can click bed/diary (dialogues show)
- [ ] Can click music box (lullaby plays, item added)
- [ ] Mirror unlocks after requirements met
- [ ] Mirror glow appears when unlocked

### Full Sequence Tests:
- [ ] Can click mirror to start final sequence
- [ ] All 9 flashback images show
- [ ] All dialogues play correctly
- [ ] Emily fades smoothly
- [ ] Music switches correctly (tense → lullaby → peaceful)
- [ ] Scene transitions to ending
- [ ] Game completion is saved

---

## Common First-Time Issues

### Issue: "NullReferenceException" errors
**Fix**: Check that ALL references are assigned in Room10_FlowController inspector

### Issue: Dialogues don't show
**Fix**: Make sure DialogueSystemV2 exists in scene

### Issue: Can't click objects
**Fix**: Make sure objects have Collider2D components

### Issue: Music doesn't play
**Fix**: Check that audio clips are assigned and AudioSource exists

### Issue: Flashback images don't show
**Fix**: Make sure flashbackImages array has 9 entries with sprites assigned

---

## Next Steps After Basic Setup

1. ✅ Test basic functionality
2. ✅ Create or assign all 9 flashback images
3. ✅ Test full sequence from start to end
4. ✅ Adjust timing if needed
5. ✅ Add visual effects (optional):
   - Screen shake during reality distortion
   - Color grading effects
   - Vignette during tense moments
   - Fade transitions
6. ✅ Create ending scene
7. ✅ Add to Build Settings
8. ✅ Final polish and testing

---

## Need More Details?

### For Technical Details:
→ Read **ROOM10_COMPLETE_DESIGN.md**

### For Flow Explanation (Tagalog):
→ Read **ROOM10_DESIGNER_FLOW_TAGALOG.md**

### For Package Overview:
→ Read **ROOM10_SUMMARY.md**

---

## Important Notes

### Dialogue System:
- All dialogues are 1-2 sentences (as required)
- Player must click to advance each dialogue
- Player controls disabled during dialogues
- Player controls re-enabled after sequences

### Progression:
- Must examine room (bed OR diary)
- Must find music box (Lullaby Fragment #4)
- Both required to unlock mirror

### Timing:
- Full sequence takes 10-15 minutes
- Don't rush - this is the emotional climax
- Let each moment breathe

### Music:
- Tense music at start
- Lullaby when music box found
- Peaceful music during Emily's departure

---

## Quick Reference

### Script Locations:
```
Assets/Scripts/Puzzle/Room 10/
├── Room10_Dialogues.cs
├── Room10_FlowController.cs
└── Room10_Interactable.cs
```

### Documentation Locations:
```
Assets/Scripts/Puzzle/Room 10/
├── START_HERE.md (this file)
├── ROOM10_SUMMARY.md
├── ROOM10_COMPLETE_DESIGN.md
└── ROOM10_DESIGNER_FLOW_TAGALOG.md
```

---

## Ready to Start?

1. ✅ Follow STEP 1-7 above
2. ✅ Test basic setup
3. ✅ Read detailed documentation
4. ✅ Implement full sequence
5. ✅ Test and polish

**This is the final room - make it memorable!** 🎮✨

---

## Questions?

Check the troubleshooting sections in:
- ROOM10_COMPLETE_DESIGN.md (English)
- ROOM10_DESIGNER_FLOW_TAGALOG.md (Tagalog)

**Good luck!** 🚀
