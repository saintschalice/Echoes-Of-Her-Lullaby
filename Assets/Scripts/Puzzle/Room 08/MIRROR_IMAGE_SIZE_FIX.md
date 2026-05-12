# Room 08 - Mirror Image Size Fix

## 🐛 PROBLEMA

Ang mirror image sa panel ay lumalaki o nag-s-stretch - hindi siya same size ng mirror sprite na ginamit mo.

---

## ✅ SOLUTION

### Option 1: Preserve Aspect (Recommended)

1. **Select MirrorImage** (yung Image component sa panel)

2. **Set Image Type**:
   - Image Type: **Simple** (NOT Filled, NOT Sliced)

3. **Enable Preserve Aspect**:
   - ☑ **Preserve Aspect** - CHECK THIS!

4. **Set Size**:
   - Width: 400 (or your preferred size)
   - Height: 600 (or your preferred size)
   - Adjust based on your mirror sprite's aspect ratio

**Result**: Mirror will maintain its original aspect ratio and won't stretch!

---

### Option 2: Set Native Size

1. **Select MirrorImage**

2. **Click "Set Native Size" button** (sa Inspector, sa Image component)

3. **Adjust RectTransform**:
   - Scale down if too big
   - Or manually set Width/Height

**Result**: Mirror will use the exact pixel size of the sprite.

---

### Option 3: Manual Size Control

1. **Select MirrorImage**

2. **Set RectTransform**:
   - Anchor: **Center** (not stretch)
   - Pivot: (0.5, 0.5)
   - Width: 400 (your desired width)
   - Height: 600 (your desired height)

3. **Set Image Type**:
   - Image Type: **Simple**
   - ☑ **Preserve Aspect**

**Result**: Full control over mirror size.

---

## 🎨 RECOMMENDED SETTINGS

### For MirrorImage (in panel):

```
MirrorImage (Image Component):
├─ Image Type: Simple
├─ Preserve Aspect: ☑ CHECKED
├─ RectTransform:
│   ├─ Anchor: Center
│   ├─ Pivot: (0.5, 0.5)
│   ├─ Width: 400 (adjust to your sprite)
│   ├─ Height: 600 (adjust to your sprite)
│   └─ Scale: (1, 1, 1)
└─ Sprite: [Your mirror sprite]
```

---

## 🔍 COMMON ISSUES

### Issue: "Mirror is stretched horizontally/vertically"

**Cause**: Preserve Aspect is not checked

**Solution**:
1. Select MirrorImage
2. ☑ Check "Preserve Aspect"

---

### Issue: "Mirror is too big/small"

**Cause**: Native size is different from desired size

**Solution**:
1. Enable Preserve Aspect
2. Manually adjust Width or Height
3. The other dimension will auto-adjust to maintain aspect ratio

---

### Issue: "Mirror changes size during gameplay"

**Cause**: Anchor is set to Stretch

**Solution**:
1. Set Anchor to **Center** (not Stretch)
2. Set Pivot to (0.5, 0.5)
3. Set specific Width/Height values

---

## 💡 TIPS

### For Best Results:

1. **Use Preserve Aspect** - Always check this for sprites
2. **Center Anchor** - Prevents stretching with screen size
3. **Consistent Size** - Use same size for all mirror phases
4. **Test on Different Resolutions** - Make sure it looks good on all screens

### Aspect Ratio Examples:

**Portrait Mirror** (taller than wide):
- Width: 400
- Height: 600
- Aspect: 2:3

**Square Mirror**:
- Width: 500
- Height: 500
- Aspect: 1:1

**Landscape Mirror** (wider than tall):
- Width: 600
- Height: 400
- Aspect: 3:2

---

## 📋 STEP-BY-STEP FIX

1. **Select MirrorImage** in Hierarchy
   - Canvas → MirrorPanel → MirrorImage

2. **In Inspector, find Image component**

3. **Change Image Type**:
   - Click dropdown next to "Image Type"
   - Select **Simple**

4. **Enable Preserve Aspect**:
   - Find checkbox "Preserve Aspect"
   - ☑ **CHECK IT**

5. **Adjust Size** (if needed):
   - In RectTransform section
   - Set Width: 400 (or your size)
   - Height will auto-adjust

6. **Test in Play Mode**:
   - Mirror should maintain aspect ratio
   - Should not stretch or distort

---

## 🎯 QUICK FIX CHECKLIST

- [ ] MirrorImage selected
- [ ] Image Type: Simple
- [ ] Preserve Aspect: ☑ CHECKED
- [ ] Anchor: Center (not Stretch)
- [ ] Width/Height set to desired size
- [ ] Test in Play Mode

---

**Fixed! Mirror should maintain its original aspect ratio now!** 🪞✨

