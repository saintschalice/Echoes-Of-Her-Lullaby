# 🛁 ROOM 08 - LISA'S BATHROOM - COMPLETE SETUP GUIDE

## 📋 OVERVIEW

Room 08 ay yung bathroom kung saan naka-lock si Lisa. Emily is outside, humming. Lisa needs to examine evidence, confront Emily through the mirror, break the mirror via QTE, then escape to Master Bathroom.

---

## 🎯 FLOW SEQUENCE

```
1. ENTRY → Lisa enters, door locks, Emily outside humming
2. EXAMINE EVIDENCE → Bathtub, Medicine Cabinet, Evidence items
3. EXAMINE MIRROR → First time: Emily confrontation dialogue
4. BREAK MIRROR (QTE) → 5 taps, decreasing time, 3 failures = game over
5. ESCAPE → Climb through passage to Master Bathroom
```

---

## 📁 FILES CREATED

1. ✅ `Room08_Dialogues.cs` - All dialogues (short, 1-2 sentences)
2. ✅ `Room08_FlowController.cs` - Main progression controller
3. ✅ `Room08_Interactable.cs` - Object interactions
4. ✅ `Room08_MirrorQTE.cs` - Mirror breaking QTE

---

## 🎨 UNITY SETUP

### **STEP 1: Create Scene Objects**

#### **A. Room08_FlowController**
1. Create Empty GameObject: `Room08_FlowController`
2. Add Component: `Room08_FlowController.cs`
3. Assign:
   - Emily AI GameObject (optional, for humming audio)
   - Emily Humming Sound (AudioClip)
   - Emily Audio Source (AudioSource component)
   - Bathroom Door GameObject
   - Next Scene Name: `"Room09_Master's_Bathroom"`

#### **B. Interactable Objects**
Create these GameObjects with `Room08_Interactable.cs`:

1. **Bathtub**
   - Object Type: `Bathtub`
   - Add Collider2D (trigger)

2. **Medicine Cabinet**
   - Object Type: `MedicineCabinet`
   - Add Collider2D (trigger)

3. **Mirror**
   - Object Type: `Mirror`
   - Add Collider2D (trigger)
   - This is the main mirror for QTE

4. **Door**
   - Object Type: `Door`
   - Add Collider2D (trigger)

5. **Passage** (behind mirror, initially hidden)
   - Object Type: `Passage`
   - Add Collider2D (trigger)
   - Initially: `SetActive(false)`
   - Activate after mirror breaks

#### **C. Evidence Objects**
Create 3 evidence items with `Room08_Interactable.cs`:

1. **Bandages**
   - Object Type: `Evidence`
   - Evidence ID: `"bandages"`
   - Place near bathtub/sink

2. **Torn Clothes**
   - Object Type: `Evidence`
   - Evidence ID: `"torn_clothes"`
   - Place near bathtub

3. **Apology Note**
   - Object Type: `Evidence`
   - Evidence ID: `"apology_note"`
   - Place in/near medicine cabinet

---

### **STEP 2: Create QTE Panel**

#### **A. QTE Panel Hierarchy**
```
Canvas
└── QTE_Panel (Panel)
    ├── Mirror_Image (Image) - Shows mirror with cracks
    ├── Tap_Target_Parent (Empty) - Where targets spawn
    ├── Timer_Text (Text) - Shows countdown
    ├── Progress_Text (Text) - Shows "3/5"
    └── Shatter_Effect (Particle System) - Initially inactive
```

#### **B. QTE Panel Setup**
1. Create Panel: `QTE_Panel`
   - Full screen (stretch to fill)
   - Initially: `SetActive(false)`

2. **Mirror_Image**
   - Image component
   - Assign initial mirror sprite
   - Anchors: Center
   - Size: 400x600 (adjust as needed)

3. **Tap_Target_Parent**
   - Empty RectTransform
   - Anchors: Center
   - Size: 400x300 (tap area)

4. **Timer_Text**
   - TextMeshProUGUI or Text
   - Font Size: 48
   - Alignment: Center
   - Position: Top center
   - Text: "2.00"

5. **Progress_Text**
   - TextMeshProUGUI or Text
   - Font Size: 36
   - Alignment: Center
   - Position: Bottom center
   - Text: "1/5"

6. **Shatter_Effect**
   - Particle System
   - Initially: `SetActive(false)`
   - Configure: Glass shatter particles

#### **C. Tap Target Prefab**
1. Create Prefab: `TapTarget`
   - Image (Circle sprite)
   - Button component
   - Size: 100x100
   - Color: White with alpha
   - Optional: Pulsing animation

---

### **STEP 3: Setup Room08_MirrorQTE**

1. Create Empty GameObject: `Room08_MirrorQTE`
2. Add Component: `Room08_MirrorQTE.cs`
3. Assign:
   - **QTE Settings:**
     - Total Taps: `5`
     - Starting Time: `2.0`
     - Minimum Time: `0.8`
     - Max Failures: `3`
   
   - **UI References:**
     - QTE Panel: `QTE_Panel`
     - Tap Target Prefab: `TapTarget` prefab
     - Tap Target Parent: `Tap_Target_Parent`
     - Timer Text: `Timer_Text`
     - Progress Text: `Progress_Text`
   
   - **Visual Effects:**
     - Mirror Image: `Mirror_Image`
     - Crack Sprites: Array of 5 sprites (progressive cracks)
     - Shatter Effect: `Shatter_Effect`
   
   - **Audio:**
     - Tap Sound: Click/tap sound
     - Crack Sound: Glass crack sound
     - Shatter Sound: Glass shatter sound
     - Fail Sound: Error/fail sound
     - Glass Stress Sounds: Array of 5 escalating stress sounds
   
   - **Camera Shake:**
     - Shake Intensity: `0.1`
     - Shake Duration: `0.2`

---

### **STEP 4: Setup Audio**

#### **A. Emily Humming (Ambient)**
1. Create AudioSource on `Room08_FlowController`
2. Assign humming audio clip
3. Settings:
   - Loop: `true`
   - Play On Awake: `false`
   - Volume: `0.3-0.5`
   - Spatial Blend: `0` (2D)

#### **B. QTE Sounds**
Prepare these audio clips:
- Tap sound (click)
- Crack sound (glass crack)
- Shatter sound (glass break)
- Fail sound (error beep)
- Glass stress sounds (5 clips, escalating tension)

---

### **STEP 5: Setup Mirror Sprites**

Create 6 mirror sprites:
1. **Mirror_Normal** - Clean mirror
2. **Mirror_Crack_1** - First crack
3. **Mirror_Crack_2** - More cracks
4. **Mirror_Crack_3** - Even more cracks
5. **Mirror_Crack_4** - Almost shattered
6. **Mirror_Crack_5** - Heavily cracked

Assign these to `Crack Sprites` array in `Room08_MirrorQTE`

---

## 🎮 TESTING CHECKLIST

### **Test 1: Entry Sequence (30 seconds)**
1. ✅ Enter Room 08
2. ✅ Intro dialogue plays (4 parts)
3. ✅ Emily humming sound plays
4. ✅ Door is locked
5. ✅ Player can move after intro

### **Test 2: Evidence Examination (1 minute)**
1. ✅ Interact with bathtub → Dialogue shows
2. ✅ Interact with medicine cabinet → Dialogue shows
3. ✅ Interact with bandages → Dialogue shows, object disappears
4. ✅ Interact with torn clothes → Dialogue shows, object disappears
5. ✅ Interact with apology note → Dialogue shows, object disappears

### **Test 3: Mirror Examination (2 minutes)**
1. ✅ Try mirror before evidence → "Need evidence" message
2. ✅ Complete all evidence
3. ✅ Interact with mirror → Long confrontation sequence
4. ✅ All Emily dialogue shows
5. ✅ Prompt to break mirror appears

### **Test 4: Mirror QTE (2 minutes)**
1. ✅ Interact with mirror again → QTE starts
2. ✅ QTE panel shows
3. ✅ Tap targets appear
4. ✅ Timer counts down
5. ✅ Successful tap → Crack appears, sound plays
6. ✅ Failed tap → Failure count increases
7. ✅ Complete 5 taps → Mirror shatters
8. ✅ Passage revealed

### **Test 5: Escape (30 seconds)**
1. ✅ Interact with passage → Climb through dialogue
2. ✅ Scene transitions to Master Bathroom
3. ✅ Progress saved

### **Test 6: QTE Failure (1 minute)**
1. ✅ Start QTE
2. ✅ Miss 3 taps intentionally
3. ✅ Game over sequence triggers
4. ✅ Returns to checkpoint

---

## 🐛 COMMON ISSUES & FIXES

### **Issue 1: QTE Panel Not Showing**
**Fix:** 
- Check `QTE_Panel` is assigned in `Room08_MirrorQTE`
- Check panel is child of Canvas
- Check Canvas has `GraphicRaycaster`

### **Issue 2: Tap Targets Not Clickable**
**Fix:**
- Check `TapTarget` prefab has `Button` component
- Check `EventSystem` exists in scene
- Check `Tap_Target_Parent` is assigned

### **Issue 3: Mirror Not Breaking**
**Fix:**
- Check all 5 crack sprites are assigned
- Check `Mirror_Image` is assigned
- Check `OnTapSuccess()` is being called

### **Issue 4: Emily Humming Not Playing**
**Fix:**
- Check `Emily Audio Source` is assigned
- Check humming clip is assigned
- Check `Play On Awake` is `false`
- Check volume is not 0

### **Issue 5: Passage Not Appearing**
**Fix:**
- Check `Passage` GameObject exists
- Check it's initially `SetActive(false)`
- Check `OnMirrorBroken()` activates it

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
- Small shake on each tap
- Big shake on mirror shatter
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
- Tap: Click sound
- Crack: Glass crack (gets louder each time)
- Stress: Escalating tension sounds
- Shatter: Big glass break
- Fail: Error beep

### **Music**
- Tense music during QTE
- Silence after mirror breaks
- Calm music during escape

---

## 💡 TIPS

1. **QTE Difficulty:**
   - Adjust `startingTime` and `minimumTime` for difficulty
   - 3 failures = game over (adjust `maxFailures`)

2. **Dialogue Pacing:**
   - All dialogues are 1-2 sentences
   - Player stops during dialogue sequences
   - No delays between dialogues

3. **Evidence Order:**
   - Player can examine in any order
   - All 3 must be found before mirror

4. **Mirror Confrontation:**
   - Long emotional sequence
   - Player cannot skip
   - Important story moment

5. **QTE Feedback:**
   - Visual: Cracks appear
   - Audio: Escalating sounds
   - Haptic: Camera shake

---

## 🌟 SUMMARY

**Objects Needed:**
- 1 FlowController
- 5 Interactable objects (Bathtub, Medicine, Mirror, Door, Passage)
- 3 Evidence items (Bandages, Clothes, Note)
- 1 QTE system
- 1 QTE Panel (UI)
- 1 Tap Target prefab

**Scripts:**
- Room08_Dialogues.cs ✅
- Room08_FlowController.cs ✅
- Room08_Interactable.cs ✅
- Room08_MirrorQTE.cs ✅

**Audio:**
- Emily humming (loop)
- 5 QTE sounds (tap, crack, shatter, fail, stress)

**Sprites:**
- 6 mirror sprites (normal + 5 cracks)
- Tap target sprite (circle)

---

**READY TO IMPLEMENT!** 🎮✨

Sundin lang yung guide na to step-by-step, tapos test mo each part! 💖
