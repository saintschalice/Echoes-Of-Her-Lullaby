# Room05_CinematicChaseTrigger - Configuration Guide

## 📋 Overview

**Room05_CinematicChaseTrigger.cs** - Updated with configurable Emily stats and cinematic settings!

---

## ⚙️ Inspector Settings

### Setup
**Spawn Point**
- Transform where Emily will appear
- Required!

---

### AI Configuration

**Spawn State**
- Dropdown: Patrol, Investigate, Hunt, Search, Cooldown
- Default: **Hunt**
- Recommended: **Hunt** (para humabol agad)

**Spawn Facing**
- Vector2 (X, Y)
- Default: (1, 0) = Right
- Examples:
  - (1, 0) = Right
  - (-1, 0) = Left
  - (0, 1) = Up
  - (0, -1) = Down

---

### Emily Stats ⭐ NEW!

**Emily Chase Speed**
- Range: 1.0 - 10.0
- Default: 3.5
- **Recommended Values:**
  - 3.0-3.5 = Mabagal (easier, first chase)
  - 4.5-5.5 = Normal (balanced)
  - 6.0-7.0 = Mabilis (intense)

**Catch Distance**
- Range: 0.5 - 3.0
- Default: 1.0
- Gaano kalapit dapat si Emily para Game Over
- **Note:** Ready for future implementation

---

### Cinematic Settings

**Push Force**
- Range: 0 - 30
- Default: 15
- Gaano kalakas ang tulak kay Lisa

**Shove Friction**
- Range: 0 - 20
- Default: 10
- Gaano kabilis tumigil si Lisa after push

**Resume Delay**
- Range: 0 - 2 seconds
- Default: 0.3
- Delay bago magsimula si Emily humabol

---

### Audio

**Jumpscare Clip**
- AudioClip for jumpscare sound
- Plays when Emily spawns

---

### Narrative

**Emily Shout**
- Text area
- Default: "YOU NEED TO GET OUT!"
- Emily's dialogue when she spawns

**Lisa Panic**
- Text area
- Default: "Holy- I need to hide!"
- Lisa's reaction

---

### Persistence

**Trigger ID**
- String
- Default: "Room05_EmilySpawn_Intro"
- Ensures dialogue doesn't repeat on retry

---

### Debug ⭐ NEW!

**Debug Mode**
- Checkbox
- Default: ✅ Enabled
- Shows debug messages in Console

---

## 🎯 Recommended Configurations

### First Chase (Easier)
```
Emily Chase Speed: 3.0
Catch Distance: 1.0
Push Force: 15
Shove Friction: 10
Resume Delay: 0.5
Spawn State: Hunt
```

### Normal Chase (Balanced)
```
Emily Chase Speed: 3.5
Catch Distance: 1.0
Push Force: 15
Shove Friction: 10
Resume Delay: 0.3
Spawn State: Hunt
```

### Hard Chase (Intense)
```
Emily Chase Speed: 5.0
Catch Distance: 1.2
Push Force: 20
Shove Friction: 8
Resume Delay: 0.2
Spawn State: Hunt
```

---

## 🔍 Visual Debugging ⭐ NEW!

When you select the trigger in Scene view:

**Orange Semi-Transparent Box**
- Trigger area (kung saan dapat pumasok ang player)

**Red Wire Sphere**
- Emily spawn point (kung saan lalabas si Emily)

**Red Line**
- Connection between trigger and spawn point

**Yellow Arrow**
- Push direction (saan itutulak si Lisa)

**Red Wire Circle**
- Catch distance preview (for reference)

---

## 🎮 How It Works

### Flow:
1. Player enters trigger zone
2. Emily spawns at spawn point (frozen)
3. Jumpscare sound plays
4. Lisa gets pushed back
5. Dialogue sequence (Emily shout → Lisa panic)
6. Resume delay
7. **Emily stats applied** (speed, etc.)
8. Player controls restored
9. Emily AI enabled → Starts hunting!

---

## 💡 Tips

### Para sa Mas Madaling Chase:
- Babaan ang Emily speed (3.0-3.5)
- Pahabain ang resume delay (0.5-1.0)
- Palakasin ang push force (18-20)

### Para sa Mas Mahirap na Chase:
- Itaas ang Emily speed (5.0-6.0)
- Paikliin ang resume delay (0.2-0.3)
- Pahinain ang push force (10-12)

### Para sa Cinematic Effect:
- Use dramatic dialogue
- Strong push force (15-20)
- Short resume delay (0.2-0.3)
- Fast Emily speed (4.5-5.5)

---

## 🐛 Troubleshooting

### Hindi lumalabas si Emily
**Solution:**
- Check kung naka-assign ang Spawn Point
- Check kung may Emily reference sa Room Controller
- Check kung naka-check ang "Is Trigger" sa collider

### Mabagal/Mabilis si Emily
**Solution:**
- I-adjust ang "Emily Chase Speed" slider
- Try: 3.0 (slow), 3.5 (normal), 5.0 (fast)

### Walang Push Effect
**Solution:**
- Check kung hindi 0 ang "Push Force"
- Check kung may Rigidbody2D ang player
- Try higher values (15-20)

### Dialogue nag-repeat
**Solution:**
- Check kung unique ang "Trigger ID"
- Check kung gumagana ang SaveSystem

---

## 📝 What's New

### Added:
- ✅ **Emily Chase Speed** slider (1-10)
- ✅ **Catch Distance** slider (0.5-3)
- ✅ **Push Force** slider (0-30)
- ✅ **Shove Friction** slider (0-20)
- ✅ **Resume Delay** slider (0-2)
- ✅ **Debug Mode** checkbox
- ✅ **Visual debugging** (Gizmos)
- ✅ **ApplyEmilyStats()** method
- ✅ Debug logging throughout

### Improved:
- ✅ All settings now have sliders
- ✅ Better tooltips
- ✅ Visual feedback in Scene view
- ✅ Debug messages for testing
- ✅ Applies stats to Emily AI

---

## ✅ Quick Setup

1. Select `Room05_CinematicChaseTrigger` GameObject
2. Assign **Spawn Point** (where Emily appears)
3. Set **Emily Chase Speed**: 3.5
4. Set **Catch Distance**: 1.0
5. Set **Push Force**: 15
6. Set **Shove Friction**: 10
7. Set **Resume Delay**: 0.3
8. Set **Spawn State**: Hunt
9. Assign **Jumpscare Clip** (audio)
10. Enable **Debug Mode**: ✅
11. Test!

---

## 🎯 Summary

**Room05_CinematicChaseTrigger** now has:
- ✅ Configurable Emily speed (1-10)
- ✅ Adjustable push mechanics (force + friction)
- ✅ Timing control (resume delay)
- ✅ Visual debugging (Gizmos)
- ✅ Debug logging
- ✅ Easy to modify in Inspector

**Perfect for balancing the first chase difficulty!** 🎮✨

---

## 🔧 Inspector Preview

```
Setup:
└─ Spawn Point: EmilySpawnPoint

AI Configuration:
├─ Spawn State: Hunt
└─ Spawn Facing: (1, 0)

Emily Stats: ⭐ NEW!
├─ Emily Chase Speed: 3.5 (slider 1-10)
└─ Catch Distance: 1.0 (slider 0.5-3)

Cinematic Settings:
├─ Push Force: 15 (slider 0-30)
├─ Shove Friction: 10 (slider 0-20)
└─ Resume Delay: 0.3 (slider 0-2)

Audio:
└─ Jumpscare Clip: (AudioClip)

Narrative:
├─ Emily Shout: "YOU NEED TO GET OUT!"
└─ Lisa Panic: "Holy- I need to hide!"

Persistence:
└─ Trigger ID: "Room05_EmilySpawn_Intro"

Debug: ⭐ NEW!
└─ Debug Mode: ☑
```

**Ready to use!** 💪✨
