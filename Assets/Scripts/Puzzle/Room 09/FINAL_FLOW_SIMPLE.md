# Room 09 - Final Flow (Simple Version)

## 🎯 Complete Flow

### 1. Entry
- Lisa enters Room 09
- Emily spawns at idle position (center)
- Emily just stands there (visible, terrifying)
- Intro dialogue

### 2. Puzzles (1-4)
- Player solves Mirror 1 → Success dialogue → Continue
- Player solves Mirror 2 → Success dialogue → Continue
- Player solves Mirror 3 → Success dialogue → Continue
- Player solves Mirror 4 → Success dialogue → **EMILY ATTACKS!**

### 3. Emily Attack (After 4th Puzzle)
- **0.3 seconds pause** (build tension)
- **JUMPSCARE!** Emily screams
- **Quick dialogue**: "NO! You can't know the truth!"
- **Fade to black** (0.5 seconds - fast!)

### 4. Ending Cutscene (Black Screen)
- 20 dialogues revealing complete truth
- Emily's story
- Lisa's understanding
- Forgiveness
- Peace

### 5. The End
- Fade to black
- Return to Main Menu

---

## 🎮 Key Points

### Emily's Behavior:
- **Idle the whole time** - Just standing, watching
- **No attacks during puzzles** - Only after 4th puzzle
- **Sudden jumpscare** - Magugulat ang player!

### Timing:
- **Puzzles 1-3**: Normal flow, no interruption
- **Puzzle 4**: Success → 0.3s pause → ATTACK!
- **Attack**: Quick and shocking (0.5s scream + dialogue)
- **Fade**: Fast (0.5s) to black screen
- **Ending**: 20 dialogues on black screen

---

## 🔧 Unity Setup

### Required GameObjects:

1. **EmilyIdlePosition** (empty GameObject)
   - Position: Center of room (0, 0, 0)

2. **Emily** (sprite from prefab)
   - Will be positioned at idle spot

3. **Room09_FlowController**
   - Emily Manifestation: Emily GameObject
   - Emily Idle Position: EmilyIdlePosition
   - Emily Scream Clip: Scream sound effect

---

## 🎬 Visual Flow

```
[Start]
   ↓
Emily idle sa gitna
   ↓
Puzzle 1 → Success → Continue
   ↓
Puzzle 2 → Success → Continue
   ↓
Puzzle 3 → Success → Continue
   ↓
Puzzle 4 → Success
   ↓
[0.3s pause]
   ↓
EMILY ATTACKS! (Jumpscare)
   ↓
"NO! You can't know the truth!"
   ↓
[Fade to black - 0.5s]
   ↓
[BLACK SCREEN]
20 Ending Dialogues
   ↓
Main Menu
```

---

## 🎯 Inspector Setup

### Room09_FlowController:

**Emily State**:
- Emily Manifestation: Emily GameObject (from Hierarchy)
- Emily Idle Position: EmilyIdlePosition GameObject
- Emily Scream Clip: Scream sound effect

**Audio**:
- Ambient Audio: AudioSource
- Tense Music Clip: Background music
- Emily Scream Clip: Jumpscare sound

**Scene Transition**:
- Main Menu Scene Name: "MainMenu"

---

## 📝 Testing Checklist

- [ ] Emily spawns at idle position
- [ ] Emily visible the whole time
- [ ] Solve Puzzle 1 → Continue (no attack)
- [ ] Solve Puzzle 2 → Continue (no attack)
- [ ] Solve Puzzle 3 → Continue (no attack)
- [ ] Solve Puzzle 4 → **EMILY ATTACKS!**
- [ ] Jumpscare plays (scream sound)
- [ ] Quick dialogue
- [ ] Fast fade to black
- [ ] 20 ending dialogues play
- [ ] Return to Main Menu

---

## 💡 Tips

### Jumpscare Effect:
- **Sound**: Loud scream (sudden)
- **Visual**: Screen shake or flash (optional)
- **Timing**: 0.3s pause before attack (build tension)

### Fade Speed:
- **Fast fade** (0.5s) - Shocking, sudden
- Not slow fade (1-2s) - Too slow, loses impact

### Emily Position:
- **Center of room** - Visible from all angles
- **Always visible** - Constant psychological pressure
- **Static** - More unsettling than moving

---

## 🎯 Summary

**Simple Flow**:
1. Emily idle (whole game)
2. Solve 4 puzzles (no interruption)
3. After 4th puzzle → ATTACK! (jumpscare)
4. Black screen → 20 dialogues → Ending

**No attacks between puzzles!** Only after all 4 complete.

**Fast and shocking!** Magugulat ang player! 🎯✨
