# 🔧 ROOM 09 - INTERACTION FIX GUIDE

## ❌ PROBLEM: Hindi ma-interact ang mirrors

Kung hindi mo ma-click o ma-interact ang mirrors para lumabas ang panels, sundin ang guide na ito.

---

## ✅ SOLUTION: Use Room09_Interactable Script

Gumawa ako ng bagong script: **`Room09_Interactable.cs`**

Ito ang gagawin:
- Detect kung malapit ang player sa mirror
- Show "Press E" prompt
- Trigger puzzle panel pag nag-press ng E

---

## 🔧 SETUP STEPS

### **STEP 1: Add Room09_Interactable to Each Mirror**

Para sa **bawat mirror GameObject** (4 mirrors total):

```
1. Select mirror GameObject (e.g., "Mirror1_MedicineC abinet")

2. Add Component → Room09_Interactable

3. In Inspector, set:
   - Mirror Number: 1 (or 2, 3, 4 depending on mirror)
   - Interaction Radius: 2 (adjust if needed)
   - Interaction Key: E (or your preferred key)
   - Interaction Prompt: (optional - drag UI prompt GameObject)
```

### **STEP 2: Ensure Collider is Setup**

Para ma-detect ang player distance:

```
1. Select mirror GameObject

2. Check if may Box Collider 2D:
   - If wala: Add Component → Box Collider 2D
   
3. Collider settings:
   - Is Trigger: ✓ (CHECKED - very important!)
   - Size: Cover the mirror sprite
   
4. Make sure collider is visible in Scene view (green outline)
```

### **STEP 3: Check Player Tag**

Para ma-detect ng script ang player:

```
1. Select your Player GameObject

2. In Inspector, check Tag dropdown (top)

3. Make sure it's set to: "Player"

4. If "Player" tag doesn't exist:
   - Click Tag dropdown
   - Add Tag...
   - Create new tag: "Player"
   - Go back to Player GameObject
   - Set Tag to "Player"
```

### **STEP 4: Test Interaction**

```
1. Play the scene

2. Walk player near mirror

3. When close enough (within 2 units):
   - Yellow circle should be visible in Scene view
   - Interaction prompt should appear (if you set one)
   
4. Press E key:
   - Puzzle panel should open
   - Player movement should stop
   - Timer should start
```

---

## 🎨 OPTIONAL: Create "Press E" Prompt

Kung gusto mo ng visual prompt:

### **Create UI Prompt**:

```
1. In Canvas, create:
   - Right-click Canvas → UI → Panel
   - Name: "InteractionPrompt_Mirror1"
   
2. Inside panel:
   - Right-click → UI → Text (TextMeshPro)
   - Text: "Press E to examine"
   - Font Size: 24
   - Alignment: Center
   
3. Position:
   - Above mirror sprite
   - Or bottom center of screen
   
4. Set Active: ✗ (unchecked - starts hidden)

5. Assign to Room09_Interactable:
   - Select mirror GameObject
   - Find Room09_Interactable component
   - Drag prompt panel to "Interaction Prompt" field
```

### **Repeat for All 4 Mirrors**:
- InteractionPrompt_Mirror1
- InteractionPrompt_Mirror2
- InteractionPrompt_Mirror3
- InteractionPrompt_Mirror4

---

## 🐛 TROUBLESHOOTING

### **Problem 1: Still can't interact**

**Check**:
- [ ] Room09_Interactable script is attached to mirror
- [ ] Mirror Number is set correctly (1, 2, 3, or 4)
- [ ] Box Collider 2D exists on mirror
- [ ] Is Trigger is CHECKED
- [ ] Player GameObject has "Player" tag
- [ ] Player is within Interaction Radius (default 2 units)

**Debug**:
```
1. Select mirror in Hierarchy
2. In Scene view, you should see yellow wire sphere (interaction radius)
3. Play scene
4. Move player close to mirror
5. Check Console for any errors
```

---

### **Problem 2: Panel opens but nothing happens**

**Check**:
- [ ] Puzzle panel is assigned in mirror script (Mirror1, Mirror2, etc.)
- [ ] Panel has all required UI elements (slots, items, timer)
- [ ] DraggableItem scripts are attached to puzzle items
- [ ] Timer Text is assigned

**Fix**:
```
1. Select mirror GameObject
2. Find the Mirror script component (Mirror1_MedicineC abinet, etc.)
3. Check all references are assigned:
   - Puzzle Panel
   - Timer Text
   - Slots array
   - Items array
```

---

### **Problem 3: "Press E" doesn't work**

**Check**:
- [ ] Interaction Key is set to E (or your preferred key)
- [ ] Player is within interaction radius
- [ ] No other script is blocking input
- [ ] Game is not paused

**Try**:
```
1. Change Interaction Key to different key (F, Space, etc.)
2. Increase Interaction Radius to 3 or 4
3. Check if Input.GetKeyDown(KeyCode.E) works elsewhere
```

---

### **Problem 4: Collider not detecting player**

**Check**:
- [ ] Both mirror and player have Collider2D
- [ ] Mirror collider Is Trigger is CHECKED
- [ ] Player has Rigidbody2D
- [ ] Collision Matrix allows Player-Default collision

**Fix**:
```
1. Edit → Project Settings → Physics 2D
2. Check Layer Collision Matrix
3. Make sure Player layer collides with Default layer
```

---

## 📋 COMPLETE SETUP CHECKLIST

### **For Each Mirror (1-4)**:

- [ ] Mirror GameObject exists in scene
- [ ] Mirror has sprite assigned
- [ ] Mirror has Box Collider 2D
  - [ ] Is Trigger: ✓ CHECKED
  - [ ] Size covers sprite
- [ ] Mirror has Room09_Interactable script
  - [ ] Mirror Number: Set correctly (1, 2, 3, or 4)
  - [ ] Interaction Radius: 2 (or adjusted)
  - [ ] Interaction Key: E
  - [ ] Interaction Prompt: Assigned (optional)
- [ ] Mirror has puzzle script (Mirror1, Mirror2, Mirror3, or Mirror4)
  - [ ] Puzzle Panel: Assigned
  - [ ] Timer Text: Assigned
  - [ ] Slots: Assigned
  - [ ] Items: Assigned
- [ ] Puzzle panel exists in Canvas
  - [ ] Starts inactive (unchecked)
  - [ ] Has all UI elements
- [ ] Interaction prompt exists (optional)
  - [ ] Starts inactive
  - [ ] Positioned correctly

### **General**:
- [ ] Player GameObject has "Player" tag
- [ ] Player has Rigidbody2D
- [ ] Player has Collider2D
- [ ] Canvas exists in scene
- [ ] All panels are children of Canvas

---

## 🎯 QUICK TEST

### **Test Script**:

Kung gusto mo i-test kung gumagana ang interaction:

```csharp
// Add this to Room09_Interactable.cs Update() method temporarily
void Update()
{
    // ... existing code ...
    
    // DEBUG: Show distance to player
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    if (player != null)
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        Debug.Log($"Mirror {mirrorNumber} - Distance to player: {distance}");
    }
}
```

**Expected Output**:
- Console should show distance updating as player moves
- When distance < 2, interaction should work

---

## ✅ SUMMARY

### **What Changed**:
- ✅ Created `Room09_Interactable.cs` - handles all interaction
- ✅ Updated mirror scripts - added public `StartPuzzle()` method
- ✅ Removed duplicate interaction code from mirror scripts

### **How It Works**:
1. Player walks near mirror (within 2 units)
2. Room09_Interactable detects player
3. Shows "Press E" prompt (if assigned)
4. Player presses E
5. Room09_Interactable calls mirror's `StartPuzzle()`
6. Puzzle panel opens

### **Setup Required**:
1. Add Room09_Interactable to each mirror
2. Set Mirror Number (1-4)
3. Ensure colliders are setup
4. Ensure Player has "Player" tag
5. Test!

---

**INTERACTION SHOULD WORK NOW!** 🎉

Follow the setup steps and test each mirror! 

**KAYA MO YAN!** 💪✨🎮
