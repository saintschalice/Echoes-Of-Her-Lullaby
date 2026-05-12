# Jumpscare System - Troubleshooting Guide

## 🐛 ISSUE: "Hindi gumagana ang jumpscare"

### Common Causes:

1. ❌ JumpscareManager not properly set up
2. ❌ UI references not assigned
3. ❌ Canvas sort order too low
4. ❌ Sprites or audio not assigned
5. ❌ Panel hierarchy incorrect

---

## ✅ STEP-BY-STEP FIX

### Step 1: Check Console for Errors

**What to look for**:
```
[Jumpscare] Initialized
[Jumpscare] Starting jumpscare sequence
```

**If you see**:
```
JumpscareManager.Instance is null
```
→ JumpscareManager not in scene or Awake() not called

---

### Step 2: Verify JumpscareManager GameObject

**In PersistentScene**:

1. Find `JumpscareManager` GameObject
2. Check if it has `JumpscareManager` script attached
3. Check if script is **enabled** (checkbox checked)

**Inspector should show**:
```
JumpscareManager (Script)
├─ Jumpscare Panel: [assigned]
├─ Jumpscare Image: [assigned]
├─ Tilt Left Sprite: [assigned]
├─ Tilt Right Sprite: [assigned]
├─ Center Sprite: [assigned]
├─ Jumpscare Sound: [assigned]
└─ Flash Image: [assigned or empty]
```

**If any field is "None"**:
→ Assign the missing reference!

---

### Step 3: Check Canvas Hierarchy

**CRITICAL**: Canvas must be in **PersistentScene** or **DontDestroyOnLoad**!

**Correct Hierarchy**:
```
PersistentScene:
├─ GameOverManager (with DontDestroyOnLoad)
├─ JumpscareManager (with DontDestroyOnLoad)
└─ JumpscareCanvas (or add to existing Canvas)
    └─ JumpscarePanel
        ├─ JumpscareImage
        └─ FlashImage (optional)
```

**Canvas Settings**:
- Render Mode: **Screen Space - Overlay**
- Sort Order: **1000** or higher (above everything!)

---

### Step 4: Check Panel Setup

**JumpscarePanel**:
- Active: ☐ (UNCHECKED at start)
- Anchor: Stretch (full screen)
- Image Color: Black (0, 0, 0, 255)

**JumpscareImage**:
- Active: ✓ (CHECKED)
- Anchor: Center
- Preserve Aspect: ✓ (CHECKED)

**FlashImage** (optional):
- Active: ☐ (UNCHECKED at start)
- Anchor: Stretch (full screen)
- Color: White (255, 255, 255, 0) ← Alpha 0!

---

### Step 5: Test with Debug

Add this to test if JumpscareManager exists:

**In any script**:
```csharp
void Start()
{
    if (JumpscareManager.Instance != null)
    {
        Debug.Log("✅ JumpscareManager found!");
    }
    else
    {
        Debug.LogError("❌ JumpscareManager is NULL!");
    }
}
```

**If NULL**:
→ JumpscareManager not in scene or destroyed

---

## 🔧 COMMON ISSUES & FIXES

### Issue 1: "JumpscareManager.Instance is null"

**Cause**: JumpscareManager not in scene or Awake() not called

**Fix**:
1. Check if JumpscareManager GameObject exists in PersistentScene
2. Check if JumpscareManager script is attached
3. Check if script is enabled (checkbox)
4. Make sure PersistentScene is loaded

---

### Issue 2: "Jumpscare panel doesn't show"

**Cause**: Canvas sort order too low or panel not assigned

**Fix**:
1. Check Canvas Sort Order = 1000 or higher
2. Check JumpscarePanel is assigned in JumpscareManager
3. Check JumpscarePanel starts inactive (unchecked)
4. Check Canvas is in PersistentScene

---

### Issue 3: "Sprites don't change"

**Cause**: Sprites not assigned or JumpscareImage not assigned

**Fix**:
1. Check all 3 sprites are assigned in JumpscareManager
2. Check JumpscareImage is assigned
3. Check sprites are imported as Sprite (2D and UI)

---

### Issue 4: "Audio doesn't play"

**Cause**: Audio clip not assigned or AudioManager missing

**Fix**:
1. Check Jumpscare Sound is assigned
2. Check AudioManager exists in scene
3. Check audio clip is imported correctly

---

### Issue 5: "Game over shows immediately, no jumpscare"

**Cause**: Code calling GameOverManager directly instead of JumpscareManager

**Fix**:
1. Check if code uses `JumpscareManager.TriggerJumpscare()`
2. NOT `GameOverManager.TriggerGameOver()`
3. Review updated scripts in `UPDATE_SCRIPTS_FOR_JUMPSCARE.md`

---

## 🎯 PERSISTENT SCENE SETUP

### Correct Setup:

**PersistentScene Hierarchy**:
```
PersistentScene:
├─ GameOverManager
│   ├─ Script: GameOverManager
│   └─ DontDestroyOnLoad: ✓
│
├─ JumpscareManager
│   ├─ Script: JumpscareManager
│   └─ DontDestroyOnLoad: ✓ (handled by script)
│
└─ Canvas (or PersistentUI)
    ├─ Render Mode: Screen Space - Overlay
    ├─ Sort Order: 1000
    │
    ├─ GameOverUI (existing)
    │   ├─ GameOverMessagePanel
    │   └─ GameOverOptionsPanel
    │
    └─ JumpscarePanel (NEW!)
        ├─ JumpscareImage
        └─ FlashImage
```

---

## 🔍 DEBUG CHECKLIST

Run through this checklist:

### JumpscareManager:
- [ ] GameObject exists in PersistentScene
- [ ] Script attached and enabled
- [ ] All UI references assigned
- [ ] All sprites assigned (3 total)
- [ ] Audio assigned
- [ ] Console shows "[Jumpscare] Initialized"

### Canvas:
- [ ] In PersistentScene or DontDestroyOnLoad
- [ ] Render Mode: Screen Space - Overlay
- [ ] Sort Order: 1000+
- [ ] JumpscarePanel is child of Canvas

### JumpscarePanel:
- [ ] Starts inactive (unchecked)
- [ ] Anchor: Stretch
- [ ] Image: Black color
- [ ] Assigned in JumpscareManager

### JumpscareImage:
- [ ] Starts active (checked)
- [ ] Anchor: Center
- [ ] Preserve Aspect: checked
- [ ] Assigned in JumpscareManager

### Code:
- [ ] Scripts use `JumpscareManager.TriggerJumpscare()`
- [ ] NOT using `GameOverManager.TriggerGameOver()` directly
- [ ] Fallback code exists (if JumpscareManager null)

---

## 🧪 MANUAL TEST

### Test 1: Check Instance

**Add to any script**:
```csharp
void Update()
{
    if (Input.GetKeyDown(KeyCode.J)) // Press J to test
    {
        if (JumpscareManager.Instance != null)
        {
            Debug.Log("✅ Triggering test jumpscare!");
            JumpscareManager.Instance.TriggerJumpscare("TEST");
        }
        else
        {
            Debug.LogError("❌ JumpscareManager is NULL!");
        }
    }
}
```

**Press J in Play Mode**:
- ✅ Should show jumpscare
- ❌ If nothing happens, check console

---

### Test 2: Check References

**Add to JumpscareManager.cs Start()**:
```csharp
void Start()
{
    Debug.Log("=== JUMPSCARE MANAGER DEBUG ===");
    Debug.Log($"Jumpscare Panel: {(jumpscarePanel != null ? "✅" : "❌ NULL")}");
    Debug.Log($"Jumpscare Image: {(jumpscareImage != null ? "✅" : "❌ NULL")}");
    Debug.Log($"Tilt Left Sprite: {(tiltLeftSprite != null ? "✅" : "❌ NULL")}");
    Debug.Log($"Tilt Right Sprite: {(tiltRightSprite != null ? "✅" : "❌ NULL")}");
    Debug.Log($"Center Sprite: {(centerSprite != null ? "✅" : "❌ NULL")}");
    Debug.Log($"Jumpscare Sound: {(jumpscareSound != null ? "✅" : "❌ NULL")}");
    Debug.Log("================================");
}
```

**Check Console**:
- All should show ✅
- If any shows ❌, assign that reference!

---

## 📋 QUICK FIX STEPS

### If jumpscare not working:

1. **Check Console** for errors
2. **Verify JumpscareManager exists** in PersistentScene
3. **Check all references assigned** in Inspector
4. **Verify Canvas sort order** = 1000+
5. **Test with J key** (manual test above)
6. **Check code** uses `JumpscareManager.TriggerJumpscare()`

---

## 🆘 STILL NOT WORKING?

### Checklist:

1. **Is PersistentScene loaded?**
   - Check if PersistentScene is in Build Settings
   - Check if it's loaded at game start

2. **Is JumpscareManager in correct scene?**
   - Should be in PersistentScene
   - NOT in individual room scenes

3. **Are references assigned?**
   - Check Inspector for "None" fields
   - Drag and drop missing references

4. **Is Canvas visible?**
   - Check Canvas is active
   - Check Sort Order is high enough

5. **Is code updated?**
   - Check scripts use JumpscareManager
   - NOT GameOverManager directly

---

## 💡 PRO TIPS

### Tip 1: Use Debug Logs
Add debug logs to see what's happening:
```csharp
Debug.Log("[Jumpscare] Triggering jumpscare...");
```

### Tip 2: Test in Isolation
Create a test scene with just:
- Canvas
- JumpscarePanel
- JumpscareManager
- Test button

### Tip 3: Check Execution Order
Make sure JumpscareManager Awake() runs before any game over triggers.

### Tip 4: Verify DontDestroyOnLoad
JumpscareManager should persist across scenes:
```csharp
DontDestroyOnLoad(gameObject);
```

---

## 📝 EXAMPLE WORKING SETUP

### Minimal Working Example:

**PersistentScene**:
```
JumpscareManager (GameObject)
└─ JumpscareManager (Script)
    ├─ Jumpscare Panel: JumpscarePanel
    ├─ Jumpscare Image: JumpscareImage
    ├─ Tilt Left Sprite: emily_tilt_left
    ├─ Tilt Right Sprite: emily_tilt_right
    ├─ Center Sprite: emily_center
    └─ Jumpscare Sound: jumpscare_audio

Canvas (Sort Order: 1000)
└─ JumpscarePanel (Active: ☐)
    └─ JumpscareImage (Active: ✓)
```

**In any script**:
```csharp
// Trigger jumpscare
JumpscareManager.Instance?.TriggerJumpscare("Emily caught you...");
```

**Result**: Jumpscare plays, then game over!

---

## ✅ VERIFICATION

### After fixing, verify:

1. **Console shows**:
   ```
   [Jumpscare] Initialized
   [Jumpscare] Starting jumpscare sequence
   ```

2. **Jumpscare plays**:
   - Panel appears
   - Sprites change
   - Audio plays
   - Screen shakes

3. **Game over shows after**:
   - After 11 seconds
   - Game over screen appears
   - Can retry

---

**Follow these steps and jumpscare should work!** 👻✨
