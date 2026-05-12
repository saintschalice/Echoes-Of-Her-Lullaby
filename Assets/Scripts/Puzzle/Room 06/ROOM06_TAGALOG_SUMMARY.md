# Room 06 - Photo Frame Interaction (Tagalog Guide)

## ✅ STATUS: Scripts ay TAMA NA!

Ang dalawang scripts ay **KUMPLETO at TAMA** na:
- `Room06_HallwayController.cs` ✅
- `Room06_PhotoFrameInteractable.cs` ✅

**Kung hindi pa rin ma-interact ang photo frame, problema sa Unity setup, hindi sa code!**

---

## 🎯 ANO ANG MANGYAYARI

### Flow ng Room 06:

1. **Papasok si Lisa sa hallway upstairs**
   - Intro dialogue (automatic)
   - "The upstairs hallway... it feels colder here."
   - "There's a photo frame on the wall. I should take a closer look."

2. **Mag-interact sa photo frame**
   - "A family photo... they look happy."
   - **Panel lalabas** - makikita yung buong photo (normal) - 1.5 seconds
   - **Scratch effect!** - Magiging bloody/scratched yung photo sa panel
   - Scratch sound
   - **Panel auto-close** - 1.0 second
   - **Photo frame sa world** - Magiging bloody na rin yung sprite
   - "What?! The faces... they're scratched out!"
   - "No... she's here!"

3. **Emily spawn**
   - 1.5 second delay
   - Emily lalabas sa spawn point
   - Jumpscare sound
   - Chase music
   - Huhuntin ka ni Emily!

4. **Game Over**
   - Pag nahuli ka ni Emily (within catch distance)

---

## 🔧 KUNG HINDI MA-INTERACT ANG PHOTO FRAME

### Check mo ito sa Unity:

#### 1. PhotoFrame GameObject
- ✅ May `BoxCollider2D` o `CircleCollider2D`
- ✅ **"Is Trigger" ay CHECKED** ← IMPORTANTE ITO!
- ✅ Size ng collider: 1.5 to 2.0 (dapat saklaw yung sprite)
- ✅ May `Room06_PhotoFrameInteractable` script

#### 2. Room06_HallwayController GameObject
- ✅ May `Room06_HallwayController` script
- ✅ **Photo Frame** field - Naka-assign yung PhotoFrame GameObject
- ✅ **Normal Photo Sprite** - Naka-assign yung normal photo
- ✅ **Scratched Photo Sprite** - Naka-assign yung bloody/scratched photo
- ✅ **Photo Panel** - Naka-assign yung UI Panel
- ✅ **Photo Panel Image** - Naka-assign yung Image component sa loob ng panel
- ✅ **Emily Game Object** - Naka-assign si Emily
- ✅ **Emily Spawn Point** - Naka-assign yung spawn point

#### 3. Photo Panel UI
- ✅ May Panel sa Canvas (name: PhotoPanel)
- ✅ May Image sa loob ng Panel (name: PhotoImage)
- ✅ **WALANG close button** - Auto-close lang

#### 4. Emily Setup
- ✅ Emily GameObject ay **DISABLED** sa simula (uncheck sa Hierarchy)
- ✅ May `NavMeshAgent` component
- ✅ May `EmilyGhost` component

#### 5. Emily Spawn Point
- ✅ Empty GameObject (name: Emily_Spawn_Point)
- ✅ Naka-position kung saan dapat lumabas si Emily

---

## 🐛 COMMON PROBLEMS

### Problem: "Hindi lumalabas yung interaction button"
**Dahilan:**
- Collider ng PhotoFrame ay hindi "Is Trigger"
- Collider masyadong maliit
- PhotoFrame nasa wrong layer

**Solution:**
1. Select PhotoFrame sa Hierarchy
2. Inspector → Collider2D
3. Check "Is Trigger" ✅
4. Size: 1.5 to 2.0

### Problem: "Nag-click ako pero walang nangyayari"
**Dahilan:**
- Room06_HallwayController wala sa scene
- PhotoFrame GameObject hindi naka-assign sa controller

**Solution:**
1. Check kung may Room06_HallwayController sa scene
2. Select controller → Inspector
3. Drag PhotoFrame GameObject sa "Photo Frame" field

### Problem: "Hindi lumalabas yung panel"
**Dahilan:**
- Photo Panel o Photo Panel Image hindi naka-assign
- Sprites hindi naka-assign

**Solution:**
1. Select Room06_HallwayController
2. Assign Photo Panel (UI Panel GameObject)
3. Assign Photo Panel Image (Image component)
4. Assign both sprites (normal + scratched)

### Problem: "Hindi lumalabas si Emily"
**Dahilan:**
- Emily GameObject o spawn point hindi naka-assign
- Emily walang NavMeshAgent
- Walang NavMesh sa scene

**Solution:**
1. Assign Emily GameObject sa controller
2. Assign Emily Spawn Point sa controller
3. Check kung may NavMeshAgent si Emily
4. Bake NavMesh sa scene (Window → AI → Navigation)

---

## 📋 QUICK CHECKLIST

Bago mag-test, check mo lahat:

### PhotoFrame GameObject:
- [ ] May Collider2D
- [ ] "Is Trigger" is CHECKED ✅
- [ ] Size: 1.5-2.0
- [ ] May Room06_PhotoFrameInteractable script

### Room06_HallwayController:
- [ ] Naka-assign ang PhotoFrame GameObject
- [ ] Naka-assign ang Normal Photo Sprite
- [ ] Naka-assign ang Scratched Photo Sprite
- [ ] Naka-assign ang Photo Panel
- [ ] Naka-assign ang Photo Panel Image
- [ ] Naka-assign si Emily
- [ ] Naka-assign ang Emily Spawn Point
- [ ] Naka-assign ang audio clips

### Photo Panel UI:
- [ ] May Panel sa Canvas
- [ ] May Image sa loob ng Panel
- [ ] Walang close button (auto-close)

### Emily:
- [ ] DISABLED sa simula
- [ ] May NavMeshAgent
- [ ] May EmilyGhost script

### Scene:
- [ ] May NavMesh (baked)
- [ ] Scene is saved

---

## 🔍 DEBUG MODE

Para makita kung ano nangyayari:

1. Select Room06_HallwayController
2. Check "Debug Mode" ✅
3. Select PhotoFrame
4. Check "Debug Mode" ✅
5. Play mode
6. Tingnan ang Console

**Expected logs:**
```
[Room06] Playing intro sequence
[Room06] Intro sequence complete
[PhotoFrame] Player focused on photo frame
[PhotoFrame] OnInteract called!
[Room06] Photo frame interacted
[Room06] Photo panel opened - showing normal photo
[Room06] Photo scratched in panel!
[Room06] Photo panel closed automatically
[Room06] World photo frame changed to bloody version
[Room06] Spawning Emily!
[Room06] Emily hunting! Speed: 4.5
```

---

## 📖 DETAILED GUIDES

Kung kailangan mo ng mas detalyadong guide:

1. **ROOM06_SETUP_GUIDE.md** - Complete Unity setup
2. **PHOTOFRAME_TROUBLESHOOTING.md** - Detailed troubleshooting
3. **PHOTO_PANEL_UI_SETUP.md** - UI panel setup guide

---

## ✅ FINAL CHECK

Kung lahat naka-setup na:

1. **Save scene**
2. **Enable debug mode** (both scripts)
3. **Play mode**
4. **Approach photo frame**
5. **Click interaction button**
6. **Watch sequence**

Kung may error, tingnan ang Console at basahin ang error message!

---

## 💡 TIPS

### Para sa Testing:
- Enable debug mode para makita ang logs
- Increase collider size kung hindi ma-detect
- Check Console para sa errors

### Para sa Balancing:
- Emily Chase Speed: 3.5 (easy) to 6.0 (hard)
- Catch Distance: 1.0 (recommended)
- Spawn Delay: 1.0 to 2.0 seconds

### Para sa Atmosphere:
- Use creepy scratch sound
- Use loud jumpscare sound
- Use tense chase music
- Dim lighting sa hallway

---

**Scripts ay TAMA NA! Problema lang sa Unity setup kung hindi gumagana!** 🎮✨

**Basahin ang PHOTOFRAME_TROUBLESHOOTING.md para sa detailed help!**
