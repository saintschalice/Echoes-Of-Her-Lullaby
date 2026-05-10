# Room 08 - Final Flow (Simplified)

## BAGONG FLOW (FINAL)

### 1. Pagpasok sa Bathroom
```
✅ Door locked
✅ Emily humming outside
✅ Mirror: Normal sprite
✅ Torn clothes: Visible sa floor
✅ Apology note: Visible sa floor
✅ Hammer: Sa medicine cabinet
```

### 2. Collect Items (Any Order)
```
Option A:
1. Pick up torn clothes (floor) → Dialogue
2. Pick up apology note (floor) → Dialogue
3. Get hammer (medicine cabinet) → Dialogue

Option B:
1. Get hammer first
2. Pick up note
3. Pick up torn clothes

Option C:
- Any order, doesn't matter!
```

### 3. After Makuha Lahat (3 items)
```
✅ Emily ENTERS the room!
✅ Dialogue: "She's inside!"
✅ Emily starts HUNTING
✅ Player must avoid Emily
```

### 4. Interact with Mirror (While Emily Hunts)
```
✅ Interact with mirror
✅ Dialogue: "I need to break this!"
✅ QTE starts immediately
```

### 5. QTE (Single Button)
```
✅ One big button on screen
✅ Click 50 times
✅ 25 seconds time limit
✅ Timer counts down
✅ Progress shows: "25/50"
```

### 6. After QTE Success
```
✅ Mirror breaks
✅ Mirror sprite changes to broken
✅ Emily disappears/stops hunting
✅ Passage becomes visible
✅ Dialogue: "There's a passage!"
```

### 7. Enter Passage
```
✅ Interact with passage
✅ Load next scene
```

---

## KEY CHANGES FROM PREVIOUS VERSION

### REMOVED:
- ❌ Emily in mirror (no longer appears in mirror)
- ❌ Complex mirror examination dialogues
- ❌ "First time" vs "second time" mirror interaction
- ❌ Bathtub auto-gives torn clothes
- ❌ Multiple tap targets

### SIMPLIFIED:
- ✅ Torn clothes = pickup from floor
- ✅ Apology note = pickup from floor
- ✅ Hammer = from cabinet
- ✅ Emily enters AFTER all items collected
- ✅ Mirror QTE = single button, 50 clicks, 25 seconds
- ✅ No complex dialogue trees

---

## SETUP REQUIREMENTS

### Scene Objects:

#### 1. Torn Clothes (Floor)
```
GameObject: TornClothes
├── SpriteRenderer (torn clothes sprite)
├── Collider2D (trigger)
└── Room08_Interactable
    ├── My Type: Evidence
    └── Evidence Id: "torn_clothes"
```

#### 2. Apology Note (Floor)
```
GameObject: ApologyNote
├── SpriteRenderer (note sprite)
├── Collider2D (trigger)
└── Room08_Interactable
    ├── My Type: Evidence
    └── Evidence Id: "apology_note"
```

#### 3. Medicine Cabinet
```
GameObject: MedicineCabinet
├── SpriteRenderer
├── Collider2D (trigger)
└── Room08_Interactable
    └── My Type: MedicineCabinet
```

#### 4. Mirror
```
GameObject: Mirror
├── SpriteRenderer
│   └── Sprite: mirrorNormalSprite (initially)
├── Collider2D (trigger)
└── Room08_Interactable
    └── My Type: Mirror
```

#### 5. Emily AI
```
GameObject: EmilyAI
├── SpriteRenderer
├── NavMeshAgent
├── EmilyGhost (AI script)
└── Active: ✗ (disabled initially)
```

#### 6. Emily Spawn Point
```
GameObject: EmilySpawnPoint
└── Transform (position where Emily spawns)
```

#### 7. Passage
```
GameObject: Passage
├── SpriteRenderer (passage sprite)
├── Collider2D (trigger)
├── Room08_Interactable
│   └── My Type: Passage
└── Active: ✗ (disabled initially)
```

---

### Room08_FlowController Setup:

```
Room08_FlowController:
├── Evidence Collection
│   ├── Has Found Torn Clothes: false
│   ├── Has Found Apology Note: false
│   └── Has Found Hammer: false
├── Emily Hunt
│   ├── Is Emily Hunting: false
│   ├── Emily AI: [Assign EmilyAI GameObject]
│   ├── Emily Spawn Point: [Assign spawn Transform]
│   └── Emily Enter Sound: [Sound when Emily enters]
├── Mirror Sprites
│   ├── Mirror Sprite Renderer: [Assign Mirror SpriteRenderer]
│   ├── Mirror Normal Sprite: [Normal mirror sprite]
│   ├── Mirror Broken Sprite: [Broken mirror sprite]
│   └── Passage Object: [Assign Passage GameObject]
└── Emily AI (Outside)
    ├── Emily Humming Sound: [Humming loop]
    └── Emily Audio Source: [AudioSource component]
```

---

### Room08_MirrorQTE Setup:

```
Room08_MirrorQTE:
├── QTE Settings
│   ├── Total Taps: 50
│   ├── Total Time Limit: 25
│   └── Max Failures: 0
├── UI References
│   ├── QTE Panel: [Assign panel]
│   ├── Full Screen Tap Area: [Assign Image - full screen button]
│   ├── Timer Text TMP: [Shows "25.0s"]
│   └── Progress Text TMP: [Shows "0/50"]
└── Visual Effects
    ├── Mirror Image: [Mirror in QTE panel]
    ├── Mirror Phase 1-4: [Crack sprites]
    └── Shatter Effect: [Particle effect]
```

---

## QTE PANEL SETUP

```
QTE Panel (Canvas)
├── FullScreenTapArea (Image) ⭐ MAIN BUTTON
│   ├── Anchor: Stretch (full screen)
│   ├── Color: Transparent or semi-transparent
│   ├── Raycast Target: ✓
│   └── Button component (added by script)
├── TimerText (TextMeshProUGUI)
│   ├── Text: "25.0s"
│   ├── Font Size: 48
│   └── Color: White → Yellow → Red
├── ProgressText (TextMeshProUGUI)
│   ├── Text: "0/50"
│   ├── Font Size: 36
│   └── Color: White
└── MirrorImage (Image)
    └── Sprite: Changes as player clicks
```

---

## DIALOGUE FLOW

### Entry Dialogues:
```
1. ENTRY_1: "The door... it's locked."
2. ENTRY_2: "I can hear Emily outside, humming..."
3. DOOR_LOCKED: "It won't budge. I'm trapped."
4. EMILY_OUTSIDE: "She's right outside the door..."
```

### Evidence Dialogues:
```
TORN_CLOTHES_1: "These clothes... they're torn and bloody."
TORN_CLOTHES_2: "What happened here?"

APOLOGY_NOTE_1: "An apology note... from Emily?"
APOLOGY_NOTE_2: "She's sorry... but for what?"

HAMMER_FOUND_1: "A hammer! This could be useful."
HAMMER_FOUND_2: "Maybe I can break something with this..."
```

### Emily Enters:
```
EMILY_ENTERS: "The door! She's coming in!"
EMILY_HUNTING: "I need to find a way out, NOW!"
```

### Mirror QTE:
```
QTE_START: "The mirror... I can break through it!"
QTE_SUCCESS: "It's breaking! There's a passage behind it!"
QTE_FAILED: "No! She's getting closer!"
```

### After Breaking:
```
PASSAGE_FOUND_1: "The mirror shattered!"
PASSAGE_FOUND_2: "There's a passage behind it!"
CLIMB_THROUGH: "I need to get through before she catches me!"
```

---

## TESTING CHECKLIST

### Evidence Collection:
- [ ] Can pick up torn clothes from floor
- [ ] Can pick up apology note from floor
- [ ] Can get hammer from medicine cabinet
- [ ] Dialogues show for each item
- [ ] Items disappear after pickup

### Emily Enters:
- [ ] Emily enters AFTER all 3 items collected
- [ ] Dialogue shows: "She's inside!"
- [ ] Emily AI activates and hunts player
- [ ] Emily spawns at correct position

### Mirror QTE:
- [ ] Can interact with mirror while Emily hunts
- [ ] QTE panel shows with full screen button
- [ ] Timer shows "25.0s" and counts down
- [ ] Progress shows "0/50" and updates
- [ ] Can click anywhere on button
- [ ] Each click increments counter
- [ ] At 50 clicks, mirror breaks

### After QTE:
- [ ] Mirror sprite changes to broken
- [ ] Emily stops hunting/disappears
- [ ] Passage becomes visible
- [ ] Can interact with passage
- [ ] Loads next scene

---

## COMMON ISSUES

### Issue 1: Emily doesn't enter
**Check**:
- All 3 items collected?
- EmilyAI GameObject assigned?
- Emily Spawn Point assigned?
- OnAllEvidenceCollected() being called?

### Issue 2: Can't click QTE button
**Check**:
- FullScreenTapArea has Raycast Target checked
- FullScreenTapArea is full screen (Anchor: Stretch)
- Button component exists (added by script)

### Issue 3: Mirror doesn't break
**Check**:
- Clicked 50 times?
- Within 25 seconds?
- mirrorBrokenSprite assigned?
- mirrorSpriteRenderer assigned?

### Issue 4: Passage doesn't appear
**Check**:
- Passage GameObject exists
- Passage assigned to Room08_FlowController
- Passage initially disabled
- OnMirrorBroken() being called?

---

## FILES MODIFIED

1. ✅ `Room08_FlowController.cs`
   - Removed Emily in mirror logic
   - Added Emily enters room logic
   - Simplified evidence checking

2. ✅ `Room08_MirrorQTE.cs`
   - Single button QTE
   - 50 clicks, 25 seconds
   - Full screen tap area

3. ✅ `Room08_Interactable.cs`
   - Torn clothes as evidence pickup
   - Simplified mirror interaction
   - Removed complex dialogue trees

---

## SUMMARY

### What This Flow Does:
1. ✅ Player enters bathroom (locked)
2. ✅ Collects 3 items from floor/cabinet
3. ✅ Emily enters and hunts player
4. ✅ Player breaks mirror with QTE
5. ✅ Passage opens, player escapes

### Why It's Better:
- ✅ Simpler and clearer
- ✅ More action-focused
- ✅ Emily is a real threat (hunting)
- ✅ QTE is straightforward (one button)
- ✅ Less dialogue, more gameplay

**Status**: ✅ CODE COMPLETE
**Ready**: For Unity scene setup
**Date**: May 4, 2026
