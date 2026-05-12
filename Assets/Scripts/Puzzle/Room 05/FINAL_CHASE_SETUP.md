# Dining Room - Final Chase Setup

## 🎯 Final Chase Behavior

### Trigger: `Trigger_FinalEmilyChase`

**When player touches trigger**:
1. **Knockback** - Player pushed back (sudden impact)
2. **Jumpscare sound** - Quick scream
3. **Emily spawns FAST** - Higher speed than first chase
4. **Pure hunt** - No long dialogue, just chase!

---

## 🔧 Unity Setup

### Trigger_FinalEmilyChase GameObject:

**Components**:
- BoxCollider2D (Is Trigger ✅)
- `TriggerFinalChase` script

**Inspector Settings**:
- **Knockback Force**: `10` (how strong the push)
- **Knockback Direction**: `(-1, 0.5)` (back and up)

**Position**: Near exit door (where player tries to leave)

---

## ⚡ Speed Comparison

### First Chase:
- Speed: `3.5` (initialChaseSpeed)
- Player can hide under table
- More forgiving

### Final Chase:
- Speed: `5.5` (finalChaseSpeed)
- **Much faster!**
- Player must escape quickly
- More intense!

---

## 🎬 Sequence Breakdown

### Step 1: Player Touches Trigger
```csharp
OnTriggerEnter2D()
  → Check if puzzle complete
  → Apply knockback to player
  → Start FinalChaseSequence()
  → Disable trigger
```

### Step 2: Final Chase Starts
```csharp
FinalChaseSequence()
  → Jumpscare sound (0.2s delay - very quick!)
  → Spawn Emily at final spawn point
  → Set speed to finalChaseSpeed (5.5)
  → Start hunting
  → Optional quick dialogue
```

---

## 📍 Spawn Points

### First Chase:
- **emilyAngrySpawnPoint**: Where Emily spawns for first chase
- Speed: 3.5

### Final Chase:
- **emilyFinalChaseSpawnPoint**: Where Emily spawns for final chase
- Speed: 5.5
- Should be closer to player for more intensity!

---

## 🎮 Recommended Setup

### Trigger Position:
```
[Table] → [Player escapes] → [TRIGGER] → [Exit Door]
                                  ↑
                          Final chase starts here!
```

### Spawn Point Position:
```
[EMILY SPAWN] → Very close to trigger
                ↓
            [Player] ← Already knocked back
                ↓
            [Exit Door] ← Must reach here!
```

---

## 💡 Design Notes

### Why Knockback?

**Creates urgency**:
- Player feels the impact
- Immediate danger
- No time to think
- Must run NOW!

### Why Faster?

**Escalation**:
- First chase: Player learns mechanics
- Final chase: Player must execute perfectly
- Increased difficulty
- More satisfying escape

### Why No Long Dialogue?

**Pure action**:
- No time for talking
- Just run!
- More intense
- Better pacing

---

## 🎯 Inspector Values

### TriggerFinalChase:

**Knockback Settings**:
- Knockback Force: `10` (adjust for stronger/weaker push)
- Knockback Direction: `(-1, 0.5)` (back and up)
  - X: `-1` = Push left (away from exit)
  - Y: `0.5` = Push up slightly

### Room05_DiningRoomController:

**Hunting System**:
- Initial Chase Speed: `3.5` (first chase)
- Final Chase Speed: `5.5` (final chase - faster!)

**Spawn Points**:
- Emily Angry Spawn Point: First chase spawn
- Emily Final Chase Spawn Point: Final chase spawn (closer!)

---

## 📝 Testing Checklist

- [ ] Complete puzzle (hide under table)
- [ ] Emily disappears
- [ ] Walk to exit trigger
- [ ] **Knockback happens** (player pushed back)
- [ ] **Jumpscare sound** plays
- [ ] **Emily spawns** at final spawn point
- [ ] **Emily is FAST** (5.5 speed)
- [ ] Player must run to exit
- [ ] Escape or get caught

---

## 🐛 Troubleshooting

### Issue 1: No knockback
**Cause**: Player doesn't have Rigidbody2D
**Fix**: Make sure Player has Rigidbody2D component

### Issue 2: Knockback too weak/strong
**Cause**: Wrong force value
**Fix**: Adjust `Knockback Force` in Inspector (try 5-15)

### Issue 3: Emily not fast enough
**Cause**: finalChaseSpeed too low
**Fix**: Increase `Final Chase Speed` (try 6-7)

### Issue 4: Emily spawns too far
**Cause**: Spawn point too far from player
**Fix**: Move `Emily Final Chase Spawn Point` closer to trigger

---

## 🎯 Summary

**Final Chase Features**:
- ✅ Knockback on trigger (sudden impact)
- ✅ Faster Emily (5.5 vs 3.5)
- ✅ Pure hunt (no long dialogue)
- ✅ More intense (escalation)

**Setup**:
1. Position trigger near exit
2. Set knockback force (10)
3. Set final chase speed (5.5)
4. Position spawn point close to trigger
5. Test!

**Result**: Intense final chase! 🎯✨
