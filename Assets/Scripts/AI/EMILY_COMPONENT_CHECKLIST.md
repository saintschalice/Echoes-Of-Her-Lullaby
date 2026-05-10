# Emily Component Checklist - Tagalog Guide

## PROBLEMA
Hindi gumagalaw si Emily kasi may components na naka-disable sa Inspector.

---

## REQUIRED COMPONENTS (All must be ENABLED!)

### Emily GameObject Inspector:

```
Emily GameObject
├─ Transform ✓
├─ Rigidbody2D ✓ (enabled)
├─ Collider2D ✓ (enabled)
├─ NavMeshAgent ✓ (enabled)
├─ EmilyGhost ✓ (enabled) ← Main AI script
├─ EmilyMovement ✓ (enabled) ← CRITICAL! Must be ON
├─ EmilyPerception ✓ (enabled) ← Vision/hearing
├─ EmilyAudio ✓ (enabled) ← Sound effects
├─ EmilyAnimator ✓ (enabled) ← Animation control
└─ AudioSource ✓ (enabled)
```

**CRITICAL**: Lahat ng components ay dapat naka-CHECK (enabled)!

---

## QUICK FIX

### Step 1: Select Emily GameObject
1. Open Kitchen scene
2. Sa Hierarchy, hanapin ang **Emily** GameObject
   - Or search: "Emily"

### Step 2: Check All Components
1. Sa **Inspector**, scroll down
2. Check LAHAT ng components:
   - [ ] **EmilyGhost** - ✓ enabled
   - [ ] **EmilyMovement** - ✓ enabled ← MOST IMPORTANT!
   - [ ] **EmilyPerception** - ✓ enabled
   - [ ] **EmilyAudio** - ✓ enabled
   - [ ] **EmilyAnimator** - ✓ enabled
   - [ ] **NavMeshAgent** - ✓ enabled
   - [ ] **Rigidbody2D** - ✓ enabled

### Step 3: Enable Disabled Components
If may naka-uncheck:
1. **Click** ang checkbox sa left ng component name
2. Should turn to ✓ (checked)
3. Component is now enabled

### Step 4: Save and Test
1. **Save** scene (Ctrl+S)
2. **Play** the scene
3. Emily should now move/hunt

---

## COMPONENT FUNCTIONS

### EmilyGhost (Main AI)
- Controls state machine (Patrol, Hunt, Search, Cooldown)
- Handles catch logic
- Manages timers

**If disabled**: Emily won't think or change states

### EmilyMovement ← CRITICAL!
- Handles all movement
- NavMesh pathfinding
- 4-directional snapping
- Velocity control

**If disabled**: Emily won't move AT ALL! (Most common issue)

### EmilyPerception
- Vision cone
- Hearing noise
- Player detection

**If disabled**: Emily can't see or hear Lisa

### EmilyAudio
- Footsteps
- Hunt music
- Search sounds
- Catch sound

**If disabled**: Emily is silent

### EmilyAnimator
- Walking animation
- Idle animation
- Hit animation

**If disabled**: Emily won't animate

---

## COMMON ISSUES

### Issue 1: Emily doesn't move
**CAUSE**: EmilyMovement disabled
**FIX**: Enable EmilyMovement component

### Issue 2: Emily doesn't chase Lisa
**CAUSE**: EmilyPerception disabled
**FIX**: Enable EmilyPerception component

### Issue 3: Emily doesn't change states
**CAUSE**: EmilyGhost disabled
**FIX**: Enable EmilyGhost component

### Issue 4: Emily is invisible
**CAUSE**: SpriteRenderer disabled or missing
**FIX**: Enable SpriteRenderer component

### Issue 5: Emily falls through floor
**CAUSE**: NavMeshAgent disabled or no NavMesh
**FIX**: 
- Enable NavMeshAgent
- Bake NavMesh (Window > AI > Navigation > Bake)

---

## NAVMESH AGENT SETTINGS

### Required Settings:
```
NavMeshAgent:
├─ Agent Type: Humanoid (or default)
├─ Base Offset: 0
├─ Speed: 0.5 (set by scripts)
├─ Angular Speed: 0
├─ Acceleration: 8
├─ Stopping Distance: 0.2
├─ Auto Braking: ✓ checked
├─ Radius: 0.3
├─ Height: 1
├─ Obstacle Avoidance: None
├─ Priority: 50
└─ Auto Repath: ✓ checked
```

**IMPORTANT**: 
- Update Rotation: ✗ UNCHECKED (2D game)
- Update Up Axis: ✗ UNCHECKED (2D game)
- Update Position: ✗ UNCHECKED (Rigidbody2D handles this)

---

## RIGIDBODY2D SETTINGS

### Required Settings:
```
Rigidbody2D:
├─ Body Type: Kinematic
├─ Material: None
├─ Simulated: ✓ checked
├─ Use Auto Mass: ✗ unchecked
├─ Mass: 1
├─ Linear Drag: 0
├─ Angular Drag: 0.05
├─ Gravity Scale: 0 (2D top-down)
├─ Collision Detection: Discrete
├─ Sleeping Mode: Never Sleep
├─ Interpolate: None
└─ Constraints:
    └─ Freeze Rotation Z: ✓ checked
```

---

## TESTING CHECKLIST

### Test 1: Emily Spawns
- [ ] Emily appears in scene
- [ ] Emily is visible
- [ ] Emily is on NavMesh (blue area)

### Test 2: Emily Moves
- [ ] Emily wanders (Patrol state)
- [ ] Emily walks smoothly
- [ ] Emily doesn't get stuck
- [ ] Emily avoids obstacles

### Test 3: Emily Detects Lisa
- [ ] Emily sees Lisa when in view
- [ ] Emily enters Hunt state
- [ ] Emily chases Lisa
- [ ] Emily catches Lisa (game over)

### Test 4: Emily States Work
- [ ] Patrol → Hunt (when sees Lisa)
- [ ] Hunt → Search (when loses sight)
- [ ] Search → Cooldown (after timer)
- [ ] Cooldown → Patrol (after timer)

---

## DEBUG COMMANDS

### In Play Mode:
1. **Select Emily** in Hierarchy
2. **Watch Inspector** - see component values change
3. **Check Console** for Emily logs:
   ```
   [EMILY] State -> Hunt
   [EMILY] State -> Search
   [EMILY] State -> Patrol
   ```

### Console Filters:
```
[EMILY]
[EmilyMovement]
[KitchenController]
```

---

## PREFAB vs SCENE INSTANCE

### If Emily is a Prefab:
1. **Select Emily** in scene
2. **Check** if it says "Prefab" at top of Inspector
3. **If yes**: 
   - Enable components
   - Click **Overrides** dropdown
   - Click **Apply All** to save to prefab
4. **If no**: Just enable components and save scene

---

## QUICK ENABLE ALL SCRIPT

If you want to enable all components via script:

```csharp
// Add this to KitchenRoomController or create a debug script
[ContextMenu("Enable All Emily Components")]
void EnableAllEmilyComponents()
{
    EmilyGhost emily = FindFirstObjectByType<EmilyGhost>();
    if (emily != null)
    {
        emily.enabled = true;
        emily.GetComponent<EmilyMovement>().enabled = true;
        emily.GetComponent<EmilyPerception>().enabled = true;
        emily.GetComponent<EmilyAudio>().enabled = true;
        emily.GetComponent<EmilyAnimator>().enabled = true;
        emily.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
        emily.GetComponent<Rigidbody2D>().simulated = true;
        
        Debug.Log("[Debug] All Emily components enabled!");
    }
}
```

---

## SUMMARY

**Problem**: Emily doesn't move
**Cause**: EmilyMovement component disabled
**Fix**: Enable EmilyMovement in Inspector

**Checklist**:
1. Select Emily GameObject
2. Check all components are enabled (✓)
3. Especially EmilyMovement!
4. Save scene
5. Test

**Result**: Emily moves and hunts properly! 🎮

---

## VISUAL GUIDE

### Enabled Component (Correct):
```
✓ EmilyMovement
  ├─ Use Four Directions: ✓
  ├─ Direction Lock Time: 0.2
  └─ Stop Distance: 0.2
```

### Disabled Component (Wrong):
```
☐ EmilyMovement (grayed out)
  ├─ Use Four Directions: ✓
  ├─ Direction Lock Time: 0.2
  └─ Stop Distance: 0.2
```

**If you see ☐ (unchecked)**: Click it to enable!

Yan lang! Just enable the EmilyMovement component. 😊
