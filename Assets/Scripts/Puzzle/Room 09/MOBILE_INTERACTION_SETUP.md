# 📱 ROOM 09 - MOBILE INTERACTION SETUP

## 🎮 PARA SA MOBILE GAME

Ito ang guide para sa mobile touch/tap interaction sa Room 09 mirrors.

---

## 🎯 2 OPTIONS FOR MOBILE INTERACTION

### **OPTION 1: Direct Tap on Mirror** (Recommended)
- Player taps directly on mirror sprite
- Automatic interaction
- Simple and intuitive

### **OPTION 2: Tap Button When Near**
- Player walks near mirror
- "Tap to examine" button appears
- Player taps button to interact

**Pwede both!** Player can tap mirror directly OR tap button.

---

## 🔧 SETUP: OPTION 1 - DIRECT TAP

### **STEP 1: Add Collider to Mirror**

```
1. Select mirror GameObject (e.g., "Mirror1_MedicineC abinet")

2. Add Component → Box Collider 2D
   - Is Trigger: ✗ (UNCHECKED for tap detection)
   - Size: Cover the entire mirror sprite
   
3. Make collider slightly bigger than sprite for easier tapping
```

### **STEP 2: Add Room09_Interactable Script**

```
1. Select mirror GameObject

2. Add Component → Room09_Interactable

3. In Inspector, set:
   - Mirror Number: 1 (or 2, 3, 4)
   - Interaction Radius: 3 (larger for mobile)
   - Use Tap Interaction: ✓ (CHECKED)
   - Use Proximity Interaction: ✓ (CHECKED - optional)
   - Interaction Prompt: (leave empty for now)
```

### **STEP 3: Test Direct Tap**

```
1. Build to Android/iOS or use Unity Remote

2. Walk player near mirror

3. Tap directly on mirror sprite

4. Puzzle panel should open!
```

---

## 🔧 SETUP: OPTION 2 - TAP BUTTON

### **STEP 1: Create Interaction Button**

```
1. In Canvas, create:
   - Right-click Canvas → UI → Button
   - Name: "InteractButton_Mirror1"
   
2. Position:
   - Bottom center of screen
   - Or floating above mirror
   
3. Button Text:
   - "Tap to Examine"
   - Or just an icon (hand, magnifying glass)
   
4. Button Settings:
   - Size: Large enough for finger tap (100x100 minimum)
   - Color: Visible but not intrusive
   
5. Set Active: ✗ (unchecked - starts hidden)
```

### **STEP 2: Create Button for Each Mirror**

```
Duplicate button 4 times:
- InteractButton_Mirror1
- InteractButton_Mirror2
- InteractButton_Mirror3
- InteractButton_Mirror4

Position each button near its mirror or use same button for all.
```

### **STEP 3: Assign Button to Room09_Interactable**

```
1. Select mirror GameObject

2. Find Room09_Interactable component

3. Drag button to "Interaction Prompt" field

4. Button will show/hide automatically when player is near
```

### **STEP 4: Setup Button OnClick**

```
1. Select button GameObject

2. In Inspector, find Button component

3. OnClick() section:
   - Click "+"
   - Drag mirror GameObject to object field
   - Function: Room09_Interactable → TriggerInteraction()
   
4. Now button will trigger puzzle when tapped!
```

---

## 🎨 RECOMMENDED UI SETUP

### **Interaction Button Design**:

```
Button:
├─ Background: Semi-transparent circle or rounded rect
├─ Icon: Hand icon or magnifying glass
└─ Text: "Tap" or "Examine" (optional)

Size: 80x80 to 120x120 pixels
Position: Bottom center or near mirror
Color: White with slight glow
Alpha: 0.8 (semi-transparent)
```

### **Example Button Hierarchy**:

```
InteractButton_Mirror1
├─ Background (Image - circle)
├─ Icon (Image - hand icon)
└─ Text (TextMeshProUGUI - "Tap")
```

---

## 🎯 BEST PRACTICES FOR MOBILE

### **1. Make Tap Targets Large**:
```
- Mirror colliders should be generous
- Buttons should be at least 80x80 pixels
- Add padding around tap areas
```

### **2. Visual Feedback**:
```
- Highlight mirror when player is near
- Pulse/glow effect on button
- Show "Tap" text clearly
```

### **3. Proximity Detection**:
```
- Interaction Radius: 3-4 units (larger than PC)
- Player should be able to tap from reasonable distance
- Don't make player walk too close
```

### **4. Clear Instructions**:
```
- First time: Show tutorial "Tap mirrors to examine"
- Use icons that are universally understood
- Test with actual mobile device
```

---

## 🔧 ALTERNATIVE: ALWAYS-VISIBLE BUTTONS

Kung gusto mo laging visible ang buttons (hindi proximity-based):

### **Setup**:

```
1. Create 4 buttons in Canvas (one per mirror)

2. Position each button near its mirror in world space
   - Or use Screen Space - Camera canvas
   - Position buttons over mirrors
   
3. Buttons always visible (Active: ✓)

4. OnClick for each button:
   - Button 1 → Mirror1.StartPuzzle()
   - Button 2 → Mirror2.StartPuzzle()
   - Button 3 → Mirror3.StartPuzzle()
   - Button 4 → Mirror4.StartPuzzle()
   
5. No need for Room09_Interactable script
```

### **Pros**:
- ✅ Simple setup
- ✅ Always clear what's interactable
- ✅ No proximity detection needed

### **Cons**:
- ❌ Buttons always visible (may clutter UI)
- ❌ Less immersive
- ❌ Player can tap from anywhere

---

## 🐛 TROUBLESHOOTING

### **Problem 1: Can't tap mirror**

**Check**:
- [ ] Mirror has Collider2D (not trigger for tap)
- [ ] Room09_Interactable script attached
- [ ] Use Tap Interaction is CHECKED
- [ ] Canvas has GraphicRaycaster component
- [ ] EventSystem exists in scene

**Fix**:
```
1. Make sure Canvas has:
   - Canvas component
   - Canvas Scaler
   - Graphic Raycaster (important!)
   
2. Make sure scene has EventSystem:
   - GameObject → UI → Event System
   - Should auto-create when you create Canvas
```

---

### **Problem 2: Button doesn't appear**

**Check**:
- [ ] Button is child of Canvas
- [ ] Button starts inactive (Active: ✗)
- [ ] Button assigned to Interaction Prompt field
- [ ] Player is within Interaction Radius
- [ ] Use Proximity Interaction is CHECKED

**Debug**:
```
1. Temporarily set button Active: ✓
2. Check if button is visible in Game view
3. Check button position (should be on screen)
4. Check Canvas Render Mode (Screen Space - Overlay)
```

---

### **Problem 3: Button appears but doesn't work**

**Check**:
- [ ] Button has Button component
- [ ] OnClick event is setup
- [ ] Correct function selected (TriggerInteraction)
- [ ] Mirror GameObject is assigned in OnClick
- [ ] Button is not blocked by other UI

**Fix**:
```
1. Select button
2. Button component → OnClick()
3. Make sure:
   - Object: Mirror GameObject (not button itself)
   - Function: Room09_Interactable.TriggerInteraction
```

---

### **Problem 4: Works in editor but not on mobile**

**Check**:
- [ ] Build settings include scene
- [ ] Input system is set to "Both" or "Input Manager"
- [ ] Touch input is enabled
- [ ] Canvas is set to correct render mode

**Test**:
```
1. Use Unity Remote for quick testing
2. Build to device and test
3. Check device logs for errors
4. Test on multiple devices if possible
```

---

## 📋 MOBILE SETUP CHECKLIST

### **For Each Mirror**:

- [ ] Mirror GameObject in scene
- [ ] Mirror has sprite
- [ ] Mirror has Box Collider 2D (not trigger)
- [ ] Mirror has Room09_Interactable
  - [ ] Mirror Number set (1-4)
  - [ ] Interaction Radius: 3-4
  - [ ] Use Tap Interaction: ✓
  - [ ] Use Proximity Interaction: ✓ (optional)
- [ ] Mirror has puzzle script (Mirror1, Mirror2, etc.)
  - [ ] All references assigned

### **UI Setup**:

- [ ] Canvas exists
  - [ ] Has Graphic Raycaster
  - [ ] Render Mode: Screen Space - Overlay
- [ ] EventSystem exists
- [ ] Interaction buttons created (optional)
  - [ ] Positioned correctly
  - [ ] Start inactive
  - [ ] OnClick setup
  - [ ] Assigned to mirrors

### **Testing**:

- [ ] Test in Unity editor (simulate touch)
- [ ] Test with Unity Remote
- [ ] Build to device and test
- [ ] Test all 4 mirrors
- [ ] Test on different screen sizes

---

## 🎯 RECOMMENDED APPROACH

### **For Mobile Game**:

**Use BOTH methods**:
1. **Direct tap on mirror** - Primary interaction
2. **Button when near** - Secondary/backup

**Why?**
- Direct tap feels natural
- Button provides clear feedback
- Works for all player types
- Accessible and intuitive

**Setup**:
```
1. Add Room09_Interactable to mirrors
2. Enable both tap and proximity
3. Create interaction buttons
4. Assign buttons to mirrors
5. Test both methods
```

---

## ✅ SUMMARY

### **Mobile Interaction Options**:

1. **Direct Tap** - Tap mirror sprite directly
2. **Button Tap** - Tap button when near mirror
3. **Always-Visible Buttons** - Buttons always on screen

### **Recommended**:
- Use Direct Tap + Button (both enabled)
- Larger interaction radius (3-4 units)
- Clear visual feedback
- Test on actual device

### **Key Differences from PC**:
- ❌ No keyboard input (E key)
- ✅ Touch/tap input
- ✅ Larger tap targets
- ✅ Visual button feedback

---

**MOBILE INTERACTION READY!** 📱🎮

Just add Room09_Interactable and setup buttons!

**KAYA MO YAN!** 💪✨📱
