# Room 08 - Lisa's Bathroom (Updated Flow)

## 🎯 COMPLETE FLOW

### Step 1: Collect Evidence Items
- Player enters bathroom
- **Collect evidence items** scattered around the room
- These are pickup items (not interactables)
- Mark `hasCollectedAllEvidence = true` when all collected

### Step 2: Get Hammer from Cabinet
- **Interact with Medicine Cabinet**
- Prerequisite: Evidence must be collected first
- Obtain **hammer** from cabinet
- Mark `hasFoundHammer = true`

### Step 3: Interact with Bathtub
- **Interact with Bathtub**
- Prerequisite: Hammer must be obtained first
- Show bathtub dialogue
- Mark `hasInteractedWithBathtub = true`

### Step 4: Interact with Mirror (QTE)
- **Interact with Mirror**
- Prerequisites:
  - ✅ Evidence collected
  - ✅ Hammer obtained
  - ✅ Bathtub interacted
- Start **Mirror QTE** (15 taps)
- Player must tap screen 15 times within 25 seconds
- Screen fills with color as progress increases

### Step 5: Mirror Breaks
- After 15 successful taps:
  - Mirror sprite changes to **broken mirror**
  - **Passage** behind mirror is revealed
  - Player can now interact with passage

### Step 6: Climb Through Passage
- **Interact with Passage**
- Transition to **Master's Bathroom** (Room 09)

---

## 📋 FLOW CONTROLLER FLAGS

```csharp
// Evidence Collection
public bool hasCollectedAllEvidence = false; // All evidence items collected
public bool hasFoundHammer = false; // Hammer from cabinet
public bool hasInteractedWithBathtub = false; // Bathtub interaction

// Mirror Progress
public bool hasExaminedMirror = false; // Examined mirror
public bool hasBrokenMirror = false; // Mirror QTE completed
public bool canClimbThrough = false; // Can climb through passage
```

---

## 🎮 MIRROR QTE SYSTEM

### Settings:
- **Total Taps**: 15 (changed from 50)
- **Time Limit**: 25 seconds
- **Tap Area**: Full screen button
- **Visual Feedback**: Color fill increases with each tap

### UI Setup:

```
QTE Panel (GameObject)
├─ Full Screen Tap Area (Image)
│   ├─ Color: Semi-transparent red (0.8, 0.2, 0.2, 0.5)
│   ├─ Button Component (added at runtime)
│   └─ Fill Image (Image, child)
│       ├─ Image Type: Filled
│       ├─ Fill Method: Horizontal or Radial
│       ├─ Fill Amount: 0 → 1 (animated)
│       └─ Color: Same as parent or different
│
├─ Timer Text (Text or TMP)
│   └─ Shows: "25.0s" → "0.0s"
│
├─ Progress Text (Text or TMP)
│   └─ Shows: "0/15" → "15/15"
│
└─ Mirror Image (Image)
    └─ Sprite changes based on progress:
        - Phase 1: Clean mirror (0-25%)
        - Phase 2: First cracks (25-50%)
        - Phase 3: More cracks (50-75%)
        - Phase 4: Almost shattered (75-100%)
```

### Color Recommendation:

**Fill Color** (for tap area and fill image):
- **Red-ish**: `new Color(0.8f, 0.2f, 0.2f, 0.5f)` - Aggressive, urgent
- **Blue-ish**: `new Color(0.2f, 0.4f, 0.8f, 0.5f)` - Calm, focused
- **Purple-ish**: `new Color(0.6f, 0.2f, 0.8f, 0.5f)` - Mysterious, eerie
- **Green-ish**: `new Color(0.2f, 0.8f, 0.4f, 0.5f)` - Progress, success

**Recommended**: Red-ish for urgency and tension!

---

## 🎨 UNITY SETUP

### 1. Scene Objects

```
Room08_Lisa'sBathroom (Scene)
├─ Room08_FlowController (GameObject)
│   └─ Room08_FlowController (Script)
│
├─ Evidence Items (GameObjects with pickup script)
│   ├─ Evidence_1
│   ├─ Evidence_2
│   └─ Evidence_3
│
├─ Medicine Cabinet (GameObject)
│   ├─ BoxCollider2D (Is Trigger ✓)
│   └─ Room08_Interactable (Script)
│       └─ Object Type: MedicineCabinet
│
├─ Bathtub (GameObject)
│   ├─ BoxCollider2D (Is Trigger ✓)
│   └─ Room08_Interactable (Script)
│       └─ Object Type: Bathtub
│
├─ Mirror (GameObject)
│   ├─ SpriteRenderer (normal mirror sprite)
│   ├─ BoxCollider2D (Is Trigger ✓)
│   └─ Room08_Interactable (Script)
│       └─ Object Type: Mirror
│
├─ Passage (GameObject)
│   ├─ Initially: SetActive(false)
│   ├─ BoxCollider2D (Is Trigger ✓)
│   └─ Room08_Interactable (Script)
│       └─ Object Type: Passage
│
└─ Canvas
    └─ QTE Panel (GameObject)
        └─ Room08_MirrorQTE (Script)
```

### 2. Room08_FlowController Setup

**Assign in Inspector**:
- Mirror Sprite Renderer: Drag mirror GameObject's SpriteRenderer
- Mirror Normal Sprite: Normal mirror sprite
- Mirror Broken Sprite: Broken/shattered mirror sprite
- Passage Object: Drag passage GameObject
- Next Scene Name: "Room09_Master's_Bathroom"

### 3. Room08_MirrorQTE Setup

**Create QTE Panel**:

1. **Create Panel**:
   - Right-click Canvas → UI → Panel
   - Rename to "QTE_Panel"
   - Stretch to full screen (Anchor: Stretch, Offset: 0,0,0,0)
   - Set Color: Black with alpha 0.8 (for background)

2. **Create Full Screen Tap Area**:
   - Right-click QTE_Panel → UI → Image
   - Rename to "TapArea"
   - Stretch to full screen
   - Set Color: `RGBA(204, 51, 51, 128)` or `(0.8, 0.2, 0.2, 0.5)`
   - **NO Button component needed** (added at runtime)

3. **Create Fill Image** (child of TapArea):
   - Right-click TapArea → UI → Image
   - Rename to "FillImage"
   - Stretch to full screen
   - Image Type: **Filled**
   - Fill Method: **Horizontal** (Left to Right) or **Radial 360**
   - Fill Origin: Left or Bottom
   - Fill Amount: 0 (will animate to 1)
   - Set Color: Same as TapArea or different

4. **Create Timer Text**:
   - Right-click QTE_Panel → UI → Text (or TextMeshPro)
   - Rename to "TimerText"
   - Position: Top center
   - Font Size: 48
   - Alignment: Center
   - Text: "25.0s"

5. **Create Progress Text**:
   - Right-click QTE_Panel → UI → Text (or TextMeshPro)
   - Rename to "ProgressText"
   - Position: Bottom center
   - Font Size: 36
   - Alignment: Center
   - Text: "0/15"

6. **Create Mirror Image** (optional):
   - Right-click QTE_Panel → UI → Image
   - Rename to "MirrorImage"
   - Position: Center
   - Size: 400x600 (or appropriate size)
   - Assign mirror sprite

**Assign in Room08_MirrorQTE Inspector**:
- QTE Panel: Drag QTE_Panel
- Full Screen Tap Area: Drag TapArea Image
- Fill Image: Drag FillImage
- Fill Color: Set to desired color (default: red-ish)
- Timer Text: Drag TimerText (if using Text)
- Timer Text TMP: Drag TimerText (if using TMP)
- Progress Text: Drag ProgressText (if using Text)
- Progress Text TMP: Drag ProgressText (if using TMP)
- Mirror Image: Drag MirrorImage (optional)
- Mirror Phase 1-4: Assign mirror sprites (clean → cracked → shattered)
- Total Taps: 15
- Total Time Limit: 25
- Audio clips: Assign tap, crack, shatter sounds

---

## 🎵 AUDIO SETUP

### Required Audio Clips:

1. **Tap Sound**: Quick tap/hit sound (0.1-0.2s)
2. **Crack Sound**: Glass cracking sound (0.3-0.5s)
3. **Shatter Sound**: Glass shattering sound (1.0-2.0s)
4. **Glass Stress Sounds** (array): Escalating stress sounds as mirror breaks

---

## 🔄 INTERACTION ORDER

### Correct Order:
1. ✅ Collect evidence items
2. ✅ Interact with cabinet → Get hammer
3. ✅ Interact with bathtub
4. ✅ Interact with mirror → Start QTE
5. ✅ Complete QTE (15 taps)
6. ✅ Mirror breaks → Passage revealed
7. ✅ Interact with passage → Go to Room 09

### Blocked Interactions:

**Cabinet (before evidence)**:
- Message: "I should collect the evidence first."

**Bathtub (before hammer)**:
- Message: "I should look around more before examining the bathtub."

**Mirror (before prerequisites)**:
- Message: "I need to finish examining everything first."

---

## ✅ TESTING CHECKLIST

### Test Flow:

1. **Enter Room 08**
   - ✅ Intro dialogue plays
   - ✅ Player can move after intro

2. **Try Cabinet First**
   - ✅ Shows "collect evidence first" message

3. **Collect Evidence Items**
   - ✅ Evidence items disappear when collected
   - ✅ `hasCollectedAllEvidence = true`

4. **Interact with Cabinet**
   - ✅ Cabinet opens
   - ✅ Hammer obtained
   - ✅ Hammer dialogue shows

5. **Try Bathtub**
   - ✅ Bathtub dialogue shows
   - ✅ `hasInteractedWithBathtub = true`

6. **Interact with Mirror**
   - ✅ QTE panel appears
   - ✅ Full screen tap area visible
   - ✅ Timer starts counting down
   - ✅ Progress shows "0/15"

7. **Tap Screen 15 Times**
   - ✅ Each tap plays sound
   - ✅ Fill image increases
   - ✅ Progress updates "1/15", "2/15", etc.
   - ✅ Mirror sprite changes at 25%, 50%, 75%
   - ✅ Camera shakes on each tap

8. **Complete QTE**
   - ✅ Shatter sound plays
   - ✅ Big camera shake
   - ✅ Success dialogue shows
   - ✅ QTE panel closes
   - ✅ Mirror sprite changes to broken
   - ✅ Passage appears

9. **Interact with Passage**
   - ✅ Climb through dialogue
   - ✅ Fade transition
   - ✅ Load Room 09

### Test Failure:

1. **Let Timer Run Out**
   - ✅ QTE fails
   - ✅ Failure dialogue shows
   - ✅ Game over or retry

---

## 🐛 TROUBLESHOOTING

### Issue: "Cabinet won't open"

**Solution**: Make sure evidence is collected first. Check `hasCollectedAllEvidence` flag.

### Issue: "Bathtub won't interact"

**Solution**: Make sure hammer is obtained first. Check `hasFoundHammer` flag.

### Issue: "Mirror won't start QTE"

**Solution**: Check all prerequisites:
- `hasCollectedAllEvidence = true`
- `hasFoundHammer = true`
- `hasInteractedWithBathtub = true`

### Issue: "Tap doesn't register"

**Solution**:
- Check if Button component is added to TapArea (added at runtime)
- Check if QTE panel is active
- Check if `isQTEActive = true`

### Issue: "Fill image doesn't fill"

**Solution**:
- Check if Fill Image is assigned in Inspector
- Check if Image Type is set to "Filled"
- Check if Fill Amount animates from 0 to 1

### Issue: "Mirror doesn't break"

**Solution**:
- Check if all 15 taps completed
- Check if `OnMirrorBroken()` is called
- Check if broken sprite is assigned

---

## 💡 TIPS

### For Better Experience:

1. **Visual Feedback**:
   - Use distinct colors for fill (red = urgent)
   - Animate fill smoothly
   - Show mirror cracking progressively

2. **Audio Feedback**:
   - Tap sound: Quick and satisfying
   - Crack sound: Escalating intensity
   - Shatter sound: Dramatic and final

3. **Camera Shake**:
   - Small shake on each tap
   - Big shake on final shatter
   - Adds impact and urgency

4. **Timer Pressure**:
   - 25 seconds for 15 taps = ~1.67s per tap
   - Comfortable but creates tension
   - Color changes (white → yellow → red) as time runs out

---

**Setup complete! Test the flow in Unity!** 🎮✨

