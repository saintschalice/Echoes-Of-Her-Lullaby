# Room 08 - Final Mirror Setup (Tagalog)

## 🎯 DALAWANG ISSUES NA NA-FIX

### Issue 1: Mirror lumalaki after puzzle ✅
**Solution**: Match Pixels Per Unit ng both sprites

### Issue 2: Hindi ma-disable ang mirror interaction during puzzle ✅
**Solution**: Auto-disable during puzzle, auto-enable after puzzle

---

## 🔧 COMPLETE SETUP

### Part 1: Fix Sprite Size

#### Step 1: Check Normal Mirror Sprite

1. **Sa Project window**, hanapin ang **normal mirror sprite**
2. **Click** sa sprite
3. **Sa Inspector**, tingnan:
   - Texture Type: **Sprite (2D and UI)**
   - Sprite Mode: **Single**
   - **Pixels Per Unit**: (note ang value, e.g., 100)

#### Step 2: Match Broken Mirror Sprite

1. **Sa Project window**, hanapin ang **broken mirror sprite**
2. **Click** sa sprite
3. **Sa Inspector**, i-set:
   - Texture Type: **Sprite (2D and UI)**
   - Sprite Mode: **Single**
   - **Pixels Per Unit**: (SAME value as normal sprite!)
4. **Click Apply**

#### Step 3: Verify Mirror GameObject

1. **Sa Hierarchy**, select **Mirror GameObject**
2. **Sa Inspector**, check:
   - Transform → Scale: **(1, 1, 1)**
   - SpriteRenderer → Draw Mode: **Simple**
   - SpriteRenderer → Sprite: [normal mirror sprite]

---

### Part 2: Mirror GameObject Setup

Ang Mirror GameObject ay **single object** na mag-transform from normal to broken:

```
Mirror (GameObject):
├─ Transform:
│   └─ Scale: (1, 1, 1) ← IMPORTANT!
│
├─ SpriteRenderer:
│   ├─ Sprite: [Normal mirror sprite]
│   └─ Draw Mode: Simple
│
├─ BoxCollider2D:
│   └─ Is Trigger: ☑ CHECKED
│
└─ Room08_Interactable (Script):
    └─ Object Type: Mirror
```

**IMPORTANT**: 
- ❌ WALA nang separate Passage GameObject!
- ✅ Ang Mirror mismo ang magiging passage after puzzle!

---

### Part 3: FlowController Setup

```
Room08_FlowController (GameObject):
└─ Room08_FlowController (Script):
    ├─ Total Evidence Items: 2
    ├─ Mirror GameObject: [drag Mirror GameObject from Hierarchy]
    ├─ Mirror Normal Sprite: [drag normal sprite from Project]
    ├─ Mirror Broken Sprite: [drag broken sprite from Project]
    └─ Next Scene Name: "Room09_Master's_Bathroom"
```

**REMOVED** (hindi na kailangan):
- ❌ Mirror Sprite Renderer field
- ❌ Passage Object field

**ADDED** (new fields):
- ✅ Mirror GameObject (whole GameObject)
- ✅ Mirror Normal Sprite
- ✅ Mirror Broken Sprite

---

## 🎮 HOW IT WORKS

### Before Puzzle:
1. Player interacts with Mirror
2. Check prerequisites (evidence, hammer, bathtub)
3. If ready: Show mirror panel
4. **Mirror interactable auto-disables** (prevents double-click)

### During Puzzle:
1. Player taps 15 times in 25 seconds
2. Fill bar increases
3. Mirror cracks progressively

### After Puzzle:
1. Mirror breaks (shatter effect)
2. **Mirror sprite changes to broken** (shows passage)
3. **Mirror interactable auto-enables** (can interact again)
4. Panel closes

### Climbing Through:
1. Player interacts with broken mirror
2. Dialogue: "Time to see what's on the other side..."
3. Fade out
4. Load Room 09

---

## 🎨 SPRITE REQUIREMENTS

### Normal Mirror Sprite:
- Shows intact mirror
- No passage visible
- Clean glass
- **Pixels Per Unit**: (e.g., 100)
- **Size**: (e.g., 512x768)

### Broken Mirror Sprite:
- Shows shattered glass
- **Passage visible behind broken glass** ← IMPORTANT!
- Cracks and shards
- Dark opening/doorway visible
- **Pixels Per Unit**: (SAME as normal, e.g., 100)
- **Size**: (SAME as normal, e.g., 512x768)

**Key Point**: Ang broken sprite ay dapat may visible passage/doorway sa likod ng broken glass!

---

## 🔄 INTERACTION FLOW

### Flow Diagram:

```
Player → Interact with Mirror
    ↓
Prerequisites check:
├─ All evidence collected? ✓
├─ Hammer obtained? ✓
└─ Bathtub examined? ✓
    ↓
Show Mirror Panel
    ↓
Disable Mirror Interactable (prevent double-click)
    ↓
Player completes puzzle (15 taps)
    ↓
Mirror breaks (sprite changes to broken)
    ↓
Enable Mirror Interactable (can interact again)
    ↓
Player → Interact with Broken Mirror
    ↓
Climb through passage → Load Room 09
```

---

## ✅ UPDATED SCRIPTS

### Room08_Interactable.cs
- ✅ Auto-disables during puzzle
- ✅ Prevents double-interaction
- ✅ Re-enabled after puzzle by UIManager

### Room08UIManager.cs
- ✅ Re-enables mirror interactable after puzzle
- ✅ Finds mirror interactable automatically
- ✅ Logs debug message

### Room08_FlowController.cs
- ✅ Changes mirror sprite to broken
- ✅ Uses Mirror GameObject (not separate passage)
- ✅ Handles climb through transition

---

## 🐛 TROUBLESHOOTING

### Issue: "Mirror lumalaki after puzzle"

**Cause**: Different Pixels Per Unit

**Solution**:
1. Check normal sprite PPU (e.g., 100)
2. Set broken sprite PPU to SAME value
3. Click Apply
4. Test again

---

### Issue: "Hindi ma-interact ang broken mirror"

**Cause**: Interactable not re-enabled

**Solution**:
1. Check Console for: "Re-enabled mirror interactable for passage"
2. If wala, check Room08UIManager script is updated
3. Check Mirror GameObject has Room08_Interactable script

---

### Issue: "Nag-double trigger ang puzzle"

**Cause**: Interactable not disabled during puzzle

**Solution**:
1. Check Room08_Interactable script is updated
2. Should auto-disable when showing panel
3. Should auto-enable after puzzle

---

### Issue: "Walang passage sa broken sprite"

**Cause**: Broken sprite doesn't show passage

**Solution**:
1. Edit broken sprite in image editor
2. Add dark opening/doorway behind broken glass
3. Make it obvious na may passage
4. Re-import to Unity

---

## 📋 FINAL CHECKLIST

### Sprites:
- [ ] Normal mirror sprite: PPU = _____ (note value)
- [ ] Broken mirror sprite: PPU = _____ (SAME as normal)
- [ ] Both sprites same dimensions (e.g., 512x768)
- [ ] Broken sprite shows visible passage

### Mirror GameObject:
- [ ] Transform Scale: (1, 1, 1)
- [ ] SpriteRenderer Draw Mode: Simple
- [ ] BoxCollider2D Is Trigger: ✓
- [ ] Room08_Interactable: Type = Mirror

### FlowController:
- [ ] Mirror GameObject assigned
- [ ] Mirror Normal Sprite assigned
- [ ] Mirror Broken Sprite assigned
- [ ] Next Scene Name: "Room09_Master's_Bathroom"

### Scripts Updated:
- [ ] Room08_Interactable.cs (auto-disable/enable)
- [ ] Room08UIManager.cs (re-enable interactable)
- [ ] Room08_FlowController.cs (sprite change)

### Testing:
- [ ] Interact with mirror → Panel appears
- [ ] Complete puzzle → Mirror breaks
- [ ] Mirror sprite changes to broken
- [ ] Mirror stays SAME SIZE
- [ ] Can interact with broken mirror
- [ ] Climb through → Load Room 09

---

## 💡 KEY POINTS

1. **One GameObject**: Mirror itself becomes passage (no separate passage object)

2. **Sprite Size**: Both sprites must have SAME Pixels Per Unit

3. **Auto-Disable**: Mirror interactable disables during puzzle, enables after

4. **Broken Sprite**: Must show visible passage behind broken glass

5. **Transform Scale**: Always (1, 1, 1), never scale sprites in Unity

---

## 🎯 QUICK TEST

1. **Play game**
2. **Collect evidence** (2 items)
3. **Get hammer** from cabinet
4. **Examine bathtub**
5. **Interact with mirror** → Panel appears
6. **Complete puzzle** (15 taps)
7. **Check mirror size** → Should stay same!
8. **Interact with broken mirror** → Should climb through
9. **Load Room 09** → Success!

---

**Setup complete! Mirror should work perfectly now!** 🪞✨

## 📝 SUMMARY OF CHANGES

### What Changed:

1. **Room08_Interactable.cs**:
   - Added `enabled = false` when showing mirror panel
   - Prevents double-interaction during puzzle

2. **Room08UIManager.cs**:
   - Added code to re-enable mirror interactable after puzzle
   - Finds mirror interactable and sets `enabled = true`

3. **Sprite Setup**:
   - Both sprites must have same Pixels Per Unit
   - Mirror GameObject Transform Scale must be (1, 1, 1)

### Why These Changes:

- **Auto-disable**: Prevents player from clicking mirror multiple times during puzzle
- **Auto-enable**: Allows player to interact with broken mirror to climb through
- **Same PPU**: Ensures both sprites render at same size in world space
- **Scale (1,1,1)**: Prevents unexpected size changes

---

**Tapos na! Test mo na!** 🎮✨
