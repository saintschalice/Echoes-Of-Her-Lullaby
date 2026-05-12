# Room 08 - Mirror as Passage Setup

## 🎯 IMPROVED DESIGN

**Before**: Mirror + separate Passage GameObject (nakapatong, conflict)
**After**: Mirror GameObject itself becomes the passage! ✨

---

## 💡 HOW IT WORKS

1. **Before puzzle**: Mirror shows normal sprite
2. **Interact with mirror**: Panel appears, tap puzzle
3. **After puzzle**: Mirror sprite changes to broken (shows passage)
4. **Interact with broken mirror**: Climb through to Room 09!

**No separate passage GameObject needed!** 🎉

---

## 🔧 UNITY SETUP

### Mirror GameObject (Single Object!)

```
Mirror (GameObject):
├─ SpriteRenderer
│   └─ Sprite: Normal mirror sprite (at start)
├─ BoxCollider2D
│   └─ Is Trigger: ☑ CHECKED
├─ Room08_Interactable (Script)
│   └─ Object Type: Mirror
└─ Transform
    └─ Scale: (1, 1, 1)
```

**That's it! No separate passage GameObject!**

---

## 🎮 ROOM08_FLOWCONTROLLER SETUP

```
Room08_FlowController:
├─ Total Evidence Items: 2
├─ Mirror GameObject: [drag Mirror GameObject]
├─ Mirror Normal Sprite: [normal mirror sprite]
├─ Mirror Broken Sprite: [broken mirror with passage visible]
└─ Next Scene Name: "Room09_Master's_Bathroom"
```

**Removed**:
- ❌ Mirror Sprite Renderer (no longer needed)
- ❌ Passage Object (no longer needed)

**Added**:
- ✅ Mirror GameObject (the whole GameObject)
- ✅ Mirror Normal Sprite
- ✅ Mirror Broken Sprite

---

## 🎨 SPRITE DESIGN

### Normal Mirror Sprite:
- Shows intact mirror
- No passage visible
- Clean glass

### Broken Mirror Sprite:
- Shows shattered mirror
- **Passage visible behind broken glass**
- Cracks and shards
- Dark opening/doorway visible

**Key**: The broken sprite should show the passage! 🚪

---

## 🔄 INTERACTION FLOW

### Before Puzzle:
```
Player → Interact with Mirror
└─ Check prerequisites
    ├─ If not ready: "I need to finish examining everything first."
    └─ If ready: Show mirror panel → Tap puzzle
```

### After Puzzle:
```
Player → Interact with Mirror (now broken)
└─ Climb through passage
    └─ Dialogue: "Time to see what's on the other side..."
    └─ Fade out
    └─ Load Room 09
```

**Same GameObject, different behavior!** 🎯

---

## ✅ ADVANTAGES

### Why This is Better:

1. **No Overlap Issues** ✅
   - No separate passage GameObject
   - No collision conflicts
   - No z-order problems

2. **Simpler Setup** ✅
   - One GameObject instead of two
   - Less to configure
   - Easier to understand

3. **Clearer Logic** ✅
   - Mirror becomes passage
   - Same interaction point
   - Natural progression

4. **Better Visual** ✅
   - Broken sprite shows passage
   - No need to hide/show objects
   - Smooth transition

---

## 📋 STEP-BY-STEP

### Step 1: Create Mirror GameObject

1. Create GameObject: "Mirror"
2. Add SpriteRenderer
   - Sprite: Normal mirror sprite
3. Add BoxCollider2D
   - Is Trigger: ☑
4. Add Room08_Interactable
   - Object Type: Mirror

### Step 2: Prepare Sprites

1. **Normal Mirror Sprite**:
   - Intact mirror
   - No passage visible

2. **Broken Mirror Sprite**:
   - Shattered glass
   - **Passage visible behind it**
   - Same size as normal sprite
   - Same Pixels Per Unit

### Step 3: Configure FlowController

1. Create Room08_FlowController GameObject
2. Assign:
   - Mirror GameObject: [drag Mirror]
   - Mirror Normal Sprite: [normal sprite]
   - Mirror Broken Sprite: [broken sprite with passage]
   - Next Scene Name: "Room09_Master's_Bathroom"

### Step 4: Test

1. **Before puzzle**:
   - Interact with mirror → Panel appears
   - Complete puzzle → Mirror breaks

2. **After puzzle**:
   - Interact with broken mirror → Climb through
   - Fade transition → Load Room 09

---

## 🎨 SPRITE EXAMPLE

### Normal Mirror:
```
┌─────────────┐
│             │
│   MIRROR    │
│   (intact)  │
│             │
│             │
└─────────────┘
```

### Broken Mirror (with passage):
```
┌─────────────┐
│  /\  /\  /\ │ ← Cracks
│ /  \/  \/  \│
│   ┌─────┐   │ ← Dark passage
│   │     │   │   visible behind
│   │ ▓▓▓ │   │   broken glass
└───┴─────┴───┘
```

---

## 🐛 TROUBLESHOOTING

### Issue: "Can't interact with broken mirror"

**Cause**: Collider might be disabled

**Solution**:
- Check BoxCollider2D is still enabled
- Check Is Trigger is still checked
- Check Room08_Interactable script is enabled

### Issue: "Mirror doesn't change sprite"

**Cause**: Sprites not assigned or wrong GameObject

**Solution**:
- Check Mirror GameObject is assigned in FlowController
- Check both sprites are assigned
- Check sprites have same Pixels Per Unit

### Issue: "Passage not visible in broken sprite"

**Cause**: Broken sprite doesn't show passage

**Solution**:
- Edit broken sprite to show dark opening/doorway
- Make it clear there's a passage behind the broken glass

---

## 💡 DESIGN TIPS

### For Broken Mirror Sprite:

1. **Show Passage Clearly**:
   - Dark opening behind glass
   - Visible doorway or tunnel
   - Contrast with broken glass

2. **Maintain Size**:
   - Same dimensions as normal sprite
   - Same Pixels Per Unit
   - Same aspect ratio

3. **Visual Feedback**:
   - Obvious that it's broken
   - Clear that you can go through
   - Inviting to interact

---

## 📝 COMPARISON

### Old Way (Separate Objects):
```
Mirror (GameObject)
└─ Shows normal/broken sprite

Passage (GameObject) ← PROBLEM!
└─ Hidden, then shown
└─ Overlaps with mirror
└─ Collision conflicts
```

### New Way (Single Object):
```
Mirror (GameObject)
├─ Before: Normal sprite
└─ After: Broken sprite (shows passage)
    └─ Same GameObject, different sprite!
    └─ No overlap issues!
```

---

## ✅ FINAL CHECKLIST

- [ ] Mirror GameObject created
- [ ] BoxCollider2D (Is Trigger ✓)
- [ ] Room08_Interactable (Type: Mirror)
- [ ] Normal mirror sprite prepared
- [ ] Broken mirror sprite prepared (shows passage!)
- [ ] Both sprites same size/PPU
- [ ] FlowController configured
- [ ] Mirror GameObject assigned
- [ ] Both sprites assigned
- [ ] Test before puzzle (panel appears)
- [ ] Test after puzzle (can climb through)

---

**Setup complete! One GameObject, two states, no conflicts!** 🪞✨

