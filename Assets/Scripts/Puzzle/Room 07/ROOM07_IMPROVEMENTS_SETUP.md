# Room 07 - Lisa's Bedroom Improvements Setup Guide

## ✅ IMPROVEMENTS IMPLEMENTED

### 1. Better Dialogue Flow ✅
- More natural and emotional progression
- Better pacing
- Improved character depth

### 2. Curtain Sprite Change ✅
- Curtain changes to open sprite after puzzle
- Visual feedback for completion

### 3. Fade Transitions ✅
- All cutscenes have fade in/out
- Lullaby fragments with black screen
- Smooth cinematic experience

---

## 🔧 UNITY SETUP

### Step 1: Add Room07_CutsceneController

1. **Create Empty GameObject**: `Room07_CutsceneController`
2. **Add Component**: `Room07_CutsceneController` script
3. **Configure in Inspector**:

```
Room07_CutsceneController:
├─ Fade System:
│   ├─ Fade Panel: [Drag UI Image - full screen black panel]
│   └─ Fade Duration: 0.5
│
├─ Cutscene Images:
│   ├─ Cutscene Image: [Drag UI Image for cutscenes]
│   ├─ Tea Party Cutscene: [Assign sprite]
│   └─ Doll Cutscene: [Assign sprite]
│
├─ Lullaby Audio:
│   ├─ Lullaby Fragment 1: [Assign audio clip]
│   ├─ Lullaby Fragment 2: [Assign audio clip]
│   ├─ Lullaby Fragment 3: [Assign audio clip]
│   └─ Lullaby Audio Source: [Drag AudioSource component]
│
└─ Debug:
    └─ Debug Mode: ☑
```

### Step 2: Setup Fade Panel UI

1. **In Canvas**, create **Panel**: `FadePanel`
2. **Set to full screen**:
   - Anchors: Stretch (all corners)
   - Left: 0, Right: 0, Top: 0, Bottom: 0
3. **Set color**: Black (0, 0, 0, 0) - Start transparent!
4. **Add Image component** if not present
5. **Set Raycast Target**: ☑ (blocks input during fade)
6. **Drag to Cutscene Controller**: Fade Panel field

### Step 3: Setup Cutscene Image UI

1. **In Canvas**, create **Image**: `CutsceneImage`
2. **Set to full screen** or centered:
   - Anchors: Center or Stretch
   - Size: 1920x1080 (or your resolution)
3. **Set to inactive** by default (uncheck in Inspector)
4. **Drag to Cutscene Controller**: Cutscene Image field

### Step 4: Update Room07UIManager

1. **Select Room07UIManager** GameObject
2. **Add new fields**:

```
Room07UIManager:
├─ Curtain System:
│   ├─ Curtain Object: [Drag curtain GameObject from scene]
│   ├─ Closed Curtain Sprite: [Assign closed sprite]
│   └─ Open Curtain Sprite: [Assign open sprite]
```

### Step 5: Setup Curtain GameObject

1. **Find curtain GameObject** in scene
2. **Ensure it has SpriteRenderer**
3. **Set initial sprite**: Closed curtain sprite
4. **Drag to UI Manager**: Curtain Object field

### Step 6: Assign Audio Clips

1. **Lullaby Fragment 1**: After tea party
2. **Lullaby Fragment 2**: After doll pickup
3. **Lullaby Fragment 3**: At mirror climax
4. **Create AudioSource** if needed:
   - Add AudioSource component to CutsceneController
   - Play On Awake: ☐ (unchecked)
   - Loop: ☐ (unchecked)
   - Volume: 0.7-1.0

---

## 🎬 HOW IT WORKS

### Curtain Puzzle Flow:

1. **Player solves curtain puzzle**
2. **OnCurtainsOpened() called**
3. **Curtain sprite changes** to open version ✅
4. **Dialogue plays**: "It feels... lighter in here now."
5. **Player can continue**

### Tea Party Cutscene Flow:

1. **Player completes tea party**
2. **Fade to black** (0.5s)
3. **Show cutscene image** (3s)
4. **Fade from black** (0.5s)
5. **Fade to black** (1.0s) - For lullaby
6. **Play lullaby fragment 1**
7. **Fade from black** (1.0s)
8. **Show dialogue sequence**
9. **Player can continue**

### Doll Cutscene Flow:

1. **Player picks up doll**
2. **Fade to black** (0.5s)
3. **Show cutscene image** (2s)
4. **Fade from black** (0.5s)
5. **Fade to black** (1.0s) - For lullaby
6. **Play lullaby fragment 2**
7. **Fade from black** (1.0s)
8. **Show dialogue sequence**
9. **Player can continue**

### Mirror Climax Flow:

1. **Player interacts with mirror**
2. **Fade to black** (1.0s)
3. **Play lullaby fragment 3**
4. **Fade from black** (1.0s)
5. **Jumpscare sequence**
6. **Chase begins**

---

## 📋 CANVAS HIERARCHY

```
Canvas (Room 07)
├─ FadePanel (Image - Black, full screen)
│   └─ Color: (0, 0, 0, 0) initially
│
├─ CutsceneImage (Image - full screen)
│   └─ Initially inactive
│
├─ CurtainPanel (Puzzle UI)
├─ CabinetPanel (Puzzle UI)
├─ TeaPartyPanel (Puzzle UI)
├─ ToyboxPanel (Puzzle UI)
└─ DollhousePanel (Puzzle UI)
```

---

## 🎨 SPRITE REQUIREMENTS

### Curtain Sprites:
1. **Closed Curtain**: Tied shut with knots
2. **Open Curtain**: Curtains pulled open, moonlight visible

### Cutscene Sprites:
1. **Tea Party Cutscene**: Lisa and Emily having tea
2. **Doll Cutscene**: Lisa holding Emily's doll

---

## 🔊 AUDIO REQUIREMENTS

### Lullaby Fragments:
1. **Fragment 1** (10-15s): Soft, comforting melody
2. **Fragment 2** (10-15s): Slightly more haunting
3. **Fragment 3** (15-20s): Full, emotional climax

### Audio Format:
- Format: WAV or OGG
- Sample Rate: 44100 Hz
- Channels: Stereo or Mono
- Compression: None or Vorbis

---

## ✅ TESTING CHECKLIST

### Curtain System:
- [ ] Curtain starts with closed sprite
- [ ] Solve curtain puzzle
- [ ] Curtain changes to open sprite ✅
- [ ] Dialogue plays correctly
- [ ] Visual change is noticeable

### Tea Party Cutscene:
- [ ] Complete tea party puzzle
- [ ] Screen fades to black ✅
- [ ] Cutscene image shows ✅
- [ ] Screen fades from black ✅
- [ ] Screen fades to black for lullaby ✅
- [ ] Lullaby plays (audio audible) ✅
- [ ] Screen fades from black ✅
- [ ] Dialogue sequence plays
- [ ] Player controls restored

### Doll Cutscene:
- [ ] Pick up doll from toybox
- [ ] Screen fades to black ✅
- [ ] Cutscene image shows ✅
- [ ] Screen fades from black ✅
- [ ] Screen fades to black for lullaby ✅
- [ ] Lullaby plays (audio audible) ✅
- [ ] Screen fades from black ✅
- [ ] Dialogue sequence plays
- [ ] Player controls restored

### Mirror Climax:
- [ ] Interact with mirror (all puzzles complete)
- [ ] Screen fades to black ✅
- [ ] Lullaby fragment 3 plays ✅
- [ ] Screen fades from black ✅
- [ ] Jumpscare triggers
- [ ] Chase begins

---

## 🐛 TROUBLESHOOTING

### Issue: "Fade panel not working"
**Solution**:
- Check if FadePanel is assigned in CutsceneController
- Check if FadePanel has Image component
- Check if FadePanel color is black (0, 0, 0, 0)
- Check if FadePanel is full screen

### Issue: "Cutscene image not showing"
**Solution**:
- Check if CutsceneImage is assigned
- Check if cutscene sprites are assigned
- Check if CutsceneImage is in front of other UI
- Check Canvas sorting order

### Issue: "Lullaby not playing"
**Solution**:
- Check if audio clips are assigned
- Check if AudioSource is assigned
- Check AudioSource volume (should be 0.7-1.0)
- Check if audio clips are imported correctly

### Issue: "Curtain sprite not changing"
**Solution**:
- Check if Curtain Object is assigned in UI Manager
- Check if Open Curtain Sprite is assigned
- Check if curtain has SpriteRenderer component
- Check if OnCurtainsOpened() is being called

### Issue: "Player stuck after cutscene"
**Solution**:
- Check if EnablePlayer() is being called
- Check if joystick is being re-enabled
- Check Console for errors
- Verify cutscene coroutine completes

---

## 💡 TIPS

### For Better Cutscenes:
- Use high-quality sprites (1920x1080 or higher)
- Keep cutscene duration short (2-3 seconds)
- Use emotional, impactful images
- Match audio to visual tone

### For Better Fades:
- Fade duration: 0.5s for quick, 1.0s for dramatic
- Always fade to black before showing cutscene
- Always fade from black after cutscene
- Use longer fades for lullaby (more cinematic)

### For Better Audio:
- Normalize audio levels
- Add slight reverb for atmosphere
- Fade in/out at start/end of clips
- Match lullaby tone to story progression

---

## 📝 OPTIONAL: Use Improved Dialogues

If you want to use the improved dialogue text:

1. **Open**: `Room07_Interactable.cs`
2. **Find**: `Room07_ShortDialogues_FINAL`
3. **Replace with**: `Room07_ImprovedDialogues`
4. **Save and test**

**Note**: Make sure to update all references!

---

**Setup complete! Test all systems!** 🎮✨
