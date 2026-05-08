# ROOM 10: VISUAL FLOWCHART

## Complete Sequence Flow Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                         ROOM 10 START                           │
│                    (Master Bedroom Scene)                       │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                      PHASE 1: ENTRY                             │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │ • Lisa enters room                                        │  │
│  │ • Feels drawn to mirror                                   │  │
│  │ • 4 entry dialogues play                                  │  │
│  │ • Player controls DISABLED                                │  │
│  └───────────────────────────────────────────────────────────┘  │
│                    Music: TENSE (starts)                        │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                  PHASE 2: EMILY BLOCKS                          │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │ • Emily manifests (solid, visible)                        │  │
│  │ • Blocks mirror access                                    │  │
│  │ • 3 Emily dialogues play                                  │  │
│  │ • Player controls RE-ENABLED                              │  │
│  └───────────────────────────────────────────────────────────┘  │
│                    Music: TENSE (continues)                     │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│              PHASE 3: EXPLORATION (Player Choice)               │
│                                                                 │
│  Player can interact with 3 objects in any order:              │
│                                                                 │
│  ┌──────────────┐    ┌──────────────┐    ┌──────────────┐     │
│  │     BED      │    │    DIARY     │    │  MUSIC BOX   │     │
│  ├──────────────┤    ├──────────────┤    ├──────────────┤     │
│  │ Click to     │    │ Click to     │    │ Click to     │     │
│  │ examine      │    │ examine      │    │ find lullaby │     │
│  │              │    │              │    │              │     │
│  │ Shows 3      │    │ Shows 2      │    │ Shows 4      │     │
│  │ dialogues    │    │ dialogues    │    │ dialogues    │     │
│  │              │    │              │    │              │     │
│  │ Sets:        │    │ Sets:        │    │ Plays audio  │     │
│  │ examined=true│    │ examined=true│    │              │     │
│  │              │    │              │    │ Adds item    │     │
│  │              │    │              │    │              │     │
│  │              │    │              │    │ Sets:        │     │
│  │              │    │              │    │ lullaby=true │     │
│  └──────────────┘    └──────────────┘    └──────────────┘     │
│                                                                 │
│  Music: TENSE → switches to LULLABY when music box found       │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼
                    ┌────────────────┐
                    │  Requirements  │
                    │  Check:        │
                    │                │
                    │  examined=true │
                    │  AND           │
                    │  lullaby=true  │
                    └────────┬───────┘
                             │
                ┌────────────┴────────────┐
                │                         │
               NO                        YES
                │                         │
                ▼                         ▼
        ┌───────────────┐      ┌─────────────────────┐
        │ Mirror Locked │      │  PHASE 4: UNLOCK    │
        │               │      │                     │
        │ Show hint if  │      │ • Reality distorts  │
        │ player clicks │      │ • 5 dialogues play  │
        │ mirror        │      │ • Mirror glow ON    │
        └───────────────┘      │ • canAccess=true    │
                               └──────────┬──────────┘
                                          │
                                          ▼
┌─────────────────────────────────────────────────────────────────┐
│                PHASE 5: MIRROR APPROACH                         │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │ Player clicks mirror                                      │  │
│  │                                                           │  │
│  │ • Lisa approaches (2 dialogues)                          │  │
│  │ • Emily desperate (2 dialogues)                          │  │
│  │ • Emily accepts (3 dialogues)                            │  │
│  │ • Mirror activates (2 dialogues)                         │  │
│  │                                                           │  │
│  │ Player controls DISABLED                                 │  │
│  └───────────────────────────────────────────────────────────┘  │
│                    Music: LULLABY (continues)                   │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│              PHASE 6: FLASHBACK SEQUENCE                        │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │ Full-screen flashback panel appears                       │  │
│  │                                                           │  │
│  │ 9 Images shown in sequence:                              │  │
│  │                                                           │  │
│  │ 1. Mother with pillow                                    │  │
│  │ 2. Young Lisa terrified                                  │  │
│  │ 3. Emily enters Lisa                                     │  │
│  │ 4. Possessed Lisa moves                                  │  │
│  │ 5. Fighting back                                         │  │
│  │ 6. Hands on throat                                       │  │
│  │ 7. Emily overlapping                                     │  │
│  │ 8. Mother falls                                          │  │
│  │ 9. Emily leaves, Lisa collapses                          │  │
│  │                                                           │  │
│  │ Each image: 3 seconds + dialogue                         │  │
│  │ Player clicks to advance                                 │  │
│  └───────────────────────────────────────────────────────────┘  │
│                    Music: LULLABY (continues)                   │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│            PHASE 7: UNDERSTANDING                               │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │ Flashback panel closes                                    │  │
│  │                                                           │  │
│  │ Dialogue sequence:                                        │  │
│  │ • Lisa processes (2 dialogues)                           │  │
│  │ • Lisa confronts (2 dialogues)                           │  │
│  │ • Emily explains (5 dialogues)                           │  │
│  │ • Lisa responds (4 dialogues)                            │  │
│  │ • Emily apologizes (4 dialogues)                         │  │
│  │                                                           │  │
│  │ Total: 17 dialogues                                      │  │
│  └───────────────────────────────────────────────────────────┘  │
│                    Music: LULLABY (continues)                   │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│              PHASE 8: FORGIVENESS                               │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │ • Lisa forgives (3 dialogues)                            │  │
│  │ • Emily relief (2 dialogues)                             │  │
│  │                                                           │  │
│  │ hasForgiven = true                                       │  │
│  └───────────────────────────────────────────────────────────┘  │
│                Music: Switches to PEACEFUL                      │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│            PHASE 9: EMILY'S DEPARTURE                           │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │ • Emily fades dialogue (3 dialogues)                     │  │
│  │ • Emily sprite fades (3 seconds, alpha 1.0 → 0.0)       │  │
│  │ • Final goodbye (2 dialogues)                            │  │
│  │                                                           │  │
│  │ emilyHasFaded = true                                     │  │
│  └───────────────────────────────────────────────────────────┘  │
│                    Music: PEACEFUL (continues)                  │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                PHASE 10: EPILOGUE                               │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │ • Epilogue dialogues (3 dialogues)                       │  │
│  │ • Fade to black (2 seconds)                              │  │
│  │ • Save game completion                                   │  │
│  │ • Load ending scene                                      │  │
│  └───────────────────────────────────────────────────────────┘  │
│                    Music: PEACEFUL (fades out)                  │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                         GAME END                                │
│                    (Ending Scene Loads)                         │
└─────────────────────────────────────────────────────────────────┘
```

---

## State Diagram

```
┌──────────────────────────────────────────────────────────────┐
│                      ROOM 10 STATES                          │
└──────────────────────────────────────────────────────────────┘

Initial State:
┌─────────────────────┐
│ isIntroDone: false  │
│ hasExaminedRoom: f  │
│ hasFoundLullaby: f  │
│ canAccessMirror: f  │
│ hasApproached: f    │
│ hasSeenFlashback: f │
│ hasForgiven: false  │
│ emilyHasFaded: f    │
└─────────────────────┘

After Intro:
┌─────────────────────┐
│ isIntroDone: TRUE   │ ← Player can now explore
│ hasExaminedRoom: f  │
│ hasFoundLullaby: f  │
│ canAccessMirror: f  │
└─────────────────────┘

After Examining Bed/Diary:
┌─────────────────────┐
│ isIntroDone: TRUE   │
│ hasExaminedRoom: T  │ ← Requirement 1 met
│ hasFoundLullaby: f  │
│ canAccessMirror: f  │
└─────────────────────┘

After Finding Music Box:
┌─────────────────────┐
│ isIntroDone: TRUE   │
│ hasExaminedRoom: T  │
│ hasFoundLullaby: T  │ ← Requirement 2 met
│ canAccessMirror: f  │
└─────────────────────┘
         │
         ▼ (CheckProgression)
┌─────────────────────┐
│ canAccessMirror: T  │ ← Mirror unlocked!
└─────────────────────┘

After Clicking Mirror:
┌─────────────────────┐
│ hasApproached: TRUE │
│ hasSeenFlashback: T │
│ hasForgiven: TRUE   │
│ emilyHasFaded: TRUE │
└─────────────────────┘
         │
         ▼
    GAME END
```

---

## Music Flow Diagram

```
┌────────────────────────────────────────────────────────────┐
│                      MUSIC TIMELINE                        │
└────────────────────────────────────────────────────────────┘

START
  │
  ▼
┌─────────────────┐
│  TENSE MUSIC    │ ← Intro, Emily blocks, Exploration
│  (Loop)         │
└────────┬────────┘
         │
         │ Music Box Found
         ▼
┌─────────────────┐
│  LULLABY        │ ← Music box, Mirror unlock, Approach,
│  (Loop)         │   Flashback, Understanding, Forgiveness
└────────┬────────┘
         │
         │ Emily Departs
         ▼
┌─────────────────┐
│  PEACEFUL       │ ← Departure, Epilogue
│  (Loop)         │
└────────┬────────┘
         │
         │ Scene Transition
         ▼
      FADE OUT
```

---

## Player Control Timeline

```
┌────────────────────────────────────────────────────────────┐
│                  PLAYER CONTROL STATUS                     │
└────────────────────────────────────────────────────────────┘

DISABLED ████████████ Entry & Emily Blocks
ENABLED  ████████████ Exploration Phase
DISABLED ████████████ Mirror Unlock Dialogues
ENABLED  ████████████ (Brief - can click mirror)
DISABLED ████████████ Approach, Flashback, Understanding,
                      Forgiveness, Departure, Epilogue
         ████████████ (Until scene transition)

Legend:
DISABLED = Player cannot move, joystick hidden
ENABLED  = Player can move and interact
```

---

## Dialogue Count by Phase

```
┌──────────────────────────────────────────────────────────┐
│                  DIALOGUE BREAKDOWN                      │
└──────────────────────────────────────────────────────────┘

Phase 1: Entry                    4 dialogues
Phase 2: Emily Blocks             3 dialogues
Phase 3: Examination
  - Bed                           3 dialogues
  - Diary                         2 dialogues
  - Music Box                     4 dialogues
Phase 4: Unlock                   5 dialogues
Phase 5: Approach                 9 dialogues
Phase 6: Flashback                9 dialogues
Phase 7: Understanding           17 dialogues
Phase 8: Forgiveness              5 dialogues
Phase 9: Departure                5 dialogues
Phase 10: Epilogue                3 dialogues
                                ───────────
                        TOTAL:   60+ dialogues

Average per phase: 6 dialogues
Longest phase: Understanding (17)
Shortest phase: Diary (2)
```

---

## Progression Requirements

```
┌──────────────────────────────────────────────────────────┐
│              UNLOCK CONDITIONS DIAGRAM                   │
└──────────────────────────────────────────────────────────┘

                    Can Access Mirror?
                           │
                           ▼
              ┌────────────────────────┐
              │  hasExaminedRoom?      │
              └────────┬───────────────┘
                       │
          ┌────────────┴────────────┐
          │                         │
         NO                        YES
          │                         │
          ▼                         ▼
    ┌─────────┐           ┌─────────────────┐
    │ LOCKED  │           │ hasFoundLullaby?│
    └─────────┘           └────────┬────────┘
                                   │
                      ┌────────────┴────────────┐
                      │                         │
                     NO                        YES
                      │                         │
                      ▼                         ▼
                ┌─────────┐              ┌──────────┐
                │ LOCKED  │              │ UNLOCKED │
                └─────────┘              └──────────┘
                                              │
                                              ▼
                                    ┌──────────────────┐
                                    │ • Glow activates │
                                    │ • Can click      │
                                    └──────────────────┘
```

---

## Object Interaction Map

```
┌──────────────────────────────────────────────────────────┐
│                INTERACTABLE OBJECTS                      │
└──────────────────────────────────────────────────────────┘

Room Layout:
┌─────────────────────────────────────────────────────┐
│                                                     │
│  [Diary]                              [Music Box]  │
│                                                     │
│                                                     │
│              [Emily]  [Mirror]                      │
│                        (Glow)                       │
│                                                     │
│                                                     │
│  [Bed]                                             │
│                                                     │
│                                    [Player Start]   │
└─────────────────────────────────────────────────────┘

Interaction Flow:
1. Player enters → Intro plays
2. Player can click: Bed, Diary, Music Box
3. After requirements → Mirror unlocks (glow appears)
4. Player clicks Mirror → Final sequence begins
```

---

## Timeline Estimate

```
┌──────────────────────────────────────────────────────────┐
│                  ESTIMATED PLAYTIME                      │
└──────────────────────────────────────────────────────────┘

Phase 1: Entry                    1 minute
Phase 2: Emily Blocks             30 seconds
Phase 3: Exploration              2-3 minutes
Phase 4: Unlock                   1 minute
Phase 5: Approach                 1.5 minutes
Phase 6: Flashback                2 minutes
Phase 7: Understanding            3 minutes
Phase 8: Forgiveness              1 minute
Phase 9: Departure                1.5 minutes
Phase 10: Epilogue                1 minute
                                ─────────────
                        TOTAL:   14-15 minutes

Note: Actual time depends on player reading speed
```

---

## Visual Effects Timeline

```
┌──────────────────────────────────────────────────────────┐
│                    VISUAL EFFECTS                        │
└──────────────────────────────────────────────────────────┘

Entry           → Normal lighting
Emily Blocks    → Normal lighting
Exploration     → Normal lighting
Unlock          → Mirror glow ACTIVATES
                  (Optional: Screen shake, vignette)
Approach        → Mirror glow INTENSIFIES
Flashback       → Full-screen black panel
                  9 images fade in/out
Understanding   → Normal lighting
Forgiveness     → Normal lighting
Departure       → Emily alpha fade (1.0 → 0.0, 3 seconds)
                  (Optional: Light particles)
Epilogue        → Fade to black (2 seconds)
```

---

## Save System Integration

```
┌──────────────────────────────────────────────────────────┐
│                    SAVE POINTS                           │
└──────────────────────────────────────────────────────────┘

Throughout Room:
• Auto-save on each milestone
• Track progression flags

End of Room:
┌─────────────────────────────────────────┐
│ SaveSystem.Instance?.MarkPuzzleSolved(  │
│     "game_complete"                     │
│ )                                       │
└─────────────────────────────────────────┘

What Gets Saved:
• Game completion flag
• All lullaby fragments (4/4)
• Final scene reached
• Timestamp
```

---

## Error Handling Flow

```
┌──────────────────────────────────────────────────────────┐
│                  ERROR PREVENTION                        │
└──────────────────────────────────────────────────────────┘

Before Each Action:
┌─────────────────────────────────────┐
│ Check if controller exists          │
│ Check if intro is done              │
│ Check if requirements are met       │
│ Check if already completed          │
└─────────────────────────────────────┘

If Error:
┌─────────────────────────────────────┐
│ • Log error to console              │
│ • Show hint dialogue (if applicable)│
│ • Don't break game flow             │
│ • Allow player to retry             │
└─────────────────────────────────────┘
```

---

This visual flowchart provides a clear overview of the entire Room 10 sequence, making it easier to understand the flow, timing, and requirements at a glance.