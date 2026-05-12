# Room 06 - Visual Setup Checklist ✅

## 🎯 QUICK VISUAL GUIDE

Use this checklist to verify your Unity setup step-by-step!

---

## 1️⃣ PHOTOFRAME GAMEOBJECT

### Hierarchy View:
```
📦 PhotoFrame
  ├─ 🖼️ SpriteRenderer
  ├─ 📦 BoxCollider2D (or CircleCollider2D)
  └─ 📜 Room06_PhotoFrameInteractable
```

### Inspector - PhotoFrame:
```
┌─────────────────────────────────────┐
│ PhotoFrame                          │
├─────────────────────────────────────┤
│ Transform                           │
│   Position: (x, y, 0)               │
│   Rotation: (0, 0, 0)               │
│   Scale: (1, 1, 1)                  │
├─────────────────────────────────────┤
│ Sprite Renderer                     │
│   Sprite: [Normal Photo Sprite]     │
│   Color: White (255, 255, 255, 255) │
├─────────────────────────────────────┤
│ Box Collider 2D                     │
│   ☑ Is Trigger  ← MUST BE CHECKED! │
│   Size: (1.5, 1.5) or (2.0, 2.0)   │
│   Offset: (0, 0)                    │
├─────────────────────────────────────┤
│ Room06_PhotoFrameInteractable       │
│   ☑ Debug Mode                      │
└─────────────────────────────────────┘
```

**✅ CRITICAL**: "Is Trigger" MUST be checked!

---

## 2️⃣ ROOM06_HALLWAYCONTROLLER GAMEOBJECT

### Hierarchy View:
```
📦 Room06_Controller (or similar name)
  └─ 📜 Room06_HallwayController
```

### Inspector - Room06_HallwayController:
```
┌─────────────────────────────────────────────────┐
│ Room06_HallwayController                        │
├─────────────────────────────────────────────────┤
│ ▼ Photo Frame                                   │
│   Photo Frame: [PhotoFrame GameObject] ← DRAG  │
│   Normal Photo Sprite: [Sprite] ← ASSIGN       │
│   Scratched Photo Sprite: [Sprite] ← ASSIGN    │
├─────────────────────────────────────────────────┤
│ ▼ Photo Panel UI                                │
│   Photo Panel: [Canvas/PhotoPanel] ← DRAG      │
│   Photo Panel Image: [Image Component] ← DRAG  │
├─────────────────────────────────────────────────┤
│ ▼ Emily Configuration                           │
│   Emily Game Object: [Emily] ← DRAG            │
│   Emily Spawn Point: [Emily_Spawn_Point] ← DRAG│
│   Emily Chase Speed: 4.5                        │
│   Catch Distance: 1.0                           │
├─────────────────────────────────────────────────┤
│ ▼ Audio                                         │
│   Scratch Sound: [AudioClip] ← ASSIGN          │
│   Emily Spawn Sound: [AudioClip] ← ASSIGN      │
│   Chase Music Loop: [AudioClip] ← ASSIGN       │
│   Room Audio Source: [AudioSource] (optional)  │
├─────────────────────────────────────────────────┤
│ ▼ Timing                                        │
│   Spawn Delay: 1.5                              │
├─────────────────────────────────────────────────┤
│ ▼ Persistence                                   │
│   Intro Dialogue Flag: "Room06_Intro"          │
│   Photo Interacted Flag: "Room06_PhotoInteracted"│
├─────────────────────────────────────────────────┤
│ ▼ Debug                                         │
│   ☑ Debug Mode                                  │
└─────────────────────────────────────────────────┘
```

**✅ CRITICAL**: All GameObject references must be assigned!

---

## 3️⃣ PHOTO PANEL UI

### Hierarchy View (Canvas):
```
📦 Canvas
  └─ 📦 PhotoPanel
      └─ 🖼️ PhotoImage (Image component)
```

### Inspector - PhotoPanel:
```
┌─────────────────────────────────────┐
│ PhotoPanel                          │
├─────────────────────────────────────┤
│ Rect Transform                      │
│   Anchors: Stretch (full screen)    │
│   Left: 0, Right: 0                 │
│   Top: 0, Bottom: 0                 │
├─────────────────────────────────────┤
│ Image (optional background)         │
│   Color: Black with alpha           │
└─────────────────────────────────────┘
```

### Inspector - PhotoImage:
```
┌─────────────────────────────────────┐
│ PhotoImage                          │
├─────────────────────────────────────┤
│ Rect Transform                      │
│   Anchors: Center                   │
│   Width: 800-1000                   │
│   Height: 600-800                   │
├─────────────────────────────────────┤
│ Image                               │
│   Source Image: [None initially]    │
│   Preserve Aspect: ☑ (recommended)  │
└─────────────────────────────────────┘
```

**✅ NO CLOSE BUTTON NEEDED** - Panel auto-closes!

---

## 4️⃣ EMILY SPAWN POINT

### Hierarchy View:
```
📦 Emily_Spawn_Point (Empty GameObject)
```

### Inspector - Emily_Spawn_Point:
```
┌─────────────────────────────────────┐
│ Emily_Spawn_Point                   │
├─────────────────────────────────────┤
│ Transform                           │
│   Position: (x, y, 0) ← Where Emily │
│             should spawn            │
│   Rotation: (0, 0, 0)               │
│   Scale: (1, 1, 1)                  │
└─────────────────────────────────────┘
```

**💡 TIP**: Position this where you want Emily to appear!

---

## 5️⃣ EMILY GAMEOBJECT

### Hierarchy View:
```
📦 Emily (DISABLED initially - uncheck checkbox)
  ├─ 🤖 NavMeshAgent
  ├─ 📜 EmilyGhost
  └─ 📜 EmilyMovement (if applicable)
```

### Inspector - Emily:
```
┌─────────────────────────────────────┐
│ ☐ Emily  ← MUST BE UNCHECKED!      │
├─────────────────────────────────────┤
│ Transform                           │
│   Position: (any - will be moved)   │
├─────────────────────────────────────┤
│ Nav Mesh Agent                      │
│   Speed: (will be set by script)    │
│   Stopping Distance: 0.1            │
│   Auto Braking: ☑                   │
├─────────────────────────────────────┤
│ EmilyGhost                          │
│   (Your existing settings)          │
└─────────────────────────────────────┘
```

**✅ CRITICAL**: Emily must be DISABLED at start!

---

## 🎯 COMPLETE SCENE HIERARCHY

```
Room06_ReturnToHallwayUpStairs (Scene)
├─ 📦 Room06_Controller
│   └─ 📜 Room06_HallwayController
│
├─ 📦 PhotoFrame
│   ├─ 🖼️ SpriteRenderer (normal photo)
│   ├─ 📦 BoxCollider2D (Is Trigger ✓)
│   └─ 📜 Room06_PhotoFrameInteractable
│
├─ 📦 Emily_Spawn_Point (Empty)
│
├─ 📦 Emily (DISABLED ☐)
│   ├─ 🤖 NavMeshAgent
│   └─ 📜 EmilyGhost
│
├─ 📦 Player
│   └─ (Your player setup)
│
└─ 📦 Canvas
    └─ 📦 PhotoPanel (initially inactive)
        └─ 🖼️ PhotoImage
```

---

## ✅ VERIFICATION CHECKLIST

### Before Testing:

#### PhotoFrame:
- [ ] Has SpriteRenderer with normal photo sprite
- [ ] Has Collider2D with "Is Trigger" ✅ CHECKED
- [ ] Collider size is 1.5-2.0
- [ ] Has Room06_PhotoFrameInteractable script
- [ ] Debug Mode enabled

#### Room06_Controller:
- [ ] Has Room06_HallwayController script
- [ ] PhotoFrame GameObject assigned
- [ ] Normal Photo Sprite assigned
- [ ] Scratched Photo Sprite assigned
- [ ] Photo Panel assigned
- [ ] Photo Panel Image assigned
- [ ] Emily GameObject assigned
- [ ] Emily Spawn Point assigned
- [ ] All audio clips assigned
- [ ] Debug Mode enabled

#### Photo Panel UI:
- [ ] PhotoPanel exists in Canvas
- [ ] PhotoImage exists inside PhotoPanel
- [ ] No close button (auto-close)
- [ ] Panel initially inactive

#### Emily:
- [ ] Emily GameObject is DISABLED (unchecked)
- [ ] Has NavMeshAgent component
- [ ] Has EmilyGhost component

#### Scene:
- [ ] NavMesh is baked (Window → AI → Navigation → Bake)
- [ ] Scene is saved

---

## 🔍 VISUAL DEBUG CHECK

### In Scene View:

1. **Select Room06_Controller**
   - You should see RED GIZMOS:
     - Red wire sphere at spawn point
     - Red line connecting controller to spawn point
     - Red wire circle showing catch distance

2. **Select PhotoFrame**
   - You should see GREEN COLLIDER outline
   - Collider should cover the sprite area

3. **Select Emily_Spawn_Point**
   - Position should be visible in scene
   - Should be on NavMesh (blue area)

---

## 🎮 TESTING SEQUENCE

### Step-by-Step Test:

1. **Press Play**
   - [ ] Intro dialogue appears automatically
   - [ ] Console shows: "[Room06] Playing intro sequence"

2. **Approach PhotoFrame**
   - [ ] Interaction button appears
   - [ ] Console shows: "[PhotoFrame] Player focused on photo frame"

3. **Click Interaction Button**
   - [ ] Console shows: "[PhotoFrame] OnInteract called!"
   - [ ] Console shows: "[Room06] Photo frame interacted"
   - [ ] Panel opens with normal photo
   - [ ] Console shows: "[Room06] Photo panel opened - showing normal photo"

4. **Wait 1.5 seconds**
   - [ ] Scratch sound plays
   - [ ] Photo changes to scratched in panel
   - [ ] Console shows: "[Room06] Photo scratched in panel!"

5. **Wait 1.0 second**
   - [ ] Panel closes automatically
   - [ ] Console shows: "[Room06] Photo panel closed automatically"
   - [ ] World photo frame changes to bloody sprite
   - [ ] Console shows: "[Room06] World photo frame changed to bloody version"

6. **Wait for dialogue**
   - [ ] Reaction dialogue appears
   - [ ] "What?! The faces... they're scratched out!"
   - [ ] "No... she's here!"

7. **Wait 1.5 seconds**
   - [ ] Emily spawns
   - [ ] Console shows: "[Room06] Spawning Emily!"
   - [ ] Console shows: "[Room06] Emily hunting! Speed: 4.5"
   - [ ] Jumpscare sound plays
   - [ ] Chase music starts

8. **Run from Emily**
   - [ ] Emily chases player
   - [ ] If caught: Game Over triggers

---

## 🐛 IF SOMETHING DOESN'T WORK

### Check Console for Errors:
- Red error messages = Something is missing
- Yellow warnings = Check references

### Common Console Messages:

**"[PhotoFrame] Room06_HallwayController not found!"**
→ Room06_Controller GameObject missing or script not attached

**"[Room06] Missing Emily GameObject or spawn point!"**
→ Emily or spawn point not assigned in Inspector

**No console messages at all?**
→ Debug Mode not enabled on scripts

---

## 📖 NEED MORE HELP?

See these guides:
1. **PHOTOFRAME_TROUBLESHOOTING.md** - Detailed troubleshooting
2. **ROOM06_TAGALOG_SUMMARY.md** - Tagalog guide
3. **ROOM06_SETUP_GUIDE.md** - Complete setup guide

---

**Follow this checklist and everything will work!** ✅🎮✨
