# Room 08 - Complete Designer Setup Guide

## PARA SA DESIGNER/ARTIST

Ito ang step-by-step guide kung paano i-setup ang Room 08 (Lisa's Bathroom) sa Unity.

---

## PART 1: SCENE OBJECTS (GameObjects)

### 1. TORN CLOTHES (Floor Pickup)

**Step 1**: Create GameObject
```
Hierarchy → Right-click → Create Empty
Name: TornClothes
```

**Step 2**: Add Sprite
```
Select TornClothes
Inspector → Add Component → Sprite Renderer
- Sprite: [Drag torn clothes sprite here]
- Sorting Layer: Default
- Order in Layer: 1
```

**Step 3**: Add Collider
```
Inspector → Add Component → Box Collider 2D
- Is Trigger: ✓ (checked)
- Size: Adjust to fit sprite
```

**Step 4**: Add Script
```
Inspector → Add Component → Room08_Interactable
- My Type: Evidence
- Evidence Id: torn_clothes
```

**Position**: Place sa floor ng bathroom (kung saan gusto mo)

---

### 2. APOLOGY NOTE (Floor Pickup)

**Step 1**: Create GameObject
```
Hierarchy → Right-click → Create Empty
Name: ApologyNote
```

**Step 2**: Add Sprite
```
Select ApologyNote
Inspector → Add Component → Sprite Renderer
- Sprite: [Drag note sprite here]
- Sorting Layer: Default
- Order in Layer: 1
```

**Step 3**: Add Collider
```
Inspector → Add Component → Box Collider 2D
- Is Trigger: ✓ (checked)
- Size: Adjust to fit sprite
```

**Step 4**: Add Script
```
Inspector → Add Component → Room08_Interactable
- My Type: Evidence
- Evidence Id: apology_note
```

**Position**: Place sa floor ng bathroom (different location from torn clothes)

---

### 3. MEDICINE CABINET

**Step 1**: Create GameObject
```
Hierarchy → Right-click → Create Empty
Name: MedicineCabinet
```

**Step 2**: Add Sprite
```
Select MedicineCabinet
Inspector → Add Component → Sprite Renderer
- Sprite: [Drag medicine cabinet sprite here]
- Sorting Layer: Default
- Order in Layer: 0
```

**Step 3**: Add Collider
```
Inspector → Add Component → Box Collider 2D
- Is Trigger: ✓ (checked)
- Size: Adjust to fit sprite
```

**Step 4**: Add Script
```
Inspector → Add Component → Room08_Interactable
- My Type: MedicineCabinet
```

**Position**: Place sa wall ng bathroom

---

### 4. MIRROR (Main Puzzle Object)

**Step 1**: Create GameObject
```
Hierarchy → Right-click → Create Empty
Name: Mirror
```

**Step 2**: Add Sprite
```
Select Mirror
Inspector → Add Component → Sprite Renderer
- Sprite: [Drag NORMAL mirror sprite here] ⭐ IMPORTANTE: Normal mirror, hindi broken
- Sorting Layer: Default
- Order in Layer: 0
```

**Step 3**: Add Collider
```
Inspector → Add Component → Box Collider 2D
- Is Trigger: ✓ (checked)
- Size: Adjust to fit sprite
```

**Step 4**: Add Script
```
Inspector → Add Component → Room08_Interactable
- My Type: Mirror
```

**Position**: Place sa wall ng bathroom (main focal point)

**IMPORTANTE**: Kailangan mo ng 2 sprites para sa mirror:
- Normal mirror sprite (before breaking)
- Broken mirror sprite (after QTE)

---

### 5. PASSAGE (Hidden Initially)

**Step 1**: Create GameObject
```
Hierarchy → Right-click → Create Empty
Name: Passage
```

**Step 2**: Add Sprite
```
Select Passage
Inspector → Add Component → Sprite Renderer
- Sprite: [Drag passage/hole sprite here]
- Sorting Layer: Default
- Order in Layer: 2 (above mirror)
```

**Step 3**: Add Collider
```
Inspector → Add Component → Box Collider 2D
- Is Trigger: ✓ (checked)
- Size: Adjust to fit sprite
```

**Step 4**: Add Script
```
Inspector → Add Component → Room08_Interactable
- My Type: Passage
```

**Step 5**: DISABLE GameObject ⭐ IMPORTANTE!
```
Select Passage
Inspector → Top checkbox: ✗ (unchecked)
```

**Position**: Place sa same position ng mirror (behind it)

---

### 6. EMILY AI (Hunting Enemy)

**Step 1**: Create GameObject
```
Hierarchy → Right-click → Create Empty
Name: EmilyAI
```

**Step 2**: Add Sprite
```
Select EmilyAI
Inspector → Add Component → Sprite Renderer
- Sprite: [Drag Emily sprite here]
- Sorting Layer: Default
- Order in Layer: 3
```

**Step 3**: Add NavMesh Agent
```
Inspector → Add Component → Nav Mesh Agent
- Agent Type: Humanoid
- Speed: 3.5
- Angular Speed: 120
- Acceleration: 8
- Stopping Distance: 0.5
- Auto Braking: ✓
```

**Step 4**: Add Emily Script
```
Inspector → Add Component → EmilyGhost
(Configure settings as needed)
```

**Step 5**: DISABLE GameObject ⭐ IMPORTANTE!
```
Select EmilyAI
Inspector → Top checkbox: ✗ (unchecked)
```

**Position**: Anywhere (will be moved to spawn point)

---

### 7. EMILY SPAWN POINT

**Step 1**: Create GameObject
```
Hierarchy → Right-click → Create Empty
Name: EmilySpawnPoint
```

**Step 2**: Position
```
Transform:
- Position: [Where Emily should spawn when she enters]
- Rotation: (0, 0, 0)
- Scale: (1, 1, 1)
```

**Visual Aid**: Add a Gizmo icon
```
Inspector → Click icon at top-left
Select a colored icon (e.g., red circle)
```

---

### 8. DOOR

**Step 1**: Create GameObject
```
Hierarchy → Right-click → Create Empty
Name: BathroomDoor
```

**Step 2**: Add Sprite
```
Select BathroomDoor
Inspector → Add Component → Sprite Renderer
- Sprite: [Drag door sprite here]
- Sorting Layer: Default
- Order in Layer: 0
```

**Step 3**: Add Collider
```
Inspector → Add Component → Box Collider 2D
- Is Trigger: ✓ (checked)
- Size: Adjust to fit sprite
```

**Step 4**: Add Script
```
Inspector → Add Component → Room08_Interactable
- My Type: Door
```

---

## PART 2: ROOM CONTROLLER SETUP

### 1. Create Room Controller

**Step 1**: Create GameObject
```
Hierarchy → Right-click → Create Empty
Name: Room08_Controller
```

**Step 2**: Add Script
```
Inspector → Add Component → Room08_FlowController
```

**Step 3**: Configure Settings

#### Evidence Collection
```
Has Found Torn Clothes: ✗ (unchecked)
Has Found Apology Note: ✗ (unchecked)
Has Found Hammer: ✗ (unchecked)
```

#### Emily Hunt
```
Is Emily Hunting: ✗ (unchecked)
Emily AI: [Drag EmilyAI GameObject here]
Emily Spawn Point: [Drag EmilySpawnPoint GameObject here]
Emily Enter Sound: [Drag sound clip here - door breaking sound]
```

#### Mirror Progress
```
Has Examined Mirror: ✗ (unchecked)
Has Broken Mirror: ✗ (unchecked)
Can Climb Through: ✗ (unchecked)
```

#### Mirror Sprites ⭐ IMPORTANTE
```
Mirror Sprite Renderer: [Drag Mirror GameObject here]
Mirror Normal Sprite: [Drag normal mirror sprite here]
Mirror Broken Sprite: [Drag broken mirror sprite here]
Passage Object: [Drag Passage GameObject here]
```

#### Emily AI (Outside)
```
Emily Humming Sound: [Drag humming loop sound here]
Emily Audio Source: [Drag AudioSource component here]
```

#### Door
```
Bathroom Door: [Drag BathroomDoor GameObject here]
Is Door Locked: ✓ (checked)
```

#### Scene Transition
```
Next Scene Name: Room09_Master's_Bathroom
```

---

## PART 3: QTE PANEL SETUP

### 1. Create QTE Canvas

**Step 1**: Create Canvas
```
Hierarchy → Right-click → UI → Canvas
Name: QTE_Canvas
```

**Step 2**: Configure Canvas
```
Canvas Component:
- Render Mode: Screen Space - Overlay
- Pixel Perfect: ✗
- Sort Order: 100 (high priority)

Canvas Scaler:
- UI Scale Mode: Scale With Screen Size
- Reference Resolution: 1920 x 1080
- Match: 0.5
```

---

### 2. Create QTE Panel

**Step 1**: Create Panel
```
QTE_Canvas → Right-click → UI → Panel
Name: QTE_Panel
```

**Step 2**: Configure Panel
```
Rect Transform:
- Anchor: Stretch (full screen)
- Left: 0, Top: 0, Right: 0, Bottom: 0

Image Component:
- Color: Black with transparency (R:0, G:0, B:0, A:200)
```

**Step 3**: DISABLE Panel ⭐ IMPORTANTE
```
Select QTE_Panel
Inspector → Top checkbox: ✗ (unchecked)
```

---

### 3. Create Full Screen Tap Area ⭐ MAIN BUTTON

**Step 1**: Create Image
```
QTE_Panel → Right-click → UI → Image
Name: FullScreenTapArea
```

**Step 2**: Configure Image
```
Rect Transform:
- Anchor: Stretch (full screen)
- Left: 0, Top: 0, Right: 0, Bottom: 0

Image Component:
- Source Image: None (leave empty)
- Color: Transparent (R:0, G:0, B:0, A:0) or semi-transparent
- Raycast Target: ✓ (CHECKED) ⭐ IMPORTANTE!
```

**IMPORTANTE**: This is the button player will click! Must be full screen!

---

### 4. Create Timer Text

**Step 1**: Create Text
```
QTE_Panel → Right-click → UI → Text - TextMeshPro
Name: TimerText
```

**Step 2**: Configure Text
```
Rect Transform:
- Anchor: Top Center
- Pos X: 0, Pos Y: -100
- Width: 300, Height: 100

TextMeshProUGUI:
- Text: "25.0s"
- Font Size: 72
- Alignment: Center
- Color: White
- Font: Bold
```

---

### 5. Create Progress Text

**Step 1**: Create Text
```
QTE_Panel → Right-click → UI → Text - TextMeshPro
Name: ProgressText
```

**Step 2**: Configure Text
```
Rect Transform:
- Anchor: Center
- Pos X: 0, Pos Y: 0
- Width: 400, Height: 150

TextMeshProUGUI:
- Text: "0/50"
- Font Size: 96
- Alignment: Center
- Color: White
- Font: Bold
```

---

### 6. Create Mirror Image (Optional Visual)

**Step 1**: Create Image
```
QTE_Panel → Right-click → UI → Image
Name: MirrorImage
```

**Step 2**: Configure Image
```
Rect Transform:
- Anchor: Center
- Pos X: 0, Pos Y: 0
- Width: 800, Height: 600

Image Component:
- Source Image: [Mirror sprite]
- Preserve Aspect: ✓
```

**Note**: This shows the mirror cracking during QTE (optional)

---

## PART 4: QTE COMPONENT SETUP

### 1. Create QTE Manager

**Step 1**: Create GameObject
```
Hierarchy → Right-click → Create Empty
Name: QTE_Manager
```

**Step 2**: Add Script
```
Inspector → Add Component → Room08_MirrorQTE
```

**Step 3**: Configure QTE Settings

#### QTE Settings
```
Total Taps: 50
Total Time Limit: 25
Max Failures: 0
```

#### UI References ⭐ IMPORTANTE
```
QTE Panel: [Drag QTE_Panel here]
Full Screen Tap Area: [Drag FullScreenTapArea Image here]
Timer Text TMP: [Drag TimerText here]
Progress Text TMP: [Drag ProgressText here]
```

#### Visual Effects
```
Mirror Image: [Drag MirrorImage here - optional]
Mirror Phase 1: [Normal mirror sprite]
Mirror Phase 2: [Slight cracks sprite]
Mirror Phase 3: [More cracks sprite]
Mirror Phase 4: [Almost shattered sprite]
Shatter Effect: [Particle effect GameObject - optional]
```

#### Audio
```
Tap Sound: [Glass tap sound]
Crack Sound: [Glass crack sound]
Shatter Sound: [Glass shatter sound]
Fail Sound: [Fail sound]
Glass Stress Sounds: [Array of escalating sounds]
```

#### Camera Shake
```
Shake Intensity: 0.1
Shake Duration: 0.2
```

---

## PART 5: AUDIO SETUP

### 1. Create Audio Source (Emily Humming)

**Step 1**: Create GameObject
```
Hierarchy → Right-click → Create Empty
Name: EmilyAudioSource
```

**Step 2**: Add Audio Source
```
Inspector → Add Component → Audio Source
- AudioClip: [Leave empty - set by script]
- Play On Awake: ✗ (unchecked)
- Loop: ✓ (checked)
- Volume: 0.5
- Spatial Blend: 0 (2D sound)
```

**Step 3**: Assign to Room Controller
```
Select Room08_Controller
Inspector → Room08_FlowController
- Emily Audio Source: [Drag EmilyAudioSource here]
- Emily Humming Sound: [Drag humming loop clip here]
```

---

## PART 6: FINAL CHECKLIST

### Scene Objects:
- [ ] TornClothes (floor, visible, Evidence type)
- [ ] ApologyNote (floor, visible, Evidence type)
- [ ] MedicineCabinet (wall, visible, MedicineCabinet type)
- [ ] Mirror (wall, visible, Mirror type, normal sprite)
- [ ] Passage (behind mirror, DISABLED, Passage type)
- [ ] EmilyAI (DISABLED, has NavMeshAgent + EmilyGhost)
- [ ] EmilySpawnPoint (empty GameObject, positioned)
- [ ] BathroomDoor (visible, Door type)

### Controllers:
- [ ] Room08_Controller (has Room08_FlowController)
  - [ ] All GameObjects assigned
  - [ ] Mirror sprites assigned (normal + broken)
  - [ ] Emily AI assigned
  - [ ] Passage assigned
  - [ ] Audio source assigned

### QTE Setup:
- [ ] QTE_Canvas (Screen Space Overlay, Sort Order: 100)
- [ ] QTE_Panel (DISABLED initially)
- [ ] FullScreenTapArea (full screen, Raycast Target: ✓)
- [ ] TimerText (shows "25.0s")
- [ ] ProgressText (shows "0/50")
- [ ] QTE_Manager (has Room08_MirrorQTE)
  - [ ] All UI references assigned
  - [ ] Total Taps: 50
  - [ ] Total Time Limit: 25

### Audio:
- [ ] EmilyAudioSource (has Audio Source component)
- [ ] Humming sound assigned
- [ ] QTE sounds assigned (tap, crack, shatter)

---

## PART 7: TESTING

### Test 1: Item Pickup
```
1. Play the scene
2. Walk to torn clothes → Press interact
3. Expected: Item disappears, dialogue shows
4. Walk to apology note → Press interact
5. Expected: Item disappears, dialogue shows
6. Walk to medicine cabinet → Press interact
7. Expected: Dialogue shows, hammer obtained
```

### Test 2: Emily Enters
```
1. After collecting all 3 items
2. Expected: Dialogue "The door! She broke through!"
3. Expected: Emily spawns and starts hunting
4. Expected: Emily chases player
```

### Test 3: Mirror QTE
```
1. While Emily hunts, interact with mirror
2. Expected: QTE panel appears
3. Expected: Timer shows "25.0s"
4. Expected: Progress shows "0/50"
5. Click anywhere on screen 50 times
6. Expected: Counter increases each click
7. Expected: Timer counts down
8. After 50 clicks:
   - Mirror sprite changes to broken
   - Emily disappears
   - Passage appears
```

### Test 4: Escape
```
1. After mirror breaks
2. Walk to passage → Press interact
3. Expected: Loads next scene
```

---

## COMMON ISSUES

### Issue 1: Can't click QTE button
**Fix**:
- Check FullScreenTapArea → Raycast Target: ✓
- Check FullScreenTapArea is full screen (Anchor: Stretch)

### Issue 2: Emily doesn't spawn
**Fix**:
- Check EmilyAI is assigned to Room08_Controller
- Check EmilySpawnPoint is assigned
- Check all 3 items collected

### Issue 3: Mirror doesn't change sprite
**Fix**:
- Check Mirror Sprite Renderer assigned
- Check Mirror Normal Sprite assigned
- Check Mirror Broken Sprite assigned

### Issue 4: Passage doesn't appear
**Fix**:
- Check Passage GameObject assigned to Room08_Controller
- Check Passage initially disabled
- Check QTE completed successfully

---

## SPRITES NEEDED

Para sa designer, kailangan ng mga sprites:

### Mirror:
1. **Mirror_Normal** - Clean mirror
2. **Mirror_Broken** - Shattered mirror with cracks
3. **Mirror_Phase1** - Clean (for QTE)
4. **Mirror_Phase2** - Slight cracks (for QTE)
5. **Mirror_Phase3** - More cracks (for QTE)
6. **Mirror_Phase4** - Almost shattered (for QTE)

### Items:
1. **TornClothes** - Bloody torn clothes
2. **ApologyNote** - Handwritten note
3. **MedicineCabinet** - Cabinet sprite
4. **Passage** - Hole/passage behind mirror

### Emily:
1. **Emily_Sprite** - Emily character sprite
2. **Emily_Animations** - Walking, idle, etc.

### Environment:
1. **Bathroom_Background** - Bathroom scene
2. **Door** - Bathroom door
3. **Bathtub** - Optional
4. **Sink** - Optional

---

## SOUNDS NEEDED

Para sa sound designer:

### Ambient:
1. **Emily_Humming** - Looping humming sound

### QTE:
1. **Glass_Tap** - Single tap sound
2. **Glass_Crack** - Cracking sound
3. **Glass_Shatter** - Final shatter sound
4. **Glass_Stress_1** - Light stress
5. **Glass_Stress_2** - Medium stress
6. **Glass_Stress_3** - Heavy stress

### Events:
1. **Door_Break** - Emily breaking through door
2. **Emily_Footsteps** - Walking sounds

---

**TAPOS NA!** ✅

Sundin lang ang guide na ito step-by-step at magiging okay na ang Room 08 setup!
