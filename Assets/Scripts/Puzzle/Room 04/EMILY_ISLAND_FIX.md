# Emily Island Stuck Fix - Tagalog Guide

## PROBLEMA
Si Emily ay pumupunta sa island at naiistuck doon. Hindi gumagana ang 4 states niya (Patrol, Hunt, Search, Cooldown) kasi hindi siya makalabas.

## ROOT CAUSE
1. **Walang NavMesh Obstacle sa island** - kaya nakakapasok si Emily
2. **NavMesh ay naka-bake sa island area** - kaya iniisip ni Emily na pwede siyang dumaan doon
3. **After intro, hindi properly naka-enable ang Emily AI** - kaya hindi nag-switch ang states

---

## SOLUTION 1: Add NavMesh Obstacle sa Island

### Step 1: Select Island GameObject
1. Sa Hierarchy, hanapin ang **Island** object sa Kitchen scene
2. Kung composite object siya (maraming parts), select ang **parent object**

### Step 2: Add NavMesh Obstacle Component
1. Click **Add Component**
2. Search: `NavMesh Obstacle`
3. Add the component

### Step 3: Configure NavMesh Obstacle
```
NavMesh Obstacle Settings:
├─ Shape: Box
├─ Center: (0, 0, 0)
├─ Size: Adjust to cover entire island
│   ├─ X: Width ng island + 0.5 margin
│   └─ Y: Height ng island + 0.5 margin
├─ Carve: ✓ CHECKED (CRITICAL!)
└─ Move Threshold: 0.1
```

**IMPORTANT**: Ang `Carve` checkbox ay DAPAT naka-check para i-cut out ng NavMesh ang island area!

### Step 4: Visualize NavMesh
1. Open **Window > AI > Navigation**
2. Click **Bake** tab
3. Click **Bake** button
4. Sa Scene view, makikita mo ang blue NavMesh
5. Dapat ang island ay **WALANG blue NavMesh** (carved out)

---

## SOLUTION 2: Fix Emily AI After Intro

Ang current code sa `KitchenRoomController.cs` ay may issue - after ng intro, hindi properly naka-enable si Emily.

### Current Issue in EmilyIntroRoutine:
```csharp
// After intro sequence...
if (emilyAgent != null)
{
    emilyAgent.enabled = true;
    emilyAgent.Warp(emilyInstance.transform.position);
}

emilyInstance.enabled = true;
emilyInstance.SetStateExternal(isPlayerHidden ? EmilyGhost.State.Search : EmilyGhost.State.Hunt);
```

**PROBLEMA**: Ang `SetStateExternal` ay tinatawag BAGO pa fully ready ang NavMeshAgent!

### Fixed Code:
```csharp
// After intro sequence...
if (emilyAgent != null)
{
    emilyAgent.enabled = true;
    emilyAgent.Warp(emilyInstance.transform.position);
    
    // CRITICAL: Wait for agent to be ready
    yield return new WaitForEndOfFrame();
}

// NOW enable Emily AI
emilyInstance.enabled = true;

// CRITICAL: Wait another frame for Emily's OnEnable to complete
yield return new WaitForEndOfFrame();

// NOW set the state
emilyInstance.SetStateExternal(isPlayerHidden ? EmilyGhost.State.Search : EmilyGhost.State.Hunt);

Debug.Log($"[KitchenController] Emily AI fully enabled. State: {(isPlayerHidden ? "Search" : "Hunt")}");
```

---

## SOLUTION 3: Verify NavMesh Bake Settings

### Check NavMesh Settings:
1. Open **Window > AI > Navigation**
2. Click **Bake** tab
3. Verify settings:

```
Agent Settings:
├─ Agent Radius: 0.3 (para hindi masyadong maliit)
├─ Agent Height: 1.0
├─ Max Slope: 45
└─ Step Height: 0.4

Generated Off Mesh Links:
├─ Drop Height: 0 (DISABLE para hindi tumalon si Emily)
└─ Jump Distance: 0 (DISABLE)
```

### Important Layers:
- **Walkable**: Floor, Ground
- **Not Walkable**: Island, Obstacles, Walls

---

## SOLUTION 4: Add Safety Check in EmilyMovement

Kung sakaling makapasok pa rin si Emily sa island, kailangan ng safety check:

### Add to EmilyMovement.cs:
```csharp
void FixedUpdate()
{
    // Existing code...
    
    // SAFETY CHECK: If Emily is not on NavMesh, try to recover
    if (_agent != null && !_agent.isOnNavMesh)
    {
        Debug.LogWarning("[Emily] Not on NavMesh! Attempting recovery...");
        
        // Try to find nearest NavMesh position
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 5.0f, NavMesh.AllAreas))
        {
            // Teleport to nearest valid position
            transform.position = hit.position;
            _agent.Warp(hit.position);
            Debug.Log($"[Emily] Recovered to NavMesh position: {hit.position}");
        }
        else
        {
            Debug.LogError("[Emily] Cannot find NavMesh! Emily is stuck!");
        }
        
        _rb.linearVelocity = Vector2.zero;
        return;
    }
    
    // Rest of existing code...
}
```

---

## TESTING CHECKLIST

### Test 1: NavMesh Obstacle
- [ ] Island has NavMesh Obstacle component
- [ ] Carve is checked
- [ ] NavMesh is baked
- [ ] Island area has NO blue NavMesh (carved out)

### Test 2: Emily Spawn
- [ ] Emily spawns at correct position
- [ ] Emily plays intro sequence
- [ ] Emily pushes Lisa
- [ ] Dialogue plays

### Test 3: Emily AI States
- [ ] After intro, Emily enters Hunt or Search state
- [ ] Emily can patrol around kitchen
- [ ] Emily does NOT go to island
- [ ] Emily can see and chase Lisa
- [ ] Emily can catch Lisa (game over)

### Test 4: Retry
- [ ] Click Retry after game over
- [ ] Scene reloads
- [ ] Emily intro plays again
- [ ] Emily AI works properly
- [ ] All 4 states work (Patrol, Hunt, Search, Cooldown)

---

## DEBUG COMMANDS

### In Unity Console, filter by:
```
[Emily]
[KitchenController]
```

### Expected Log Sequence:
```
[KitchenController] Player entered trigger. Handing off to KitchenRoomController.
[KitchenController] Emily already exists in scene. Moving to spawn point and starting intro.
[KitchenController] Existing Emily reset and moved to: (x, y, z)
[KitchenController] Emily AI fully enabled. State: Hunt
[EMILY] State -> Hunt
```

### If you see this ERROR:
```
[Emily] Not on NavMesh! Attempting recovery...
```
**MEANING**: Emily is stuck somewhere without NavMesh (probably island!)
**FIX**: Add NavMesh Obstacle to that area

---

## QUICK FIX SUMMARY

1. **Add NavMesh Obstacle to Island** (with Carve checked)
2. **Rebake NavMesh** (Window > AI > Navigation > Bake)
3. **Update KitchenRoomController** (add WaitForEndOfFrame before SetStateExternal)
4. **Test in Play Mode**

Yan lang! Dapat hindi na makapasok si Emily sa island at gumagana na lahat ng states niya.
