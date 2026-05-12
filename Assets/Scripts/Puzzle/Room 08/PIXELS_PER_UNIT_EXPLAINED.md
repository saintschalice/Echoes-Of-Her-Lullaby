# Pixels Per Unit - Explained Simply

## 🎯 WHAT IS PIXELS PER UNIT?

**Pixels Per Unit (PPU)** tells Unity how many pixels in your sprite = 1 Unity unit in the world.

---

## 📐 HOW IT WORKS

### Example 1: PPU = 100

```
Sprite: 512 pixels wide
PPU: 100
World Size: 512 ÷ 100 = 5.12 units wide
```

### Example 2: PPU = 50

```
Sprite: 512 pixels wide
PPU: 50
World Size: 512 ÷ 50 = 10.24 units wide ← BIGGER!
```

**Same sprite, different PPU = different size in world!**

---

## 🐛 THE PROBLEM

### Your Issue:

```
Normal Mirror Sprite:
├─ Size: 512x768 pixels
├─ PPU: 100
└─ World Size: 5.12 x 7.68 units

Broken Mirror Sprite:
├─ Size: 512x768 pixels
├─ PPU: 50 ← DIFFERENT!
└─ World Size: 10.24 x 15.36 units ← LUMALAKI!
```

**Result**: Broken mirror appears TWICE as big! 😱

---

## ✅ THE SOLUTION

### Make PPU the Same:

```
Normal Mirror Sprite:
├─ Size: 512x768 pixels
├─ PPU: 100
└─ World Size: 5.12 x 7.68 units

Broken Mirror Sprite:
├─ Size: 512x768 pixels
├─ PPU: 100 ← SAME!
└─ World Size: 5.12 x 7.68 units ← SAME SIZE!
```

**Result**: Both sprites same size in world! ✅

---

## 🔧 HOW TO FIX

### Step 1: Find Normal Sprite PPU

1. Select **normal mirror sprite** in Project
2. Look at Inspector
3. Find **Pixels Per Unit**: (e.g., 100)
4. **Write it down!**

### Step 2: Match Broken Sprite PPU

1. Select **broken mirror sprite** in Project
2. Look at Inspector
3. Change **Pixels Per Unit** to SAME value (e.g., 100)
4. **Click Apply**

### Step 3: Test

1. Play game
2. Break mirror
3. Check if same size ✅

---

## 📊 COMMON PPU VALUES

### Standard Sprites:
- **PPU = 100**: Most common, good for most games
- **PPU = 32**: Pixel art games
- **PPU = 200**: High resolution sprites

### Your Game:
- Check what PPU your other sprites use
- Use the SAME PPU for consistency
- All sprites in same scene should use same PPU

---

## 🎨 VISUAL COMPARISON

### PPU = 100 (Normal):
```
┌─────────┐
│         │
│ MIRROR  │  ← 5.12 units wide
│         │
└─────────┘
```

### PPU = 50 (Wrong):
```
┌───────────────────┐
│                   │
│                   │
│      MIRROR       │  ← 10.24 units wide (DOUBLE!)
│                   │
│                   │
└───────────────────┘
```

### PPU = 100 (Fixed):
```
┌─────────┐
│         │
│ MIRROR  │  ← 5.12 units wide (SAME!)
│         │
└─────────┘
```

---

## 💡 KEY FORMULA

```
World Size = Sprite Pixel Size ÷ Pixels Per Unit

Example:
512 pixels ÷ 100 PPU = 5.12 units
512 pixels ÷ 50 PPU = 10.24 units
512 pixels ÷ 200 PPU = 2.56 units
```

**Lower PPU = Bigger in world**
**Higher PPU = Smaller in world**

---

## 🔍 QUICK CHECK

### To verify your sprites:

1. **Normal Sprite**:
   - Pixel Size: _____ x _____
   - PPU: _____
   - World Size: _____ x _____

2. **Broken Sprite**:
   - Pixel Size: _____ x _____ (should match!)
   - PPU: _____ (should match!)
   - World Size: _____ x _____ (should match!)

---

## 🐛 TROUBLESHOOTING

### "I changed PPU but still wrong size"

**Check**:
1. Did you click Apply?
2. Is Transform Scale (1, 1, 1)?
3. Is Draw Mode set to Simple?
4. Did you save the scene?

### "I don't see Pixels Per Unit"

**Check**:
1. Is Texture Type set to "Sprite (2D and UI)"?
2. If not, change it and click Apply
3. PPU should appear now

### "Both sprites have same PPU but different size"

**Check**:
1. Are pixel dimensions the same? (e.g., both 512x768)
2. Is one sprite using Sliced or Tiled draw mode?
3. Is Transform Scale different?

---

## ✅ FINAL CHECKLIST

- [ ] Found normal sprite PPU value: _____
- [ ] Set broken sprite PPU to same value: _____
- [ ] Clicked Apply
- [ ] Both sprites same pixel dimensions
- [ ] Transform Scale is (1, 1, 1)
- [ ] Draw Mode is Simple
- [ ] Tested in Play Mode
- [ ] Sprites are same size ✅

---

## 📝 REMEMBER

**The Golden Rule**:
> If two sprites should be the same size in the world,
> they MUST have the same Pixels Per Unit value!

**Simple as that!** 🎯

---

**Now you understand PPU! Go fix those sprites!** 🪞✨
