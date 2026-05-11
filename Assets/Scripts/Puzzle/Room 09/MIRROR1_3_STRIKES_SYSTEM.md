# 🎯 MIRROR 1 - 3 STRIKES SYSTEM

## ✨ NEW FEATURES

### **1. Slot-Specific Validation** ✅
- Each slot only accepts the CORRECT bottle
- Wrong bottle = rejected and returns to original position
- Right bottle = snaps into slot

### **2. 3 Mistakes System** ⚠️
- Player gets 3 chances
- Each wrong placement = 1 mistake
- 3 mistakes = Emily attacks (Game Over)

### **3. Visual Hints** 💡
- Hint text shows: "Arrange chronologically: 1973 → 1976"
- Mistakes counter shows: "Mistakes: 0/3"
- Wrong placement shows: "Wrong! That bottle doesn't belong there. (1/3 mistakes)"

---

## 🎮 HOW IT WORKS

### **Correct Placement**:
```
1. Player drags bottle_1973 to Slot_1
2. System checks: Is bottle_1973 correct for Slot_1? YES!
3. Bottle snaps into slot
4. Hint text stays: "Arrange chronologically: 1973 → 1976"
5. Continue to next bottle
```

### **Wrong Placement**:
```
1. Player drags bottle_1976b to Slot_1
2. System checks: Is bottle_1976b correct for Slot_1? NO!
3. Bottle returns to original position
4. Mistake counter: 0/3 → 1/3
5. Hint text: "Wrong! That bottle doesn't belong there. (1/3 mistakes)"
6. Wrong sound plays
7. Player tries again
```

### **3rd Mistake**:
```
1. Player makes 3rd wrong placement
2. Mistake counter: 3/3 (turns RED)
3. Emily jumpscare appears
4. Emily attack dialogue plays
5. Game Over - scene reloads
```

---

## 📋 CORRECT BOTTLE ORDER

Each slot only accepts ONE specific bottle:

```
Slot_1 → bottle_1973 ✅
Slot_2 → bottle_1974 ✅
Slot_3 → bottle_1975a ✅
Slot_4 → bottle_1975b ✅
Slot_5 → bottle_1976a ✅
Slot_6 → bottle_1976b ✅
```

**Any other combination = REJECTED!**

---

## 🔧 UNITY SETUP

### **Step 1: Add New UI Elements**

Sa MedicineCabinet_Panel, add these:

#### **A. Mistakes Text**
```
1. Right-click MedicineCabinet_Panel
2. UI → Text - TextMeshPro
3. Name: "Mistakes_Text"
4. Position: Top-right corner
5. Text: "Mistakes: 0/3"
6. Font Size: 24
7. Color: White
8. Alignment: Right
```

#### **B. Hint Text**
```
1. Right-click MedicineCabinet_Panel
2. UI → Text - TextMeshPro
3. Name: "Hint_Text"
4. Position: Bottom center
5. Text: "Arrange chronologically: 1973 → 1976"
6. Font Size: 20
7. Color: Yellow
8. Alignment: Center
```

---

### **Step 2: Assign References**

Select **Mirror1_MedicineCabinet** GameObject:

#### **Inspector → Mirror1_MedicineCabinet Component**:

```
✅ Puzzle Panel: MedicineCabinet_Panel
✅ Bottle Slots (6):
   - Element 0: Slot_1
   - Element 1: Slot_2
   - Element 2: Slot_3
   - Element 3: Slot_4
   - Element 4: Slot_5
   - Element 5: Slot_6

✅ Timer Text: Timer_Text

✅ Success Effect: (your success particle/image)
✅ Success Sound: (your success audio clip)

✅ Emily Jumpscare Panel: (your Emily jumpscare panel)
✅ Emily Scream Sound: (your Emily scream audio clip)
✅ Time Limit: 60

🆕 Mistakes Text: Mistakes_Text
🆕 Max Mistakes: 3
🆕 Wrong Placement Sound: (your error/buzz sound)

🆕 Hint Text: Hint_Text
```

---

### **Step 3: Verify Bottle Item IDs**

Select each bottle GameObject and check DraggableItem component:

```
Bottle_1973:
  ✅ Item Id: "bottle_1973"
  ✅ Puzzle Number: 1

Bottle_1974:
  ✅ Item Id: "bottle_1974"
  ✅ Puzzle Number: 1

Bottle_1975a:
  ✅ Item Id: "bottle_1975a"
  ✅ Puzzle Number: 1

Bottle_1975b:
  ✅ Item Id: "bottle_1975b"
  ✅ Puzzle Number: 1

Bottle_1976a:
  ✅ Item Id: "bottle_1976a"
  ✅ Puzzle Number: 1

Bottle_1976b:
  ✅ Item Id: "bottle_1976b"
  ✅ Puzzle Number: 1
```

**IMPORTANT**: Item IDs must match EXACTLY (with underscore!)

---

### **Step 4: Verify Slot Names**

```
Slot_1 ✅
Slot_2 ✅
Slot_3 ✅
Slot_4 ✅
Slot_5 ✅
Slot_6 ✅
```

Names must contain "Slot" for detection to work!

---

## 🎨 VISUAL LAYOUT

### **Recommended Panel Layout**:

```
┌─────────────────────────────────────────┐
│  Medicine Cabinet Puzzle    [Timer: 1:00]│
│                          [Mistakes: 0/3] │
│                                          │
│  ┌────┐ ┌────┐ ┌────┐ ┌────┐ ┌────┐ ┌────┐│
│  │ S1 │ │ S2 │ │ S3 │ │ S4 │ │ S5 │ │ S6 ││
│  └────┘ └────┘ └────┘ └────┘ └────┘ └────┘│
│                                          │
│  🍾    🍾    🍾    🍾    🍾    🍾        │
│ 1973  1974  1975a 1975b 1976a 1976b     │
│                                          │
│  💡 Arrange chronologically: 1973 → 1976 │
└─────────────────────────────────────────┘
```

---

## 🐛 CONSOLE MESSAGES

### **Correct Placement**:
```
[DraggableItem] Started dragging: bottle_1973
[DraggableItem] bottle_1973 dropped on Slot_1
[Mirror1] 🍾 Validating bottle bottle_1973 for slot Slot_1
[Mirror1] ✅ CORRECT! bottle_1973 belongs in slot 0
[DraggableItem] bottle_1973 placed in Slot_1
```

### **Wrong Placement**:
```
[DraggableItem] Started dragging: bottle_1976b
[DraggableItem] bottle_1976b dropped on Slot_1
[Mirror1] 🍾 Validating bottle bottle_1976b for slot Slot_1
[Mirror1] ❌ WRONG! Slot 0 expects bottle_1973, got bottle_1976b
[DraggableItem] bottle_1976b placement rejected - returning to original position
```

### **3rd Mistake (Game Over)**:
```
[Mirror1] ❌ WRONG! Slot 0 expects bottle_1973, got bottle_1976b
[Mirror1] ☠️ TOO MANY MISTAKES! EMILY ATTACKS!
```

---

## 🎯 TESTING CHECKLIST

### **Test 1: Correct Placement**
```
✅ Drag bottle_1973 to Slot_1
✅ Bottle should snap into slot
✅ Mistakes counter stays 0/3
✅ Hint text stays normal
```

### **Test 2: Wrong Placement**
```
✅ Drag bottle_1976b to Slot_1
✅ Bottle should return to original position
✅ Mistakes counter: 0/3 → 1/3
✅ Hint text: "Wrong! That bottle doesn't belong there. (1/3 mistakes)"
✅ Wrong sound plays
```

### **Test 3: 3 Mistakes**
```
✅ Make 3 wrong placements
✅ Mistakes counter: 3/3 (RED)
✅ Emily jumpscare appears
✅ Emily attack dialogue plays
✅ Scene reloads (Game Over)
```

### **Test 4: Complete Puzzle**
```
✅ Place all 6 bottles in correct order
✅ Success dialogue plays
✅ Panel closes
✅ Room09_FlowController notified
```

---

## 💡 PLAYER HINTS

### **Visual Clues on Bottles**:

Para mas madali, lagyan ng labels ang bottles:

```
Bottle_1973: Add Text "1973"
Bottle_1974: Add Text "1974"
Bottle_1975a: Add Text "1975 (A)"
Bottle_1975b: Add Text "1975 (B)"
Bottle_1976a: Add Text "1976 (A)"
Bottle_1976b: Add Text "1976 (B)"
```

### **Visual Clues on Slots**:

Para mas clear, lagyan din ng labels ang slots:

```
Slot_1: Add Text "1st"
Slot_2: Add Text "2nd"
Slot_3: Add Text "3rd"
Slot_4: Add Text "4th"
Slot_5: Add Text "5th"
Slot_6: Add Text "6th"
```

---

## 🎨 COLOR CODING

### **Mistakes Text Colors**:

```
0 mistakes: White ⚪
1 mistake: Yellow 🟡
2 mistakes: Orange 🟠
3 mistakes: Red 🔴
```

### **Timer Text Colors**:

```
> 20 seconds: White ⚪
10-20 seconds: Yellow 🟡
< 10 seconds: Red 🔴
```

---

## 🔊 AUDIO FEEDBACK

### **Sounds Needed**:

```
✅ Success Sound: Pleasant chime/bell
❌ Wrong Placement Sound: Buzz/error sound
⏰ Timer Warning: Ticking sound (optional)
☠️ Emily Scream: Loud scream/jumpscare sound
```

---

## 📊 GAME FLOW

### **Success Path**:
```
1. Player places bottle_1973 in Slot_1 ✅
2. Player places bottle_1974 in Slot_2 ✅
3. Player places bottle_1975a in Slot_3 ✅
4. Player places bottle_1975b in Slot_4 ✅
5. Player places bottle_1976a in Slot_5 ✅
6. Player places bottle_1976b in Slot_6 ✅
7. All 6 correct → Puzzle complete! 🎉
8. Success dialogue plays
9. Panel closes
10. Continue to next mirror
```

### **Failure Path (Mistakes)**:
```
1. Player tries wrong bottle → Mistake 1/3 ⚠️
2. Player tries wrong bottle → Mistake 2/3 ⚠️⚠️
3. Player tries wrong bottle → Mistake 3/3 ☠️
4. Emily jumpscare appears
5. Emily attack dialogue
6. Game Over - scene reloads
```

### **Failure Path (Timeout)**:
```
1. Timer reaches 0:00
2. Emily jumpscare appears
3. Emily attack dialogue
4. Game Over - scene reloads
```

---

## ✅ SUMMARY

### **What Changed**:

1. ✅ Each slot validates bottle before accepting
2. ✅ Wrong bottles are rejected and return to start
3. ✅ 3 mistakes = Emily attack (Game Over)
4. ✅ Visual feedback (mistakes counter, hint text)
5. ✅ Audio feedback (wrong sound)
6. ✅ Color-coded warnings

### **What Player Sees**:

1. 💡 Hint: "Arrange chronologically: 1973 → 1976"
2. ⏰ Timer counting down
3. ⚠️ Mistakes counter: "Mistakes: X/3"
4. ✅ Correct bottles snap into slots
5. ❌ Wrong bottles bounce back
6. 🎉 Success when all 6 correct
7. ☠️ Emily attack after 3 mistakes or timeout

---

**SYSTEM COMPLETE!** ✅

**3 STRIKES** and you're out! ⚾

**EACH SLOT** only accepts the correct bottle! 🎯

**VISUAL HINTS** help the player! 💡

