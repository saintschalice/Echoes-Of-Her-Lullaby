# Mirror 4 - Evidence Sequence (Tagalog Guide)

## 🎯 Ano ang Puzzle?

**Concept**: I-arrange ang 4 evidence items sa tamang order para ipakita ang murder plan ng mother

**Tamang Order**: Rope → Pills → Knife → BloodyTowel

**Time Limit**: 90 seconds

---

## 📦 Kailangan Mong Assets

### 4 Evidence Items:
1. **Rope** (lubid) - Para i-restrain
2. **Pills** (gamot) - Para i-sedate
3. **Knife** (kutsilyo) - Murder weapon
4. **BloodyTowel** (tuwalyang may dugo) - Para mag-cleanup

### 4 Flashback Images:
1. **Flashback_Rope** - Mother bumibili ng lubid
2. **Flashback_Pills** - Mother ginigiling ang pills
3. **Flashback_Knife** - Mother tinatasa ang kutsilyo
4. **Flashback_Towel** - Mother naghahanda ng cleanup supplies

---

## 🎮 Paano Gumawa ng Hierarchy

```
Canvas
└── Mirror4_Panel
    ├── Timer_Text
    ├── Flashback_Image (naka-hide at start)
    ├── Frame_1 (empty frame)
    ├── Frame_2 (empty frame)
    ├── Frame_3 (empty frame)
    ├── Frame_4 (empty frame)
    ├── Rope (draggable)
    ├── Pills (draggable)
    ├── Knife (draggable)
    └── BloodyTowel (draggable)
```

**IMPORTANTE**: Lahat ng items at frames ay dapat nasa LOOB ng Mirror4_Panel!

---

## 🔧 Step-by-Step Setup

### Step 1: Gumawa ng Mirror 4 GameObject

1. Create empty GameObject: `Mirror_4`
2. Add component: `Mirror4_EvidenceSequence`
3. Add component: `Room09_Interactable`
4. Add component: `BoxCollider2D` (check "Is Trigger")
5. Sa Room09_Interactable, set `mirrorNumber` = `4`

---

### Step 2: Gumawa ng Panel

1. Sa Canvas, create GameObject: `Mirror4_Panel`
2. Add Image component (itim na background, semi-transparent)
3. I-resize para sakupin ang screen
4. **Uncheck yung panel** sa Inspector (script ang mag-show)

---

### Step 3: Gumawa ng Timer

1. Create TextMeshPro: `Timer_Text`
2. Parent: `Mirror4_Panel`
3. Position: Top center
4. Text: "1:30"
5. Font size: 48

---

### Step 4: Gumawa ng Flashback Image

1. Create Image: `Flashback_Image`
2. Parent: `Mirror4_Panel`
3. Position: Center ng screen
4. Size: Malaki (halos buong screen)
5. **Uncheck sa Inspector** (hidden at start)

---

### Step 5: Gumawa ng 4 Picture Frames

1. Create 4 Images:
   - `Frame_1`
   - `Frame_2`
   - `Frame_3`
   - `Frame_4`

2. Parent: `Mirror4_Panel`

3. I-arrange horizontally (left to right)

4. Bawat frame:
   - Empty frame sprite (border lang)
   - Size: 200x200

---

### Step 6: Gumawa ng 4 Evidence Items

1. Create 4 Images:
   - `Rope`
   - `Pills`
   - `Knife`
   - `BloodyTowel`

2. Parent: `Mirror4_Panel`

3. Position: Scattered sa baba ng frames

4. Bawat item:
   - Add Image component
   - Set sprite (larawan ng item)
   - Size: 150x150
   - **IMPORTANTE**: Name dapat EXACTLY ganito:
     - "Rope" (hindi "rope" o "Rope_Item")
     - "Pills" (hindi "pills" o "Pills_Item")
     - "Knife" (hindi "knife" o "Knife_Item")
     - "BloodyTowel" (hindi "bloodytowel" o "Bloody_Towel")

---

### Step 7: I-assign ang References

Select `Mirror_4` GameObject, hanapin ang `Mirror4_EvidenceSequence` component:

**UI References**:
- **Puzzle Panel**: I-drag ang `Mirror4_Panel`
- **Timer Text**: I-drag ang `Timer_Text`
- **Picture Frames**: Set size to 4
  - Element 0: I-drag ang `Frame_1`
  - Element 1: I-drag ang `Frame_2`
  - Element 2: I-drag ang `Frame_3`
  - Element 3: I-drag ang `Frame_4`
- **Evidence Items**: Set size to 4
  - Element 0: I-drag ang `Rope`
  - Element 1: I-drag ang `Pills`
  - Element 2: I-drag ang `Knife`
  - Element 3: I-drag ang `BloodyTowel`

**Flashback System**:
- **Flashback Image**: I-drag ang `Flashback_Image`
- **Flashback Sprites**: Set size to 4
  - Element 0: I-drag ang `Flashback_Rope` sprite
  - Element 1: I-drag ang `Flashback_Pills` sprite
  - Element 2: I-drag ang `Flashback_Knife` sprite
  - Element 3: I-drag ang `Flashback_Towel` sprite
- **Flashback Duration**: 2

**Settings**:
- **Time Limit**: 90
- **Snap Distance**: 200

**Audio** (optional):
- Item Place Sound
- Flashback Sound
- Success Sound
- Fail Sound

---

## 🎯 Tamang Sagot

```
Frame 1: Rope (i-restrain ang bata)
Frame 2: Pills (i-sedate ang bata)
Frame 3: Knife (murder weapon)
Frame 4: BloodyTowel (mag-cleanup)
```

**Logic**: Ito ang order ng murder plan ng mother:
1. Una, i-restrain ang bata (Rope)
2. Tapos, i-sedate ang bata (Pills)
3. Tapos, patayin (Knife)
4. Finally, mag-cleanup ng evidence (BloodyTowel)

---

## 🎮 Paano Gumagana

### Flow:

1. **Pag-interact sa Mirror 4**
   - Lumabas ang panel
   - 4 empty frames
   - 4 items scattered
   - Timer starts (90 seconds)

2. **I-drag ang items sa frames**
   - Drag Rope to Frame 1
   - Drag Pills to Frame 2
   - Drag Knife to Frame 3
   - Drag BloodyTowel to Frame 4

3. **Pag tama ang placement**
   - Lumabas ang flashback (2 seconds)
   - Ipapakita kung paano nag-prepare ang mother

4. **Pag lahat tama**
   - Success dialogue
   - Panel nawawala
   - Mirror 4 complete

5. **Pag timeout**
   - Emily attacks
   - Game Over

---

## 🎨 Visual Feedback

### Habang nag-drag:
- Item becomes semi-transparent
- Item sumusunod sa cursor

### Pag nag-drop:
- Kung malapit sa frame: Snap sa center ng frame
- Kung malayo: Bumalik sa original position

### Pag tama ang placement:
- Lumabas ang flashback
- May sound effect
- Item stays sa frame

### Pag mali ang placement:
- Item stays sa frame (pwede pa i-move)
- Walang flashback

### Pag lahat tama:
- Success sound
- Success dialogue
- Panel closes

---

## 🐛 Common Problems

### Problem 1: Hindi ma-drag ang items
**Dahilan**: Walang Image component o nasa labas ng panel
**Fix**: 
- Add Image component sa bawat item
- Siguraduhing nasa loob ng Mirror4_Panel ang items

### Problem 2: Nawawala ang items after puzzle
**Dahilan**: Items ay nasa labas ng panel
**Fix**: Ilipat ang lahat ng items sa loob ng Mirror4_Panel

### Problem 3: Walang lumalabas na flashbacks
**Dahilan**: Walang assigned na flashback sprites
**Fix**: 
- I-assign ang 4 flashback sprites sa Inspector
- I-assign ang Flashback_Image reference

### Problem 4: Mali ang item names
**Dahilan**: GameObject names ay hindi exact match
**Fix**: I-rename ang GameObjects to EXACTLY:
- "Rope"
- "Pills"
- "Knife"
- "BloodyTowel"

---

## 📝 Testing Checklist

- [ ] Panel hidden at start
- [ ] Interact → Panel shows
- [ ] 4 frames visible
- [ ] 4 items visible at draggable
- [ ] Timer counts down
- [ ] Drag Rope to Frame 1 → Flashback shows
- [ ] Drag Pills to Frame 2 → Flashback shows
- [ ] Drag Knife to Frame 3 → Flashback shows
- [ ] Drag BloodyTowel to Frame 4 → Flashback shows
- [ ] All correct → Success
- [ ] Panel hides
- [ ] Items hide with panel
- [ ] Timeout → Emily attack

---

## 💡 Tips

### Item Placement:
- I-arrange ang items sa isang row sa baba ng frames
- O i-scatter randomly para mas challenging

### Frame Spacing:
- I-space evenly ang frames
- Siguraduhing may space para sa items

### Flashback Images:
- Gumamit ng clear, dramatic images
- Brief lang (2 seconds)
- Ipakita clearly ang preparation ng mother

---

## 🎯 Summary

**Key Points**:
1. 4 evidence items sa logical order
2. Correct placement = flashback
3. All correct = puzzle solved
4. Timeout = Emily attack

**Critical Setup**:
- All items at frames INSIDE Mirror4_Panel
- Item names must match EXACTLY
- Flashback sprites assigned
- All references assigned

**Flow**:
1. Interact → Panel shows
2. Drag items to frames
3. Correct order → Success
4. Panel hides

Yan lang! Good luck! 🎯
