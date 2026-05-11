# 🎮 ROOM 09 - COMPLETE UNITY SETUP GUIDE (TAGALOG)

## 📋 BASED ON YOUR ACTUAL ASSETS

Nakita ko na ang assets mo! Ito ang complete step-by-step guide para sa Room 09 setup.

---

## 🎨 YOUR ASSETS (From Screenshot)

### **Bathtubs** (3 sprites):
1. Top bathtub - Empty/clean
2. Middle bathtub - With dirty water
3. Bottom bathtub - With blood/dark water

### **Torn Note Pieces** (4 pieces):
1. "Tonight I"
2. "end this child's"
3. "suffering and"
4. "mine forever"

### **Medicine Cabinet Sprites**:
- Cabinet closed
- Cabinet open with bottles visible
- Individual bottle sprites (6 bottles)

### **Other Items**:
- Rope (coiled)
- Pills/medicine bottles
- Knife
- Bloody towel/cloth
- Diary pages
- Picture frames (empty and with paper)
- Various furniture pieces

---

## 🏗️ PART 1: SCENE SETUP

### **STEP 1: Create Room09 Scene**

```
1. File → New Scene
2. Save as: "Room09_MasterBathroomFinal"
3. Location: Assets/Scenes/
```

### **STEP 2: Add Background**

```
1. Create GameObject: "Background"
2. Add Component: Sprite Renderer
3. Assign: Your bathroom background sprite
4. Position: (0, 0, 0)
5. Order in Layer: -10
```

### **STEP 3: Add Lighting**

```
1. Create GameObject: "Directional Light"
2. Light Type: Directional
3. Intensity: 0.5-0.7 (dim, creepy)
4. Color: Slight blue tint (cold atmosphere)
```

---

## 🎮 PART 2: GAME OBJECTS SETUP

### **STEP 4: Create Room Controller**

```
1. Create Empty GameObject: "Room09_Controller"
2. Position: (0, 0, 0)
3. Add Component: Room09_FlowController
4. Keep Inspector open (we'll assign references later)
```

### **STEP 5: Create Emily Manifestation**

```
1. Create GameObject: "Emily_Manifestation"
2. Add Component: Sprite Renderer
3. Assign: Emily full power sprite (solid, terrifying)
4. Position: Center of bathroom (visible to player)
5. Order in Layer: 5
6. Scale: Make her prominent (1.5, 1.5, 1) or bigger
7. Add Component: Animator (optional, for idle animation)
```

### **STEP 6: Create Player Spawn Point**

```
1. Create Empty GameObject: "PlayerSpawnPoint"
2. Position: Near broken mirror entrance
3. Tag: "Respawn" (or your spawn tag)
4. This is where Lisa enters from Room 08
```

---

## 🪞 PART 3: MIRROR PUZZLES SETUP

### **MIRROR 1: MEDICINE CABINET**

#### **Step 6.1: Create Medicine Cabinet GameObject**

```
1. Create GameObject: "Mirror1_MedicineC abinet"
2. Add Component: Sprite Renderer
3. Assign: Medicine cabinet closed sprite
4. Position: On bathroom wall (upper area)
5. Order in Layer: 1
6. Add Component: Box Collider 2D
   - Is Trigger: ✓ (checked)
   - Size: Cover the cabinet sprite
7. Add Component: Mirror1_MedicineCabinet (script)
```

#### **Step 6.2: Create Medicine Cabinet Panel**

```
1. In Canvas (or create Canvas if wala pa):
   - Right-click Canvas → UI → Panel
   - Rename: "MedicineC abinet_Panel"
   - Anchor: Stretch (full screen)
   - Color: Semi-transparent black (0, 0, 0, 200)
   - Active: ✗ (unchecked - start inactive)

2. Inside MedicineC abinet_Panel:
   
   A. Create Title Text:
      - Right-click Panel → UI → Text (TextMeshPro)
      - Name: "Title_Text"
      - Text: "Medicine Cabinet"
      - Font Size: 48
      - Alignment: Center, Top
      - Position: Top center
   
   B. Create Timer Text:
      - Right-click Panel → UI → Text (TextMeshPro)
      - Name: "Timer_Text"
      - Text: "1:00"
      - Font Size: 36
      - Color: White (will change to red when low)
      - Position: Top right corner
   
   C. Create 6 Empty Slots (Horizontal Layout):
      - Right-click Panel → UI → Panel
      - Name: "Slots_Container"
      - Add Component: Horizontal Layout Group
        - Spacing: 20
        - Child Alignment: Middle Center
      - Position: Center of screen
      
      - Inside Slots_Container, create 6 slots:
        - Right-click → UI → Image
        - Names: "Slot_1" to "Slot_6"
        - Size: (100, 150) each
        - Color: Dark gray (empty slot indicator)
        - Add Component: Box Collider 2D (for drop detection)
   
   D. Create 6 Draggable Bottles:
      - Right-click Panel → UI → Image
      - Names: "Bottle_1973" to "Bottle_1976_B"
      - Assign: Your bottle sprites
      - Size: (80, 120) each
      - Position: Scattered around panel (random positions)
      - Add Component: Draggable Item Script (create simple drag script)
      - Add Text child for year label:
        - "1973", "1974", "1975", "1975", "1976", "1976"
   
   E. Create Close Button:
      - Right-click Panel → UI → Button
      - Name: "Close_Button"
      - Text: "X"
      - Position: Top right corner
      - OnClick: Close panel
```

---

### **MIRROR 2: BATHTUB DRAIN**

#### **Step 7.1: Create Bathtub GameObject**

```
1. Create GameObject: "Mirror2_BathtubDrain"
2. Add Component: Sprite Renderer
3. Assign: Bathtub sprite (empty - top one from your assets)
4. Position: Floor level, prominent position
5. Order in Layer: 1
6. Add Component: Box Collider 2D
   - Is Trigger: ✓
   - Size: Cover bathtub
7. Add Component: Mirror2_BathtubDrain (script - to be created)
```

#### **Step 7.2: Create Bathtub Drain Panel**

```
1. In Canvas:
   - Right-click Canvas → UI → Panel
   - Rename: "BathtubDrain_Panel"
   - Anchor: Stretch (full screen)
   - Color: Semi-transparent black (0, 0, 0, 200)
   - Active: ✗ (unchecked)

2. Inside BathtubDrain_Panel:
   
   A. Create Title & Timer (same as Mirror 1)
      - Title: "Bathtub"
      - Timer: "1:00"
   
   B. Create Bathtub Image:
      - Right-click Panel → UI → Image
      - Name: "Bathtub_Image"
      - Assign: Bathtub with drain sprite
      - Size: Large (400, 300)
      - Position: Upper center
   
   C. Create Drain Cover Button:
      - Right-click Bathtub_Image → UI → Button
      - Name: "DrainCover_Button"
      - Position: Over the drain area
      - Size: (80, 80)
      - Image: Drain cover sprite
      - OnClick: Remove cover, show note pieces
   
   D. Create Assembly Area:
      - Right-click Panel → UI → Panel
      - Name: "Assembly_Area"
      - Position: Lower center
      - Size: (600, 200)
      - Add Component: Vertical Layout Group
      - Create 4 slots for note pieces:
        - "Slot_1" to "Slot_4"
        - Size: (500, 40) each
        - Vertical arrangement
   
   E. Create 4 Torn Note Pieces:
      - Right-click Panel → UI → Image
      - Names: "Note_Piece_1" to "Note_Piece_4"
      - Assign: Your torn note sprites
      - Size: (450, 35) each
      - Position: Scattered (initially hidden)
      - Add Text overlay:
        - Piece 1: "Tonight I"
        - Piece 2: "end this child's"
        - Piece 3: "suffering and"
        - Piece 4: "mine forever"
      - Make draggable
   
   F. Create Close Button
```

---

### **MIRROR 3: VANITY TERROR**

#### **Step 8.1: Create Vanity Mirror GameObject**

```
1. Create GameObject: "Mirror3_VanityTerror"
2. Add Component: Sprite Renderer
3. Assign: Vanity/mirror sprite
4. Position: On wall, medium height
5. Order in Layer: 1
6. Add Component: Box Collider 2D
   - Is Trigger: ✓
7. Add Component: Mirror3_VanityTerror (script - to be created)
```

#### **Step 8.2: Create Vanity Terror Panel**

```
1. In Canvas:
   - Right-click Canvas → UI → Panel
   - Rename: "VanityTerror_Panel"
   - Anchor: Stretch
   - Color: Semi-transparent black
   - Active: ✗

2. Inside VanityTerror_Panel:
   
   A. Create Title & Timer:
      - Title: "Mother's Diary"
      - Timer: "1:30" (90 seconds - longer time)
   
   B. Create 8 Numbered Slots:
      - Right-click Panel → UI → Panel
      - Name: "Slots_Container"
      - Add Component: Grid Layout Group
        - Cell Size: (250, 150)
        - Spacing: (10, 10)
        - Constraint: Fixed Column Count = 4
      - Position: Center
      
      - Create 8 slots:
        - Names: "Slot_1" to "Slot_8"
        - Each has number label: "1", "2", "3"... "8"
        - Size: (250, 150)
        - Color: Dark gray
   
   C. Create 8 Diary Pages:
      - Right-click Panel → UI → Image
      - Names: "DiaryPage_1" to "DiaryPage_8"
      - Assign: Your diary page sprites (or use frame + text)
      - Size: (230, 130)
      - Position: Scattered randomly
      - Add Text component with diary content:
        
        Page 1: "Child defied me at dinner. Refused to sit properly..."
        Page 2: "The defiance continues. Found the child talking..."
        Page 3: "I've increased discipline sessions..."
        Page 4: "Now strange things are happening..."
        Page 5: "The supernatural events have escalated..."
        Page 6: "The presence grows bolder..."
        Page 7: "I've made my preparations..."
        Page 8: "Everything is ready. Tomorrow night I end this..."
      
      - Make draggable
      - Font Size: 12-14 (small, readable)
   
   D. Create Close Button
```

---

### **MIRROR 4: EVIDENCE SEQUENCE**

#### **Step 9.1: Create Large Mirror GameObject**

```
1. Create GameObject: "Mirror4_EvidenceSequence"
2. Add Component: Sprite Renderer
3. Assign: Large mirror/frame sprite
4. Position: Prominent wall position
5. Order in Layer: 1
6. Add Component: Box Collider 2D
   - Is Trigger: ✓
7. Add Component: Mirror4_EvidenceSequence (script - to be created)
```

#### **Step 9.2: Create Evidence Sequence Panel**

```
1. In Canvas:
   - Right-click Canvas → UI → Panel
   - Rename: "EvidenceSequence_Panel"
   - Anchor: Stretch
   - Color: Semi-transparent black
   - Active: ✗

2. Inside EvidenceSequence_Panel:
   
   A. Create Title & Timer:
      - Title: "The Plan"
      - Timer: "1:00"
   
   B. Create Large Mirror Image:
      - Right-click Panel → UI → Image
      - Name: "Mirror_Image"
      - Assign: Large mirror sprite
      - Size: (400, 500)
      - Position: Upper center
   
   C. Create 4 Picture Frames (Below Mirror):
      - Right-click Panel → UI → Panel
      - Name: "Frames_Container"
      - Add Component: Horizontal Layout Group
        - Spacing: 15
      - Position: Below mirror
      
      - Create 4 frames:
        - Names: "Frame_1" to "Frame_4"
        - Assign: Your empty frame sprites
        - Size: (120, 120) each
        - Add number labels: "1", "2", "3", "4"
   
   D. Create 4 Evidence Items:
      - Right-click Panel → UI → Image
      - Names: "Evidence_Rope", "Evidence_Pills", "Evidence_Knife", "Evidence_Towel"
      - Assign sprites:
        - Rope: Your rope sprite
        - Pills: Your pills/medicine sprite
        - Knife: Your knife sprite
        - Towel: Your bloody towel sprite
      - Size: (100, 100) each
      - Position: Scattered around panel
      - Make draggable
   
   E. Create Flashback Image Display:
      - Right-click Panel → UI → Image
      - Name: "Flashback_Image"
      - Size: (300, 300)
      - Position: Center of mirror
      - Active: ✗ (shows when item placed correctly)
      - Will display flashback images
   
   F. Create Close Button
```

---

## 🎭 PART 4: EMILY JUMPSCARE PANEL

### **Step 10: Create Jumpscare Panel**

```
1. In Canvas:
   - Right-click Canvas → UI → Panel
   - Rename: "Emily_Jumpscare_Panel"
   - Anchor: Stretch (full screen)
   - Color: Black (0, 0, 0, 255)
   - Active: ✗

2. Inside Jumpscare Panel:
   
   A. Create Emily Face Image:
      - Right-click Panel → UI → Image
      - Name: "Emily_Face"
      - Assign: Emily screaming/terrifying face sprite
      - Anchor: Stretch (full screen)
      - Size: Fill entire screen
   
   B. Create Scream Text:
      - Right-click Panel → UI → Text (TextMeshPro)
      - Name: "Scream_Text"
      - Text: "FAILED"
      - Font Size: 72
      - Color: Red
      - Alignment: Center
      - Position: Center
      - Add Outline effect (black, thick)
   
   C. Add Audio Source:
      - Select Panel
      - Add Component: Audio Source
      - Assign: Emily scream sound
      - Play On Awake: ✗
      - Loop: ✗
```

---

## 🔊 PART 5: AUDIO SETUP

### **Step 11: Create Audio Sources**

```
1. Create Empty GameObject: "Audio_Manager"
2. Position: (0, 0, 0)

3. Add 3 Audio Sources:
   
   A. Ambient Audio:
      - Add Component: Audio Source
      - Name: "Ambient_Audio"
      - Clip: Tense music / water dripping
      - Loop: ✓
      - Volume: 0.5
      - Play On Awake: ✗ (controller will start it)
   
   B. SFX Audio:
      - Add Component: Audio Source
      - Name: "SFX_Audio"
      - Loop: ✗
      - Volume: 0.7
      - For: Puzzle sounds, door slam, etc.
   
   C. Emily Audio:
      - Add Component: Audio Source
      - Name: "Emily_Audio"
      - Loop: ✗
      - Volume: 0.8
      - For: Emily scream, whisper, breathing
```

---

## 🔗 PART 6: ASSIGN REFERENCES

### **Step 12: Assign References in Room09_FlowController**

```
1. Select "Room09_Controller" GameObject
2. In Inspector, find Room09_FlowController component
3. Assign references:

   Story Milestones:
   - Is Intro Done: ✗ (unchecked)
   - Is Door Locked: ✓ (checked)
   
   Mirror Puzzle Progress:
   - All ✗ (unchecked - will be set by scripts)
   
   Emily State:
   - Emily Manifestation: Drag "Emily_Manifestation" GameObject
   - Emily Has Collapsed: ✗
   
   Ending Trigger:
   - Can Trigger Ending: ✗
   
   Scene Transition:
   - Main Menu Scene Name: "MainMenu"
   
   Audio:
   - Ambient Audio: Drag "Ambient_Audio" component
   - Tense Music Clip: Assign your tense music clip
   - Emily Scream Clip: Assign scream sound clip
```

### **Step 13: Assign References in Mirror Scripts**

#### **Mirror1_MedicineCabinet**:
```
Select "Mirror1_MedicineC abinet" GameObject:
- Panel: Drag "MedicineC abinet_Panel"
- Timer Text: Drag "Timer_Text"
- Slots: Drag all 6 slot GameObjects
- Bottles: Drag all 6 bottle GameObjects
- Time Limit: 60
```

#### **Mirror2_BathtubDrain** (when script created):
```
Select "Mirror2_BathtubDrain" GameObject:
- Panel: Drag "BathtubDrain_Panel"
- Timer Text: Drag "Timer_Text"
- Drain Cover Button: Drag "DrainCover_Button"
- Assembly Slots: Drag 4 slots
- Note Pieces: Drag 4 note pieces
- Time Limit: 60
```

#### **Mirror3_VanityTerror** (when script created):
```
Select "Mirror3_VanityTerror" GameObject:
- Panel: Drag "VanityTerror_Panel"
- Timer Text: Drag "Timer_Text"
- Slots: Drag all 8 slots
- Diary Pages: Drag all 8 pages
- Time Limit: 90
```

#### **Mirror4_EvidenceSequence** (when script created):
```
Select "Mirror4_EvidenceSequence" GameObject:
- Panel: Drag "EvidenceSequence_Panel"
- Timer Text: Drag "Timer_Text"
- Frames: Drag 4 frames
- Evidence Items: Drag 4 evidence items
- Flashback Image: Drag "Flashback_Image"
- Flashback Sprites: Assign 4 flashback images
- Time Limit: 60
```

---

## 🎨 PART 7: VISUAL POLISH

### **Step 14: Add Visual Effects**

```
1. Emily Glow Effect:
   - Select "Emily_Manifestation"
   - Add Component: Sprite Renderer
   - Material: Create glowing material (optional)
   - Or add Particle System for aura effect

2. Mirror Glow (When Complete):
   - For each mirror GameObject
   - Add child GameObject: "Glow_Effect"
   - Add Component: Sprite Renderer
   - Assign: Glow sprite (white circle)
   - Color: Green (success)
   - Active: ✗ (activate when puzzle complete)

3. Water Effects (Optional):
   - Add Particle System for water drips
   - Position near bathtub
   - Slow drip rate

4. Blood Effects (Optional):
   - Add blood sprites on floor
   - Near broken mirror entrance
   - Order in Layer: 0
```

---

## 🧪 PART 8: TESTING SETUP

### **Step 15: Create Test Buttons (Temporary)**

```
1. In Canvas, create Panel: "Debug_Panel"
2. Position: Bottom right corner
3. Add buttons for testing:
   
   - Button: "Complete Mirror 1"
     OnClick: Room09_FlowController.mirror1Complete = true
   
   - Button: "Complete Mirror 2"
     OnClick: Room09_FlowController.mirror2Complete = true
   
   - Button: "Complete Mirror 3"
     OnClick: Room09_FlowController.mirror3Complete = true
   
   - Button: "Complete Mirror 4"
     OnClick: Room09_FlowController.mirror4Complete = true
   
   - Button: "Trigger Ending"
     OnClick: Test ending cutscene
   
4. Active: ✓ (for testing only)
5. DELETE THIS PANEL before release!
```

---

## ✅ PART 9: FINAL CHECKLIST

### **Scene Objects**:
- [ ] Background sprite assigned
- [ ] Lighting setup
- [ ] Room09_Controller created
- [ ] Emily_Manifestation created and positioned
- [ ] Player spawn point created
- [ ] All 4 mirror GameObjects created
- [ ] All colliders setup (Is Trigger checked)

### **UI Panels**:
- [ ] MedicineC abinet_Panel complete (6 slots, 6 bottles)
- [ ] BathtubDrain_Panel complete (4 slots, 4 notes)
- [ ] VanityTerror_Panel complete (8 slots, 8 pages)
- [ ] EvidenceSequence_Panel complete (4 frames, 4 items)
- [ ] Emily_Jumpscare_Panel complete
- [ ] All panels start INACTIVE

### **Scripts**:
- [ ] Room09_FlowController assigned
- [ ] Mirror1_MedicineCabinet assigned
- [ ] Mirror2_BathtubDrain assigned (when created)
- [ ] Mirror3_VanityTerror assigned (when created)
- [ ] Mirror4_EvidenceSequence assigned (when created)

### **References**:
- [ ] Room09_FlowController references assigned
- [ ] All mirror script references assigned
- [ ] Audio sources assigned
- [ ] Panel references assigned

### **Audio**:
- [ ] Ambient audio source created
- [ ] SFX audio source created
- [ ] Emily audio source created
- [ ] Audio clips assigned

### **Testing**:
- [ ] Can enter scene
- [ ] Emily appears
- [ ] Intro dialogue plays
- [ ] Can interact with mirrors
- [ ] Panels open correctly
- [ ] Timers work
- [ ] Drag and drop works
- [ ] Success triggers next phase
- [ ] Failure triggers jumpscare
- [ ] Ending cutscene plays
- [ ] Returns to main menu

---

## 🎯 NEXT STEPS

### **1. Create Remaining Scripts**:
```
Tell me: "Create Mirror2_BathtubDrain script"
Tell me: "Create Mirror3_VanityTerror script"
Tell me: "Create Mirror4_EvidenceSequence script"
Tell me: "Create Room09_Interactable script"
```

### **2. Create Drag and Drop System**:
```
Tell me: "Create drag and drop system for puzzles"
```

### **3. Test Each Puzzle**:
```
1. Test Mirror 1 (medicine cabinet)
2. Test Mirror 2 (bathtub drain)
3. Test Mirror 3 (vanity terror)
4. Test Mirror 4 (evidence sequence)
5. Test ending cutscene
```

### **4. Polish and Balance**:
```
1. Adjust timer durations
2. Fine-tune difficulty
3. Add sound effects
4. Add visual feedback
5. Test complete flow
```

---

## 💡 IMPORTANT NOTES

### **About Your Assets**:
- ✅ You have 3 bathtub sprites (perfect for progression)
- ✅ You have 4 torn note pieces (perfect for Mirror 2)
- ✅ You have medicine cabinet sprites
- ✅ You have rope, pills, knife, towel (perfect for Mirror 4)
- ✅ You have frame sprites (perfect for evidence display)

### **About Draggable Items**:
- All puzzle items need drag and drop functionality
- I can create a simple DraggableItem script
- Or you can use Unity's Event System

### **About Timers**:
- Timers count down from time limit
- Change color when < 10 seconds (red warning)
- When reaches 0 → Emily jumpscare → Game Over

### **About Success**:
- When puzzle solved correctly → Panel closes
- Mirror glows green
- Room09_FlowController.OnMirrorComplete(number) called
- If all 4 complete → Ending cutscene triggers

---

## 🎉 SUMMARY

### **What This Guide Covers**:
- ✅ Complete scene setup
- ✅ All 4 mirror puzzle panels
- ✅ Emily jumpscare panel
- ✅ Audio setup
- ✅ Reference assignments
- ✅ Testing setup
- ✅ Based on YOUR actual assets

### **What You Need Next**:
- ⏳ 3 more mirror puzzle scripts
- ⏳ Drag and drop system
- ⏳ Testing and polish

### **Result**:
**Complete Room 09 with all puzzles and ending cutscene!** 🎮✨

---

## 💬 QUICK COMMANDS

```
"Create Mirror2_BathtubDrain script"
"Create Mirror3_VanityTerror script"
"Create Mirror4_EvidenceSequence script"
"Create drag and drop system"
"Explain [specific puzzle] in detail"
```

---

**COMPLETE GUIDE BASED ON YOUR ASSETS!** 🎉

Follow this step-by-step and your Room 09 will be complete! 

**KAYA MO YAN!** 💪✨🚀
