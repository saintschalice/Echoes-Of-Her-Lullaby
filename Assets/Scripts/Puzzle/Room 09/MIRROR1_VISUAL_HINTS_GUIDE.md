# 🎨 MIRROR 1 - VISUAL HINTS GUIDE

## 🎯 PARA MAINTINDIHAN NG PLAYER

Kailangan ng player ng **visual clues** para malaman kung paano i-arrange ang bottles!

---

## 💡 OPTION 1: LABELS ON BOTTLES (Recommended)

### **Add Text Labels to Each Bottle**

Para makita ng player ang year ng bawat bottle:

```
1. Select Bottle_1973 GameObject
2. Right-click → UI → Text - TextMeshPro
3. Name: "Year_Label"
4. Text: "1973"
5. Font Size: 18-24
6. Color: White or Black (depende sa bottle color)
7. Alignment: Center
8. Position: Center of bottle

Repeat for all bottles:
- Bottle_1974 → "1974"
- Bottle_1975a → "1975-A"
- Bottle_1975b → "1975-B"
- Bottle_1976a → "1976-A"
- Bottle_1976b → "1976-B"
```

### **Visual Example**:

```
┌─────┐  ┌─────┐  ┌─────┐  ┌─────┐  ┌─────┐  ┌─────┐
│ 🍾  │  │ 🍾  │  │ 🍾  │  │ 🍾  │  │ 🍾  │  │ 🍾  │
│1973 │  │1974 │  │1975A│  │1975B│  │1976A│  │1976B│
└─────┘  └─────┘  └─────┘  └─────┘  └─────┘  └─────┘
```

---

## 💡 OPTION 2: COLORED BOTTLES

### **Different Colors for Different Years**

Para mas visual ang difference:

```
1973: Blue bottle 🔵
1974: Green bottle 🟢
1975a: Yellow bottle 🟡
1975b: Orange bottle 🟠
1976a: Red bottle 🔴
1976b: Purple bottle 🟣
```

**Pero kailangan pa rin ng labels** para malaman ang exact year!

---

## 💡 OPTION 3: SLOT LABELS

### **Add Labels to Slots**

Para malaman ng player ang order:

```
1. Select Slot_1 GameObject
2. Right-click → UI → Text - TextMeshPro
3. Name: "Slot_Label"
4. Text: "1st"
5. Font Size: 16
6. Color: Gray
7. Position: Top or bottom of slot

Repeat for all slots:
- Slot_1 → "1st"
- Slot_2 → "2nd"
- Slot_3 → "3rd"
- Slot_4 → "4th"
- Slot_5 → "5th"
- Slot_6 → "6th"
```

### **Visual Example**:

```
┌─────┐  ┌─────┐  ┌─────┐  ┌─────┐  ┌─────┐  ┌─────┐
│ 1st │  │ 2nd │  │ 3rd │  │ 4th │  │ 5th │  │ 6th │
├─────┤  ├─────┤  ├─────┤  ├─────┤  ├─────┤  ├─────┤
│     │  │     │  │     │  │     │  │     │  │     │
│     │  │     │  │     │  │     │  │     │  │     │
└─────┘  └─────┘  └─────┘  └─────┘  └─────┘  └─────┘
```

---

## 💡 OPTION 4: PRESCRIPTION LABELS (Most Realistic)

### **Make Bottles Look Like Real Prescriptions**

Para mas realistic at immersive:

#### **Bottle Label Design**:

```
┌─────────────┐
│ PRESCRIPTION│
│             │
│ Date: 1973  │
│ Patient: M  │
│ Rx: [drug]  │
│             │
│ Refills: 0  │
└─────────────┘
```

#### **What to Show**:

```
Bottle_1973:
  Date: Jan 1973
  Patient: Mother
  Rx: Sedative

Bottle_1974:
  Date: Mar 1974
  Patient: Mother
  Rx: Antipsychotic

Bottle_1975a:
  Date: Jun 1975
  Patient: Mother
  Rx: Sedative (increased)

Bottle_1975b:
  Date: Dec 1975
  Patient: Mother
  Rx: Antipsychotic (increased)

Bottle_1976a:
  Date: Mar 1976
  Patient: Mother
  Rx: Sedative (high dose)

Bottle_1976b:
  Date: Jun 1976
  Patient: Mother
  Rx: Antipsychotic (high dose)
```

---

## 🎨 RECOMMENDED SETUP

### **Combination Approach** (Best for Players):

1. **Bottle Labels**: Show year clearly (1973, 1974, etc.)
2. **Hint Text**: "Arrange chronologically: 1973 → 1976"
3. **Slot Numbers**: Show order (1st, 2nd, 3rd, etc.)
4. **Color Coding**: Different colors for different years (optional)

### **Visual Layout**:

```
┌──────────────────────────────────────────────────┐
│  Medicine Cabinet Puzzle         [Timer: 1:00]  │
│                               [Mistakes: 0/3]    │
│                                                  │
│  ┌─────┐  ┌─────┐  ┌─────┐  ┌─────┐  ┌─────┐  ┌─────┐│
│  │ 1st │  │ 2nd │  │ 3rd │  │ 4th │  │ 5th │  │ 6th ││
│  ├─────┤  ├─────┤  ├─────┤  ├─────┤  ├─────┤  ├─────┤│
│  │     │  │     │  │     │  │     │  │     │  │     ││
│  │     │  │     │  │     │  │     │  │     │  │     ││
│  └─────┘  └─────┘  └─────┘  └─────┘  └─────┘  └─────┘│
│                                                  │
│  ┌─────┐  ┌─────┐  ┌─────┐  ┌─────┐  ┌─────┐  ┌─────┐│
│  │ 🍾  │  │ 🍾  │  │ 🍾  │  │ 🍾  │  │ 🍾  │  │ 🍾  ││
│  │1973 │  │1974 │  │1975A│  │1975B│  │1976A│  │1976B││
│  └─────┘  └─────┘  └─────┘  └─────┘  └─────┘  └─────┘│
│                                                  │
│  💡 Arrange chronologically: 1973 → 1976        │
└──────────────────────────────────────────────────┘
```

---

## 🔧 QUICK SETUP STEPS

### **Step 1: Add Year Labels to Bottles**

```
For each bottle (Bottle_1973 to Bottle_1976b):

1. Select bottle GameObject
2. Right-click → UI → Text - TextMeshPro
3. Name: "Year_Label"
4. Set text to year (e.g., "1973")
5. Font Size: 20-24
6. Color: White (with black outline for visibility)
7. Position: Center of bottle
8. Make sure it's visible on top of bottle sprite
```

### **Step 2: Add Order Labels to Slots**

```
For each slot (Slot_1 to Slot_6):

1. Select slot GameObject
2. Right-click → UI → Text - TextMeshPro
3. Name: "Order_Label"
4. Set text to order (e.g., "1st", "2nd", "3rd")
5. Font Size: 16-18
6. Color: Gray or White
7. Position: Top of slot (above the slot area)
```

### **Step 3: Style the Labels**

```
Year Labels (on bottles):
- Font: Bold
- Color: White
- Outline: Black (2-3 pixels)
- Shadow: Optional (for depth)

Order Labels (on slots):
- Font: Regular
- Color: Gray (#CCCCCC)
- No outline needed
```

---

## 🎯 WHAT PLAYER WILL UNDERSTAND

### **With Labels**:

```
Player sees:
1. 6 bottles with years: 1973, 1974, 1975A, 1975B, 1976A, 1976B
2. 6 slots labeled: 1st, 2nd, 3rd, 4th, 5th, 6th
3. Hint: "Arrange chronologically: 1973 → 1976"

Player thinks:
"Oh! I need to put the bottles in order from oldest to newest!"
"1973 goes in 1st slot, 1974 in 2nd slot, etc."
```

### **Without Labels**:

```
Player sees:
1. 6 identical-looking bottles
2. 6 empty slots
3. Hint: "Arrange chronologically: 1973 → 1976"

Player thinks:
"How do I know which bottle is which year??"
"This is impossible!"
```

---

## 📊 TESTING WITH LABELS

### **Test 1: Player Can Identify Bottles**

```
✅ Player can see year on each bottle
✅ Player can distinguish between 1975A and 1975B
✅ Player understands chronological order
```

### **Test 2: Player Understands Slots**

```
✅ Player knows which slot is first
✅ Player knows the order (1st → 6th)
✅ Player can match bottles to slots
```

### **Test 3: Feedback is Clear**

```
✅ Wrong placement: Bottle returns to start
✅ Mistakes counter updates
✅ Hint text shows error message
✅ Player can try again
```

---

## 🎨 ALTERNATIVE: TOOLTIP SYSTEM

### **Show Info on Hover** (Advanced)

Para sa mas clean na UI, pwede ring gumamit ng tooltips:

```
When player hovers over bottle:
- Show tooltip: "Prescription Bottle - January 1973"

When player hovers over slot:
- Show tooltip: "Place the oldest prescription here"
```

**Pero mas simple at clear ang labels!**

---

## 💡 RECOMMENDED APPROACH

### **For Best Player Experience**:

1. ✅ **Add year labels to bottles** (1973, 1974, 1975A, 1975B, 1976A, 1976B)
2. ✅ **Add order labels to slots** (1st, 2nd, 3rd, 4th, 5th, 6th)
3. ✅ **Keep hint text visible** ("Arrange chronologically: 1973 → 1976")
4. ✅ **Show mistakes counter** ("Mistakes: 0/3")
5. ✅ **Use color coding** (optional, for extra clarity)

### **This Gives Player**:

- 🎯 Clear goal (arrange chronologically)
- 👀 Visual identification (year labels)
- 📍 Clear positions (slot order)
- ⚠️ Feedback (mistakes counter)
- 💡 Guidance (hint text)

---

## 🚫 WHAT NOT TO DO

### **Don't Make It Too Hard**:

```
❌ No labels on bottles (player can't identify them)
❌ No hint text (player doesn't know the goal)
❌ No feedback (player doesn't know if they're wrong)
❌ Identical bottles (player can't tell them apart)
```

### **Don't Make It Too Easy**:

```
❌ Show exact answer (no challenge)
❌ Auto-complete (no gameplay)
❌ Unlimited tries with no consequence (no tension)
```

### **Perfect Balance**:

```
✅ Clear labels (player can identify)
✅ Hint text (player knows goal)
✅ 3 mistakes limit (creates tension)
✅ Visual feedback (player learns from mistakes)
```

---

## ✅ FINAL CHECKLIST

### **Visual Elements**:

- [ ] Year labels on all 6 bottles
- [ ] Order labels on all 6 slots
- [ ] Hint text visible at bottom
- [ ] Mistakes counter visible at top
- [ ] Timer visible at top
- [ ] All text is readable and clear

### **Gameplay Elements**:

- [ ] Each slot only accepts correct bottle
- [ ] Wrong bottles return to start
- [ ] Mistakes counter updates
- [ ] 3 mistakes = Emily attack
- [ ] Success = puzzle complete

### **Testing**:

- [ ] Player can identify each bottle
- [ ] Player understands the goal
- [ ] Player gets clear feedback
- [ ] Puzzle is challenging but fair
- [ ] Puzzle is completable

---

**VISUAL HINTS COMPLETE!** 🎨✨

**LABELS** make it clear! 🏷️

**PLAYER** can understand the puzzle! 🎯

**BALANCE** between challenge and clarity! ⚖️

