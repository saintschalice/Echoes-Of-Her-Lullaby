# Room 07 Complete Flow Guide

## ✅ FIXED: Dialogue and Panel Sequence

Lahat ng panels ay may proper sequence na:
1. Dialogue muna (kung may dialogue)
2. Wait for dialogue to finish
3. Then open panel

---

## 🎮 Complete Gameplay Flow

### Phase 1: Introduction & Environmental Storytelling

#### 1. Enter Room
```
Trigger: Player enters room
Action: Intro dialogue plays automatically
Dialogue: "This room... I know exactly where everything should be..."
Result: isIntroDone = true
```

#### 2. Explore Environmental Objects (Any Order)
```
Bed:
  → Dialogue: "Child's bed has two pillow indentations..."
  
Wall Drawings:
  → Dialogue: "Crayon drawings show two figures..."
  
Diary:
  → Dialogue: "Child's diary: Emily came to me again..."
  
Chair:
  → Dialogue: "Small chair marked Emily's Chair..."
  
Closet:
  → Dialogue: "Scratches inside the closet..."
  
Reading Table:
  → Dialogue: "Fairy tale books..."
```

---

### Phase 2: Puzzle Sequence (MUST BE IN ORDER!)

#### Puzzle 1: Window Curtains (FIRST PUZZLE)
```
Interact with Window Curtains:
  1. Dialogue: "Curtains tied shut with child's knots..."
  2. Wait for dialogue to finish
  3. Curtain Panel opens
  4. Player opens left curtain
  5. Player opens right curtain
  6. Panel closes automatically
  7. Dialogue: "The curtains are open..."
  8. Result: areCurtainsOpened = true
```

#### Puzzle 2: Get Emily's Cup (AFTER CURTAINS)
```
Interact with Small Cabinet:
  Condition: areCurtainsOpened must be true
  
  If curtains NOT open:
    → Nothing happens (or show hint dialogue)
  
  If curtains open:
    1. Dialogue: "I found a small cup..."
    2. Wait for dialogue to finish
    3. Item notification shows
    4. Player taps to continue
    5. Cup added to inventory
    6. Result: hasEmilyCup = true
```

#### Puzzle 3: Tea Party (NEED CUP)
```
Interact with Tea Party Spot:
  Condition: Must have emily_cup in inventory
  
  If NO cup:
    → Dialogue: "Three cups set on floor. One is missing..."
  
  If HAS cup:
    1. Tea Party Panel opens
    2. Player drags cup to slot
    3. Panel closes
    4. Memory Cutscene 1 plays (black screen)
    5. Cup removed from inventory
    6. Result: isTeaPartyDone = true
```

#### Puzzle 4: Toybox Sliding Puzzle
```
Interact with Toybox (First Time):
  1. Toybox Panel opens (8-tile sliding puzzle)
  2. Player solves puzzle
  3. Panel closes
  4. Dialogue: "The lock clicked..."
  5. Result: isToyboxSolved = true
```

#### Puzzle 5: Get Emily Doll (AFTER TOYBOX SOLVED)
```
Interact with Toybox (Second Time):
  Condition: isToyboxSolved must be true
  
  1. Dialogue: "Hidden compartment! Note: Dear Emily..."
  2. Wait for dialogue to finish
  3. Item notification shows
  4. Player taps to continue
  5. Doll added to inventory
  6. Memory Cutscene 2 plays (black screen)
  7. Result: hasEmilyDoll = true
```

#### Puzzle 6: Dollhouse (NEED DOLL)
```
Interact with Dollhouse:
  Condition: Must have emily_doll in inventory
  
  If NO doll:
    → Dialogue: "Family figures removed except the child doll..."
  
  If HAS doll:
    1. Dollhouse Panel opens
    2. Player drags doll to slot
    3. Panel closes
    4. Dialogue: "Extra doll added labeled Emily..."
    5. Doll removed from inventory
    6. Result: isDollhouseDone = true
```

---

### Phase 3: Climax & Chase

#### Mirror Interaction (AFTER ALL PUZZLES)
```
Interact with Mirror:
  Check conditions:
    - areCurtainsOpened = true?
    - isTeaPartyDone = true?
    - isToyboxSolved = true?
    - isDollhouseDone = true?
  
  If NOT all complete:
    → Dialogue: "I feel like I'm still missing something..."
  
  If ALL complete:
    1. Dialogue: "Let me check the mirror..."
    2. JUMPSCARE! Emily appears
    3. Lullaby Fragment #3 plays
    4. Dialogue: "That lullaby..."
    5. Bedroom door locks
    6. Dialogue: "The door is locked! I need to get to the bathroom!"
    7. Emily starts chasing (fast!)
    8. Player must escape to bathroom
```

---

## 📊 Flow Diagram

```
START
  ↓
Intro Dialogue
  ↓
Explore Environmental Objects (any order)
  ↓
Window Curtains Puzzle ✓
  ↓
Get Emily's Cup ✓
  ↓
Tea Party Puzzle ✓
  ↓
Toybox Sliding Puzzle ✓
  ↓
Get Emily Doll ✓
  ↓
Dollhouse Puzzle ✓
  ↓
Mirror Interaction
  ↓
JUMPSCARE & CHASE
  ↓
Escape to Bathroom
  ↓
END (Next Scene)
```

---

## 🔒 Puzzle Dependencies

```
Window Curtains
  └─→ Unlocks: Small Cabinet
       └─→ Gives: Emily's Cup
            └─→ Unlocks: Tea Party Puzzle
            
Toybox Puzzle
  └─→ Unlocks: Toybox (second interaction)
       └─→ Gives: Emily Doll
            └─→ Unlocks: Dollhouse Puzzle

All 4 Puzzles Complete
  └─→ Unlocks: Mirror Jumpscare
```

---

## ✅ Completion Checklist

### Environmental Exploration (Optional):
- [ ] Bed dialogue seen
- [ ] Wall Drawings dialogue seen
- [ ] Diary dialogue seen
- [ ] Chair dialogue seen
- [ ] Closet dialogue seen
- [ ] Reading Table dialogue seen

### Required Puzzles (Must Complete):
- [ ] Window Curtains opened
- [ ] Emily's Cup obtained
- [ ] Tea Party completed
- [ ] Toybox puzzle solved
- [ ] Emily Doll obtained
- [ ] Dollhouse completed

### Final Sequence:
- [ ] Mirror interaction triggers jumpscare
- [ ] Emily chases player
- [ ] Player escapes to bathroom

---

## 🐛 Common Flow Issues

### Issue 1: Can't Get Cup
```
Problem: Cabinet doesn't give cup
Cause: Curtains not opened yet
Solution: Open curtains first!
```

### Issue 2: Tea Party Won't Open
```
Problem: Tea Party Spot doesn't open panel
Cause: Don't have cup in inventory
Solution: Get cup from cabinet first!
```

### Issue 3: Can't Get Doll
```
Problem: Toybox doesn't give doll
Cause: Puzzle not solved yet
Solution: Solve sliding puzzle first!
```

### Issue 4: Dollhouse Won't Open
```
Problem: Dollhouse doesn't open panel
Cause: Don't have doll in inventory
Solution: Get doll from toybox first!
```

### Issue 5: Mirror Doesn't Trigger
```
Problem: Mirror just shows "missing something" dialogue
Cause: Not all puzzles complete
Solution: Complete all 4 puzzles:
  ✓ Curtains
  ✓ Tea Party
  ✓ Toybox
  ✓ Dollhouse
```

---

## 🧪 Testing the Flow

### Test 1: Linear Progression
```
1. Enter room → Intro plays ✓
2. Interact with curtains → Panel opens ✓
3. Solve curtains → Get cup ✓
4. Interact with tea party → Panel opens ✓
5. Solve tea party → Cutscene plays ✓
6. Interact with toybox → Panel opens ✓
7. Solve toybox → Get doll ✓
8. Interact with dollhouse → Panel opens ✓
9. Solve dollhouse → Complete ✓
10. Interact with mirror → Jumpscare! ✓
```

### Test 2: Out of Order (Should Block)
```
Try tea party without cup:
  → Should show "missing cup" dialogue ✓

Try dollhouse without doll:
  → Should show "lonely doll" dialogue ✓

Try mirror without all puzzles:
  → Should show "missing something" dialogue ✓
```

### Test 3: Skip Environmental Objects
```
Go straight to puzzles:
  → Should work! Environmental objects are optional ✓
```

---

## 🎯 Debug: Check Flow State

### In Play Mode:
```
Select Room07_Manager
→ Room07_FlowController component
→ Check booleans:
  isIntroDone: true/false
  areCurtainsOpened: true/false
  hasEmilyCup: true/false
  isTeaPartyDone: true/false
  isToyboxSolved: true/false
  hasEmilyDoll: true/false
  isDollhouseDone: true/false
```

### Console Logs:
```
"[Room07] Showing Curtain Panel" = Curtain panel opened
"[Room07] Showing Tea Party Panel" = Tea party opened
"[Room07] Showing Toybox Panel" = Toybox opened
"[Room07] Showing Dollhouse Panel" = Dollhouse opened
"Climax Triggered! Emily appears." = All puzzles done!
```

---

## 📝 Summary

### Correct Order:
1. **Intro** (automatic)
2. **Explore** (optional, any order)
3. **Curtains** → Opens panel
4. **Cabinet** → Get cup (after curtains)
5. **Tea Party** → Opens panel (need cup)
6. **Toybox** → Opens panel
7. **Toybox again** → Get doll (after solving)
8. **Dollhouse** → Opens panel (need doll)
9. **Mirror** → Jumpscare (after all puzzles)
10. **Chase** → Escape!

### Key Points:
- ✅ Dialogue always shows BEFORE panels
- ✅ Panels wait for dialogue to finish
- ✅ Items show notification after dialogue
- ✅ Puzzles have dependencies (must follow order)
- ✅ Environmental objects are optional

---

**Follow this flow and everything should work!** 🎮✨
