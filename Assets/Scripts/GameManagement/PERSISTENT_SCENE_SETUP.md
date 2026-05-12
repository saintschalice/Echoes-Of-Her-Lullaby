# Jumpscare - Persistent Scene Setup Guide

## 🎯 SETUP SA PERSISTENT SCENE

Kung nilagay mo na sa PersistentScene kasama ng GameOverManager, follow this guide.

---

## 📋 STEP-BY-STEP SETUP

### Step 1: Check PersistentScene Hierarchy

**Dapat ganito ang structure**:

```
PersistentScene:
│
├─ GameOverManager (existing)
│   └─ GameOverManager (Script)
│
├─ JumpscareManager (NEW!)
│   └─ JumpscareManager (Script)
│
└─ Canvas (or PersistentUI)
    ├─ Render Mode: Screen Space - Overlay
    ├─ Sort Order: 1000 (IMPORTANT!)
    │
    ├─ GameOverUI (existing)
    │   ├─ BlackBackgroundFader
    │   ├─ GameOverMessagePanel
    │   └─ GameOverOptionsPanel
    │
    └─ JumpscarePanel (NEW!)
        ├─ Active: ☐ (UNCHECKED!)
        ├─ JumpscareImage
        │   └─ Active: ✓ (checked)
        └─ FlashImage (optional)
            └─ Active: ☐ (unchecked)
```

---

### Step 2: Create JumpscareManager GameObject

**In PersistentScene**:

1. Right-click in Hierarchy
2. Create Empty
3. Name: `JumpscareManager`
4. Add Component → `JumpscareManager` script

**Position**: Doesn't matter (it's not visual)

---

### Step 3: Create JumpscarePanel in Canvas

**Find or create Canvas**:
- If may existing Canvas na (e.g., PersistentUI), use that
- If wala, create new Canvas

**Canvas Settings**:
```
Canvas:
├─ Render Mode: Screen Space - Overlay
├─ Canvas Scaler:
│   ├─ UI Scale Mode: Scale With Screen Size
│   └─ Reference Resolution: 1920x1080
└─ Sort Order: 1000 (CRITICAL!)
```

**Create JumpscarePanel**:
1. Right-click Canvas
2. UI → Panel
3. Name: `JumpscarePanel`

**JumpscarePanel Settings**:
```
JumpscarePanel:
├─ Active: ☐ (UNCHECKED - hidden at start!)
├─ RectTransform:
│   ├─ Anchor: Stretch (full screen)
│   └─ Left/Right/Top/Bottom: 0
└─ Image:
    ├─ Color: Black (0, 0, 0, 255)
    └─ Raycast Target: ✓
```

---

### Step 4: Create JumpscareImage

**Inside JumpscarePanel**:

1. Right-click JumpscarePanel
2. UI → Image
3. Name: `JumpscareImage`

**JumpscareImage Settings**:
```
JumpscareImage:
├─ Active: ✓ (CHECKED)
├─ RectTransform:
│   ├─ Anchor: Center
│   ├─ Width: 1920 (or your sprite size)
│   └─ Height: 1080 (or your sprite size)
└─ Image:
    ├─ Source Image: (empty - set by script)
    ├─ Preserve Aspect: ✓
    └─ Raycast Target: ☐ (unchecked)
```

---

### Step 5: Create FlashImage (Optional)

**Inside JumpscarePanel**:

1. Right-click JumpscarePanel
2. UI → Image
3. Name: `FlashImage`

**FlashImage Settings**:
```
FlashImage:
├─ Active: ☐ (UNCHECKED)
├─ RectTransform:
│   ├─ Anchor: Stretch (full screen)
│   └─ Left/Right/Top/Bottom: 0
└─ Image:
    ├─ Source Image: (none or white sprite)
    ├─ Color: White (255, 255, 255, 0) ← Alpha 0!
    └─ Raycast Target: ☐ (unchecked)
```

---

### Step 6: Assign References to JumpscareManager

**Select JumpscareManager GameObject**:

**In Inspector**:
```
JumpscareManager (Script):

[Jumpscare UI]
├─ Jumpscare Panel: [drag JumpscarePanel here]
└─ Jumpscare Image: [drag JumpscareImage here]

[Jumpscare Sprites]
├─ Tilt Left Sprite: [drag your sprite]
├─ Tilt Right Sprite: [drag your sprite]
└─ Center Sprite: [drag your sprite]

[Timing]
├─ Tilt Left Duration: 0.3
├─ Tilt Right Duration: 0.3
├─ Center Duration: 2.0
└─ Total Jumpscare Duration: 11.0

[Audio]
└─ Jumpscare Sound: [drag your 11-second audio]

[Visual Effects]
├─ Enable Screen Shake: ✓
├─ Shake Intensity: 0.5
├─ Enable Flash: ✓
├─ Flash Color: White (255, 255, 255, 255)
└─ Flash Image: [drag FlashImage here]

[Fade Settings]
├─ Fade In Duration: 0.2
└─ Fade Out Duration: 0.5
```

**IMPORTANT**: Lahat ng fields dapat may value! Walang "None"!

---

### Step 7: Test Setup

**Add JumpscareDiagnostic script** (temporary):

1. Create empty GameObject: `JumpscareTester`
2. Add Component → `JumpscareDiagnostic` script
3. Play game
4. Press **J** to test jumpscare
5. Press **D** to show diagnostic info

**Expected Result**:
- Press J → Jumpscare plays
- Press D → Console shows all ✅

**If not working**:
- Check Console for errors
- Check all references assigned
- Check Canvas sort order = 1000

---

## 🔍 VERIFICATION CHECKLIST

### PersistentScene:
- [ ] JumpscareManager GameObject exists
- [ ] JumpscareManager script attached
- [ ] Canvas exists with Sort Order 1000+
- [ ] JumpscarePanel is child of Canvas
- [ ] JumpscareImage is child of JumpscarePanel

### JumpscareManager Inspector:
- [ ] Jumpscare Panel: Assigned (not None)
- [ ] Jumpscare Image: Assigned (not None)
- [ ] Tilt Left Sprite: Assigned (not None)
- [ ] Tilt Right Sprite: Assigned (not None)
- [ ] Center Sprite: Assigned (not None)
- [ ] Jumpscare Sound: Assigned (not None)
- [ ] Flash Image: Assigned or empty (optional)

### GameObject States:
- [ ] JumpscarePanel: Active = ☐ (unchecked)
- [ ] JumpscareImage: Active = ✓ (checked)
- [ ] FlashImage: Active = ☐ (unchecked)

### Test:
- [ ] Play game
- [ ] Press J → Jumpscare plays
- [ ] Press D → All ✅ in console
- [ ] Trigger actual game over → Jumpscare plays

---

## 🐛 COMMON ISSUES

### Issue: "JumpscareManager.Instance is null"

**Cause**: Script not in scene or Awake() not called

**Fix**:
1. Check JumpscareManager GameObject exists in PersistentScene
2. Check script is attached and enabled
3. Check PersistentScene is loaded at game start

---

### Issue: "Panel doesn't show"

**Cause**: Canvas sort order too low or not assigned

**Fix**:
1. Check Canvas Sort Order = 1000 or higher
2. Check JumpscarePanel assigned in JumpscareManager
3. Check JumpscarePanel is child of Canvas

---

### Issue: "Sprites don't change"

**Cause**: Sprites not assigned

**Fix**:
1. Check all 3 sprites assigned in Inspector
2. Check sprites imported as Sprite (2D and UI)
3. Check JumpscareImage assigned

---

## 💡 PRO TIPS

### Tip 1: Use Existing Canvas
If may Canvas na sa PersistentScene (e.g., for GameOverUI), use that! Just add JumpscarePanel as child.

### Tip 2: Sort Order
Make sure Canvas sort order is HIGH (1000+) para lumabas sa ibabaw ng lahat.

### Tip 3: DontDestroyOnLoad
JumpscareManager automatically calls DontDestroyOnLoad sa Awake(), so it persists across scenes.

### Tip 4: Test Early
Use JumpscareDiagnostic script to test BEFORE triggering actual game over.

---

## 📊 VISUAL GUIDE

### Before (Wrong):
```
❌ JumpscareManager in Room scene (destroyed on scene change)
❌ Canvas sort order = 0 (hidden behind game)
❌ References not assigned (None in Inspector)
```

### After (Correct):
```
✅ JumpscareManager in PersistentScene (persists)
✅ Canvas sort order = 1000 (above everything)
✅ All references assigned (no None)
```

---

## 🎯 FINAL TEST

### Test Sequence:

1. **Play game** from PersistentScene
2. **Press D** → Check diagnostic (all ✅)
3. **Press J** → Test jumpscare (should play)
4. **Trigger actual game over** → Jumpscare plays
5. **After jumpscare** → Game over screen shows

**If all steps work**: ✅ Setup complete!

**If any step fails**: Check troubleshooting section

---

## 📝 QUICK REFERENCE

### Hierarchy:
```
PersistentScene
├─ JumpscareManager (script)
└─ Canvas (sort order: 1000)
    └─ JumpscarePanel (inactive)
        └─ JumpscareImage (active)
```

### Inspector:
```
JumpscareManager:
├─ Panel: ✓ assigned
├─ Image: ✓ assigned
├─ Sprites: ✓ all 3 assigned
└─ Audio: ✓ assigned
```

### Test:
```
Press J → Jumpscare plays
Press D → All ✅
Game over → Jumpscare → Game over screen
```

---

**Follow this guide and jumpscare should work perfectly!** 👻✨

---

## 🆘 STILL NOT WORKING?

1. **Check Console** for errors
2. **Use JumpscareDiagnostic** script (press D)
3. **Verify all references** assigned (no None)
4. **Check Canvas sort order** = 1000+
5. **Test with J key** before actual game over

**If still broken**: Check `JUMPSCARE_TROUBLESHOOTING.md` for more help!
