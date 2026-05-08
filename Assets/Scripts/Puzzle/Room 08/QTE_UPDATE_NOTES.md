# 🎯 QTE UPDATE - 15 TAPS, 2 MINUTES, 4 PHASES

## 🔄 CHANGES MADE

### **OLD SYSTEM:**
- 5 taps total
- Decreasing time per tap (2.0s → 0.8s)
- 5 crack sprites (array)

### **NEW SYSTEM:** ✅
- **15 taps total**
- **2 minutes (120 seconds) total time limit**
- **3 seconds per tap window**
- **3 failures = game over**
- **4 mirror phases** (your sprites!)

---

## 🎨 MIRROR PHASES

Based on your sprites:

### **Phase 1: Clean Mirror** (0-3 taps, 0-25%)
```
┌─────────┐
│         │
│  Clean  │
│  Mirror │
│         │
└─────────┘
```
- Sprite: `mirrorPhase1`
- Taps: 0-3

### **Phase 2: First Cracks** (4-7 taps, 25-50%)
```
┌─────────┐
│    /    │
│  First  │
│  Cracks │
│         │
└─────────┘
```
- Sprite: `mirrorPhase2`
- Taps: 4-7

### **Phase 3: More Cracks** (8-11 taps, 50-75%)
```
┌─────────┐
│  / \    │
│  More   │
│  Cracks │
│         │
└─────────┘
```
- Sprite: `mirrorPhase3`
- Taps: 8-11

### **Phase 4: Almost Shattered** (12-15 taps, 75-100%)
```
┌─────────┐
│/|\|\/\  │
│ Almost  │
│Shattered│
│         │
└─────────┘
```
- Sprite: `mirrorPhase4`
- Taps: 12-15

---

## 🎮 NEW QTE MECHANICS

### **Timing:**
- **Total Time:** 2 minutes (120 seconds)
- **Per Tap:** 3 seconds to click each target
- **Failures:** 3 misses = game over

### **Win Condition:**
- Complete all 15 taps within 2 minutes
- Maximum 2 failures allowed

### **Lose Conditions:**
1. 3 failed taps (missed targets)
2. 2 minutes run out
3. Either condition triggers game over

---

## 🔧 INSPECTOR SETUP

### **Room08_MirrorQTE Component**

#### **QTE Settings:**
```
Total Taps: 15
Total Time Limit: 120
Tap Time Window: 3
Max Failures: 3
```

#### **UI References:**
```
QTE Panel: [QTE_Panel]
Tap Target Prefab: [TapTarget]
Tap Target Parent: [Tap_Target_Parent]
Timer Text: [Timer_Text] - Shows total time (2:00)
Progress Text: [Progress_Text] - Shows taps (5/15)
Tap Timer Text: [Optional] - Shows per-tap countdown (3.0)
```

#### **Visual Effects:**
```
Mirror Image: [Mirror_Image]
Mirror Phase 1: [Your clean mirror sprite]
Mirror Phase 2: [Your first cracks sprite]
Mirror Phase 3: [Your more cracks sprite]
Mirror Phase 4: [Your almost shattered sprite]
Shatter Effect: [Particle System] (optional)
```

#### **Audio:**
```
Tap Sound: [Click sound]
Crack Sound: [Glass crack]
Shatter Sound: [Glass shatter]
Fail Sound: [Error beep]
Glass Stress Sounds: [Array of 5 sounds]
```

#### **Camera Shake:**
```
Shake Intensity: 0.1
Shake Duration: 0.2
```

---

## 📊 UI LAYOUT

### **Timer Text** (Top Center)
Shows total time remaining:
```
2:00  (at start)
1:30  (after 30 seconds)
1:00  (after 1 minute)
0:30  (30 seconds left - turns yellow)
0:15  (15 seconds left - turns red)
```

### **Progress Text** (Bottom Center)
Shows tap progress:
```
0/15   (at start)
5/15   (after 5 taps)
10/15  (after 10 taps)
15/15  (complete!)

MISS! (2 left)  (when failed, turns red)
```

### **Tap Timer Text** (Optional, near target)
Shows per-tap countdown:
```
3.0  (when target spawns)
2.5  (half second later)
1.0  (2 seconds later)
0.0  (time's up - miss!)
```

---

## 🎯 GAMEPLAY FLOW

```
START QTE
  ↓
Total Timer: 2:00
  ↓
┌─────────────────────┐
│  Spawn Tap Target   │
│  Per-Tap Timer: 3s  │
└─────────────────────┘
  ↓
Player Clicks?
  ↓
YES → Success!
  ├─ Play tap sound
  ├─ Update mirror sprite
  ├─ Camera shake
  ├─ Progress: X/15
  └─ Spawn next target
  
NO (3s expired) → Miss!
  ├─ Play fail sound
  ├─ Failure count++
  ├─ Show "MISS!"
  └─ Check failures
      ↓
      3 failures? → GAME OVER
      < 3 failures? → Spawn next target
  
Continue until:
- 15 taps complete → WIN!
- 3 failures → GAME OVER
- 2 minutes expired → GAME OVER
```

---

## 🐛 TESTING CHECKLIST

### **Test 1: Normal Success**
- [ ] Start QTE
- [ ] Total timer shows 2:00
- [ ] Tap all 15 targets successfully
- [ ] Mirror changes phases (1→2→3→4)
- [ ] Mirror shatters after 15th tap
- [ ] Passage revealed

### **Test 2: Time Pressure**
- [ ] Start QTE
- [ ] Wait until 30 seconds left
- [ ] Timer turns yellow
- [ ] Wait until 15 seconds left
- [ ] Timer turns red
- [ ] Complete before time runs out

### **Test 3: Failure Path**
- [ ] Start QTE
- [ ] Miss 1 target → "MISS! (2 left)"
- [ ] Miss 2 targets → "MISS! (1 left)"
- [ ] Miss 3 targets → Game Over

### **Test 4: Time Out**
- [ ] Start QTE
- [ ] Wait 2 minutes without completing
- [ ] Game Over triggers

### **Test 5: Per-Tap Timer**
- [ ] Target spawns
- [ ] Per-tap timer shows 3.0
- [ ] Timer counts down
- [ ] If not clicked in 3s → Miss

---

## 💡 DIFFICULTY TUNING

If QTE is too hard/easy, adjust these:

### **Make Easier:**
```
Total Time Limit: 180 (3 minutes)
Tap Time Window: 4.0 (4 seconds per tap)
Max Failures: 5 (5 misses allowed)
```

### **Make Harder:**
```
Total Time Limit: 90 (1.5 minutes)
Tap Time Window: 2.0 (2 seconds per tap)
Max Failures: 2 (only 2 misses)
```

### **Current (Balanced):**
```
Total Time Limit: 120 (2 minutes)
Tap Time Window: 3.0 (3 seconds per tap)
Max Failures: 3 (3 misses)
```

---

## 🎨 VISUAL FEEDBACK

### **Mirror Progression:**
```
Taps 0-3:   Phase 1 (clean)
Taps 4-7:   Phase 2 (first cracks)
Taps 8-11:  Phase 3 (more cracks)
Taps 12-15: Phase 4 (almost shattered)
Tap 15:     SHATTER! 💥
```

### **Timer Colors:**
```
2:00 - 1:00  → White (plenty of time)
1:00 - 0:30  → Yellow (getting close)
0:30 - 0:00  → Red (hurry!)
```

### **Progress Colors:**
```
Normal: White
Miss:   Red (briefly)
```

---

## 🔊 AUDIO PROGRESSION

### **Glass Stress Sounds:**
If you have 5 stress sounds, they play based on progress:

```
Taps 0-2:   Stress Sound 1 (light)
Taps 3-5:   Stress Sound 2 (medium)
Taps 6-8:   Stress Sound 3 (louder)
Taps 9-11:  Stress Sound 4 (very loud)
Taps 12-15: Stress Sound 5 (almost breaking)
```

---

## 📝 SUMMARY

### **What Changed:**
- ✅ 5 taps → **15 taps**
- ✅ Decreasing time → **2 minutes total + 3s per tap**
- ✅ 5 crack sprites array → **4 phase sprites**
- ✅ Updated mirror sprite logic
- ✅ Added total timer + per-tap timer
- ✅ Better failure handling

### **What Stayed Same:**
- ✅ 3 failures = game over
- ✅ Random tap positions
- ✅ Camera shake
- ✅ Audio feedback
- ✅ Particle effects

---

## 🎉 READY!

Your QTE is now:
- **15 taps** instead of 5
- **2 minutes** total time
- **3 seconds** per tap
- **4 mirror phases** (your sprites!)
- **3 failures** = game over

**GOOD LUCK!** 🎮✨
