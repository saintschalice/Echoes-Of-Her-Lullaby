# Universal Jumpscare System - Setup Guide (Tagalog)

## 🎯 OVERVIEW

Universal jumpscare system na gumagana sa **LAHAT** ng game over scenarios:
- Emily catches player
- QTE failed
- Puzzle failed
- Any game over situation

**Flow**: Jumpscare (11 seconds) → Game Over Screen

---

## 🎨 JUMPSCARE SEQUENCE

### 3-Sprite Animation:
1. **Tilt Left** (0.3 seconds) - Emily tilted left
2. **Tilt Right** (0.3 seconds) - Emily tilted right  
3. **Center** (2+ seconds) - Emily centered, final scare
4. **Hold** (remaining time to reach 11 seconds total)

### Effects:
- ✅ Screen shake
- ✅ Flash effects (white/red)
- ✅ 11-second audio
- ✅ Fade in/out transitions
- ✅ Freezes game
- ✅ Stops all audio

---

## 🔧 UNITY SETUP

### Step 1: Create Jumpscare UI

#### 1.1 Create Canvas (if wala pa)
```
Hierarchy → Right-click → UI → Canvas
└─ Name: "JumpscareCanvas"
    ├─ Render Mode: Screen Space - Overlay
    ├─ Canvas Scaler:
    │   ├─ UI Scale Mode: Scale With Screen Size
    │   └─ Reference Resolution: 1920x1080
    └─ Sort Order: 1000 (above everything!)
```

#### 1.2 Create Jumpscare Panel
```
JumpscareCanvas → Right-click → UI → Panel
└─ Name: "JumpscarePanel"
    ├─ Anchor: Stretch (full screen)
    ├─ Left/Right/Top/Bottom: 0
    ├─ Image Component:
    │   ├─ Color: Black (0, 0, 0, 255)
    │   └─ Raycast Target: ✓ (checked)
    └─ Active: ☐ (unchecked - hidden at start)
```

#### 1.3 Create Jumpscare Image
```
JumpscarePanel → Right-click → UI → Image
└─ Name: "JumpscareImage"
    ├─ Anchor: Center
    ├─ Width: 1920 (or your sprite width)
    ├─ Height: 1080 (or your sprite height)
    ├─ Image Component:
    │   ├─ Source Image: [leave empty for now]
    │   ├─ Preserve Aspect: ✓ (checked)
    │   └─ Raycast Target: ☐ (unchecked)
    └─ Active: ✓ (checked)
```

#### 1.4 Create Flash Image (Optional)
```
JumpscarePanel → Right-click → UI → Image
└─ Name: "FlashImage"
    ├─ Anchor: Stretch (full screen)
    ├─ Left/Right/Top/Bottom: 0
    ├─ Image Component:
    │   ├─ Color: White (255, 255, 255, 0) ← Alpha 0!
    │   └─ Raycast Target: ☐ (unchecked)
    └─ Active: ☐ (unchecked - hidden at start)
```

---

### Step 2: Create JumpscareManager GameObject

```
Hierarchy → Right-click → Create Empty
└─ Name: "JumpscareManager"
    ├─ Add Component: JumpscareManager (script)
    └─ Tag: (optional) "GameController"
```

---

### Step 3: Configure JumpscareManager Script

```
JumpscareManager (Script):

[Jumpscare UI]
├─ Jumpscare Panel: [drag JumpscarePanel]
└─ Jumpscare Image: [drag JumpscareImage]

[Jumpscare Sprites]
├─ Tilt Left Sprite: [drag tilt left sprite]
├─ Tilt Right Sprite: [drag tilt right sprite]
└─ Center Sprite: [drag center sprite]

[Timing]
├─ Tilt Left Duration: 0.3
├─ Tilt Right Duration: 0.3
├─ Center Duration: 2.0
└─ Total Jumpscare Duration: 11.0 (match audio length!)

[Audio]
└─ Jumpscare Sound: [drag 11-second audio clip]

[Visual Effects]
├─ Enable Screen Shake: ✓
├─ Shake Intensity: 0.5
├─ Enable Flash: ✓
├─ Flash Color: White (255, 255, 255, 255)
└─ Flash Image: [drag FlashImage]

[Fade Settings]
├─ Fade In Duration: 0.2
└─ Fade Out Duration: 0.5
```

---

## 🎮 HOW TO USE

### Old Way (Direct Game Over):
```csharp
// OLD - Direct to game over
GameOverManager.Instance?.TriggerGameOver("Emily caught you...");
```

### New Way (With Jumpscare):
```csharp
// NEW - Jumpscare first, then game over
JumpscareManager.Instance?.TriggerJumpscare("Emily caught you...");
```

**That's it!** Jumpscare will play automatically, then show game over screen.

---

## 📝 UPDATE EXISTING SCRIPTS

### Example 1: Room 06 Hallway Controller

**Before**:
```csharp
private void TriggerGameOver()
{
    // ...
    GameOverManager gameOverManager = FindFirstObjectByType<GameOverManager>();
    if (gameOverManager != null)
    {
        gameOverManager.TriggerGameOver("Emily caught you...");
    }
}
```

**After**:
```csharp
private void TriggerGameOver()
{
    // ...
    // Use jumpscare instead of direct game over
    if (JumpscareManager.Instance != null)
    {
        JumpscareManager.Instance.TriggerJumpscare("Emily caught you...");
    }
    else
    {
        // Fallback to direct game over if no jumpscare
        GameOverManager.Instance?.TriggerGameOver("Emily caught you...");
    }
}
```

---

### Example 2: Room 08 Mirror QTE

**Before**:
```csharp
System.Collections.IEnumerator GameOver()
{
    isQTEActive = false;
    
    // Show failure dialogue
    DialogueSystemV2.Instance?.StartDialogue(Room08_Dialogues.QTE_FAILED, "Lisa");
    
    // ... wait for dialogue ...
    
    // Trigger game over
    Debug.Log("[Room08] QTE Failed - Game Over!");
}
```

**After**:
```csharp
System.Collections.IEnumerator GameOver()
{
    isQTEActive = false;
    
    // Show failure dialogue
    DialogueSystemV2.Instance?.StartDialogue(Room08_Dialogues.QTE_FAILED, "Lisa");
    
    while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
    {
        yield return null;
    }
    
    yield return new WaitForSeconds(0.5f);
    
    // Trigger jumpscare + game over
    if (JumpscareManager.Instance != null)
    {
        JumpscareManager.Instance.TriggerJumpscare("Time ran out...");
    }
    else
    {
        GameOverManager.Instance?.TriggerGameOver("Time ran out...");
    }
}
```

---

### Example 3: Cinematic Chase Trigger

**Before**:
```csharp
private void TriggerGameOver()
{
    // ...
    GameOverManager gameOverManager = FindFirstObjectByType<GameOverManager>();
    if (gameOverManager != null)
    {
        gameOverManager.TriggerGameOver(gameOverMessage);
    }
}
```

**After**:
```csharp
private void TriggerGameOver()
{
    // ...
    // Use jumpscare for Emily catches
    if (JumpscareManager.Instance != null)
    {
        JumpscareManager.Instance.TriggerJumpscare(gameOverMessage);
    }
    else
    {
        GameOverManager.Instance?.TriggerGameOver(gameOverMessage);
    }
}
```

---

## 🎨 SPRITE REQUIREMENTS

### Tilt Left Sprite:
- Emily's face tilted to the left
- Scary expression
- High contrast
- Recommended size: 1920x1080 or larger

### Tilt Right Sprite:
- Emily's face tilted to the right
- Scary expression
- High contrast
- Recommended size: 1920x1080 or larger

### Center Sprite:
- Emily's face centered (final scare!)
- Most intense/scary expression
- High contrast
- Recommended size: 1920x1080 or larger

**Tip**: All sprites should have same dimensions for smooth transition!

---

## 🔊 AUDIO REQUIREMENTS

### Jumpscare Sound:
- **Duration**: 11 seconds (or adjust `totalJumpscareDuration`)
- **Format**: WAV or OGG (high quality)
- **Content**: 
  - Build-up sound
  - Scream/scare sound
  - Tension/horror ambience
- **Volume**: Should be loud and impactful

**Import Settings**:
```
Audio Clip:
├─ Load Type: Decompress On Load
├─ Preload Audio Data: ✓
├─ Compression Format: PCM (uncompressed for quality)
└─ Sample Rate Setting: Preserve Sample Rate
```

---

## ⚙️ TIMING CUSTOMIZATION

### Adjust Sprite Durations:

```csharp
// Quick flashes (intense)
Tilt Left Duration: 0.2
Tilt Right Duration: 0.2
Center Duration: 1.5

// Slower build-up (suspenseful)
Tilt Left Duration: 0.5
Tilt Right Duration: 0.5
Center Duration: 3.0

// Very quick (shocking)
Tilt Left Duration: 0.1
Tilt Right Duration: 0.1
Center Duration: 2.0
```

**Important**: Total of all durations + fade times should not exceed `totalJumpscareDuration`!

---

## 🎬 VISUAL EFFECTS

### Screen Shake:
- **Enable**: Adds intensity to jumpscare
- **Intensity**: 0.5 = moderate, 1.0 = intense, 0.2 = subtle
- **Continuous**: Shakes throughout entire jumpscare

### Flash Effect:
- **Enable**: Adds impact at key moments
- **Color**: White (classic), Red (horror), Yellow (intense)
- **Timing**: Flashes at start and when center sprite appears

### Fade In/Out:
- **Fade In**: Smooth entrance (0.2 seconds recommended)
- **Fade Out**: Smooth exit to game over (0.5 seconds recommended)

---

## 🐛 TROUBLESHOOTING

### Issue: "Jumpscare doesn't show"

**Check**:
1. JumpscarePanel is assigned in JumpscareManager
2. JumpscarePanel is child of Canvas
3. Canvas Sort Order is high (e.g., 1000)
4. JumpscarePanel starts inactive (unchecked)

---

### Issue: "Sprites don't change"

**Check**:
1. All 3 sprites are assigned in JumpscareManager
2. JumpscareImage is assigned
3. Sprites are imported correctly (Texture Type: Sprite 2D)

---

### Issue: "Audio doesn't play"

**Check**:
1. Jumpscare Sound is assigned
2. AudioManager exists in scene
3. Audio clip is imported correctly
4. Audio clip duration matches `totalJumpscareDuration`

---

### Issue: "Screen shake too intense/weak"

**Solution**:
- Adjust `Shake Intensity` in JumpscareManager
- 0.2 = subtle, 0.5 = moderate, 1.0 = intense

---

### Issue: "Jumpscare too short/long"

**Solution**:
- Adjust `Total Jumpscare Duration` to match audio length
- Adjust individual sprite durations
- Formula: `fadeIn + tiltLeft + tiltRight + center + remaining = total`

---

### Issue: "Game over shows before jumpscare ends"

**Cause**: Total duration too short

**Solution**:
- Increase `Total Jumpscare Duration`
- Or decrease individual sprite durations

---

## 📋 SETUP CHECKLIST

### UI Setup:
- [ ] JumpscareCanvas created (Sort Order: 1000)
- [ ] JumpscarePanel created (full screen, black)
- [ ] JumpscareImage created (centered)
- [ ] FlashImage created (optional, full screen)

### GameObject Setup:
- [ ] JumpscareManager GameObject created
- [ ] JumpscareManager script attached

### Script Configuration:
- [ ] Jumpscare Panel assigned
- [ ] Jumpscare Image assigned
- [ ] All 3 sprites assigned (tilt left, tilt right, center)
- [ ] Jumpscare Sound assigned (11 seconds)
- [ ] Flash Image assigned (if using flash)
- [ ] Timing values set
- [ ] Visual effects configured

### Sprite Assets:
- [ ] Tilt left sprite imported
- [ ] Tilt right sprite imported
- [ ] Center sprite imported
- [ ] All sprites same dimensions
- [ ] All sprites high quality

### Audio Assets:
- [ ] Jumpscare audio imported (11 seconds)
- [ ] Audio format: WAV or OGG
- [ ] Audio quality: High

### Code Updates:
- [ ] Updated Room 06 TriggerGameOver()
- [ ] Updated Room 08 GameOver()
- [ ] Updated CinematicChaseTrigger TriggerGameOver()
- [ ] Updated any other game over triggers

### Testing:
- [ ] Test Emily catches player → Jumpscare plays
- [ ] Test QTE failed → Jumpscare plays
- [ ] Test puzzle failed → Jumpscare plays
- [ ] Jumpscare duration matches audio
- [ ] Sprites change correctly
- [ ] Screen shake works
- [ ] Flash effect works (if enabled)
- [ ] Game over screen shows after jumpscare

---

## 🎯 QUICK START

### Minimum Setup (5 Minutes):

1. **Create UI**:
   - Canvas → Panel (JumpscarePanel)
   - Panel → Image (JumpscareImage)

2. **Create Manager**:
   - Empty GameObject → Add JumpscareManager script

3. **Assign**:
   - Drag panel and image to script
   - Drag 3 sprites to script
   - Drag audio to script

4. **Update Code**:
   - Replace `GameOverManager.TriggerGameOver()` 
   - With `JumpscareManager.TriggerJumpscare()`

5. **Test**:
   - Trigger game over
   - Watch jumpscare play
   - See game over screen after

**Done!** 🎉

---

## 💡 PRO TIPS

### Tip 1: Sprite Timing
- Keep tilt sprites SHORT (0.1-0.3s) for disorienting effect
- Keep center sprite LONGER (2-3s) for maximum scare

### Tip 2: Audio Sync
- Match `totalJumpscareDuration` EXACTLY to audio length
- Test multiple times to ensure sync

### Tip 3: Screen Shake
- Use moderate shake (0.3-0.5) for horror
- Use intense shake (0.8-1.0) for action/panic

### Tip 4: Flash Color
- White = Classic jumpscare
- Red = Horror/blood
- Yellow = Intense/shocking
- Black = Darkness/void

### Tip 5: Testing
- Test jumpscare from EVERY game over scenario
- Ensure consistent experience across all triggers

---

## 📝 EXAMPLE USAGE

### Complete Example:

```csharp
// In any script that triggers game over:

public void OnPlayerCaught()
{
    // Stop player
    DisablePlayer();
    
    // Trigger jumpscare + game over
    if (JumpscareManager.Instance != null)
    {
        JumpscareManager.Instance.TriggerJumpscare("Emily caught you...");
    }
    else
    {
        // Fallback if jumpscare not available
        GameOverManager.Instance?.TriggerGameOver("Emily caught you...");
    }
}
```

---

**Setup complete! Lahat ng game over may jumpscare na!** 👻✨
