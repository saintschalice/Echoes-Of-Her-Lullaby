# Jumpscare System - Unity Hierarchy Setup

## 🎯 COMPLETE HIERARCHY STRUCTURE

```
Scene Hierarchy:
│
├─ Canvas (or JumpscareCanvas)
│   ├─ Render Mode: Screen Space - Overlay
│   ├─ Canvas Scaler:
│   │   ├─ UI Scale Mode: Scale With Screen Size
│   │   └─ Reference Resolution: 1920x1080
│   └─ Sort Order: 1000 ← IMPORTANT! (above everything)
│   │
│   └─ JumpscarePanel (Panel)
│       ├─ Anchor: Stretch (full screen)
│       ├─ Left: 0, Right: 0, Top: 0, Bottom: 0
│       ├─ Image:
│       │   ├─ Color: Black (0, 0, 0, 255)
│       │   └─ Raycast Target: ✓
│       ├─ Active: ☐ (UNCHECKED - hidden at start)
│       │
│       ├─ JumpscareImage (Image)
│       │   ├─ Anchor: Center
│       │   ├─ Pos X: 0, Pos Y: 0
│       │   ├─ Width: 1920 (or your sprite width)
│       │   ├─ Height: 1080 (or your sprite height)
│       │   ├─ Image:
│       │   │   ├─ Source Image: (empty - set by script)
│       │   │   ├─ Preserve Aspect: ✓
│       │   │   └─ Raycast Target: ☐
│       │   └─ Active: ✓ (checked)
│       │
│       └─ FlashImage (Image) [OPTIONAL]
│           ├─ Anchor: Stretch (full screen)
│           ├─ Left: 0, Right: 0, Top: 0, Bottom: 0
│           ├─ Image:
│           │   ├─ Color: White (255, 255, 255, 0) ← Alpha 0!
│           │   └─ Raycast Target: ☐
│           └─ Active: ☐ (UNCHECKED - hidden at start)
│
└─ JumpscareManager (Empty GameObject)
    ├─ Tag: (optional) "GameController"
    └─ JumpscareManager (Script)
        ├─ [Jumpscare UI]
        │   ├─ Jumpscare Panel: → JumpscarePanel
        │   └─ Jumpscare Image: → JumpscareImage
        │
        ├─ [Jumpscare Sprites]
        │   ├─ Tilt Left Sprite: → [your sprite]
        │   ├─ Tilt Right Sprite: → [your sprite]
        │   └─ Center Sprite: → [your sprite]
        │
        ├─ [Timing]
        │   ├─ Tilt Left Duration: 0.3
        │   ├─ Tilt Right Duration: 0.3
        │   ├─ Center Duration: 2.0
        │   └─ Total Jumpscare Duration: 11.0
        │
        ├─ [Audio]
        │   └─ Jumpscare Sound: → [your audio clip]
        │
        ├─ [Visual Effects]
        │   ├─ Enable Screen Shake: ✓
        │   ├─ Shake Intensity: 0.5
        │   ├─ Enable Flash: ✓
        │   ├─ Flash Color: White (255, 255, 255, 255)
        │   └─ Flash Image: → FlashImage
        │
        └─ [Fade Settings]
            ├─ Fade In Duration: 0.2
            └─ Fade Out Duration: 0.5
```

---

## 📐 DETAILED COMPONENT SETTINGS

### Canvas Settings:
```
Component: Canvas
├─ Render Mode: Screen Space - Overlay
├─ Pixel Perfect: ☐ (optional)
└─ Sort Order: 1000 ← CRITICAL!

Component: Canvas Scaler
├─ UI Scale Mode: Scale With Screen Size
├─ Reference Resolution: 1920 x 1080
├─ Screen Match Mode: Match Width Or Height
├─ Match: 0.5 (middle)
└─ Reference Pixels Per Unit: 100
```

---

### JumpscarePanel Settings:
```
Component: RectTransform
├─ Anchor Presets: Stretch (full screen)
│   └─ Hold Alt+Shift, click bottom-right preset
├─ Left: 0
├─ Right: 0
├─ Top: 0
├─ Bottom: 0
├─ Pivot: X: 0.5, Y: 0.5
└─ Rotation: 0, 0, 0

Component: Image
├─ Source Image: (none) or solid white sprite
├─ Color: Black
│   ├─ R: 0
│   ├─ G: 0
│   ├─ B: 0
│   └─ A: 255
├─ Material: None (Default UI Material)
├─ Raycast Target: ✓ (checked)
└─ Maskable: ✓ (checked)

Component: Canvas Group (added by script)
├─ Alpha: 0 (set by script)
├─ Interactable: ✓
└─ Block Raycasts: ✓

GameObject:
└─ Active: ☐ (UNCHECKED at start!)
```

---

### JumpscareImage Settings:
```
Component: RectTransform
├─ Anchor Presets: Center
├─ Pos X: 0
├─ Pos Y: 0
├─ Width: 1920 (adjust to your sprite)
├─ Height: 1080 (adjust to your sprite)
├─ Pivot: X: 0.5, Y: 0.5
└─ Rotation: 0, 0, 0

Component: Image
├─ Source Image: (empty - set by script)
├─ Color: White (255, 255, 255, 255)
├─ Material: None
├─ Raycast Target: ☐ (UNCHECKED)
├─ Preserve Aspect: ✓ (checked)
└─ Set Native Size: (click after assigning sprite)

GameObject:
└─ Active: ✓ (checked)
```

---

### FlashImage Settings (Optional):
```
Component: RectTransform
├─ Anchor Presets: Stretch (full screen)
├─ Left: 0
├─ Right: 0
├─ Top: 0
├─ Bottom: 0
├─ Pivot: X: 0.5, Y: 0.5
└─ Rotation: 0, 0, 0

Component: Image
├─ Source Image: (none) or solid white sprite
├─ Color: White
│   ├─ R: 255
│   ├─ G: 255
│   ├─ B: 255
│   └─ A: 0 ← ALPHA 0!
├─ Material: None
├─ Raycast Target: ☐ (UNCHECKED)
└─ Maskable: ✓

GameObject:
└─ Active: ☐ (UNCHECKED at start!)
```

---

### JumpscareManager Settings:
```
Component: Transform
├─ Position: 0, 0, 0
├─ Rotation: 0, 0, 0
└─ Scale: 1, 1, 1

Component: JumpscareManager (Script)
└─ See "Complete Hierarchy Structure" above for all fields
```

---

## 🎨 SPRITE IMPORT SETTINGS

### For All 3 Jumpscare Sprites:

```
Import Settings (Inspector):
├─ Texture Type: Sprite (2D and UI)
├─ Sprite Mode: Single
├─ Pixels Per Unit: 100
├─ Mesh Type: Full Rect
├─ Extrude Edges: 0
├─ Pivot: Center
├─ Generate Mip Maps: ☐ (unchecked)
├─ Filter Mode: Bilinear
├─ Max Size: 2048 (or higher for quality)
├─ Compression: None (for quality)
└─ Format: RGBA 32 bit

Recommended Dimensions:
├─ Width: 1920 pixels
├─ Height: 1080 pixels
└─ Aspect Ratio: 16:9
```

---

## 🔊 AUDIO IMPORT SETTINGS

### For Jumpscare Sound:

```
Import Settings (Inspector):
├─ Force To Mono: ☐ (keep stereo for immersion)
├─ Load In Background: ☐ (unchecked)
├─ Ambisonic: ☐ (unchecked)
├─ Load Type: Decompress On Load
├─ Preload Audio Data: ✓ (checked)
├─ Compression Format: PCM (uncompressed)
├─ Quality: 100 (maximum)
├─ Sample Rate Setting: Preserve Sample Rate
└─ Duration: 11 seconds (verify!)
```

---

## 🔍 VERIFICATION CHECKLIST

### Canvas Hierarchy:
- [ ] Canvas exists
- [ ] Canvas Sort Order = 1000 or higher
- [ ] Canvas Scaler configured
- [ ] JumpscarePanel is child of Canvas
- [ ] JumpscareImage is child of JumpscarePanel
- [ ] FlashImage is child of JumpscarePanel (optional)

### GameObject States:
- [ ] JumpscarePanel: Active = ☐ (unchecked)
- [ ] JumpscareImage: Active = ✓ (checked)
- [ ] FlashImage: Active = ☐ (unchecked)

### RectTransform Settings:
- [ ] JumpscarePanel: Stretch anchor, all margins 0
- [ ] JumpscareImage: Center anchor, appropriate size
- [ ] FlashImage: Stretch anchor, all margins 0

### Image Components:
- [ ] JumpscarePanel: Black color, Raycast Target ✓
- [ ] JumpscareImage: White color, Raycast Target ☐
- [ ] FlashImage: White color with Alpha 0, Raycast Target ☐

### JumpscareManager:
- [ ] GameObject created
- [ ] Script attached
- [ ] All UI references assigned
- [ ] All sprites assigned (3 total)
- [ ] Audio assigned
- [ ] Timing values set
- [ ] Visual effects configured

### Assets:
- [ ] 3 sprites imported (tilt left, right, center)
- [ ] All sprites same dimensions
- [ ] All sprites Texture Type = Sprite (2D and UI)
- [ ] Audio imported (11 seconds)
- [ ] Audio Load Type = Decompress On Load

---

## 🎯 QUICK SETUP STEPS

### Step 1: Create Canvas (if needed)
1. Hierarchy → Right-click → UI → Canvas
2. Select Canvas
3. Inspector → Canvas → Sort Order: 1000
4. Inspector → Canvas Scaler → UI Scale Mode: Scale With Screen Size
5. Reference Resolution: 1920 x 1080

### Step 2: Create JumpscarePanel
1. Canvas → Right-click → UI → Panel
2. Rename to "JumpscarePanel"
3. RectTransform → Anchor: Stretch (Alt+Shift+Click bottom-right)
4. Set all margins to 0 (Left, Right, Top, Bottom)
5. Image → Color: Black (0, 0, 0, 255)
6. Uncheck "Active" checkbox (hide at start)

### Step 3: Create JumpscareImage
1. JumpscarePanel → Right-click → UI → Image
2. Rename to "JumpscareImage"
3. RectTransform → Anchor: Center
4. Set Width: 1920, Height: 1080 (or your sprite size)
5. Image → Preserve Aspect: ✓
6. Image → Raycast Target: ☐ (uncheck)
7. Keep "Active" checked

### Step 4: Create FlashImage (Optional)
1. JumpscarePanel → Right-click → UI → Image
2. Rename to "FlashImage"
3. RectTransform → Anchor: Stretch
4. Set all margins to 0
5. Image → Color: White (255, 255, 255, 0) ← Alpha 0!
6. Image → Raycast Target: ☐ (uncheck)
7. Uncheck "Active" checkbox

### Step 5: Create JumpscareManager
1. Hierarchy → Right-click → Create Empty
2. Rename to "JumpscareManager"
3. Add Component → JumpscareManager (script)
4. Drag JumpscarePanel to "Jumpscare Panel" field
5. Drag JumpscareImage to "Jumpscare Image" field
6. Drag FlashImage to "Flash Image" field (if using)
7. Drag 3 sprites to sprite fields
8. Drag audio to "Jumpscare Sound" field
9. Configure timing and effects

### Step 6: Test
1. Play game
2. Trigger a game over
3. Verify jumpscare plays
4. Verify game over shows after

---

## 💡 TIPS

### Tip 1: Canvas Sort Order
- Set to 1000 or higher to ensure jumpscare appears above everything
- Higher number = appears on top

### Tip 2: Anchor Presets
- Hold Alt+Shift when clicking preset to set both anchor and position
- Stretch preset = full screen coverage

### Tip 3: Raycast Targets
- JumpscarePanel: ✓ (blocks clicks during jumpscare)
- JumpscareImage: ☐ (doesn't need to receive clicks)
- FlashImage: ☐ (doesn't need to receive clicks)

### Tip 4: Active States
- JumpscarePanel: Start inactive (script activates it)
- JumpscareImage: Start active (visible when panel shows)
- FlashImage: Start inactive (script activates for flash)

### Tip 5: Sprite Size
- Match JumpscareImage size to your sprite dimensions
- Use "Preserve Aspect" to prevent distortion
- Click "Set Native Size" after assigning sprite

---

## 🐛 COMMON SETUP MISTAKES

### Mistake 1: Wrong Canvas Sort Order
```
❌ Sort Order: 0 (default)
✅ Sort Order: 1000 (above everything)
```

### Mistake 2: Wrong Active States
```
❌ JumpscarePanel: Active ✓ (visible at start)
✅ JumpscarePanel: Active ☐ (hidden at start)
```

### Mistake 3: Wrong Anchor
```
❌ JumpscarePanel: Center anchor (doesn't fill screen)
✅ JumpscarePanel: Stretch anchor (fills screen)
```

### Mistake 4: Wrong Raycast Settings
```
❌ JumpscareImage: Raycast Target ✓ (blocks panel clicks)
✅ JumpscareImage: Raycast Target ☐ (doesn't block)
```

### Mistake 5: Missing References
```
❌ Jumpscare Panel: None (not assigned)
✅ Jumpscare Panel: JumpscarePanel (assigned)
```

---

**Follow this hierarchy exactly for perfect setup!** 🎯✨
