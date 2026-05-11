# Room 09 - Final Cutscene Guide

## 🎬 Overview

**WALANG ROOM 10!** After completing all 4 mirror puzzles in Room 09, the game ends with a **20-dialogue cutscene** that reveals the complete truth and concludes the story.

---

## 📋 Complete Flow

### Phase 1: Entry & Intro
1. Lisa enters bathroom through broken mirror
2. Gets cut by glass
3. Door slams shut (locked)
4. Emily manifests at full power
5. Intro dialogue sequence

### Phase 2: 4 Mirror Puzzles (Any Order)
1. Mirror 1: Medicine Cabinet (60s)
2. Mirror 2: Bathtub Drain (60s)
3. Mirror 3: Vanity Terror (90s)
4. Mirror 4: Evidence Sequence (90s)

### Phase 3: All Mirrors Complete
1. All mirrors show complete story
2. Mother's voice echoes
3. Emily's breakdown sequence
4. Emily collapses and becomes translucent

### Phase 4: Final Cutscene (20 Dialogues)
**This is the ending - no Room 10!**

---

## 🎬 Final Cutscene Sequence (20 Dialogues)

### Part 1: Final Realization (Dialogues 1-3)

**ENDING_1**: "All four mirrors... they show the complete truth."

**ENDING_2**: "Mother planned everything. The medications. The note. The timeline. The execution."

**ENDING_3**: "She was going to kill me that night. And herself. A murder-suicide."

---

### Part 2: Understanding Emily (Dialogues 4-6)

**ENDING_4**: "Emily... she saved me. That night, she manifested fully to stop mother."

**ENDING_5**: "But every time she protected me, she absorbed more of mother's rage. Her methods. Her violence."

**ENDING_6**: "'I became what I fought against... to keep you alive.'" *(Emily speaking)*

---

### Part 3: Mother's Plan Revealed (Dialogues 7-9)

**ENDING_7**: "The rope was to restrain me. The pills to sedate me. The knife to... end it."

**ENDING_8**: "Mother saw my defiance as a disease. Emily as a demon. Both needed to be eliminated."

**ENDING_9**: "She couldn't control me anymore. So she decided to end us both."

---

### Part 4: Emily's Sacrifice (Dialogues 10-12)

**ENDING_10**: "'I stopped her that night. But I couldn't save her from herself. She took her own life after I intervened.'" *(Emily speaking)*

**ENDING_11**: "Emily saved me... but at the cost of becoming a monster herself."

**ENDING_12**: "'Every scar you carry... I put there trying to protect you the only way I learned how.'" *(Emily speaking)*

---

### Part 5: Forgiveness (Dialogues 13-15)

**ENDING_13**: "You were never the monster, Emily. You were a child too. Trying to save another child."

**ENDING_14**: "Mother's violence... it infected us both. But you fought it. You tried to break the cycle."

**ENDING_15**: "'Thank you... for finally understanding. For finally letting me rest.'" *(Emily speaking)*

---

### Part 6: Emily Fades Away (Dialogues 16-18)

**ENDING_16**: "Emily's form... it's fading. Becoming light. Peaceful."

*(Emily sprite fades out completely over 3 seconds)*

**ENDING_17**: "She's smiling. For the first time, she looks... free."

**ENDING_18**: "The bathroom is quiet now. The water still. The mirrors dark."

---

### Part 7: Final Words (Dialogues 19-20)

**ENDING_19**: "I understand now. The echoes of her lullaby weren't a threat. They were a cry for help."

**ENDING_20**: "Rest now, Emily. You've protected me long enough. We're both free now."

---

### Part 8: Fade to Black & Credits

1. Screen fades to black (2 seconds)
2. Save game completion flag
3. Return to Main Menu

---

## 🎨 Visual Effects During Cutscene

### Emily's Fade Out (During Dialogue 16-18):
```csharp
// Emily becomes translucent (alpha 0.2) after breakdown
// Then fades completely (alpha 0.0) during ENDING_16-17
// Takes 3 seconds to fade out completely
```

### Screen Fade (After Dialogue 20):
```csharp
// Fade to black over 2 seconds
// Wait 1 second
// Load Main Menu scene
```

---

## 🔊 Audio During Cutscene

### Background Music:
- Tense music fades out during Emily's breakdown
- Soft, emotional music during forgiveness (optional)
- Silence during final words
- Peaceful music during fade out (optional)

### Sound Effects:
- Emily's whisper (soft, fading)
- Water sounds fade out
- Peaceful ambience during ending

---

## 💾 Save System

### Game Completion Flag:
```csharp
SaveSystem.Instance?.MarkPuzzleSolved("game_complete");
```

This marks the game as completed and can unlock:
- Credits
- New Game+
- Bonus content
- Achievements

---

## 🎮 Implementation Details

### Room09_FlowController.cs:

**Key Methods**:
1. `AllMirrorsCompleteSequence()` - Triggered when all 4 mirrors solved
2. `EmilyBreakdownSequence()` - Emily's collapse
3. `EndingCutsceneSequence()` - 20 dialogue ending

**Flow**:
```
OnMirrorComplete(4) 
  → AreAllMirrorsComplete() returns true
  → AllMirrorsCompleteSequence()
  → EmilyBreakdownSequence()
  → EndingCutsceneSequence() (20 dialogues)
  → Fade to black
  → Load Main Menu
```

---

## 📝 Dialogue Timing

### Estimated Duration:
- **Part 1** (1-3): ~15 seconds
- **Part 2** (4-6): ~20 seconds
- **Part 3** (7-9): ~20 seconds
- **Part 4** (10-12): ~25 seconds
- **Part 5** (13-15): ~20 seconds
- **Part 6** (16-18): ~25 seconds (includes fade)
- **Part 7** (19-20): ~15 seconds
- **Fade out**: ~3 seconds

**Total**: ~2.5-3 minutes

---

## 🎯 Key Story Revelations

### What the Player Learns:

1. **Mother's Plan**: Murder-suicide to "end the child's defiance"
2. **Emily's Origin**: Manifested to protect Lisa from mother
3. **Emily's Transformation**: Absorbed mother's violence while protecting Lisa
4. **The Truth**: Emily saved Lisa but became a monster in the process
5. **Resolution**: Lisa forgives Emily, Emily finds peace and fades away
6. **Freedom**: Both Lisa and Emily are finally free from the cycle of violence

---

## 🐛 Testing Checklist

- [ ] Complete all 4 mirror puzzles
- [ ] All mirrors complete → Cutscene triggers automatically
- [ ] Emily breakdown sequence plays
- [ ] All 20 ending dialogues play in order
- [ ] Emily sprite fades out during dialogues 16-17
- [ ] Screen fades to black after dialogue 20
- [ ] Game completion flag saved
- [ ] Returns to Main Menu
- [ ] No errors in console

---

## 💡 Design Notes

### Why No Room 10?

**Original Plan**: Room 10 was the Master Bedroom with final revelation

**New Plan**: All revelation happens in cutscene after Room 09 puzzles

**Benefits**:
- More cinematic ending
- No need for additional scene/assets
- Cleaner narrative flow
- Player doesn't need to explore another room
- Focuses on emotional resolution

### Emotional Arc:

1. **Horror** → Solving puzzles under pressure
2. **Revelation** → Understanding the truth
3. **Empathy** → Seeing Emily as victim too
4. **Forgiveness** → Breaking the cycle
5. **Peace** → Both characters find freedom

---

## 🎬 Cutscene Script Summary

```
[All 4 mirrors complete]

LISA: All four mirrors... they show the complete truth.
LISA: Mother planned everything. The medications. The note. The timeline. The execution.
LISA: She was going to kill me that night. And herself. A murder-suicide.

[Pause]

LISA: Emily... she saved me. That night, she manifested fully to stop mother.
LISA: But every time she protected me, she absorbed more of mother's rage. Her methods. Her violence.
EMILY: I became what I fought against... to keep you alive.

[Pause]

LISA: The rope was to restrain me. The pills to sedate me. The knife to... end it.
LISA: Mother saw my defiance as a disease. Emily as a demon. Both needed to be eliminated.
LISA: She couldn't control me anymore. So she decided to end us both.

[Pause]

EMILY: I stopped her that night. But I couldn't save her from herself. She took her own life after I intervened.
LISA: Emily saved me... but at the cost of becoming a monster herself.
EMILY: Every scar you carry... I put there trying to protect you the only way I learned how.

[Pause]

LISA: You were never the monster, Emily. You were a child too. Trying to save another child.
LISA: Mother's violence... it infected us both. But you fought it. You tried to break the cycle.
EMILY: Thank you... for finally understanding. For finally letting me rest.

[Emily begins to fade]

LISA: Emily's form... it's fading. Becoming light. Peaceful.
LISA: She's smiling. For the first time, she looks... free.
LISA: The bathroom is quiet now. The water still. The mirrors dark.

[Long pause]

LISA: I understand now. The echoes of her lullaby weren't a threat. They were a cry for help.
LISA: Rest now, Emily. You've protected me long enough. We're both free now.

[Fade to black]
[Return to Main Menu]
```

---

## 🎯 Summary

**No Room 10 needed!**

After completing all 4 mirror puzzles in Room 09:
1. Emily breaks down
2. 20-dialogue cutscene plays
3. Emily fades away peacefully
4. Screen fades to black
5. Game complete → Main Menu

**Total playtime**: Room 09 = 5-10 minutes (puzzles) + 3 minutes (cutscene) = **8-13 minutes**

**The End!** 🎮✨
