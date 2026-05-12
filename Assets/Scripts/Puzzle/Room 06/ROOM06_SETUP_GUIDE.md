# Room 06 - Hallway Upstairs Setup Guide

## ✅ SCRIPTS STATUS: COMPLETE & CORRECT!
**If photo frame interaction not working, see `PHOTOFRAME_TROUBLESHOOTING.md`**

## 📋 Overview

**Room 06 Flow:**
1. Lisa enters upstairs hallway → Intro dialogue
2. Player examines photo frame → Panel shows normal photo (1.5s)
3. Photo transitions to scratched/bloody in panel
4. Panel auto-closes (1.0s)
5. World photo frame GameObject changes to bloody sprite
6. Reaction dialogue
7. Emily spawns at spawn point → Hunts player

---

## 🔧 Unity Setup

### Step 1: Create Room Controller

1. Create empty GameObject: `Room06_HallwayController`
2. Add Component → **Room06_HallwayController**
3. Configure settings (see below)

### Step 2: Setup Photo Frame

1. Create Sprite GameObject: `PhotoFrame`
2. Add Component → **Room06_PhotoFrameInteractable**
3. Add Component → **BoxCollider2D** (or CircleCollider2D)
   - ⚠️ **CRITICAL**: Check "Is Trigger" ✅
   - Set Size to 1.5-2.0 (must cover sprite area)
4. Set Layer to **Interactable** (if you have one)

### Step 3: Setup Photo Panel UI

1. In Canvas, create Panel: `PhotoPanel`
2. Inside PhotoPanel, create Image: `PhotoImage`
3. Set PhotoImage to stretch/fill panel
4. **NO close button needed** - Panel auto-closes after transition

### Step 4: Create Emily Spawn Point

1. Create empty GameObject: `Emily_Spawn_Point`
2. Position where Emily should appear
3. This is where Emily teleports after photo interaction

### Step 5: Setup Emily

1. Drag Emily prefab/GameObject into scene
2. Make sure Emily has:
   - `NavMeshAgent` component
   - `EmilyGhost` component
   - `EmilyMovement` component
3. **Disable Emily** in Hierarchy (uncheck checkbox)

---

## ⚙️ Inspector Settings

### Room06_HallwayController

**Photo Frame:**
- Photo Frame: Drag `PhotoFrame` GameObject
- Normal Photo Sprite: Drag normal family photo sprite
- Scratched Photo Sprite: Drag scratched photo sprite

**Photo Panel UI:**
- Photo Panel: Drag UI Panel GameObject
- Photo Panel Image: Drag Image component inside panel
- Photo Panel Close Button: Drag close button (optional)

**Emily Configuration:**
- Emily Game Object: Drag Emily GameObject (from Hierarchy)
- Emily Spawn Point: Drag `Emily_Spawn_Point`
- Emily Chase Speed: 4.5 (recommended)
- Catch Distance: 1.0

**Audio:**
- Scratch Sound: Sound when photo gets scratched
- Emily Spawn Sound: Jumpscare sound
- Chase Music Loop: Looping chase music
- Room Audio Source: AudioSource component (optional)

**Timing:**
- Spawn Delay: 1.5 seconds (delay after scratch before Emily spawns)

**Persistence:**
- Intro Dialogue Flag: "Room06_Intro"
- Photo Interacted Flag: "Room06_PhotoInteracted"

**Debug:**
- Debug Mode: ✅ Enabled (for testing)

---

## 🎮 How It Works

### Flow:

1. **Player Enters Room:**
   - Intro dialogue plays automatically
   - "The upstairs hallway... it feels colder here."
   - "There's a photo frame on the wall. I should take a closer look."

2. **Player Examines Photo:**
   - "A family photo... they look happy."
   - **SCRATCH EFFECT!** Photo sprite changes
   - Scratch sound plays
   - "What?! The faces... they're scratched out!"
   - "No... she's here!"

3. **Emily Spawns:**
   - 1.5 second delay
   - Emily appears at spawn point
   - Jumpscare sound plays
   - Chase music starts looping
   - Emily hunts player!

4. **Game Over:**
   - If Emily catches player (within catch distance)
   - Game Over screen appears

---

## 🎯 Recommended Settings

### Easy Mode:
```
Emily Chase Speed: 3.5
Catch Distance: 1.0
Spawn Delay: 2.0
```

### Normal Mode:
```
Emily Chase Speed: 4.5
Catch Distance: 1.0
Spawn Delay: 1.5
```

### Hard Mode:
```
Emily Chase Speed: 6.0
Catch Distance: 1.5
Spawn Delay: 1.0
```

---

## 🔍 Visual Debugging

When you select Room06_HallwayController in Scene view:

**Red Wire Sphere:**
- Emily spawn point

**Red Line:**
- Connection between controller and spawn point

**Red Wire Circle:**
- Catch distance (Game Over radius)

---

## 📝 Required Assets

### Sprites:
1. **Normal Photo** - Family photo (before scratch)
2. **Scratched Photo** - Same photo with faces scratched out

### Audio:
1. **Scratch Sound** - Scratching/tearing sound effect
2. **Emily Spawn Sound** - Jumpscare/scream sound
3. **Chase Music** - Looping tense music

### GameObjects:
1. **PhotoFrame** - Sprite with collider
2. **Emily** - AI enemy with NavMeshAgent
3. **Emily_Spawn_Point** - Empty GameObject for spawn position

---

## ✅ Testing Checklist

### Initial State:
- [ ] Emily is **DISABLED** in Hierarchy
- [ ] Photo frame shows **normal sprite**
- [ ] Room controller has all references assigned

### Intro Sequence:
- [ ] Enter room → Intro dialogue plays
- [ ] Player controls disabled during dialogue
- [ ] Player controls restored after dialogue

### Photo Interaction:
- [ ] Interact with photo → Examination dialogue
- [ ] Photo sprite changes to scratched version
- [ ] Scratch sound plays
- [ ] Reaction dialogue plays
- [ ] 1.5 second delay

### Emily Spawn:
- [ ] Emily appears at spawn point
- [ ] Jumpscare sound plays
- [ ] Chase music starts looping
- [ ] Emily chases player
- [ ] Emily speed feels right

### Game Over:
- [ ] Emily catches player → Game Over triggers
- [ ] Chase music stops
- [ ] Game Over message appears

---

## 🐛 Troubleshooting

### Intro doesn't play
**Solution:**
- Check SaveSystem - might be marked as played
- Delete PlayerPrefs to reset
- Check Debug Mode for messages

### Photo doesn't scratch
**Solution:**
- Check if both sprites are assigned
- Check if photo frame has SpriteRenderer
- Check Console for errors

### Emily doesn't spawn
**Solution:**
- Check if Emily GameObject is assigned
- Check if spawn point is assigned
- Check if Emily has required components
- Check Debug Mode for messages

### Emily doesn't chase
**Solution:**
- Check if Emily has NavMeshAgent
- Check if NavMesh is baked in scene
- Check Emily Chase Speed (not 0)
- Check if EmilyGhost component is working

### No Game Over
**Solution:**
- Check if catch distance is not too small
- Check if GameOverManager exists in scene
- Check Debug Mode for catch messages

---

## 💡 Tips

### For Better Pacing:
- Adjust spawn delay (1.0-2.0 seconds)
- Adjust Emily speed (3.5-6.0)
- Add camera shake on scratch
- Add screen flash on Emily spawn

### For Atmosphere:
- Use creepy ambient sound
- Dim lighting in hallway
- Add flickering lights
- Use dramatic music

### For Difficulty:
- **Easy**: Slow Emily (3.5), long delay (2.0s)
- **Normal**: Medium Emily (4.5), medium delay (1.5s)
- **Hard**: Fast Emily (6.0), short delay (1.0s)

---

## 🎬 Dialogue Reference

### Intro:
1. "The upstairs hallway... it feels colder here."
2. "There's a photo frame on the wall. I should take a closer look."

### Photo Examination:
1. "A family photo... they look happy."
2. "What?! The faces... they're scratched out!"
3. "No... she's here!"

### Already Interacted:
- "The faces are scratched out... just like the others."

---

## 📋 Quick Setup Checklist

1. [ ] Create Room06_HallwayController GameObject
2. [ ] Create PhotoFrame GameObject with sprite
3. [ ] Create Emily_Spawn_Point GameObject
4. [ ] Add Emily to scene (disabled)
5. [ ] Assign all references in Inspector
6. [ ] Assign both photo sprites (normal + scratched)
7. [ ] Assign audio clips (scratch, spawn, chase)
8. [ ] Set Emily speed and catch distance
9. [ ] Test intro dialogue
10. [ ] Test photo interaction
11. [ ] Test Emily spawn and chase
12. [ ] Test Game Over

---

## ✅ Done!

**Room 06 is ready!** Simple but effective chase sequence! 🎮✨

---

## 🔧 Inspector Preview

```
Room06_HallwayController:

Photo Frame:
├─ Photo Frame: PhotoFrame GameObject
├─ Normal Photo Sprite: FamilyPhoto_Normal
└─ Scratched Photo Sprite: FamilyPhoto_Scratched

Emily Configuration:
├─ Emily Game Object: Emily (from Hierarchy)
├─ Emily Spawn Point: Emily_Spawn_Point
├─ Emily Chase Speed: 4.5
└─ Catch Distance: 1.0

Audio:
├─ Scratch Sound: ScratchSound.wav
├─ Emily Spawn Sound: Jumpscare.wav
├─ Chase Music Loop: ChaseMusic.wav
└─ Room Audio Source: (optional)

Timing:
└─ Spawn Delay: 1.5

Persistence:
├─ Intro Dialogue Flag: "Room06_Intro"
└─ Photo Interacted Flag: "Room06_PhotoInteracted"

Debug:
└─ Debug Mode: ☑
```

**Ready to use!** 💪✨
