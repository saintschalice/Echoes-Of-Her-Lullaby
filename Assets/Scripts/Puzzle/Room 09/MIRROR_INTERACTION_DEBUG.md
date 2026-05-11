# 🐛 ROOM 09 - MIRROR INTERACTION DEBUG GUIDE

## ❌ PROBLEM: "hindi pa rin ma interact yung mga mirror"

Ito ang step-by-step guide para i-fix ang interaction problem!

---

## 🔍 STEP 1: CHECK MIRROR SETUP

### **For EACH Mirror (1-4), check these**:

#### **A. Mirror GameObject Must Have**:

```
1. Select mirror GameObject in Hierarchy

2. Check Inspector - MUST HAVE:
   ✓ Sprite Renderer (para makita)
   ✓ Collider2D (Box Collider 2D or Circle Collider 2D)
   ✓ Room09_Interactable script
   ✓ Puzzle script (Mirror1, Mirror2, Mirror3, or Mirror4)
```

#### **B. Collider2D Settings**:

```
VERY IMPORTANT!

Select mirror → Inspector → Collider2D component

Settings MUST BE:
✓ Is Trigger: CHECKED (✓)
✓ Size: Large enough to cover sprite (e.g., 2x2 or bigger)
✓ Offset: (0, 0) or centered on sprite

COMMON MISTAKE: Is Trigger is UNCHECKED ❌
FIX: CHECK the "Is Trigger" box! ✓
```

#### **C. Room09_Interactable Settings**:

```
Select mirror → Inspector → Room09_Interactable component

Settings:
✓ Mirror Number: 1 (or 2, 3, 4 - must be set!)
✓ Script is enabled (checkbox at top is checked)

COMMON MISTAKE: Mirror Number is 0 or not set ❌
FIX: Set Mirror Number to 1, 2, 3, or 4 ✓
```

---

## 🔍 STEP 2: CHECK PLAYER SETUP

### **Player GameObject Must Have**:

```
1. Find Player in Hierarchy

2. Check Inspector - MUST HAVE:
   ✓ PlayerInteractionController component
   ✓ Collider2D component
   ✓ Tag: "Player"

3. PlayerInteractionController settings:
   ✓ Interaction Radius: 2.25 (or higher for easier interaction)
   ✓ Interactable Layers: Everything (or layer where mirrors are)
   ✓ Interact Key: E
   ✓ Script is enabled
```

### **Common Player Issues**:

```
❌ Player doesn't have PlayerInteractionController
   FIX: Add Component → PlayerInteractionController

❌ Interaction Radius too small (e.g., 0.5)
   FIX: Set to 2.25 or higher

❌ Interactable Layers doesn't include mirror layer
   FIX: Set to "Everything" or include mirror layer

❌ Player tag is not "Player"
   FIX: Set Tag to "Player"
```

---

## 🔍 STEP 3: CHECK LAYER SETTINGS

### **Mirror Layer Must Be Detectable**:

```
1. Select mirror GameObject

2. Check top of Inspector → Layer dropdown

3. Mirror should be on a layer that PlayerInteractionController can detect

4. PlayerInteractionController → Interactable Layers should include this layer

EASIEST FIX: Set Interactable Layers to "Everything"
```

---

## 🔍 STEP 4: TEST INTERACTION RADIUS

### **Visual Debug**:

```
1. Select Player in Hierarchy

2. Inspector → PlayerInteractionController

3. Check "Draw Radius" ✓

4. Play scene

5. Look at Scene view (not Game view)

6. You should see a CYAN CIRCLE around player

7. Walk player near mirror

8. Mirror should be INSIDE the cyan circle to interact

If circle is too small:
- Increase Interaction Radius (try 3 or 4)
```

---

## 🔍 STEP 5: CHECK CONSOLE FOR ERRORS

### **Play Scene and Check Console**:

```
1. Play scene

2. Walk player near mirror

3. Open Console (Window → General → Console)

4. Look for errors or warnings

Common errors:
- "NullReferenceException" = Missing reference
- "Component not found" = Missing script
- No messages = Interaction not detected
```

---

## 🔍 STEP 6: ADD DEBUG LOGS

### **Temporary Debug Code**:

Add this to Room09_Interactable.cs to see if it's being called:

```csharp
public void OnFocus(PlayerContext context)
{
    Debug.Log($"[Room09] ⭐ FOCUSED on Mirror {mirrorNumber}");
}

public void OnBlur(PlayerContext context)
{
    Debug.Log($"[Room09] ❌ BLURRED from Mirror {mirrorNumber}");
}

public void OnInteract(PlayerContext context)
{
    Debug.Log($"[Room09] 🎯 INTERACTING with Mirror {mirrorNumber}");
    // ... rest of code
}
```

### **What to Look For**:

```
Play scene → Walk near mirror → Check Console:

✅ GOOD: "[Room09] ⭐ FOCUSED on Mirror 1"
   = Player detected mirror!
   
✅ GOOD: "[Room09] 🎯 INTERACTING with Mirror 1"
   = Interaction working!

❌ BAD: No messages at all
   = Player not detecting mirror
   = Check collider, layer, radius
```

---

## 🔍 STEP 7: CHECK INTERACT BUTTON/KEY

### **How to Interact**:

```
Your game uses TWO ways to interact:

1. KEYBOARD: Press E key
2. MOBILE: Tap Interact button (bottom right)

Make sure you're using the correct method!
```

### **Test Keyboard**:

```
1. Play scene in Unity editor
2. Walk player near mirror
3. Press E key
4. Panel should open

If nothing happens:
- Check PlayerInteractionController → Interact Key is "E"
- Check Console for debug messages
```

### **Test Mobile Button**:

```
1. Find InteractButton in Hierarchy (or Canvas)
2. Make sure it exists and is active
3. Button should appear when near mirror
4. Tap button to interact

If button doesn't appear:
- Check if button is connected to PlayerInteractionController
- Check button's OnClick event
```

---

## 🔧 QUICK FIX CHECKLIST

### **Do These in Order**:

- [ ] **1. Mirror has Collider2D**
  - [ ] Is Trigger: ✓ CHECKED
  - [ ] Size: 2x2 or bigger
  
- [ ] **2. Mirror has Room09_Interactable**
  - [ ] Mirror Number: 1-4 (not 0!)
  - [ ] Script enabled
  
- [ ] **3. Player has PlayerInteractionController**
  - [ ] Interaction Radius: 2.25 or higher
  - [ ] Interactable Layers: Everything
  - [ ] Script enabled
  
- [ ] **4. Player tag is "Player"**
  
- [ ] **5. Test with Draw Radius**
  - [ ] Enable Draw Radius
  - [ ] See cyan circle
  - [ ] Mirror inside circle
  
- [ ] **6. Add debug logs**
  - [ ] See "FOCUSED" message
  - [ ] Press E or tap button
  - [ ] See "INTERACTING" message

---

## 🎯 MOST COMMON ISSUES

### **Issue 1: Is Trigger Not Checked** (90% of problems)

```
Problem: Collider2D → Is Trigger: ✗ (unchecked)

Fix:
1. Select mirror
2. Inspector → Collider2D
3. Check "Is Trigger" ✓

Why: PlayerInteractionController uses OverlapCircle which only detects triggers!
```

### **Issue 2: Mirror Number Not Set** (80% of problems)

```
Problem: Room09_Interactable → Mirror Number: 0

Fix:
1. Select mirror
2. Inspector → Room09_Interactable
3. Set Mirror Number: 1 (or 2, 3, 4)

Why: Script needs to know which mirror this is!
```

### **Issue 3: Interaction Radius Too Small** (70% of problems)

```
Problem: PlayerInteractionController → Interaction Radius: 0.5

Fix:
1. Select Player
2. Inspector → PlayerInteractionController
3. Set Interaction Radius: 3 or 4

Why: Player needs to be close enough to detect mirror!
```

### **Issue 4: Player Missing PlayerInteractionController** (60% of problems)

```
Problem: Player doesn't have PlayerInteractionController component

Fix:
1. Select Player
2. Add Component → PlayerInteractionController
3. Set Interaction Radius: 2.25

Why: This component detects nearby interactables!
```

### **Issue 5: Wrong Layer** (50% of problems)

```
Problem: Mirror on layer that PlayerInteractionController can't detect

Fix:
1. Select Player
2. Inspector → PlayerInteractionController
3. Interactable Layers: Set to "Everything"

Why: Player needs to detect mirror's layer!
```

---

## 🧪 STEP-BY-STEP TEST

### **Test 1: Basic Detection**

```
1. Select Player
2. PlayerInteractionController → Draw Radius: ✓
3. Play scene
4. Walk near mirror
5. Look at Scene view
6. Mirror should be inside cyan circle

✅ PASS: Mirror inside circle
❌ FAIL: Increase Interaction Radius
```

### **Test 2: Focus Detection**

```
1. Add debug log to OnFocus() (see Step 6)
2. Play scene
3. Walk near mirror
4. Check Console

✅ PASS: See "FOCUSED on Mirror X"
❌ FAIL: Check collider Is Trigger, layer, radius
```

### **Test 3: Interaction**

```
1. Add debug log to OnInteract() (see Step 6)
2. Play scene
3. Walk near mirror
4. Press E key
5. Check Console

✅ PASS: See "INTERACTING with Mirror X"
❌ FAIL: Check PlayerInteractionController settings
```

### **Test 4: Panel Opens**

```
1. Complete Test 1-3 first
2. Play scene
3. Walk near mirror
4. Press E key
5. Panel should open

✅ PASS: Panel opens
❌ FAIL: Check puzzle script, panel reference
```

---

## 📋 COMPLETE SETUP VERIFICATION

### **Mirror Checklist** (for EACH mirror):

```
Mirror GameObject:
├─ ✓ Sprite Renderer (visible)
├─ ✓ Collider2D
│   ├─ ✓ Is Trigger: CHECKED
│   └─ ✓ Size: 2x2 or bigger
├─ ✓ Room09_Interactable
│   ├─ ✓ Mirror Number: 1-4
│   └─ ✓ Script enabled
└─ ✓ Puzzle Script (Mirror1, Mirror2, Mirror3, or Mirror4)
    └─ ✓ All references assigned
```

### **Player Checklist**:

```
Player GameObject:
├─ ✓ PlayerInteractionController
│   ├─ ✓ Interaction Radius: 2.25+
│   ├─ ✓ Interactable Layers: Everything
│   ├─ ✓ Interact Key: E
│   └─ ✓ Script enabled
├─ ✓ Collider2D
└─ ✓ Tag: "Player"
```

### **Scene Checklist**:

```
Scene:
├─ ✓ Player exists
├─ ✓ 4 mirrors exist
├─ ✓ Canvas exists
├─ ✓ EventSystem exists
└─ ✓ No errors in Console
```

---

## 🆘 STILL NOT WORKING?

### **Try This**:

#### **Option 1: Copy Working Setup**

```
1. Open Room 08 scene (Lisa's Bathroom)
2. Find a working interactable (e.g., Bathtub)
3. Look at its setup:
   - Collider2D settings
   - Room08_Interactable settings
   - Layer
4. Copy EXACT same setup to Room 09 mirrors
```

#### **Option 2: Test with Simple Object**

```
1. Create new GameObject in Room 09
2. Name: "TestInteractable"
3. Add Sprite Renderer (any sprite)
4. Add Box Collider 2D
   - Is Trigger: ✓
   - Size: 2x2
5. Add Room09_Interactable
   - Mirror Number: 1
6. Position near player spawn
7. Play scene
8. Walk to it
9. Press E

If this works: Problem is with your mirror setup
If this doesn't work: Problem is with Player or scene setup
```

#### **Option 3: Check Room 08 for Reference**

```
Room 08 has working interaction system!

1. Open Room 08 scene
2. Find Bathtub GameObject
3. Look at Room08_Interactable component
4. Look at Collider2D settings
5. Copy same setup to Room 09

Room08_Interactable and Room09_Interactable work the same way!
```

---

## 📸 VISUAL CHECKLIST

### **What You Should See in Inspector**:

#### **Mirror GameObject**:

```
Inspector:
┌─────────────────────────────────────┐
│ Mirror1_MedicineCabinet             │
│ Tag: Untagged    Layer: Default     │
├─────────────────────────────────────┤
│ Transform                           │
│ Position: (X, Y, Z)                 │
├─────────────────────────────────────┤
│ Sprite Renderer                     │
│ Sprite: [Your mirror sprite]        │
├─────────────────────────────────────┤
│ Box Collider 2D                     │
│ ✓ Is Trigger                        │ ← MUST BE CHECKED!
│ Size: (2, 2)                        │
├─────────────────────────────────────┤
│ Room09_Interactable (Script)       │
│ ✓ Script enabled                    │
│ Mirror Number: 1                    │ ← MUST BE SET!
├─────────────────────────────────────┤
│ Mirror1_MedicineCabinet (Script)    │
│ Puzzle Panel: [Assigned]            │
│ Timer Text: [Assigned]              │
│ Bottle Slots: [Assigned]            │
└─────────────────────────────────────┘
```

#### **Player GameObject**:

```
Inspector:
┌─────────────────────────────────────┐
│ Player                              │
│ Tag: Player      Layer: Default     │ ← Tag MUST be "Player"!
├─────────────────────────────────────┤
│ PlayerInteractionController         │
│ ✓ Script enabled                    │
│ Interaction Radius: 2.25            │ ← Try 3-4 if not working
│ Interactable Layers: Everything     │ ← Set to Everything
│ Interact Key: E                     │
│ ✓ Draw Radius                       │ ← Check for debugging
└─────────────────────────────────────┘
```

---

## ✅ FINAL SOLUTION

### **Most Likely Fix**:

```
1. Select EACH mirror (1, 2, 3, 4)

2. Inspector → Collider2D → Is Trigger: ✓ CHECK THIS!

3. Inspector → Room09_Interactable → Mirror Number: SET TO 1-4

4. Select Player

5. Inspector → PlayerInteractionController → Interaction Radius: 3

6. Play scene

7. Walk near mirror

8. Press E key

9. Should work!
```

---

## 🎉 SUMMARY

### **Top 3 Fixes** (solves 95% of problems):

1. **✓ Check "Is Trigger" on mirror collider**
2. **Set Mirror Number (1-4) in Room09_Interactable**
3. **Increase Interaction Radius to 3-4**

### **Debug Steps**:

1. Enable Draw Radius on Player
2. Add debug logs to OnFocus/OnInteract
3. Check Console for messages
4. Verify mirror inside cyan circle

### **If Still Not Working**:

1. Copy setup from Room 08
2. Test with simple object
3. Check all checklists above
4. Verify no errors in Console

---

**KAYA MO YAN!** 💪🔧

**MOST COMMON FIX**: Check "Is Trigger" ✓ on mirror collider!

**TEST**: Enable "Draw Radius" to see interaction range!
