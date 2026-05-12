# Room 07 - Lisa's Bedroom Improvements Plan

## 🎯 REQUESTED IMPROVEMENTS

### 1. Dialogue Flow
- ✅ Review and improve all dialogues
- ✅ Make them more natural and emotional
- ✅ Better pacing and progression

### 2. Curtain Sprite Change
- ✅ After curtain puzzle solved → Change to open curtain sprite
- ✅ Visual feedback for puzzle completion

### 3. Fade Transitions
- ✅ All cutscenes need fade in/fade out
- ✅ Lullaby fragments need fade transitions
- ✅ Black screen for lullaby playback

### 4. Cinematic Experience
- ✅ Proper screen fading
- ✅ Player control management
- ✅ Audio transitions

---

## 📝 IMPLEMENTATION PLAN

### Phase 1: Dialogue Improvements ✅
**File**: `Room07_ImprovedDialogues.cs` (NEW)
- Rewrite all dialogues for better flow
- More emotional depth
- Better pacing
- Natural progression

### Phase 2: Curtain System ✅
**File**: `Room07UIManager.cs` (UPDATE)
- Add curtain GameObject reference
- Add open curtain sprite reference
- Change sprite after puzzle completion
- Visual feedback

### Phase 3: Fade System ✅
**File**: `Room07_CutsceneController.cs` (NEW)
- Centralized cutscene management
- Fade in/out transitions
- Black screen management
- Audio fade support

### Phase 4: Lullaby System ✅
**File**: `Room07_LullabyController.cs` (NEW)
- Lullaby fragment playback
- Black screen during playback
- Fade transitions
- Audio management

---

## 🎬 CUTSCENE FLOW

### Tea Party Cutscene:
1. Fade to black (0.5s)
2. Show cutscene image (3s)
3. Fade from black (0.5s)
4. Play dialogue sequence
5. Complete

### Doll Cutscene:
1. Fade to black (0.5s)
2. Show cutscene image (2s)
3. Fade from black (0.5s)
4. Play dialogue sequence
5. Complete

### Lullaby Fragments:
1. Fade to black (1.0s)
2. Play lullaby audio
3. Wait for audio to finish
4. Fade from black (1.0s)
5. Continue gameplay

---

## 🎨 VISUAL CHANGES

### Curtain GameObject:
- **Before Puzzle**: Closed curtain sprite
- **After Puzzle**: Open curtain sprite
- **Transition**: Instant change after puzzle completion

### Black Screen:
- Full screen black panel
- Used for cutscenes and lullaby
- Fade in/out transitions
- Blocks all input during transitions

---

## 🔊 AUDIO IMPROVEMENTS

### Lullaby Fragments:
- Fragment 1: After tea party (with fade)
- Fragment 2: After doll pickup (with fade)
- Fragment 3: At mirror climax (with fade)

### Audio Fade:
- Fade out current audio (0.5s)
- Play lullaby fragment
- Fade in after lullaby (0.5s)

---

## 📋 FILES TO CREATE/UPDATE

### NEW FILES:
1. `Room07_ImprovedDialogues.cs` - Better dialogue text
2. `Room07_CutsceneController.cs` - Centralized cutscene management
3. `Room07_LullabyController.cs` - Lullaby playback system
4. `ROOM07_IMPROVEMENTS_GUIDE.md` - Setup guide

### UPDATE FILES:
1. `Room07UIManager.cs` - Add curtain sprite change
2. `Room07_FlowController.cs` - Integrate new systems
3. `Room07_Interactable.cs` - Use improved dialogues

---

## ✅ IMPLEMENTATION CHECKLIST

### Dialogue System:
- [ ] Create improved dialogue file
- [ ] Review all dialogue text
- [ ] Ensure proper pacing
- [ ] Test in-game

### Curtain System:
- [ ] Add curtain GameObject reference
- [ ] Add open/closed sprite references
- [ ] Implement sprite change logic
- [ ] Test visual feedback

### Fade System:
- [ ] Create cutscene controller
- [ ] Implement fade in/out
- [ ] Add black screen management
- [ ] Test transitions

### Lullaby System:
- [ ] Create lullaby controller
- [ ] Implement black screen playback
- [ ] Add fade transitions
- [ ] Test audio playback

### Integration:
- [ ] Update UI Manager
- [ ] Update Flow Controller
- [ ] Update Interactable
- [ ] Test complete flow

---

## 🎮 TESTING PLAN

### Test Sequence:
1. Enter room → Intro dialogue
2. Check bed → Dialogue
3. Check wall → Dialogue
4. Check diary → Dialogue
5. **Curtain puzzle** → Sprite changes to open ✅
6. Get cup → Dialogue
7. **Tea party** → Fade to black → Cutscene → Fade in → Lullaby (black screen) → Dialogue ✅
8. Check chair → Dialogue
9. Check closet → Dialogue
10. **Toybox puzzle** → Dialogue
11. **Get doll** → Fade to black → Cutscene → Fade in → Lullaby (black screen) → Dialogue ✅
12. **Dollhouse** → Dialogue
13. Check reading table → Dialogue
14. **Mirror** → Fade to black → Lullaby (black screen) → Jumpscare → Chase ✅

---

**Ready to implement!** 🎮✨
