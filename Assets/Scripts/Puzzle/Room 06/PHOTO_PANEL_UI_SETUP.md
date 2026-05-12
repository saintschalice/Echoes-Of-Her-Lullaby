# Photo Panel UI Setup Guide

## 📋 Overview

Panel na lalabas para makita yung full photo, with transition from normal to scratched.

---

## 🔧 Unity UI Setup

### Step 1: Create Photo Panel

1. Right-click in Hierarchy → UI → Panel
2. Rename to: `PhotoPanel`
3. Set as child of Canvas

### Step 2: Configure Panel

**RectTransform:**
- Anchor: Stretch (full screen)
- Left: 0, Right: 0, Top: 0, Bottom: 0
- Scale: (1, 1, 1)

**Image Component:**
- Color: Black with alpha 200 (semi-transparent background)
- Or use your own background sprite

### Step 3: Create Photo Image

1. Right-click PhotoPanel → UI → Image
2. Rename to: `PhotoImage`
3. This will show the actual photo

**RectTransform:**
- Anchor: Center
- Width: 800 (adjust to your photo size)
- Height: 600 (adjust to your photo size)
- Pos X: 0, Pos Y: 0

**Image Component:**
- Source Image: (will be set by script)
- Preserve Aspect: ✅ Checked (recommended)

### Step 4: Create Close Button (Optional)

1. Right-click PhotoPanel → UI → Button
2. Rename to: `CloseButton`
3. Position at top-right corner

**RectTransform:**
- Anchor: Top-Right
- Width: 100, Height: 100
- Pos X: -50, Pos Y: -50

**Button Component:**
- OnClick() → Add Room06_HallwayController.ClosePhotoPanel()

**Text:**
- Change button text to "X" or "Close"

---

## ⚙️ Inspector Setup

### Room06_HallwayController:

**Photo Panel UI:**
```
Photo Panel: PhotoPanel GameObject
Photo Panel Image: PhotoImage (Image component)
Photo Panel Close Button: CloseButton (optional)
```

---

## 🎮 How It Works

### Flow:

1. **Player interacts with photo frame**
2. **Dialogue**: "A family photo... they look happy."
3. **Panel opens** showing normal photo (1.5 seconds)
4. **SCRATCH EFFECT!** Photo changes to scratched version
5. **Scratch sound** plays
6. **Panel closes** automatically (1 second)
7. **Dialogue**: "What?! The faces... they're scratched out!"
8. **Dialogue**: "No... she's here!"
9. **Emily spawns** and hunts!

---

## 🎯 Timing Breakdown

```
[Dialogue: "A family photo..."]
   ↓ (0.5s)
[Panel opens - Normal photo]
   ↓ (1.5s)
[SCRATCH! - Changes to scratched photo]
   ↓ (1.0s)
[Panel closes]
   ↓ (0.3s)
[Dialogue: "What?! The faces..."]
   ↓
[Dialogue: "No... she's here!"]
   ↓ (spawn delay: 1.5s)
[Emily spawns!]
```

---

## 🎨 Visual Setup

### Hierarchy Structure:

```
Canvas
└── PhotoPanel (Panel)
    ├── PhotoImage (Image) ← Assign this to Photo Panel Image
    └── CloseButton (Button) ← Optional
```

### PhotoPanel Settings:

```
GameObject: PhotoPanel
Active: ☐ (Disabled at start)

Components:
├─ RectTransform (Stretch full screen)
├─ CanvasRenderer
└─ Image (Black background, alpha 200)
```

### PhotoImage Settings:

```
GameObject: PhotoImage

RectTransform:
├─ Anchor: Center
├─ Width: 800
├─ Height: 600
└─ Position: (0, 0, 0)

Image:
├─ Source Image: (set by script)
├─ Preserve Aspect: ☑
└─ Raycast Target: ☐ (optional)
```

---

## 📝 Required Assets

### Sprites:
1. **Normal Photo** - Family photo (happy faces)
2. **Scratched Photo** - Same photo with scratched faces

**Recommended Size:**
- 1024x768 or higher
- PNG format with transparency
- Same dimensions for both sprites

---

## ✅ Testing Checklist

### Panel Setup:
- [ ] PhotoPanel exists in Canvas
- [ ] PhotoPanel is disabled at start
- [ ] PhotoImage is child of PhotoPanel
- [ ] PhotoImage size is appropriate

### Script References:
- [ ] Photo Panel assigned to controller
- [ ] Photo Panel Image assigned to controller
- [ ] Normal Photo Sprite assigned
- [ ] Scratched Photo Sprite assigned

### Testing:
- [ ] Interact with photo frame
- [ ] Panel opens with normal photo
- [ ] Wait 1.5 seconds
- [ ] Photo scratches (sprite changes)
- [ ] Scratch sound plays
- [ ] Panel closes automatically
- [ ] Dialogue appears
- [ ] Emily spawns

---

## 🐛 Troubleshooting

### Panel doesn't appear
**Solution:**
- Check if PhotoPanel is assigned in controller
- Check if PhotoPanel is child of Canvas
- Check if Canvas is in scene

### Photo doesn't show
**Solution:**
- Check if PhotoImage is assigned in controller
- Check if Normal Photo Sprite is assigned
- Check if PhotoImage has Image component

### Photo doesn't scratch
**Solution:**
- Check if Scratched Photo Sprite is assigned
- Check if scratch sound is assigned
- Check Console for errors

### Panel doesn't close
**Solution:**
- Panel closes automatically after 1 second
- If using close button, check if OnClick is set up
- Check if ClosePhotoPanel() method is assigned

---

## 💡 Tips

### For Better Effect:
- Use high-quality photo sprites
- Add fade in/out animation (optional)
- Add screen shake on scratch (optional)
- Use dramatic scratch sound

### For Mobile:
- Make sure panel is large enough
- Add close button for manual close
- Test on different screen sizes

### For Atmosphere:
- Dim the background (black with alpha)
- Add vignette effect (optional)
- Use creepy music during panel view

---

## 🎯 Quick Setup

1. Create PhotoPanel (UI Panel, full screen)
2. Create PhotoImage (UI Image, centered, 800x600)
3. Assign to Room06_HallwayController
4. Assign both photo sprites
5. Test!

**Done!** 🎮✨

---

## 📋 Inspector Preview

```
Room06_HallwayController:

Photo Frame:
├─ Photo Frame: PhotoFrame GameObject
├─ Normal Photo Sprite: FamilyPhoto_Normal
└─ Scratched Photo Sprite: FamilyPhoto_Scratched

Photo Panel UI: ⭐ NEW!
├─ Photo Panel: PhotoPanel
├─ Photo Panel Image: PhotoImage (Image component)
└─ Photo Panel Close Button: CloseButton (optional)

Emily Configuration:
├─ Emily Game Object: Emily
├─ Emily Spawn Point: Emily_Spawn_Point
├─ Emily Chase Speed: 4.5
└─ Catch Distance: 1.0
```

**Ready to use!** 💪✨
