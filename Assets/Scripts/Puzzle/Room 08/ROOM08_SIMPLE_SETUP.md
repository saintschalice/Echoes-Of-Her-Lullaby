# Room 08 - Simple Setup (Like Room 06 Photo Frame)

## 🎯 COMPLETE FLOW

1. Collect 2 evidence items (torn dress + note)
2. Get hammer from cabinet
3. Interact with bathtub
4. **Interact with mirror** → Panel appears
5. **Tap puzzle** (15 taps, 25 seconds)
6. **Panel auto-closes** → World mirror changes to broken
7. **Passage appears** → Interact to go to Room 09

---

## 📋 STEP-BY-STEP UNITY SETUP

### STEP 1: Create Evidence Items (2)

#### Evidence 1: Torn Dress
```
1. Create GameObject: "Evidence_TornDress"
2. Add SpriteRenderer (torn dress sprite)
3. Add BoxCollider2D
   - Is Trigger: ☑ CHECKED
4. Add Script: Room08_EvidencePickup
   - Evidence Id: "torn_dress"
   - Evidence Name: "Torn Dress"
   - Evidence Description: "A torn and bloodied dress. Someone was hurt here."
   - Auto Pickup: ☑
   - Pickup Sound: [assign sound]
```

#### Evidence 2: Note
```
1. Create GameObject: "Evidence_Note"
2. Add SpriteRenderer (note sprite)
3. Add BoxCollider2D
   - Is Trigger: ☑ CHECKED
4. Add Script: Room08_EvidencePickup
   - Evidence Id: "apology_note"
   - Evidence Name: "Apology Note"
   - Evidence Description: "A crumpled note. 'I'm sorry... I didn't mean to...'"
   - Auto Pickup: ☑
   - Pickup Sound: [assign sound]
```

---

### STEP 2: Create Cabinet

```
1. Create GameObject: "MedicineCabinet"
2. Add SpriteRenderer (cabinet sprite)
3. Add BoxCollider2D
   - Is Trigger: ☑ CHECKED
4. Add Script: Room08_Interactable
   - Object Type: MedicineCabinet
```

---

### STEP 3: Create Bathtub

```
1. Create GameObject: "Bathtub"
2. Add SpriteRenderer (bathtub sprite)
3. Add BoxCollider2D
   - Is Trigger: ☑ CHECKED
4. Add Script: Room08_Interactable
   - Object Type: Bathtub
```

---

### STEP 4: Create Mirror (World Object)

```
1. Create GameObject: "Mirror"
2. Add SpriteRenderer
   - Sprite: Normal mirror sprite
   - Draw Mode: Simple
3. Add BoxCollider2D
   - Is Trigger: ☑ CHECKED
4. Add Script: Room08_Interactable
   - Object Type: Mirror
5. Transform:
   - Scale: (1, 1, 1) ← IMPORTANT!
```

---

### STEP 5: Create Passage (Hidden Initially)

```
1. Create GameObject: "Passage"
2. SetActive: ☐ UNCHECKED (hidden at start)
3. Add SpriteRenderer (optional - passage sprite)
4. Add BoxCollider2D
   - Is Trigger: ☑ CHECKED
5. Add Script: Room08_Interactable
   - Object Type: Passage
```

---

### STEP 6: Create Mirror Panel (UI)

#### Panel Structure:
```
Canvas
└─ MirrorPanel
    ├─ SetActive: ☐ UNCHECKED (hidden at start)
    ├─ Panel (Image) - Black background
    │   └─ Color: (0, 0, 0, 200) - Black with alpha
    │
    ├─ TapArea (Image) - Tap button
    │   ├─ Anchor: Center
    │   ├─ Size: 800x600 (or full screen)
    │   ├─ Color: (204, 51, 51, 128) - Red-ish
    │   └─ Add Button component (at runtime by script)
    │
    ├─ FillImage (Image, child of TapArea)
    │   ├─ Image Type: Filled
    │   ├─ Fill Method: Horizontal (Left to Right)
    │   ├─ Fill Amount: 0 (will animate to 1)
    │   └─ Color: (204, 51, 51, 128) - Same as TapArea
    │
    ├─ MirrorImage (Image) - Shows mirror
    │   ├─ Anchor: Center
    │   ├─ Size: 400x600 (adjust to your sprite)
    │   ├─ Sprite: Normal mirror sprite
    │   ├─ Image Type: Simple
    │   └─ Preserve Aspect: ☑ CHECKED
    │
    ├─ TimerText (TextMeshPro)
    │   ├─ Position: Top center
    │   ├─ Text: "25.0s"
    │   ├─ Font Size: 48
    │   └─ Alignment: Center
    │
    ├─ ProgressText (TextMeshPro)
    │   ├─ Position: Bottom center
    │   ├─ Text: "0/15"
    │   ├─ Font Size: 36
    │   └─ Alignment: Center
    │
    └─ Room08_MirrorQTE (Script)
        └─ [Configure below]
```

#### Configure Room08_MirrorQTE:
```
Room08_MirrorQTE (on MirrorPanel):
├─ Total Taps: 15
├─ Total Time Limit: 25
├─ Full Screen Tap Area: [drag TapArea Image]
├─ Fill Image: [drag FillImage]
├─ Fill Color: (0.8, 0.2, 0.2, 0.5)
├─ Timer Text TMP: [drag TimerText]
├─ Progress Text TMP: [drag ProgressText]
├─ Mirror Image: [drag MirrorImage]
├─ Mirror Phase 1: [normal mirror sprite]
├─ Mirror Phase 2: [cracked mirror sprite]
├─ Mirror Phase 3: [more cracks sprite]
├─ Mirror Phase 4: [almost broken sprite]
├─ Tap Sound: [assign tap sound]
├─ Crack Sound: [assign crack sound]
├─ Shatter Sound: [assign shatter sound]
├─ Shatter Effect: [optional particle effect]
├─ Shake Intensity: 0.1
└─ Shake Duration: 0.2
```

---

### STEP 7: Create Room08UIManager

```
1. Create Empty GameObject: "Room08UIManager"
2. Add Script: Room08UIManager
3. Configure:
   - Mirror Panel: [drag MirrorPanel from Canvas]
```

---

### STEP 8: Create Room08_FlowController

```
1. Create Empty GameObject: "Room08_FlowController"
2. Add Script: Room08_FlowController
3. Configure:
   ├─ Total Evidence Items: 2
   ├─ Mirror Sprite Renderer: [drag Mirror's SpriteRenderer]
   ├─ Mirror Normal Sprite: [normal mirror sprite]
   ├─ Mirror Broken Sprite: [broken mirror sprite]
   ├─ Passage Object: [drag Passage GameObject]
   └─ Next Scene Name: "Room09_Master's_Bathroom"
```

---

## 🎨 VISUAL SETUP

### Colors:

**Tap Area & Fill**:
- Red: `RGBA(204, 51, 51, 128)` or `(0.8, 0.2, 0.2, 0.5)`

**Panel Background**:
- Black: `RGBA(0, 0, 0, 200)` or `(0, 0, 0, 0.78)`

### Sprites Needed:

1. **Evidence**:
   - Torn dress sprite
   - Note sprite

2. **Interactables**:
   - Cabinet sprite
   - Bathtub sprite
   - Mirror sprite (normal)
   - Mirror sprite (broken)
   - Passage sprite (optional)

3. **Mirror Phases** (for panel):
   - Phase 1: Clean mirror
   - Phase 2: First cracks
   - Phase 3: More cracks
   - Phase 4: Almost shattered

### Audio Needed:

1. **Evidence**: Pickup sound (paper rustle, cloth pickup)
2. **Hammer**: Pickup sound (metal clink)
3. **Mirror Puzzle**:
   - Tap sound (glass tap)
   - Crack sound (glass cracking)
   - Shatter sound (glass breaking)

---

## 🔄 INTERACTION ORDER

```
1. Walk around → Collect Evidence_TornDress
   └─ Notification appears: "Torn Dress"
   └─ Click to continue

2. Walk around → Collect Evidence_Note
   └─ Notification appears: "Apology Note"
   └─ Click to continue
   └─ Dialogue: "I've collected all the evidence..."

3. Interact with Cabinet
   └─ Cabinet opens
   └─ Notification appears: "Hammer"
   └─ Click to continue

4. Interact with Bathtub
   └─ Dialogue: "The bathtub... there's dried blood..."

5. Interact with Mirror
   └─ Panel appears (normal mirror)
   └─ Tap 15 times within 25 seconds
   └─ Each tap: sound + fill increases + mirror cracks
   └─ After 15 taps: Shatter sound + big shake
   └─ Panel shows broken mirror briefly
   └─ Panel auto-closes
   └─ World mirror changes to broken sprite
   └─ Passage appears
   └─ Dialogue: "The mirror... it's shattered!"
   └─ Dialogue: "There's a passage behind it..."

6. Interact with Passage
   └─ Fade transition
   └─ Load Room 09 (Master's Bathroom)
```

---

## ✅ QUICK CHECKLIST

### Evidence Items:
- [ ] Evidence_TornDress created with Room08_EvidencePickup
- [ ] Evidence_Note created with Room08_EvidencePickup
- [ ] Both have BoxCollider2D (Is Trigger ✓)
- [ ] Both have pickup sounds

### Interactables:
- [ ] MedicineCabinet with Room08_Interactable (Type: MedicineCabinet)
- [ ] Bathtub with Room08_Interactable (Type: Bathtub)
- [ ] Mirror with Room08_Interactable (Type: Mirror)
- [ ] Passage with Room08_Interactable (Type: Passage)
- [ ] All have BoxCollider2D (Is Trigger ✓)

### Mirror Panel:
- [ ] MirrorPanel created under Canvas
- [ ] TapArea with FillImage
- [ ] MirrorImage with Preserve Aspect ✓
- [ ] TimerText and ProgressText
- [ ] Room08_MirrorQTE configured
- [ ] All sprites assigned (normal + broken + phases)
- [ ] All sounds assigned

### Managers:
- [ ] Room08UIManager created and configured
- [ ] Room08_FlowController created and configured
- [ ] Total Evidence Items = 2
- [ ] Mirror sprites assigned
- [ ] Passage assigned

### Scene Setup:
- [ ] Passage initially hidden (SetActive ☐)
- [ ] MirrorPanel initially hidden (SetActive ☐)
- [ ] Mirror Transform Scale = (1, 1, 1)

---

## 🎯 SCENE HIERARCHY

```
Room08_Lisa'sBathroom (Scene)
├─ Room08_FlowController
│   └─ Room08_FlowController (Script)
│
├─ Room08UIManager
│   └─ Room08UIManager (Script)
│
├─ Evidence_TornDress
│   ├─ SpriteRenderer
│   ├─ BoxCollider2D (Trigger ✓)
│   └─ Room08_EvidencePickup
│
├─ Evidence_Note
│   ├─ SpriteRenderer
│   ├─ BoxCollider2D (Trigger ✓)
│   └─ Room08_EvidencePickup
│
├─ MedicineCabinet
│   ├─ SpriteRenderer
│   ├─ BoxCollider2D (Trigger ✓)
│   └─ Room08_Interactable (Type: MedicineCabinet)
│
├─ Bathtub
│   ├─ SpriteRenderer
│   ├─ BoxCollider2D (Trigger ✓)
│   └─ Room08_Interactable (Type: Bathtub)
│
├─ Mirror
│   ├─ SpriteRenderer (normal sprite)
│   ├─ BoxCollider2D (Trigger ✓)
│   └─ Room08_Interactable (Type: Mirror)
│
├─ Passage (SetActive ☐)
│   ├─ SpriteRenderer (optional)
│   ├─ BoxCollider2D (Trigger ✓)
│   └─ Room08_Interactable (Type: Passage)
│
└─ Canvas
    └─ MirrorPanel (SetActive ☐)
        ├─ Panel (Image)
        ├─ TapArea (Image)
        │   └─ FillImage (Image)
        ├─ MirrorImage (Image)
        ├─ TimerText (TMP)
        ├─ ProgressText (TMP)
        └─ Room08_MirrorQTE (Script)
```

---

## 💡 TIPS

### For Best Results:

1. **Test Each Step**:
   - Test evidence pickup first
   - Then test cabinet → hammer
   - Then test bathtub
   - Finally test mirror puzzle

2. **Sprite Sizes**:
   - Normal mirror and broken mirror should have same Pixels Per Unit
   - Mirror Transform Scale should be (1, 1, 1)
   - Use Preserve Aspect on MirrorImage in panel

3. **Audio**:
   - Use short, satisfying sounds
   - Tap sound: 0.1-0.2s
   - Crack sound: 0.3-0.5s
   - Shatter sound: 1.0-2.0s

4. **Colors**:
   - Red fill creates urgency
   - Black background focuses attention
   - Timer changes color (white → yellow → red)

---

**Setup complete! Test mo na sa Unity!** 🎮✨

