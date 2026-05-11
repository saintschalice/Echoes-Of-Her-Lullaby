# Room 09 - Complete Summary (Tagalog)

## 🎯 Overview

**Room 09 = FINAL ROOM!** Walang Room 10. Lahat ng ending ay cutscene na lang after ng 4 mirror puzzles.

---

## 📋 Complete Flow

### 1. Entry (1 minute)
- Lisa enters through broken mirror
- Gets cut by glass
- Door slams shut (locked)
- Emily manifests at full power
- Intro dialogue

### 2. Four Mirror Puzzles (5-10 minutes)
Pwedeng i-solve in any order:

**Mirror 1: Medicine Cabinet** (60 seconds)
- 6 prescription bottles
- Arrange chronologically (1973 → 1976)
- Reveals: Mother planning for years

**Mirror 2: Bathtub Drain** (60 seconds)
- Click drain button
- Reassemble 4 torn note pieces
- Reveals: Murder-suicide note

**Mirror 3: Vanity Terror** (90 seconds)
- 8 diary pages (shuffled)
- Arrange chronologically
- Reveals: Mother's descent into madness

**Mirror 4: Evidence Sequence** (90 seconds)
- 4 evidence items (Rope, Pills, Knife, BloodyTowel)
- Arrange in order of murder plan
- Reveals: Complete murder plan

### 3. All Mirrors Complete (2 minutes)
- Mother's voice echoes
- Emily's breakdown
- Emily collapses (becomes translucent)
- Emily's final words

### 4. Final Cutscene (3 minutes)
**20 dialogues** revealing complete truth:
- Lisa understands everything
- Emily saved Lisa but became a monster
- Lisa forgives Emily
- Emily fades away peacefully
- Both are finally free

### 5. The End
- Fade to black
- Save game completion
- Return to Main Menu

---

## 🎬 Final Cutscene (20 Dialogues)

### Part 1: Realization (1-3)
Lisa realizes mother planned murder-suicide

### Part 2: Understanding Emily (4-6)
Emily saved Lisa but absorbed mother's violence

### Part 3: Mother's Plan (7-9)
Rope → Pills → Knife → Cleanup

### Part 4: Emily's Sacrifice (10-12)
Emily stopped mother but became a monster

### Part 5: Forgiveness (13-15)
Lisa forgives Emily, Emily finds peace

### Part 6: Emily Fades (16-18)
Emily becomes light and disappears

### Part 7: Final Words (19-20)
Both are finally free

### Part 8: The End
Fade to black → Main Menu

---

## 📦 Assets Needed

### Scene Objects:
- Bathroom background
- Broken mirror (entry point)
- Blood effects
- Emily sprite (full power)
- Locked door
- 4 mirror objects (interactable)

### Mirror 1 Assets:
- Medicine Cabinet sprite
- 6 bottle sprites with labels
- Mirror1_Panel (UI)

### Mirror 2 Assets:
- Bathtub sprite (with/without water)
- Drain button
- 4 torn note pieces
- Mirror2_Panel (UI)

### Mirror 3 Assets:
- Vanity mirror sprite
- 8 diary page sprites
- Mirror3_Panel (UI)

### Mirror 4 Assets:
- Large mirror sprite
- 4 evidence item sprites
- 4 flashback images
- Mirror4_Panel (UI)

---

## 🔧 Scripts Needed

### Main Scripts:
1. ✅ **Room09_FlowController.cs** - Main controller
2. ✅ **Room09_Dialogues.cs** - All dialogues (including 20 ending dialogues)
3. ✅ **Room09_Interactable.cs** - Mirror interactions

### Puzzle Scripts:
4. ✅ **Mirror1_MedicineCabinet.cs** - Medicine cabinet puzzle
5. ✅ **Mirror2_BathtubDrain.cs** - Bathtub drain puzzle
6. ✅ **Mirror3_DiaryArrangement.cs** - Diary arrangement puzzle
7. ✅ **Mirror4_EvidenceSequence.cs** - Evidence sequence puzzle

**All scripts are COMPLETE!** ✅

---

## 🎮 Unity Setup

### GameObjects Needed:

1. **Room09_FlowController** (empty GameObject)
   - Add `Room09_FlowController` component
   - Assign Emily sprite
   - Assign audio clips

2. **Mirror_1** (sprite + collider)
   - Add `Mirror1_MedicineCabinet` component
   - Add `Room09_Interactable` component (mirrorNumber = 1)
   - Create Mirror1_Panel (UI)

3. **Mirror_2** (sprite + collider)
   - Add `Mirror2_BathtubDrain` component
   - Add `Room09_Interactable` component (mirrorNumber = 2)
   - Create Mirror2_Panel (UI)

4. **Mirror_3** (sprite + collider)
   - Add `Mirror3_DiaryArrangement` component
   - Add `Room09_Interactable` component (mirrorNumber = 3)
   - Create Mirror3_Panel (UI)

5. **Mirror_4** (sprite + collider)
   - Add `Mirror4_EvidenceSequence` component
   - Add `Room09_Interactable` component (mirrorNumber = 4)
   - Create Mirror4_Panel (UI)

---

## ⚠️ IMPORTANTE: Panel Hierarchy

**LAHAT ng puzzle items ay dapat nasa LOOB ng panel!**

### Mirror 1:
```
Mirror1_Panel
├── Timer_Text
├── Mistakes_Text
├── Hint_Text
├── Slot_1 to Slot_6
└── All 6 bottles (Antidepressants, Lithium, etc.)
```

### Mirror 2:
```
Mirror2_Panel
├── Timer_Text
├── Bathtub_Container
│   ├── Bathtub_Image
│   └── DrainCover_Button
└── NotePieces_Container
    ├── Slot_1 to Slot_4
    └── All 4 note pieces
```

### Mirror 3:
```
Mirror3_Panel
├── Timer_Text
├── Slot_1 to Slot_8
└── All 8 diary pages
```

### Mirror 4:
```
Mirror4_Panel
├── Timer_Text
├── Flashback_Image
├── Frame_1 to Frame_4
└── All 4 evidence items (Rope, Pills, Knife, BloodyTowel)
```

**Kung nasa labas ang items, hindi sila matatago!**

---

## 🎯 Correct Solutions

### Mirror 1:
```
Slot 1: Antidepressants_1973
Slot 2: Lithium_1974
Slot 3: Valium_1975
Slot 4: PainPills_1975
Slot 5: SleepingPills_1976
Slot 6: UnknownPills_1976
```

### Mirror 2:
```
Slot 1: Note_Piece_1 ("Tonight I")
Slot 2: Note_Piece_2 ("end this child's")
Slot 3: Note_Piece_3 ("suffering and")
Slot 4: Note_Piece_4 ("mine forever")
```

### Mirror 3:
```
Slot 1: DiaryPage_1
Slot 2: DiaryPage_2
... (in order)
Slot 8: DiaryPage_8
```

### Mirror 4:
```
Frame 1: Rope
Frame 2: Pills
Frame 3: Knife
Frame 4: BloodyTowel
```

---

## 📝 Testing Checklist

### Entry:
- [ ] Lisa enters through broken mirror
- [ ] Blood effects visible
- [ ] Door slams and locks
- [ ] Emily appears
- [ ] Intro dialogue plays

### Puzzles:
- [ ] All 4 mirrors interactable
- [ ] Each puzzle panel shows/hides correctly
- [ ] Items are draggable
- [ ] Timers count down
- [ ] Correct solutions work
- [ ] Success dialogues play
- [ ] Panels hide after success

### Ending:
- [ ] All 4 mirrors complete → Cutscene triggers
- [ ] Emily breakdown sequence
- [ ] All 20 ending dialogues play
- [ ] Emily fades out
- [ ] Screen fades to black
- [ ] Returns to Main Menu

---

## 💡 Key Points

### No Room 10!
- Original plan: Room 10 = Master Bedroom
- New plan: Ending cutscene after Room 09
- Benefit: More cinematic, cleaner flow

### All Scripts Complete!
- Room09_FlowController ✅
- Room09_Dialogues ✅ (including 20 ending dialogues)
- All 4 mirror puzzle scripts ✅
- Room09_Interactable ✅

### What You Need to Do:
1. Create UI panels for each mirror
2. Create sprites/images for items
3. Set up hierarchy correctly (items inside panels!)
4. Assign references in Inspector
5. Test each puzzle
6. Test complete flow

---

## 🎯 Summary

**Room 09 = Final Room**

**Flow**:
1. Entry → Emily manifests
2. Solve 4 mirror puzzles (any order)
3. All complete → Emily breakdown
4. 20-dialogue cutscene (final revelation)
5. Emily fades away
6. Fade to black
7. Main Menu

**Total Time**: 8-13 minutes

**The End!** 🎮✨

---

## 📚 Guide Files Available:

1. **MIRROR1_DEBUG_GUIDE.md** - Mirror 1 setup
2. **MIRROR2_CORRECT_FLOW.md** - Mirror 2 setup
3. **MIRROR3_FRESH_START_SETUP.md** - Mirror 3 setup
4. **MIRROR4_SETUP_TAGALOG.md** - Mirror 4 setup
5. **FINAL_CUTSCENE_GUIDE.md** - Ending cutscene details
6. **ALL_MIRRORS_VISIBILITY_FLOW.md** - Panel visibility guide
7. **FINAL_SETUP_GUIDE_TAGALOG.md** - Complete setup guide

**Good luck!** 🎯
