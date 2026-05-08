# Curtain Puzzle Setup Guide

## 🎨 4-State Curtain System

Based sa sprites mo, may 4 states ang curtains:
1. **Both Closed** - Default state
2. **Left Open, Right Closed** - After clicking left button
3. **Left Closed, Right Open** - After clicking right button
4. **Both Open** - Puzzle complete!

---

## 🔧 Unity Setup

### Step 1: Create Curtain Panel

```
1. Right-click Canvas → UI → Panel
2. Rename: "CurtainPanel"
3. Set to full screen (Anchor: Stretch-Stretch)
4. Background: Semi-transparent black (0, 0, 0, 200)
5. Add Component → CurtainPuzzleUI
6. Disable panel (uncheck in Inspector)
```

### Step 2: Add Curtain Image

```
1. Right-click CurtainPanel → UI → Image
2. Rename: "CurtainImage"
3. Size: Large enough to show curtains clearly
4. Position: Center of screen
5. Sprite: Set to "bothClosedSprite" (default state)
```

### Step 3: Add Buttons

#### Left Curtain Button:
```
1. Right-click CurtainPanel → UI → Button
2. Rename: "LeftCurtainButton"
3. Position: Left side of curtain image
4. Size: Cover left half of curtains
5. Optional: Make transparent (alpha = 0) so only curtain shows
```

#### Right Curtain Button:
```
1. Right-click CurtainPanel → UI → Button
2. Rename: "RightCurtainButton"
3. Position: Right side of curtain image
4. Size: Cover right half of curtains
5. Optional: Make transparent (alpha = 0)
```

#### Close Button:
```
1. Right-click CurtainPanel → UI → Button
2. Rename: "CloseButton"
3. Position: Top-right corner
4. Text: "X" or "Close"
5. Size: Small (50x50)
```

### Step 4: Assign References

Select CurtainPanel, sa CurtainPuzzleUI component:

```
UI References:
  Curtain Panel: [CurtainPanel] ← Drag itself
  Left Curtain Button: [LeftCurtainButton]
  Right Curtain Button: [RightCurtainButton]
  Close Button: [CloseButton]

Curtain Sprites (4 States):
  Curtain Image: [CurtainImage] ← The Image component
  Both Closed Sprite: [Sprite 1 - Both closed]
  Left Open Sprite: [Sprite 2 - Left open, right closed]
  Right Open Sprite: [Sprite 3 - Left closed, right open]
  Both Open Sprite: [Sprite 4 - Both open]

Audio:
  Curtain Open Sound: [Audio clip]
```

---

## 🎮 How It Works

### Player Interaction:
```
1. Player clicks Left Button
   → Sprite changes to "Left Open" state
   → Sound plays
   
2. Player clicks Right Button
   → Sprite changes to "Both Open" state
   → Sound plays
   → Puzzle complete!
   → Panel closes
   → Dialogue: "The curtains are open..."
```

### Alternative Order:
```
1. Player clicks Right Button first
   → Sprite changes to "Right Open" state
   
2. Player clicks Left Button
   → Sprite changes to "Both Open" state
   → Puzzle complete!
```

### Toggle Feature:
```
Player can click buttons multiple times:
- Click Left → Opens left
- Click Left again → Closes left
- Click Right → Opens right
- Click Right again → Closes right

Must have BOTH open to complete!
```

---

## 📊 State Diagram

```
Start: Both Closed
   ↓
Click Left → Left Open, Right Closed
   ↓
Click Right → Both Open ✓ (Complete!)

OR

Start: Both Closed
   ↓
Click Right → Left Closed, Right Open
   ↓
Click Left → Both Open ✓ (Complete!)
```

---

## 🎨 Visual Layout

```
┌─────────────────────────────────────┐
│ CurtainPanel (Full Screen)          │
│                                     │
│  ┌─────────────────────────────┐   │
│  │                             │   │
│  │   [Curtain Image]           │   │
│  │   (Shows current state)     │   │
│  │                             │   │
│  │  ┌──────────┬──────────┐   │   │
│  │  │  Left    │  Right   │   │   │
│  │  │  Button  │  Button  │   │   │
│  │  │ (Transp) │ (Transp) │   │   │
│  │  └──────────┴──────────┘   │   │
│  └─────────────────────────────┘   │
│                                     │
│                          [X] Close  │
└─────────────────────────────────────┘
```

---

## 🧪 Testing

### Test 1: Left First
```
1. Press Play
2. Interact with Window Curtains
3. Panel opens (shows both closed)
4. Click left button
5. Sprite changes to left open
6. Click right button
7. Sprite changes to both open
8. Panel closes automatically
9. Dialogue appears
```

### Test 2: Right First
```
1. Press Play
2. Interact with Window Curtains
3. Panel opens
4. Click right button
5. Sprite changes to right open
6. Click left button
7. Sprite changes to both open
8. Panel closes
9. Dialogue appears
```

### Test 3: Toggle
```
1. Press Play
2. Open panel
3. Click left → Opens
4. Click left again → Closes
5. Click left again → Opens
6. Click right → Both open
7. Complete!
```

---

## 🔍 Inspector Setup Example

```
CurtainPanel
├─ CurtainPuzzleUI
│  ├─ UI References
│  │  ├─ Curtain Panel: CurtainPanel
│  │  ├─ Left Curtain Button: LeftCurtainButton
│  │  ├─ Right Curtain Button: RightCurtainButton
│  │  └─ Close Button: CloseButton
│  ├─ Curtain Sprites (4 States)
│  │  ├─ Curtain Image: CurtainImage
│  │  ├─ Both Closed Sprite: curtain_both_closed
│  │  ├─ Left Open Sprite: curtain_left_open
│  │  ├─ Right Open Sprite: curtain_right_open
│  │  └─ Both Open Sprite: curtain_both_open
│  └─ Audio
│     └─ Curtain Open Sound: curtain_sound
├─ CurtainImage (Image)
│  └─ Sprite: curtain_both_closed (default)
├─ LeftCurtainButton (Button)
├─ RightCurtainButton (Button)
└─ CloseButton (Button)
```

---

## 🐛 Common Issues

### Issue 1: Sprite Doesn't Change
```
Problem: Click button but sprite stays the same
Cause: Sprites not assigned
Fix: Assign all 4 sprites in Inspector
```

### Issue 2: Wrong Sprite Shows
```
Problem: Wrong state sprite appears
Cause: Sprites assigned to wrong slots
Fix: Check sprite assignment:
  - Both Closed = State 1 (your first image)
  - Left Open = State 2 (your second image)
  - Right Open = State 3 (your third image)
  - Both Open = State 4 (your fourth image)
```

### Issue 3: Buttons Don't Work
```
Problem: Click buttons but nothing happens
Cause: Buttons not assigned or no EventSystem
Fix: 
  - Assign buttons in Inspector
  - Check if EventSystem exists in scene
```

### Issue 4: Panel Doesn't Close
```
Problem: Both curtains open but panel stays
Cause: Room07UIManager not assigned
Fix: Assign Room07_Manager to panel's script
```

---

## 📝 Sprite Assignment Guide

Based sa images mo:

```
Image 1 (Both Closed):
  → Assign to "Both Closed Sprite"
  
Image 2 (Left Open, Right Closed):
  → Assign to "Left Open Sprite"
  
Image 3 (Left Closed, Right Open):
  → Assign to "Right Open Sprite"
  
Image 4 (Both Open):
  → Assign to "Both Open Sprite"
```

---

## ✅ Checklist

### Setup:
- [ ] CurtainPanel created
- [ ] CurtainPuzzleUI script added
- [ ] CurtainImage created
- [ ] Left button created
- [ ] Right button created
- [ ] Close button created
- [ ] All 4 sprites assigned
- [ ] Buttons assigned
- [ ] Audio assigned
- [ ] Panel disabled initially

### Testing:
- [ ] Panel opens when interacting with curtains
- [ ] Left button changes sprite
- [ ] Right button changes sprite
- [ ] Both open completes puzzle
- [ ] Panel closes automatically
- [ ] Dialogue appears after
- [ ] Game resumes after

---

## 🎓 Pro Tips

1. **Transparent Buttons** - Make button backgrounds transparent so only curtain shows
2. **Button Size** - Make buttons cover entire curtain area for easy clicking
3. **Visual Feedback** - Add button hover effects (optional)
4. **Sound** - Use curtain sliding sound for better feedback
5. **Test Both Orders** - Make sure both left-first and right-first work

---

**Setup mo na ang 4 sprites at buttons, tapos na!** 🎮✨
