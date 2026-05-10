# Room 07 - Strict Prerequisite System

## 🎯 Overview

Room 07 now has a **STRICT LINEAR SEQUENCE** where each step must be completed before the next can be accessed. Players will receive helpful validation messages if they try to skip ahead.

---

## 📋 Complete Sequence with Prerequisites

### 1. **Intro** (Automatic)
```
Trigger: Enter room
Dialogue: "This room... I know exactly where everything should be. But how? I've never been here before... have I?"
Result: isIntroDone = true
Prerequisite for: Bed
```

---

### 2. **Bed**
```
Prerequisite: isIntroDone = true
If not met: "I should take a moment to look around first..."

Dialogue: "Child's bed has two pillow indentations. Note pinned to second pillow: 'For my friend Emily - she keeps me safe at night.'"
Result: hasCheckedBed = true
Prerequisite for: Wall Drawings
```

---

### 3. **Wall Drawings**
```
Prerequisite: hasCheckedBed = true
If not met: "I should check the bed first. Something about it feels important."

Dialogue: "Crayon drawings show two figures - one labeled 'Me' and another labeled 'Emily'. They're holding hands, playing together."
Result: hasCheckedWall = true
Prerequisite for: Bookshelf/Diary
```

---

### 4. **Bookshelf/Diary**
```
Prerequisite: hasCheckedWall = true
If not met: "I should examine the wall drawings first. They might tell me something."

Dialogue 1: "I found a child's diary on the nightstand..."
Dialogue 2: "Child's diary: 'Emily came to me again last night. She sang the pretty song and made the scary dreams go away.'"
Result: hasCheckedDiary = true
Prerequisite for: Window Curtains
```

---

### 5. **Window Curtains**
```
Prerequisite: hasCheckedDiary = true
If not met: "I should read the diary first. It might have important information."

Dialogue: "Curtains tied shut with child's knots. Note: 'Emily says tie them tight to keep bad things out.'"
Action: Curtain Panel opens
Task: Open both left and right curtains
Result: areCurtainsOpened = true
Prerequisite for: Cabinet
```

---

### 6. **Small Cabinet (Get Emily's Cup)**
```
Prerequisite: areCurtainsOpened = true
If not met: "The cabinet is locked. I need to open the curtains first."

Action: Cabinet panel opens
Task: Click cup to take it
Dialogue: "Found Emily's Cup. This must be Emily's special cup."
Notification: Emily's Cup added to inventory
Result: hasEmilyCup = true
Prerequisite for: Tea Party
```

---

### 7. **Tea Party**
```
Prerequisite: hasEmilyCup = true (and emily_cup in inventory)
If not met: "Three cups set on floor. One is missing... 'Emily's Special Cup'. I need to find it first."

Action: Tea Party Panel opens
Task: Drag Emily's Cup to slot
Result: Tea party complete
Cutscene: Lisa regains memory of tea party with Emily
Dialogue 1: "I remember... Emily and I used to have tea parties together. She was always there for me."
Dialogue 2: "The tea party is complete. I should continue exploring the room."
Result: isTeaPartyDone = true
Prerequisite for: Chair
```

---

### 8. **Emily's Chair**
```
Prerequisite: isTeaPartyDone = true
If not met: "I should complete the tea party ritual first. Something tells me it's important."

Dialogue: "Small chair marked 'Emily's Chair - Do Not Sit'. It's always cold to the touch. The strongest presence is felt here."
Result: hasCheckedChair = true
Prerequisite for: Closet
```

---

### 9. **Closet**
```
Prerequisite: hasCheckedChair = true
If not met: "I should check Emily's chair first. The presence there is so strong..."

Dialogue: "Scratches inside the closet. A child hid here often. Emily's presence is overwhelming - she protected Lisa here."
Result: hasCheckedCloset = true
Prerequisite for: Toybox
```

---

### 10. **Toybox (First Interaction - Puzzle)**
```
Prerequisite: hasCheckedCloset = true
If not met: "I should check the closet first. Something about it feels significant."

Action: Toybox Panel opens
Task: Solve 8-tile sliding puzzle (swipe controls)
Result: isToyboxSolved = true
Dialogue: "The lock clicked. Something opened inside the toybox."
Prerequisite for: Getting Doll
```

---

### 11. **Toybox (Second Interaction - Get Doll)**
```
Prerequisite: isToyboxSolved = true
If not met: Panel opens (puzzle not solved yet)

Dialogue: "Hidden compartment! Note: 'Dear Emily, thank you for making mommy stop hurting me yesterday.'"
Notification: Emily Doll added to inventory
Cutscene: Lisa's memory with Emily's doll
Result: hasEmilyDoll = true
Prerequisite for: Dollhouse
```

---

### 12. **Dollhouse**
```
Prerequisite: hasEmilyDoll = true (and emily_doll in inventory)
If not met: "Family figures removed except the child doll. It looks lonely. Maybe I need to find something to complete it."

Action: Dollhouse Panel opens
Task: Drag Emily Doll to dollhouse
Dialogue: "Extra doll added labeled 'Emily'. She was very real to this child."
Result: isDollhouseDone = true
Prerequisite for: Reading Table
```

---

### 13. **Reading Table**
```
Prerequisite: isDollhouseDone = true
If not met: "I should complete the dollhouse first. It feels like the next step."

Dialogue: "Fairy tale books with notes: 'Emily likes the stories where the princess gets saved.'"
Result: hasCheckedReadingTable = true
Prerequisite for: Mirror (final trigger)
```

---

### 14. **Mirror (FINAL)**
```
Prerequisite: EVERYTHING must be complete
  ✓ hasCheckedBed
  ✓ hasCheckedWall
  ✓ hasCheckedDiary
  ✓ areCurtainsOpened
  ✓ isTeaPartyDone
  ✓ hasCheckedChair
  ✓ hasCheckedCloset
  ✓ isToyboxSolved
  ✓ hasEmilyDoll
  ✓ isDollhouseDone
  ✓ hasCheckedReadingTable

If not met: Specific hint about what's missing (see below)

If ALL complete:
  Cutscene: Emily appears in mirror behind Lisa
  Audio: Lullaby Fragment #3 plays
  Memory: Someone tucking young Lisa into bed, singing softly
  Action: Door locks
  Chase: Emily becomes aggressive
  Goal: Escape to bathroom
```

---

## 🔍 Smart Hint System

When player tries to interact with Mirror before completing everything, they get a **specific hint** about the next step:

```csharp
if (!hasCheckedBed) → "I should check the bed first."
if (!hasCheckedWall) → "I should examine the wall drawings."
if (!hasCheckedDiary) → "I should read the diary on the bookshelf."
if (!areCurtainsOpened) → "I should open the window curtains."
if (!hasEmilyCup) → "I should check the small cabinet."
if (!isTeaPartyDone) → "I should complete the tea party ritual."
if (!hasCheckedChair) → "I should check Emily's chair."
if (!hasCheckedCloset) → "I should check the closet."
if (!isToyboxSolved) → "I should solve the toybox puzzle."
if (!hasEmilyDoll) → "I should get the doll from the toybox."
if (!isDollhouseDone) → "I should complete the dollhouse."
if (!hasCheckedReadingTable) → "I should check the reading table."
```

---

## 📊 Prerequisite Chain Diagram

```
INTRO (auto)
  ↓ (prerequisite for Bed)
BED
  ↓ (prerequisite for Wall)
WALL DRAWINGS
  ↓ (prerequisite for Diary)
BOOKSHELF/DIARY
  ↓ (prerequisite for Curtains)
WINDOW CURTAINS
  ↓ (prerequisite for Cabinet)
SMALL CABINET → Get Emily's Cup
  ↓ (prerequisite for Tea Party)
TEA PARTY → Use Cup
  ↓ (prerequisite for Chair)
EMILY'S CHAIR
  ↓ (prerequisite for Closet)
CLOSET
  ↓ (prerequisite for Toybox)
TOYBOX → Solve Puzzle
  ↓ (prerequisite for getting Doll)
TOYBOX → Get Emily Doll
  ↓ (prerequisite for Dollhouse)
DOLLHOUSE → Use Doll
  ↓ (prerequisite for Reading Table)
READING TABLE
  ↓ (prerequisite for Mirror)
MIRROR → FINAL TRIGGER
  ↓
CHASE SEQUENCE
```

---

## 🎮 Validation Messages

### Environmental Objects:
```
Bed (no intro): "I should take a moment to look around first..."
Wall (no bed): "I should check the bed first. Something about it feels important."
Diary (no wall): "I should examine the wall drawings first. They might tell me something."
Chair (no tea party): "I should complete the tea party ritual first. Something tells me it's important."
Closet (no chair): "I should check Emily's chair first. The presence there is so strong..."
Reading Table (no dollhouse): "I should complete the dollhouse first. It feels like the next step."
```

### Puzzle Objects:
```
Curtains (no diary): "I should read the diary first. It might have important information."
Cabinet (no curtains): "The cabinet is locked. I need to open the curtains first."
Tea Party (no cup): "Three cups set on floor. One is missing... 'Emily's Special Cup'. I need to find it first."
Toybox (no closet): "I should check the closet first. Something about it feels significant."
Dollhouse (no doll): "Family figures removed except the child doll. It looks lonely. Maybe I need to find something to complete it."
```

### Mirror:
```
If anything missing: Specific hint about next step
If everything complete: Trigger climax sequence
```

---

## 🧪 Testing the Prerequisite System

### Test 1: Try to Skip Ahead
```
1. Enter room (intro plays)
2. Try to interact with Wall (should block)
3. Message: "I should check the bed first..."
4. Interact with Bed
5. Now Wall should work ✓
```

### Test 2: Try to Open Cabinet Early
```
1. Complete: Intro → Bed → Wall → Diary
2. Try to interact with Cabinet (should block)
3. Message: "The cabinet is locked. I need to open the curtains first."
4. Open Curtains
5. Now Cabinet should work ✓
```

### Test 3: Try Mirror Too Early
```
1. Complete only half the sequence
2. Interact with Mirror
3. Should get specific hint about next step
4. Example: "I should check Emily's chair."
```

### Test 4: Complete Everything
```
1. Follow entire sequence in order
2. Complete all 13 steps
3. Interact with Mirror
4. Should trigger climax sequence ✓
```

---

## 🐛 Debugging

### Check Current Progress:
```
1. In Unity Hierarchy, select: Room07_FlowController
2. Inspector → Room07_FlowController component
3. Check all boolean flags
4. See which steps are complete
```

### Console Logs:
```
Every interaction logs:
"[Room07] Interacting with: {ObjectType}"

This helps you see which object is being interacted with
```

### Common Issues:

**Issue 1: Validation not working**
```
Check:
- Room07_FlowController exists in scene?
- All booleans are properly set?
- Scripts are updated?
```

**Issue 2: Wrong validation message**
```
Check:
- Correct ObjectType assigned in Inspector?
- Flow controller booleans match actual progress?
```

**Issue 3: Can't progress**
```
Check:
- Previous step actually completed?
- Boolean flag set to true?
- No errors in Console?
```

---

## ✅ Implementation Checklist

- [x] Intro → Bed prerequisite
- [x] Bed → Wall prerequisite
- [x] Wall → Diary prerequisite
- [x] Diary → Curtains prerequisite
- [x] Curtains → Cabinet prerequisite
- [x] Cabinet → Tea Party prerequisite
- [x] Tea Party → Chair prerequisite
- [x] Chair → Closet prerequisite
- [x] Closet → Toybox prerequisite
- [x] Toybox solved → Get Doll prerequisite
- [x] Doll → Dollhouse prerequisite
- [x] Dollhouse → Reading Table prerequisite
- [x] Everything → Mirror prerequisite
- [x] Smart hint system for Mirror
- [x] Validation messages for all steps
- [x] Debug logging

---

## 🎯 Summary

**The system now enforces a strict linear sequence:**
1. Each step requires the previous step to be complete
2. Players get helpful hints if they try to skip
3. Mirror gives specific guidance about what's missing
4. All validation messages are contextual and helpful
5. Debug logs help troubleshoot any issues

**No more confusion about what to do next!** 🎮✨

