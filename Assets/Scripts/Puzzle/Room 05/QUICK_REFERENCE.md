# Room 05 - Quick Reference

## ✅ Tapos Na!

### 1. Item Notifications ✅
**Files:**
- `Room05_DiningRoomController.cs` - Updated

**Changes:**
- Spoon pickup → May notification na
- Bedroom key pickup → May notification na
- Same style sa Room 02

---

### 2. Cinematic Chase Reference Script ✅
**Files:**
- `CinematicChaseTrigger.cs` - NEW!
- `CINEMATIC_CHASE_REFERENCE_GUIDE.md` - English guide
- `CINEMATIC_CHASE_SETUP_TAGALOG.md` - Tagalog guide

**Features:**
- ✅ Adjustable Emily speed (1-10)
- ✅ Adjustable catch distance (0.5-3)
- ✅ Knockback on trigger
- ✅ **Game Over on contact** (FIXED!)
- ✅ Dialogue support
- ✅ Audio support
- ✅ Visual debugging

---

## 🎮 Paano Gamitin

### Item Notifications:
```csharp
// Automatic na! Just pick up items:
InventoryManager.Instance.AddItemWithNotification("item_id");
```

### Cinematic Chase:
1. Create trigger zone (BoxCollider2D, Is Trigger = true)
2. Create spawn point (empty GameObject)
3. Add `CinematicChaseTrigger` script
4. Assign Emily GameObject
5. Assign spawn point
6. Set Emily speed: 5.5
7. Set catch distance: 1.0
8. Enable Game Over: ✅
9. Test!

---

## ⚙️ Key Settings

### Emily Speed:
- **3.5** = Mabagal (easier)
- **5.5** = Normal (recommended)
- **7.0** = Mabilis (intense)

### Catch Distance:
- **0.8** = Strict (mahirap mahuli)
- **1.0** = Standard (recommended)
- **1.5** = Forgiving (madaling mahuli)

### Knockback:
- **Force**: 10 (recommended)
- **Direction**: (-1, 0.5) = Back and up

---

## 🐛 Common Issues

### Walang Game Over pag nahuli ni Emily
**Solution:**
- ✅ Check "Enable Game Over" = enabled
- ✅ Check "Catch Distance" = 1.0 (hindi masyadong maliit)
- ✅ Check Emily has EmilyGhost component

### Mabilis/Mabagal si Emily
**Solution:**
- ✅ Adjust "Emily Chase Speed" slider
- ✅ Try: 3.5 (slow), 5.5 (normal), 7.0 (fast)

### Walang Knockback
**Solution:**
- ✅ Check "Enable Knockback" = enabled
- ✅ Check "Knockback Force" = 10
- ✅ Check player has Rigidbody2D

---

## 📚 Documentation

### English:
- `CINEMATIC_CHASE_REFERENCE_GUIDE.md` - Complete guide

### Tagalog:
- `CINEMATIC_CHASE_SETUP_TAGALOG.md` - Tagalog guide

### Summary:
- `ROOM05_UPDATES_SUMMARY.md` - Detailed summary
- `QUICK_REFERENCE.md` - This file

---

## 🎯 Quick Start

### Para sa Item Notifications:
```csharp
// Spoon
InventoryManager.Instance.AddItemWithNotification("spoon");

// Key
InventoryManager.Instance.AddItemWithNotification("bedroom_key");
```

### Para sa Cinematic Chase:
1. Gumawa ng trigger zone
2. Gumawa ng spawn point
3. Add CinematicChaseTrigger script
4. Assign Emily + spawn point
5. Set speed = 5.5, distance = 1.0
6. Enable Game Over
7. Test!

---

## ✅ Testing

### Item Notifications:
- [ ] Pick up spoon → Notification appears
- [ ] Pick up key → Notification appears

### Cinematic Chase:
- [ ] Enter trigger → Chase starts
- [ ] Knockback works
- [ ] Emily spawns correctly
- [ ] Emily chases player
- [ ] **Game Over triggers when caught**

---

## 💡 Tips

### Easy Chase:
- Speed: 3.5
- Distance: 1.0
- Knockback: 12

### Normal Chase:
- Speed: 5.5
- Distance: 1.0
- Knockback: 10

### Hard Chase:
- Speed: 7.0
- Distance: 1.5
- Knockback: 8

---

## 🎉 Done!

**Lahat tapos na!**
- ✅ Item notifications working
- ✅ Cinematic chase script ready
- ✅ Game Over on contact fixed
- ✅ Emily fully configurable

**Ready to use!** 🎮✨
