# Room 08 - Sprite Size Fix (Tagalog)

## 🐛 PROBLEMA

Ang broken mirror sprite ay lumalaki pagkatapos ng puzzle. Hindi siya same size ng normal mirror sprite.

---

## ✅ SOLUTION

Kailangan **pareho ang Pixels Per Unit** ng both sprites (normal at broken).

---

## 🔧 STEP-BY-STEP FIX

### Step 1: Check Normal Mirror Sprite

1. **Sa Project window**, hanapin ang **normal mirror sprite**
2. **Click** sa sprite
3. **Sa Inspector**, tingnan ang:
   - **Pixels Per Unit**: (halimbawa: 100)
   - **Sprite Mode**: Single
   - **Texture Type**: Sprite (2D and UI)

4. **I-note ang Pixels Per Unit value** (e.g., 100)

---

### Step 2: Check Broken Mirror Sprite

1. **Sa Project window**, hanapin ang **broken mirror sprite**
2. **Click** sa sprite
3. **Sa Inspector**, tingnan ang:
   - **Pixels Per Unit**: (dapat SAME sa normal sprite!)
   - **Sprite Mode**: Single
   - **Texture Type**: Sprite (2D and UI)

---

### Step 3: Match Pixels Per Unit

Kung **different** ang Pixels Per Unit:

1. **Select broken mirror sprite** (sa Project)
2. **Sa Inspector**:
   - **Pixels Per Unit**: I-change to SAME value as normal sprite
   - Example: Kung normal sprite ay 100, broken sprite ay dapat 100 din
3. **Click Apply** (sa bottom ng Inspector)

---

### Step 4: Check Mirror GameObject

1. **Sa Hierarchy**, select **Mirror GameObject**
2. **Sa Inspector**, check:
   - **Transform → Scale**: Dapat **(1, 1, 1)**
   - **SpriteRenderer → Draw Mode**: Dapat **Simple**

Kung hindi (1, 1, 1):
- I-reset to (1, 1, 1)

---

### Step 5: Test

1. **Play** ang game
2. **Complete** ang mirror puzzle
3. **Check** kung same size pa rin ang mirror

---

## 📊 EXAMPLE VALUES

### Kung Normal Mirror Sprite ay:
```
Pixels Per Unit: 100
Size: 512x768 pixels
```

### Broken Mirror Sprite dapat:
```
Pixels Per Unit: 100 (SAME!)
Size: 512x768 pixels (SAME!)
```

---

## 🎯 COMMON VALUES

### Option 1: Standard Size
- **Pixels Per Unit**: 100
- **Sprite Size**: 512x768 or 1024x1536

### Option 2: High Resolution
- **Pixels Per Unit**: 200
- **Sprite Size**: 1024x1536 or 2048x3072

**Important**: Both sprites dapat same values!

---

## 🔍 QUICK CHECK

Gawin ito para ma-verify:

1. **Select normal mirror sprite** → Note Pixels Per Unit
2. **Select broken mirror sprite** → Check if same
3. **If different** → Change broken sprite to match
4. **Click Apply**
5. **Test in Play Mode**

---

## 🐛 TROUBLESHOOTING

### Issue: "Hindi ko makita ang Pixels Per Unit"

**Solution**:
1. Select sprite sa Project window
2. Sa Inspector, check kung:
   - Texture Type: **Sprite (2D and UI)**
3. Kung hindi, i-change to Sprite (2D and UI)
4. Click Apply
5. Pixels Per Unit dapat lumabas na

---

### Issue: "Nag-apply na ako pero lumalaki pa rin"

**Solution**:
1. Check Mirror GameObject Transform Scale
2. Dapat (1, 1, 1)
3. Check SpriteRenderer Draw Mode
4. Dapat Simple (NOT Sliced or Tiled)

---

### Issue: "Paano ko malalaman kung tama na?"

**Solution**:
1. Play game
2. Tingnan ang mirror BEFORE puzzle
3. Complete puzzle
4. Tingnan ang mirror AFTER puzzle
5. Dapat **SAME SIZE** lang

---

## 💡 PRO TIP

### Para sigurado na same size:

1. **Export both sprites** from same source file
2. **Same dimensions** (e.g., both 512x768)
3. **Import to Unity**
4. **Set same Pixels Per Unit** (e.g., both 100)
5. **Never scale** using Transform

---

## 📋 FINAL CHECKLIST

- [ ] Normal sprite Pixels Per Unit: _____ (note the value)
- [ ] Broken sprite Pixels Per Unit: _____ (should match!)
- [ ] Both sprites same dimensions (e.g., 512x768)
- [ ] Mirror GameObject Scale: (1, 1, 1)
- [ ] SpriteRenderer Draw Mode: Simple
- [ ] Tested in Play Mode
- [ ] Size stays the same after puzzle

---

## 🎨 VISUAL GUIDE

### Before Fix:
```
Normal Mirror: 512x768, PPU=100 → Renders at 5.12 x 7.68 units
Broken Mirror: 512x768, PPU=50  → Renders at 10.24 x 15.36 units ❌ WRONG!
```

### After Fix:
```
Normal Mirror: 512x768, PPU=100 → Renders at 5.12 x 7.68 units
Broken Mirror: 512x768, PPU=100 → Renders at 5.12 x 7.68 units ✅ CORRECT!
```

---

## 🛠️ IF STILL NOT WORKING

Kung after ng lahat ng steps, lumalaki pa rin:

1. **Take screenshot** ng:
   - Normal sprite Inspector
   - Broken sprite Inspector
   - Mirror GameObject Inspector

2. **Check Console** for errors

3. **Verify** na tama ang sprite assignments sa FlowController:
   - Mirror GameObject: [assigned]
   - Mirror Normal Sprite: [assigned]
   - Mirror Broken Sprite: [assigned]

---

**Yan na! Dapat same size na ang mirror before and after puzzle!** 🪞✨
