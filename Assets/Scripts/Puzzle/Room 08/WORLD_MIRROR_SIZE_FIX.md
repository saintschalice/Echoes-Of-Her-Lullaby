# Room 08 - World Mirror Size Fix

## 🐛 PROBLEMA

Ang **world mirror object** (yung mirror sa scene, hindi yung sa panel) ay lumalaki pagkatapos mag-transition sa broken sprite.

---

## ✅ SOLUTION

### Check Mirror GameObject Settings

Ang mirror GameObject sa scene ay dapat may **consistent size** before and after sprite change.

---

## 🔧 UNITY FIX

### Option 1: Lock Transform Scale

1. **Select Mirror GameObject** (sa scene)

2. **Check Transform**:
   - Position: (x, y, z)
   - Rotation: (0, 0, 0)
   - **Scale: (1, 1, 1)** ← IMPORTANT!

3. **Check SpriteRenderer**:
   - Sprite: Normal mirror sprite
   - Draw Mode: **Simple** (NOT Sliced or Tiled)
   - Size: Should be grayed out (not editable)

4. **Test**:
   - Play game
   - Break mirror
   - Check if size stays the same

---

### Option 2: Match Sprite Sizes

Ang issue ay baka different sizes ang normal sprite at broken sprite.

1. **Check Normal Mirror Sprite**:
   - Select sprite in Project
   - Check Pixels Per Unit (e.g., 100)
   - Check size (e.g., 512x768)

2. **Check Broken Mirror Sprite**:
   - Select sprite in Project
   - **Pixels Per Unit should be SAME** (e.g., 100)
   - **Size should be SAME** (e.g., 512x768)

3. **If different**:
   - Adjust Pixels Per Unit to match
   - Or resize sprites to same dimensions

---

### Option 3: Use Draw Mode Simple

1. **Select Mirror GameObject**

2. **SpriteRenderer settings**:
   - Draw Mode: **Simple**
   - NOT Sliced
   - NOT Tiled

3. **Transform Scale**:
   - Scale: **(1, 1, 1)**
   - Do NOT use scale to resize

4. **Resize sprite in image editor** if needed

---

## 🎨 RECOMMENDED SETTINGS

### Mirror GameObject (in scene):

```
Mirror (GameObject):
├─ Transform:
│   ├─ Position: (x, y, 0)
│   ├─ Rotation: (0, 0, 0)
│   └─ Scale: (1, 1, 1) ← MUST BE 1,1,1
│
├─ SpriteRenderer:
│   ├─ Sprite: [Normal mirror sprite]
│   ├─ Draw Mode: Simple
│   ├─ Color: White (255, 255, 255, 255)
│   └─ Sorting Layer: Default
│
└─ BoxCollider2D:
    └─ Is Trigger: ☑
```

### Both Sprites (Normal + Broken):

```
Normal Mirror Sprite:
├─ Texture Type: Sprite (2D and UI)
├─ Pixels Per Unit: 100 (or your value)
├─ Sprite Mode: Single
└─ Size: 512x768 (example)

Broken Mirror Sprite:
├─ Texture Type: Sprite (2D and UI)
├─ Pixels Per Unit: 100 (SAME as normal!)
├─ Sprite Mode: Single
└─ Size: 512x768 (SAME as normal!)
```

---

## 🔍 COMMON ISSUES

### Issue: "Mirror gets bigger after breaking"

**Cause**: Broken sprite has different Pixels Per Unit

**Solution**:
1. Select both sprites in Project
2. Check Pixels Per Unit
3. Make them the same value
4. Click Apply

---

### Issue: "Mirror stretches or distorts"

**Cause**: Draw Mode is Sliced or Tiled

**Solution**:
1. Select Mirror GameObject
2. SpriteRenderer → Draw Mode: **Simple**

---

### Issue: "Mirror size changes randomly"

**Cause**: Transform Scale is not (1, 1, 1)

**Solution**:
1. Select Mirror GameObject
2. Transform → Scale: **(1, 1, 1)**
3. Never use scale to resize sprites

---

## 💡 BEST PRACTICES

### For Consistent Sprite Sizes:

1. **Same Dimensions**:
   - Normal: 512x768
   - Broken: 512x768
   - Both should be exact same size

2. **Same Pixels Per Unit**:
   - Normal: 100 PPU
   - Broken: 100 PPU
   - This ensures same world size

3. **Transform Scale (1, 1, 1)**:
   - Never scale sprites in Unity
   - Resize in image editor instead

4. **Draw Mode Simple**:
   - Always use Simple for regular sprites
   - Sliced/Tiled only for UI

---

## 📋 STEP-BY-STEP FIX

### Step 1: Check Sprites

1. **Select Normal Mirror Sprite** (in Project)
2. **Note Pixels Per Unit** (e.g., 100)
3. **Note Size** (e.g., 512x768)

4. **Select Broken Mirror Sprite** (in Project)
5. **Check Pixels Per Unit** - Should match normal
6. **Check Size** - Should match normal

7. **If different**:
   - Change Pixels Per Unit to match
   - Click Apply

### Step 2: Check GameObject

1. **Select Mirror GameObject** (in scene)
2. **Transform → Scale**: Set to **(1, 1, 1)**
3. **SpriteRenderer → Draw Mode**: Set to **Simple**

### Step 3: Test

1. **Play game**
2. **Break mirror**
3. **Check size** - Should stay the same

---

## 🎯 QUICK FIX CHECKLIST

- [ ] Normal sprite Pixels Per Unit: ___
- [ ] Broken sprite Pixels Per Unit: ___ (same as normal)
- [ ] Mirror GameObject Scale: (1, 1, 1)
- [ ] SpriteRenderer Draw Mode: Simple
- [ ] Test in Play Mode

---

## 🛠️ IF STILL NOT WORKING

### Add Debug to FlowController:

```csharp
// In OnMirrorBroken() method, add:
if (mirrorSpriteRenderer != null && mirrorBrokenSprite != null)
{
    Debug.Log($"[Mirror] Changing sprite from {mirrorSpriteRenderer.sprite.name} to {mirrorBrokenSprite.name}");
    Debug.Log($"[Mirror] Transform scale: {mirrorSpriteRenderer.transform.localScale}");
    
    mirrorSpriteRenderer.sprite = mirrorBrokenSprite;
    
    Debug.Log($"[Mirror] New sprite: {mirrorSpriteRenderer.sprite.name}");
}
```

This will show you what's happening in Console.

---

**Fixed! Mirror should maintain same size now!** 🪞✨

