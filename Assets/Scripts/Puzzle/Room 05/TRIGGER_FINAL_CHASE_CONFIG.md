# TriggerFinalChase - Configuration Guide

## 📋 Overview

**TriggerFinalChase.cs** - Updated with configurable Emily stats for easy balancing!

---

## ⚙️ Inspector Settings

### Emily Configuration

**Emily Chase Speed**
- Range: 1.0 - 10.0
- Default: 5.5
- **Recommended Values:**
  - 3.5 = Mabagal (easier)
  - 5.5 = Normal (balanced)
  - 7.0 = Mabilis (intense!)

**Catch Distance**
- Range: 0.5 - 3.0
- Default: 1.0
- Gaano kalapit dapat si Emily para Game Over
- **Note:** Hindi pa implemented sa Room05 controller, pero ready na for future use

---

### Knockback Settings

**Knockback Force**
- Range: 0 - 20
- Default: 10
- Gaano kalakas ang tulak

**Knockback Direction**
- Vector2 (X, Y)
- Default: (-1, 0.5) = Pabalik at pataas
- Examples:
  - `(-1, 0)` = Tulak pakaliwa
  - `(1, 0)` = Tulak pakanan
  - `(-1, 0.5)` = Tulak pabalik-kaliwa at pataas

---

### Timing Settings

**Chase Start Delay**
- Range: 0 - 2 seconds
- Default: 0.2
- Delay bago magsimula si Emily humabol
- **0.0** = Instant
- **0.2** = Mabilis (recommended)
- **0.5** = Dramatic

---

### Audio Settings

**Play Jumpscare Sound**
- ✅ Enabled: May tunog pag nag-trigger
- ❌ Disabled: Walang tunog
- **Note:** Not yet implemented, ready for future use

**Jumpscare Clip**
- AudioClip for jumpscare sound
- **Note:** Not yet implemented, ready for future use

---

### Dialogue Settings

**Show Dialogue**
- ✅ Enabled: May dialogue pag nagsimula chase
- ❌ Disabled: Walang dialogue
- **Note:** Not yet implemented, ready for future use

**Chase Dialogue**
- Custom dialogue text
- Leave empty for default
- **Note:** Not yet implemented, ready for future use

---

### Debug Settings

**Debug Mode**
- ✅ Enabled: May debug messages sa Console
- ❌ Disabled: Walang debug messages
- **Recommended:** ✅ Enabled during development

---

## 🎯 Recommended Configurations

### Easy Mode
```
Emily Chase Speed: 3.5
Catch Distance: 1.0
Knockback Force: 12
Chase Start Delay: 0.5
```

### Normal Mode (Current)
```
Emily Chase Speed: 5.5
Catch Distance: 1.0
Knockback Force: 10
Chase Start Delay: 0.2
```

### Hard Mode
```
Emily Chase Speed: 7.0
Catch Distance: 1.5
Knockback Force: 8
Chase Start Delay: 0.0
```

---

## 🔍 Visual Debugging

When you select the trigger in Scene view:

**Orange Semi-Transparent Box**
- Trigger area (kung saan dapat pumasok ang player)

**Yellow Arrow**
- Knockback direction (saan itutulak ang player)

**Red Wire Circle**
- Catch distance preview (for reference)

---

## 🎮 How It Works

### Flow:
1. Player completes dining room puzzle
2. Player walks to trigger zone
3. **Knockback** pushes player back
4. **Emily stats applied** to Room Controller
5. **Emily spawns** and starts chasing
6. Emily chases at configured speed

---

## 💡 Tips

### Para sa Mas Madaling Chase:
- Babaan ang Emily speed (3.0-4.0)
- Pahabain ang start delay (0.5-1.0)
- Palakasin ang knockback (12-15)

### Para sa Mas Mahirap na Chase:
- Itaas ang Emily speed (6.0-8.0)
- Paikliin ang start delay (0.0-0.2)
- Pahinain ang knockback (5-8)

### Para sa Balanced Experience:
- Emily speed: 5.5
- Start delay: 0.2
- Knockback: 10

---

## 🐛 Troubleshooting

### Mabagal/Mabilis si Emily
**Solution:**
- I-adjust ang "Emily Chase Speed" slider
- Try: 3.5 (slow), 5.5 (normal), 7.0 (fast)

### Walang Knockback
**Solution:**
- Check kung hindi 0 ang "Knockback Force"
- Check kung may Rigidbody2D ang player

### Hindi nag-trigger
**Solution:**
- Check kung tapos na ang puzzle
- Check kung naka-check ang "Is Trigger" sa collider
- Check kung may "Player" tag ang player

---

## ✅ Quick Setup

1. Select `TriggerFinalChase` GameObject
2. Set Emily Chase Speed: **5.5**
3. Set Catch Distance: **1.0**
4. Set Knockback Force: **10**
5. Set Knockback Direction: **(-1, 0.5)**
6. Set Chase Start Delay: **0.2**
7. Enable Debug Mode: **✅**
8. Test!

---

## 📝 Notes

### Current Implementation:
- ✅ Emily speed configuration
- ✅ Knockback configuration
- ✅ Timing configuration
- ✅ Visual debugging (Gizmos)
- ✅ Debug logging

### Future Features (Ready but not implemented):
- ⏳ Catch distance (for Game Over)
- ⏳ Jumpscare sound
- ⏳ Custom dialogue
- ⏳ Audio configuration

---

## 🎯 Summary

**TriggerFinalChase** now has:
- ✅ Configurable Emily speed (1-10)
- ✅ Adjustable knockback (force + direction)
- ✅ Timing control (chase start delay)
- ✅ Visual debugging (Gizmos)
- ✅ Easy to modify in Inspector

**Perfect for balancing the final chase difficulty!** 🎮✨

---

## 🔧 Inspector Preview

```
Emily Configuration:
├─ Emily Chase Speed: 5.5 (slider 1-10)
└─ Catch Distance: 1.0 (slider 0.5-3)

Knockback Settings:
├─ Knockback Force: 10 (slider 0-20)
└─ Knockback Direction: (-1, 0.5)

Timing Settings:
└─ Chase Start Delay: 0.2 (slider 0-2)

Audio Settings:
├─ Play Jumpscare Sound: ☐
└─ Jumpscare Clip: None

Dialogue Settings:
├─ Show Dialogue: ☐
└─ Chase Dialogue: (empty)

Debug:
└─ Debug Mode: ☑
```

**Ready to use!** 💪✨
