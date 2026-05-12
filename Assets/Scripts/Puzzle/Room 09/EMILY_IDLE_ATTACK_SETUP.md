# Room 09 - Emily Idle & Attack Setup (Tagalog)

## 🎯 New Flow

### Emily's Behavior:

1. **Start** → Emily idle sa gitna ng room (visible, terrifying)
2. **During puzzles** → Emily just stands there, watching
3. **After each puzzle** → Emily attacks! → Black screen → Narration
4. **After 4th puzzle** → Final ending cutscene → Main Menu

---

## 🔧 Unity Setup

### Step 1: Create Emily Idle Position

1. Create empty GameObject: `EmilyIdlePosition`
2. Position: **Center ng bathroom** (example: `0, 0, 0`)
3. This is where Emily will stand the whole time

---

### Step 2: Setup Room09_FlowController

Select `Room09_FlowController` GameObject:

**Emily State**:
- **Emily Manifestation**: Drag Emily GameObject
- **Emily Idle Position**: Drag `EmilyIdlePosition` GameObject

---

## 🎮 How It Works

### Flow for Each Puzzle:

1. **Player solves puzzle** (Mirror 1, 2, 3, or 4)
2. **Success dialogue** plays
3. **Emily attacks!** → "You solved one... but you're still trapped here with me!"
4. **Fade to black** (1 second)
5. **Black screen narration** → Lisa reflects on what she learned
6. **Fade back in** (1 second)
7. **Player can move again** → Continue to next puzzle

### After 4th Puzzle:

1. **All 4 mirrors complete**
2. **Final sequence** → Emily breakdown
3. **20-dialogue ending cutscene**
4. **Fade to black**
5. **Main Menu**

---

## 📝 Narration for Each Mirror

### Mirror 1 (Medicine Cabinet):
```
"The medicine cabinet... Mother was planning this for years. 
Increasing dosages, preparing..."
```

### Mirror 2 (Bathtub Drain):
```
"The torn note... 'Tonight I end this child's suffering and mine - forever.' 
A murder-suicide plan."
```

### Mirror 3 (Vanity Terror):
```
"Mother's diary... Her descent into madness. 
Emily protecting me. The final plan."
```

### Mirror 4 (Evidence Sequence):
```
"The evidence... Rope, pills, knife, towel. 
Every step of her plan laid out."
```

---

## 🎬 Visual Flow

```
[Start]
   ↓
Emily idle sa gitna (0, 0, 0)
   ↓
Player solves Mirror 1
   ↓
Emily: "You solved one..."
   ↓
[BLACK SCREEN]
Narration: "The medicine cabinet..."
   ↓
[FADE IN]
   ↓
Player solves Mirror 2
   ↓
Emily: "You solved one..."
   ↓
[BLACK SCREEN]
Narration: "The torn note..."
   ↓
[FADE IN]
   ↓
Player solves Mirror 3
   ↓
Emily: "You solved one..."
   ↓
[BLACK SCREEN]
Narration: "Mother's diary..."
   ↓
[FADE IN]
   ↓
Player solves Mirror 4
   ↓
[ALL COMPLETE]
   ↓
Final ending cutscene (20 dialogues)
   ↓
[FADE TO BLACK]
   ↓
Main Menu
```

---

## 🎯 Inspector Setup

### Room09_FlowController:

**Emily State**:
- Emily Manifestation: `Emily_Manifestation` GameObject
- Emily Idle Position: `EmilyIdlePosition` GameObject (center of room)

**Example Positions**:
```
EmilyIdlePosition: (0, 0, 0) ← Center
PlayerSpawnPoint: (-3, -2, 0) ← Left side
```

---

## 🎨 Scene Layout

```
Room 09 Bathroom:

        [Mirror 1]    [Mirror 2]
              ↓           ↓
    [Player] → [EMILY] ← (idle, center)
              ↓           ↓
        [Mirror 3]    [Mirror 4]
```

Emily stands in the center, visible the whole time, watching Lisa solve puzzles.

---

## 📝 Testing Checklist

### Test 1: Emily Position
- [ ] Emily spawns at idle position (center)
- [ ] Emily is visible
- [ ] Emily doesn't move

### Test 2: First Puzzle
- [ ] Solve Mirror 1
- [ ] Success dialogue plays
- [ ] Emily attack dialogue
- [ ] Fade to black
- [ ] Narration plays (medicine cabinet)
- [ ] Fade back in
- [ ] Player can move

### Test 3: Subsequent Puzzles
- [ ] Solve Mirror 2 → Attack → Narration → Continue
- [ ] Solve Mirror 3 → Attack → Narration → Continue
- [ ] Solve Mirror 4 → Final sequence

### Test 4: Final Sequence
- [ ] All 4 mirrors complete
- [ ] Emily breakdown
- [ ] 20-dialogue ending
- [ ] Fade to black
- [ ] Main Menu

---

## 💡 Tips

### Emily Idle Position:
- **Center of room** - So visible from all angles
- **Floor level** - Not floating
- **Clear space** - Not blocking puzzles

### Timing:
- **Fade out**: 1 second (quick)
- **Narration**: Player reads at their pace
- **Fade in**: 1 second (quick)

### Narration:
- **Short and impactful** - 1-2 sentences
- **Reflects puzzle content** - What Lisa learned
- **Builds tension** - Each puzzle reveals more

---

## 🎯 Summary

**Emily's Role**:
1. **Idle** - Stands in center, watching
2. **Attack** - After each puzzle success
3. **Narration** - Black screen reflection
4. **Final** - Breakdown after 4th puzzle

**Setup**:
1. Create `EmilyIdlePosition` (center of room)
2. Assign to `Room09_FlowController`
3. Done!

**Flow**:
- Puzzle → Attack → Black screen → Narration → Continue
- Repeat 4 times
- Final sequence → Ending

Yan lang! 🎯✨
