# Cinematic Chase Trigger - Reference Guide

## 📋 Overview

**CinematicChaseTrigger.cs** is a reference script for creating cinematic chase sequences with Emily. It provides full control over Emily's behavior, speed, and game over conditions.

---

## ✨ Features

### Core Features:
- ✅ **Configurable Emily Speed** - Adjust how fast Emily chases (1-10 units/sec)
- ✅ **Adjustable Catch Distance** - Set how close Emily needs to be for Game Over (0.5-3 units)
- ✅ **Knockback Effect** - Push player back when trigger is activated
- ✅ **Game Over on Contact** - Automatic Game Over when Emily catches player
- ✅ **Dialogue Support** - Show dialogue when chase starts
- ✅ **Audio Support** - Jumpscare sound + looping chase music
- ✅ **One-time Trigger** - Prevents multiple activations
- ✅ **Visual Debugging** - Gizmos show trigger area, spawn point, and catch distance

---

## 🎮 How It Works

### Flow:
1. Player enters trigger zone
2. **Knockback** pushes player back (optional)
3. **Sound effect** plays (jumpscare/scream)
4. **Dialogue** appears (optional)
5. **Emily spawns** at designated position
6. **Emily chases** player at configured speed
7. **Game Over** triggers when Emily gets within catch distance

---

## 🔧 Unity Setup

### Step 1: Create Trigger Zone

1. Create empty GameObject: `CinematicChaseTrigger`
2. Add **BoxCollider2D** or **CircleCollider2D**
3. ✅ Check **"Is Trigger"**
4. Position where you want chase to start

### Step 2: Create Emily Spawn Point

1. Create empty GameObject: `EmilySpawnPoint`
2. Position where Emily should appear
3. This is where Emily teleports when chase starts

### Step 3: Add Script

1. Select `CinematicChaseTrigger` GameObject
2. Add Component → **CinematicChaseTrigger**
3. Configure all settings (see below)

---

## ⚙️ Inspector Settings

### Trigger Settings

**Trigger Once**
- ✅ Enabled: Trigger only once per game session
- ❌ Disabled: Can trigger multiple times
- **Recommended**: ✅ Enabled

---

### Emily Configuration

**Emily GameObject**
- Drag Emily GameObject from Hierarchy
- Must have: `NavMeshAgent` + `EmilyGhost` components
- **IMPORTANT**: Use GameObject from Hierarchy, NOT prefab!

**Emily Spawn Point**
- Drag `EmilySpawnPoint` GameObject
- Where Emily appears when chase starts

**Emily Chase Speed**
- Range: 1.0 - 10.0
- Default: 5.5
- **Slow**: 3.0-4.0 (easier)
- **Normal**: 5.0-6.0 (balanced)
- **Fast**: 7.0-10.0 (intense!)

**Catch Distance**
- Range: 0.5 - 3.0
- Default: 1.0
- How close Emily needs to be for Game Over
- **Smaller** = Harder (must be very close)
- **Larger** = Easier (catches from farther away)

---

### Knockback Settings

**Enable Knockback**
- ✅ Enabled: Player gets pushed back when trigger activates
- ❌ Disabled: No knockback

**Knockback Force**
- Range: 0 - 20
- Default: 10
- How strong the push is

**Knockback Direction**
- Vector2 (X, Y)
- Default: (-1, 0.5) = Back and up
- Examples:
  - `(-1, 0)` = Push left
  - `(1, 0)` = Push right
  - `(0, 1)` = Push up
  - `(-1, 0.5)` = Push back-left and up

---

### Dialogue Settings

**Show Dialogue**
- ✅ Enabled: Show dialogue when chase starts
- ❌ Disabled: No dialogue

**Chase Dialogue**
- Text to show
- Example: "She's coming!"
- Leave empty for no dialogue

**Speaker Name**
- Who's speaking
- Default: "Lisa"

---

### Audio Settings

**Play Sound Effect**
- ✅ Enabled: Play sound when chase starts
- ❌ Disabled: No sound

**Chase Sound Effect**
- Drag AudioClip (jumpscare/scream sound)
- Plays once when chase starts

**Audio Source**
- Optional: Drag AudioSource component
- If empty, uses AudioManager

**Chase Loop Music**
- Drag AudioClip (footsteps/tension music)
- Loops during entire chase
- Stops when Game Over triggers

---

### Timing Settings

**Chase Start Delay**
- Range: 0 - 2 seconds
- Default: 0.2
- Delay before Emily starts chasing
- **0.0** = Instant (no delay)
- **0.2** = Quick (slight pause)
- **0.5** = Dramatic (build tension)

---

### Game Over Settings

**Enable Game Over**
- ✅ Enabled: Game Over when Emily catches player
- ❌ Disabled: No Game Over (for testing)

**Game Over Message**
- Text shown on Game Over screen
- Default: "Emily caught you..."
- Examples:
  - "Emily caught you..."
  - "You couldn't escape..."
  - "She got you..."

---

### Debug Settings

**Debug Mode**
- ✅ Enabled: Show debug messages in Console
- ❌ Disabled: No debug messages
- **Recommended**: ✅ Enabled during development

---

## 🎯 Example Configurations

### Configuration 1: First Chase (Easier)
```
Emily Chase Speed: 3.5
Catch Distance: 1.0
Knockback Force: 8.0
Chase Start Delay: 0.5
Chase Dialogue: "What was that sound?"
```

### Configuration 2: Final Chase (Harder)
```
Emily Chase Speed: 5.5
Catch Distance: 1.0
Knockback Force: 10.0
Chase Start Delay: 0.2
Chase Dialogue: "She's coming!"
```

### Configuration 3: Intense Chase (Very Hard)
```
Emily Chase Speed: 7.0
Catch Distance: 1.5
Knockback Force: 12.0
Chase Start Delay: 0.0
Chase Dialogue: "RUN!"
```

---

## 🔍 Visual Debugging (Gizmos)

When you select the trigger in Scene view, you'll see:

**Red Semi-Transparent Box**
- Trigger area (where player must enter)

**Red Wire Sphere**
- Emily spawn point (where Emily appears)

**Red Line**
- Connection between trigger and spawn point

**Red Wire Circle**
- Catch distance (Game Over radius around Emily)

**Yellow Arrow**
- Knockback direction (which way player gets pushed)

---

## 🎬 How to Use in Different Rooms

### Room 05 (Dining Room) - Example

**First Chase Trigger:**
```
Position: Between calendar and exit
Emily Speed: 3.5
Catch Distance: 1.0
Dialogue: "What was that sound?"
```

**Final Chase Trigger:**
```
Position: Near exit door
Emily Speed: 5.5
Catch Distance: 1.0
Dialogue: "She's coming!"
Knockback: Enabled
```

### Room 04 (Kitchen) - Example

**Chase Trigger:**
```
Position: Near island counter
Emily Speed: 4.5
Catch Distance: 1.2
Dialogue: "I need to hide!"
```

---

## 🐛 Troubleshooting

### Emily doesn't spawn
- ✅ Check Emily GameObject is assigned
- ✅ Check Emily has NavMeshAgent component
- ✅ Check Emily Spawn Point is assigned
- ✅ Check trigger collider has "Is Trigger" enabled

### Game Over doesn't trigger
- ✅ Check "Enable Game Over" is enabled
- ✅ Check Emily has EmilyGhost component
- ✅ Check GameOverManager exists in scene
- ✅ Check Catch Distance is not too small

### Knockback doesn't work
- ✅ Check "Enable Knockback" is enabled
- ✅ Check player has Rigidbody2D component
- ✅ Check Knockback Force is not 0

### Emily moves too slow/fast
- ✅ Adjust "Emily Chase Speed" slider
- ✅ Check NavMeshAgent speed is not overridden elsewhere
- ✅ Test different values (3.5 = slow, 5.5 = normal, 7.0 = fast)

### Dialogue doesn't show
- ✅ Check "Show Dialogue" is enabled
- ✅ Check "Chase Dialogue" is not empty
- ✅ Check DialogueSystemV2 exists in scene

### Sound doesn't play
- ✅ Check "Play Sound Effect" is enabled
- ✅ Check AudioClip is assigned
- ✅ Check AudioManager exists in scene
- ✅ Check audio volume is not 0

---

## 📝 Code Examples

### Manually Start Chase (from another script)
```csharp
CinematicChaseTrigger trigger = FindFirstObjectByType<CinematicChaseTrigger>();
if (trigger != null)
{
    trigger.ManuallyStartChase();
}
```

### Stop Chase (from another script)
```csharp
CinematicChaseTrigger trigger = FindFirstObjectByType<CinematicChaseTrigger>();
if (trigger != null)
{
    trigger.StopChase();
}
```

---

## 🎯 Best Practices

### Speed Balancing:
- **First chase**: 3.0-4.0 (give player time to learn)
- **Mid-game chase**: 4.5-5.5 (balanced challenge)
- **Final chase**: 6.0-7.5 (intense climax)

### Catch Distance:
- **1.0** = Standard (must be very close)
- **1.5** = Forgiving (easier to catch)
- **0.8** = Strict (harder to catch)

### Knockback:
- Use knockback to create distance
- Direction should push player AWAY from Emily
- Force 8-12 is usually good

### Dialogue:
- Keep it SHORT (1-2 sentences)
- Create urgency ("She's coming!", "Run!")
- Match intensity to chase difficulty

### Audio:
- Jumpscare sound: Short, loud, sudden
- Chase music: Looping, tense, rhythmic
- Stop music on Game Over for impact

---

## 🎮 Testing Checklist

- [ ] Trigger activates when player enters
- [ ] Knockback pushes player in correct direction
- [ ] Sound effect plays
- [ ] Dialogue appears
- [ ] Emily spawns at correct position
- [ ] Emily chases player
- [ ] Emily speed feels right
- [ ] Game Over triggers when caught
- [ ] Game Over message is correct
- [ ] Chase music loops correctly
- [ ] Trigger only activates once

---

## 💡 Tips

### For Easier Chase:
- Lower Emily speed (3.0-4.0)
- Smaller catch distance (0.8-1.0)
- Longer start delay (0.5-1.0)
- Stronger knockback (12-15)

### For Harder Chase:
- Higher Emily speed (6.0-8.0)
- Larger catch distance (1.5-2.0)
- Shorter start delay (0.0-0.2)
- Weaker knockback (5-8)

### For Cinematic Effect:
- Use dialogue to build tension
- Add camera shake (separate script)
- Use dramatic sound effects
- Time knockback with music

---

## 📚 Related Scripts

- **EmilyGhost.cs** - Emily AI behavior
- **GameOverManager.cs** - Game Over screen
- **DialogueSystemV2.cs** - Dialogue system
- **AudioManager.cs** - Audio management
- **TriggerFinalChase.cs** - Room 05 specific implementation

---

## ✅ Summary

**CinematicChaseTrigger** is a flexible, configurable script for creating chase sequences with Emily. It handles:
- ✅ Emily spawning and movement
- ✅ Player knockback
- ✅ Dialogue and audio
- ✅ Game Over on contact
- ✅ Visual debugging

**Perfect for**: Creating intense, cinematic chase moments throughout your game!

**Customize**: Adjust speed, distance, and timing to match your game's difficulty curve!

---

## 🎯 Quick Start

1. Create trigger zone (BoxCollider2D, Is Trigger = true)
2. Create spawn point (empty GameObject)
3. Add CinematicChaseTrigger script
4. Assign Emily GameObject
5. Assign spawn point
6. Set Emily speed (5.5 recommended)
7. Set catch distance (1.0 recommended)
8. Enable knockback
9. Add dialogue and sound
10. Test!

**You're ready to create epic chase sequences!** 🎮✨
