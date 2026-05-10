# ROOM 09 & 10 - QUICK REFERENCE CARD

## 📋 AT A GLANCE

| | ROOM 09 | ROOM 10 |
|---|---|---|
| **Name** | Master Bedroom's Bathroom | Master Bedroom |
| **Type** | Puzzle Room | Story Room |
| **Time** | 5-7 min | 14-15 min |
| **Difficulty** | Hard | Easy |
| **Puzzles** | 4 timed puzzles | 0 puzzles |
| **Fail State** | Yes (timeout) | No |
| **Scripts Status** | 3/7 complete | 3/3 complete ✅ |
| **Sprites Needed** | ~35 | ~20 |
| **Panels Needed** | 4 | 1 |
| **Audio Needed** | 12 files | 13 files |

---

## 🎮 ROOM 09 QUICK GUIDE

### What Player Does:
1. Enters through broken mirror
2. Solves 4 mirror puzzles (any order)
3. Each puzzle has 60-90 second timer
4. All 4 must be complete
5. Door unlocks → Go to Room 10

### 4 Puzzles:
1. **Medicine Cabinet** (60s) - Order 6 bottles chronologically
2. **Bathtub Drain** (60s) - Reassemble torn note (4 pieces)
3. **Vanity Terror** (90s) - Order 8 diary pages
4. **Evidence Sequence** (60s) - Order 4 murder items

### If Timeout:
- Emily attacks
- Game Over
- Restart puzzle

### Assets Needed:
- 35+ sprites (bottles, notes, pages, items, etc.)
- 4 UI panels (one per puzzle)
- Intense battle music + SFX

### Scripts Needed:
- ✅ Room09_Dialogues.cs
- ✅ Room09_FlowController.cs
- ✅ Mirror1_MedicineCabinet.cs
- ⏳ Mirror2_BathtubDrain.cs
- ⏳ Mirror3_VanityTerror.cs
- ⏳ Mirror4_EvidenceSequence.cs
- ⏳ Room09_Interactable.cs

---

## 🎮 ROOM 10 QUICK GUIDE

### What Player Does:
1. Enters master bedroom
2. Examines bed OR diary
3. Finds music box (Lullaby Fragment #4)
4. Clicks mirror
5. Watches 9-image flashback
6. Reads 60+ dialogues
7. Forgives Emily
8. Game ends

### No Puzzles:
- Just exploration + dialogue
- No time limits
- No fail state
- Story-driven

### Assets Needed:
- 20 sprites (room objects + Emily)
- **9 flashback images** (possession/murder)
- 1 UI panel (flashback display)
- 3 music tracks (tense, lullaby, peaceful)

### Scripts Status:
- ✅ Room10_Dialogues.cs (60+ dialogues)
- ✅ Room10_FlowController.cs (10-phase flow)
- ✅ Room10_Interactable.cs (4 objects)

---

## 🎯 PUZZLE SOLUTIONS (Room 09)

### Mirror 1: Medicine Cabinet
```
1973 → 1974 → 1975 (Valium) → 1975 (Pain) → 1976 (Sleep) → 1976 (Unknown)
```

### Mirror 2: Bathtub Drain
```
"Tonight I" + "end this child's" + "suffering and" + "mine forever"
```

### Mirror 3: Vanity Terror
```
Page 1 → Page 2 → Page 3 → Page 4 → Page 5 → Page 6 → Page 7 → Page 8
(Chronological order of mother's diary)
```

### Mirror 4: Evidence Sequence
```
Rope → Pills → Knife → Bloody Towel
(Order of mother's murder plan)
```

---

## ⏱️ TIMING

### Room 09:
- Entry: 30 sec
- Puzzle 1: 60 sec max
- Puzzle 2: 60 sec max
- Puzzle 3: 90 sec max
- Puzzle 4: 60 sec max
- Navigation: 1-2 min
- Complete: 30 sec
- **Total: 5-7 min**

### Room 10:
- Entry: 1 min
- Emily Blocks: 30 sec
- Exploration: 2-3 min
- Unlock: 1 min
- Approach: 1.5 min
- Flashback: 2 min
- Understanding: 3 min
- Forgiveness: 1 min
- Departure: 1.5 min
- Epilogue: 1.5 min
- **Total: 14-15 min**

---

## 🎵 MUSIC

### Room 09:
- Intense battle music (entire room)
- Success jingles (puzzle solved)
- Failure sound (timeout)

### Room 10:
- Tense music (intro, exploration)
- Lullaby (music box → forgiveness)
- Peaceful music (departure, epilogue)

---

## 🎨 CRITICAL ASSETS

### Room 09 Must-Have:
- [ ] 6 prescription bottle sprites
- [ ] 4 torn note piece sprites
- [ ] 8 diary page sprites
- [ ] 4 evidence item sprites
- [ ] 4 puzzle panel UIs
- [ ] Emily full power sprite
- [ ] Intense battle music

### Room 10 Must-Have:
- [ ] **9 flashback images** (most important!)
- [ ] Emily solid sprite
- [ ] Mirror + glow effect
- [ ] Music box sprite
- [ ] Bed + diary sprites
- [ ] Flashback panel UI
- [ ] 3 music tracks (tense, lullaby, peaceful)

---

## 🔧 SETUP PRIORITY

### Room 09 Priority:
1. Create 4 puzzle panels (UI)
2. Implement drag-and-drop system
3. Create Mirror2, Mirror3, Mirror4 scripts
4. Assign all puzzle sprites
5. Test each puzzle
6. Balance timing

### Room 10 Priority:
1. Create flashback panel (UI)
2. **Create/assign 9 flashback images**
3. Assign all references in FlowController
4. Add Lullaby Fragment #4 to inventory
5. Test full sequence
6. Create ending scene

---

## ✅ COMPLETION STATUS

### Room 09:
- Scripts: 43% complete (3/7)
- Documentation: 100% complete
- Unity Setup: 0%
- Assets: 0%
- **Status: IN PROGRESS**

### Room 10:
- Scripts: 100% complete ✅
- Documentation: 100% complete ✅
- Unity Setup: 0%
- Assets: 0%
- **Status: READY FOR IMPLEMENTATION**

---

## 📞 WHERE TO START

### For Room 09:
1. Read: `ROOM09_ASSETS_AND_FLOW.md`
2. Read: `ROOM09_DESIGNER_FLOW_TAGALOG.md`
3. Create 4 puzzle panels
4. Create remaining scripts

### For Room 10:
1. Read: `ROOM10_ASSETS_AND_FLOW.md`
2. Read: `START_HERE.md`
3. Follow 5-step quick start
4. **Focus on creating 9 flashback images**

---

## 🎯 SUCCESS CRITERIA

### Room 09 Success:
- [ ] All 4 puzzles work
- [ ] Timers work correctly
- [ ] Emily attacks on timeout
- [ ] Door unlocks when all complete
- [ ] Can proceed to Room 10

### Room 10 Success:
- [ ] All 60+ dialogues play
- [ ] All 9 flashback images show
- [ ] Music switches correctly
- [ ] Emily fades smoothly
- [ ] Scene transitions to ending
- [ ] Game completion saved

---

## 🚀 RECOMMENDED APPROACH

### Option 1: Easy First
1. Complete Room 10 (easier, scripts done)
2. Test ending sequence
3. Then tackle Room 09 (harder, needs scripts)

### Option 2: Hard First
1. Complete Room 09 (harder, get it done)
2. Test puzzle systems
3. Then relax with Room 10 (easier)

### Option 3: Parallel
1. One person on Room 09 puzzles
2. Another person on Room 10 assets
3. Combine and test together

---

## 📊 ASSET CREATION PRIORITY

### Room 09 Assets (Priority Order):
1. **HIGH**: 4 puzzle panel UIs
2. **HIGH**: Puzzle item sprites (bottles, notes, pages, evidence)
3. **MEDIUM**: Emily full power sprite
4. **MEDIUM**: Bathroom scene sprites
5. **LOW**: Visual effects

### Room 10 Assets (Priority Order):
1. **CRITICAL**: 9 flashback images (cannot skip!)
2. **HIGH**: Flashback panel UI
3. **HIGH**: 3 music tracks
4. **MEDIUM**: Room scene sprites (bed, diary, music box)
5. **MEDIUM**: Emily solid sprite
6. **LOW**: Visual effects

---

## ⚠️ COMMON PITFALLS

### Room 09 Pitfalls:
- ❌ Timers too short (frustrating)
- ❌ Drag-and-drop not working
- ❌ Puzzle solutions unclear
- ❌ Emily attack too harsh
- ✅ Test with real players!

### Room 10 Pitfalls:
- ❌ Forgetting 9 flashback images
- ❌ Dialogues too long (keep 1-2 sentences)
- ❌ Pacing too fast (let moments breathe)
- ❌ Music not switching
- ✅ This is the ending - make it count!

---

## 🎊 FINAL TIPS

### Room 09 Tips:
- Balance difficulty (not too hard, not too easy)
- Give clear visual feedback
- Make timers visible
- Allow puzzle retry without full restart
- Test with different skill levels

### Room 10 Tips:
- **Focus on emotional impact**
- Don't rush the pacing
- Make flashback images powerful
- Music is crucial for emotion
- Test the full 15-minute sequence
- Ensure satisfying closure

---

## 📁 FILE LOCATIONS

### Room 09 Files:
```
Assets/Scripts/Puzzle/Room 09/
├── ROOM09_ASSETS_AND_FLOW.md
├── ROOM09_COMPLETE_DESIGN.md
├── ROOM09_DESIGNER_FLOW_TAGALOG.md
├── Room09_Dialogues.cs ✅
├── Room09_FlowController.cs ✅
└── Mirror1_MedicineCabinet.cs ✅
```

### Room 10 Files:
```
Assets/Scripts/Puzzle/Room 10/
├── ROOM10_ASSETS_AND_FLOW.md
├── ROOM10_COMPLETE_DESIGN.md
├── ROOM10_DESIGNER_FLOW_TAGALOG.md
├── ROOM10_VISUAL_FLOWCHART.md
├── START_HERE.md
├── README.md
├── Room10_Dialogues.cs ✅
├── Room10_FlowController.cs ✅
└── Room10_Interactable.cs ✅
```

---

## 🎮 COMBINED EXPERIENCE

```
ROOM 09: Intense Puzzle Challenge
         ↓
         Door Unlocks
         ↓
ROOM 10: Emotional Story Climax
         ↓
         Game Complete
         ↓
         Credits/Ending
```

**Total Experience**: 19-22 minutes of gameplay
**Emotional Arc**: Desperation → Revelation → Catharsis → Peace

---

**THESE ARE THE FINAL TWO ROOMS!**
**MAKE THEM UNFORGETTABLE!** 🎮✨

---

**Quick Reference Version**: 1.0  
**Last Updated**: [Current Date]  
**Status**: Ready for Implementation
