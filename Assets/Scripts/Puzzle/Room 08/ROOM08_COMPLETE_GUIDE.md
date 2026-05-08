# 🛁 ROOM 08 - LISA'S BATHROOM - COMPLETE IMPLEMENTATION GUIDE

## 📋 BUOD (SUMMARY)

Room 08 ay yung bathroom kung saan naka-lock si Lisa. Emily is outside, humming. Lisa needs to examine evidence, confront Emily through the mirror, break the mirror via QTE, then escape to Master Bathroom.

**Lahat ng scripts ay TAPOS NA!** ✅ Kailangan mo na lang i-setup sa Unity Editor.

---

## 📁 MGA FILES NA GINAWA (ALL SCRIPTS CREATED)

### ✅ **1. Room08_Dialogues.cs**
- Lahat ng dialogues (1-2 sentences)
- Entry sequence, evidence, mirror confrontation, escape
- **Location:** `Assets/Scripts/Puzzle/Room 08/Room08_Dialogues.cs`

### ✅ **2. Room08_FlowController.cs**
- Main progression controller
- Tracks evidence, mirror progress
- Handles intro sequence, Emily humming, scene transition
- **Location:** `Assets/Scripts/Puzzle/Room 08/Room08_FlowController.cs`

### ✅ **3. Room08_Interactable.cs**
- Handles all object interactions
- Bathtub, Medicine Cabinet, Evidence, Mirror, Door, Passage
- **Location:** `Assets/Scripts/Puzzle/Room 08/Room08_Interactable.cs`

### ✅ **4. Room08_MirrorQTE.cs**
- Complete QTE system
- 5 taps, decreasing time (2.0s to 0.8s)
- 3 failures = game over
- Progressive cracks, camera shake, audio
- **Location:** `Assets/Scripts/Puzzle/Room 08/Room08_MirrorQTE.cs`

---

## 🎯 FLOW SEQUENCE

```
1. ENTRY → Lisa enters, door locks, Emily outside humming
2. EXAMINE EVIDENCE → Bathtub, Medicine Cabinet, Evidence items (3 items)
3. EXAMINE MIRROR → First time: Emily confrontation dialogue (long sequence)
4. BREAK MIRROR (QTE) → 5 taps, decreasing time, 3 failures = game over
5. ESCAPE → Climb through passage to Master Bathroom
```

---

## 🎨 UNITY SETUP - STEP BY STEP

### **STEP 1: Create Room08_FlowController**

1. **Create Empty GameObject:**
   - Name: `Room08_FlowController`
   - Position: (0, 0, 0)

2. **Add Component:**
   - Add `Room08_FlowController.cs`

3. **Assign References:**
   - **Emily AI:** (Optional) GameObject for Emily (outside, not visible)
   - **Emily Humming Sound:** AudioClip ng humming sound
   - **Emily Audio Source:** AudioSource component (create if needed)
   - **Bathroom Door:** GameObject ng door
   - **Next Scene Name:** `"Room09_Master's_Bathroom"`

4. **Audio Source Settings:**
   - Loop: `true`
   - Play On Awake: `false`
   - Volume: `0.3-0.5`
   - Spatial Blend: `0` (2D sound)

---

### **STEP 2: Create Interactable Objects**

Gumawa ng mga GameObjects with `Room08_Interactable.cs`:

#### **A. Bathtub**
1. Create GameObject: `Bathtub`
2. Add Component: `Room08_Interactable.cs`
3. Settings:
   - **Object Type:** `Bathtub`
4. Add Collider2D (trigger)
5. Position sa scene

#### **B. Medicine Cabinet**
1. Create GameObject: `MedicineCabinet`
2. Add Component: `Room08_Interactable.cs`
3. Settings:
   - **Object Type:** `MedicineCabinet`
4. Add Collider2D (trigger)
5. Position sa scene

#### **C. Mirror** (Main mirror for QTE)
1. Create GameObject: `Mirror`
2. Add Component: `Room08_Interactable.cs`
3. Settings:
   - **Object Type:** `Mirror`
4. Add Collider2D (trigger)
5. Add SpriteRenderer (mirror sprite)
6. Position sa scene

#### **D. Door**
1. Create GameObject: `Door`
2. Add Component: `Room08_Interactable.cs`
3. Settings:
   - **Object Type:** `Door`
4. Add Collider2D (trigger)
5. Position sa scene

#### **E. Passage** (Behind mirror, initially hidden)
1. Create GameObject: `Passage`
2. Add Component: `Room08_Interactable.cs`
3. Settings:
   - **Object Type:** `Passage`
4. Add Collider2D (trigger)
5. **IMPORTANT:** Set `Active` to `false` (initially hidden)
6. Position behind mirror

---

### **STEP 3: Create Evidence Objects**

Gumawa ng 3 evidence items:

#### **A. Bandages**
1. Create GameObject: `Bandages`
2. Add Component: `Room08_Interactable.cs`
3. Settings:
   - **Object Type:** `Evidence`
   - **Evidence ID:** `"bandages"`
4. Add Collider2D (trigger)
5. Add SpriteRenderer (bandages sprite)
6. Position near bathtub/sink

#### **B. Torn Clothes**
1. Create GameObject: `TornClothes`
2. Add Component: `Room08_Interactable.cs`
3. Settings:
   - **Object Type:** `Evidence`
   - **Evidence ID:** `"torn_clothes"`
4. Add Collider2D (trigger)
5. Add SpriteRenderer (torn clothes sprite)
6. Position near bathtub

#### **C. Apology Note**
1. Create GameObject: `ApologyNote`
2. Add Component: `Room08_Interactable.cs`
3. Settings:
   - **Object Type:** `Evidence`
   - **Evidence ID:** `"apology_note"`
4. Add Collider2D (trigger)
5. Add SpriteRenderer (note sprite)
6. Position in/near medicine cabinet

---

### **STEP 4: Create QTE Panel (UI)**

#### **A. QTE Panel Hierarchy**
```
Canvas
└── QTE_Panel (Panel)
    ├── Mirror_Image (Image) - Shows mirror with cracks
    ├── Tap_Target_Parent (Empty RectTransform) - Where targets spawn
    ├── Timer_Text (Text) - Shows countdown
    ├── Progress_Text (Text) - Shows "3/5"
    └── Shatter_Effect (Particle System) - Initially inactive
```

#### **B. Create QTE_Panel**
1. Right-click Canvas → UI → Panel
2. Name: `QTE_Panel`
3. Settings:
   - Anchors: Stretch to fill (full screen)
   - Color: Black with alpha 0.8 (semi-transparent)
   - **IMPORTANT:** Set `Active` to `false` (initially hidden)

#### **C. Create Mirror_Image**
1. Right-click QTE_Panel → UI → Image
2. Name: `Mirror_Image`
3. Settings:
   - Anchors: Center
   - Size: 400x600 (adjust as needed)
   - Assign initial mirror sprite (normal, no cracks)

#### **D. Create Tap_Target_Parent**
1. Right-click QTE_Panel → Create Empty
2. Name: `Tap_Target_Parent`
3. Settings:
   - Anchors: Center
   - Size: 400x300 (tap area)
   - This is where tap targets will spawn

#### **E. Create Timer_Text**
1. Right-click QTE_Panel → UI → Text (or TextMeshProUGUI)
2. Name: `Timer_Text`
3. Settings:
   - Font Size: 48
   - Alignment: Center
   - Position: Top center of panel
   - Text: "2.00"
   - Color: White

#### **F. Create Progress_Text**
1. Right-click QTE_Panel → UI → Text (or TextMeshProUGUI)
2. Name: `Progress_Text`
3. Settings:
   - Font Size: 36
   - Alignment: Center
   - Position: Bottom center of panel
   - Text: "1/5"
   - Color: White

#### **G. Create Shatter_Effect** (Optional)
1. Right-click QTE_Panel → Effects → Particle System
2. Name: `Shatter_Effect`
3. Settings:
   - Configure glass shatter particles
   - **IMPORTANT:** Set `Active` to `false` (initially hidden)

---

### **STEP 5: Create Tap Target Prefab**

#### **A. Create TapTarget**
1. Create UI → Image
2. Name: `TapTarget`
3. Settings:
   - Size: 100x100
   - Sprite: Circle sprite (white circle)
   - Color: White with alpha 0.8

#### **B. Add Button Component**
1. Add Component: `Button`
2. Settings:
   - Transition: Color Tint
   - Normal Color: White
   - Highlighted Color: Yellow
   - Pressed Color: Green

#### **C. Optional: Add Animation**
- Add pulsing/scaling animation
- Makes target more visible

#### **D. Create Prefab**
1. Drag `TapTarget` to `Assets/Prefabs/UI/`
2. Delete from scene (it will be spawned by script)

---

### **STEP 6: Setup Room08_MirrorQTE**

1. **Create Empty GameObject:**
   - Name: `Room08_MirrorQTE`
   - Position: (0, 0, 0)

2. **Add Component:**
   - Add `Room08_MirrorQTE.cs`

3. **Assign QTE Settings:**
   - **Total Taps:** `15`
   - **Total Time Limit:** `120` (2 minutes)
   - **Tap Time Window:** `3.0` (3 seconds per tap)
   - **Max Failures:** `3`

4. **Assign UI References:**
   - **QTE Panel:** Drag `QTE_Panel` from Canvas
   - **Tap Target Prefab:** Drag `TapTarget` prefab
   - **Tap Target Parent:** Drag `Tap_Target_Parent` from QTE_Panel
   - **Timer Text:** Drag `Timer_Text` from QTE_Panel
   - **Progress Text:** Drag `Progress_Text` from QTE_Panel

5. **Assign Visual Effects:**
   - **Mirror Image:** Drag `Mirror_Image` from QTE_Panel
   - **Mirror Phase 1:** Clean mirror sprite
   - **Mirror Phase 2:** First cracks sprite
   - **Mirror Phase 3:** More cracks sprite
   - **Mirror Phase 4:** Almost shattered sprite
   - **Shatter Effect:** Drag `Shatter_Effect` from QTE_Panel (optional)

6. **Assign Audio:**
   - **Tap Sound:** Click/tap sound
   - **Crack Sound:** Glass crack sound
   - **Shatter Sound:** Glass shatter sound
   - **Fail Sound:** Error/fail sound
   - **Glass Stress Sounds:** Array of 5 escalating stress sounds

7. **Camera Shake Settings:**
   - **Shake Intensity:** `0.1`
   - **Shake Duration:** `0.2`

---

### **STEP 7: Create Mirror Crack Sprites**

Kailangan mo ng 4 mirror phase sprites:

1. **Mirror_Phase_1** - Clean mirror (0-3 taps, 0-25%)
2. **Mirror_Phase_2** - First cracks (4-7 taps, 25-50%)
3. **Mirror_Phase_3** - More cracks (8-11 taps, 50-75%)
4. **Mirror_Phase_4** - Almost shattered (12-15 taps, 75-100%)

**How to create:**
- Use image editing software (Photoshop, GIMP, etc.)
- Start with clean mirror sprite
- Add progressive cracks for each phase
- Export as PNG with transparency

**Assign to Room08_MirrorQTE:**
- Drag Mirror_Phase_1 to `Mirror Phase 1` field
- Drag Mirror_Phase_2 to `Mirror Phase 2` field
- Drag Mirror_Phase_3 to `Mirror Phase 3` field
- Drag Mirror_Phase_4 to `Mirror Phase 4` field

---

### **STEP 8: Setup Audio**

#### **A. Emily Humming (Ambient)**
- Already assigned in Room08_FlowController
- Loop: `true`
- Volume: `0.3-0.5`

#### **B. QTE Sounds**
Prepare these audio clips:

1. **Tap Sound** - Click/tap sound (short)
2. **Crack Sound** - Glass crack sound (medium)
3. **Shatter Sound** - Glass shatter sound (loud)
4. **Fail Sound** - Error beep (short)
5. **Glass Stress Sounds** - 5 clips, escalating tension:
   - Stress_1: Light creaking
   - Stress_2: Medium creaking
   - Stress_3: Louder creaking
   - Stress_4: Very loud creaking
   - Stress_5: Almost breaking

**Assign to Room08_MirrorQTE:**
- Drag each audio clip to corresponding field

---

## 🎮 TESTING CHECKLIST

### **Test 1: Entry Sequence (30 seconds)**
- [ ] Enter Room 08
- [ ] Intro dialogue plays (4 parts)
- [ ] Emily humming sound plays
- [ ] Door is locked
- [ ] Player can move after intro

### **Test 2: Evidence Examination (1 minute)**
- [ ] Interact with bathtub → Dialogue shows
- [ ] Interact with medicine cabinet → Dialogue shows
- [ ] Interact with bandages → Dialogue shows, object disappears
- [ ] Interact with torn clothes → Dialogue shows, object disappears
- [ ] Interact with apology note → Dialogue shows, object disappears

### **Test 3: Mirror Examination (2 minutes)**
- [ ] Try mirror before evidence → "Need evidence" message
- [ ] Complete all evidence
- [ ] Interact with mirror → Long confrontation sequence
- [ ] All Emily dialogue shows
- [ ] Prompt to break mirror appears

### **Test 4: Mirror QTE (2 minutes)**
- [ ] Interact with mirror again → QTE starts
- [ ] QTE panel shows
- [ ] Total timer shows 2:00
- [ ] Tap targets appear at random positions
- [ ] Per-tap timer counts down (3 seconds)
- [ ] Successful tap → Mirror phase changes, sound plays
- [ ] Failed tap → Failure count increases, "MISS!" shows
- [ ] Complete 15 taps → Mirror shatters
- [ ] Passage revealed

### **Test 5: Escape (30 seconds)**
- [ ] Interact with passage → Climb through dialogue
- [ ] Scene transitions to Master Bathroom
- [ ] Progress saved

### **Test 6: QTE Failure (1 minute)**
- [ ] Start QTE
- [ ] Miss 3 taps intentionally → Game over
- [ ] OR wait 2 minutes → Game over
- [ ] Game over sequence triggers
- [ ] Returns to checkpoint

---

## 🐛 COMMON ISSUES & FIXES

### **Issue 1: QTE Panel Not Showing**
**Symptoms:** QTE doesn't start, panel stays hidden

**Fix:**
- Check `QTE_Panel` is assigned in `Room08_MirrorQTE`
- Check panel is child of Canvas
- Check Canvas has `GraphicRaycaster` component
- Check panel is initially `SetActive(false)`

### **Issue 2: Tap Targets Not Clickable**
**Symptoms:** Can't click tap targets

**Fix:**
- Check `TapTarget` prefab has `Button` component
- Check `EventSystem` exists in scene
- Check `Tap_Target_Parent` is assigned
- Check Canvas has `GraphicRaycaster`

### **Issue 3: Mirror Not Breaking**
**Symptoms:** QTE completes but mirror doesn't shatter

**Fix:**
- Check all 5 crack sprites are assigned
- Check `Mirror_Image` is assigned
- Check `OnTapSuccess()` is being called
- Check `OnMirrorBroken()` in FlowController

### **Issue 4: Emily Humming Not Playing**
**Symptoms:** No humming sound

**Fix:**
- Check `Emily Audio Source` is assigned
- Check humming clip is assigned
- Check `Play On Awake` is `false`
- Check volume is not 0
- Check audio is not muted

### **Issue 5: Passage Not Appearing**
**Symptoms:** Mirror breaks but no passage

**Fix:**
- Check `Passage` GameObject exists
- Check it's initially `SetActive(false)`
- Check `OnMirrorBroken()` activates it
- Check passage has collider

### **Issue 6: Player Can Move During Dialogues**
**Symptoms:** Player moves between dialogues

**Fix:**
- Check `ShowDialogueSequence()` disables player at START
- Check player is re-enabled at END
- Check no delays between dialogues

### **Issue 7: QTE Timer Not Showing**
**Symptoms:** Timer text doesn't update

**Fix:**
- Check `Timer_Text` is assigned
- Check text component exists
- Check `WaitForTap()` coroutine is running

---

## 📊 PROGRESSION FLAGS

Save these flags in SaveSystem:

```csharp
// Evidence found
"bathroom_evidence_found"

// Mirror examined
"bathroom_mirror_examined"

// Mirror broken
"bathroom_mirror_broken"

// QTE completed
"bathroom_mirror_qte"

// Escaped to Master Bathroom
"bathroom_escaped"
```

---

## 🎨 VISUAL EFFECTS

### **Mirror Crack Animation**
- 5 progressive crack sprites
- Each successful tap shows next crack
- Final tap triggers shatter effect

### **Camera Shake**
- Small shake on each tap (0.1 intensity)
- Big shake on mirror shatter (0.3 intensity)
- Intensity increases with each tap

### **Particle Effects**
- Glass shatter particles on final break
- Optional: Dust particles when passage revealed

---

## 🔊 AUDIO DESIGN

### **Ambient**
- Emily humming (looping, outside door)
- Bathroom ambience (water drips, etc.)

### **QTE Sounds**
- **Tap:** Click sound (short)
- **Crack:** Glass crack (gets louder each time)
- **Stress:** Escalating tension sounds (5 stages)
- **Shatter:** Big glass break (loud)
- **Fail:** Error beep (short)

### **Music**
- Tense music during QTE
- Silence after mirror breaks
- Calm music during escape

---

## 💡 TIPS & BEST PRACTICES

### **1. QTE Difficulty**
- Adjust `startingTime` and `minimumTime` for difficulty
- 3 failures = game over (adjust `maxFailures`)
- Test with different players to find sweet spot

### **2. Dialogue Pacing**
- All dialogues are 1-2 sentences
- Player stops during dialogue sequences
- No delays between dialogues
- Player re-enabled at END of sequence

### **3. Evidence Order**
- Player can examine in any order
- All 3 must be found before mirror
- Objects disappear after examination

### **4. Mirror Confrontation**
- Long emotional sequence (11 dialogues)
- Player cannot skip
- Important story moment
- Player stops during entire sequence

### **5. QTE Feedback**
- **Visual:** Cracks appear progressively
- **Audio:** Escalating stress sounds
- **Haptic:** Camera shake increases
- **UI:** Timer changes color (white → yellow → red)

### **6. Testing**
- Test each sequence separately
- Test QTE success and failure paths
- Test with different timing settings
- Test audio levels

---

## 🌟 SUMMARY

### **Objects Needed:**
- ✅ 1 FlowController (Room08_FlowController)
- ✅ 5 Interactable objects (Bathtub, Medicine, Mirror, Door, Passage)
- ✅ 3 Evidence items (Bandages, Clothes, Note)
- ✅ 1 QTE system (Room08_MirrorQTE)
- ✅ 1 QTE Panel (UI)
- ✅ 1 Tap Target prefab

### **Scripts:**
- ✅ Room08_Dialogues.cs
- ✅ Room08_FlowController.cs
- ✅ Room08_Interactable.cs
- ✅ Room08_MirrorQTE.cs

### **Audio:**
- Emily humming (loop)
- 5 QTE sounds (tap, crack, shatter, fail, stress)

### **Sprites:**
- 6 mirror sprites (normal + 5 cracks)
- Tap target sprite (circle)
- Evidence sprites (bandages, clothes, note)

---

## 📝 QUICK START CHECKLIST

Para mabilis na ma-implement:

1. [ ] Copy all 4 scripts to `Assets/Scripts/Puzzle/Room 08/`
2. [ ] Create Room08_FlowController GameObject
3. [ ] Create 5 Interactable objects (Bathtub, Medicine, Mirror, Door, Passage)
4. [ ] Create 3 Evidence objects (Bandages, Clothes, Note)
5. [ ] Create QTE Panel (UI) with all children
6. [ ] Create TapTarget prefab
7. [ ] Create Room08_MirrorQTE GameObject
8. [ ] Assign all references in Inspector
9. [ ] Create 6 mirror sprites (normal + 5 cracks)
10. [ ] Add all audio clips
11. [ ] Test each sequence
12. [ ] Adjust timing/difficulty as needed

---

**READY TO IMPLEMENT!** 🎮✨

Sundin lang yung guide na to step-by-step, tapos test mo each part! Kung may tanong, just ask! 💖

**IMPORTANT NOTES:**
- Player STOPS at START of dialogue sequence
- NO DELAYS between dialogues
- Player RE-ENABLED at END of sequence
- QTE uses 5 taps with decreasing time (2.0s to 0.8s)
- 3 failures = game over
- All evidence must be examined before mirror
