# Room 07 - EXACT Sequence Guide

## 🎯 Complete Flow (IN ORDER)

This is the EXACT sequence that must be followed.

---

## Phase 1: Introduction

### 1. Enter Room (Automatic)
```
Trigger: Lisa enters room
Dialogue: "This room... I know exactly where everything should be. But how? I've never been here before... have I?"
Result: isIntroDone = true
```

---

## Phase 2: Environmental Discovery (Can be any order, but all required)

### 2. Bed Interaction
```
Interact: Bed
Dialogue: "Child's bed has two pillow indentations. Note pinned to second pillow: 'For my friend Emily - she keeps me safe at night.'"
Result: hasCheckedBed = true
```

### 3. Wall Drawings
```
Interact: Wall
Action: Preview wall drawings image
Dialogue: "Crayon drawings show two figures - one labeled 'Me' and another labeled 'Emily'. They're holding hands, playing together."
Result: hasCheckedWall = true
```

### 4. Bookshelf/Diary Discovery
```
Interact: Bookshelf
Dialogue 1: "I found a child's diary on the nightstand..."
Dialogue 2: "Child's diary: 'Emily came to me again last night. She sang the pretty song and made the scary dreams go away.'"
Result: hasCheckedDiary = true
```

---

## Phase 3: Puzzle Sequence (MUST be in order)

### 5. Window Curtains Puzzle
```
Interact: Window Curtains
Dialogue: "Curtains tied shut with child's knots. Note: 'Emily says tie them tight to keep bad things out.'"
Action: Curtain Panel opens
Task: Open BOTH left and right curtains
Result: areCurtainsOpened = true
Note: "Emily's favorite spot. Emily is always looking at this window."
```

### 6. Small Cabinet - Get Emily's Cup
```
Interact: Small Cabinet
Condition: areCurtainsOpened must be true
Action: Cabinet panel opens showing cup
Task: Click cup image to take it
Dialogue: "Found Emily's Cup. This must be Emily's special cup."
Notification: Emily's Cup added to inventory
Result: hasEmilyCup = true
```

### 7. Tea Party Puzzle
```
Interact: Tea Party spot
Condition: Must have emily_cup in inventory
Dialogue: "Three cups set on floor, one marked 'Emily's Special Cup.' Child had daily tea parties with her invisible friend."
Action: Tea Party Panel opens
Task: Drag Emily's Cup to the tea party panel
Result: Tea party complete
Cutscene: Lisa regains memory of having tea party with Emily
Result: isTeaPartyDone = true
```

---

## Phase 4: More Environmental Discovery

### 8. Emily's Chair
```
Interact: Chair
Dialogue: "Small chair marked 'Emily's Chair - Do Not Sit.' Always cold to touch, the strongest presence felt here."
Result: hasCheckedChair = true
```

### 9. Closet
```
Interact: Closet
Dialogue: "Scratches inside closet show child hid here often. Emily's presence is overwhelming - she protected Lisa here."
Result: hasCheckedCloset = true
```

---

## Phase 5: Toybox Puzzle Sequence

### 10. Toybox - First Interaction
```
Interact: Toybox
Dialogue: "The toybox is locked with a puzzle. I should tap the icon in the middle to open it."
Result: isToyboxOpened = true
```

### 11. Toybox Icon - Second Interaction
```
Interact: Icon in middle of toybox
Action: 8-tile sliding puzzle panel opens
Task: Solve sliding puzzle (swipe controls)
Result: isToyboxSolved = true
Dialogue: "The lock clicked. Something opened inside the toybox."
```

### 12. Toybox - Third Interaction (Get Doll)
```
Interact: Toybox again
Condition: isToyboxSolved must be true
Dialogue: "Hidden compartment! Letters: 'Dear Emily, thank you for making mommy stop hurting me yesterday.'"
Notification: Emily Doll added to inventory
Cutscene: Lisa's memory with Emily's doll
Result: hasEmilyDoll = true
```

---

## Phase 6: Dollhouse Puzzle

### 13. Dollhouse Puzzle
```
Interact: Dollhouse
Condition: Must have emily_doll in inventory
Action: Dollhouse Panel opens
Task: Drag Emily Doll to dollhouse
Dialogue: "Family figures removed except child doll. Extra doll added labeled 'Emily' - the invisible friend was very real to this child."
Result: isDollhouseDone = true
```

---

## Phase 7: Final Environmental Check

### 14. Reading Table
```
Interact: Table beside mirror
Dialogue: "Fairy tale books with notes: 'Emily likes the stories where the princess gets saved.'"
Result: hasCheckedReadingTable = true
```

---

## Phase 8: Climax & Chase

### 15. Mirror Interaction (FINAL)
```
Interact: Mirror
Condition: EVERYTHING must be checked/completed:
  ✓ hasCheckedBed
  ✓ hasCheckedWall
  ✓ hasCheckedDiary
  ✓ hasCheckedChair
  ✓ hasCheckedCloset
  ✓ hasCheckedReadingTable
  ✓ areCurtainsOpened
  ✓ isTeaPartyDone
  ✓ isToyboxSolved
  ✓ isDollhouseDone

If NOT complete:
  Dialogue: "I feel like I'm still missing something... I should check everything in this room first."

If ALL complete:
  Cutscene: Emily appears standing behind Lisa in mirror
  Audio: Lullaby Fragment #3 plays from music box
  Memory: Someone tucking young Lisa into bed, singing softly
  Dialogue: "That lullaby..."
  Action: Bedroom door locks
  Dialogue: "The door is locked! I need to get to the bathroom!"
  Chase: Emily becomes aggressive, chases at terrifying speed
  Goal: Run to bathroom to escape
```

---

## 📊 Flow Diagram

```
ENTER ROOM
  ↓
Intro Dialogue
  ↓
┌─────────────────────────────────┐
│ Environmental Discovery         │
│ (Any order, all required)       │
│                                 │
│ • Bed                           │
│ • Wall Drawings                 │
│ • Bookshelf/Diary               │
│ • Chair                         │
│ • Closet                        │
│ • Reading Table                 │
└─────────────────────────────────┘
  ↓
┌─────────────────────────────────┐
│ Puzzle Sequence                 │
│ (MUST be in order)              │
│                                 │
│ 1. Window Curtains              │
│    ↓                            │
│ 2. Get Emily's Cup              │
│    ↓                            │
│ 3. Tea Party Puzzle             │
│    ↓                            │
│ 4. Toybox (tap 1)               │
│    ↓                            │
│ 5. Toybox Icon (tap 2 - puzzle)│
│    ↓                            │
│ 6. Toybox (tap 3 - get doll)   │
│    ↓                            │
│ 7. Dollhouse Puzzle             │
└─────────────────────────────────┘
  ↓
ALL COMPLETE?
  ↓ YES
Mirror Interaction
  ↓
JUMPSCARE & CHASE
  ↓
Escape to Bathroom
```

---

## 🔒 Dependencies

### Curtains → Cabinet:
```
areCurtainsOpened = true
  → Can access Small Cabinet
  → Get Emily's Cup
```

### Cup → Tea Party:
```
hasEmilyCup = true
  → Can do Tea Party Puzzle
```

### Toybox Sequence:
```
First tap → isToyboxOpened = true
  → Can tap icon
Second tap (icon) → Opens puzzle
  → Solve puzzle → isToyboxSolved = true
Third tap → Get doll
  → hasEmilyDoll = true
```

### Doll → Dollhouse:
```
hasEmilyDoll = true
  → Can do Dollhouse Puzzle
```

### Everything → Mirror:
```
All environmental checks = true
All puzzles = true
  → Mirror triggers climax
```

---

## ✅ Completion Checklist

### Environmental (6 items):
- [ ] Bed checked
- [ ] Wall Drawings checked
- [ ] Diary checked (from bookshelf)
- [ ] Chair checked
- [ ] Closet checked
- [ ] Reading Table checked

### Puzzles (4 items):
- [ ] Curtains opened
- [ ] Tea Party completed
- [ ] Toybox puzzle solved
- [ ] Dollhouse completed

### Items Obtained (2 items):
- [ ] Emily's Cup
- [ ] Emily Doll

### Final:
- [ ] Mirror interaction triggers climax
- [ ] Chase sequence starts
- [ ] Can escape to bathroom

---

## 🎮 GameObject Setup

### Interactable Objects Needed:

1. **Bed** (ObjectType.Bed)
2. **Wall** (ObjectType.WallDrawings)
3. **Bookshelf** (ObjectType.Bookshelf)
4. **Window Curtains** (ObjectType.WindowCurtains)
5. **Small Cabinet** (ObjectType.Cabinet_Cup)
6. **Tea Party Spot** (ObjectType.TeaParty)
7. **Chair** (ObjectType.Chair)
8. **Closet** (ObjectType.Closet)
9. **Toybox** (ObjectType.Toybox)
10. **Toybox Icon** (ObjectType.ToyboxIcon) - Separate object!
11. **Dollhouse** (ObjectType.Dollhouse)
12. **Reading Table** (ObjectType.ReadingTable)
13. **Mirror** (ObjectType.Mirror)

---

## 🐛 Common Issues

### Issue 1: Mirror Won't Trigger
```
Problem: Mirror shows "missing something" message

Check:
1. All 6 environmental objects checked?
2. All 4 puzzles completed?
3. Check FlowController booleans in Inspector
```

### Issue 2: Can't Get Cup
```
Problem: Cabinet won't open

Check:
1. Curtains opened first?
2. areCurtainsOpened = true?
```

### Issue 3: Toybox Puzzle Won't Open
```
Problem: Can't see puzzle

Check:
1. Tapped toybox first? (isToyboxOpened = true)
2. Then tap icon in middle
3. Icon is separate GameObject with ToyboxIcon type
```

---

## 🎯 Summary

### Total Interactions: 15
- 6 Environmental discoveries
- 4 Puzzle sequences
- 2 Item pickups
- 3 Toybox interactions (open, puzzle, get doll)
- 1 Final mirror trigger

### All Must Be Complete Before Mirror!

---

**Follow this EXACT sequence!** 🎮✨

**Mirror only triggers when EVERYTHING is done!** 🪞
