# Room 08 - Mirror Panel Setup (Like Photo Frame)

## 🎯 OVERVIEW

Ang mirror interaction sa Room 08 ay **panel-based** - parang Room 06 photo frame interaction. Hindi full-screen QTE, kundi panel lang na lalabas.

---

## 🎮 FLOW

1. **Player interacts with mirror** (world object)
2. **Panel appears** (like photo frame panel)
3. **Tap puzzle** - Tap 15 times within 25 seconds
4. **Panel closes** - Auto-close after completion
5. **Mirror transitions** - World mirror object changes to broken sprite
6. **Passage revealed** - Passage interactable appears

---

## 🔧 UNITY SETUP

### Step 1: Create Room08UIManager

1. **Create Empty GameObject**: `Room08UIManager`
2. **Add Script**: `Room08UIManager`
3. **Configure**:
   - Mirror Panel: (assign later)

### Step 2: Create Mirror Panel

**Panel Structure**:

```
Canvas
└─ MirrorPanel (GameObject)
    ├─ Panel (Image) - Background
    │   └─ Color: Black with alpha 0.8
    │
    ├─ TapArea (Image) - Tap button area
    │   ├─ Anchor: Center, Size: 800x600 (or full screen)
    │   ├─ Color: (0.8, 0.2, 0.2, 0.5) - Red-ish
    │   └─ FillImage (Image, child)
    │       ├─ Image Type: Filled
    │       ├─ Fill Method: Horizontal or Radial
    │       ├─ Fill Amount: 0 → 1
    │       └─ Color: Same as parent
    │
    ├─ MirrorImage (Image) - Shows mirror cracking
    │   ├─ Position: Center
    │   ├─ Size: 400x600
    │   └─ Sprite: Mirror phase 1
    │
    ├─ TimerText (Text/TMP)
    │   ├─ Position: Top center
    │   ├─ Text: "25.0s"
    │   └─ Font Size: 48
    │
    ├─ ProgressText (Text/TMP)
    │   ├─ Position: Bottom center
    │   ├─ Text: "0/15"
    │   └─ Font Size: 36
    │
    └─ Room08_MirrorQTE (Script)
        └─ Attached to MirrorPanel
```

### Step 3: Configure Room08_MirrorQTE

**Assign in Inspector**:

```
Room08_MirrorQTE (on MirrorPanel):
├─ Total Taps: 15
├─ Total Time Limit: 25
├─ Full Screen Tap Area: Drag TapArea Image
├─ Fill Image: Drag FillImage
├─ Fill Color: (0.8, 0.2, 0.2, 0.5)
├─ Timer Text: Drag TimerText (if Text)
├─ Timer Text TMP: Drag TimerText (if TMP)
├─ Progress Text: Drag ProgressText (if Text)
├─ Progress Text TMP: Drag ProgressText (if TMP)
├─ Mirror Image: Drag MirrorImage
├─ Mirror Phase 1-4: Assign sprites
├─ Tap Sound: Assign sound
├─ Crack Sound: Assign sound
├─ Shatter Sound: Assign sound
├─ Shatter Effect: Optional particle effect
├─ Shake Intensity: 0.1
└─ Shake Duration: 0.2
```

### Step 4: Configure Room08UIManager

**Assign in Inspector**:

```
Room08UIManager:
└─ Mirror Panel: Drag MirrorPanel GameObject
```

### Step 5: Setup World Mirror Object

**Mirror GameObject in Scene**:

```
Mirror (GameObject in scene)
├─ SpriteRenderer
│   └─ Sprite: Normal mirror sprite
├─ BoxCollider2D (Is Trigger ✓)
└─ Room08_Interactable (Script)
    └─ Object Type: Mirror
```

**Assign in Room08_FlowController**:

```
Room08_FlowController:
├─ Mirror Sprite Renderer: Drag Mirror's SpriteRenderer
├─ Mirror Normal Sprite: Normal mirror sprite
├─ Mirror Broken Sprite: Broken mirror sprite
└─ Passage Object: Drag Passage GameObject
```

### Step 6: Setup Passage

**Passage GameObject** (initially hidden):

```
Passage (GameObject in scene)
├─ Initially: SetActive(false)
├─ SpriteRenderer (optional)
├─ BoxCollider2D (Is Trigger ✓)
└─ Room08_Interactable (Script)
    └─ Object Type: Passage
```

---

## 🎨 VISUAL DESIGN

### Panel Layout:

```
┌─────────────────────────────────────┐
│         MirrorPanel (Black BG)      │
│                                     │
│         Timer: 25.0s                │
│                                     │
│    ┌─────────────────────┐         │
│    │                     │         │
│    │   Mirror Image      │         │
│    │   (Shows cracking)  │         │
│    │                     │         │
│    │   [Tap Area]        │         │
│    │   (Fills with red)  │         │
│    │                     │         │
│    └─────────────────────┘         │
│                                     │
│       Progress: 0/15                │
│                                     │
└─────────────────────────────────────┘
```

### Color Scheme:

**Tap Area Fill**:
- **Red**: `(0.8, 0.2, 0.2, 0.5)` - Urgent, aggressive
- **Purple**: `(0.6, 0.2, 0.8, 0.5)` - Mysterious, eerie
- **Blue**: `(0.2, 0.4, 0.8, 0.5)` - Calm, focused

**Recommended**: Red for urgency!

---

## 🔄 INTERACTION FLOW

### Complete Flow:

1. **Player approaches mirror** (world object)
2. **Player presses interact button**
3. **Check prerequisites**:
   - ✅ Evidence collected
   - ✅ Hammer obtained
   - ✅ Bathtub interacted
4. **Show mirror panel** (like photo frame)
5. **Disable player controls**
6. **Start tap puzzle**:
   - Timer starts: 25 seconds
   - Progress: 0/15
   - Player taps screen
7. **Each tap**:
   - Play tap sound
   - Increase fill amount
   - Update progress (1/15, 2/15, etc.)
   - Update mirror sprite (phases)
   - Camera shake
8. **Complete puzzle** (15 taps):
   - Play shatter sound
   - Big camera shake
   - Show shatter effect
   - Close panel
9. **World mirror transitions**:
   - Mirror sprite → Broken sprite
   - Passage appears
10. **Enable player controls**
11. **Player can interact with passage**

---

## ✅ TESTING CHECKLIST

### Test Mirror Panel:

1. **Prerequisites Complete**
   - ✅ Evidence collected
   - ✅ Hammer obtained
   - ✅ Bathtub interacted

2. **Interact with Mirror**
   - ✅ Panel appears (not full screen)
   - ✅ Player controls disabled
   - ✅ Timer shows "25.0s"
   - ✅ Progress shows "0/15"

3. **Tap 15 Times**
   - ✅ Each tap plays sound
   - ✅ Fill increases
   - ✅ Progress updates
   - ✅ Mirror sprite changes
   - ✅ Camera shakes

4. **Complete Puzzle**
   - ✅ Shatter sound plays
   - ✅ Big camera shake
   - ✅ Panel closes automatically
   - ✅ Player controls enabled

5. **World Mirror Changes**
   - ✅ Mirror sprite → Broken sprite
   - ✅ Passage appears
   - ✅ Can interact with passage

6. **Interact with Passage**
   - ✅ Transition to Room 09

---

## 🐛 TROUBLESHOOTING

### Issue: "Panel doesn't appear"

**Solution**:
- Check if Room08UIManager exists
- Check if Mirror Panel is assigned
- Check if prerequisites are met

### Issue: "Panel is full screen"

**Solution**:
- Panel should be child of Canvas
- TapArea should be sized appropriately (not full screen)
- Use Anchor: Center, Size: 800x600

### Issue: "Taps don't register"

**Solution**:
- Check if Button component added to TapArea (runtime)
- Check if TapArea is active
- Check if isQTEActive = true

### Issue: "Mirror doesn't change sprite"

**Solution**:
- Check if Mirror Sprite Renderer assigned
- Check if Broken Sprite assigned
- Check if OnMirrorBroken() called

### Issue: "Passage doesn't appear"

**Solution**:
- Check if Passage Object assigned
- Check if SetActive(true) called
- Check if Passage has collider

---

## 💡 COMPARISON: Panel vs Full Screen

### Panel-Based (Current):
- ✅ Like photo frame interaction
- ✅ Player sees room in background
- ✅ Less intrusive
- ✅ Easier to setup
- ✅ Consistent with Room 06

### Full-Screen QTE (Old):
- ❌ Covers entire screen
- ❌ More complex setup
- ❌ Pauses game completely
- ❌ Different from other rooms

**Panel-based is better!** 🎯

---

## 📝 SCENE HIERARCHY

```
Room08_Lisa'sBathroom (Scene)
├─ Room08_FlowController (GameObject)
│   └─ Room08_FlowController (Script)
│
├─ Room08UIManager (GameObject)
│   └─ Room08UIManager (Script)
│       └─ Mirror Panel: [assigned]
│
├─ Mirror (GameObject in world)
│   ├─ SpriteRenderer (normal sprite)
│   ├─ BoxCollider2D (Is Trigger ✓)
│   └─ Room08_Interactable (Script)
│
├─ Passage (GameObject in world)
│   ├─ SetActive(false) initially
│   ├─ BoxCollider2D (Is Trigger ✓)
│   └─ Room08_Interactable (Script)
│
└─ Canvas
    └─ MirrorPanel (GameObject)
        ├─ SetActive(false) initially
        ├─ Panel (Image) - Background
        ├─ TapArea (Image) - Tap button
        │   └─ FillImage (Image) - Fill progress
        ├─ MirrorImage (Image) - Shows cracking
        ├─ TimerText (Text/TMP)
        ├─ ProgressText (Text/TMP)
        └─ Room08_MirrorQTE (Script)
```

---

**Setup complete! Panel-based mirror interaction like photo frame!** 🪞✨

