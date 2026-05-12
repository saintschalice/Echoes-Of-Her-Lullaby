# Room 09 - Player Spawn Setup (Tagalog)

## 🎯 Ano ang Kailangan?

1. **Spawn Point** - GameObject kung saan lalabas si Lisa pag load ng Room 09
2. **Emily Position** - Emily lalabas malapit kay Lisa automatically

---

## 🔧 Step-by-Step Setup

### Step 1: Gumawa ng Spawn Point GameObject

1. Sa Room 09 scene, create empty GameObject
2. Name: `PlayerSpawnPoint`
3. Position: Kung saan dapat lumabas si Lisa (example: `0, -2, 0`)
4. **Yan lang!** Marker lang ito, walang sprite needed

---

### Step 2: Gumawa ng Player Spawner

1. Create empty GameObject: `Room09_PlayerSpawner`
2. Add component: `Room09_PlayerSpawner`
3. Sa Inspector:
   - **Spawn Point**: I-drag yung `PlayerSpawnPoint` GameObject
   - **Spawn On Start**: ✅ Check (automatic spawn)
   - **Face Right**: ✅ Check kung dapat nakaharap sa kanan

---

### Step 3: Setup Emily

1. Find `Room09_FlowController` GameObject
2. Sa Inspector:
   - **Emily Manifestation**: I-drag yung Emily GameObject
   - **Emily Distance From Player**: `2` (2 units away from Lisa)

**Emily will automatically spawn near Lisa!** ✅

---

## 🎮 Paano Gumagana

### Pag Load ng Room 09:

1. **Room09_PlayerSpawner** → Moves Lisa to `PlayerSpawnPoint` position
2. **Room09_FlowController** → Positions Emily near Lisa (2 units away)
3. **Intro Sequence** → Dialogue starts
4. **Player Can Move** → After dialogue

---

## 📍 Recommended Positions

### PlayerSpawnPoint:
```
Position: (0, -2, 0) ← Center ng bathroom, floor level
```

### Emily:
```
Distance From Player: 2 ← 2 units to the right of Lisa
```

**Emily's position is automatic!** Based on Lisa's position + distance.

---

## 🎨 Visual Guide

```
Room 09 Layout:

[Broken Mirror] ← Entry point (visual only)
       ↓
[PlayerSpawnPoint] ← Lisa spawns here (0, -2, 0)
       ↓
[Lisa]  →  [Emily] ← Emily spawns 2 units to the right
       ↓
[4 Mirrors] ← Puzzles
```

---

## 🔧 Unity Hierarchy

```
Room 09 Scene:
├── PlayerSpawnPoint (empty GameObject, position marker)
├── Room09_PlayerSpawner (empty GameObject with script)
├── Room09_FlowController (empty GameObject with script)
├── Emily_Manifestation (sprite)
├── Player (will be moved to spawn point)
└── 4 Mirrors (interactables)
```

---

## 🎯 Inspector Setup

### Room09_PlayerSpawner:

**Spawn Point**:
- Drag `PlayerSpawnPoint` GameObject here

**Settings**:
- Spawn On Start: ✅ Checked
- Face Right: ✅ Checked (or uncheck kung kaliwa)

---

### Room09_FlowController:

**Emily State**:
- Emily Manifestation: Drag `Emily_Manifestation` GameObject
- Emily Distance From Player: `2` (or adjust)

---

## 🎬 Testing

### Test 1: Player Spawn
1. Play Room 09 scene
2. **Expected**: Lisa appears at PlayerSpawnPoint position
3. **Expected**: Lisa faces correct direction

### Test 2: Emily Position
1. Play Room 09 scene
2. **Expected**: Emily appears near Lisa (2 units away)
3. **Expected**: Emily is visible

### Test 3: Scene Transition
1. Play Room 08
2. Break mirror (complete QTE)
3. **Expected**: Room 09 loads
4. **Expected**: Lisa spawns at PlayerSpawnPoint
5. **Expected**: Emily spawns near Lisa

---

## 🐛 Troubleshooting

### Problem 1: Lisa hindi lumalabas sa spawn point
**Cause**: Walang assigned na Spawn Point
**Fix**: I-drag ang `PlayerSpawnPoint` sa Inspector

### Problem 2: Emily hindi lumalabas
**Cause**: Walang assigned na Emily Manifestation
**Fix**: I-drag ang `Emily_Manifestation` GameObject sa Inspector

### Problem 3: Emily masyadong malayo o malapit
**Cause**: Mali ang distance setting
**Fix**: Adjust `Emily Distance From Player` (try 1.5, 2, or 3)

### Problem 4: Lisa nakaharap sa mali
**Cause**: Mali ang Face Right setting
**Fix**: Toggle `Face Right` checkbox

---

## 💡 Tips

### PlayerSpawnPoint Position:
- **Center ng bathroom** - Para balanced
- **Floor level** - Hindi nakalutang
- **May space** - Para may room si Emily

### Emily Distance:
- **2 units** - Good default
- **1.5 units** - Kung gusto mo mas malapit
- **3 units** - Kung gusto mo mas malayo

### Facing Direction:
- **Face Right** ✅ - Kung nasa kanan yung mirrors
- **Face Right** ❌ - Kung nasa kaliwa yung mirrors

---

## 📝 Quick Setup Checklist

- [ ] Create `PlayerSpawnPoint` GameObject
- [ ] Position `PlayerSpawnPoint` (example: 0, -2, 0)
- [ ] Create `Room09_PlayerSpawner` GameObject
- [ ] Add `Room09_PlayerSpawner` component
- [ ] Assign `PlayerSpawnPoint` to Spawn Point field
- [ ] Check `Spawn On Start`
- [ ] Set `Face Right` (check or uncheck)
- [ ] Find `Room09_FlowController`
- [ ] Assign `Emily_Manifestation` GameObject
- [ ] Set `Emily Distance From Player` (default: 2)
- [ ] Test!

---

## 🎯 Summary

**Simple Setup**:

1. **PlayerSpawnPoint** → Marker kung saan lalabas si Lisa
2. **Room09_PlayerSpawner** → Script na mag-move kay Lisa sa spawn point
3. **Room09_FlowController** → Automatic position ni Emily near Lisa

**Emily spawns near Lisa automatically!** ✅

---

## 🎮 In-Game Flow

1. **Room 08**: Lisa breaks mirror
2. **Scene Loads**: Room 09
3. **Player Spawns**: Lisa moves to PlayerSpawnPoint
4. **Emily Spawns**: Emily appears near Lisa (2 units away)
5. **Intro**: Dialogue sequence
6. **Gameplay**: Player can move and solve puzzles

---

## 📍 Example Setup

```
PlayerSpawnPoint:
- Position: (0, -2, 0)

Room09_PlayerSpawner:
- Spawn Point: PlayerSpawnPoint
- Spawn On Start: ✅
- Face Right: ✅

Room09_FlowController:
- Emily Manifestation: Emily_Manifestation
- Emily Distance From Player: 2
```

**Yan lang!** Simple! 🎯✨

---

## 🎨 Visual in Scene View

Pag naka-select ang `Room09_PlayerSpawner`:
- **Cyan sphere** - Spawn point location
- **Blue arrow** - Facing direction

Pag nag-play:
- Lisa → Moves to cyan sphere
- Emily → Appears 2 units away

Good luck! 🎮✨
