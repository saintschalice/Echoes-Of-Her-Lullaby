# Room 08 (Lisa's Bathroom) - New Changes Summary

## MAJOR CHANGES

### 1. QTE Changes ⭐
**OLD**:
- 15 taps on moving targets
- 2 minutes (120 seconds) time limit
- 3 seconds per tap
- 3 failures allowed

**NEW**:
- ✅ **50 taps** on full screen
- ✅ **25 seconds** time limit
- ✅ **No tap targets** - tap anywhere on screen
- ✅ **No failures** - only time limit matters

### 2. Evidence Changes
**OLD**:
- Bandages (evidence)
- Torn Clothes (evidence)
- Apology Note (evidence)
- Hammer (tool)

**NEW**:
- ❌ **Removed Bandages**
- ✅ Torn Clothes (evidence)
- ✅ Apology Note (evidence)
- ✅ Hammer (tool)

### 3. Emily in Mirror Changes
**OLD**:
- Emily visible in mirror from start
- Always there watching

**NEW**:
- ✅ **Emily NOT visible initially**
- ✅ **Emily appears AFTER all items collected**
- ✅ **Emily disappears after mirror breaks**

### 4. Mirror Visual Changes
**OLD**:
- Mirror cracks during QTE
- No sprite change after breaking

**NEW**:
- ✅ **Normal mirror sprite** before QTE
- ✅ **Broken mirror sprite** after QTE
- ✅ **Passage becomes visible** after breaking
- ✅ **Passage is interactable** after breaking

---

## DETAILED CHANGES

### Room08_MirrorQTE.cs

#### QTE Settings
```csharp
// OLD
public int totalTaps = 15;
public float totalTimeLimit = 120f; // 2 minutes
public float tapTimeWindow = 3.0f;
public int maxFailures = 3;
public GameObject tapTargetPrefab;
public RectTransform tapTargetParent;

// NEW
public int totalTaps = 50;
public float totalTimeLimit = 25f; // 25 seconds
public int maxFailures = 0; // No failures
public Image fullScreenTapArea; // Full screen tap
```

#### QTE Mechanics
```csharp
// OLD: Spawn moving targets
void SpawnTapTarget()
{
    currentTarget = Instantiate(tapTargetPrefab, tapTargetParent);
    // Random position
    // 3 second timer per tap
}

// NEW: Full screen tap
void StartQTE()
{
    // Setup full screen button
    Button tapButton = fullScreenTapArea.GetComponent<Button>();
    tapButton.onClick.AddListener(OnScreenTapped);
}

void OnScreenTapped()
{
    // Tap anywhere on screen
    currentTap++;
}
```

#### Timer Display
```csharp
// OLD: Minutes:Seconds format
string timeString = $"{minutes}:{seconds:00}";

// NEW: Seconds with decimal
string timeString = totalTimeRemaining.ToString("F1") + "s";
```

---

### Room08_FlowController.cs

#### Evidence Tracking
```csharp
// OLD
public bool hasFoundBandages = false;
public bool hasFoundTornClothes = false;
public bool hasFoundApologyNote = false;
public bool hasFoundHammer = false;

public bool IsAllEvidenceFound()
{
    return hasFoundBandages && hasFoundTornClothes && hasFoundApologyNote;
}

// NEW
public bool hasFoundTornClothes = false;
public bool hasFoundApologyNote = false;
public bool hasFoundHammer = false;

public bool IsAllEvidenceFound()
{
    return hasFoundTornClothes && hasFoundApologyNote && hasFoundHammer;
}
```

#### Mirror Sprites
```csharp
// NEW: Added mirror sprite management
[Header("Mirror Sprites")]
public SpriteRenderer mirrorSpriteRenderer;
public Sprite mirrorNormalSprite; // Before breaking
public Sprite mirrorBrokenSprite; // After breaking
public GameObject passageObject; // Hidden initially
```

#### Start() Method
```csharp
// NEW: Hide passage and set normal mirror
void Start()
{
    // Hide Emily in mirror initially
    if (emilyInMirror != null) emilyInMirror.SetActive(false);
    
    // Hide passage initially
    if (passageObject != null) passageObject.SetActive(false);
    
    // Set normal mirror sprite
    if (mirrorSpriteRenderer != null && mirrorNormalSprite != null)
    {
        mirrorSpriteRenderer.sprite = mirrorNormalSprite;
    }
    
    // ... rest of code
}
```

#### OnMirrorBroken() Method
```csharp
// NEW: Change mirror sprite and show passage
public void OnMirrorBroken()
{
    hasBrokenMirror = true;
    canClimbThrough = true;
    
    // Stop Emily humming
    if (emilyAudioSource != null && emilyAudioSource.isPlaying)
    {
        emilyAudioSource.Stop();
    }
    
    // Hide Emily from mirror
    if (emilyInMirror != null)
    {
        emilyInMirror.SetActive(false);
    }
    
    // Change mirror sprite to broken
    if (mirrorSpriteRenderer != null && mirrorBrokenSprite != null)
    {
        mirrorSpriteRenderer.sprite = mirrorBrokenSprite;
    }
    
    // Show passage
    if (passageObject != null)
    {
        passageObject.SetActive(true);
    }
    
    StartCoroutine(MirrorBrokenSequence());
}
```

---

### Room08_Interactable.cs

#### Evidence Handling
```csharp
// OLD
void ExamineEvidence()
{
    switch (evidenceId)
    {
        case "bandages":
            StartCoroutine(ExamineBandages());
            flow.hasFoundBandages = true;
            break;
        case "apology_note":
            // ...
            break;
    }
}

// NEW: Removed bandages case
void ExamineEvidence()
{
    switch (evidenceId)
    {
        case "apology_note":
            StartCoroutine(ExamineApologyNote());
            flow.hasFoundApologyNote = true;
            break;
    }
}
```

---

## FLOW CHANGES

### OLD FLOW:
```
1. Enter bathroom
2. See Emily in mirror (always visible)
3. Find bandages
4. Find torn clothes
5. Find apology note
6. Find hammer
7. Interact with mirror
8. QTE: 15 taps on targets, 2 minutes, 3 failures allowed
9. Mirror breaks
10. Climb through
```

### NEW FLOW:
```
1. Enter bathroom
2. Mirror is normal, NO Emily visible
3. Find torn clothes (bathtub)
4. Find apology note (evidence)
5. Find hammer (medicine cabinet)
6. ✨ Emily APPEARS in mirror (after all items collected)
7. Interact with mirror
8. QTE: 50 taps anywhere, 25 seconds, no failures
9. Mirror breaks → Changes to broken sprite
10. Emily disappears from mirror
11. Passage becomes visible and interactable
12. Climb through passage
```

---

## SETUP REQUIREMENTS

### In Unity Scene:

#### 1. QTE Panel Setup
```
QTE Panel (GameObject)
├── FullScreenTapArea (Image) ⭐ NEW
│   ├── Anchor: Stretch (full screen)
│   ├── Color: Transparent or semi-transparent
│   ├── Raycast Target: ✓ (checked)
│   └── Button component (will be added by script)
├── TimerText (TextMeshProUGUI)
│   └── Text: "25.0s"
├── ProgressText (TextMeshProUGUI)
│   └── Text: "0/50"
└── MirrorImage (Image)
    └── Sprite: mirrorPhase1
```

#### 2. Room08_MirrorQTE Component
```
Room08_MirrorQTE:
├── QTE Settings
│   ├── Total Taps: 50
│   ├── Total Time Limit: 25
│   └── Max Failures: 0
├── UI References
│   ├── QTE Panel: [Assign]
│   ├── Full Screen Tap Area: [Assign FullScreenTapArea Image] ⭐
│   ├── Timer Text TMP: [Assign]
│   ├── Progress Text TMP: [Assign]
│   └── Mirror Image: [Assign]
├── Visual Effects
│   ├── Mirror Phase 1: [Normal mirror]
│   ├── Mirror Phase 2: [Slight cracks]
│   ├── Mirror Phase 3: [More cracks]
│   ├── Mirror Phase 4: [Almost shattered]
│   └── Shatter Effect: [Particle effect]
└── Audio
    ├── Tap Sound: [Glass tap]
    ├── Crack Sound: [Glass crack]
    └── Shatter Sound: [Glass shatter]
```

#### 3. Room08_FlowController Component
```
Room08_FlowController:
├── Emily Appearance
│   └── Emily In Mirror: [Assign, initially disabled]
├── Mirror Sprites ⭐ NEW
│   ├── Mirror Sprite Renderer: [Assign mirror SpriteRenderer]
│   ├── Mirror Normal Sprite: [Normal mirror sprite]
│   ├── Mirror Broken Sprite: [Broken mirror sprite]
│   └── Passage Object: [Assign passage GameObject, initially disabled]
└── Scene Transition
    └── Next Scene Name: "Room09_Master's_Bathroom"
```

#### 4. Scene Objects
```
Mirror (GameObject)
├── SpriteRenderer
│   └── Sprite: mirrorNormalSprite (initially)
└── Room08_Interactable
    └── My Type: Mirror

EmilyInMirror (GameObject)
├── SpriteRenderer
│   └── Sprite: Emily sprite
└── Active: ✗ (disabled initially) ⭐

Passage (GameObject)
├── SpriteRenderer
│   └── Sprite: Passage sprite
├── Room08_Interactable
│   └── My Type: Passage
└── Active: ✗ (disabled initially) ⭐

Bandages (GameObject) ❌ REMOVE THIS
```

---

## TESTING CHECKLIST

### QTE Testing:
- [ ] QTE shows "0/50" at start
- [ ] Timer shows "25.0s" at start
- [ ] Can tap anywhere on screen
- [ ] Each tap increments counter
- [ ] Timer counts down
- [ ] Timer turns yellow at 15s
- [ ] Timer turns red at 10s
- [ ] Mirror cracks progressively
- [ ] At 50 taps, mirror shatters
- [ ] If time runs out, game over

### Evidence Testing:
- [ ] Can find torn clothes (bathtub)
- [ ] Can find apology note
- [ ] Can find hammer (medicine cabinet)
- [ ] NO bandages in scene
- [ ] Emily appears after all 3 items collected

### Mirror Testing:
- [ ] Mirror starts with normal sprite
- [ ] Emily NOT visible initially
- [ ] Emily appears after items collected
- [ ] After QTE, mirror changes to broken sprite
- [ ] After QTE, Emily disappears
- [ ] After QTE, passage becomes visible
- [ ] Can interact with passage to climb through

---

## MIGRATION GUIDE

### For Existing Scenes:

1. **Remove Bandages**:
   ```
   - Find "Bandages" GameObject
   - Delete it
   ```

2. **Update QTE Panel**:
   ```
   - Remove tap target prefab references
   - Add FullScreenTapArea Image (full screen)
   - Update Room08_MirrorQTE component settings
   ```

3. **Add Mirror Sprites**:
   ```
   - Create mirrorNormalSprite
   - Create mirrorBrokenSprite
   - Assign to Room08_FlowController
   ```

4. **Setup Passage**:
   ```
   - Create Passage GameObject
   - Add SpriteRenderer with passage sprite
   - Add Room08_Interactable (type: Passage)
   - Set Active: false (disabled initially)
   - Assign to Room08_FlowController.passageObject
   ```

5. **Update Emily**:
   ```
   - Find EmilyInMirror GameObject
   - Set Active: false (disabled initially)
   ```

---

## SUMMARY

### What Changed:
- ✅ QTE: 50 taps, 25 seconds, full screen
- ✅ Removed bandages evidence
- ✅ Emily appears after items collected
- ✅ Mirror sprite changes after breaking
- ✅ Passage becomes visible after breaking

### What Stayed Same:
- ✅ Torn clothes from bathtub
- ✅ Apology note evidence
- ✅ Hammer from medicine cabinet
- ✅ Emily humming outside
- ✅ Door locked
- ✅ Dialogue sequences

### Files Modified:
1. ✅ `Room08_MirrorQTE.cs`
2. ✅ `Room08_FlowController.cs`
3. ✅ `Room08_Interactable.cs`

---

**Status**: ✅ CODE UPDATED
**Next**: Update Unity scene with new setup
**Date**: May 4, 2026
