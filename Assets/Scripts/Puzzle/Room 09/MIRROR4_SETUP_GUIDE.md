# Mirror 4 - Evidence Sequence Setup Guide

## 🎯 Puzzle Overview

**Concept**: Arrange 4 evidence items in correct order showing mother's murder plan progression

**Correct Sequence**: Rope → Pills → Knife → BloodyTowel

**Time Limit**: 90 seconds

**Difficulty**: Medium (logical sequence)

---

## 📦 Assets Needed

### Evidence Items (4):
1. **Rope** - For restraining
2. **Pills** - For sedation
3. **Knife** - Murder weapon
4. **BloodyTowel** - Cleanup

### Flashback Images (4):
1. **Flashback_Rope** - Mother buying rope at hardware store
2. **Flashback_Pills** - Mother crushing pills into powder
3. **Flashback_Knife** - Mother sharpening kitchen knife
4. **Flashback_Towel** - Mother preparing cleanup supplies

### UI Elements:
- Mirror4_Panel (main panel)
- Timer_Text
- 4 Picture Frames (Frame_1 to Frame_4)
- Flashback_Image (shows flashbacks)

---

## 🎮 Unity Hierarchy

```
Canvas
└── Mirror4_Panel
    ├── Background (Image - semi-transparent black)
    ├── Timer_Text (TextMeshPro)
    ├── Flashback_Image (Image - hidden at start)
    ├── Frames_Container
    │   ├── Frame_1 (Image - empty frame)
    │   ├── Frame_2 (Image - empty frame)
    │   ├── Frame_3 (Image - empty frame)
    │   └── Frame_4 (Image - empty frame)
    └── Items_Container
        ├── Rope (Image - draggable)
        ├── Pills (Image - draggable)
        ├── Knife (Image - draggable)
        └── BloodyTowel (Image - draggable)
```

**IMPORTANTE**: All items and frames must be CHILDREN of Mirror4_Panel!

---

## 🔧 Component Setup

### Step 1: Create Mirror 4 GameObject

1. Create empty GameObject: `Mirror_4`
2. Add component: `Mirror4_EvidenceSequence`
3. Add component: `Room09_Interactable`
4. Add component: `BoxCollider2D` (set as trigger)
5. Set `mirrorNumber` to `4` in Room09_Interactable

---

### Step 2: Create UI Panel

1. In Canvas, create: `Mirror4_Panel`
2. Set as child of Canvas
3. Add Image component (semi-transparent black background)
4. Set RectTransform to fill screen or centered
5. **Uncheck the panel** in Inspector (will be shown by script)

---

### Step 3: Add Timer

1. Create TextMeshPro: `Timer_Text`
2. Parent: `Mirror4_Panel`
3. Position: Top center
4. Text: "1:30"
5. Font size: 48
6. Color: White

---

### Step 4: Create Flashback Image

1. Create Image: `Flashback_Image`
2. Parent: `Mirror4_Panel`
3. Position: Center of screen
4. Size: Large (covers most of screen)
5. **Uncheck in Inspector** (hidden at start)
6. Add Image component (will be set by script)

---

### Step 5: Create Picture Frames

1. Create 4 Images: `Frame_1`, `Frame_2`, `Frame_3`, `Frame_4`
2. Parent: `Mirror4_Panel` (or create `Frames_Container` first)
3. Arrange horizontally (left to right)
4. Each frame: Empty frame sprite (border only)
5. Size: 200x200 (or appropriate size)
6. Spacing: Even distribution

---

### Step 6: Create Evidence Items

1. Create 4 Images:
   - `Rope`
   - `Pills`
   - `Knife`
   - `BloodyTowel`

2. Parent: `Mirror4_Panel` (or create `Items_Container` first)

3. Position: Scattered below frames (or in a row)

4. Each item:
   - Add Image component
   - Set sprite (evidence item image)
   - Size: 150x150 (or appropriate)
   - **Name MUST match exactly**: "Rope", "Pills", "Knife", "BloodyTowel"

---

### Step 7: Assign References in Inspector

Select `Mirror_4` GameObject, find `Mirror4_EvidenceSequence` component:

**UI References**:
- Puzzle Panel: Drag `Mirror4_Panel`
- Timer Text: Drag `Timer_Text`
- Picture Frames: Set size to 4
  - Element 0: Drag `Frame_1`
  - Element 1: Drag `Frame_2`
  - Element 2: Drag `Frame_3`
  - Element 3: Drag `Frame_4`
- Evidence Items: Set size to 4
  - Element 0: Drag `Rope`
  - Element 1: Drag `Pills`
  - Element 2: Drag `Knife`
  - Element 3: Drag `BloodyTowel`

**Flashback System**:
- Flashback Image: Drag `Flashback_Image`
- Flashback Sprites: Set size to 4
  - Element 0: Drag `Flashback_Rope` sprite
  - Element 1: Drag `Flashback_Pills` sprite
  - Element 2: Drag `Flashback_Knife` sprite
  - Element 3: Drag `Flashback_Towel` sprite
- Flashback Duration: 2 (seconds)

**Settings**:
- Time Limit: 90
- Snap Distance: 200

**Audio** (optional):
- Item Place Sound
- Flashback Sound
- Success Sound
- Fail Sound

---

## 🎯 Correct Solution

```
Frame 1: Rope (restraint)
Frame 2: Pills (sedation)
Frame 3: Knife (murder weapon)
Frame 4: BloodyTowel (cleanup)
```

**Logic**: This is the order of mother's murder plan:
1. First, restrain the child (Rope)
2. Then, sedate the child (Pills)
3. Then, commit murder (Knife)
4. Finally, clean up evidence (BloodyTowel)

---

## 🎮 How It Works

### Flow:

1. **Player interacts with Mirror 4**
   - Panel shows
   - 4 frames visible (empty)
   - 4 items scattered below
   - Timer starts (90 seconds)

2. **Player drags items to frames**
   - Drag Rope to Frame 1
   - Drag Pills to Frame 2
   - Drag Knife to Frame 3
   - Drag BloodyTowel to Frame 4

3. **Correct placement shows flashback**
   - When item placed in CORRECT frame
   - Flashback image appears (2 seconds)
   - Shows mother's preparation for that step

4. **All correct → Puzzle solved**
   - Success dialogue
   - Panel hides
   - Mirror 4 marked complete

5. **Timeout → Emily attacks**
   - Game Over
   - Scene reloads

---

## 🎨 Visual Feedback

### During Drag:
- Item becomes semi-transparent (60% alpha)
- Item follows cursor/touch

### On Drop:
- If near frame: Snaps to frame center
- If far from frame: Returns to original position

### Correct Placement:
- Flashback image appears
- Flashback sound plays
- Item stays in frame

### Wrong Placement:
- Item stays in frame (can be moved again)
- No flashback shown

### All Correct:
- Success sound plays
- Success dialogue
- Panel closes

---

## 🐛 Troubleshooting

### Issue 1: Items not draggable
**Cause**: Items don't have Image component or are outside panel
**Fix**: 
- Add Image component to each item
- Make sure items are children of Mirror4_Panel

### Issue 2: Items disappear after puzzle
**Cause**: Items are not children of panel
**Fix**: Move all items inside Mirror4_Panel

### Issue 3: Flashbacks don't show
**Cause**: Flashback sprites not assigned or Flashback_Image not set
**Fix**: 
- Assign all 4 flashback sprites in Inspector
- Assign Flashback_Image reference

### Issue 4: Wrong item names
**Cause**: GameObject names don't match script expectations
**Fix**: Rename GameObjects to exactly:
- "Rope"
- "Pills"
- "Knife"
- "BloodyTowel"

### Issue 5: Items snap to wrong frames
**Cause**: Snap distance too large
**Fix**: Reduce Snap Distance in Inspector (try 150 or 100)

---

## 📝 Testing Checklist

- [ ] Panel hidden at start
- [ ] Interact with Mirror 4 → Panel shows
- [ ] 4 frames visible
- [ ] 4 items visible and draggable
- [ ] Timer counts down
- [ ] Drag Rope to Frame 1 → Flashback shows
- [ ] Drag Pills to Frame 2 → Flashback shows
- [ ] Drag Knife to Frame 3 → Flashback shows
- [ ] Drag BloodyTowel to Frame 4 → Flashback shows
- [ ] All correct → Success dialogue
- [ ] Panel hides after success
- [ ] Items hide with panel
- [ ] Timeout → Emily attack → Game Over

---

## 💡 Design Tips

### Item Placement:
- Arrange items in a row below frames
- Or scatter them randomly for more challenge
- Make sure they're visible and accessible

### Frame Spacing:
- Space frames evenly (horizontal layout)
- Leave enough room for items to fit
- Clear visual separation between frames

### Flashback Images:
- Use dramatic, clear images
- Show mother's preparation clearly
- Brief but impactful (2 seconds)

### Difficulty Balance:
- Logical sequence (not random)
- 90 seconds is generous for 4 items
- Flashbacks reward correct placement

---

## 🎯 Summary

**Key Points**:
1. 4 evidence items in logical order
2. Correct placement shows flashback
3. All correct → Puzzle solved
4. Timeout → Emily attack

**Critical Setup**:
- All items and frames INSIDE Mirror4_Panel
- Item names must match exactly
- Flashback sprites assigned
- References assigned in Inspector

**Flow**:
1. Interact → Panel shows
2. Drag items to frames
3. Correct order → Success
4. Panel hides

Yan lang! 🎯
