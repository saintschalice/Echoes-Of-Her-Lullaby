# Room 08 - Quick Fix Checklist ✅

## 🎯 SPRITE SIZE FIX (5 Steps)

### Step 1: Check Normal Sprite
```
Project → Normal Mirror Sprite → Inspector
└─ Pixels Per Unit: _____ (write it down!)
```

### Step 2: Match Broken Sprite
```
Project → Broken Mirror Sprite → Inspector
├─ Pixels Per Unit: _____ (SAME as normal!)
└─ Click Apply
```

### Step 3: Verify GameObject
```
Hierarchy → Mirror → Inspector
└─ Transform Scale: (1, 1, 1)
```

### Step 4: Check Draw Mode
```
Mirror → SpriteRenderer
└─ Draw Mode: Simple
```

### Step 5: Test
```
Play → Complete Puzzle → Check Size
└─ Should stay SAME SIZE ✅
```

---

## 🎮 INTERACTION FIX (Already Done!)

### Scripts Updated:
- ✅ Room08_Interactable.cs (auto-disable)
- ✅ Room08UIManager.cs (auto-enable)

### What It Does:
- Disables mirror during puzzle (no double-click)
- Enables mirror after puzzle (can climb through)

---

## 📋 UNITY SETUP CHECKLIST

### Mirror GameObject:
- [ ] Transform Scale: (1, 1, 1)
- [ ] SpriteRenderer Draw Mode: Simple
- [ ] BoxCollider2D Is Trigger: ✓
- [ ] Room08_Interactable: Type = Mirror

### FlowController:
- [ ] Mirror GameObject: [assigned]
- [ ] Mirror Normal Sprite: [assigned]
- [ ] Mirror Broken Sprite: [assigned]
- [ ] Next Scene Name: "Room09_Master's_Bathroom"

### Sprites:
- [ ] Normal PPU: _____ (note value)
- [ ] Broken PPU: _____ (SAME!)
- [ ] Both same dimensions (e.g., 512x768)

---

## 🧪 TESTING CHECKLIST

- [ ] Collect 2 evidence items
- [ ] Get hammer from cabinet
- [ ] Examine bathtub
- [ ] Interact with mirror → Panel appears
- [ ] Can't double-click during puzzle
- [ ] Complete puzzle (15 taps)
- [ ] Mirror breaks (shatter effect)
- [ ] Mirror sprite changes to broken
- [ ] **Mirror stays SAME SIZE** ← KEY!
- [ ] Can interact with broken mirror
- [ ] Climb through → Load Room 09

---

## 🐛 IF STILL BROKEN

### Mirror lumalaki?
→ Check: Both sprites have SAME Pixels Per Unit
→ Check: Transform Scale is (1, 1, 1)
→ Check: Draw Mode is Simple

### Can't interact with broken mirror?
→ Check: Console for "Re-enabled mirror interactable"
→ Check: Room08UIManager script updated
→ Check: Mirror has Room08_Interactable script

### Double interaction?
→ Check: Room08_Interactable script updated
→ Check: Has `enabled = false` in ExamineMirror()

---

## 📁 NEED MORE HELP?

### Sprite Size Issue:
→ `SPRITE_SIZE_FIX_TAGALOG.md`
→ `PIXELS_PER_UNIT_EXPLAINED.md`

### Complete Setup:
→ `FINAL_MIRROR_SETUP_TAGALOG.md`
→ `ROOM08_ISSUES_FIXED.md`

### Mirror Design:
→ `MIRROR_AS_PASSAGE_SETUP.md`

---

## ⚡ QUICK REFERENCE

### The Golden Rule:
> Same Pixels Per Unit = Same Size in World

### The Formula:
```
World Size = Pixel Size ÷ Pixels Per Unit

Example:
512 pixels ÷ 100 PPU = 5.12 units
512 pixels ÷ 50 PPU = 10.24 units (BIGGER!)
```

### The Fix:
```
Normal Sprite: 512x768, PPU=100 → 5.12x7.68 units
Broken Sprite: 512x768, PPU=100 → 5.12x7.68 units ✅
```

---

**Print this and keep it handy!** 📋✨
