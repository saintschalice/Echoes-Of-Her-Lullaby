# Jumpscare Ayaw Gumana - Troubleshooting Guide (Tagalog)

## 🚨 PROBLEMA: "Deretso na sa game over, walang jumpscare"

Kung nangyayari ito, ibig sabihin **hindi nag-trigger ang jumpscare** o **may missing references**.

---

## 🔍 STEP 1: I-CHECK ANG CONSOLE

**PINAKAIMPORTANTE**: Tignan ang Unity Console!

### Ano ang dapat makita:

Kapag nag-play ka ng game, dapat may ganito sa Console:
```
=== JUMPSCARE MANAGER AWAKE ===
[Jumpscare] ✅ Instance created and set to DontDestroyOnLoad
=== JUMPSCARE INITIALIZATION ===
[Jumpscare] ✅ Panel hidden at start
=== REFERENCE CHECK ===
Jumpscare Panel: ✅ Assigned
Jumpscare Image: ✅ Assigned
Tilt Left Sprite: ✅ Assigned
Tilt Right Sprite: ✅ Assigned
Center Sprite: ✅ Assigned
Jumpscare Sound: ✅ Assigned
[Jumpscare] ✅ All critical references assigned - Ready to use!
=== INITIALIZATION COMPLETE ===
```

### Kung may nakita kang ganito:

#### ❌ Walang "JUMPSCARE MANAGER AWAKE" message
**Problema**: JumpscareManager hindi nag-load o wala sa scene

**Solusyon**:
1. Check kung may JumpscareManager GameObject sa PersistentScene
2. Check kung naka-attach ang JumpscareManager script
3. Check kung enabled ang script (may checkmark)

---

#### ❌ May "NULL" sa Reference Check
**Problema**: May kulang na references sa Inspector

**Halimbawa**:
```
Jumpscare Panel: ❌ NULL
```

**Solusyon**:
1. Select JumpscareManager GameObject
2. Tignan ang Inspector
3. I-drag ang missing reference (yung may "None")

---

#### ❌ "jumpscarePanel is NULL! Assign it in Inspector!"
**Problema**: Hindi naka-assign ang JumpscarePanel

**Solusyon**:
1. Select JumpscareManager GameObject
2. Sa Inspector, hanapin ang "Jumpscare Panel" field
3. I-drag ang JumpscarePanel GameObject doon

---

## 🔧 STEP 2: GAMITIN ANG DEBUG HELPER

May bagong script ako para sa'yo na mas madaling mag-debug!

### Paano gamitin:

1. **I-add ang JumpscareDebugHelper script**:
   - Create empty GameObject: `DebugHelper`
   - Add Component → `JumpscareDebugHelper`

2. **Play ang game**

3. **Press D** (Diagnostic)
   - Makikita mo sa Console kung ano ang problema
   - Lahat ng missing references lalabas

4. **Press J** (Test Jumpscare)
   - Mag-trigger ng test jumpscare
   - Kung gumana, ibig sabihin OK ang setup

5. **Press I** (Instance Info)
   - Check kung nag-exist ang JumpscareManager.Instance

---

## 🎯 STEP 3: COMMON ISSUES AT SOLUTIONS

### Issue #1: "JumpscareManager.Instance is NULL"

**Ibig sabihin**: Walang JumpscareManager sa scene o hindi pa nag-run ang Awake()

**Check mo**:
1. ✅ May JumpscareManager GameObject ba sa PersistentScene?
2. ✅ Naka-attach ba ang JumpscareManager script?
3. ✅ Enabled ba ang script? (may checkmark sa Inspector)
4. ✅ Naka-load ba ang PersistentScene sa game start?

**Solusyon**:
- Kung wala: Create JumpscareManager GameObject sa PersistentScene
- Kung meron pero NULL pa rin: Check kung tama ang scene na naka-load

---

### Issue #2: "jumpscarePanel is NULL"

**Ibig sabihin**: Hindi naka-assign ang JumpscarePanel sa Inspector

**Solusyon**:
1. Select JumpscareManager GameObject
2. Sa Inspector, hanapin ang "Jumpscare Panel" field
3. I-drag ang JumpscarePanel GameObject (nasa Canvas)
4. Click Apply

**Dapat ganito ang hierarchy**:
```
PersistentScene:
└─ Canvas
    └─ JumpscarePanel ← I-drag ito sa Inspector
        └─ JumpscareImage
```

---

### Issue #3: "jumpscareImage is NULL"

**Ibig sabihin**: Hindi naka-assign ang JumpscareImage sa Inspector

**Solusyon**:
1. Select JumpscareManager GameObject
2. Sa Inspector, hanapin ang "Jumpscare Image" field
3. I-drag ang JumpscareImage GameObject (nasa loob ng JumpscarePanel)
4. Click Apply

---

### Issue #4: "One or more sprites are NULL"

**Ibig sabihin**: May kulang na sprite (tilt left, tilt right, o center)

**Solusyon**:
1. Select JumpscareManager GameObject
2. Sa Inspector, hanapin ang "Jumpscare Sprites" section
3. I-drag ang 3 sprites:
   - **Tilt Left Sprite**: Emily tilted left
   - **Tilt Right Sprite**: Emily tilted right
   - **Center Sprite**: Emily centered (final scare)
4. Click Apply

**IMPORTANTE**: Sprites dapat naka-import as "Sprite (2D and UI)"!

---

### Issue #5: "Canvas sort order is low"

**Ibig sabihin**: Canvas nasa likod ng ibang UI, hindi makikita

**Solusyon**:
1. Select Canvas (yung parent ng JumpscarePanel)
2. Sa Inspector, hanapin ang "Canvas" component
3. I-set ang **Sort Order** to **1000** or higher
4. Click Apply

**Bakit**: Para lumabas ang jumpscare sa ibabaw ng lahat!

---

### Issue #6: "JumpscarePanel is active at start"

**Ibig sabihin**: JumpscarePanel naka-show na agad, dapat hidden

**Solusyon**:
1. Select JumpscarePanel GameObject
2. Sa Inspector, i-uncheck ang checkbox sa taas (Active)
3. Dapat ☐ (walang checkmark)

**Bakit**: Panel dapat hidden at start, tapos i-show lang ng script

---

## 📋 COMPLETE SETUP CHECKLIST

I-check mo lahat ng ito:

### PersistentScene Hierarchy:
- [ ] May JumpscareManager GameObject
- [ ] May Canvas GameObject
- [ ] JumpscarePanel ay child ng Canvas
- [ ] JumpscareImage ay child ng JumpscarePanel

### JumpscareManager GameObject:
- [ ] May JumpscareManager script attached
- [ ] Script is enabled (may checkmark)

### JumpscareManager Inspector:
- [ ] Jumpscare Panel: **Assigned** (hindi "None")
- [ ] Jumpscare Image: **Assigned** (hindi "None")
- [ ] Tilt Left Sprite: **Assigned** (hindi "None")
- [ ] Tilt Right Sprite: **Assigned** (hindi "None")
- [ ] Center Sprite: **Assigned** (hindi "None")
- [ ] Jumpscare Sound: **Assigned** (hindi "None")

### Canvas Settings:
- [ ] Render Mode: **Screen Space - Overlay**
- [ ] Sort Order: **1000** or higher

### JumpscarePanel Settings:
- [ ] Active: **☐ UNCHECKED** (hidden at start)
- [ ] Anchor: **Stretch** (full screen)
- [ ] Image Color: **Black** (0, 0, 0, 255)

### JumpscareImage Settings:
- [ ] Active: **✓ CHECKED**
- [ ] Anchor: **Center**
- [ ] Preserve Aspect: **✓ CHECKED**

---

## 🧪 TESTING STEPS

### Test 1: Check Console Messages

1. Play game
2. Tignan ang Console
3. Dapat may "JUMPSCARE MANAGER AWAKE" message
4. Dapat lahat ng references ay "✅ Assigned"

**Kung may ❌ NULL**: I-assign ang missing reference!

---

### Test 2: Use Debug Helper

1. Add JumpscareDebugHelper script sa scene
2. Play game
3. Press **D** (Diagnostic)
4. Basahin ang Console output
5. I-fix lahat ng ❌ errors

---

### Test 3: Manual Jumpscare Test

1. Play game
2. Press **J** (Test Jumpscare)
3. Dapat mag-play ang jumpscare
4. Kung hindi: Check Console for errors

---

### Test 4: Actual Game Over Test

1. Play game
2. Puntahan si Emily sa Room 03 (Hallway)
3. Hayaan kang mahuli
4. Dapat:
   - ✅ Jumpscare plays (11 seconds)
   - ✅ Game over screen shows after

**Kung deretso game over**: May problema sa setup!

---

## 🔥 QUICK FIX GUIDE

### Kung walang jumpscare, gawin ito:

1. **Open Console** (Ctrl+Shift+C)
2. **Play game**
3. **Basahin ang errors**

#### Kung nakita mo: "JumpscareManager.Instance is NULL"
→ Walang JumpscareManager sa scene
→ **Fix**: Create JumpscareManager GameObject sa PersistentScene

#### Kung nakita mo: "jumpscarePanel is NULL"
→ Hindi naka-assign ang panel
→ **Fix**: I-drag ang JumpscarePanel sa Inspector

#### Kung nakita mo: "jumpscareImage is NULL"
→ Hindi naka-assign ang image
→ **Fix**: I-drag ang JumpscareImage sa Inspector

#### Kung nakita mo: "One or more sprites are NULL"
→ May kulang na sprite
→ **Fix**: I-drag ang 3 sprites sa Inspector

#### Kung walang error pero walang jumpscare pa rin
→ Canvas sort order mababa
→ **Fix**: Set Canvas Sort Order to 1000

---

## 💡 PRO TIPS

### Tip 1: Gamitin ang Debug Helper
Mas madali mag-debug gamit ang JumpscareDebugHelper script!
- Press D = Full diagnostic
- Press J = Test jumpscare
- Press I = Instance info

### Tip 2: Check Console Always
Lagi mong tignan ang Console para makita ang errors!

### Tip 3: Test Early
Huwag maghintay na mahuli ka ni Emily para mag-test.
Use Debug Helper (Press J) para mag-test agad!

### Tip 4: One Issue at a Time
I-fix ang issues one by one:
1. Fix Instance issue first
2. Then fix references
3. Then test

### Tip 5: Save Often
I-save ang scene after every fix!

---

## 🆘 STILL NOT WORKING?

Kung nag-follow ka na sa lahat pero ayaw pa rin gumana:

### Last Resort Checklist:

1. **Delete JumpscareManager GameObject**
2. **Create new JumpscareManager GameObject**
3. **Add JumpscareManager script**
4. **Assign ALL references from scratch**
5. **Test with Debug Helper (Press J)**

### Verify Scene Loading:

1. Check Build Settings
2. Make sure PersistentScene is in build
3. Make sure PersistentScene loads at start

### Check Script Execution Order:

1. Edit → Project Settings → Script Execution Order
2. Make sure JumpscareManager runs early (negative number)

---

## 📊 VISUAL GUIDE

### Correct Setup:

```
PersistentScene:
│
├─ JumpscareManager ← GameObject with script
│   └─ JumpscareManager (Script)
│       ├─ Jumpscare Panel: ✅ JumpscarePanel
│       ├─ Jumpscare Image: ✅ JumpscareImage
│       ├─ Tilt Left Sprite: ✅ emily_tilt_left
│       ├─ Tilt Right Sprite: ✅ emily_tilt_right
│       ├─ Center Sprite: ✅ emily_center
│       └─ Jumpscare Sound: ✅ jumpscare_audio
│
└─ Canvas (Sort Order: 1000)
    └─ JumpscarePanel (Active: ☐)
        └─ JumpscareImage (Active: ✓)
```

### Console Output (Correct):

```
=== JUMPSCARE MANAGER AWAKE ===
[Jumpscare] ✅ Instance created
=== REFERENCE CHECK ===
Jumpscare Panel: ✅ Assigned
Jumpscare Image: ✅ Assigned
Tilt Left Sprite: ✅ Assigned
Tilt Right Sprite: ✅ Assigned
Center Sprite: ✅ Assigned
[Jumpscare] ✅ All critical references assigned - Ready to use!
```

### Test Result (Correct):

```
Press J → Jumpscare plays → Game over screen shows
```

---

## ✅ FINAL VERIFICATION

Kapag nag-work na, dapat ganito:

1. **Play game**
2. **Console shows**: "JUMPSCARE MANAGER AWAKE" + all ✅
3. **Press J**: Jumpscare plays
4. **Get caught by Emily**: Jumpscare → Game over
5. **All game overs**: Jumpscare → Game over

**Kung lahat gumana**: ✅ SUCCESS! 🎉

**Kung may hindi gumana**: Bumalik sa checklist at i-check ulit!

---

## 📞 DEBUG COMMANDS

### In Play Mode:

- **D** = Full diagnostic (check lahat)
- **J** = Test jumpscare (manual trigger)
- **I** = Instance info (check kung nag-exist)

### Console Messages to Look For:

- ✅ "JUMPSCARE MANAGER AWAKE" = Good!
- ✅ "All critical references assigned" = Good!
- ❌ "NULL" = Bad! I-assign ang reference!
- ❌ "Instance is NULL" = Bad! Check GameObject!

---

**Follow this guide step by step and jumpscare should work!** 👻✨

**Kung may tanong pa, check ang Console at gamitin ang Debug Helper!**
