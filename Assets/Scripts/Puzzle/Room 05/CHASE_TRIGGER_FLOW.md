# Dining Room - Chase Trigger Flow (Updated)

## 🎯 Correct Flow

### 1. Player Enters Room
- Calendar is visible
- No chase yet

### 2. Player Interacts with Calendar
- Calendar UI opens
- Player reads calendar
- Calendar closes
- **No chase yet!** Just marks calendar as seen

### 3. Player Walks to Trigger Zone
- Player enters `DiningRoomChaseTrigger` collider
- **Trigger activates** (only if calendar was seen)
- **Intro dialogue** (R05_ANGRY_1)
- **Wait for dialogue to finish**
- **Jumpscare + Knockback**
- **Emily spawns and chases**

---

## 🔧 What Changed

### Before:
```
Calendar closed → IMMEDIATE chase (wrong!)
```

### After:
```
Calendar closed → Player moves → Trigger zone → Chase starts (correct!)
```

---

## 🎮 Unity Setup

### Required GameObjects:

1. **Calendar** (interactable)
   - Opens calendar UI
   - Marks calendar as seen

2. **DiningRoomChaseTrigger** (trigger collider)
   - Position: Where you want chase to start
   - Component: `DiningRoomChaseTrigger`
   - Collider: BoxCollider2D (Is Trigger ✅)

3. **EmilyAngrySpawnPoint** (empty GameObject)
   - Position: Where Emily spawns when chase starts

---

## 📍 Recommended Trigger Position

### Trigger Zone:
- **After calendar** - So player has time to read
- **Before exit** - So chase starts before leaving
- **Center of room** - Good middle ground

### Example:
```
[Entry] → [Calendar] → [TRIGGER ZONE] → [Exit]
                            ↑
                    Chase starts here!
```

---

## 🎬 Sequence Breakdown

### Step 1: Calendar Interaction
```csharp
OnCalendarInteract()
  → Opens calendar UI
  → Marks isCalendarSeen = true
  → Pauses game

CloseCalendarUI()
  → Closes calendar UI
  → Resumes game
  → NO CHASE (just continue playing)
```

### Step 2: Trigger Zone
```csharp
OnTriggerEnter2D()
  → Check if calendar seen (if not, skip)
  → Check if not already hunting
  → Start EmilyGetsAngrySequence()
```

### Step 3: Chase Sequence
```csharp
EmilyGetsAngrySequence()
  → Intro dialogue (R05_ANGRY_1)
  → Wait for dialogue to finish
  → Jumpscare sound
  → Knockback player
  → Dialogue (R05_ANGRY_2)
  → Spawn Emily
  → Start hunting
```

---

## 🐛 Troubleshooting

### Issue 1: Chase starts immediately after calendar
**Cause**: Trigger zone is too close to calendar
**Fix**: Move trigger zone further away

### Issue 2: Chase doesn't start at all
**Cause**: Trigger zone not set up correctly
**Fix**: 
- Check collider is trigger (Is Trigger ✅)
- Check DiningRoomChaseTrigger component attached
- Check trigger zone is in player's path

### Issue 3: Player still being force-moved
**Cause**: Knockback happening during dialogue
**Fix**: Already fixed - dialogue plays FIRST, then knockback

---

## 📝 Testing Checklist

- [ ] Player enters room (no chase)
- [ ] Player interacts with calendar
- [ ] Calendar opens and closes (no chase)
- [ ] Player walks to trigger zone
- [ ] Intro dialogue plays (R05_ANGRY_1)
- [ ] Dialogue finishes
- [ ] Jumpscare + knockback
- [ ] Emily spawns and chases

---

## 💡 Design Notes

### Why Trigger Zone?

**Better player experience**:
- Player has time to read calendar
- Player can explore a bit
- Chase starts at controlled point
- More dramatic (player walks into danger)

### Trigger Position:

**Too close to calendar**: Chase starts immediately (bad)
**Too far from calendar**: Player might forget (confusing)
**Just right**: Player has time to process, then chase starts

---

## 🎯 Summary

**Flow**:
1. Calendar interaction → Marks as seen
2. Player walks to trigger zone
3. Trigger → Intro dialogue → Knockback → Chase

**Key Points**:
- Calendar doesn't start chase
- Trigger zone starts chase
- Intro dialogue BEFORE knockback
- Better player experience!

Yan lang! 🎯✨
