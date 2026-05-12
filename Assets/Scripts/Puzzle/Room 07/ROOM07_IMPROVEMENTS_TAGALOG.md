# Room 07 - Lisa's Bedroom Improvements (Tagalog Guide)

## ✅ MGA IMPROVEMENTS

### 1. Mas Magandang Dialogue Flow ✅
- Mas natural at emotional
- Better pacing
- Deeper character development

### 2. Curtain Sprite Change ✅
- Pag natapos ang curtain puzzle → Curtain sprite changes to open version
- Visual feedback para sa player

### 3. Fade Transitions ✅
- Lahat ng cutscenes may fade in/fade out
- Lullaby fragments with black screen
- Smooth cinematic experience

---

## 🔧 PAANO I-SETUP

### Step 1: Create Cutscene Controller

1. **Create Empty GameObject**: `Room07_CutsceneController`
2. **Add script**: `Room07_CutsceneController.cs`
3. **Assign sa Inspector**:
   - **Fade Panel**: Black UI Image (full screen)
   - **Cutscene Image**: UI Image for cutscenes
   - **Tea Party Cutscene**: Sprite
   - **Doll Cutscene**: Sprite
   - **Lullaby Fragment 1, 2, 3**: Audio clips
   - **Lullaby Audio Source**: AudioSource component

### Step 2: Setup Fade Panel

1. **Sa Canvas**, create **Panel**: `FadePanel`
2. **Full screen**:
   - Anchors: Stretch all
   - Left: 0, Right: 0, Top: 0, Bottom: 0
3. **Color**: Black (0, 0, 0, 0) - Start transparent!
4. **Drag to CutsceneController**: Fade Panel field

### Step 3: Setup Cutscene Image

1. **Sa Canvas**, create **Image**: `CutsceneImage`
2. **Full screen or centered**
3. **Inactive by default** (uncheck)
4. **Drag to CutsceneController**: Cutscene Image field

### Step 4: Update UI Manager

1. **Select Room07UIManager**
2. **Assign**:
   - **Curtain Object**: Curtain GameObject from scene
   - **Closed Curtain Sprite**: Closed sprite
   - **Open Curtain Sprite**: Open sprite

### Step 5: Setup Curtain GameObject

1. **Find curtain** sa scene
2. **Check**: May SpriteRenderer
3. **Initial sprite**: Closed curtain
4. **Drag to UI Manager**: Curtain Object field

---

## 🎬 ANO ANG MANGYAYARI

### Curtain Puzzle:
1. Solve puzzle
2. **Curtain sprite changes** to open ✅
3. Dialogue: "It feels... lighter in here now."
4. Continue gameplay

### Tea Party Cutscene:
1. Complete tea party
2. **Fade to black** (0.5s)
3. **Show cutscene** (3s)
4. **Fade from black** (0.5s)
5. **Fade to black** (1.0s) - Para sa lullaby
6. **Play lullaby fragment 1**
7. **Fade from black** (1.0s)
8. Dialogue sequence
9. Continue gameplay

### Doll Cutscene:
1. Pick up doll
2. **Fade to black** (0.5s)
3. **Show cutscene** (2s)
4. **Fade from black** (0.5s)
5. **Fade to black** (1.0s) - Para sa lullaby
6. **Play lullaby fragment 2**
7. **Fade from black** (1.0s)
8. Dialogue sequence
9. Continue gameplay

### Mirror Climax:
1. Interact with mirror
2. **Fade to black** (1.0s)
3. **Play lullaby fragment 3**
4. **Fade from black** (1.0s)
5. Jumpscare!
6. Chase begins

---

## 📋 KAILANGAN MO

### Sprites:
1. **Closed Curtain** - Tied shut
2. **Open Curtain** - Open, with moonlight
3. **Tea Party Cutscene** - Lisa and Emily
4. **Doll Cutscene** - Lisa with doll

### Audio:
1. **Lullaby Fragment 1** (10-15s) - Soft melody
2. **Lullaby Fragment 2** (10-15s) - Haunting
3. **Lullaby Fragment 3** (15-20s) - Climax

---

## ✅ TESTING

### Curtain:
- [ ] Starts closed
- [ ] Solve puzzle
- [ ] **Changes to open sprite** ✅
- [ ] Dialogue plays

### Tea Party:
- [ ] Complete puzzle
- [ ] **Fade to black** ✅
- [ ] **Cutscene shows** ✅
- [ ] **Fade from black** ✅
- [ ] **Black screen for lullaby** ✅
- [ ] **Lullaby plays** ✅
- [ ] **Fade from black** ✅
- [ ] Dialogue plays
- [ ] Player can move

### Doll:
- [ ] Pick up doll
- [ ] **Fade to black** ✅
- [ ] **Cutscene shows** ✅
- [ ] **Fade from black** ✅
- [ ] **Black screen for lullaby** ✅
- [ ] **Lullaby plays** ✅
- [ ] **Fade from black** ✅
- [ ] Dialogue plays
- [ ] Player can move

### Mirror:
- [ ] Interact with mirror
- [ ] **Fade to black** ✅
- [ ] **Lullaby plays** ✅
- [ ] **Fade from black** ✅
- [ ] Jumpscare
- [ ] Chase starts

---

## 🐛 KUNG MAY PROBLEMA

### "Fade panel hindi gumagana"
- Check kung assigned ang FadePanel
- Check kung may Image component
- Check kung black ang color (0, 0, 0, 0)
- Check kung full screen

### "Cutscene image hindi lumalabas"
- Check kung assigned ang CutsceneImage
- Check kung assigned ang sprites
- Check kung nasa harap ng other UI
- Check Canvas sorting order

### "Lullaby hindi tumutugtog"
- Check kung assigned ang audio clips
- Check kung assigned ang AudioSource
- Check volume (0.7-1.0)
- Check kung imported correctly ang audio

### "Curtain sprite hindi nagbabago"
- Check kung assigned ang Curtain Object
- Check kung assigned ang Open Curtain Sprite
- Check kung may SpriteRenderer ang curtain
- Check kung natatawag ang OnCurtainsOpened()

### "Player stuck after cutscene"
- Check kung natatawag ang EnablePlayer()
- Check kung nag-re-enable ang joystick
- Check Console for errors
- Check kung natapos ang coroutine

---

## 💡 TIPS

### Para sa Better Cutscenes:
- Use high-quality sprites (1920x1080+)
- Keep short (2-3 seconds)
- Use emotional images
- Match audio to visuals

### Para sa Better Fades:
- 0.5s for quick transitions
- 1.0s for dramatic moments
- Always fade before cutscene
- Always fade after cutscene
- Longer fades for lullaby

### Para sa Better Audio:
- Normalize audio levels
- Add reverb for atmosphere
- Fade in/out sa start/end
- Match tone to story

---

## 📝 OPTIONAL: Improved Dialogues

Kung gusto mo ng better dialogue text:

1. Open `Room07_Interactable.cs`
2. Find `Room07_ShortDialogues_FINAL`
3. Replace with `Room07_ImprovedDialogues`
4. Save and test

---

**Setup complete! Test mo na!** 🎮✨

---

## 📖 DETAILED GUIDES

Para sa mas detailed instructions:
- `ROOM07_IMPROVEMENTS_SETUP.md` - Complete English guide
- `ROOM07_IMPROVEMENTS_PLAN.md` - Implementation plan

---

**Lahat ng improvements implemented na! Ready for testing!** 💪✨
